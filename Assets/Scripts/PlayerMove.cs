using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1f, 10f)] private float moveSpeed = 3f;
    [SerializeField][Range(1f, 10f)] private float sprintSpeed = 5f;
    [SerializeField][Range(0f, 1f)] private float verticalDamping = 0.8f;
    private Transform sprite;

    private PlayerState state;
    private Rigidbody2D rb;
    private InputManager input;
    private Animator anim;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<InputManager>();

        sprite = transform.GetChild(0);
    }

    private void Start()
    {
        input.OnMove += ToggleMove;
    }

    private void FixedUpdate()
    {
        if (input == null) return;

        // handle sprinting
        float currentSpeed = input.IsSprinting ? sprintSpeed : moveSpeed;

        Vector2 moveInput = input.Move;
        moveInput = new Vector2(moveInput.x, moveInput.y * verticalDamping);

        Vector2 moveDir = moveInput.normalized;

        rb.linearVelocity = moveDir * currentSpeed;

        state.PrevDir = moveDir;

        float animVal = moveInput.magnitude < 0.1f ? 0f : (input.IsSprinting ? 1f : 0.5f);
        anim.SetFloat("move", animVal);

        if (moveInput.x != 0f) sprite.localScale = new Vector3(moveInput.x < 0f ? -1 : 1, 1, 1);
    }

    private void ToggleMove(bool isPressing)
    {
        //
    }
}
