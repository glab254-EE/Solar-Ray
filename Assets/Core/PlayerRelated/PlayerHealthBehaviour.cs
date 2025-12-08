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
    internal bool Godded { get; private set; } = false;
    internal event Action OnPlayerDeath;
    private float goddedTimer;
    void Start()
    {
        Health = MaxHealth;
    }
    void Update()
    {
        if (goddedTimer > 0)
        {
            Godded = true;
            goddedTimer -= Time.deltaTime;
        } else if (goddedTimer != float.NaN)
        {
            Godded = false;
            goddedTimer = 0;
        }
    }
    public bool TryDamage(float damage)
    {
        if (Godded)
        {
            return false;
        } else
        {
            Health -= damage;
            Godded = true;
            if (Health <= 0)
            {
                goddedTimer = float.NaN;
                OnPlayerDeath?.Invoke();
            } else
            {
                goddedTimer = takeDamageCooldown;                
            }
            return true;
        }
    }
    private IEnumerator DeathEnumerator()
    {
        yield return new WaitForSecondsRealtime(10);
        GameScenesManager.LoadScene();
    }
}
