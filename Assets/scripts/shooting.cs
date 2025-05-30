using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;

    public KeyCode shootKey = KeyCode.G;

    public PlayerStats playerStats;
    public string playerTag;

    private float lastFireTime = 0f;

    void Update()
    {
        if (Input.GetKey(shootKey) && Time.time >= lastFireTime + (1f / playerStats.fireRate))
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || playerStats == null || string.IsNullOrEmpty(playerTag))
        {
            Debug.LogWarning("Missing projectile setup or player info in Shooting script!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.up * playerStats.projectileForce, ForceMode2D.Impulse);
        }

        Bullet bulletScript = projectile.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.ownerTag = playerTag;
            bulletScript.ownerStats = playerStats;
        }
    }

}
