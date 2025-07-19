// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : CellValidator.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Valida células do grid e gerencia ocupação
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;
using GRID.TILE;

namespace GRID.MODULES
{
    public class CellValidator
    {
        #region Properties and Fields
        private readonly Tilemap _tilemap;
        private readonly GridManager _gridManager;
        #endregion

        #region Constructor
        public CellValidator(Tilemap tilemap, GridManager gridManager)
        {
            _tilemap = tilemap;
            _gridManager = gridManager;
        }
        #endregion

        #region Public Methods
        /// <summary> Verifica se existe um tile na célula especificada no Tilemap. </summary>
        /// <param name="cell">A posição da célula a ser verificada.</param>
        /// <returns> Retorna true se houver um tile na célula; caso contrário, false. </returns>
        public bool HasTileAtCell(Vector3Int cell)
        {
            if (!_tilemap.HasTile(cell))
            {
                Debug.Log("Célula não demarcada no tilemap. Ignorando clique.");
                return false;
            }
            return true;
        }

        /// <summary> Verifica se a célula lógica (coluna, linha) já está ocupada por um objeto. </summary>
        /// <param name="coordinate"> Coordenada lógica da célula (coluna, linha). </param>
        /// <returns> Retorna true se a célula está ocupada; caso contrário, false. </returns>
        public bool IsCellOccupied(Vector3Int coordinate)
        {
            GameObject existingObject = _gridManager.TileObjects[coordinate.x, coordinate.y];
            if (existingObject != null)
            {
                // Verifica se é um gap (pode ter diferentes nomes ou características)
                if (IsGapObject(existingObject))
                {
                    // Remove o gap da célula
                    RemoveGapFromCell(existingObject, coordinate);
                    Debug.Log($"Gap removido da célula ({coordinate.x}, {coordinate.y}) para permitir colocação de tile.");
                    return false;
                }

                Debug.Log($"Célula ({coordinate.x}, {coordinate.y}) já ocupada por: {existingObject.name} (Tipo: {existingObject.GetType().Name})");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna o GameObject da célula especificada pela coordenada lógica (coluna, linha).
        /// </summary>
        public GameObject GetCellByCoordinate(Vector3Int coordinate)
        {
            if (coordinate.x < 0 || coordinate.x >= _gridManager.TileObjects.GetLength(0) ||
            coordinate.y < 0 || coordinate.y >= _gridManager.TileObjects.GetLength(1))
            {
                Debug.LogWarning($"Coordenada ({coordinate.x}, {coordinate.y}) fora dos limites.");
                return null;
            }
            return _gridManager.TileObjects[coordinate.x, coordinate.y];
        }
        #endregion

        #region Private Methods
        /// <summary> Verifica se um GameObject é um gap. </summary>
        private bool IsGapObject(GameObject gapObject)
        {
            // Verifica se é um gap pelos possíveis nomes ou características
            return gapObject.name == "PathGap" ||
                   gapObject.name.Contains("Gap") ||
                   (gapObject.GetComponent<SpriteRenderer>()?.color == Color.red && gapObject.GetComponent<TILE.Tile>() == null);
        }

        /// <summary> Remove um gap da célula especificada. </summary>
        private void RemoveGapFromCell(GameObject gapObject, Vector3Int coordinate)
        {
            // Troca o sprite para "Square" antes de desativar
            Sprite squareSprite = Resources.Load<Sprite>("Square");
            if (squareSprite != null)
            {
                SpriteRenderer spriteRenderer = gapObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = squareSprite;
                    spriteRenderer.color = Color.white;
                    spriteRenderer.sortingOrder = 0;
                }
            }
            gapObject.SetActive(false);
            gapObject.transform.SetParent(TilePool.Instance.pooledObjectsContainer.transform);
            _gridManager.TileObjects[coordinate.x, coordinate.y] = null;
        }
        #endregion
    }
}
