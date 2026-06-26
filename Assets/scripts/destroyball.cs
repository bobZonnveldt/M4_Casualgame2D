using UnityEngine;

public class Destroyball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Destroy(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Destroy(other.gameObject);
    }
}
