using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string ownerTag; // Set when fired (e.g., "Player1", "Player2")
    public PlayerStats ownerStats; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Ignore collisions with other bullets
            return;
        }

        if (string.IsNullOrEmpty(ownerTag))
        {
            Debug.LogWarning("Bullet has no ownerTag assigned.");
        }
        else if (other.CompareTag(ownerTag))
        {
            return; // Ignore friendly fire
        }

        PlayerController targetController = other.GetComponent<PlayerController>();
        if (targetController != null && ownerStats != null)
        {
            targetController.TakeDamage(ownerStats.damage);
        }

        Destroy(gameObject);
    }
}
