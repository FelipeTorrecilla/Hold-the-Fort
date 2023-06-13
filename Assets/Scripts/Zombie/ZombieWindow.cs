using UnityEngine;

public class ZombieWindow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player collision detected, block the player's movement
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
            }
        }
        else if (other.CompareTag("Zombie"))
        {
            // Zombie collision detected, allow the zombie to pass
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        }
    }
}



