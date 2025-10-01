using UnityEngine;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1f, 10f)] private float moveSpeed = 3f;
    [SerializeField][Range(1f, 10f)] private float sprintSpeed = 5f;
    [SerializeField][Range(0f, 1f)] private float verticalDampingDefault = 0.8f;
    [SerializeField][Range(0f, 1f)] private float verticalDampingRamp = 0.65f;

    private Transform sprite;

    private PlayerState state;
    private StatsManager stats;
    private Rigidbody2D rb;
    private InputManager input;
    private Animator anim;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        stats = GetComponent<StatsManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<InputManager>();

        sprite = transform.GetChild(0);
    }

    private void OnEnable()
    {
        input.OnSprint += ToggleSprint;
    }

    private void OnDisable()
    {
        input.OnSprint -= ToggleSprint;
    }

    private void FixedUpdate()
    {
        // handle sprinting
        float currentSpeed = (input.IsSprinting && !stats.sprintConsumed) ? sprintSpeed : moveSpeed;

        // vertical damping
        float verticalDamping = state.OnRamp ? verticalDampingRamp : verticalDampingDefault;

        // set movement vectors
        Vector2 moveInput = input.Move.normalized;
        Vector2 moveDir = new Vector2(moveInput.x, moveInput.y * verticalDamping);

        // apply movement
        rb.linearVelocity = moveDir * currentSpeed;
        state.PrevDir = moveDir;

        // animations
        float animVal = moveInput.magnitude < 0.1f ? 0f : (input.IsSprinting ? 1f : 0.5f);
        anim.SetFloat("move", animVal);

        // turning
        if (moveInput.x != 0f) sprite.localScale = new Vector3(moveInput.x < 0f ? -1 : 1, 1, 1);
    }

    void ToggleSprint(bool isPressing)
    {
        if (!isPressing) stats.sprintConsumed = false;
        stats.isSprinting = isPressing;
    }
}
