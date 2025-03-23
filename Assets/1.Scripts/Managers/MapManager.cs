using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private List<GameObject> mapPrefabs;
    [SerializeField] private Transform mapParent;

    #region Unity Life Cycles
    public async Task Init()
    {
        mapPrefabs = await ResourceManager.Instance.LoadAssetList<GameObject>("Maps", eAddressableType.Prefab, eAssetType.prefab);
    }
    #endregion

    #region Main Methods
    public void ResetPattern()
    {
        foreach (Transform t in mapParent)
        {
            Destroy(t.gameObject);
        }
    }

    public void ShowRandomMap()
    {
        int idx = Random.Range(0, mapPrefabs.Count);
        Instantiate(mapPrefabs[idx], mapParent);
    }
    #endregion
}