using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;

/// <summary>
/// The mock horizontal map, when placed, spawns its real counterpart and its more detailed
/// MapRenderer, as well as the hand menu and horizontal menu used to control it. It also has
/// to check the MapSpawner for cached data in case it exists because someone needed to move the
/// real horizontal map.
/// </summary>
public class MockHorizontalMap : MockMap
{
    #region Serialized Fields
    #pragma warning disable 0649
    [SerializeField]
    HorizontalMap realHorizontalPrefab;
    #pragma warning restore 0649
    #endregion

    #region Methods
    // Update is called once per frame
    public override void SpawnReal()
    {
        // Take mock map position, build real map, and destroy mock
        HorizontalMap realHorizontalMap = Instantiate(realHorizontalPrefab,
                                                      this.transform.position,
                                                      this.transform.rotation,
                                                      sceneContentTransform);

        // If map data cached for a move, re-import zoom from fields and center.
        if (mapSpawner.IsMapDataCached == true)
        {
            LatLon mapCenter = realHorizontalMap.GetComponent<MapRendererBase>().Center;
            float mapZoomLevel = realHorizontalMap.GetComponent<MapRendererBase>().ZoomLevel;
            mapCenter = mapSpawner.StoredCenter;
            mapZoomLevel = mapSpawner.StoredZoomLevel;
        }
        mapSpawner.HorizontalMapStatus = MapStatus.Real;
        Destroy(this.gameObject);
    }
    #endregion
}
