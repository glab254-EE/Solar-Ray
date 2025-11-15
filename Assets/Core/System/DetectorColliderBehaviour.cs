using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectorColliderBehaviour : MonoBehaviour
{
    [field:SerializeField]
    private string IncludedTag = "Enemies";
    private List<GameObject> enemyObjects = new();
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(IncludedTag)
        && !enemyObjects.Contains(other.gameObject))
        {
            enemyObjects.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (enemyObjects.Contains(other.gameObject))
        {
            enemyObjects.Remove(other.gameObject);
        }
    }
    public GameObject GetFirstEnemy()
    {
        if (enemyObjects.Count < 1) return null;
        if (enemyObjects[0] == null)
        {
            enemyObjects.RemoveAt(0);
        } 
        if (enemyObjects.Count < 1) return null;
        return enemyObjects[0];
    }
}
