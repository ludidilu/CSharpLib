#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace effect{
	
	[AddComponentMenu("UI/Effects/TextSequenceEffect")]
	public class TextSequenceEffect : BaseMeshEffect
	{
		private bool[] showArr;

		private int strLength;

		private int showNum = -1;

		private Action<int> callBack;

		public void SetShowNum(int _showNum){

			showNum = _showNum;
		}

		public void Init(Action<int> _callBack){

			showNum = 0;

			callBack = _callBack;
		}

		#if UNITY_5_2_2 || UNITY_5_2_3 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6
		public override void ModifyMesh (VertexHelper vh){
			
			if (!IsActive () || showNum == -1) {
				
				return;
			}

			if(callBack != null){

				InitReal(vh);

				Action<int> tmpCallBack = callBack;
				
				callBack = null;
				
				tmpCallBack(strLength);
			}

			int index = 0;
			
			for(int i = 0 ; i < vh.currentIndexCount / 6 ; i++){

				if(showArr[i]){
					
					if(index >= showNum){

						for(int m = 0 ; m < 4 ; m++){
							
							UIVertex uiVertex1 = new UIVertex();

							vh.SetUIVertex(uiVertex1,i * 4 + m);
						}
					}
					
					index++;
				}
			}
		}
		
		#endif

		private void InitReal(VertexHelper vh){

			showArr = new bool[vh.currentIndexCount / 6];
			
			strLength = 0;
			
			for(int i = 0 ; i < vh.currentIndexCount / 6 ; i++){
				
				UIVertex uiVertex0 = new UIVertex();
				
				vh.PopulateUIVertex(ref uiVertex0,i * 4);

				bool isSame = false;
				
				for(int m = 1 ; m < 4 ; m++){
					
					UIVertex uiVertex1 = new UIVertex();
					
					vh.PopulateUIVertex(ref uiVertex1,i * 4 + m);

					if(uiVertex1.position == uiVertex0.position){
						
						isSame = true;
						
						break;
					}
				}
				
				if(!isSame){
					
					strLength++;
				}
				
				showArr[i] = !isSame;
			}
		}

		public override void ModifyMesh (Mesh _mesh)
		{
			if (!IsActive () || showNum == -1) {
				
				return;
			}
			
			using (VertexHelper vh = new VertexHelper(_mesh)) {
					
				if(callBack != null){
					
					InitReal(vh);
					
					Action<int> tmpCallBack = callBack;
					
					callBack = null;
					
					tmpCallBack(strLength);
				}
				
				int index = 0;
				
				for(int i = 0 ; i < vh.currentIndexCount / 6 ; i++){
					
					if(showArr[i]){
						
						if(index >= showNum){
							
							for(int m = 0 ; m < 4 ; m++){
								
								UIVertex uiVertex1 = new UIVertex();
								
								vh.SetUIVertex(uiVertex1,i * 4 + m);
							}
						}
						
						index++;
					}
				}

				vh.FillMesh (_mesh);
			}
		}
		
	}
}

#else

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace effect{
	
	[AddComponentMenu("UI/Effects/TextSequenceEffect")]
	public class TextSequenceEffect : BaseVertexEffect {

		private bool[] showArr;
		
		private int strLength;
		
		private int showNum = -1;
		
		private Action<int> callBack;
		
		public void SetShowNum(int _showNum){
			
			showNum = _showNum;
		}
		
		public void Init(Action<int> _callBack){
			
			showNum = 0;
			
			callBack = _callBack;
		}
		
		public override void ModifyVertices(List<UIVertex> vertexList) {

			if (!IsActive() || showNum == -1) {

				return;
			}

			if(callBack != null){
				
				InitReal(vertexList);
				
				Action<int> tmpCallBack = callBack;
				
				callBack = null;
				
				tmpCallBack(strLength);
			}

			int index = 0;

			for(int i = 0 ; i < vertexList.Count / 4 ; i++){

				if(showArr[i]){
					
					if(index >= showNum){

						for(int m = 0 ; m < 4 ; m++){

							vertexList[i * 4 + m] = new UIVertex();
						}
					}
					
					index++;
				}
			}
		}

		private void InitReal(List<UIVertex> vertexList){

			showArr = new bool[vertexList.Count / 4];
			
			strLength = 0;
			
			for(int i = 0 ; i < vertexList.Count / 4 ; i++){
				
				UIVertex uiVertex0 = vertexList[i * 4];
				
				bool isSame = true;
				
				for(int m = 1 ; m < 4 ; m++){
					
					UIVertex uiVertex1 = vertexList[i * 4 + m];
					
					if(uiVertex1.position != uiVertex0.position){
						
						isSame = false;
						
						break;
					}
				}

				if(!isSame){

					strLength++;
				}

				showArr[i] = !isSame;
			}
		}
	}
}
#endif
