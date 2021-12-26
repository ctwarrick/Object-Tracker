using System;
using System.Text;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.Maps.Unity;
using Microsoft.Maps.Unity.Search;
using Microsoft.Geospatial;

/// <summary>
/// A detailed map where you can zoom in and out and see where tracked objects are.
/// </summary>
public class HorizontalMap : Map
{
    // Disabled because Visual Studio squawks at serialized fields
    #pragma warning disable 0649
    #region Serialized Fields
    [SerializeField]
    MapSearchText mapSearchText;
    [SerializeField]
    AddressTooltip addressTooltip;
    #endregion
    #pragma warning restore 0649

    #region Fields
    //difference between min and max zoom
    private float _zoomDifference;
    private MixedRealityKeyboard _mixedRealityKeyboard;
    private MapRenderer _mapRenderer;
    #endregion

    #region Properties
    public double Latitude
    {
        get { return mapRendererBase.Center.LatitudeInDegrees; }
    }

    public double Longitude
    {
        get { return mapRendererBase.Center.LongitudeInDegrees; }
    }

    public float ZoomLevel
    {
        get { return mapRendererBase.ZoomLevel; }
        set
        {
            if (value >= mapRendererBase.MinimumZoomLevel && value <= mapRendererBase.MaximumZoomLevel)
            {
                mapRendererBase.ZoomLevel = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must be between {mapRendererBase.MinimumZoomLevel} and {mapRendererBase.MaximumZoomLevel}.");
            }
        }
    }

    public float MinimumZoom
    {
        get { return mapRendererBase.MinimumZoomLevel; }
    }

    public float MaximumZoom
    {
        get { return mapRendererBase.MaximumZoomLevel; }
    }
    #endregion

    #region Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _zoomDifference = mapRendererBase.MaximumZoomLevel - mapRendererBase.MinimumZoomLevel;

        // Get keyboard and hide it till needed
        _mixedRealityKeyboard = GetComponentInChildren<MixedRealityKeyboard>();
        _mixedRealityKeyboard.HideKeyboard();
        _mapRenderer = GetComponent<MapRenderer>();
    }

    private void Update()
    {
        if (_mixedRealityKeyboard.Visible == true)
        {
            mapSearchText.SetText(_mixedRealityKeyboard.Text);
        }
    }

    public void UpdateZoom(SliderEventData eventData)
    {
        mapRendererBase.ZoomLevel = (eventData.NewValue * _zoomDifference) + mapRendererBase.MinimumZoomLevel;
    }

    public override void MoveMap()
    {
        // Pull the map spawner so you can stuff current state into it
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        MapSpawner mapSpawner = mainCamera.GetComponent<MapSpawner>();

        // Stuff data in cache and prepare to respawn a mock
        mapSpawner.StoredCenter = mapRendererBase.Center;
        mapSpawner.StoredZoomLevel = mapRendererBase.ZoomLevel;
        mapSpawner.HorizontalMapStatus = MapStatus.Blank;

        Destroy(this.gameObject);
    }
    

    public void SpawnKeyboard()
    {
        _mixedRealityKeyboard.ShowKeyboard("",false);
    }

    public async void CommitKeyboardAsync()
    {
        string query = _mixedRealityKeyboard.Text;
        _mixedRealityKeyboard.HideKeyboard();
        MapLocationFinderResult result = await MapLocationFinder.FindLocations(query);
        mapSearchText.IsResponse = true;

        // if status was bad, spawn error message, else process
        if (result.Status != MapLocationFinderStatus.Success)
        {
            // Generate the error announcement
            DisplayBadStatus(result.Status);
        }
        else
        {
            mapSearchText.SetText("Zooming . . .");
            // For now, just process the first result until I can code an expanding
            // selection menu for the user to pick
            MapLocation zoomLocation = result.Locations[0];
            mapRendererBase.Center = zoomLocation.Point;
        }    
    }

    public async void SpawnToolTip(LatLonAlt latLonAlt)
    {
        LatLon query = latLonAlt.LatLon;

        // Query bing for resulting lat/long
        MapLocationFinderResult result = await MapLocationFinder.FindLocationsAt(query);

        if (result.Status != MapLocationFinderStatus.Success)
        {
            // Generate the error announcement
            mapSearchText.IsResponse = true;
            DisplayBadStatus(result.Status);
        }
        else
        {
            // Instantiate the tooltip
            AddressTooltip tooltip = Instantiate(addressTooltip);

            // Put the tooltip at the position of the hand ray
            Vector3 position = MapRendererTransformExtensions.TransformLatLonAltToWorldPoint
                (_mapRenderer, latLonAlt);
            tooltip.transform.position = position;

            // There should only be one result for a LatLon, so pull it
            MapLocation location = result.Locations[0];

            // If the location is a US location with a street address,
            // format it to conventional US standards
            // If this is used in prod, will need a separate formatter for known other countries
            if (location.Address.CountryRegion == "United States" && 
                location.Address.AddressLine != "")
            {
                // Split string into street, city, state, ZIP, Country with commas
                string[] splitAddress = location.Address.FormattedAddress.Split(',');

                // Format address with newline after street
                var sb = new StringBuilder();
                sb.Append(splitAddress[0]);
                sb.Append(Environment.NewLine);
                sb.Append(splitAddress[1].Trim() + ",");
                sb.Append(splitAddress[2]);
                string address = sb.ToString();
                tooltip.SetAddress(address);
            }
            else
            {
                tooltip.SetAddress(location.Address.FormattedAddress);
            }
        }
    }

    // Handles the standard bad responses from MapLocationFinder by displaying errors above the
    // horizontal map menu
    private void DisplayBadStatus(MapLocationFinderStatus status)
    {
        switch (status)
        {
            case MapLocationFinderStatus.BadResponse:
                mapSearchText.SetText("Azure Error While Processing Response");
                break;
            case MapLocationFinderStatus.Cancel:
                mapSearchText.SetText("Request Cancelled");
                break;
            case MapLocationFinderStatus.InvalidCredentials:
                mapSearchText.SetText("Azure Credentials Invalid");
                break;
            case MapLocationFinderStatus.NetworkFailure:
                mapSearchText.SetText("Network Error; Try Again");
                break;
            case MapLocationFinderStatus.ServerError:
                mapSearchText.SetText("Azure Server Error");
                break;
            case MapLocationFinderStatus.EmptyResponse:
                mapSearchText.SetText("No Such Location Found");
                break;
            default:
                mapSearchText.SetText("Unknown Error");
                break;
        }
    }
    #endregion
}
