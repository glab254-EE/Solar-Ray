using System;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour,IDamagable
{
    public float Health { get; private set; } = float.NaN;
    internal event Action OnDeath;
    internal event Action OnDamaged;
    public void Initialize(float MaxHealth)
    {
        if (float.IsNaN(Health))
        {
            Health = MaxHealth;            
        }
    }
    public bool TryDamage(float damage)
    {
        if (Health > 0)
        {
            OnDamaged?.Invoke();
            Health -= damage;
            if (Health <= 0 && Health != -1)
            {
                Health = -1;
                OnDeath?.Invoke();
            }
            return true;
        } else
        {
            if (Health <= 0 && Health != -1)
            {
                Health = -1;
                OnDeath?.Invoke();
            }
            return false;
        }
    }
}
