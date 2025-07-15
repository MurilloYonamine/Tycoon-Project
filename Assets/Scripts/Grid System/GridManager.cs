// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : GridManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 20:59
// > Purpose  : Describe this script
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.InputSystem;

namespace GRID
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private GameObject _tilePrefab;

        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private PlayerInputActions _playerInputActions;
        private InputAction _click;
        private void Awake()
        {
            //GenerateGrid();
            _playerInputActions = new PlayerInputActions();
        }
        private void OnEnable()
        {
            _click = _playerInputActions.Player.Click;
            _click.Enable();
            _click.performed += Click;
        }
        private void OnDisable()
        {
            _click.Disable();
            _click.performed -= Click;
        }
        private Vector3 GetMouseWorldCellPosition()
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
            Vector3Int cell = Vector3Int.FloorToInt(mouseWorldPosition);
            return _grid.GetCellCenterWorld(cell);
        }

        private void Click(InputAction.CallbackContext context)
        {
            Vector3 cellPosition = GetMouseWorldCellPosition();
            Debug.Log(cellPosition);
            GameObject gameObject = Instantiate(_tilePrefab, cellPosition, Quaternion.identity);
        }
        private void GenerateGrid()
        {
            float offsetX = (_width - 1) / 2f;
            float offsetY = (_height - 1) / 2f;

            for (int x = 0; x < _width; x++)
            {
                GameObject column = new GameObject($"Column {x}");
                column.transform.SetParent(_grid.transform);

                for (int y = 0; y < _height; y++)
                {
                    Vector3 position = _grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    position.x -= offsetX * _grid.cellSize.x;
                    position.y -= offsetY * _grid.cellSize.y;

                    GameObject tileObject = Instantiate(_tilePrefab, position, Quaternion.identity, column.transform);
                    tileObject.transform.SetParent(column.transform);
                    tileObject.name = $"Tile ({x}, {y})";
                }
            }
        }

    }
}