using UnityEngine;

public abstract class AProjectileSO : ScriptableObject
{
    internal abstract float Damage{ get; set; }
    internal abstract float LifeTime{ get; set; }
    internal protected abstract void OnHit(IDamagable damagable, Vector3 position);
}
