// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : PathManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Gerencia o sistema de caminhos e detecção de gaps
// ════════════════════════════════════════════════════

using System.Collections.Generic;
using UnityEngine;
using GRID.TILE;
using System.Linq;

namespace GRID.MODULES
{
    public class PathManager
    {
        private readonly List<TILE.Tile> _pathTiles = new List<TILE.Tile>();
        private readonly GridManager _gridManager;

        /// <summary> Inicializa o PathManager com uma referência ao GridManager. </summary>
        public PathManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        #region Public Methods
        /// <summary> Adiciona um tile à lista de caminhos, reorganiza a lista e verifica por gaps. </summary>
        public void AddTileToPathList(TILE.Tile tile)
        {
            _pathTiles.Add(tile);
            OrganizePathList();
        }

        /// <summary> Retorna uma cópia da lista de tiles do caminho. </summary>
        public List<TILE.Tile> GetPathTiles() => new List<TILE.Tile>(_pathTiles);

        /// <summary> Remove todos os tiles da lista de caminhos. </summary>
        public void ClearPath() => _pathTiles.Clear();
        #endregion

        #region Private Methods
        /// <summary> Organiza a lista de tiles do caminho por coordenadas usando bubble sort. </summary>
        private void OrganizePathList()
        {
            for (int x = 0; x < _pathTiles.Count - 1; x++)
            {
                for (int y = 0; y < _pathTiles.Count - x - 1; y++)
                {
                    Vector2 coordinateA = _pathTiles[y].TileItemData.Coordinate;
                    Vector2 coordinateB = _pathTiles[y + 1].TileItemData.Coordinate;

                    if (coordinateA.x > coordinateB.x || (coordinateA.x == coordinateB.x && coordinateA.y > coordinateB.y))
                        (_pathTiles[y + 1], _pathTiles[y]) = (_pathTiles[y], _pathTiles[y + 1]);
                }
            }
        }
        #endregion
    } 
}
