using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Переменные для дебага
    [SerializeField] private float flySpeed;
    [SerializeField] private int damage;
    [SerializeField] private float flyRange;

    // Ссылки на компоненты
    private Rigidbody2D rb;

    // Переменные внутреннего состояния
    private Vector2 startPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Начальная точка выстрела
        startPos = rb.position;
    }

    private void FixedUpdate()
    {
        // Если улетела дальше положенного
        if (IsOutOfFlyRange())
        {
            //TODO: добавить эффект при уничтожении пули
            Destroy(gameObject);
        }

        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();

        var player = collision.GetComponent<Player>();

        // Если попали в игрока
        if (player != null)
        {
            //TODO: добавить эффект при попадании
            player.TakeDamage(damage);
        }

        // Если попали во врага
        if (enemy != null)
        {
            //TODO: добавить эффект при попадании
            enemy.TakeDamage(damage);
        }

        // Уничтожение пули про попадании во врага или объекты мира
        if (IsShouldBeDestroyed(collision))
        {
            //TODO: добавить эффект при уничтожении пули
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        rb.velocity = transform.right * flySpeed;
        rb.AddForce(rb.velocity);
    }

    private bool IsOutOfFlyRange()
    {
        return Mathf.Abs(rb.position.x) > Mathf.Abs(startPos.x) + flyRange;
    }

    private static bool IsShouldBeDestroyed(Collider2D collision)
    {
        return collision.CompareTag("Enemy") || collision.CompareTag("Ground") || collision.CompareTag("Player");
    }
}