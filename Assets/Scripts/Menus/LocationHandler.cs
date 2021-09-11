using System;
using UnityEngine;

/// <summary>
/// This class takes Microsoft Geospatial values for Latitude, Longitude, and Altitude
/// and turns them into strings which are formatted for display in the HorizontalMapMenu
/// and the ShipmentStatusDialog GameObjects.
/// </summary>
public class LocationHandler : MonoBehaviour
{
    #region Fields
    private string _latitudeText;
    private string _longitudeText;
    private string _elevationText;
    #endregion

    #region Constants
    private const float MetersToFeet = 3.28084f;
    #endregion

    #region Methods
    public string WriteLatitude(double latitude)
    {
        // Round the value off at 3
        decimal decLatitude = new decimal(latitude);
        decLatitude = Math.Round(decLatitude, 3);

        // Make string with N or S latitude as appropriate
        if (decLatitude < 0)
        {
            decLatitude = Math.Abs(decLatitude);
            _latitudeText = "S" + decLatitude.ToString();
        }
        else if (decLatitude > 0)
        {
            _latitudeText = "N" + decLatitude.ToString();
        }
        else
        {
            _latitudeText = decLatitude.ToString();
        }

        return _latitudeText;
    }
    
    public string WriteLongitude(double longitude)
    {
        // Round the value off at 3
        decimal decLongitude = new decimal(longitude);
        decLongitude = Math.Round(decLongitude, 3);

        if (decLongitude < 0)
        {
            decLongitude = Math.Abs(decLongitude);
            _longitudeText = "W" + decLongitude.ToString();
        }
        else if (longitude > 0)
        {
            _longitudeText = "E" + decLongitude.ToString();
        }
        else
        {
            _longitudeText = decLongitude.ToString();
        }

        return _longitudeText;
    }

    public string WriteElevation(double elevation)
    {
        decimal decElevation = new decimal(elevation * MetersToFeet);
        _elevationText = Math.Round(decElevation, 0).ToString();

        return _elevationText;
    }
    #endregion
}
