using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using assetManager;

namespace audio{

	public class AudioFactoryUnit {

		private string name;

		private AudioClip data;

		private int type = -1;

		public bool willDispose = true;

		private List<Action<AudioClip>> callBackList = new List<Action<AudioClip>>();

		public AudioFactoryUnit(string _name){

			name = _name;
		}

		public AudioClip GetClip(Action<AudioClip> _callBack,bool _willDispose){

			willDispose = willDispose && _willDispose;

			if (type == -1) {
				
				type = 0;
				
				callBackList.Add (_callBack);

                return null;
				
			} else if (type == 0) {
				
				callBackList.Add (_callBack);
				
				return null;
				
			} else {
				
				if(_callBack != null){
					
					_callBack(data);
				}
				
				return data;
			}
		}

		private void GetAsset(AudioClip _data){

			data = _data;

			data.name = name;
			
			type = 1;

            for (int i = 0; i < callBackList.Count; i++)
            {
                Action<AudioClip> callBack = callBackList[i];

                if (callBack != null)
                {
                    callBack(data);
                }
            }

			callBackList.Clear();
		}

		public void Dispose(){

			Resources.UnloadAsset(data);
		}
	}
}