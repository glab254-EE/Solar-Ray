using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : MonoBehaviour
{
    internal static BuildingManager instance;
    internal static float Money {get;private set;} = new();
    internal float Resources {get;private set;} = new();
    internal event Action OnBuildEvent;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
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
        float cost = builtObject.BuildCost;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            cost = 1;
        }
        if (cost<= Resources)
        {
            Resources -= cost;
        } else
        {
            return false;
        }
        GameObject newPlacedObject = Instantiate(builtObject.Model, position, rotation);
        OnBuildEvent?.Invoke();
        return true;
    }
}
