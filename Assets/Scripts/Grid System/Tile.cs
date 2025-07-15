// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : Tile.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 21:05
// > Purpose  : Describe this script
// ════════════════════════════════════════════════════
using UnityEngine;

namespace GRID
{
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;

        [Header("Tile Colors")]
        public Color selectedColor = Color.red;
        public Color notSelectedColor = Color.white;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
        }

        // private void OnMouseDown()
        // {
        //     Debug.Log($"Clicou em: {gameObject.name}");
        //     _spriteRenderer.color = selectedColor;
        // }
    }
}