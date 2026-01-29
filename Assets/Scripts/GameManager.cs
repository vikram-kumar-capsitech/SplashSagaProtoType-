using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public GameObject Drop;
    int num = 130;

    public bool isWater;
    bool waterStarted = false;
    public bool isGameOver = false;

    PanelManager panelManager;

    public int currnetLevels = 0;
    public GameObject currentLevelPrefabs;

    public List<GameObject> levels;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))

        {

            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hit = Physics2D.OverlapCircle(mousepos, 0.1f);

            if (hit != null)
            {
                if (!hit.gameObject.CompareTag("Ground"))
                {
                    Rigidbody2D rb = hit.gameObject.GetComponent<Rigidbody2D>();
                    rb.gravityScale = 1.0f;

                    SpriteRenderer image = hit.gameObject.GetComponent<SpriteRenderer>();
                    image.color = Color.white;
                }
            }
        }
        if (isWater && !waterStarted)
        {
            waterStarted = true;
            StartCoroutine(SpawnWater());
        }

        
        if (panelManager == null && SceneManager.GetActiveScene().name == "GameScene")
        {
            panelManager = GameObject.Find("Panel Manager").GetComponent<PanelManager>();
        }
    }


    IEnumerator SpawnWater()
    {
        while (num > 0)
        {
            yield return new WaitForSeconds(0.05f);
            Vector3 offset = new Vector3 (1, 0, 0);

            Vector3 minBounds = transform.position - offset;
            Vector3 maxBounds = transform.position + offset;

            Vector3 spawnPos = new Vector3(
                Random.Range(minBounds.x, maxBounds.x), 
                Random.Range(minBounds.y, maxBounds.y), 
                Random.Range(minBounds.z, maxBounds.z)  
            );
            Instantiate(Drop, spawnPos, transform.rotation);

            num--;
        }
        if (num == 0)
        {
            yield return new WaitForSeconds(2.5f);

            if (!isGameOver)
            {
                if (panelManager.WinPanel != null)
                {
                    panelManager.WinPanel.SetActive(true);
                    panelManager.PauseButton.SetActive(false);
                }
            }
            if (isGameOver)
            {
                if (panelManager.LosePanel != null)
                {
                    panelManager.LosePanel.SetActive(true);
                    panelManager.PauseButton.SetActive(false);
                }
            }
        }
    }

    public void SpawnLevels()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (currentLevelPrefabs != null)
            {
                Destroy(currentLevelPrefabs.gameObject);
            }

            Vector3 spawnPos = new Vector3(0, 0, 0);
            Quaternion spawnRot = Quaternion.Euler(0, 0, 0);

            Instantiate(levels[currnetLevels], spawnPos, spawnRot);

            panelManager.LevelText.text = "Level - " + (currnetLevels + 1);

            currnetLevels++;
        }
        
    }

}
