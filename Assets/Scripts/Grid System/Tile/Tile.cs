// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : Tile.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 21:05
// > Purpose  : Gerencia o comportamento, posicionamento e estado visual de um tile na grade do jogo.
// ════════════════════════════════════════════════════

using System;
using SHOP;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

namespace GRID.TILE
{
    /// <summary>
    /// Componente responsável pelo comportamento individual de cada tile no sistema de grid.
    /// Gerencia posicionamento, estado visual, interação com mouse e integração com o sistema de grade.
    /// </summary>
    /// <remarks>
    /// Esta classe representa um tile individual que pode ser posicionado na grade do jogo.
    /// Implementa funcionalidades como modo fantasma (ghost mode), validação de posicionamento,
    /// seguimento do mouse e transições de estado. Integra-se com o GridManager para coordenação
    /// global e com o sistema de input para responder a cliques do usuário.
    /// </remarks>
    public class Tile : MonoBehaviour
    {
        #region Serialized Properties
        /// <summary>
        /// Estado atual do tile no ciclo de vida do posicionamento.
        /// </summary>
        /// <value>Valor padrão é <see cref="TileState.Unplaced"/>.</value>
        [field: SerializeField] public TileState CurrentState { get; private set; } = TileState.Unplaced;
        
        /// <summary>
        /// Dados configuráveis do tile contendo informações visuais e funcionais.
        /// </summary>
        /// <value>Null por padrão, deve ser configurado durante a instanciação.</value>
        [field: SerializeField] public TileItemData TileItemData { get; private set; } = null;
        #endregion

        #region Private Fields
        /// <summary>
        /// Componente SpriteRenderer para renderização visual do tile.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Referência ao grid principal do sistema para conversões de coordenadas.
        /// </summary>
        private Grid _grid => GridManager.Instance.Grid;
        
        /// <summary>
        /// Referência ao manipulador de cliques para eventos de input.
        /// </summary>
        private ClickHandler _clickHandler => GridManager.Instance.ClickHandler;

        /// <summary>
        /// Flag indicando se o tile está em modo fantasma (seguindo o mouse).
        /// </summary>
        private bool _isGhostModeActive = false;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa os componentes necessários do tile.
        /// </summary>
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Atualiza o comportamento do tile a cada frame quando em modo fantasma.
        /// </summary>
        private void Update()
        {
            if (_isGhostModeActive)
            {
                FollowMouse();
                ApplyGhostOpacity();
            }
        }

        /// <summary>
        /// Registra listener para eventos de clique quando o tile é ativado.
        /// </summary>
        private void OnEnable() => _clickHandler.OnClickPerformed += HandleClick;
        
        /// <summary>
        /// Remove listener para eventos de clique quando o tile é desativado.
        /// </summary>
        private void OnDisable() => _clickHandler.OnClickPerformed -= HandleClick;
        #endregion

        #region Input Handling
        /// <summary>
        /// Manipula o clique do usuário para posicionar o tile na grade, caso esteja no modo de posicionamento.
        /// </summary>
        /// <param name="context">Contexto do input action contendo informações do clique.</param>
        /// <remarks>
        /// Este método valida se o tile pode ser posicionado, verifica se o clique não está sobre UI,
        /// obtém a célula válida sob o mouse e posiciona o tile se todas as condições forem atendidas.
        /// Também registra tiles do tipo Path no sistema de pathfinding.
        /// </remarks>
        private void HandleClick(InputAction.CallbackContext context)
        {
            if (!CanPlaceTile()) return;
            if (IsPointerOverUI()) return;
            if (TileItemData == null) return;

            Vector3Int? cellNullable = GetValidCell();
            if (!cellNullable.HasValue) return;

            Vector3Int cell = cellNullable.Value;

            Vector3Int coordinate = GridManager.Instance.GetTileCoordinate(cell);
            if (GridManager.Instance.IsCellOccupied(coordinate)) return;

            PlaceTile(cell, coordinate);

            switch (TileItemData.TileType)
            {
                case TileType.Path: GridManager.Instance.AddTileToPathList(this); break;
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Verifica se o tile pode ser colocado na grade, ou seja, se ainda não foi posicionado.
        /// </summary>
        /// <returns>Retorna true se o tile ainda não estiver colocado.</returns>
        /// <remarks>
        /// Esta validação previne que tiles já posicionados sejam movidos acidentalmente.
        /// </remarks>
        private bool CanPlaceTile() => CurrentState != TileState.Placed;

        /// <summary>
        /// Verifica se o ponteiro do mouse está sobre um elemento da interface de usuário (UI).
        /// </summary>
        /// <returns>Retorna true se o ponteiro estiver sobre UI; caso contrário, false.</returns>
        /// <remarks>
        /// Esta verificação previne que tiles sejam posicionados quando o usuário interage com UI,
        /// como botões ou painéis sobrepostos ao grid.
        /// </remarks>
        private bool IsPointerOverUI() => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        
        /// <summary>
        /// Obtém a célula válida do grid sob o ponteiro do mouse, se houver uma tile marcada no tilemap.
        /// </summary>
        /// <returns>A célula correspondente no grid, ou null se não for válida.</returns>
        /// <remarks>
        /// Este método converte a posição do mouse para coordenadas do grid e valida se
        /// existe um tile válido na posição para permitir o posicionamento.
        /// </remarks>
        private Vector3Int? GetValidCell()
        {
            Vector3 worldPosition = GridManager.Instance.GetMouseWorldPosition();
            Vector3Int cell = _grid.WorldToCell(worldPosition);
            if (!GridManager.Instance.HasTileAtCell(cell))
                return null;
            return cell;
        }
        #endregion

        #region Placement Methods
        /// <summary>
        /// Posiciona o tile na célula e coordenada lógica informada, atualizando o estado visual e estrutural do tile.
        /// </summary>
        /// <param name="cell">Célula física do grid onde o tile será posicionado.</param>
        /// <param name="coordinate">Coordenada lógica do tile no sistema de grid.</param>
        /// <remarks>
        /// Este método executa todo o processo de posicionamento: centraliza o tile na célula,
        /// organiza na hierarquia de objetos, registra no GridManager, atualiza coordenadas
        /// e finaliza a transição de estado para Placed.
        /// </remarks>
        private void PlaceTile(Vector3Int cell, Vector3Int coordinate)
        {
            if (TileItemData == null) return;

            Vector3 cellCenter = _grid.GetCellCenterWorld(cell);
            transform.position = cellCenter;
            gameObject.name = $"Tile: ({coordinate.x}, {coordinate.y})";

            GameObject columnObject = GridManager.Instance.GetColumnObject(coordinate.x);
            transform.SetParent(columnObject.transform);

            GridManager.Instance.TileObjects[coordinate.x, coordinate.y] = gameObject;

            //Debug.Log($"Coordenada: ({coordinate.x}, {coordinate.y})");

            TileItemData.SetCoordinate(coordinate.x, coordinate.y);

            HandleSelection(false);
            ResetOpacity();
            CurrentState = TileState.Placed;
        }
        #endregion

        #region Visual Effects Methods
        /// <summary>
        /// Ativa ou desativa o modo fantasma (ghost mode) do tile, alterando sua opacidade.
        /// </summary>
        /// <param name="isGhostModeActive">True para ativar o modo fantasma, false para desativar.</param>
        /// <remarks>
        /// O modo fantasma faz o tile seguir o cursor do mouse com opacidade reduzida,
        /// indicando visualmente que está sendo posicionado. Quando desativado,
        /// a opacidade é restaurada automaticamente.
        /// </remarks>
        public void HandleSelection(bool isGhostModeActive)
        {
            this._isGhostModeActive = isGhostModeActive;

            if (!isGhostModeActive)
                ResetOpacity();
        }

        /// <summary>
        /// Faz o tile seguir a posição do mouse na tela.
        /// </summary>
        /// <remarks>
        /// Utilizado durante o modo fantasma para providenciar feedback visual em tempo real
        /// da posição onde o tile será colocado quando o usuário clicar.
        /// </remarks>
        private void FollowMouse() => transform.position = GridManager.Instance.GetMouseWorldPosition();

        /// <summary>
        /// Aplica opacidade reduzida ao tile para indicar o modo fantasma.
        /// </summary>
        /// <remarks>
        /// A opacidade reduzida (50%) indica visualmente que o tile está em processo de posicionamento
        /// e ainda não foi confirmado na grade.
        /// </remarks>
        private void ApplyGhostOpacity()
        {
            Color color = _spriteRenderer.color;
            color.a = 0.5f;
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// Restaura a opacidade total do tile.
        /// </summary>
        /// <remarks>
        /// Chamado quando o tile sai do modo fantasma ou é definitivamente posicionado,
        /// restaurando a visibilidade completa do tile.
        /// </remarks>
        public void ResetOpacity()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }
        #endregion

        #region Configuration Methods
        /// <summary>
        /// Define os dados do tile e atualiza sua cor visual.
        /// </summary>
        /// <param name="newItemData">Novos dados do tile contendo informações visuais e funcionais.</param>
        /// <remarks>
        /// Este método deve ser chamado durante a inicialização do tile para configurar
        /// suas propriedades visuais e comportamentais baseadas no TileItemData.
        /// </remarks>
        public void SetTileItemData(TileItemData newItemData)
        {
            TileItemData = newItemData;
            _spriteRenderer.color = newItemData.Color;
        }
        #endregion
    }
}