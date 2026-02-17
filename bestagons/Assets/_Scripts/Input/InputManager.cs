using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static event Action<bool> OnRightClick;
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
    void OnEnable()
    {
        inputBase.Enable();
        inputBase.Player.RightClick.performed += OnRightClickAction;
        inputBase.Player.RightClick.canceled += OnRightClickAction;
    }

    void OnDisable()
    {
        inputBase.Disable();
        inputBase.Player.RightClick.performed -= OnRightClickAction;
        inputBase.Player.RightClick.canceled -= OnRightClickAction;
    }

}
