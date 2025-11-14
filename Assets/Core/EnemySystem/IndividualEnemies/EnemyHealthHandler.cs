using System;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour,IDamagable
{
    public float Health { get; private set; } = float.NaN;
    internal event Action OnDeath;
    public void Initialize(float MaxHealth)
    {
        if (float.IsNaN(Health))
        {
            Health = MaxHealth;            
        }
    }
    public bool TryDamage(float damage)
    {
        if (Health >= 0)
        {
            Health -= damage;
            if (Health <= 0)
            {
                OnDeath?.Invoke();
            }
            return true;
        } else
        {
            return false;
        }
    }
}
