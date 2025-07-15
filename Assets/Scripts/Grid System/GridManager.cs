// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : GridManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 20:59
// > Purpose  : Describe this script
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace GRID
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private GameObject _tilePrefab;

        private GameObject[] _collumnsArray;

        private PlayerInputActions _playerInputActions;
        private InputAction _click;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            GenerateColumns();
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
            return _tilemap.GetCellCenterLocal(cell);
        }

        private void Click(InputAction.CallbackContext context)
        {
            Vector3 cellPosition = GetMouseWorldCellPosition();

            if (!_tilemap.HasTile(Vector3Int.FloorToInt(cellPosition)))
            {
                Debug.Log("Célula não demarcada no tilemap. Ignorando clique.");
                return;
            }

            GameObject tileObject = Instantiate(_tilePrefab, cellPosition, Quaternion.identity);

            BoundsInt bounds = _tilemap.cellBounds;

            int col = Vector3Int.FloorToInt(cellPosition).x - bounds.xMin; // Desloca para começar do 0
            int row = bounds.yMax - 1 - Vector3Int.FloorToInt(cellPosition).y; // Inverte o Y

            Debug.Log($"Coordenada lógica: ({col}, {row})");

            tileObject.name = $"Tile: ({row}, {col})";


            for (int i = 0; i < _collumnsArray.Length; i++)
            {
                if (_collumnsArray[i].name == $"Column: {col}")
                {
                    GameObject collumn = _collumnsArray[i];
                    tileObject.transform.SetParent(collumn.transform);
                    break;
                }
            }
        }
        private void GenerateColumns()
        {
            _collumnsArray = new GameObject[_tilemap.size.x];

            GameObject columnsObject = new GameObject("Columns");
            columnsObject.transform.SetParent(_tilemap.transform);

            for (int i = 0; i < _tilemap.size.x; i++)
            {
                GameObject columnObject = new GameObject($"Column: {i}");
                columnObject.transform.SetParent(columnsObject.transform);

                _collumnsArray[i] = columnObject;
            }
        }
    }
}