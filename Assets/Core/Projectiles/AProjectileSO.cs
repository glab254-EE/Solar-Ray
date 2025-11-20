using UnityEngine;

public abstract class AProjectileSO : ScriptableObject
{
    internal abstract float Damage{ get; set; }
    internal abstract float LifeTime{ get; set; }
    internal abstract GameObject VisualObject{get;set;}
    internal abstract HitboxInfo HitboxInfo{ get; set; }
    internal protected abstract bool OnHit(IDamagable damagable, Vector3 position);
}