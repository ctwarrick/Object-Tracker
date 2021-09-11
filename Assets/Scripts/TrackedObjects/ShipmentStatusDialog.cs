using Microsoft.Maps.Unity;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Governs the dialog which displays detailed info for a shipment.
/// </summary>
public class ShipmentStatusDialog : MonoBehaviour
{
    #region Fields
    private TrackedObject _trackedObject;
    private MapPin _trackedObjectPin;
    private MapRenderer _mapRenderer;
    private TextMeshPro _locationText;
    private TextMeshPro _elevationText;
    private TextMeshPro _velocityText;
    private TextMeshPro _originText;
    private TextMeshPro _destinationText;
    private TextMeshPro _arrivalText;
    private LocationHandler _locationHandler;
    private MapSceneOfLocationAndZoomLevel _mapScene;
    #endregion

    #region Methods
    private void Start()
    {
        // Pull text objects from children in hierarchy
        _locationText = this.transform.Find("LocationText").GetComponent<TextMeshPro>();
        _elevationText = this.transform.Find("ElevationText").GetComponent<TextMeshPro>();
        _velocityText = this.transform.Find("VelocityText").GetComponent<TextMeshPro>();
        _originText = this.transform.Find("OriginText").GetComponent<TextMeshPro>();
        _destinationText = this.transform.Find("DestinationText").GetComponent<TextMeshPro>();
        _arrivalText = this.transform.Find("ArrivalText").GetComponent<TextMeshPro>();

        // Set up map renderer from horizontal map
        _mapRenderer = this.gameObject.GetComponentInParent<MapRenderer>();

        // Set up location handler from main camera
        _locationHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LocationHandler>();

        // Set parent
        _trackedObject = this.gameObject.GetComponentInParent<TrackedObject>();
        _trackedObjectPin = this.gameObject.GetComponentInParent<MapPin>();
    }
    // Update is called once per frame

    void Update()
    {
        // Assemble object lat/long
        string latitudeText = _locationHandler.WriteLatitude(_trackedObject.LatLonAlt.LatitudeInDegrees);
        string longitudeText = _locationHandler.WriteLongitude(_trackedObject.LatLonAlt.LongitudeInDegrees);
        _locationText.text = latitudeText + " " + longitudeText;

        _elevationText.text = _locationHandler.WriteElevation(_trackedObject.LatLonAlt.AltitudeInMeters);
        _velocityText.text = _trackedObject.Velocity.ToString();
        _originText.text = _trackedObject.Origin;
        _destinationText.text = _trackedObject.Destination;
        _arrivalText.text = _trackedObject.EstimatedArrival.ToString();
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }

    public void ZoomToShipment()
    {
        // Reassignment is a hack to avoid NullReferenceExceptions on device build
        if (_trackedObjectPin == null)
        {
            _trackedObjectPin = this.gameObject.GetComponentInParent<MapPin>();
        }   
        else
        {
            Debug.Log("trackedObjectPin is not null");
        }
        if (_mapRenderer == null)
        {
            _mapRenderer = this.gameObject.GetComponentInParent<MapRenderer>();
        }
        else
        {
            Debug.Log("mapRenderer is not null");
        }
        try
        {
            _mapScene = new MapSceneOfLocationAndZoomLevel(_trackedObjectPin.Location, 14.0f);
        }
        catch (NullReferenceException n)
        {
            Debug.Log("Could not assign _mapScene.");
            Debug.Log(n);
        }

        try
        {
            _mapRenderer.SetMapScene(_mapScene);
        }
        catch (NullReferenceException n)
        {
            Debug.Log("Could not set Zoom Level.");
            Debug.Log(n);
        }
    }
    #endregion
}
