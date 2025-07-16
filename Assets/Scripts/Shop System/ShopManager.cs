// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ShopManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 17:16
// > Purpose: Gerencia a interface da loja, seleção de tiles e animações da loja.
// ════════════════════════════════════════════════════

using System.Collections;
using System.Collections.Generic;
using GRID;
using GRID.TILE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SHOP
{
    /// <summary> Gerencia a interface da loja, controla animações de abrir/fechar, seleção de tiles e instanciamento de tiles. </summary>
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject _tileContainer;
        [SerializeField] private List<GameObject> _tiles;

        private TileItemData tileItemData;

        private bool _shopIsOpen = false;

        private void Start()
        {
            _shopIsOpen = false;
            SetTiles();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!_shopIsOpen)
                    OpenShop();
                else
                    CloseShop();
            }
        }

        /// <summary> Inicia a coroutine para lidar com as ações do tile após o fechamento da loja. </summary>
        private void HandleButton() => StartCoroutine(HandleTileAfterClose());

        /// <summary> Fecha a loja e, após a animação, instancia e configura o tile selecionado. </summary>
        private IEnumerator HandleTileAfterClose()
        {
            CloseShop();

            // Aguarda até que a loja esteja fechada
            while (_shopIsOpen) yield return null;

            // Obtém um objeto tile da pool e ativa
            GameObject tilePool = TilePool.Instance.GetPooledObject();
            tilePool.transform.SetParent(null);
            tilePool.SetActive(true);

            // Configura o tile com os dados selecionados
            Tile tile = tilePool.GetComponent<Tile>();
            tile.SetTileItemData(tileItemData);
            tile.HandleSelection(true);
        }

        /// <summary> Configura os tiles da loja, atribuindo cores, nomes e eventos de clique. </summary>
        private void SetTiles()
        {
            Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.gray };
            string[] colorNames = { "Vermelho", "Verde", "Azul", "Amarelo", "Magenta", "Cinza" };

            for (int i = 0; i < _tiles.Count; i++)
            {
                GameObject tile = _tiles[i];
                Image squareImage = tile.GetComponentInChildren<Image>();
                TextMeshProUGUI colorText = tile.GetComponentInChildren<TextMeshProUGUI>();

                squareImage.color = colors[i % colors.Length];
                colorText.text = colorNames[i % colorNames.Length];

                Button button = tile.GetComponentInChildren<Button>();

                TileItemData itemData = TileItemData.CreateInstance<TileItemData>();
                itemData.SetName(colorText.text);
                itemData.SetColor(squareImage.color);

                button.onClick.RemoveAllListeners();

                button.onClick.AddListener(() =>
                {
                    tileItemData = itemData;
                    HandleButton();
                });
            }
        }

        /// <summary> Abre a loja, acionando a animação correspondente. </summary>
        private void OpenShop()
        {
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            _shopIsOpen = true;
        }

        /// <summary> Fecha a loja, acionando a animação correspondente e aguardando seu término. </summary>
        private void CloseShop()
        {
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            StartCoroutine(OnCloseAnimationEnd());
        }

        /// <summary> Aguarda o término da animação de fechamento da loja. </summary>
        private IEnumerator OnCloseAnimationEnd()
        {
            float closeAnimLength = 0f;
            if (_animator.runtimeAnimatorController != null)
            {
                foreach (var clip in _animator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == "Close")
                    {
                        closeAnimLength = clip.length;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(closeAnimLength);

            _shopIsOpen = false;
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", false);
        }
    }
}