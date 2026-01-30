using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject LosePanel;
    public GameObject WinPanel;

    public GameObject PauseButton;
    public TextMeshProUGUI LevelText;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        gameManager.currnetLevels++;
        Time.timeScale = 1f;
        gameManager.SpawnLevel();
    }
}
