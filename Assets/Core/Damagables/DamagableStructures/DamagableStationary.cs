using System;
using UnityEngine;

public class DamagableStationary : MonoBehaviour, IDamagable
{
    [field:SerializeField]
    internal float MaxHealth { get; private set; } = 10f;
    [field: SerializeField]
    internal float Health { get; private set; }   
    internal event Action OnDestroy;
    void Start()
    {
        Health = MaxHealth;
    }
    public bool TryDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDestroy?.Invoke();
            Destroy(gameObject);
        }
        return true;
    }
}
