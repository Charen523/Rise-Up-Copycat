using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : Singleton<ResourceManager>
{
    //에셋의 Mapping 정보 : [ type / path-key / file info ]
    private readonly Dictionary<eAddressableType, Dictionary<string, AddressableMap>> addressableMaps = new();

    #region Unity Life Cycles
    //Initialize the Addressable assets 
    public async Task Init()
    {
        await Addressables.InitializeAsync().Task;
        await InitAddressableMap();

        //Firebase Storage 등 원격저장소 필요함.
        //Check Update & Download Server Assets.
        //await DownloadAssetBundles();
    }
    #endregion

    #region Main Methods
    /// <summary>
    /// 파일을 <typeparamref name="T"/>로 호출
    /// </summary>
    /// <typeparam name="T">불러올 형식</typeparam>
    /// <param name="key">파일 이름</param>
    /// <param name="addressableType">addressable group</param>
    /// <returns><typeparamref name="T"/></returns>
    public async Task<T> LoadAsset<T>(string key, eAddressableType addressableType)
    {
        var path = GetAssetPath(key, addressableType);
        return await LoadAssetAsync<T>(path);
    }

    /// <summary>
    /// 주소에 key를 포함하는 파일들을 List<typeparamref name="T"/>로 호출
    /// </summary>
    /// <typeparam name="T">불러올 형식</typeparam>
    /// <param name="key">폴더/파일 이름</param>
    /// <param name="addressableType">addressable group</param>
    /// <param name="assetType">확장자 종류</param>
    /// <returns>List<typeparamref name="T"/></returns>
    public async Task<List<T>> LoadAssetList<T>(string key, eAddressableType addressableType, eAssetType assetType)
    {
        var paths = GetAssetPaths(key, addressableType, assetType);
        List<T> objList = new();
        foreach (var path in paths)
        {
            objList.Add(await Addressables.LoadAssetAsync<T>(path).Task);
        }
        return objList;
    }
    #endregion

    #region Sub Methods
    private async Task InitAddressableMap()
    {
        var handle = Addressables.LoadAssetsAsync<TextAsset>("AddressableMap", (text) =>
        {
            AddressableMapList mapList = JsonUtility.FromJson<AddressableMapList>(text.text);
            eAddressableType type = eAddressableType.Data;
            Dictionary<string, AddressableMap> mapDic = new();

            foreach (AddressableMap data in mapList.list)
            {
                type = data.addressableType;
                if (!mapDic.ContainsKey(data.key))
                    mapDic.Add(data.key, data);
            }
            if (!addressableMaps.ContainsKey(type)) addressableMaps.Add(type, mapDic);

        });
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            ICLogger.LogError("Failed to load Addressable Map.");
            return;
        }
    }

    public string GetAssetPath(string key, eAddressableType addressableType)
    {
        var map = addressableMaps[addressableType][key.ToLower()];
        return map.path;
    }

    public List<string> GetAssetPaths(string key, eAddressableType group, eAssetType assetType)
    {
        var keys = new List<string>(addressableMaps[group].Keys);
        keys.RemoveAll(obj => !obj.Contains(key));
        List<string> pathList = new List<string>();
        keys.ForEach(obj =>
        {
            if (addressableMaps[group][obj].assetType == assetType)
                pathList.Add(addressableMaps[group][obj].path);
        });
        return pathList;
    }

    private async Task<T> LoadAssetAsync<T>(string path)
    {
        if (path.Contains(".prefab") && typeof(T) != typeof(GameObject))
        {//return component
            var obj = await Addressables.LoadAssetAsync<GameObject>(path).Task;
            return obj.GetComponent<T>();
        }
        else if (path.Contains(".json"))
        {//return converted class
            var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).Task;
            return JsonUtility.FromJson<T>(textAsset.text);
        }
        else
        {//return as it is
            return await Addressables.LoadAssetAsync<T>(path).Task;
        }
    }
    #endregion
}