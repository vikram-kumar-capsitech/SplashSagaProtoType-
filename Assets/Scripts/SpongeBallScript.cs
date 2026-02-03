using UnityEngine;

public class SpongeBallScript : MonoBehaviour
{
    LevelsManager levelsManager;
    SpriteRenderer image;

    void Start()
    {
        levelsManager = GameObject.Find("Level Manager").GetComponent<LevelsManager>();
        image = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y < -10f)
        {
            levelsManager.isGameOver = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            image.color = Color.gray;
            levelsManager.isGameOver = true;
        }
    }
}
