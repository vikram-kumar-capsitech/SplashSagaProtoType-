using UnityEngine;

public class ColliderLogic : MonoBehaviour
{
    private Collider2D collide;

    void Start()
    {
        collide = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collide != null) 
            {
                collide.isTrigger = false;
            }
        }

        if (collision.gameObject.CompareTag("Tile"))
        {
            Collider2D hit = collision.gameObject.GetComponent<Collider2D>();

            if (hit != null)
            {
                if(hit.isTrigger == false)
                {
                    collide.isTrigger = false;
                }
            }
        }
    }
}
