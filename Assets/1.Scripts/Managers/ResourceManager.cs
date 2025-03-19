using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : Singleton<ResourceManager>
{
    //current loaded assets: [ type / file name / object ]
    public Dictionary<eAddressableType, Dictionary<string, object>> AssetPools = new();

    //for loading asset efficiently: [ type / file name / file info ]
    private readonly Dictionary<eAddressableType, Dictionary<string, AddressableMap>> addressableMaps = new();

    #region Init Addressable Assets
    //Initialize the Addressable assets 
    public async Task Init()
    {
        //Initialize Addressable
        await Addressables.InitializeAsync().Task;

        //Init Addressable Map
        await InitAddressableMap();

        //Check Update & Download Server Assets.
        //await DownloadAssetBundles();
    }

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
    #endregion

    #region Ingame Asset Loading
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

    /// <summary>
    /// Load a single asset
    /// </summary>
    /// <typeparam name="T">class name</typeparam>
    /// <param name="key">file name</param>
    /// <param name="addressableType">addressable group name</param>
    /// <returns>T object</returns>
    public async Task<T> LoadAsset<T>(string key, eAddressableType addressableType)
    {
        var path = GetAssetPath(key, addressableType);
        return await LoadAssetAsync<T>(path);
    }

    /// <summary>
    /// Load multiple assets 
    /// </summary>
    /// <typeparam name="T">class name</typeparam>
    /// <param name="key">item name</param>
    /// <param name="addressableType">addressable group name</param>
    /// <param name="assetType">file extension type</param>
    /// <returns>T object List</returns>
    public async Task<List<T>> LoadAssetList<T>(string key, eAddressableType addressableType, eAssetType assetType)
    {
        var paths = GetAssetPaths(key, addressableType, assetType);
        List<T> objList = new List<T>();
        foreach (var path in paths)
        {
            objList.Add(await Addressables.LoadAssetAsync<T>(path).Task);
        }
        return objList;
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