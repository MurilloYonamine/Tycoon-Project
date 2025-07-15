// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ClickHandler.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 03:36
// > Purpose  : Lida com o click do mouse.
// ════════════════════════════════════════════════════

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private InputAction _click;

    public event Action<InputAction.CallbackContext> OnClickPerformed;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _click = _playerInputActions.Player.Click;
        _click.Enable();
        _click.performed += HandleClick;
    }
    private void OnDisable()
    {
        _click.Disable();
        _click.performed -= HandleClick;
    }
    private void HandleClick(InputAction.CallbackContext context)
    {
        OnClickPerformed?.Invoke(context);
    }
}
