// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : HoverManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Módulo responsável pelo sistema de hover sobre tiles vazios
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GRID.MODULES
{
    public class HoverManager
    {
        #region Fields
        private readonly GridManager _gridManager;
        private readonly Tilemap _tilemap;
        private readonly Grid _grid;

        // Configurações de hover
        private bool _enableHoverEffect;
        private Color _hoverColor;

        // Estado do hover
        private Vector3Int _currentHoveredCell = Vector3Int.zero;
        private bool _isHovering = false;
        private bool _hasStoredOriginalColor = false;
        private Color _storedOriginalColor = Color.white;
        #endregion

        #region Constructor
        public HoverManager(GridManager gridManager, bool enableHover = true, Color hoverColor = default)
        {
            _gridManager = gridManager;
            _tilemap = gridManager.Tilemap;
            _grid = gridManager.Grid;
            _enableHoverEffect = enableHover;
            _hoverColor = hoverColor == default ? Color.yellow : hoverColor; // Amarelo semi-transparente
        }
        #endregion

        #region Public Methods
        /// <summary> Atualiza o sistema de hover. Deve ser chamado no Update do GridManager. </summary>
        public void UpdateHover()
        {
            if (!_enableHoverEffect) return;

            HandleHoverDetection();
        }

        /// <summary> Remove todos os efeitos de hover. </summary>
        public void ClearHoverEffects()
        {
            RemoveHoverFromPreviousCell();
            _isHovering = false;
            _currentHoveredCell = Vector3Int.zero;
        }

        /// <summary> Define a cor do efeito de hover. </summary>
        public void SetHoverColor(Color color)
        {
            _hoverColor = color;
        }

        /// <summary> Ativa ou desativa o efeito de hover. </summary>
        public void SetHoverEnabled(bool enabled)
        {
            if (!enabled && _isHovering)
            {
                ClearHoverEffects();
            }
            _enableHoverEffect = enabled;
        }

        /// <summary> Verifica se está fazendo hover sobre um tile. </summary>
        public bool IsHovering => _isHovering;

        /// <summary> Obtém a célula atualmente sob hover. </summary>
        public Vector3Int CurrentHoveredCell => _currentHoveredCell;

        /// <summary> Obtém a cor atual do hover. </summary>
        public Color HoverColor => _hoverColor;

        /// <summary> Verifica se o hover está habilitado. </summary>
        public bool IsHoverEnabled => _enableHoverEffect;
        #endregion

        #region Private Methods
        /// <summary> Detecta hover sobre tiles vazios e aplica efeito visual. </summary>
        private void HandleHoverDetection()
        {
            if (_gridManager?.CoordinateConverter == null || _tilemap == null) return;

            // Obter posição do mouse no mundo
            Vector3 mouseWorldPos = _gridManager.GetMouseWorldPosition();
            Vector3Int currentCell = _grid.WorldToCell(mouseWorldPos);

            // Se mudou de célula, processa o hover
            if (currentCell != _currentHoveredCell)
            {
                // Remove hover da célula anterior
                RemoveHoverFromPreviousCell();

                // Atualiza célula atual
                _currentHoveredCell = currentCell;

                // Verifica se a nova célula é válida para hover
                if (IsCellValidForHover(currentCell))
                {
                    ApplyHoverToCurrentCell();
                    _isHovering = true;
                }
                else
                {
                    _isHovering = false;
                }
            }
        }

        /// <summary> Verifica se a célula é válida para aplicar hover. </summary>
        private bool IsCellValidForHover(Vector3Int cell)
        {
            // Verifica se existe um tile na posição (tile vazio do palette)
            TileBase tileAtPosition = _tilemap.GetTile(cell);
            if (tileAtPosition == null) return false;

            // Converte para coordenada lógica e verifica se não está ocupada
            Vector3Int coordinate = _gridManager.GetTileCoordinate(cell);
            bool isOccupied = _gridManager.IsCellOccupied(coordinate);

            return !isOccupied;
        }

        /// <summary> Aplica efeito de hover na célula atual. </summary>
        private void ApplyHoverToCurrentCell()
        {
            if (!_hasStoredOriginalColor)
            {
                // Armazena a cor original do tile
                _storedOriginalColor = _tilemap.GetColor(_currentHoveredCell);
                _hasStoredOriginalColor = true;
            }

            // Aplica a cor de hover
            _tilemap.SetTileFlags(_currentHoveredCell, TileFlags.None);
            _tilemap.SetColor(_currentHoveredCell, _hoverColor);
        }

        /// <summary> Remove efeito de hover da célula anterior. </summary>
        private void RemoveHoverFromPreviousCell()
        {
            if (_isHovering && _hasStoredOriginalColor)
            {
                // Restaura a cor original
                _tilemap.SetTileFlags(_currentHoveredCell, TileFlags.None);
                _tilemap.SetColor(_currentHoveredCell, _storedOriginalColor);
                _hasStoredOriginalColor = false;
            }
        }
        #endregion
    }
}
