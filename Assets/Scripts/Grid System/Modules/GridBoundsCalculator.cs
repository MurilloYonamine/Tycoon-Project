// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : GridBoundsCalculator.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Calcula limites e contagens do grid
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GRID.MODULES
{
    public class GridBoundsCalculator
    {
        private readonly Tilemap _tilemap;

        public GridBoundsCalculator(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }

        #region Public Methods
        /// <summary>
        /// Calcula os limites reais do Tilemap, baseando-se apenas nas células que possuem tiles.
        /// Ignora células vazias nas bordas que ainda estão inclusas em Tilemap.cellBounds.
        /// </summary>
        public BoundsInt GetTrimmedBounds()
        {
            Vector3Int? min = null;
            Vector3Int? max = null;

            foreach (Vector3Int position in _tilemap.cellBounds.allPositionsWithin)
            {
                if (!_tilemap.HasTile(position)) continue;

                if (min == null || max == null)
                {
                    min = max = position;
                }
                else
                {
                    min = new Vector3Int(
                        Mathf.Min(min.Value.x, position.x),
                        Mathf.Min(min.Value.y, position.y),
                        0
                    );
                    max = new Vector3Int(
                        Mathf.Max(max.Value.x, position.x),
                        Mathf.Max(max.Value.y, position.y),
                        0
                    );
                }
            }

            if (min == null || max == null)
                return new BoundsInt(Vector3Int.zero, Vector3Int.zero); // Nenhum tile

            Vector3Int size = max.Value - min.Value + Vector3Int.one;
            return new BoundsInt(min.Value, size);
        }

        /// <summary> Conta quantas células dentro dos limites (_bounds) do Tilemap estão marcadas com tiles. </summary>
        public int GetAllMarkedTiles(BoundsInt bounds)
        {
            int markedTilesCount = 0;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    if (_tilemap.HasTile(new Vector3Int(x, y, 0)))
                        markedTilesCount++;
                }
            }
            return markedTilesCount;
        }
        #endregion
    }
}
