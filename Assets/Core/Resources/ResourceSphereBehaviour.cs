using UnityEngine;

public class ResourceSphereBehaviour : MonoBehaviour
{
    [field:SerializeField]
    public float ResourceGain{get;private set;}
    [field:SerializeField]
    public float ResourceDeathTime{get;private set;} = 20;
    [field:SerializeField]
    private string PlayerTag;
    void Start()
    {
        Destroy(gameObject,ResourceDeathTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(PlayerTag))
        {
            BuildingManager.instance.OnResourcePickup(this);
        }
    }
}
