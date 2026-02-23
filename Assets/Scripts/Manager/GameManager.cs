using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("WaterDrops")]
    [SerializeField] private GameObject Drop;
    [SerializeField] private GameObject WaterDrop;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;

    private bool isWater;
    private bool waterStarted;
    public bool isPaused;
    private int dropNum = 130;
    private int remainingWaterDrops;
    private Coroutine waterCoroutine;
    public bool check;
    private Camera mainCamera;

    private List<GameObject> activeWaterDrops = new List<GameObject>();

    public bool isGameOver;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        HandleMouseInput();
        CheckObjectStatus();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0f && !waterStarted)
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

        if (isGameOver)
        {
            levelManager.ShowGameResult();
        }

    }

    void CheckObjectStatus()
    {
        if (!check || levelManager.currentLevelPrefab == null || levelManager.currentLevelPrefab.Count == 0)
            return;

        bool allDone = true;

        for (int i = 0; i < levelManager.currentLevelPrefab.Count; i++)
        {
            GameObject obj = levelManager.currentLevelPrefab[i];
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
                remainingWaterDrops = dropNum;
                waterCoroutine = StartCoroutine(SpawnWater());
            }
        }
    }

    public void ResetGameState()
    {
        isWater = false;
        waterStarted = false;
        isGameOver = false;
        isPaused = false;
        remainingWaterDrops = dropNum;
        check = false;
    }

    public IEnumerator SpawnWater()
    {
        if (Drop == null) yield break;

        while (remainingWaterDrops > 0)
        {
            if (!isWater || isPaused)
                yield break;

            yield return new WaitForSecondsRealtime(0.05f);

            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), 8f, 0);
            GameObject drop = Instantiate(Drop, spawnPos, Quaternion.identity,WaterDrop.transform);
            activeWaterDrops.Add(drop);

            remainingWaterDrops--;
        }
        if (!isGameOver)
        {
            yield return new WaitForSecondsRealtime(2f);
            levelManager.ShowGameResult();
        }
    }

    public void CleanupWaterDrops()
    {
        for (int i = activeWaterDrops.Count - 1; i >= 0; i--)
        {
            if (activeWaterDrops[i] != null)
                Destroy(activeWaterDrops[i]);
        }
        activeWaterDrops.Clear();
    }

    public void StartButton()
    {
        uiManager.ScreenHandle(false, true, false);
        uiManager.SetUpDialog(false, false, false, true);
        ResetGameState();
        levelManager.SpawnLevelButton();
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

    public void RestartButton()
    {
        waterCoroutine = null;
        Time.timeScale = 1f;
        ResetGameState();
        levelManager.SpawnLevel();
    }

    public void NextLevel()
    {
        CleanupWaterDrops();
        levelManager.SpawnNextLevel();
    }

    public void HomeButton()
    {
        StopAllCoroutines();
        ResetGameState();
        levelManager.CleanupButtons();
        levelManager.DestroyPreviousObjects();
        CleanupWaterDrops();
        Time.timeScale = 1f;
        uiManager.ScreenHandle(true, false, false);
    }

    public void SaveSteps()
    {

    }
    public void UndoSteps()
    {

    }

    public void GameHints()
    {

    }
}