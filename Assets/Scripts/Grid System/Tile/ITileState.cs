// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ITileState.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 22:08
// > Purpose  : Interface para a troca de estados de um tile.
// ════════════════════════════════════════════════════

using UnityEngine;

namespace GRID.TILE
{
    public interface ITileState
    {
        void Enter(Tile tile);
        void Exit(Tile tile);
        void Update(Tile tile);
        bool CanPlace();
    }
}