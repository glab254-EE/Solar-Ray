using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    private string ExcludedTag;
    private AProjectileSO currentProjectileSO;
    internal void Initialize(AProjectileSO aProjectileSO,string excludedTag)
    {
        ExcludedTag = excludedTag;
        currentProjectileSO = aProjectileSO;
        Destroy(gameObject,currentProjectileSO.LifeTime);    
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && (ExcludedTag == null || other.gameObject.CompareTag(ExcludedTag) == false))
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                currentProjectileSO.OnHit(damagable, other.ClosestPoint(transform.position));              
            } else
            {
                currentProjectileSO.OnHit(null, other.ClosestPoint(transform.position));  
            }
            Destroy(gameObject);  
        }
    }
}
