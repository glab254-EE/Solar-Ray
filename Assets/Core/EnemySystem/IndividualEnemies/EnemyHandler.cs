using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)),RequireComponent(typeof(EnemyHealthHandler))]
public class EnemyHandler : MonoBehaviour
{
    [field: SerializeField]
    internal EnemySO EnemySO;
    internal Transform AttackPoint;
    internal bool isEnabled = false;
    internal bool isAttacking = false;
    internal float AttackCooldown = 0;
    private EnemyHealthHandler healthHandler;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private GameObject enemyVisual;
    private GeneralPurposeEventBehaviour AttackEvent;
    void Start()
    {
        if (EnemySO == null)
        {
            Debug.LogWarning("Enemy failed to initialize due of no enemy SO.");
            return;
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (EnemySO.EnemyVisualPrefab != null)
        {
            enemyVisual = Instantiate(EnemySO.EnemyVisualPrefab, transform);
            AttackPoint = enemyVisual.transform.Find("Attack Point");
            AttackEvent = enemyVisual.GetComponent<GeneralPurposeEventBehaviour>();
            if (AttackEvent != null)
            {
                AttackEvent.connections += Attack;
            }
        }

        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Initialize(EnemySO.MaxHealth);
        healthHandler.OnDeath += OnDeath;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = EnemySO.MoveSpeed;
        agent.angularSpeed = EnemySO.TurnSpeed;
        agent.acceleration = EnemySO.MoveAcceleration;
        agent.stoppingDistance = EnemySO.StoppingDistance;

        isEnabled = true;
    }
    void Update()
    {
        if (isEnabled)
        {
            if (!isAttacking)
            {        
                if (Vector3.Distance(transform.position,playerTransform.position) > EnemySO.AttackDistance)
                {
                    if (Vector3.Distance(agent.destination,playerTransform.position) > 0.1f)
                    {
                        agent.SetDestination(playerTransform.position);                          
                    }                  
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
            }
        }
    }
    void OnDeath()
    {
        isEnabled = false;
        StartCoroutine(DeathEnumerator());
    }
    IEnumerator AttackEnumerator()
    {
        isAttacking = true;
        AttackCooldown = EnemySO.AttackCooldown;
        if (enemyVisual.TryGetComponent(out Animator a))
        {
            a.SetTrigger("attack");
        }
        else
        {
            Debug.Log("Attacked without animator");
        }
        yield return new WaitForSeconds(EnemySO.AttackDuration);
        isAttacking = false;   
    }
    IEnumerator DeathEnumerator()
    {
        isEnabled = false;
        if (enemyVisual.TryGetComponent(out Animator a))
        {
            a.SetTrigger("death");
        }
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
    internal void Attack()
    {
        if (EnemySO.AttackHitboxSize != null && AttackPoint != null)
        {
            if (Physics.BoxCast(AttackPoint.position,EnemySO.AttackHitboxSize/2,AttackPoint.forward,out RaycastHit hit, AttackPoint.rotation, EnemySO.AttackHitboxSize.z))
            {
                if (hit.transform == playerTransform && hit.transform.gameObject.TryGetComponent(out IDamagable damagable))
                {
                    Debug.Log("Damaged!");
                    damagable.TryDamage(EnemySO.AttackDamage);
                }
            }
        }
    }
}
