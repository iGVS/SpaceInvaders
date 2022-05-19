using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMobile : MonoBehaviour
{
    Vector3 resp = new Vector3(0, -9, 0);
    public float speed = 5.0f;
    public Projectile laserPrefab;
    public System.Action killed;
    public bool laserActive { get; private set; }
    private Rigidbody2D rb;
    [SerializeField] private int direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector3 position = transform.position;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
    public void ChangeDir(int buttonDirection)
    {
        direction = buttonDirection;

    }

        public void FireBtn()
        {
            Shoot();
        }
    

    private void Shoot()
    {
        if (!laserActive)
        {
            laserActive = true;

            Projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.destroyed += OnLaserDestroyed;
        }

    }

    private void OnLaserDestroyed(Projectile laser)
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy_Bullet") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.lives--;
            gameObject.transform.position = resp;
        }
    }

}