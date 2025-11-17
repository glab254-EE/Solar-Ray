using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyWaveBehaviour : MonoBehaviour
{
    [field: SerializeField]
    private HoverOverInterfaceGiver HoverOverChecker;
    [field: SerializeField]
    private GameObject ScannerGO;
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
    private Transform UIParent;
    [field: SerializeField]
    private GameObject AlterTextPrefab;
    [field: SerializeField]
    private string AlterTextString;
    [field: SerializeField]
    private AudioClip clip;
    internal bool WaveFinished { get; private set; } = false;
    private List<GameObject> spawnedEnemies = new();
    private bool spawningActive = false; 
    private bool prologeShown = false;
    private float MainTimer = 0f;
    private float SpawningTimer = 0f;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        HoverOverChecker = GetComponent<HoverOverInterfaceGiver>();
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
        if (!spawningActive) return;
        HoverOverChecker.LabelText = "Алтарь иследуется, остаётся: "+Mathf.Round(EnemiesSpawnDuration-MainTimer)+" секунды.";
        if (ScannerGO != null && !ScannerGO.activeInHierarchy)
        {
            ScannerGO.SetActive(true);
        }
        MainTimer += Time.deltaTime;
        SpawningTimer += Time.deltaTime;
        if (MainTimer >= EnemiesSpawnDuration)
        {
            spawningActive = false;
            WaveFinished = true;
            if (ScannerGO != null && !ScannerGO.activeInHierarchy)
            {
                ScannerGO.SetActive(false);
            }
            DespawnEnemies();
            HoverOverChecker.LabelText = "Алтарь иследован, идите в другую точку.";
            TaskManager potentialManager = FindAnyObjectByType<TaskManager>();
            if (potentialManager != null)
            {
                potentialManager.ContinueTask();
            }
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
                yield return new WaitForEndOfFrame();
            } while (shownText != null);
            Destroy(shownText);
        }
        spawningActive = true;
    }
    private void DespawnEnemies()
    {
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
            handler.EnemySO = randomEnemy;
        }
        spawnedEnemies.Add(newenemy);
    }
}
