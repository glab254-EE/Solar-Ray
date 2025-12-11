using UnityEngine;

public class MoneySphere : MonoBehaviour
{
    [field:SerializeField]
    public float MoneyGain{get;private set;} = 1;
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
            BuildingManager.instance.OnMoneyPickup(this);
            if (collision.gameObject.TryGetComponent(out PlayerHealthBehaviour playerHealth))
            {
                playerHealth.TryDamage(-HealingPower);
            }
        }
    }
}
