using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;

/// <summary>
/// Controls the spawning of mock and real maps
/// </summary>
public class MapSpawner : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    public GameObject mockHorizontalPrefab;

    [SerializeField]
    public GameObject mockVerticalPrefab;

    #endregion

    #region Fields
    private MapStatus _horizontalMapStatus;
    private MapStatus _verticalMapStatus;

    // Know if you need to pull data from a moved map
    private bool _isMapDataCached;
    private bool _isMapPlaced;

    // Fields to hold info for map location while the object is being moved
    private LatLon _storedCenter;
    private float _storedZoomLevel;
    #endregion

    #region Properties
    public bool IsMapPlaced
    {
        get { return _isMapPlaced; }
        set { _isMapPlaced = value; }
    }
    public bool IsMapDataCached
    {
        get { return _isMapDataCached; }
        set { _isMapDataCached = value; }
    }
    public MapStatus VerticalMapStatus
    {
        get { return _verticalMapStatus; }
        set { _verticalMapStatus = value; }
    }
    public MapStatus HorizontalMapStatus
    {
        get { return _horizontalMapStatus; }
        set { _horizontalMapStatus = value; }
    }

    public LatLon StoredCenter
    {
        get { return _storedCenter; }
        set { _storedCenter = value; }
    }
    public float StoredZoomLevel
    {
        get { return _storedZoomLevel; }
        set { _storedZoomLevel = value; }
    }
    #endregion

    #region Methods
    // MapSpawner starts at Awake so it is the first thing loaded
    private void Awake()
    {
        // At first, no map spawned, no data to import
        _horizontalMapStatus = MapStatus.Blank;
        _verticalMapStatus = MapStatus.Blank;
        _isMapDataCached = false;
        _isMapPlaced = false;
    }
    private void Update()
    {
        // If no maps, spawn mock maps; Unity will turn them into real ones
        while (_verticalMapStatus == MapStatus.Blank)
        {
            SpawnMockVertical();
        }
        while (_horizontalMapStatus == MapStatus.Blank && _verticalMapStatus == MapStatus.Real)
        {
            SpawnMockHorizontal();
        }
    }

    public void SpawnMockHorizontal()
    {
        // Pull map rotations from the prefabs or Unity will instantiate however it wants
        Quaternion mapRotation = mockHorizontalPrefab.transform.rotation;

        // Instantiate map as child of MixedRealityPlayspace
        GameObject mockHorizontalMap = Instantiate(mockHorizontalPrefab,
                                                   this.transform.parent.position,
                                                   mapRotation,
                                                   this.transform.parent);
        _horizontalMapStatus = MapStatus.Mock;
    }
    public void SpawnMockVertical()
    {
        // Pull map rotations from the prefabs or Unity will instantiate however it wants
        Quaternion mapRotation = mockVerticalPrefab.transform.rotation;

        // Instantiate under MixedRealityPlaySpace at the center with prefab's rotation
        GameObject mockVerticalMap = Instantiate(mockVerticalPrefab,
                                                 this.transform.parent.position,
                                                 mapRotation,
                                                 this.transform.parent);
        _verticalMapStatus = MapStatus.Mock;
    }
  
    public void MoveRealHorizontal()
    {
        // Cache map state before you destroy it
        GameObject realHorizontalMap = GameObject.FindGameObjectWithTag("RealHorizontal");
        _storedCenter = realHorizontalMap.GetComponent<MapRendererBase>().Center;
        _storedZoomLevel = realHorizontalMap.GetComponent<MapRendererBase>().ZoomLevel;
        Destroy(realHorizontalMap);

        // Setting to blank will cause Update to spawn a mock
        _horizontalMapStatus = MapStatus.Blank;
    }
    #endregion
}
