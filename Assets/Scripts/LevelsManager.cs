using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    [Header("Game Bool Value")]
    public bool isWater;
    public bool waterStarted;
    public bool isGameOver;
    public bool isSpawn;


    [Header("UI GameObject")]
    public GameObject PausePanel;
    public GameObject LosePanel;
    public GameObject WinPanel;
    public GameObject PauseButton;
    public TextMeshProUGUI LevelText;
    public Sprite LockIcon ;

    [Header("Grid Level Buttons")]
    public Grid gridParent;
    public GameObject levelButtonPrefab;
    public int columns = 5;

    [Header("Levels Prefabs & Spawn")]
    private GameManager gameManager;
    public GameObject Drop;
    public int num = 130;

    public List<GameObject> levelPrefabs;
    public List<GameObject> currentLevelPrefab = new List<GameObject>();
    public List<GameObject> Button = new List<GameObject>();
    public int currnetLevels = 0;

    Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    public bool check;
    LevelData data;

    void Start()
    {
        foreach (var p in levelPrefabs)
        {
            prefabDict[p.name] = p;
        }
        isWater = false;
        waterStarted = false;
        isGameOver = false;

        SpawnLevelButton();

        LevelText.text = "Levels";
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0f)
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

    void SpawnLevelButton()
    {
        int totalLevels = LevelsData.levels.Length;
        int columns = 5;
        int rows = Mathf.CeilToInt((float)totalLevels / columns);

        int index = 1;

        int half = columns / 2;

        for (int row = -3; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index > totalLevels)
                    return;

                int centeredCol = col - half;

                Vector3Int cellPos = new Vector3Int(centeredCol, -row, 0);
                Vector3 worldPos = gridParent.CellToWorld(cellPos);

                GameObject button = Instantiate(levelButtonPrefab, worldPos, Quaternion.identity, gridParent.transform);

                Button.Add(button);

                LevelData level = LevelsData.levels[index - 1];

                if (level.levelUnLock == true)
                {
                    button.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        currnetLevels = int.Parse(
                            button.GetComponentInChildren<TextMeshProUGUI>().text
                        );
                        SpawnLevel();
                    });
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "" + index;
                }
                else
                {
                    button.GetComponent<Image>().sprite = LockIcon;
                    button.GetComponent<Button>().interactable = false;
                }

                index++;
            }
        }

    }

    IEnumerator SpawnWater()
    {
        int count = num;

        while (count > 0)
        {
            if (!isWater)
                yield break;

            yield return new WaitForSecondsRealtime(0.05f);

            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), 8f, 0);
            Instantiate(Drop, spawnPos, Quaternion.identity);

            count--;
        }

        yield return new WaitForSecondsRealtime(2f);

        if (isWater)
        {
            if (!isGameOver)
            {
                LevelData Nextdata = LevelsData.levels[currnetLevels];
                if (Nextdata != null) 
                {
                    Nextdata.levelUnLock = true;
                    data.levelComplete = true;
                }
                WinPanel.SetActive(true);
            }
                
            else
                LosePanel.SetActive(true);

            PauseButton.SetActive(false);
        }
    }

    public void SpawnLevel()
    {
        StopAllCoroutines();

        if (Button != null)
        {
            for (int i = 0; i < Button.Count; i++) 
            {
                Destroy(Button[i]);
            }
        }

        isWater = false;
        waterStarted = false;
        isGameOver = false;
        check = false;
        isSpawn = false;

        PauseButton.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);

        StartCoroutine(LevelSpawner());
    }

    IEnumerator LevelSpawner()
    {
        if (isSpawn) yield break;
        isSpawn = true;

        yield return new WaitForSeconds(0.1f);

        DestroyPreviosObject();

        data = LevelsData.levels[currnetLevels - 1];

        foreach (var obj in data.objects)
        {
            if (prefabDict.ContainsKey(obj.prefab))
            {
                GameObject spawned = Instantiate(prefabDict[obj.prefab], new Vector3(obj.x, obj.y, obj.z), Quaternion.Euler(obj.rx, obj.ry, obj.rz));
                currentLevelPrefab.Add(spawned);
            }
            else
            {
                Debug.LogError("Prefab NOT FOUND: " + obj.prefab);
            }
        }

        if (LevelText != null)
            LevelText.text = "Level " + (currnetLevels);

        isSpawn = false;
        check = true;

    }

    void DestroyPreviosObject()
    {
        if (currentLevelPrefab != null)
        {
            for (int i = 0; i < currentLevelPrefab.Count; i++)
            {
                Destroy(currentLevelPrefab[i].gameObject);
            }
            currentLevelPrefab.Clear();
        }

        GameObject[] drop = GameObject.FindGameObjectsWithTag("Water");

        if (drop != null)
        {
            for (int i = 0; i < drop.Length; i++)
            {
                Destroy(drop[i].gameObject);
            }
        }
    }

    public void PauseButtonClick()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void ClosePause()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void HomeButton()
    {
        isWater = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }

    public void RestartButton()
    {
        StopAllCoroutines();

        Time.timeScale = 1f;

        PausePanel.SetActive(false);
        PauseButton.SetActive(true);

        SpawnLevel();
    }


    public void NextLevel()
    {
        currnetLevels++;
        if (currnetLevels <= LevelsData.levels.Length)
        {
            Time.timeScale = 1f;
            SpawnLevel();
        }
        else
        {
            HomeButton();
        }
    }
}
