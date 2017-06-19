using UnityEngine;
using System.Collections;
using superTween;
using System;
using gameObjectFactory;

namespace scene{

	public class SceneContinueStart : MonoBehaviour {

		public const string COPY_NAME = "SceneContinueCopy";

		private Action<SceneContinue> callBack;

		public void SetCallBack(Action<SceneContinue> _callBack){

			callBack = _callBack;
		}

		// Use this for initialization
		void Start () {

			GameObject tmpGo = gameObject;

			Action<SceneContinue> tmpCb = callBack;

			Action initCb = delegate() {
				
				GameObject go = GameObject.Instantiate(tmpGo);

				Scene scene = go.GetComponent<Scene>();

				if(scene != null){

					scene.resetWhenDisable = false;

					GameObject.Destroy(scene);
				}

				Light light = go.GetComponentInChildren<Light>();

				if(light != null){

					GameObject.Destroy(light);
				}

				go.name = COPY_NAME;
				
				go.transform.SetParent(tmpGo.transform.parent,false);

				SceneContinue sceneContinue = tmpGo.AddComponent<SceneContinue>();
				
				sceneContinue.SetCopy(go);

				go.SetActive(false);

				if(tmpCb != null){

					tmpCb(sceneContinue);
				}
			};

			SuperTween.Instance.DelayCall(0,initCb);

			GameObject.Destroy(this);
		}
	}
}
