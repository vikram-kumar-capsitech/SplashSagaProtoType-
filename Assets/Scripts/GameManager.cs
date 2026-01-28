using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
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
}
