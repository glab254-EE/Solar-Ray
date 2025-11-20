using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveProjectileSO", menuName = "Scriptable Objects/Projectiles/ExplosiveProjectileSO")]
public class ExplosiveProjectileSO : AProjectileSO
{
    [field:SerializeField]
    internal override float Damage { get; set; }
    [field:SerializeField]
    internal override float LifeTime  { get; set; }
    [field:SerializeField]
    internal override GameObject VisualObject {get; set;}
    [field:SerializeField]
    internal override  HitboxInfo HitboxInfo {get; set;}
    [field:SerializeField]
    internal float ExplosiveDamage { get; set; }
    [field:SerializeField]
    internal float ExplosionRadius { get; set; }
    [field: SerializeField]
    internal string ExcludedTag { get; set; }
    protected internal override bool OnHit(IDamagable damagable, Vector3 position)
    {
        if (damagable != null && damagable.TryDamage(Damage))
        {
            Debug.Log("Hit!");
        }
        RaycastHit[] hits = Physics.SphereCastAll(position - new Vector3(0,0,ExplosionRadius/2), ExplosionRadius, new Vector3(0,0,1), ExplosionRadius);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider != null 
            && hit.collider.gameObject != null 
            && hit.collider.gameObject.CompareTag(ExcludedTag) == false 
            && hit.collider.gameObject.TryGetComponent(out IDamagable damagable1))
            {
                if (damagable1.TryDamage(ExplosiveDamage))
                {
                    Debug.Log("Hit explosive!");
                }              
            }
        }
        return true;
    }
}
