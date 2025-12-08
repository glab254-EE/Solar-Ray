using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField]
    public float MaxHealth { get; private set; } = 3f;
    [field: SerializeField]
    public float MoveSpeed { get; private set; } = 5;
    [field: SerializeField]
    public float MoveAcceleration { get; private set; } = 2.5f;
    [field: SerializeField]
    public float TurnSpeed { get; private set; } = 1f;
    [field: SerializeField]
    public float StoppingDistance { get; private set; } = .5f;
    [field: SerializeField]
    public float AttackDamage { get; private set; } = 1f;
    [field: SerializeField]
    public float AttackDistance { get; private set; } = 10f;
    [field: SerializeField]
    public float AttackDuration { get; private set; } = .5f;
    [field: SerializeField]
    public float AttackCooldown { get; private set; } = 2f;
    [field: SerializeField]
    public Vector3 AttackHurtboxSize { get; private set; } = Vector3.one;
    [field: SerializeField]
    public GameObject EnemyVisualPrefab { get; private set; }
    [field: SerializeField]
    public HitboxInfo hitboxInfo{get;private set;}
    [field: SerializeField]
    public EnemyProjectileInfo enemyProjectileInfo{get;private set;}
}
[Serializable]
public struct HitboxInfo
{
    public Vector3 size;
    public Vector3 offset;
}
[Serializable]
public struct EnemyProjectileInfo
{
    public float speed ;
    public float spread;
    public AProjectileSO projectileSO;
    public GameObject projectile;
}