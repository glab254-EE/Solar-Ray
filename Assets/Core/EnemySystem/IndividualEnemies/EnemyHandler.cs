using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)),RequireComponent(typeof(EnemyHealthHandler))]
public class EnemyHandler : MonoBehaviour
{
    [field: SerializeField]
    internal EnemySO EnemySO;
    [field: SerializeField]
    private GameObject ResourceObject;
    [field: SerializeField]
    private DetectorColliderBehaviour detector;
    internal Transform AttackPoint;
    internal bool isEnabled = false;
    internal bool isAttacking = false;
    internal float AttackCooldown = 0;
    private EnemyHealthHandler healthHandler;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Transform currentTarget;
    private GameObject enemyVisual;
    private GeneralPurposeEventBehaviour AttackEvent;
    private bool isDead = false;
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

        if (gameObject.TryGetComponent(out BoxCollider collider))
        {
            collider.size = EnemySO.hitboxInfo.size;
            collider.center = EnemySO.hitboxInfo.offset;
        } 
        if (gameObject.TryGetComponent(out SphereCollider collider2))
        {
            collider2.radius = EnemySO.hitboxInfo.size.x;
            collider2.center = EnemySO.hitboxInfo.offset;
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

        currentTarget = playerTransform;
    }
    void Update()
    {
        if (isEnabled)
        {
            GameObject? newtarget = detector.GetFirstEnemy();
            if (newtarget != null && newtarget.transform != currentTarget)
            {
                currentTarget = newtarget.transform;
            }
            if (!isAttacking)
            {        
                if (Vector3.Distance(transform.position,currentTarget.position) > EnemySO.AttackDistance)
                {
                    if (Vector3.Distance(agent.destination,currentTarget.position) > 0.1f)
                    {
                        agent.SetDestination(currentTarget.position);                          
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
        if (isDead) return;
        isDead = true;
        isEnabled = false;
        Instantiate(ResourceObject,transform.position+Vector3.up,Quaternion.identity);
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
        if (EnemySO.AttackHurtboxSize != null && AttackPoint != null)
        {
            Collider[] hits = Physics.OverlapBox(AttackPoint.position,EnemySO.AttackHurtboxSize,AttackPoint.rotation);
            foreach (Collider hit in hits)
            {
                Debug.Log("Checking.");
                if (hit.CompareTag("Player") && hit.transform.gameObject.TryGetComponent(out IDamagable damagable))
                {
                    Debug.Log("Damaged!");
                    damagable.TryDamage(EnemySO.AttackDamage);
                }
            }
        }
    }
}
