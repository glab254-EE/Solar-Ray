using UnityEngine;
using UnityEngine.InputSystem;

public class TurretBehaviour : MonoBehaviour, IDamagable
{
    [field: SerializeField]
    private HoverOverInterfaceGiver HoverOverChecker;
    [field:SerializeField]
    private Transform TurningObject;
    [field:SerializeField]
    private Transform MuzzlePoint;    
    [field:SerializeField]
    private DetectorColliderBehaviour detector;
    [field:SerializeField]
    private float MaxAmmunition;
    [field:SerializeField]
    private float TurningSpeed = 1f;
    [field:SerializeField]
    private float ShootDelay = 0.5f;
    [field:SerializeField]
    private GameObject ProjectileGO;
    [field:SerializeField]
    private AProjectileSO projectileSO;
    [field:SerializeField]
    private GameObject EmmiterObject;
    [field:SerializeField]
    private int EmitCount = 15;
    [field:SerializeField]
    private float ProjectileSpeed;
    [field:SerializeField]
    private float RequiredAccuracy = 0.1f;
    [field:SerializeField]
    internal float Health {get;private set;}= 300;
    private float Cooldown = 0;
    private float Ammunition;    
    Transform target;
    void Start()
    {
        Ammunition = MaxAmmunition;
    }
    void Update()
    {
        ReloadCheckLoop();
        
        GameObject foundTarget = detector.GetFirstEnemy();
        if (foundTarget != null)
        {
            target = foundTarget.transform;
        }
        if (target != null)
        {
            TurretLoop();
        }
    }
    private void ReloadCheckLoop()
    { 
        if (HoverOverChecker.IsHoveringOver == true && Mouse.current.leftButton.isPressed && Ammunition < MaxAmmunition)
        {
            Ammunition = MaxAmmunition;
        }
    }
    private void TurretLoop()
    {
        if (Health == 0)
        {
            Destroy(gameObject);
        }
        if (Ammunition > 0 || Ammunition == -1)
        {
            if (TurningObject == null ) // god, i understand how fucked this thing is...
            {
                if (Cooldown <= 0)
                {
                    Vector3 directionVector = (target.position-transform.position).normalized;
                    Cooldown = ShootDelay;
                    ShootProjectile(directionVector);                        
                }
            } else
            {
                Vector3 directionVector = (target.position-TurningObject.position).normalized;
                Quaternion direction = Quaternion.LookRotation(directionVector);

                TurningObject.rotation = Quaternion.RotateTowards(TurningObject.rotation,
                direction,TurningSpeed*Time.deltaTime);

                Quaternion rotationDiff = direction * Quaternion.Inverse(TurningObject.rotation);
                if (rotationDiff.eulerAngles.magnitude <= RequiredAccuracy)
                {
                    if (Cooldown <= 0)
                    {
                        Cooldown = ShootDelay;
                        ShootProjectile(directionVector);
                    }
                }                
            }
        }
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;                
        }
    }
    private void ShootProjectile(Vector3 directionVector)
    {
        Ammunition = Mathf.Clamp(Ammunition-1,-1,MaxAmmunition);
        if (EmmiterObject != null && EmmiterObject.TryGetComponent(out ParticleSystem system))
        {
            system.Emit(EmitCount);
        }
        ProjectileManager.Instance.ShootProjectile(MuzzlePoint.position,directionVector*ProjectileSpeed,projectileSO);
    }
    public bool TryDamage(float damage)
    {
        if (Health<=0) return false;
        Health-=damage;
        return true;
    }
}
