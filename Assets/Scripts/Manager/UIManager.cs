using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject levelSelectionScreen;
    [SerializeField] private GameObject gamePlayScreen;

    [Header("Dialogs")]
    [SerializeField] private GameObject pauseDialog;
    [SerializeField] private GameObject winDialog;
    [SerializeField] private GameObject loseDialog;

    [Header("Button")]
    [SerializeField] private GameObject PauseButton;

    private void Start()
    {
        ScreenHandle(true, false, false);
    }

    public void ScreenHandle(bool isHome, bool isLevelScreen, bool isGamePlay)
    {
        homeScreen.SetActive(isHome);
        levelSelectionScreen.SetActive(isLevelScreen);
        gamePlayScreen.SetActive(isGamePlay);
    }

    public void SetUpDialog(bool pause, bool win, bool lose, bool pauseBtn)
    {
        pauseDialog.SetActive(pause);
        winDialog.SetActive(win);
        loseDialog.SetActive(lose);
        PauseButton.SetActive(pauseBtn);
    }
}
