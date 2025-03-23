using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


internal class AddressableUtils : Editor
{
    #region Menu Items
    [InitializeOnEnterPlayMode]
    [MenuItem("Tools/Addressable/Mapping")]
    internal static void Mapping()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        foreach (var group in settings.groups)
        {//Traverse addressable groups
            foreach (var entry in group.entries)
            {//Traverse entries(resources)
                if (!entry.AssetPath.Contains("Assets") || entry.AssetPath.Contains("addressableMap")) continue;

                /*Define path of the addressableMap.json*/
                string newPath = entry.AssetPath + "/addressableMap.json";


                /*Map the entry and convert to json file*/
                string directory = Application.dataPath + entry.AssetPath.Replace("Assets", "");
                eAddressableType type = (eAddressableType)Enum.Parse(typeof(eAddressableType), group.Name);
                AddressableMapList mapData = MapEntryData(directory, type);
                string mapJsonData = JsonUtility.ToJson(mapData);
                File.WriteAllText(newPath, mapJsonData);

                /*Import into Unity's AssetDatabase*/
                AssetDatabase.ImportAsset(newPath);
            }
        }
    }

    [MenuItem("Tools/Addressable/Addressable Build")]
    internal static void BuildAddressable()
    {
        Mapping();
        AddressableAssetSettingsDefaultObject.Settings.OverridePlayerVersion = Application.version;
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

        string versionedPath = Application.dataPath + $"/BuildData/lastBuildData_{Application.version}.txt";

        List<string> uploadList = result.FileRegistry.GetFilePaths()
            .Where(obj => obj.Contains("ServerData")) //TODO: 업로드 파일 선별 시 참고
            .ToList();
        string list = string.Join("\n", uploadList);

        File.WriteAllText(versionedPath, list);
    }

    private static AddressableMapList MapEntryData(string entryDir, eAddressableType type)
    {
        AddressableMapList mapList = new AddressableMapList();

        /*Traverse entry folders*/
        List<string> dirList = Directory.GetDirectories(entryDir).ToList();
        foreach (string dir in dirList)
        {
            var res = MapEntryData(dir, type);
            mapList.AddRange(res.list);
        }

        /*Traverse entry files*/
        List<string> files = Directory.GetFiles(entryDir)
            .Where(file => Path.GetExtension(file) != ".meta" && Path.GetExtension(file) != ".spriteatlasv2")
            .ToList();
        foreach (string file in files)
        {
            string extension = Path.GetExtension(file);
            string path = file.Replace(Application.dataPath, "Assets").Replace("\\", "/");

            /*Create new addressable map*/
            AddressableMap data = new AddressableMap();
            data.assetType = extension switch
            {
                ".png" or ".jpg" or ".jpeg" => eAssetType.sprite,
                ".json" => eAssetType.json,
                ".prefab" => eAssetType.prefab,
                ".wav" or ".mp3" or ".ogg" => eAssetType.audio,
                ".ttf" or ".otf" => eAssetType.font,
                ".anim" or ".controller" => eAssetType.animation,
                _ => eAssetType.other,
            };
            data.addressableType = type;
            data.path = path;
            data.key = Path.Combine(
                Path.GetDirectoryName(path
                .Replace("Assets/AddressableDatas/", "")
                .Replace(type.ToString() + "/", "")) 
                ?? "", Path.GetFileNameWithoutExtension(path))
                .ToLower();

            /*Add to AddressableMapList*/
            mapList.Add(data);
        }
        return mapList;
    }
    #endregion
}