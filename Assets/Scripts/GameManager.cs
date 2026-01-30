using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
    public List<GameObject> levels;
    public GameObject currentLevelPrefab;
    public int currnetLevels = 0;

    PanelManager panelManager;
    bool isSpawn;

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

        if (isWater && !waterStarted)
        {
            waterStarted = true;
            StartCoroutine(SpawnWater());
        }

    }
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
            return;

        if (AllCollidersAreNotTrigger())
        {
            isWater = true;
        }
    }

    bool AllCollidersAreNotTrigger()
    {
        Collider2D[] colliders = GameObject.FindObjectsByType<Collider2D>(
            FindObjectsSortMode.None
        );
        Debug.Log(colliders.Length);
        foreach (Collider2D col in colliders)
        {
            if (col.isTrigger)
            {
                return false;
            }
        }   
        return true; 
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
            Destroy(currentLevelPrefab);
            GameObject[] drops = GameObject.FindGameObjectsWithTag("Water");
            if (drops != null)
            {
                for (int i = 0; i < drops.Length; i++) 
                {
                    Destroy(drops[i].gameObject);
                }
            }

            yield return null;
        }

        if (currnetLevels >= levels.Count)
            currnetLevels = 0;

        currentLevelPrefab = Instantiate(levels[currnetLevels], Vector3.zero, Quaternion.identity);

        if (panelManager != null && panelManager.LevelText != null)
            panelManager.LevelText.text = "Level " + (currnetLevels + 1);

        isSpawn = false;
    }
}
