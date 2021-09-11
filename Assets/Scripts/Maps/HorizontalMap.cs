using System;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

/// <summary>
/// A detailed map where you can zoom in and out and see where tracked objects are.
/// </summary>
public class HorizontalMap : Map
{
    #region Fields
    //difference between min and max zoom
    private float _zoomDifference;
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
}
