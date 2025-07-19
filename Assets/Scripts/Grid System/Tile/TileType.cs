// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : TileType.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025 | 17:50
// > Purpose  : Describe this script
// ════════════════════════════════════════════════════

using UnityEngine;

namespace GRID.TILE
{
    /// <summary>
    /// Define os diferentes tipos de tiles disponíveis no sistema de grid.
    /// </summary>
    /// <remarks>
    /// Cada tipo de tile tem comportamentos específicos:
    /// - Default: Tile básico sem funcionalidades especiais
    /// - Building: Tiles para construções e estruturas
    /// - Path: Tiles que formam caminhos e conectam outras estruturas
    /// </remarks>
    public enum TileType
    {
        /// <summary>
        /// Tipo padrão de tile sem funcionalidades especiais.
        /// </summary>
        Default,
        
        /// <summary>
        /// Tile usado para construções e estruturas do jogo.
        /// </summary>
        Building,
        
        /// <summary>
        /// Tile usado para criar caminhos e conectar diferentes áreas.
        /// Participa do sistema de pathfinding automático.
        /// </summary>
        Path,
    }
}