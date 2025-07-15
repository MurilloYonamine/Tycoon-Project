// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : TilePool.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 00/00/0000 | 00:00
// > Purpose  : Describe this script
// ════════════════════════════════════════════════════

using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    public static TilePool Instance;

    public GameObject pooledObjectsContainer;
    public GameObject objectToPool;
    [HideInInspector] public List<GameObject> pooledObjects;
    public int amountToPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;

        GameObject tilePool = new GameObject("Tile Pool");
        tilePool.transform.SetParent(pooledObjectsContainer.transform);

        int parentCount = Mathf.CeilToInt(amountToPool / 10f);
        GameObject[] subContainers = new GameObject[parentCount];
        for (int i = 0; i < parentCount; i++)
        {
            subContainers[i] = new GameObject($"Tile Pool {i * 10}-{(i + 1) * 10 - 1}");
            subContainers[i].transform.SetParent(tilePool.transform);
        }

        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
            int parentIndex = i / 10;
            temp.transform.SetParent(subContainers[parentIndex].transform);
        }
    }
    /// <summary> Retorna um GameObject inativo da pool, ativa-o e retorna a referência. </summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }
}
