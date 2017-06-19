using UnityEngine;
using System.Collections;
using audio;
using assetManager;
using UnityEngine.UI;
using superGraphicRaycast;
using superFunction;

namespace screenScale{

	public class ScreenScale : MonoBehaviour {

		private static ScreenScale _Instance;

		public static ScreenScale Instance{

			get{

				if(_Instance == null){

					GameObject tmpGo = new GameObject("ScreenScaleGameObject");

					_Instance = tmpGo.AddComponent<ScreenScale>();

					_Instance.go = tmpGo;
				}

				return _Instance;
			}
		}

		public const string SCALE_CHANGE = "ScaleChange";

		public GameObject go{ get; private set; }

		private float distance = -1;

		// Update is called once per frame
		void Update () {

			if(Input.touchCount > 1){

				if(distance == -1){

					distance = Vector2.Distance(Input.GetTouch(0).position,Input.GetTouch(1).position);

				}else{

					float tmpDistance = Vector2.Distance(Input.GetTouch(0).position,Input.GetTouch(1).position);

					Vector2 pos = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;

					SuperFunction.Instance.DispatchEvent(gameObject,SCALE_CHANGE,tmpDistance / distance,pos);

					distance = tmpDistance;
				}

			}else if(distance != -1){

				distance = -1;
			}

			if (Input.mouseScrollDelta.y != 0) {

				SuperFunction.Instance.DispatchEvent(gameObject,SCALE_CHANGE,1 + Input.mouseScrollDelta.y,(Vector2)Input.mousePosition);
			}
		}
	}
}