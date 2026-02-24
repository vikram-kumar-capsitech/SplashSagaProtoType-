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
    [SerializeField] private AdsManager adsManager;

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
    private int interAdNum;
    bool result;

    void Start()
    {
        interAdNum = Random.Range(2, 6);
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
                    if (rb.gravityScale == 0f)
                    {
                        levelManager.Steps.Add(hit.gameObject);
                    }
                    rb.gravityScale = 1f;

                SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = Color.white;
            }
        }

        if (isGameOver && !result)
        {
            levelManager.ShowGameResult();
            result = true;
        }

    }

    void CheckObjectStatus()
    {
        if (!check || levelManager.levelPrefab == null || levelManager.levelPrefab.Count == 0)
            return;

        bool allDone = true;

        for (int i = 0; i < levelManager.levelPrefab.Count; i++)
        {
            GameObject obj = levelManager.levelPrefab[i].currentPrefabs;
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
        adsManager.loadBanner();
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
        if (interAdNum != 0)
        {
            interAdNum--;
        }
    }

    public void NextLevel()
    {
        CleanupWaterDrops();
        levelManager.SpawnNextLevel();

        if(interAdNum == 0)
        {
            int num = Random.Range(2,6);
            interAdNum = num;
            adsManager.loadInterstitialAd();
        }
        else
        {
            interAdNum--;
        }
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
        adsManager.destroyBanner();
        if(interAdNum != 0)
        {
            interAdNum--;
        }
    }

    public void SaveSteps()
    {

    }
    public void UndoSteps()
    {
        if (!waterStarted)
        {
            levelManager.undo();
        }
    }

    public void GameHints()
    {
        adsManager.loadRewardAd();
    }
}