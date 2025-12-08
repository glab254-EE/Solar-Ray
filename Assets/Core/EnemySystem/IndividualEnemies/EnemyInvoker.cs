using UnityEngine;

public class EnemyInvoker
{
    private EnemySO enemySO;
    internal void TriggerHitboxAndDamage(Transform AttackPoint)
    {
        
        RaycastHit[] hits = Physics.BoxCastAll(AttackPoint.position,enemySO.AttackHurtboxSize/2,AttackPoint.forward);
        foreach (RaycastHit _hit in hits)
        {
            Collider hit = _hit.collider;
            if (hit.CompareTag("Player") && hit.transform.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TryDamage(enemySO.AttackDamage);
            }
        }
    }
    public EnemyInvoker(EnemySO enemy)
    {
        enemySO = enemy;
    }
}
