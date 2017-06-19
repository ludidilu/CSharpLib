using UnityEngine;
using System.Collections;
using superFunction;
using System;
using System.Collections.Generic;

namespace superRaycast{

	public class SuperRaycast : MonoBehaviour {

		public const string GetMouseButtonDown = "GetMouseButtonDown";
		public const string GetMouseButton = "GetMouseButton";
		public const string GetMouseButtonUp = "GetMouseButtonUp";
		public const string GetMouseEnter = "GetMouseEnter";
		public const string GetMouseExit = "GetMouseExit";
		public const string GetMouseClick = "GetMouseClick";

		public const string GetBlockByUi = "GetBlockByUi";

		private static SuperRaycast _Instance;

		public static void SetCamera(Camera _camera){

			Instance.renderCamera = _camera;
		}

		public static void AddLayer(string _layerName){

			Instance.AddLayerReal(_layerName);
		}

		public static void DelLayer(string _layerName){

			Instance.DelLayerReal(_layerName);
		}

		public static GameObject Go{

			get{

				return Instance.gameObject;
			}
		}

		private static SuperRaycast Instance{

			get{

				if(_Instance == null){

					GameObject go = new GameObject("SuperRaycastGameObject");
					
					_Instance = go.AddComponent<SuperRaycast>();
				}

				return _Instance;
			}
		}

		public static bool GetIsOpen(){

			return Instance.m_isOpen > 0;
		}

		public static void SetIsOpen(bool _isOpen, string _str){

			Instance.m_isOpen = Instance.m_isOpen + (_isOpen ? 1 : -1);
			
			if(Instance.m_isOpen == 0){
				
				if(!Instance.isProcessingUpdate){
					
					Instance.objs.Clear();
					
				}else{
					
					Instance.needClearObjs = true;
				}
				
			}else if(Instance.m_isOpen > 1)
            {
                PrintLog();

                SuperDebug.Log("SuperRaycast error!!!!!!!!!!!!!");

            }

            if (Instance.dic.ContainsKey(_str)){

				if(_isOpen){

					Instance.dic[_str]++;

				}else{

					Instance.dic[_str]--;
				}

				if(Instance.dic[_str] == 0){

					Instance.dic.Remove(_str);
				}

			}else{

				if(_isOpen){

					Instance.dic.Add(_str,1);

				}else{

					Instance.dic.Add(_str,-1);
				}
			}
		}

        public static void PrintLog()
        {
            if (Instance.needClearObjs)
            {
                foreach (KeyValuePair<string, int> pair in Instance.dic)
                {
                    SuperDebug.Log("SuperRaycast key:" + pair.Key + "  value:" + pair.Value);
                }
            }
        }

        public static string filterTag{

			get{

				return Instance.m_filterTag;
			}

			set{

				Instance.m_filterTag = value;
			}
		}

		public static bool filter{

			get{

				return Instance.m_filter;
			}

			set{

				Instance.m_filter = value;
			}
		}

		public static bool checkBlockByUi{
			
			get{
				
				return Instance.m_checkBlockByUi;
			}
			
			set{

				if(Instance.m_checkBlockByUi != value){
				
					Instance.m_checkBlockByUi = value;

					if(Instance.m_checkBlockByUi){

						SuperFunction.Instance.AddEventListener(Instance.gameObject,GetBlockByUi,Instance.CheckBlockByUiHandler);

					}else{

						SuperFunction.Instance.RemoveEventListener(Instance.gameObject,GetBlockByUi,Instance.CheckBlockByUiHandler);
					}
				}
			}
		}

		public Dictionary<string,int> dic = new Dictionary<string, int>();
		
		private int layerIndex;

		private int m_isOpen = 0;

		private bool m_filter = false;
		
		private string m_filterTag;

		private List<GameObject> downObjs = new List<GameObject>();

		private List<GameObject> objs = new List<GameObject>();

		private bool isProcessingUpdate = false;

		private bool needClearObjs = false;

		private bool m_checkBlockByUi = false;

		private bool isBlockByUi = false;

		private Camera renderCamera;

		private void AddLayerReal(string _layerName){
			
			layerIndex = layerIndex | ( 1 << LayerMask.NameToLayer(_layerName));
		}
		
		private void DelLayerReal(string _layerName){
			
			layerIndex = layerIndex & ~(1 << LayerMask.NameToLayer(_layerName));
		}

		private void CheckBlockByUiHandler(int _index){

			isBlockByUi = true;
		}

		void Update(){

			if (m_checkBlockByUi && isBlockByUi) {

				isBlockByUi = false;

				return;
			}
			
			if(m_isOpen > 0 && renderCamera != null){

				isProcessingUpdate = true;

				RaycastHit[] hits = null;

				if(Input.GetMouseButtonDown(0)){

					Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);

					if(layerIndex == 0){

						hits = Physics.RaycastAll(ray,float.MaxValue);

					}else{

						hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);
					}

					int i = 0;
					
					for(int m = 0 ; m < hits.Length ; m++){
						
						RaycastHit hit = hits[m];
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}
						
						objs.Add(hit.collider.gameObject);

						downObjs.Add(hit.collider.gameObject);

						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,GetMouseButtonDown,hit,i);
						
						i++;
					}

				}

				if(Input.GetMouseButton(0)){

					if(hits == null){

						Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);
						
						if(layerIndex == 0){
							
							hits = Physics.RaycastAll(ray,float.MaxValue);
							
						}else{
							
							hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);
						}
					}
					
					List<GameObject> newObjs = new List<GameObject>();

					List<GameObject> enterObjs = new List<GameObject>();
					
					int i = 0;
					
					for(int m = 0 ; m < hits.Length ; m++){

						RaycastHit hit = hits[m];
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}
						
						newObjs.Add(hit.collider.gameObject);
						
						if(!objs.Contains(hit.collider.gameObject)){

							enterObjs.Add(hit.collider.gameObject);

						}else{
							
							objs.Remove(hit.collider.gameObject);
						}

						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,GetMouseButton,hit,i);
						
						i++;
					}

					for(i = 0 ; i < objs.Count ; i++){

						SuperFunction.Instance.DispatchEvent(objs[i],GetMouseExit);
					}
					
					objs = newObjs;

					for(i = 0 ; i < enterObjs.Count ; i++){
						
						SuperFunction.Instance.DispatchEvent(enterObjs[i],GetMouseEnter);
					}
				}

				if(Input.GetMouseButtonUp(0)){

					if(hits == null){

						Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);
						
						if(layerIndex == 0){
							
							hits = Physics.RaycastAll(ray,float.MaxValue);
							
						}else{
							
							hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);
						}
					}

					int i = 0;
					
					for(int m = 0 ; m < hits.Length ; m++){
						
						RaycastHit hit = hits[m];
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}

						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,GetMouseButtonUp,hit,i);

						if(downObjs.Contains(hit.collider.gameObject)){

							SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,GetMouseClick,hit,i);
						}
						
						i++;
					}

					downObjs.Clear();
					
					objs.Clear();
				}

				if(needClearObjs){

					needClearObjs = false;

					objs.Clear();

					downObjs.Clear();
				}
				
				isProcessingUpdate = false;
			}
		}
	}
}
