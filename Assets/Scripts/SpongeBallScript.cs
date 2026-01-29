using UnityEngine;

public class SpongeBallScript : MonoBehaviour
{
    GameManager gameManager;
    SpriteRenderer image;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        image = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y < -10f)
        {
            gameManager.isGameOver = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            image.color = Color.gray;
            gameManager.isGameOver = true;
        }
    }
}
