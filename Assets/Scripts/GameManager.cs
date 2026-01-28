using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public GameObject Drop;
    int num = 130;

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
        StartCoroutine(spwanWater());
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) 
        {

            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hit = Physics2D.OverlapCircle(mousepos,0.1f);

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

    }

    IEnumerator spwanWater()
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
