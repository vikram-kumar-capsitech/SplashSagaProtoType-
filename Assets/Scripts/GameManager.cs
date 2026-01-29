using System.Collections;
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
    }

    IEnumerator SpawnWater()
    {
        while (num > 0)
        {
            yield return new WaitForSeconds(0.05f);
            Instantiate(Drop, transform.position, transform.rotation);

            num--;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
