using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 180f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    void Update()
    {
        // Get the input from the arrow keys.
        float horizontalInput = Input.GetAxis("Horizontal2");
        float verticalInput = Input.GetAxis("Vertical2");

        // Calculate the movement direction.
        Vector2 movementDirection = new Vector2(0, verticalInput);

        // Move the sprite forward or backward.
        transform.Translate(movementDirection * speed * Time.deltaTime);

        // Rotate the sprite left or right.
        transform.Rotate(0, 0, -horizontalInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            Debug.Log("Player 2 hit Player 1!");
        }
    }
}