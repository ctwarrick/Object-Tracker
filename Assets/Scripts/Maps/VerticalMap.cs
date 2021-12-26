using System.Collections.Generic;
using System.Timers;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;

/// <summary>
/// The vertical map is an overview map which will eventually display the bounds of the more
/// detailed horizontal map as well as its center.  It hangs on walls or other vertical surfaces.
/// </summary>
public class VerticalMap : Map
{
    #region Constants
    const float BoundingBoxThickness = 0.075f;
    const float MapScaleFactor = 1.25f;
    const int RotationCorrection = -90;
    #endregion

    #region Serialized Fields
    [SerializeField]
    protected GameObject boundingBoxModel;
    #endregion

    #region Fields
    MapRendererBase horizontalRenderer;
    MapSpawner mapSpawner;
    #endregion

    #region Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        // Create base map class and set zoom to highest for overview
        base.Start();
        mapRendererBase.ZoomLevel = 1;

        // Get MapSpawner to track state of horizontal map
        mapSpawner = Camera.main.GetComponent<MapSpawner>();

        // Fix bounding box to 0 degrees rotation
        Vector3 rotationCorrectionAngles = new Vector3(RotationCorrection, 0, 0);
        Quaternion rotationCorrection = new Quaternion();
        rotationCorrection.eulerAngles = rotationCorrectionAngles;
        boundingBoxModel.transform.rotation = rotationCorrection;
    }
    protected void Update()
    {
        // If horizontal map newly spawned, get its renderer
        if (mapSpawner.HorizontalMapStatus == MapStatus.Real && horizontalRenderer == null)
        {
            horizontalRenderer = GameObject.FindGameObjectWithTag("RealHorizontal").GetComponent<MapRendererBase>();
        }

        // If horizontal map spawned and bounds exist, get bounds and set yellow box
        // Else deactivate if active.  Set overview zoom to 50% of detail zoom or 1
        if (mapSpawner.HorizontalMapStatus == MapStatus.Real &&
            horizontalRenderer.Bounds.TopRight.LatitudeInDegrees != 0)
        {
            if (boundingBoxModel.activeInHierarchy == false)
            {
                boundingBoxModel.SetActive(true);
            }

            if (horizontalRenderer.ZoomLevel / MapScaleFactor > 1.0f)
            {
                mapRendererBase.ZoomLevel = (horizontalRenderer.ZoomLevel / MapScaleFactor);
            }
            else
            {
                mapRendererBase.ZoomLevel = 1.0f;
            }

            // If zoom not 1, align centers
            if (horizontalRenderer != null && mapRendererBase.ZoomLevel > 1)
            {
                mapRendererBase.Center = horizontalRenderer.Center;
            }

            SetBoxSize(horizontalRenderer.Bounds);
        } 
        else if (boundingBoxModel.activeInHierarchy == true)
        {
            boundingBoxModel.SetActive(false);
        }
    }
    public override void MoveMap()
    {
        // We'll write this later
        throw new System.NotImplementedException();
    }

    protected void SetBoxSize(GeoBoundingBox box)
    {
        // Get top right and bottom left of horizontal map
        LatLonAlt topRightLla = new LatLonAlt(box.TopRight, 0.0f);
        Vector3 topRight = MapRendererTransformExtensions.TransformLatLonAltToLocalPoint(GetComponent<MapRenderer>(),
                                                                                         topRightLla);
        LatLonAlt bottomLeftLla = new LatLonAlt(box.BottomLeft, 0.0f);
        Vector3 bottomLeft = MapRendererTransformExtensions.TransformLatLonAltToLocalPoint(GetComponent<MapRenderer>(),
                                                                                           bottomLeftLla);
        LatLonAlt centerLla = new LatLonAlt(box.Center, 0.0f);
        Vector3 center = MapRendererTransformExtensions.TransformLatLonAltToLocalPoint(GetComponent<MapRenderer>(),
                                                                                       centerLla);
        // Set bounding box transform to match
        float deltaLongitude = topRight.x - bottomLeft.x;
        float deltaLatitude = topRight.y - bottomLeft.y;
        float deltaThickness = topRight.z - bottomLeft.z;
        Vector3 newScale = new Vector3(deltaLongitude,
                                       BoundingBoxThickness,
                                       deltaLongitude);
        boundingBoxModel.transform.localScale = newScale;
        boundingBoxModel.transform.localPosition = center;
    }
    #endregion
}
