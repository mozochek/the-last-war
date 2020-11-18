using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Переменные для дебага
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private float killZonePosY;
    [SerializeField] private Joystick playerJoystick;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int health;

    // Ссылки на компоненты
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;

    // Переменные внутреннего состояния
    private string currentState;
    private bool isFacingRight = true;
    private bool isGrounded;

    // Переменные для анимации
    private const string PlayerIdle = "Player_Idle";
    private const string PlayerWalk = "Player_Walk";


    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        ChangeAnimationState(PlayerIdle);
    }

    private void FixedUpdate()
    {
        // Получение данных о движении из джойстика
        var horizontalInput = playerJoystick.Horizontal;
        var verticalInput = playerJoystick.Vertical;
        isGrounded = IsGrounded();

        // Запускаем анимацию ходьбы
        if (horizontalInput != 0 && isGrounded)
        {
            ChangeAnimationState(PlayerWalk);
        }
        else
        {
            ChangeAnimationState(PlayerIdle);
        }

        // Придаем ускорение персонажу
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Поворачиваем спрайт
        if (isFacingRight == false && horizontalInput > 0)
        {
            Flip();
        }
        else if (isFacingRight && horizontalInput < 0)
        {
            Flip();
        }

        // Если персонаж упал в killzone
        if (transform.position.y < killZonePosY)
        {
            //TODO: сделать нормальную логику для killzone
            rb.velocity = new Vector2(0, 0);
            transform.position = new Vector2(0, 0);
        }

        // Прыжок
        if (isGrounded && verticalInput >= .5f)
        {
            Jump();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // Проверка коллизии с поверхностью
    private bool IsGrounded()
    {
        // Offset нужен, чтобы персонаж не карабкался по стенам
        var offset = new Vector3(0.1f, 0f, 0f);
        var bounds = boxCollider.bounds;
        var feetRaycastHit = Physics2D.BoxCast(bounds.center, bounds.size - offset, 0f, Vector2.down,
            bounds.extents.y, groundLayerMask);
        return feetRaycastHit.collider != null;
    }

    // Переключение анимаций
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animator.Play(newState);
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
}