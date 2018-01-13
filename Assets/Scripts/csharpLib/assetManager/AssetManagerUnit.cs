using UnityEngine;
using System.Collections.Generic;
using assetBundleManager;
using System;

namespace assetManager
{
    public class AssetManagerUnit<T> : IAssetManagerUnit where T : UnityEngine.Object
    {
        private AssetManagerData data;

        private T asset;

        private int type = -1;

        private List<Action<T>> callBackList = new List<Action<T>>();
        private string name;

        public AssetManagerUnit(string _name)
        {
            name = _name;

            data = AssetManager.Instance.GetData(name);
        }

        public void Load(Action<T> _callBack)
        {
            if (type == -1)
            {
                type = 0;

                callBackList.Add(_callBack);

                StartLoad();
            }
            else if (type == 0)
            {
                callBackList.Add(_callBack);
            }
            else
            {
                _callBack(asset);
            }
        }

        private void StartLoad()
        {
            int loadNum = 2;

            AssetBundle assetBundle = null;

            Action<AssetBundle> callBack = delegate (AssetBundle _assetBundle)
            {
                assetBundle = _assetBundle;

                GetAssetBundle(ref loadNum, assetBundle);
            };

            AssetBundleManager.Instance.Load(data.assetBundle, callBack);

            if (data.assetBundleDep != null)
            {
                callBack = delegate (AssetBundle _assetBundle)
                {
                    GetAssetBundle(ref loadNum, assetBundle);
                };

                for (int i = 0; i < data.assetBundleDep.Length; i++)
                {
                    loadNum++;

                    AssetBundleManager.Instance.Load(data.assetBundleDep[i], callBack);
                }
            }

            GetAssetBundle(ref loadNum, assetBundle);
        }

        private void GetAssetBundle(ref int _loadNum, AssetBundle _assetBundle)
        {
            _loadNum--;

            if (_loadNum == 0)
            {
                if (AssetManager.LOADASYNC)
                {
                    AssetManager.Instance.script.Load<T>(name, _assetBundle, LoadOver);
                }
                else
                {
                    T asset = _assetBundle.LoadAsset<T>(name);

                    LoadOver(asset);
                }
            }
        }

        private void LoadOver(T _asset)
        {
            type = 1;

            asset = _asset;

            for (int i = 0; i < callBackList.Count; i++)
            {
                callBackList[i](_asset);
            }

            callBackList.Clear();

            AssetBundleManager.Instance.Unload(data.assetBundle);

            if (data.assetBundleDep != null)
            {
                for (int i = 0; i < data.assetBundleDep.Length; i++)
                {
                    AssetBundleManager.Instance.Unload(data.assetBundleDep[i]);
                }
            }

            AssetManager.Instance.RemoveUnit(name);
        }
    }
}