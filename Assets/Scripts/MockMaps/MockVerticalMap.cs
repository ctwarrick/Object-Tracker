using UnityEngine;

/// <summary>
/// The mock vertical map spawns a wall-mounted overview map when placed.
/// </summary>
public class MockVerticalMap : MockMap
{
    #region Serialized Fields
    #pragma warning disable 0649
    [SerializeField]
    VerticalMap realVerticalPrefab;
    #pragma warning restore 0649
    #endregion

    public override void SpawnReal()
    {
        // Instantiate map as child of MixedRealityPlayspace
        VerticalMap realVerticalMap = Instantiate(realVerticalPrefab,
                                                  this.transform.position,
                                                  this.transform.rotation,
                                                  sceneContentTransform);

        // You have to rotate the real map -90 degrees because it's a 1,1,1 scale,
        // not a morphed cube
        Vector3 rotationDelta = new Vector3(-90, 0, 0);
        realVerticalMap.transform.eulerAngles += rotationDelta;

        mapSpawner.VerticalMapStatus = MapStatus.Real;
        mapSpawner.IsMapPlaced = true;
        Destroy(this.gameObject);
    }
}
