using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI GameObject & WaterDrops")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private Sprite LockIcon;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject levelSelectionScreen;
    [SerializeField] private GameObject GamePlayScreen;
    [SerializeField] private GameObject Drop;

    [Header("Level Manager")]
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private LevelDataSO allLevels;

    [Header("Game Bool Value")]
    private bool isWater;
    private bool waterStarted;
    private bool isSpawn;
    private bool isPaused = false;
    private int num = 130;
    private List<GameObject> currentLevelPrefab = new List<GameObject>();
    private List<GameObject> LevelButton = new List<GameObject>();
    private int currnetLevels = 0;
    private int remainingWaterDrops;
    private Coroutine waterCoroutine;
    private bool check;
    private Camera mainCamera;
    private GameObject WaterDrop;
    private List<GameObject> activeWaterDrops = new List<GameObject>();

    public bool isGameOver;

    void Start()
    {
        mainCamera = Camera.main;
        WaterDrop = GameObject.Find("WaterDrops");
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

    void SetupUI()
    {
        if (PauseButton != null)
            PauseButton.SetActive(true);
        if (WinPanel != null)
            WinPanel.SetActive(false);
        if (LosePanel != null)
            LosePanel.SetActive(false);
        if (PausePanel != null)
            PausePanel.SetActive(false);
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

            if (button == null) return;

            LevelButton.Add(button);

            LevelDataSO level = allLevels;
            TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();
            Button buttonComponent = button.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();

            GameObject completedMedal = button.transform.Find("Image").gameObject;

            if (level == null) return;

            if (level.levels[index].levelUnLock)
            {
                if (txt != null)
                    txt.text = level.levels[index].levelNumber.ToString();

                int safeIndex = index;
                buttonComponent.onClick.RemoveAllListeners();
                buttonComponent.onClick.AddListener(() =>
                {
                    currnetLevels = safeIndex;
                    SpawnLevel();
                });

                if (level.levels[index].levelComplete == true)
                {
                    completedMedal.SetActive(true);
                }
            }
            else
            {
                if (txt != null)
                    txt.text = "";

                if (LockIcon != null && buttonImage != null)
                    buttonImage.sprite = LockIcon;

                buttonComponent.interactable = false;
            }
        }
    }
    public void SpawnLevel()
    {
        levelSelectionScreen.SetActive(false);
        HomeScreen.SetActive(false);
        GamePlayScreen.SetActive(true);
        StopWaterCoroutine();
        CleanupButtons();
        ResetGameState();
        SetupUI();
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
            LevelDataSO data = allLevels;

            if (data != null && data.levels != null)
            {
                foreach (var obj in data.levels[currnetLevels].objects)
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

            if (LevelText != null && data != null)
                LevelText.text = "Level " + data.levels[currnetLevels].levelNumber;
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

        GameObject[] strayDrops = GameObject.FindGameObjectsWithTag("Water");
        foreach (GameObject drop in strayDrops)
        {
            if (drop != null)
                Destroy(drop);
        }
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

            if (WinPanel != null)
                WinPanel.SetActive(true);
        }
        else
        {
            if (LosePanel != null)
                LosePanel.SetActive(true);
        }

        if (PauseButton != null)
            PauseButton.SetActive(false);
        if (PausePanel != null)
            PausePanel.SetActive(false);
    }

    public void PauseButtonClick()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (PausePanel != null)
            PausePanel.SetActive(true);
        if (PauseButton != null)
            PauseButton.SetActive(false);
    }

    public void ClosePause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (PausePanel != null)
            PausePanel.SetActive(false);
        if (PauseButton != null)
            PauseButton.SetActive(true);

        if (isWater && waterStarted && remainingWaterDrops > 0)
        {
            if (waterCoroutine != null)
                StopCoroutine(waterCoroutine);

            waterCoroutine = StartCoroutine(SpawnWater());
        }
    }

    public void StartButton()
    {
        HomeScreen.SetActive(false);
        GamePlayScreen.SetActive(false);
        levelSelectionScreen.SetActive(true);

        SetupUI();
        ResetGameState();
        SpawnLevelButton();

        if (LevelText != null)
            LevelText.text = "Levels";
    }

    public void HomeButton()
    {
        StopAllCoroutines();
        CleanupButtons();
        ResetGameState();
        DestroyPreviousObjects();
        CleanupWaterDrops();
        Time.timeScale = 1f;

        GamePlayScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }

    public void RestartButton()
    {
        StopAllCoroutines();
        waterCoroutine = null;
        Time.timeScale = 1f;

        ResetGameState();
        SetupUI();
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