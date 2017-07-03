using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

using wwwManager;
using assetBundleManager;
using thread;

#if USE_ASSETBUNDLE

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

		public static AssetManager Instance {

			get {

				if (_Instance == null) {

					_Instance = new AssetManager ();
				}

				return _Instance;
			}
		}

		private Dictionary<string,IAssetManagerUnit> dic;
		
		public AssetManagerScript script;

		private Dictionary<string,AssetManagerData> dataDic;

		public AssetManager ()
		{
			dic = new Dictionary<string, IAssetManagerUnit> ();

			if(LOADASYNC){
				
				GameObject go = new GameObject("AssetManagerGameObject");

				GameObject.DontDestroyOnLoad(go);
				
				script = go.AddComponent<AssetManagerScript>();
			}
		}

		public void Init(Action _callBack){

			Init(_callBack,dataName);
		}

		public void Init(Action _callBack,string _dataName){

#if USE_ASSETBUNDLE
			
			Action<WWW> cb = delegate(WWW obj) {
				
				ThreadScript.Instance.Add(InitDic,obj.bytes,_callBack,CheckInitDicOK);
			};
			
			WWWManager.Instance.Load(_dataName,cb);

#else

			if(_callBack != null){

				_callBack();
			}

#endif
		}
		
		private void InitDic(object _dic){
			
			dataDic = AssetManagerDataFactory.GetData(_dic as Byte[]);
		}
		
		private bool CheckInitDicOK(){
			
			return dataDic != null;
		}

		public AssetManagerData GetData (string _name)
		{
			string name = _name.ToLower();
			
			if(!dataDic.ContainsKey(name)){
				
				SuperDebug.LogError("AssetBundle中没有找到Asset:" + _name);
			}
			
			return dataDic [name];
		}

		public void RemoveUnit (string _name)
		{
			dic.Remove (_name);
		}

		public void FixAssetBundleData(Dictionary<string,AssetManagerData> _addDic,BinaryWriter _bw){
			
			Dictionary<string,AssetManagerData>.Enumerator enumerator = _addDic.GetEnumerator();
			
			while(enumerator.MoveNext()){
				
				dataDic.Add(enumerator.Current.Key,enumerator.Current.Value);
			}

			AssetManagerDataFactory.SetData(_bw,dataDic);
		}

		public void GetAsset<T> (string _name, Action<T> _callBack) where T:UnityEngine.Object
		{

#if USE_ASSETBUNDLE

			AssetManagerUnit<T> unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new AssetManagerUnit<T> (_name);
				
				dic.Add(_name,unit);
				
			} else {
				
				unit = dic [_name] as AssetManagerUnit<T>;
			}
			
			unit.Load (_callBack);
#else

			T data = AssetDatabase.LoadAssetAtPath<T> (_name);

			_callBack (data);
#endif
		}

		public T[] GetAsset<T> (string _name, Action<T[]> _callBack) where T:UnityEngine.Object
		{
#if USE_ASSETBUNDLE
			
			AssetManagerUnit2<T> unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new AssetManagerUnit2<T> (_name);
				
				dic.Add(_name,unit);
				
			} else {
				
				unit = dic [_name] as AssetManagerUnit2<T>;
			}
			
			unit.Load (_callBack);
			
			return null;
			
#else
			UnityEngine.Object[] datas = AssetDatabase.LoadAllAssetsAtPath (_name);
			
			List<T> tmpList = new List<T>();

			for(int i = 0 ; i < datas.Length ; i++){

				UnityEngine.Object data = datas[i];

				if(data is T){

					tmpList.Add(data as T);
				}
			}

			T[] result = tmpList.ToArray();

			_callBack (result);
			
			return result;

#endif
		}
	}
}
