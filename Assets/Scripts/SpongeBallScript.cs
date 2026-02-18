using UnityEngine;

public class SpongeBallScript : MonoBehaviour
{
    GameManager GameManager;
    SpriteRenderer image;

    void Start()
    {
        GameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        image = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y < -10f)
        {
            GameManager.isGameOver = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            image.color = Color.gray;
            GameManager.isGameOver = true;
        }
    }
}
