using System.Collections.Generic;
using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using Newtonsoft.Json;

/// <summary>
/// This script imports tracked freight objects from a JSON file, adds them to a list,
/// and creates MapPins of them for the Bing Maps SDK.
/// </summary>
public class MapPinManager : MonoBehaviour
{
    #region Serialized Fields
    #pragma warning disable 0649
    [SerializeField]
    GameObject mapPinPrefab;
    #pragma warning restore 0649
    #endregion

    #region Fields
    private MapPinLayer _mapPinLayer;
    private List<ImportObject> _importObjects;
    #endregion

    #region Methods
    // Start is called before the first frame update
    private void Start()
    {
        // Grab JSON
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath,
                                                 "TrackedObjects.json");

        string jsonFile = System.IO.File.ReadAllText(filePath);

        // Deserialize JSON into TrackedObjects
        // This imports them ALL; may not scale past MVP demo.
        _importObjects = JsonConvert.DeserializeObject<List<ImportObject>>(jsonFile);

        // Get the map pin container
        _mapPinLayer = this.gameObject.GetComponent<MapPinLayer>();

        // Add to MapPinLayer
        foreach(ImportObject obj in _importObjects)
        {
            // Instantiate the marker under the mapPinLayer and pull the MapPin from GameObject
            GameObject mapPinObject = Instantiate(mapPinPrefab, _mapPinLayer.transform);
            MapPin mapPin = mapPinObject.GetComponent<MapPin>();

            // Pull the TrackedObject script from GameObject and pass data from TrackedObjectData
            // This was because Newtonsoft and Unity weren't playing nice; refactor if needed
            TrackedObject trackedObject = mapPinObject.GetComponent<TrackedObject>();
            trackedObject.LatLonAlt = new LatLonAlt(obj.Latitude, obj.Longitude, obj.Altitude);
            trackedObject.Velocity = obj.Velocity;
            trackedObject.Origin = obj.Origin;
            trackedObject.Destination = obj.Destination;
            trackedObject.EstimatedArrival = obj.EstimatedArrival;

            // Set location from tracked object; maybe make this a latlon in the JSON later
            mapPin.Location = trackedObject.LatLonAlt.LatLon;

            _mapPinLayer.MapPins.Add(mapPin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Expand this later to add more objects post-MVP
    }
    #endregion
}
