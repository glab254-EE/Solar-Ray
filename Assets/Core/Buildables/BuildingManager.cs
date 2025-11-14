using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    internal static BuildingManager instance;
    internal Dictionary<ABuildableSO, int> buildingCounts { get; private set; } = new();
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    internal bool TryPlaceObject(ABuildableSO builtObject,Vector3 position, Quaternion rotation)
    {
        if (builtObject == null || position == null || rotation == null) return false;
        if (buildingCounts.ContainsKey(builtObject))
        {
            if (buildingCounts[builtObject] <= 0)
            {
                return false;
            }
            else
            {
                buildingCounts[builtObject] -= 1;
            }
        }
        else
        {
            buildingCounts.Add(builtObject, builtObject.MaxBuildCount);
            buildingCounts[builtObject] -= 1;
        }
        GameObject newPlacedObject = Instantiate(builtObject.Model, position, rotation);
        builtObject.OnPlace(newPlacedObject);
        return true;
    }
}
