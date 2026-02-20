using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Level Manager")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private LevelDataConfig allLevels;

    [SerializeField] private UIManager uiManager;

    public List<GameObject> currentLevelPrefab = new List<GameObject>();
    public List<GameObject> LevelButton = new List<GameObject>();

    private bool isSpawn;

    int Level;

    public void SpawnLevelButton()
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
                Level = index;
                SpawnLevel();
            });
        }
    }

    public void SpawnLevel()
    {
        uiManager.ScreenHandle(false, false, true);
        uiManager.SetUpDialog(false, false, false, true);
        isSpawn = false;
        CleanupButtons();
        gameManager.ResetGameState();
        StartCoroutine(LevelSpawner());
    }

    public void CleanupButtons()
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

        if (Level >= 0 && Level < allLevels.levels.Length)
        {

            if (allLevels != null && allLevels.levels != null)
            {
                foreach (var obj in allLevels.levels[Level].objects)
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
                LevelText.text = "Level " + allLevels.levels[Level].levelNumber;
        }

        isSpawn = false;
        gameManager.check = true;
    }

    public void DestroyPreviousObjects()
    {
        for (int i = currentLevelPrefab.Count - 1; i >= 0; i--)
        {
            if (currentLevelPrefab[i] != null)
                Destroy(currentLevelPrefab[i]);
        }
        currentLevelPrefab.Clear();

        gameManager.CleanupWaterDrops();
    }

    public void ShowGameResult()
    {
        if (!gameManager.isGameOver)
        {
            if (Level >= 0 && Level < allLevels.levels.Length)
            {
                allLevels.levels[Level].levelComplete = true;
                    
                if (Level + 1 < allLevels.levels.Length)
                    allLevels.levels[Level + 1].levelUnLock = true;
            }
            uiManager.SetUpDialog(false, true, false, false);
        }
        else
        {
            uiManager.SetUpDialog(false, false, true, false);
        }
    }

    public void SpawnNextLevel()
    {
        Level++;
        if (Level < allLevels.levels.Length)
        {
            Time.timeScale = 1f;
            gameManager.isPaused = false;
            SpawnLevel();
        }
        else
        {
            gameManager.HomeButton();
        }
    }
}
