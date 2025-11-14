using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveBehaviour : MonoBehaviour
{
    [field: SerializeField]
    private Terrain terrain;
    [field: SerializeField]
    private Transform playerTransform;
    [field: SerializeField]
    private float MaxRange = 4f;
    [field: SerializeField]
    private GameObject EnemyTemplate;
    [field: SerializeField]
    private List<EnemySO> Enemies = new();
    [field: SerializeField]
    private float EnemiesSpawnRange = 2f;
    [field: SerializeField]
    private float EnemiesSpawnCooldown = 1.5f;
    [field: SerializeField]
    private float EnemiesSpawnDuration = 60f;
    internal bool WaveFinished { get; private set; } = false;
    private bool spawningActive = false;
    private float MainTimer = 0f;
    private float SpawningTimer = 0f;
    void Update()
    {
        if (!spawningActive) return;
        MainTimer += Time.deltaTime;
        SpawningTimer += Time.deltaTime;
        if (MainTimer >= EnemiesSpawnDuration)
        {
            spawningActive = false;
            WaveFinished = true;
        }
        if (SpawningTimer >= EnemiesSpawnCooldown)
        {
            SpawnRandomEnemy();
        }
    }
    void OnMouseUpAsButton()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= MaxRange && !WaveFinished && !spawningActive)
        {
            spawningActive = true;
        }
    }
    private void SpawnRandomEnemy()
    {
        if (EnemyTemplate == null || Enemies == null) return;

        EnemySO randomEnemy = Enemies[UnityEngine.Random.Range(0, Enemies.Count)];
        float RandomDirection = UnityEngine.Random.Range(0.00f, 1.00f);

        float X = Mathf.Sin(RandomDirection) * EnemiesSpawnRange;
        float Y = Mathf.Cos(RandomDirection) * EnemiesSpawnRange;

        Vector3 newposition = transform.position + new Vector3(X, 0, Y);
        newposition.y = terrain.SampleHeight(newposition);

        GameObject newenemy = Instantiate(EnemyTemplate, newposition, Quaternion.identity);
        if (newenemy.TryGetComponent<EnemyHandler>(out EnemyHandler handler))
        {
            handler.EnemySO = randomEnemy;
        }
    }
}
