using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static event Action<bool> OnRightClick;
    public static event Action<bool> OnLeftClick;

    private InputBase inputBase;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        inputBase = new InputBase();
        inputBase.Enable();
    }
    public Vector2 MovementVector()
    {
        return inputBase.Player.Move.ReadValue<Vector2>();
    }

    private void OnRightClickAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnRightClick?.Invoke(context.performed);
    }
    private void OnLeftClickAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnLeftClick?.Invoke(context.performed);
    }
    void OnEnable()
    {
        inputBase.Enable();
        inputBase.Player.RightClick.performed += OnRightClickAction;
        inputBase.Player.RightClick.canceled += OnRightClickAction;
        inputBase.Player.LeftClick.performed += OnLeftClickAction;
        inputBase.Player.LeftClick.canceled += OnLeftClickAction;
    }

    void OnDisable()
    {
        inputBase.Disable();
        inputBase.Player.RightClick.performed -= OnRightClickAction;
        inputBase.Player.RightClick.canceled -= OnRightClickAction;
        inputBase.Player.LeftClick.performed -= OnLeftClickAction;
        inputBase.Player.LeftClick.canceled -= OnLeftClickAction;
    }

}
