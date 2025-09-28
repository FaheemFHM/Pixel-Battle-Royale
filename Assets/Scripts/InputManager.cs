using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerControls controls;

    // variables
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    // boolean flags
    public bool IsMoving { get; private set; }
    public bool IsLooking { get; private set; }

    // events
    public event Action<bool> OnMove;
    public event Action<bool> OnLook;

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
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
