using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using assetManager;

namespace animatorFactoty{

	public class AnimatorControllerManager : ScriptableObject {

		public static readonly string PATH = "Assets/Arts/animation";

		public static readonly string FILE_NAME = "animators.asset";

		private static AnimatorControllerManager _Instance;

		public static AnimatorControllerManager Instance{

			get{

				return _Instance;
			}
		}

		[SerializeField]
		private string[] names;

		[SerializeField]
		private RuntimeAnimatorController[] animators;

		private Dictionary<string,RuntimeAnimatorController> dic;

		public static void Init(Action _callBack){

			Action<AnimatorControllerManager> callBack = delegate(AnimatorControllerManager obj) {

				LoadOK (obj,_callBack);
			};

			AssetManager.Instance.GetAsset<AnimatorControllerManager>(PATH + "/" + FILE_NAME,callBack);
		}

		private void InitDic(){

			dic = new Dictionary<string, RuntimeAnimatorController>();
			
			for(int i = 0 ; i < names.Length ; i++){
				
				dic.Add(names[i],animators[i]);
			}
		}

		private static void LoadOK(AnimatorControllerManager _asset,Action _callBack){

			_asset.InitDic();

			_Instance = _asset;

			if(_callBack != null){

				_callBack();
			}
		}

		public void Save(string[] _names,RuntimeAnimatorController[] _animators){

			names = _names;
			animators = _animators;
		}

		public RuntimeAnimatorController GetAnimator(string _name){

			if(dic.ContainsKey(_name)){

				return dic[_name];

			}else{

				return null;
			}
		}
	}
}