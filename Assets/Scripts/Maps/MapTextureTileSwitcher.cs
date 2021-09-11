using UnityEngine;
using Microsoft.Maps.Unity;

/// <summary>
/// Controller which interacts with the horizontal map's hand menu, enabling the user to control
/// whether the map renders symbolic or aerial tiles, whether it displays roads or labels, and
/// whether it renders buildings in 3D.
/// </summary>
public class MapTextureTileSwitcher : MonoBehaviour
{
    #region Fields
    private DefaultTextureTileLayer _defaultTextureTileLayer;
    private MapRenderer _mapRenderer;
    #endregion

    #region Methods
    // Start is called before the first frame update
    private void Start()
    {
        _defaultTextureTileLayer = GetComponent<DefaultTextureTileLayer>();
        _mapRenderer = GetComponent<MapRenderer>();
    }

    public void SelectAerialTiles()
    {
        _defaultTextureTileLayer.ImageryType = MapImageryType.Aerial;
    }

    public void SelectSymbolicTiles()
    { 
        _defaultTextureTileLayer.ImageryType = MapImageryType.Symbolic;
    }

    public void ToggleRoads()
    {
        _defaultTextureTileLayer.AreRoadsEnabled = !_defaultTextureTileLayer.AreRoadsEnabled;
    }

    public void ToggleLabels()
    {
        _defaultTextureTileLayer.AreLabelsEnabled = !_defaultTextureTileLayer.AreLabelsEnabled;
    }

    public void ToggleBuildings()
    {
        // Buildings show on Default; don't show on Elevated.  Don't use Flat b/c it kills terrain.
        if (_mapRenderer.MapTerrainType == MapTerrainType.Default)
        {
            _mapRenderer.MapTerrainType = MapTerrainType.Elevated;
        }
        else
        {
            _mapRenderer.MapTerrainType = MapTerrainType.Default;
        }
    }
    #endregion
}
