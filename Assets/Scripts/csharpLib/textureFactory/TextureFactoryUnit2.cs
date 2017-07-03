using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using assetManager;
using System;

namespace textureFactory{
	
	public class TextureFactoryUnit2<T> :ITextureFactoryUnit where T:UnityEngine.Object {
		
		private string name;
		private T[] data;
		private int type = -1;
		
		private bool isDispose = false;

		private List<KeyValuePair<Action<T>,int>> callBackList = new List<KeyValuePair<Action<T>, int>>();
		
		public TextureFactoryUnit2(string _name){
			
			name = _name;
		}
		
		public T GetTexture(int _index,Action<T> _callBack){
			
			if (type == -1) {
				
				type = 0;

				callBackList.Add (new KeyValuePair<Action<T>, int>(_callBack,_index));

				AssetManager.Instance.GetAsset<T> (name,GetAsset);

                return default(T);

            } else if (type == 0) {

				callBackList.Add (new KeyValuePair<Action<T>, int>(_callBack,_index));
				
				return default(T);
				
			} else {
				
				if(_callBack != null){
					
					_callBack(data[_index]);
				}
				
				return data[_index];
			}
		}
		
		private void GetAsset(T[] _data){

			if(_data.Length < 1){

				throw new Exception("Texture load fail! name:" + name);
			}
			
			if(isDispose){

				for(int i = 0 ; i < _data.Length ; i++){
				
					Resources.UnloadAsset(_data[i]);
				}

				return;
			}
		
			data = _data;
			
			type = 1;

            for (int i = 0; i < callBackList.Count; i++)
            {
                KeyValuePair<Action<T>, int> pair = callBackList[i];

                Action<T> callBack = pair.Key;

                if (callBack != null)
                {
                    int index = pair.Value;

                    callBack(data[index]);
                }
            }

			callBackList.Clear();
		}
		
		public void Dispose(){
			
			if (type == 1) {
				
				for(int i = 0 ; i < data.Length ; i++){
					
					Resources.UnloadAsset(data[i]);
				}
				
				data = null;
				
			}else{
				
				isDispose = true;
			}
		}
	}
}
