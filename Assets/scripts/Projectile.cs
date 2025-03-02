using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxDistance = 10f;
    private Vector3 startPosition;

    void Start()
    {
        if (maxDistance <= 0f)
        {
            maxDistance = 10f;
            Debug.LogWarning("Projectile maxDistance was invalid. Reset to default value of 10.");
        }
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            Debug.Log("Projectile hit Player 1!"); // Message for Player 1 hit
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("Projectile hit Player 2!"); // Message for Player 2 hit
            Destroy(gameObject);
        }
    }
}