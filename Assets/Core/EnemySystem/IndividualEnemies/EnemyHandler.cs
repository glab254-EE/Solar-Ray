using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent)),RequireComponent(typeof(EnemyHealthHandler))]
public class EnemyHandler : MonoBehaviour
{
    [field: SerializeField]
    internal EnemySO enemySO;
    [field: SerializeField]
    private GameObject ResourceObject;
    [field: SerializeField]
    private DetectorColliderBehaviour detector;
    internal Transform AttackPoint;
    internal Transform currentTarget;
    internal bool isEnabled = false;
    internal bool isAttacking = false;
    internal float AttackCooldown = 0;
    private EnemyHealthHandler healthHandler;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private GameObject enemyVisual;
    private GeneralPurposeEventBehaviour AttackEvent;
    private bool isDead = false;
    private EnemyInvoker invoker;
    void Start()
    {
        if (enemySO == null)
        {
            Debug.LogWarning("Enemy failed to initialize due of no enemy SO.");
            return;
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (enemySO.EnemyVisualPrefab != null)
        {
            enemyVisual = Instantiate(enemySO.EnemyVisualPrefab, transform);
            AttackPoint = enemyVisual.transform.Find("Attack Point");
            AttackEvent = enemyVisual.GetComponent<GeneralPurposeEventBehaviour>();
            if (AttackEvent != null)
            {
                AttackEvent.connections += Attack;
            }
        }

        if (gameObject.TryGetComponent(out BoxCollider collider))
        {
            collider.size = enemySO.hitboxInfo.size;
            collider.center = enemySO.hitboxInfo.offset;
        } 
        if (gameObject.TryGetComponent(out SphereCollider collider2))
        {
            collider2.radius = enemySO.hitboxInfo.size.x;
            collider2.center = enemySO.hitboxInfo.offset;
        } 

        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Initialize(enemySO.MaxHealth);
        healthHandler.OnDeath += OnDeath;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySO.MoveSpeed;
        agent.angularSpeed = enemySO.TurnSpeed;
        agent.acceleration = enemySO.MoveAcceleration;
        agent.stoppingDistance = enemySO.StoppingDistance;

        invoker = new(enemySO);

        isEnabled = true;

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
        currentTarget = playerTransform;            
        }
    }
    void Update()
    {
        if (isEnabled)
        {
            if (isDead) return;
            if (isAttacking) return;
            if (Vector3.Distance(transform.position,currentTarget.position) > enemySO.AttackDistance)
            {
                agent.SetDestination(currentTarget.position);   
            } else
            {
                if (agent.destination != transform.position)
                {
                    agent.SetDestination(transform.position);
                }
                if (AttackCooldown < 0)
                {
                    StartCoroutine(AttackEnumerator());
                } else
                {
                    AttackCooldown -= Time.deltaTime;
                }
            }
            GameObject newtarget = detector.GetFirstEnemy();
            if (newtarget != null && newtarget.transform != currentTarget)
            {
                currentTarget = newtarget.transform;
            }
        }
    }

    private void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        isEnabled = false;
        Instantiate(ResourceObject,transform.position+Vector3.up,Quaternion.identity);
        StartCoroutine(DeathEnumerator());
    }
    IEnumerator AttackEnumerator()
    {
        isAttacking = true;
        AttackCooldown = enemySO.AttackCooldown;
        if (enemyVisual.TryGetComponent(out Animator a))
        {
            a.SetTrigger("attack");
        }
        else
        {
            Debug.Log("Attacked without animator");
        }
        yield return new WaitForSeconds(enemySO.AttackDuration);
        isAttacking = false;   
    }
    IEnumerator DeathEnumerator()
    {
        isEnabled = false;
        if (enemyVisual.TryGetComponent(out Animator a))
        {
            a.SetTrigger("death");
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
    internal void Attack()
    {
        if (isDead) return;
        if (enemySO.AttackHurtboxSize != null && AttackPoint != null)
        {
            invoker.TriggerHitboxAndDamage(AttackPoint);
        }
        if (enemySO.enemyProjectileInfo.projectile != null && AttackPoint != null)
        {
            Vector3 direction = (currentTarget.position-AttackPoint.position).normalized;
            ProjectileManager.Instance.ShootProjectile(AttackPoint.position,direction*enemySO.enemyProjectileInfo.speed,enemySO.enemyProjectileInfo.projectileSO,"Enemies");
        }
    }
    internal void Fire()
    {
        if (isDead) return;
        if (enemySO.enemyProjectileInfo.projectile != null && AttackPoint != null)
        {
            Vector3 direction = (currentTarget.position-AttackPoint.position).normalized;
            ProjectileManager.Instance.ShootProjectile(AttackPoint.position,direction*enemySO.enemyProjectileInfo.speed,enemySO.enemyProjectileInfo.projectileSO,"Enemies");
        }
    }
}
