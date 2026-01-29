using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject LosePanel;
    public GameObject WinPanel;

    public GameObject PauseButton;

    public void pauseButton()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void closeButton()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
