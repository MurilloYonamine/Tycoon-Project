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
    /// <summary>
    /// Módulo especializado responsável pelo sistema de hover visual sobre tiles vazios no grid.
    /// Fornece feedback visual em tempo real quando o cursor está sobre células disponíveis para posicionamento.
    /// </summary>
    /// <remarks>
    /// Este módulo implementa um sistema eficiente de hover que:
    /// - Detecta movimento do mouse sobre o grid
    /// - Valida se células são elegíveis para hover (vazias mas com tiles base)
    /// - Aplica efeitos visuais temporários mantendo estado original
    /// - Gerencia transições suaves entre células
    /// - Otimiza performance evitando atualizações desnecessárias
    /// 
    /// O sistema de hover melhora significativamente a experiência do usuário ao
    /// providenciar feedback visual claro sobre onde tiles podem ser posicionados.
    /// </remarks>
    public class HoverManager
    {
        #region Fields
        /// <summary>
        /// Referência ao GridManager principal para acesso a funcionalidades centrais.
        /// </summary>
        private readonly GridManager _gridManager;
        
        /// <summary>
        /// Tilemap onde os efeitos de hover são aplicados.
        /// </summary>
        private readonly Tilemap _tilemap;
        
        /// <summary>
        /// Grid principal para conversões de coordenadas.
        /// </summary>
        private readonly Grid _grid;

        // Configurações de hover
        /// <summary>
        /// Flag indicando se efeitos de hover estão habilitados.
        /// </summary>
        private bool _enableHoverEffect;
        
        /// <summary>
        /// Cor aplicada aos tiles durante o efeito de hover.
        /// </summary>
        private Color _hoverColor;

        // Estado do hover
        /// <summary>
        /// Célula atualmente sob efeito de hover.
        /// </summary>
        private Vector3Int _currentHoveredCell = Vector3Int.zero;
        
        /// <summary>
        /// Flag indicando se alguma célula está atualmente com hover ativo.
        /// </summary>
        private bool _isHovering = false;
        
        /// <summary>
        /// Flag indicando se a cor original de um tile foi armazenada.
        /// </summary>
        private bool _hasStoredOriginalColor = false;
        
        /// <summary>
        /// Cor original do tile antes da aplicação do efeito de hover.
        /// </summary>
        private Color _storedOriginalColor = Color.white;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicializa o HoverManager com configurações personalizáveis.
        /// </summary>
        /// <param name="gridManager">Referência ao GridManager principal.</param>
        /// <param name="enableHover">Se os efeitos de hover devem estar habilitados inicialmente.</param>
        /// <param name="hoverColor">Cor para efeitos de hover (padrão é amarelo se não especificado).</param>
        /// <remarks>
        /// O construtor configura todas as dependências necessárias e define configurações padrão
        /// sensatas para o sistema de hover. A cor padrão amarela fornece bom contraste visual
        /// na maioria dos cenários.
        /// </remarks>
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
        /// <summary>
        /// Atualiza o sistema de hover. Deve ser chamado no Update do GridManager.
        /// </summary>
        /// <remarks>
        /// Este método deve ser chamado a cada frame para manter a responsividade do sistema de hover.
        /// É otimizado para executar apenas quando necessário, verificando mudanças de estado
        /// antes de processar efeitos visuais.
        /// </remarks>
        public void UpdateHover()
        {
            if (!_enableHoverEffect) return;

            HandleHoverDetection();
        }

        /// <summary>
        /// Remove todos os efeitos de hover.
        /// </summary>
        /// <remarks>
        /// Método útil para limpeza quando o sistema precisa ser reiniciado ou
        /// quando o contexto muda (ex: abertura de menus). Garante que nenhum
        /// efeito visual permanente seja deixado no grid.
        /// </remarks>
        public void ClearHoverEffects()
        {
            RemoveHoverFromPreviousCell();
            _isHovering = false;
            _currentHoveredCell = Vector3Int.zero;
        }

        /// <summary>
        /// Define a cor do efeito de hover.
        /// </summary>
        /// <param name="color">Nova cor para efeitos de hover.</param>
        /// <remarks>
        /// Permite personalização dinâmica da cor de hover, útil para diferentes
        /// contextos de jogo ou preferências do usuário.
        /// </remarks>
        public void SetHoverColor(Color color)
        {
            _hoverColor = color;
        }

        /// <summary>
        /// Ativa ou desativa o efeito de hover.
        /// </summary>
        /// <param name="enabled">True para ativar, false para desativar.</param>
        /// <remarks>
        /// Quando desabilitado, automaticamente limpa qualquer efeito de hover ativo
        /// para evitar artifacts visuais. Útil para diferentes modos de jogo onde
        /// hover pode não ser apropriado.
        /// </remarks>
        public void SetHoverEnabled(bool enabled)
        {
            if (!enabled && _isHovering)
            {
                ClearHoverEffects();
            }
            _enableHoverEffect = enabled;
        }

        /// <summary>
        /// Verifica se está fazendo hover sobre um tile.
        /// </summary>
        /// <value>True se algum tile está atualmente destacado por hover.</value>
        /// <remarks>
        /// Propriedade somente leitura que permite outros sistemas verificarem
        /// o estado do hover para coordenação de funcionalidades.
        /// </remarks>
        public bool IsHovering => _isHovering;

        /// <summary>
        /// Obtém a célula atualmente sob hover.
        /// </summary>
        /// <value>Coordenada da célula sendo destacada, ou Vector3Int.zero se nenhuma.</value>
        /// <remarks>
        /// Útil para outros sistemas que precisam coordenar com o sistema de hover,
        /// como sistemas de preview de posicionamento ou tooltips contextuais.
        /// </remarks>
        public Vector3Int CurrentHoveredCell => _currentHoveredCell;

        /// <summary>
        /// Obtém a cor atual do hover.
        /// </summary>
        /// <value>Cor sendo usada para efeitos de hover.</value>
        public Color HoverColor => _hoverColor;

        /// <summary>
        /// Verifica se o hover está habilitado.
        /// </summary>
        /// <value>True se o sistema de hover está ativo.</value>
        public bool IsHoverEnabled => _enableHoverEffect;
        #endregion

        #region Private Methods
        /// <summary>
        /// Detecta hover sobre tiles vazios e aplica efeito visual.
        /// </summary>
        /// <remarks>
        /// Este método centraliza a lógica de detecção de hover, verificando mudanças
        /// de posição do mouse e aplicando/removendo efeitos conforme necessário.
        /// Otimizado para executar apenas quando há mudança de célula.
        /// </remarks>
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

        /// <summary>
        /// Verifica se a célula é válida para aplicar hover.
        /// </summary>
        /// <param name="cell">Célula a ser validada.</param>
        /// <returns>True se a célula é elegível para hover, false caso contrário.</returns>
        /// <remarks>
        /// Uma célula é válida para hover se:
        /// 1. Contém um tile base (não é completamente vazia)
        /// 2. Não está ocupada por um tile de jogo
        /// Esta validação garante que hover só apareça em locais onde tiles podem ser posicionados.
        /// </remarks>
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

        /// <summary>
        /// Aplica efeito de hover na célula atual.
        /// </summary>
        /// <remarks>
        /// Armazena a cor original do tile antes de aplicar a cor de hover.
        /// Usa TileFlags.None para permitir modificação de cor individual do tile.
        /// O armazenamento da cor original é crucial para restauração posterior.
        /// </remarks>
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

        /// <summary>
        /// Remove efeito de hover da célula anterior.
        /// </summary>
        /// <remarks>
        /// Restaura a cor original do tile que estava com hover ativo.
        /// Reseta flags de estado para evitar vazamentos de memória ou estado inconsistente.
        /// Só executa se havia um hover ativo e cor armazenada.
        /// </remarks>
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
