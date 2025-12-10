using System;
using System.Collections;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float takeDamageCooldown = 1f;
    [field:SerializeField]
    internal float MaxHealth { get; private set; } = 10f;
    [field:SerializeField]
    internal float Health { get; private set; }
    internal bool Dead {get;private set;} = false;
    internal event Action OnPlayerDeath;
    void Start()
    {
        Health = MaxHealth;
    }
    void FixedUpdate()
    {
        if (!Dead && Health <= 0)
        {
            Dead = true;
            OnDeath();
        }
    }
    public bool TryDamage(float damage)
    {
        Health = Math.Clamp(Health-damage,0,MaxHealth);
        if (Health <= 0&&!Dead)
        {
            OnDeath();
        }
        return true;
    }
    private void OnDeath()
    {
        Dead = true;
        OnPlayerDeath?.Invoke();
        StartCoroutine(DeathEnumerator());
    }
    private IEnumerator DeathEnumerator()
    {
        yield return new WaitForSecondsRealtime(10);
        GameScenesManager.LoadScene();
    }
}
