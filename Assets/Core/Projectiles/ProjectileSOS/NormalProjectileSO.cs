using UnityEngine;

[CreateAssetMenu(fileName = "NormalProjectileSO", menuName = "Scriptable Objects/Projectiles/NormalProjectileSO")]
public class NormalProjectileSO : AProjectileSO
{
    [field:SerializeField]
    internal override float Damage { get; set; }
    [field:SerializeField]
    internal override float LifeTime  { get; set; }
    protected internal override void OnHit(IDamagable damagable, Vector3 position)
    {
        if (damagable == null) return;
        if (damagable.TryDamage(Damage))
        {
            Debug.Log("Hit!");
        }
    }
}
