using System;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.Maps.Unity.Search;

/// <summary>
/// A detailed map where you can zoom in and out and see where tracked objects are.
/// </summary>
public class HorizontalMap : Map
{
    #region Serialized Fields
    [SerializeField]
    MapSearchText mapSearchText;
    #endregion

    #region Fields
    //difference between min and max zoom
    private float _zoomDifference;
    private MixedRealityKeyboard _mixedRealityKeyboard;
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
    #endregion

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
            switch (result.Status)
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
        else
        {
            mapSearchText.SetText("Zooming . . .");
            // For now, just process the first result until I can code an expanding
            // selection menu for the user to pick
            MapLocation zoomLocation = result.Locations[0];
            mapRendererBase.Center = zoomLocation.Point;
        }    
    }
}
