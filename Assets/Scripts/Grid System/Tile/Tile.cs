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
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileState _currentState = TileState.Unplaced;
        [SerializeField] private TileItemData _tileItemData = null;

        private SpriteRenderer _spriteRenderer;

        private Grid _grid => GridManager.Instance.Grid;
        private ClickHandler _clickHandler => GridManager.Instance.ClickHandler;

        private bool _isGhostModeActive = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_isGhostModeActive)
            {
                FollowMouse();
                ApplyGhostOpacity();
            }
        }

        private void OnEnable() => _clickHandler.OnClickPerformed += HandleClick;
        private void OnDisable() => _clickHandler.OnClickPerformed -= HandleClick;

        /// <summary> Manipula o clique do usuário para posicionar o tile na grade, caso esteja no modo de posicionamento. </summary>
        private void HandleClick(InputAction.CallbackContext context)
        {
            if (!CanPlaceTile()) return;
            if (IsPointerOverUI()) return;

            Vector3Int cell = GetValidCell();
            if (cell == Vector3Int.zero) return;

            Vector3Int coordinate = GridManager.Instance.GetTileCoordinate(cell);
            if (GridManager.Instance.IsCellOccupied(coordinate)) return;

            PlaceTile(cell, coordinate);
        }
        /// <summary> Verifica se o tile pode ser colocado na grade, ou seja, se ainda não foi posicionado. </summary>
        /// <returns> Retorna true se o tile ainda não estiver colocado. </returns>
        private bool CanPlaceTile() => _currentState != TileState.Placed;

        /// <summary> Verifica se o ponteiro do mouse está sobre um elemento da interface de usuário (UI). </summary>
        /// <returns> Retorna true se o ponteiro estiver sobre UI; caso contrário, false. </returns>
        private bool IsPointerOverUI() => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        /// <summary> Obtém a célula válida do grid sob o ponteiro do mouse, se houver uma tile marcada no tilemap. </summary>
        /// <returns> A célula correspondente no grid, ou Vector3Int.zero se não for válida. </returns>
        private Vector3Int GetValidCell()
        {
            Vector3 worldPosition = GridManager.Instance.GetMouseWorldPosition();
            Vector3Int cell = _grid.WorldToCell(worldPosition);
            if (!GridManager.Instance.HasTileAtCell(cell))
                return Vector3Int.zero;
            return cell;
        }

        /// <summary> Posiciona o tile na célula e coordenada lógica informada, atualizando o estado visual e estrutural do tile. </summary>
        private void PlaceTile(Vector3Int cell, Vector3Int coordinate)
        {
            Vector3 cellCenter = _grid.GetCellCenterWorld(cell);
            transform.position = cellCenter;
            gameObject.name = $"Tile: ({coordinate.x}, {coordinate.y})";

            GameObject columnObject = GridManager.Instance.GetColumnObject(coordinate.x);
            transform.SetParent(columnObject.transform);

            GridManager.Instance.TileObjects[coordinate.x, coordinate.y] = gameObject;

            Debug.Log($"Coordenada: ({coordinate.x}, {coordinate.y})");

            HandleSelection(false);
            ResetOpacity();
            _currentState = TileState.Placed;
        }

        /// <summary> Ativa ou desativa o modo fantasma (ghost mode) do tile, alterando sua opacidade. </summary>
        public void HandleSelection(bool isGhostModeActive)
        {
            this._isGhostModeActive = isGhostModeActive;

            if (!isGhostModeActive)
                ResetOpacity();
        }

        /// <summary> Define os dados do tile e atualiza sua cor visual. </summary>
        public void SetTileItemData(TileItemData tileItemData)
        {
            this._tileItemData = tileItemData;
            _spriteRenderer.color = tileItemData._color;
        }

        /// <summary> Faz o tile seguir a posição do mouse na tela. </summary>
        private void FollowMouse() => transform.position = GridManager.Instance.GetMouseWorldPosition();

        /// <summary> Aplica opacidade reduzida ao tile para indicar o modo fantasma. </summary>
        private void ApplyGhostOpacity()
        {
            Color color = _spriteRenderer.color;
            color.a = 0.5f;
            _spriteRenderer.color = color;
        }

        /// <summary> Restaura a opacidade total do tile. </summary>
        public void ResetOpacity()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }
    }
}
