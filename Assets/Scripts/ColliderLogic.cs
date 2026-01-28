using UnityEngine;

public class ColliderLogic : MonoBehaviour
{
    Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Active"))
        {
            if (collider != null) 
            {
                collider.isTrigger = false;
            }
        }
    }
}
