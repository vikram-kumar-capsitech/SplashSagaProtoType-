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
        if (collision.gameObject.CompareTag("Active"))
        {
            if (collide != null) 
            {
                collide.isTrigger = false;
            }
        }
    }
}
