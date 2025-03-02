using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 180f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileMaxDistance = 15f; // Control max distance from here

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal2");
        float verticalInput = Input.GetAxis("Vertical2");

        Vector2 movementDirection = new Vector2(0, verticalInput);
        transform.Translate(movementDirection * speed * Time.deltaTime);
        transform.Rotate(0, 0, -horizontalInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Projectile projectileScript = projectileObject.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.maxDistance = projectileMaxDistance;
                projectileObject.GetComponent<Rigidbody2D>().linearVelocity = transform.up * projectileSpeed;
            }
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