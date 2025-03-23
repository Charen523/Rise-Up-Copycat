using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private List<GameObject> mapPrefabs;
    [SerializeField] private Transform mapParent;
    [SerializeField] private GameObject curMap;

    #region Unity Life Cycles
    public async Task Init()
    {
        mapPrefabs = await ResourceManager.Instance.LoadAssetList<GameObject>("Maps", eAddressableType.Prefab, eAssetType.prefab);
    }
    #endregion

    #region Main Methods
    public void ShowRandomMap()
    {
        int idx = Random.Range(0, mapPrefabs.Count);
        Instantiate(mapPrefabs[idx], mapParent);
    }
    #endregion
}