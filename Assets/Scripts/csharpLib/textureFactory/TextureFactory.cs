using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using superTween;

namespace textureFactory{

	public class TextureFactory{

		private static TextureFactory _Instance;
		
		public static TextureFactory Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new TextureFactory ();
				}
				
				return _Instance;
			}
		}

		public Dictionary<string,ITextureFactoryUnit> dic  = new Dictionary<string, ITextureFactoryUnit>();
		public Dictionary<string,ITextureFactoryUnit> dicWillDispose  = new Dictionary<string, ITextureFactoryUnit>();

		public T GetTexture<T> (string _name,Action<T> _callBack,bool _doNotDispose) where T:UnityEngine.Object {

			return GetTexture(_name,0,_callBack,_doNotDispose);
		}

		public T GetTexture<T> (string _name,int _index,Action<T> _callBack,bool _doNotDispose) where T:UnityEngine.Object {
			
			TextureFactoryUnit2<T> unit;
			
			Dictionary<string,ITextureFactoryUnit> tmpDic;
			
			if (_doNotDispose) {
				
				tmpDic = dic;
				
			} else {
				
				tmpDic = dicWillDispose;
			}
			
			if (!tmpDic.ContainsKey (_name)) {
				
				unit = new TextureFactoryUnit2<T> (_name);
				
				tmpDic.Add (_name, unit);
				
			} else {
				
				unit = tmpDic [_name] as TextureFactoryUnit2<T>;
			}
			
			return unit.GetTexture(_index,_callBack);
		}

		public void Dispose(bool _force){

			Dictionary<string,ITextureFactoryUnit>.ValueCollection.Enumerator enumerator = dicWillDispose.Values.GetEnumerator();

			while(enumerator.MoveNext()){

				enumerator.Current.Dispose();
			}

			dicWillDispose.Clear ();

			if(_force){

				enumerator = dic.Values.GetEnumerator();

				while(enumerator.MoveNext()){

					enumerator.Current.Dispose();
				}

				dic.Clear();
			}
		}
	}
}
