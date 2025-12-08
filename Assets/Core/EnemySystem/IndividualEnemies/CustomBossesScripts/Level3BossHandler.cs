using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealthHandler))]
public class Level3BossHandler : MonoBehaviour
{
    [field: SerializeField]
    internal EnemySO enemySO;
    [field: SerializeField]
    private GameObject ResourceObject;
    [field: SerializeField]
    private DetectorColliderBehaviour detector;
    internal Transform AttackPoint;
    internal bool isEnabled = false;
    internal bool isAttacking = false;
    internal float AttackCooldown = 0;
    private EnemyHealthHandler healthHandler;
    private Transform playerTransform;
    private Transform currentTarget;
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
        AttackPoint = transform.Find("Attack Point");
        
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

        invoker = new(enemySO);

        isEnabled = true;

    }
    void Update()
    {
        if (isEnabled)
        {
            if (isDead) return;
            GameObject newtarget = detector.GetFirstEnemy();
            if (newtarget != null && newtarget.transform != currentTarget)
            {
                currentTarget = newtarget.transform;
            }
            if (isAttacking || currentTarget == null) return;
            if (Vector3.Distance(transform.position,currentTarget.position) <= enemySO.AttackDistance)
            {
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

    private void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        isEnabled = false;
        //Instantiate(ResourceObject,transform.position+Vector3.up,Quaternion.identity);
        StartCoroutine(DeathEnumerator());
    }
    IEnumerator AttackEnumerator()
    {
        Debug.Log("attacking");
        isAttacking = true;
        AttackCooldown = enemySO.AttackCooldown;
        if (gameObject.TryGetComponent(out Animator a))
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
        if (gameObject.TryGetComponent(out Animator a))
        {
            a.SetTrigger("death");
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
    internal void Attack()
    {
        if (isDead) return;
        if (enemySO.AttackHurtboxSize != null)
        {        
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (child.name == "Attack Point")
                {
                    invoker.TriggerHitboxAndDamage(child);
                }
            }
        }
    }
    internal void Fire()
    {
        if (isDead) return;
        if (enemySO.enemyProjectileInfo.projectileSO != null && AttackPoint != null)
        {
            Vector3 direction = (currentTarget.position-AttackPoint.position).normalized;
            Vector3 RandomizedVector = new Vector3(Random.Range(-enemySO.enemyProjectileInfo.spread,enemySO.enemyProjectileInfo.spread),Random.Range(-enemySO.enemyProjectileInfo.spread,enemySO.enemyProjectileInfo.spread),Random.Range(-enemySO.enemyProjectileInfo.spread,enemySO.enemyProjectileInfo.spread));
            direction = (direction + RandomizedVector).normalized;
            ProjectileManager.Instance.ShootProjectile(AttackPoint.position,direction*enemySO.enemyProjectileInfo.speed,enemySO.enemyProjectileInfo.projectileSO,"Enemies");
        }
    }
}
