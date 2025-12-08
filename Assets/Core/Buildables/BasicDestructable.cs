using UnityEngine;

public class BasicDestructable : MonoBehaviour, IDamagable
{
    [field:SerializeField]
    internal float Health;
    [field:SerializeField]
    private float DamageOnStay;
    private bool isDead = false;
    public bool TryDamage(float damage)
    {
        if (isDead) return false;
        Health -= damage;
        isDead = Health <= 0;
        if (isDead) OnDeath();
        return true;
    }
    void OnCollisionStay(Collision collision)
    {
        if (DamageOnStay <= 0 || collision.gameObject.CompareTag("Player") == false)
        {
            return;
        }
        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TryDamage(DamageOnStay*Time.deltaTime);
        }
    }
    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
