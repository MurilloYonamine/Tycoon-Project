// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : TileStateType.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 22:06
// > Purpose  : Script de estado do Tile.
// ════════════════════════════════════════════════════

using UnityEngine;

namespace GRID.TILE
{
    /// <summary>
    /// Define os estados possíveis de um tile durante seu ciclo de vida.
    /// </summary>
    /// <remarks>
    /// O sistema de estados controla:
    /// - Comportamento visual (ghost mode, opacidade)
    /// - Interações permitidas (movimento, posicionamento)
    /// - Validações de posicionamento
    /// - Transições entre estados
    /// </remarks>
    public enum TileState
    {
        /// <summary>
        /// Estado inicial quando o tile não foi posicionado no grid.
        /// Neste estado o tile segue o cursor em modo fantasma.
        /// </summary>
        Unplaced,
        
        /// <summary>
        /// Estado final quando o tile foi posicionado com sucesso no grid.
        /// Neste estado o tile não pode mais ser movido e tem opacidade total.
        /// </summary>
        Placed
    }
}
