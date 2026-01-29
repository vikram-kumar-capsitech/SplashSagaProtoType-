using UnityEngine;
using UnityEngine.SceneManagement;

public class FindGameManager : MonoBehaviour
{ 
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void startButton()
    {
        SceneManager.LoadScene("GameScene");
        gameManager.SpawnLevels();
    }
}
