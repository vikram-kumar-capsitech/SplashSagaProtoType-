using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Water Settings")]
    public GameObject Drop;
    public int num = 130;

    public bool isWater;
    bool waterStarted;
    public bool isGameOver;

    [Header("Levels")]
    public List<GameObject> levelPrefabs;
    public List<GameObject> currentLevelPrefab = new List<GameObject>();
    public int currnetLevels = 1;

    PanelManager panelManager;
    bool isSpawn;

    Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    public bool check;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (var p in levelPrefabs)
        {
            prefabDict[p.name] = p;
        }

        isWater = false;
        waterStarted = false;
        isGameOver = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            panelManager = GameObject.Find("Panel Manager")?.GetComponent<PanelManager>();
            StopAllCoroutines();
            SpawnLevel();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapCircle(mousepos, 0.1f);

            if (hit != null && !hit.CompareTag("Ground"))
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.gravityScale = 1f;

                SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = Color.white;
            }
        }

    }
    void FixedUpdate()
    {
        if (check)
        {

            if (currentLevelPrefab == null || currentLevelPrefab.Count == 0)
                return;

            bool allDone = true;

            foreach (var obj in currentLevelPrefab)
            {
                if (obj == null) continue;

                Collider2D col = obj.GetComponent<Collider2D>();

                if (col != null && col.isTrigger)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone)
            {
                check = false;
                isWater = true;
                if (isWater && !waterStarted)
                {
                    waterStarted = true;
                    StartCoroutine(SpawnWater());
                }
            }
        }
        
    }


    IEnumerator SpawnWater()
    {
        int count = num;

        while (count > 0)
        {
            yield return new WaitForSeconds(0.05f);

            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), 0, 0);
            Instantiate(Drop, spawnPos, Quaternion.identity);

            count--;
        }

        yield return new WaitForSeconds(2f);

        if (panelManager == null) yield break;

        if (!isGameOver)
                panelManager.WinPanel.SetActive(true);
        else
                panelManager.LosePanel.SetActive(true);

        panelManager.PauseButton.SetActive(false);
    }

    public void SpawnLevel()
    {
        isWater = false;
        waterStarted = false;
        isGameOver = false;

        if (panelManager != null)
        {
            panelManager.PauseButton.SetActive(true);
            panelManager.WinPanel.SetActive(false);
            panelManager.LosePanel.SetActive(false);
        }

        StartCoroutine(LevelSpawner());
    }

    IEnumerator LevelSpawner()
    {
        if (isSpawn) yield break;
        isSpawn = true;

        yield return new WaitForSeconds(0.1f);

        if (currentLevelPrefab != null)
        {
            for (int i = 0;i< currentLevelPrefab.Count; i++)
            {
                Destroy(currentLevelPrefab[i].gameObject);
            }
            currentLevelPrefab.Clear();
        }

        GameObject[] drop = GameObject.FindGameObjectsWithTag("Water");

        if (drop != null)
        {
            for(int i = 0; i< drop.Length; i++)
            {
                Destroy(drop[i].gameObject);
            }
        }

        LevelData data = LevelsData.levels[currnetLevels - 1];

        foreach (var obj in data.objects)
        {
            if (prefabDict.ContainsKey(obj.prefab))
            {
                GameObject spawned = Instantiate(prefabDict[obj.prefab], new Vector3(obj.x, obj.y,obj.z), Quaternion.Euler(obj.rx,obj.ry,obj.rz));
                currentLevelPrefab.Add(spawned);
            }
            else
            {
                Debug.LogError("Prefab NOT FOUND: " + obj.prefab);
            }
        }

        if (panelManager != null && panelManager.LevelText != null)
            panelManager.LevelText.text = "Level " + (currnetLevels);

        isSpawn = false;
        check = true;

    }
}
