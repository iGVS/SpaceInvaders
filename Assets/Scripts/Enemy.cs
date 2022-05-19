using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Enemy[] prefabs = new Enemy[5];
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }
    public static float speed = 3.0f;
    public System.Action<Enemy> killed;
    public static int rows = 2;
    public static int columns = 3;
    public static int AmountKilled { get; private set; }
    public int Alive;

    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    private void Awake()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                Enemy enemy = Instantiate(prefabs[i], transform);
                Vector3 position = rowPosition;
                position.x += 2f * j;
                enemy.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = Alive;

        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform enemy in transform)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1f / (float)this.Alive))
            {
                Instantiate(missilePrefab, enemy.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        Alive = rows * columns - AmountKilled;
        if (Alive == 0)
        {
            AmountKilled = 0;
            if (rows < 6) rows++;
            if (columns < 12) columns++;
            if (speed < 8.0f) speed+= 1.0f;
            SceneManager.LoadScene(3);

        }

        transform.position += direction * speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform enemy in transform)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && enemy.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && enemy.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }
    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0f, 0f);

        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player_Bullet"))
        {
            Destroy(this.gameObject);
            AmountKilled++;
            GameManager.score++;
        }
    }

}

