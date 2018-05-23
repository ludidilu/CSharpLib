using UnityEngine;
using System.Collections;
using System;

public class AssetManagerScript : MonoBehaviour
{
    public void Load<T>(string _name, AssetBundle _assetBundle, Action<T> _callBack) where T : UnityEngine.Object
    {
        StartCoroutine(LoadCorotine(_name, _assetBundle, _callBack));
    }

    private IEnumerator LoadCorotine<T>(string _name, AssetBundle _assetBundle, Action<T> _callBack) where T : UnityEngine.Object
    {
        AssetBundleRequest request = _assetBundle.LoadAssetAsync<T>(_name);

        yield return request;

        T asset = request.asset as T;

        _callBack(asset);
    }

    public void Load<T>(string _name, AssetBundle _assetBundle, Action<T[]> _callBack) where T : UnityEngine.Object
    {
        StartCoroutine(LoadCorotine(_name, _assetBundle, _callBack));
    }

    private IEnumerator LoadCorotine<T>(string _name, AssetBundle _assetBundle, Action<T[]> _callBack) where T : UnityEngine.Object
    {
        AssetBundleRequest request = _assetBundle.LoadAssetWithSubAssetsAsync<T>(_name);

        yield return request;

        T[] asset = new T[request.allAssets.Length];

        for (int i = 0; i < asset.Length; i++)
        {
            asset[i] = request.allAssets[i] as T;
        }

        try
        {
            _callBack(asset);
        }
        catch (Exception e)
        {
            Debug.Log("callback load asset error:" + e.ToString());
        }
    }
}
