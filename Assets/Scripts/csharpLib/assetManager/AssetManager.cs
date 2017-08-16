using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

#if USE_ASSETBUNDLE

using wwwManager;
using thread;

#else

using UnityEditor;

#endif

namespace assetManager
{
    public class AssetManager
    {
        public const bool LOADASYNC = true;

        public const string dataName = "ab.dat";

        private static AssetManager _Instance;

        public static AssetManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AssetManager();
                }

                return _Instance;
            }
        }

        private Dictionary<string, IAssetManagerUnit> dic;

        public AssetManagerScript script;

        private Dictionary<string, AssetManagerData> dataDic;

        public Func<string, Type, string> getAssetPathDelegate;

        public AssetManager()
        {
            dic = new Dictionary<string, IAssetManagerUnit>();

            if (LOADASYNC)
            {
                GameObject go = new GameObject("AssetManagerGameObject");

                GameObject.DontDestroyOnLoad(go);

                script = go.AddComponent<AssetManagerScript>();
            }
        }

        public void Init(Action _callBack)
        {
            Init(_callBack, dataName);
        }

        public void Init(Action _callBack, string _dataName)
        {

#if USE_ASSETBUNDLE

            Action<WWW> cb = delegate (WWW obj)
            {
                ThreadScript.Instance.Add(InitDic, obj.bytes, _callBack);
            };

            WWWManager.Instance.Load(_dataName, cb);

#else

            if (_callBack != null)
            {

                _callBack();
            }

#endif
        }

        private void InitDic(object _dic)
        {
            dataDic = AssetManagerDataFactory.GetData(_dic as Byte[]);
        }

        public AssetManagerData GetData(string _name)
        {
            string name = _name.ToLower();

            AssetManagerData data;

            if (!dataDic.TryGetValue(name, out data))
            {
                SuperDebug.LogError("AssetBundle中没有找到Asset:" + _name);
            }

            return data;
        }

        public void RemoveUnit(string _name)
        {
            dic.Remove(_name);
        }

        public void FixAssetBundleData(Dictionary<string, AssetManagerData> _addDic, BinaryWriter _bw)
        {
            Dictionary<string, AssetManagerData>.Enumerator enumerator = _addDic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                dataDic.Add(enumerator.Current.Key, enumerator.Current.Value);
            }

            AssetManagerDataFactory.SetData(_bw, dataDic);
        }

        public void GetAsset<T>(string _name, Action<T> _callBack) where T : UnityEngine.Object
        {
            string assetName;

            if (getAssetPathDelegate != null)
            {
                string tmpName = getAssetPathDelegate(_name, typeof(T));

                if (!string.IsNullOrEmpty(tmpName))
                {
                    assetName = tmpName;
                }
                else
                {
                    assetName = _name;
                }
            }
            else
            {
                assetName = _name;
            }

#if USE_ASSETBUNDLE

            IAssetManagerUnit unit;

            if (!dic.TryGetValue(assetName, out unit))
            {
                unit = new AssetManagerUnit<T>(assetName);

                dic.Add(assetName, unit);
            }

            (unit as AssetManagerUnit<T>).Load(_callBack);
#else

            T data = AssetDatabase.LoadAssetAtPath<T>(assetName);

            _callBack(data);
#endif
        }

        public void GetAsset<T>(string _name, Action<T[]> _callBack) where T : UnityEngine.Object
        {
            string assetName;

            if (getAssetPathDelegate != null)
            {
                string tmpName = getAssetPathDelegate(_name, typeof(T));

                if (!string.IsNullOrEmpty(tmpName))
                {
                    assetName = tmpName;
                }
                else
                {
                    assetName = _name;
                }
            }
            else
            {
                assetName = _name;
            }

#if USE_ASSETBUNDLE

            IAssetManagerUnit unit;

            if (!dic.TryGetValue(assetName, out unit))
            {
                unit = new AssetManagerUnit2<T>(assetName);

                dic.Add(assetName, unit);
            }

            (unit as AssetManagerUnit2<T>).Load(_callBack);

#else
            UnityEngine.Object[] datas = AssetDatabase.LoadAllAssetsAtPath(assetName);

            List<T> tmpList = new List<T>();

            for (int i = 0; i < datas.Length; i++)
            {
                UnityEngine.Object data = datas[i];

                if (data is T)
                {
                    tmpList.Add(data as T);
                }
            }

            T[] result = tmpList.ToArray();

            _callBack(result);
#endif
        }
    }
}
