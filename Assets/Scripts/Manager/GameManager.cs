using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI GameObject & WaterDrops")]
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private GameObject Drop;
    [SerializeField] private GameObject WaterDrop;

    [Header("Level Manager")]
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private LevelDataConfig allLevels;
    [SerializeField] private UIManager uiManager;

    [Header("Game Bool Value")]
    private bool isWater;
    private bool waterStarted;
    private bool isSpawn;
    private bool isPaused;
    private int num = 130;
    private int currnetLevels = 0;
    private int remainingWaterDrops;
    private Coroutine waterCoroutine;
    private bool check;
    private Camera mainCamera;

    private List<GameObject> currentLevelPrefab = new List<GameObject>();
    private List<GameObject> LevelButton = new List<GameObject>();
    private List<GameObject> activeWaterDrops = new List<GameObject>();

    public bool isGameOver;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        HandleMouseInput();
        CheckLevelCompletion();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0f && mainCamera != null)
        {
            Vector2 mousepos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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

    void CheckLevelCompletion()
    {
        if (!check || currentLevelPrefab == null || currentLevelPrefab.Count == 0)
            return;

        bool allDone = true;

        for (int i = 0; i < currentLevelPrefab.Count; i++)
        {
            GameObject obj = currentLevelPrefab[i];
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
                remainingWaterDrops = num;
                waterCoroutine = StartCoroutine(SpawnWater());
            }
        }
    }

    void ResetGameState()
    {
        isWater = false;
        waterStarted = false;
        isGameOver = false;
        isSpawn = false;
        isPaused = false;
        remainingWaterDrops = num;
        check = false;
    }

    void SpawnLevelButton()
    {
        int totalLevels = allLevels.levels.Length;

        for (int i = 0; i < totalLevels; i++)
        {
            int index = i;

            GameObject button = Instantiate(
                levelButtonPrefab,
                gridParent.transform.position,
                Quaternion.identity,
                gridParent.transform
            );

            LevelButton.Add(button);

            button.GetComponent<LevelButton>().levelNumber = index;

            Button buttonComponent = button.GetComponent<Button>();

            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() =>
            {
                currnetLevels = index;
                SpawnLevel();
            });
        }
    }
    public void SpawnLevel()
    {
        uiManager.ScreenHandle(false, false, true);
        uiManager.SetUpDialog(false, false, false, true);
        StopWaterCoroutine();
        CleanupButtons();
        ResetGameState();
        StartCoroutine(LevelSpawner());
    }

    void StopWaterCoroutine()
    {
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
            waterCoroutine = null;
        }
    }

    void CleanupButtons()
    {
        foreach (var b in LevelButton)
        {
            if (b != null)
                b.GetComponent<Button>()?.onClick.RemoveAllListeners();
        }

        for (int i = LevelButton.Count - 1; i >= 0; i--)
        {
            if (LevelButton[i] != null)
                Destroy(LevelButton[i]);
        }
        LevelButton.Clear();
    }

    IEnumerator LevelSpawner()
    {
        if (isSpawn) yield break;

        isSpawn = true;
        yield return new WaitForSeconds(0.1f);

        DestroyPreviousObjects();

        if (currnetLevels >= 0 && currnetLevels < allLevels.levels.Length)
        {

            if (allLevels != null && allLevels.levels != null)
            {
                foreach (var obj in allLevels.levels[currnetLevels].objects)
                {
                    if (obj != null)
                    {
                        GameObject spawned = Instantiate(
                            obj.prefab,
                            obj.position,
                            Quaternion.Euler(obj.rotation)
                        );
                        currentLevelPrefab.Add(spawned);
                    }
                }
            }

            if (LevelText != null && allLevels != null)
                LevelText.text = "Level " + allLevels.levels[currnetLevels].levelNumber;
        }

        isSpawn = false;
        check = true;
    }

    void DestroyPreviousObjects()
    {
        for (int i = currentLevelPrefab.Count - 1; i >= 0; i--)
        {
            if (currentLevelPrefab[i] != null)
                Destroy(currentLevelPrefab[i]);
        }
        currentLevelPrefab.Clear();

        CleanupWaterDrops();
    }


    IEnumerator SpawnWater()
    {
        if (Drop == null) yield break;

        while (remainingWaterDrops > 0)
        {
            if (!isWater || isPaused)
                yield break;

            yield return new WaitForSecondsRealtime(0.05f);

            Vector3 spawnPos = transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 8f, 0);
            GameObject drop = Instantiate(Drop, spawnPos, Quaternion.identity,WaterDrop.transform);
            activeWaterDrops.Add(drop);

            remainingWaterDrops--;
        }

        yield return new WaitForSecondsRealtime(2f);
        ShowGameResult();
    }

    void CleanupWaterDrops()
    {
        for (int i = activeWaterDrops.Count - 1; i >= 0; i--)
        {
            if (activeWaterDrops[i] != null)
                Destroy(activeWaterDrops[i]);
        }
        activeWaterDrops.Clear();
    }

    void ShowGameResult()
    {
        CleanupWaterDrops();

        if (!isGameOver)
        {
            if (currnetLevels >= 0 && currnetLevels < allLevels.levels.Length)
            {
                allLevels.levels[currnetLevels].levelComplete = true;

                if (currnetLevels + 1 < allLevels.levels.Length)
                    allLevels.levels[currnetLevels + 1].levelUnLock = true;
            }
            uiManager.SetUpDialog(false, true, false, true);
        }
        else
        {
            uiManager.SetUpDialog(false, false, true, false);
        }
    }

    public void PauseButtonClick()
    {
        isPaused = true;
        Time.timeScale = 0f;
        uiManager.SetUpDialog(true, false, false, false);

    }

    public void ClosePause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        uiManager.SetUpDialog(false, false, false, true);

        if (isWater && waterStarted && remainingWaterDrops > 0)
        {
            if (waterCoroutine != null)
                StopCoroutine(waterCoroutine);

            waterCoroutine = StartCoroutine(SpawnWater());
        }
    }

    public void StartButton()
    {
        uiManager.ScreenHandle(false, true, false);
        uiManager.SetUpDialog(false, false, false, true);
        ResetGameState();
        SpawnLevelButton();
    }

    public void HomeButton()
    {
        StopAllCoroutines();
        CleanupButtons();
        ResetGameState();
        DestroyPreviousObjects();
        CleanupWaterDrops();
        Time.timeScale = 1f;
        uiManager.ScreenHandle(true, false, false);
    }

    public void RestartButton()
    {
        StopAllCoroutines();
        waterCoroutine = null;
        Time.timeScale = 1f;

        ResetGameState();
        uiManager.SetUpDialog(false, false, false, true);
        SpawnLevel();
    }

    public void NextLevel()
    {
        currnetLevels++;
        if (currnetLevels < allLevels.levels.Length)
        {
            Time.timeScale = 1f;
            isPaused = false;
            SpawnLevel();
        }
        else
        {
            HomeButton();
        }
    }
}