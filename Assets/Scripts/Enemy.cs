using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Переменные для дебага
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shootingRange;
    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayerMask;

    private bool isFacingRight = true;
    private bool isGrounded;
    private int timer = 0;
    private int timerMax = 50;
    private int action = 0;

    private int reload = 0;
    private int reloadMax = 5;

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        timer++;

        if (timer >= timerMax)
        {
            timer = 0;
            action++;
        }

        var moveX = 0f;

        switch (action)
        {
            case 0:
                moveX = 2f;
                break;
            case 1:
                moveX = 0f;
                break;
            case 2:
                moveX = -2f;
                break;
            case 3:
                moveX = 0f;
                break;
            default:
                action = 0;
                break;
        }

        // Поворачиваем спрайт
        if (isGrounded)
        {
            if (isFacingRight == false && moveX > 0)
            {
                Flip();
            }
            else if (isFacingRight && moveX < 0)
            {
                Flip();
            }
        }
        
        // Придаем ускорение персонажу
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        //Shooting

        if (reload > 0)
        {
            reload--;
        }

        TryShoot();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            //TODO: добавить эффект при убийстве
            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void TryShoot()
    {
        var hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, shootingRange, playerLayerMask);

        // Если игрок в радиусе атаки
        if (hitInfo.collider != null && hitInfo.transform.CompareTag("Player"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (audioSource == null || shotSound == null || reload > 0) return;

        audioSource.PlayOneShot(shotSound);
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        reload = reloadMax;
    }

    private bool IsGrounded()
    {
        // Offset нужен, чтобы персонаж не карабкался по стенам
        var offset = new Vector3(0.1f, 0f, 0f);
        var bounds = boxCollider.bounds;
        var feetRaycastHit = Physics2D.BoxCast(bounds.center, bounds.size - offset, 0f, Vector2.down,
            bounds.extents.y, groundLayerMask);
        return feetRaycastHit.collider != null;
    }
}