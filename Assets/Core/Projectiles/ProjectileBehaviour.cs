using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    [field:SerializeField]
    private string ProjectileTag = "Projectile";
    private GameObject VisualObject;
    private string ExcludedTag;
    private AProjectileSO currentProjectileSO;
    private bool HitboxActive = false;
    internal void Initialize(AProjectileSO aProjectileSO,string excludedTag)
    {
        ExcludedTag = excludedTag;
        currentProjectileSO = aProjectileSO;
        if (currentProjectileSO.VisualObject != null)
        {
            VisualObject = Instantiate(currentProjectileSO.VisualObject,transform);
        }
        if (currentProjectileSO.HitboxInfo.size.magnitude > 0 && transform.TryGetComponent(out BoxCollider component))
        {
            component.size = currentProjectileSO.HitboxInfo.size;
            component.center = currentProjectileSO.HitboxInfo.offset;
        }
        if (currentProjectileSO.LifeTime > 0)
        {
            StartCoroutine(LifetimeEnumerator());
        }  
        HitboxActive = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!HitboxActive) return;
        if (other.gameObject != null && (ExcludedTag == null || other.gameObject.CompareTag(ExcludedTag) == false) && other.gameObject.CompareTag("Detectors") == false)
        {
            var damagable = other.gameObject.GetComponent<IDamagable>();
            bool ToDestroy = currentProjectileSO.OnHit(damagable,other.ClosestPoint(transform.position));
            if (ToDestroy)
            {
                DisableObject(); 
            }
        }
    }
    private IEnumerator LifetimeEnumerator()
    {
        yield return new WaitForSeconds(currentProjectileSO.LifeTime);
        DisableObject(); 
    }
    private void DisableObject()
    {
        HitboxActive = false;
        if (VisualObject != null)
        {
            Destroy(VisualObject);
        }
        ObjectPoolManager.Instance.ReturnObject(ProjectileTag,gameObject);
    }
}
