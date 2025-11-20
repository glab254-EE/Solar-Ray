using UnityEngine;

[CreateAssetMenu(fileName = "NormalProjectileSO", menuName = "Scriptable Objects/Projectiles/NormalProjectileSO")]
public class NormalProjectileSO : AProjectileSO
{
    [field:SerializeField]
    internal override float Damage { get; set; }
    [field:SerializeField]
    internal override float LifeTime  { get; set; }
    [field:SerializeField]
    internal override GameObject VisualObject {get; set;}
    [field:SerializeField]
    internal override  HitboxInfo HitboxInfo {get; set;}
    protected internal override bool OnHit(IDamagable damagable, Vector3 position)
    {
        if (damagable != null && damagable.TryDamage(Damage))
        {
            Debug.Log("Hit!");
        }
        return true;
    }
}
