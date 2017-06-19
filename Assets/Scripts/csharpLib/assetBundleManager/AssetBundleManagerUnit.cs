using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using wwwManager;
using System;

namespace assetBundleManager{

	public class AssetBundleManagerUnit{

		private AssetBundle assetBundle;
		private string name;
		private int type = -1;
		private List<Action<AssetBundle>> callBackList;
		private int useTimes;

		public AssetBundleManagerUnit(string _name){

			name = _name;

			callBackList = new List<Action<AssetBundle>> ();
		}

		public void Load(Action<AssetBundle> _callBack){

			useTimes++;

//			SuperDebug.Log ("LoadAssetBundle:" + name);

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);

				WWWManager.Instance.Load(AssetBundleManager.path + name,GetAssetBundle);

			} else if (type == 0) {

				callBackList.Add (_callBack);

			} else {

				_callBack (assetBundle);
			}
		}

		private void GetAssetBundle(WWW _www){

			type = 1;

			if(string.IsNullOrEmpty(_www.error)){

				assetBundle = _www.assetBundle;

                for (int i = 0; i < callBackList.Count; i++)
                {
                    callBackList[i](assetBundle);
                }

			}else{

                for (int i = 0; i < callBackList.Count; i++)
                {
                    callBackList[i](null);
                }
			}

			callBackList.Clear ();
		}

		public void Unload(){

			useTimes--;

			if (useTimes == 0) {

//				SuperDebug.Log ("dispose assetBundle:" + name);

				assetBundle.Unload (false);

				AssetBundleManager.Instance.Remove (name);
			}
		}
	}
}
