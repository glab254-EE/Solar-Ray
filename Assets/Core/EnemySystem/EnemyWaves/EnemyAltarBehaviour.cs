using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAltarBehaviour : MonoBehaviour, IDamagable
{
    [field: SerializeField]
    private HoverOverInterfaceGiver HoverOverChecker;
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
    [field: SerializeField]
    private float EnemiesDespawnTime = 30f;
    [field: SerializeField]
    internal float Health {get;private set;}= 300;
    [field: SerializeField]
    internal float DamageTaken {get;private set;}= 0.5f;
    [field: SerializeField]
    private float AltarDeathDamagePersecond= 300;
    [field: SerializeField]
    private Transform UIParent;
    [field: SerializeField]
    private GameObject AlterTextPrefab;
    [field: SerializeField]
    private string AlterTextString;
    [field: SerializeField]
    private AudioClip clip;
    [field:SerializeField]
    private int priority = 0; 
    internal bool WaveFinished { get; private set; } = false;
    private List<GameObject> spawnedEnemies = new();
    private bool spawningActive = false; 
    private bool prologeShown = false;
    private float MainTimer = 0f;
    private float SpawningTimer = 0f;
    private InputSystem_Actions inputActions;
    private TaskManager taskManager;
    void Awake()
    {
        HoverOverChecker = GetComponent<HoverOverInterfaceGiver>();
        TaskManager potentialManager = FindAnyObjectByType<TaskManager>();
        if (potentialManager != null)
        {
            taskManager = potentialManager;
        }
        inputActions = new();
    }
    void Update()
    {
        if (HoverOverChecker.IsHoveringOver == true && Mouse.current.leftButton.isPressed && !spawningActive)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= MaxRange && !WaveFinished && !prologeShown)
            {
                if (transform.TryGetComponent(out AudioSource source))
                {
                    source.Play();
                }
                StartCoroutine(PrologeEnumerator());
            }
        }
        if (Health == 0)
        {
            if (playerTransform.TryGetComponent(out PlayerHealthBehaviour healthBehaviour))
            {
                healthBehaviour.TryDamage(AltarDeathDamagePersecond*Time.deltaTime);
            }
        }
        if (!spawningActive || Health <= 0) return;
        HoverOverChecker.LabelText = "Алтарь иследуется, остаётся: "+Mathf.Round(EnemiesSpawnDuration-MainTimer)+" секунды.";
        MainTimer += Time.deltaTime;
        SpawningTimer += Time.deltaTime;
        if (MainTimer >= EnemiesSpawnDuration)
        {
            spawningActive = false;
            WaveFinished = true;
            StartCoroutine(DespawnEnemies());
            HoverOverChecker.LabelText = "Алтарь иследован, идите в другую точку.";
            if (taskManager != null) 
                taskManager.ContinueTask(priority);
        } else
        {
            if (taskManager != null) 
                taskManager.currentTaskMessage = "Ждите изучения алтаря. Осталось "+Mathf.Round(EnemiesSpawnDuration-MainTimer)+" секунды.";;
        }
        if (SpawningTimer >= EnemiesSpawnCooldown)
        {
            SpawningTimer = 0;
            SpawnRandomEnemy();
        }
    }
    private IEnumerator PrologeEnumerator()
    {
        prologeShown = true;
        if (AlterTextPrefab != null && UIParent != null)
        {
            GameObject shownText = Instantiate(AlterTextPrefab,UIParent);
            if (AlterTextString != ""&&shownText.TryGetComponent(out TMP_Text texta))
            {
                texta.text = AlterTextString;
            }
            if (clip != null)
            {
                AudioSource source = shownText.AddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
            }
            do
            {
                yield return new WaitForSecondsRealtime(0.1f);
            } while (shownText != null);
            Destroy(shownText);
        }
        spawningActive = true;
    }
    private IEnumerator DespawnEnemies()
    {
        yield return new WaitForSeconds(EnemiesDespawnTime);
        foreach(GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
    }
    private void SpawnRandomEnemy()
    {
        if (EnemyTemplate == null || Enemies == null) return;

        EnemySO randomEnemy = Enemies[UnityEngine.Random.Range(0, Enemies.Count)];
        float RandomDirection = UnityEngine.Random.Range(0.00f, 360.00f);

        float X = Mathf.Sin(RandomDirection) * EnemiesSpawnRange;
        float Y = Mathf.Cos(RandomDirection) * EnemiesSpawnRange;

        Vector3 newposition = transform.position + new Vector3(X, 0, Y);
        newposition.y = terrain.SampleHeight(newposition);

        GameObject newenemy = Instantiate(EnemyTemplate, newposition, Quaternion.identity);
        if (newenemy.TryGetComponent<EnemyHandler>(out EnemyHandler handler))
        {
            handler.enemySO = randomEnemy;
        }
        spawnedEnemies.Add(newenemy);
    }

    public bool TryDamage(float damage)
    {
        if (Health - damage <= 0)
        {
            Health = 0;
            return false;
        }
        Health -= DamageTaken;
        return true;
    }
}
