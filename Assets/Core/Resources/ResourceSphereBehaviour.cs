using UnityEngine;

public class ResourceSphereBehaviour : MonoBehaviour
{
    [field:SerializeField]
    public float ResourceGain{get;private set;}
    [field:SerializeField]
    public float ResourceDeathTime{get;private set;} = 20;
    [field:SerializeField]
    private float HealingPower = 20f;
    [field:SerializeField]
    private string PlayerTag;
    void Start()
    {
        Destroy(gameObject,ResourceDeathTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            BuildingManager.instance.OnResourcePickup(this);
            if (collision.gameObject.TryGetComponent(out PlayerHealthBehaviour playerHealth))
            {
                playerHealth.TryDamage(-HealingPower);
            }
        }
    }
}
