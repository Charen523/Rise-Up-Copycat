using System;
using System.Collections.Generic;

/// <summary>addressable group name</summary>
public enum eAddressableType
{
    Default,
    Asset,
    Data,
    Prefab,
    UI,
}

/// <summary>type of file extension</summary>
public enum eAssetType
{
    other,
    sprite,
    json,
    prefab,
    audio,
    font,
    animation
}

[Serializable]
public class AddressableMapList
{
    public List<AddressableMap> list = new List<AddressableMap>();

    public void AddRange(List<AddressableMap> list)
    {
        this.list.AddRange(list);
    }

    public void Add(AddressableMap data)
    {
        list.Add(data);
    }
}

[Serializable]
public class AddressableMap
{
    public eAssetType assetType;
    public eAddressableType addressableType;
    public string path;
    public string key;
}