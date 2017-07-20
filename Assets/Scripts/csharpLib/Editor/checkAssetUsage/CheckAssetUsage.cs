using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using assetBundleManager;
using System.IO;

public static class DepRecord
{
    public static void ToBytes(BinaryWriter _bw, Dictionary<string, Dictionary<string, bool>> _dic)
    {
        _bw.Write(_dic.Count);

        Dictionary<string, Dictionary<string, bool>>.Enumerator enumerator = _dic.GetEnumerator();

        while (enumerator.MoveNext())
        {
            string key = enumerator.Current.Key;

            Dictionary<string, bool> tmpDic = enumerator.Current.Value;

            _bw.Write(key);

            _bw.Write(tmpDic.Count);

            Dictionary<string, bool>.KeyCollection.Enumerator enumerator2 = tmpDic.Keys.GetEnumerator();

            while (enumerator2.MoveNext())
            {
                _bw.Write(enumerator2.Current);
            }
        }
    }

    public static Dictionary<string, Dictionary<string, bool>> FromBytes(BinaryReader _br)
    {
        Dictionary<string, Dictionary<string, bool>> dic = new Dictionary<string, Dictionary<string, bool>>();

        int num = _br.ReadInt32();

        for (int i = 0; i < num; i++)
        {
            string key = _br.ReadString();

            Dictionary<string, bool> tmpDic = new Dictionary<string, bool>();

            dic.Add(key, tmpDic);

            int num2 = _br.ReadInt32();

            for (int m = 0; m < num2; m++)
            {
                tmpDic.Add(_br.ReadString(), false);
            }
        }

        return dic;
    }
}

public static class CheckAssetUsage
{
    private const string DEP_DATA_NAME = "dep";

    private const string DEP_DATA_EXT = "dat";

    private static Dictionary<string, Dictionary<string, bool>> dic;

    [MenuItem("CheckAssetUsage/Check Asset Usage")]
    public static void Start()
    {
        Object obj = Selection.activeObject;

        if (obj == null)
        {
            return;
        }

        string findPath = AssetDatabase.GetAssetPath(obj);

        Debug.Log("asset:" + findPath);

        if (dic == null)
        {
            string path = EditorUtility.OpenFilePanel("title", "", DEP_DATA_EXT);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            FileInfo fi = new FileInfo(path);

            using (FileStream fs = fi.OpenRead())
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    dic = DepRecord.FromBytes(br);
                }
            }
        }

        Dictionary<string, Dictionary<string, bool>>.Enumerator enumerator = dic.GetEnumerator();

        while (enumerator.MoveNext())
        {
            string key = enumerator.Current.Key;

            Dictionary<string, bool> tmpDic = enumerator.Current.Value;

            if (tmpDic.ContainsKey(findPath))
            {
                Debug.Log("parent:" + key);
            }
        }
    }

    [MenuItem("CheckAssetUsage/Create Dep Data")]
    public static void Start2()
    {
        dic = new Dictionary<string, Dictionary<string, bool>>();

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/" + AssetBundleManager.path, BuildAssetBundleOptions.DryRunBuild, BuildTarget.StandaloneWindows64);

        string[] abs = manifest.GetAllAssetBundles();

        for (int i = 0; i < abs.Length; i++)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + AssetBundleManager.path + abs[i]);

            string[] assets = ab.GetAllAssetNames();

            for (int m = 0; m < assets.Length; m++)
            {
                if (assets[m].EndsWith(".prefab"))
                {
                    Dictionary<string, bool> tmpDic = new Dictionary<string, bool>();

                    dic.Add(assets[m], tmpDic);

                    string[] strs = AssetDatabase.GetDependencies(assets[m]);

                    for (int n = 0; n < strs.Length; n++)
                    {
                        tmpDic.Add(strs[n], false);
                    }
                }
            }

            ab.Unload(false);
        }

        string savePath = EditorUtility.SaveFilePanel("title", "", DEP_DATA_NAME, DEP_DATA_EXT);

        if (string.IsNullOrEmpty(savePath))
        {
            return;
        }

        FileInfo fi = new FileInfo(savePath);

        if (fi.Exists)
        {
            fi.Delete();
        }

        using (FileStream fs = fi.Create())
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                DepRecord.ToBytes(bw, dic);
            }
        }
    }
}
