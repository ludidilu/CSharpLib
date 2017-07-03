using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckAssetUsage {

    [MenuItem("CheckAssetUsage/Do")]
    public static void Start()
    {
        List<AssetBundle> assetbundles = new List<AssetBundle>();

        Object obj = Selection.activeObject;

        string objPath = AssetDatabase.GetAssetPath(obj);

        Debug.Log("path:" + objPath);

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetBundles/", BuildAssetBundleOptions.DryRunBuild, BuildTarget.StandaloneWindows64);

        string[] abs = manifest.GetAllAssetBundles();

        Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < abs.Length; i++)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + abs[i]);

            assetbundles.Add(ab);

            string[] assets = ab.GetAllAssetNames();

            for (int m = 0; m < assets.Length; m++)
            {
                string[] strs = AssetDatabase.GetDependencies(assets[m]);

                for (int n = 0; n < strs.Length; n++)
                {
                    if (strs[n] == objPath)
                    {
                        Debug.Log("asset:" + assets[m]);
                    }
                }
            }

            ab.Unload(true);
        }
    }
}
