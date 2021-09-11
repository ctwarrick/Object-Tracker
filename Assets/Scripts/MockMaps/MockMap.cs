using UnityEngine;

/// <summary>
/// Base class for the horizontal and vertical mock maps.  Sets up the main camera's MapSpawner
/// for control of map states, and sets the MixedRealityPlaySpace's Transform as the parent
/// for both horizontal and vertical maps, which each spawn their real counterpart differently.
/// </summary>
public abstract class MockMap : MonoBehaviour
{
    #region Fields
    protected Transform sceneContentTransform;
    protected MapSpawner mapSpawner;
    #endregion

    // Start is called before the first frame update
    protected void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mapSpawner = mainCamera.GetComponent<MapSpawner>();

        GameObject sceneContent = GameObject.FindGameObjectWithTag("SceneContent");
        sceneContentTransform = sceneContent.GetComponent<Transform>();
    }

    public abstract void SpawnReal();
}
