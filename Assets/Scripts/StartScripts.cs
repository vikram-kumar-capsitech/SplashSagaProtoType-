using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScripts : MonoBehaviour
{
    public void StartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
