using System.Collections.Generic;
using UnityEngine;

public class OneventEnemySpawner : MonoBehaviour
{
    [field:SerializeField]
    private GeneralPurposeEventBehaviour ActivationEvent;
    [field:SerializeField]
    private List<EnemySO> ToSpawn;
    [field:SerializeField]
    private GameObject EnemyTemplate;
    void Start()
    {
        ActivationEvent.connections += SpawnEnemies;
    }
    private void SpawnEnemies()
    {
        if (ToSpawn != null && ToSpawn.Count > 0)
        {
            foreach (EnemySO enemySO in ToSpawn)
            {
                
                GameObject newenemy = Instantiate(EnemyTemplate, transform.position, Quaternion.identity);
                if (newenemy.TryGetComponent<EnemyHandler>(out EnemyHandler handler))
                {
                    handler.enemySO = enemySO;
                }
            }
        }
        // TODO: ADD SPAWNING
    }
}
