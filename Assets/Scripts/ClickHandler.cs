// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ClickHandler.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 03:36
// > Purpose  : Lida com o click do mouse.
// ════════════════════════════════════════════════════

using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gerencia os eventos de clique do mouse usando o Unity Input System.
/// Atua como intermediário entre o sistema de input e outros componentes que precisam detectar cliques.
/// </summary>
/// <remarks>
/// Este componente:
/// - Utiliza o Unity Input System para capturar inputs
/// - Dispara eventos quando cliques são detectados
/// - Permite que múltiplos objetos se inscrevam nos eventos de clique
/// - Gerencia automaticamente a habilitação/desabilitação dos inputs
/// </remarks>
public class ClickHandler : MonoBehaviour
{
    #region Fields
    private PlayerInputActions _playerInputActions;
    private InputAction _click;
    #endregion

    #region Events
    /// <summary>
    /// Evento disparado quando um clique é detectado.
    /// </summary>
    /// <remarks>
    /// Inscreva-se neste evento para receber notificações de cliques.
    /// O parâmetro contém informações detalhadas sobre o contexto do input.
    /// </remarks>
    public event Action<InputAction.CallbackContext> OnClickPerformed;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa o sistema de input actions.
    /// </summary>
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    /// <summary>
    /// Habilita o input action de clique e registra o handler de eventos.
    /// </summary>
    private void OnEnable()
    {
        _click = _playerInputActions.Player.Click;
        _click.Enable();
        _click.performed += HandleClick;
    }

    /// <summary>
    /// Desabilita o input action e remove o handler de eventos para evitar vazamentos de memória.
    /// </summary>
    private void OnDisable()
    {
        _click.Disable();
        _click.performed -= HandleClick;
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Manipula o evento de clique e notifica todos os assinantes.
    /// </summary>
    /// <param name="context">Contexto do input action contendo informações sobre o clique.</param>
    private void HandleClick(InputAction.CallbackContext context)
    {
        OnClickPerformed?.Invoke(context);
    }
    #endregion
}
