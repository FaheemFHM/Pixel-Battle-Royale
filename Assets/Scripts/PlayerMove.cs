using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(1f, 10f)] private float moveSpeed = 3f;
    [SerializeField] [Range(0f, 1f)] private float verticalDamping = 0.8f;

    private Rigidbody2D rb;
    private InputManager input;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<InputManager>();
    }

    private void Start()
    {
        input.OnMove += ToggleMove;
    }

    private void FixedUpdate()
    {
        if (input == null) return;

        Vector2 moveInput = input.Move;
        moveInput = new Vector2(moveInput.x, moveInput.y * verticalDamping);

        Vector2 moveDir = moveInput.normalized;

        rb.linearVelocity = moveDir * moveSpeed;

        anim.SetBool("move", moveInput.magnitude > 0.1f);

        if (moveInput.x != 0f) transform.localScale = new Vector3(moveInput.x < 0f ? -1 : 1, 1, 1);
    }

    private void ToggleMove(bool isPressing)
    {
        //
    }
}
