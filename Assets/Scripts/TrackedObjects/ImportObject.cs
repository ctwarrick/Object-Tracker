using System;

/// <summary>
/// This class holds the data of one TrackedObject.  It's used because NewtonSoft JSON
/// was not playing well with Unity and Microsoft.Geospatial was also being a pain.  Use this
/// to import data and then stuff it into the appropriate Microsoft.Geospatial data structures.
/// </summary>
public class ImportObject
{
    #region Fields
    // Position Fields
    private double _latitude;
    private double _longitude;
    private double _altitude;
    private float _velocity;
    private string _origin;
    private string _destination;
    private DateTime _estimatedArrival;

    // Condition Fields
    private float _temperature;
    #endregion

    #region Properties
    public double Latitude
    {
        get { return _latitude; }
        set
        {
            if (value > 90.0f || value < -90.0f)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must contain a valid latitude.");
            }
            else
            {
                _latitude = value;
            }
        }
    }
    public double Longitude
    {
        get { return _longitude; }
        set
        {
            if (value > 180.0f || value < -180.0f)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must contain a valid longitude.");
            }
            else
            {
                _longitude = value;
            }
        }
    }
    public double Altitude
    {
        get { return _altitude; }
        set
        {
            if (value > 16000.0f)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must be below 16,000 meters.");
            }
            else
            {
                _altitude = value;
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
}
