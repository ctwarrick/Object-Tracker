using System;
using UnityEngine;
using Microsoft.Geospatial;

/// <summary>
/// A freight item or other thing that you want to track on the map.  It has a teardrop-shaped
/// pointer to show where it is, and other properties you can track.
/// </summary>
public class TrackedObject : MonoBehaviour
{

    [SerializeField]
    public GameObject shipmentStatusPrefab;

    #region Fields
    // Position Fields
    private LatLonAlt _latLonAlt;
    private string _latitudeText;
    private string _longitudeText;
    private string _altitudeText;
    private float _velocity;
    private string _origin;
    private string _destination;
    private DateTime _estimatedArrival;

    // Condition Fields
    private float _temperature;
    #endregion

    #region Properties
    public LatLonAlt LatLonAlt
    {
        get { return _latLonAlt; }
        set
        {
            if (value.LatLon.IsValid == false)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must contain a valid LatLon.");
            }
            else
            {
                _latLonAlt = value;
            }
        }
    }

    public float Velocity
    {
        get { return _velocity; }
        set
        {
            // Used the m/s value of the speed of sound at sea level
            if (value > 383.0f)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must be below 383 meters per second.");
            }
            else
            {
                _velocity = value;
            }
        }
    }
    public string Origin
    {
        get { return _origin; }
        set
        {
            _origin = value;
        }
    }
    public string Destination
    {
        get { return _destination; }
        set
        {
            _destination = value;
        }
    }
    public DateTime EstimatedArrival
    {
        get { return _estimatedArrival; }
        set
        {
            if (value < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} cannot be in the past.");
            }
            else
            {
                _estimatedArrival = value;
            }
        }
    }
    #endregion
    // Start is called before the first frame update
    private void Start()
    {
        // You might need this later to consume position data

    }
    public void SpawnShipmentStatus()
    {
        // First destroy any existing one because you're going to replace it
        GameObject existingDialog = GameObject.FindWithTag("ShipmentDialog");
        if (existingDialog != null)
        {
            GameObject.Destroy(existingDialog);
        }

        // Find the transform of the tracked object and spawn it .3m to the right and up for now
        Quaternion dialogRotation = shipmentStatusPrefab.transform.rotation;
        Vector3 dialogPosition = this.gameObject.transform.position;
        dialogPosition.x += 0.15f;
        dialogPosition.y += 0.15f;
        GameObject shipmentStatus = Instantiate(shipmentStatusPrefab,
                                                dialogPosition,
                                                dialogRotation,
                                                this.transform);
    }
}
