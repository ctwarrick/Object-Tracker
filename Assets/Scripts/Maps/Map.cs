using UnityEngine;
using Microsoft.Maps.Unity;

/// <summary>
/// Base class for the horizontal and vertical maps which contain MapRenderer objects and actually
/// display map data once placed.
/// </summary>
public abstract class Map : MonoBehaviour
{
    #region Fields
    protected MapRendererBase mapRendererBase;
    #endregion

    #region Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        mapRendererBase = GetComponent<MapRendererBase>();
    }
    public abstract void MoveMap();
    #endregion
}
