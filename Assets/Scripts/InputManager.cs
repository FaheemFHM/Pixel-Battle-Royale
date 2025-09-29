using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerControls controls;
    public bool useKeyboardInput;

    // variables
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    // boolean flags
    public bool IsMoving { get; private set; }
    public bool IsLooking { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsSecondary { get; private set; }

    // events
    public event Action<bool> OnMove;
    public event Action<bool> OnLook;
    public event Action<bool> OnSprint;
    public event Action<bool> OnPrimary;
    public event Action<bool> OnSecondary;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        // move
        controls.Player.Move.started += ctx =>
        {
            IsMoving = true;
            OnMove?.Invoke(true);
        };

        controls.Player.Move.performed += ctx =>
        {
            Move = ctx.ReadValue<Vector2>();
        };

        controls.Player.Move.canceled += ctx =>
        {
            Move = Vector2.zero;
            IsMoving = false;
            OnMove?.Invoke(false);
        };

        // look
        controls.Player.Look.started += ctx =>
        {
            IsLooking = true;
            OnLook?.Invoke(true);
        };

        controls.Player.Look.performed += ctx =>
        {
            Look = ctx.ReadValue<Vector2>();
        };

        controls.Player.Look.canceled += ctx =>
        {
            Look = Vector2.zero;
            IsLooking = false;
            OnLook?.Invoke(false);
        };

        // sprint
        controls.Player.Sprint.started += ctx =>
        {
            IsSprinting = true;
            OnSprint?.Invoke(true);
        };

        controls.Player.Sprint.canceled += ctx =>
        {
            IsSprinting = false;
            OnSprint?.Invoke(false);
        };

        // primary
        controls.Player.Primary.started += ctx =>
        {
            IsPrimary = true;
            OnPrimary?.Invoke(true);
        };

        controls.Player.Primary.canceled += ctx =>
        {
            IsPrimary = false;
            OnPrimary?.Invoke(false);
        };

        // secondary
        controls.Player.Secondary.started += ctx =>
        {
            IsSecondary = true;
            OnSecondary?.Invoke(true);
        };

        controls.Player.Secondary.canceled += ctx =>
        {
            IsSecondary = false;
            OnSecondary?.Invoke(false);
        };
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
