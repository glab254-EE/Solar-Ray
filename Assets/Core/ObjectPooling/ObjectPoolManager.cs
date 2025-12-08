using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public struct ObjectPool
    {
        public string PooledObjectTag;
        public GameObject Prefab;
        public int StartSize;
    }
    [field:SerializeField]
    private List<ObjectPool> pools;
    private Dictionary<string,Queue<GameObject>> currentPooledObjects = new();
    private Dictionary<string,GameObject> pooledObjectParents = new();
    public static ObjectPoolManager Instance;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpPools();
    }
    private void SetUpPools()
    {
        foreach(ObjectPool pool in pools)
        {
            if (pool.PooledObjectTag == "" || pool.Prefab == null) continue;
            GameObject parent = new("Pool - "+pool.PooledObjectTag);
            pooledObjectParents.Add(pool.PooledObjectTag,parent);

            Queue<GameObject> newPool= new();

            for (int i = 0; i < pool.StartSize; i++)
            {
                GameObject newObject = Instantiate(pool.Prefab,parent.transform);
                newPool.Enqueue(newObject);
                newObject.SetActive(false);
            }

            currentPooledObjects.Add(pool.PooledObjectTag,newPool);
        }
    }
    private ObjectPool GetPoolFromTag(string tag)
    {
        foreach(ObjectPool pool in pools)
        {
            if (pool.PooledObjectTag == tag)
            {
                return pool;
            }
        }
        return default;
    }
    private GameObject ReturnPooledObjectByTag(string tag)
    {
        GameObject outputObject = null;
        if (currentPooledObjects.ContainsKey(tag) )
        {
            
            if (currentPooledObjects[tag].Count == 0)
            {
                ObjectPool truePool =  GetPoolFromTag(tag);
                if (truePool.Equals(default))
                {
                    Debug.LogWarning("Failed to create object. pool is default");
                    return null;
                }
                Transform parent = pooledObjectParents[tag].transform;
                GameObject newObject = Instantiate(truePool.Prefab);
                currentPooledObjects[tag].Enqueue(newObject);
                newObject.transform.parent = parent.transform;   
                outputObject = newObject;         
            } else
            {
                outputObject = currentPooledObjects[tag].Dequeue();
            }
            outputObject.SetActive(true);
        }
        return outputObject;
    }
    internal GameObject Create(string tag,Vector3 position, Quaternion orientation)
    {
        GameObject output = null;
        try
        {
            GameObject pooledObject = ReturnPooledObjectByTag(tag);
            if (pooledObject != null)
            {
                pooledObject.transform.SetPositionAndRotation(
                    position, 
                    orientation);
                output = pooledObject;
            }            
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to create object. \nError Message:\n"+e.Message);
        }
        Debug.Log(output==null);
        return output;
    }
    internal void ReturnObject(string tag,GameObject gameObject)
    {
        try
        {
            if (currentPooledObjects.ContainsKey(tag))
            {
                currentPooledObjects[tag].Enqueue(gameObject);
                gameObject.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to return object. \nError Message:\n"+e.Message);
        }        
    }
}
