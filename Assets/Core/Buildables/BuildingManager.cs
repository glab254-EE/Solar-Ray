using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    internal static BuildingManager instance;
    internal float Resources {get;private set;} = new();
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    internal void OnResourcePickup(ResourceSphereBehaviour sphere)
    {
        if (sphere == null) return;
        Resources += sphere.ResourceGain;
        Destroy(sphere.gameObject);
    }
    internal bool TryPlaceObject(ABuildableSO builtObject,Vector3 position, Quaternion rotation)
    {
        if (builtObject == null || position == null || rotation == null) return false;
        if (builtObject.BuildCost <= Resources)
        {
            Resources -= builtObject.BuildCost;
        } else
        {
            return false;
        }
        GameObject newPlacedObject = Instantiate(builtObject.Model, position, rotation);
        builtObject.OnPlace(newPlacedObject,builtObject);
        return true;
    }
}
