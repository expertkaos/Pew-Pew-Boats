using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform player; // Assign the player's transform
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Assign offset to player

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the world position above the player
            Vector3 worldPosition = player.position + offset;

            // Convert the world position to screen space
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // Convert the screen position back to world space for the UI
            Vector3 uiWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // Set the z-coordinate to match the Canvas's plane
            uiWorldPosition.z = 0;

            // Update the health bar's position
            transform.position = uiWorldPosition;
        }
    }
}
