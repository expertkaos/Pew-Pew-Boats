using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string horizontalInputAxis = "Horizontal";
    public string verticalInputAxis = "Vertical";

    public PlayerStats stats;
    public HealthBar healthBar;

    private Vector2 velocity = Vector2.zero;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool canControl = true;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        stats.ResetStats();
        healthBar.SetMaxHealth(stats.maxHealth);
        healthBar.SetHealth(stats.currentHealth);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis(horizontalInputAxis);
        float verticalInput = Input.GetAxis(verticalInputAxis);

        if (!canControl || Time.timeScale == 0f)
            return;

        // Rotate the boat
        transform.Rotate(0, 0, -horizontalInput * stats.rotationSpeed * Time.deltaTime);

        // Apply acceleration in the forward/backward direction
        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            Vector2 direction = transform.up * verticalInput;
            velocity += direction * stats.acceleration * Time.deltaTime;

            if (velocity.magnitude > stats.maxSpeed)
            {
                velocity = velocity.normalized * stats.maxSpeed;
            }
        }
        else
        {
            if (velocity.magnitude > 0.1f)
            {
                velocity -= velocity.normalized * stats.friction * Time.deltaTime;
            }
            else
            {
                velocity = Vector2.zero;
            }
        }

        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    public void TakeDamage(int damage)
    {
        stats.currentHealth -= damage;
        stats.currentHealth = Mathf.Max(0, stats.currentHealth);

        healthBar.SetHealth(stats.currentHealth);

        if (stats.currentHealth <= 0)
        {
            // Hide the player GameObject (disappear)
            gameObject.SetActive(false);
        }
    }

    public void ResetPlayerPosition()
    {
        gameObject.SetActive(true); // Reactivate player

        transform.position = startPosition;
        transform.rotation = startRotation;
        velocity = Vector2.zero;
    }


}
