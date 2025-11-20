using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [field:SerializeField]
    private string ProjectileTag = "Projectile";
    public static ProjectileManager Instance;
    private ObjectPoolManager poolManager;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        poolManager = ObjectPoolManager.Instance;        
    }
    internal GameObject ShootProjectile(Vector3 origin, Vector3 speed, AProjectileSO projectile,string ExcludedTag = "Player")
    {
        if (origin == null || speed == null || projectile == null) return null;
        GameObject newbullet = poolManager.Create(ProjectileTag,origin,Quaternion.LookRotation(speed.normalized));
        if (newbullet == null)
        {
            Debug.LogWarning("No bullet were created.");
            return null;
        }
        if (newbullet.TryGetComponent(out ProjectileBehaviour behaviour))
        {
            behaviour.Initialize(projectile,ExcludedTag);
        }
        if (newbullet.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.linearVelocity = speed;
        }
        return newbullet;
    }
}
