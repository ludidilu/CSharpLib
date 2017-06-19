using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

[Serializable]
public class AddAtt{

	public string att;

	public int attType;

	public int dataInt;
	public int[] dataInts;
	
	public string dataString;
	public string[] dataStrings;
	
	public bool dataBool;
	public bool[] dataBools;
	
	public float dataFloat;
	public float[] dataFloats;
	
	public UnityEngine.Object dataObj;
	public UnityEngine.Object[] dataObjs;

	public Color32 dataColor32;
	public Color32[] dataColor32s;

	public Color dataColor;
	public Color[] dataColors;

	public Vector4 dataVector4;
	public Vector4[] dataVector4s;

	public LayerMask dataLayerMask;
	public LayerMask[] dataLayerMasks;

	public AnimationCurve dataAnimationCurve;
	public AnimationCurve[] dataAnimationCurves;

	public int dataMonoBehaviourInstanceID;
	public int[] dataMonoBehaviourInstanceIDs;

	public AddAtt(string _attName,object _target,Dictionary<int,MonoBehaviour> _dic){

		att = _attName;

		GetData(_target,_dic);
	}

	private void GetData(object vv,Dictionary<int,MonoBehaviour> _dic){

		if(vv is MonoBehaviour){

			if(_dic.ContainsKey((vv as MonoBehaviour).GetInstanceID())){

				attType = 0;

				dataMonoBehaviourInstanceID = (vv as MonoBehaviour).GetInstanceID();

				return;
			}

		}else if(vv is MonoBehaviour[]){

			MonoBehaviour[] tmpArr = vv as MonoBehaviour[];

			if(tmpArr.Length == 0){

				throw new Exception("AddAtt error! Type MonoBehaviour[] length == 0!");
			}

			bool getBehaviour = _dic.ContainsKey(tmpArr[0].GetInstanceID());
			
			for(int i = 0 ; i < tmpArr.Length ; i++){

				bool b = _dic.ContainsKey(tmpArr[i].GetInstanceID());

				if(b != getBehaviour){

					throw new Exception("AddAtt error! Type MonoBehaviour[] type error!");
				}
			}

			if(getBehaviour){

				attType = 100;

				dataMonoBehaviourInstanceIDs = new int[tmpArr.Length];

				for(int i = 0 ; i < tmpArr.Length ; i++){

					dataMonoBehaviourInstanceIDs[i] = tmpArr[i].GetInstanceID();
				}

				return;
			}
		}


		if(vv is Int32){
			
			attType = 1;
			
			dataInt = (int)vv;
			
		}else if(vv is String){
			
			attType = 2;
			
			dataString = (string)vv;
			
		}else if(vv is Boolean){
			
			attType = 3;
			
			dataBool = (bool)vv;
			
		}else if(vv is Single){
			
			attType = 4;
			
			dataFloat = (float)vv;
			
		}else if(vv is Color32){
			
			attType = 5;
			
			dataColor32 = (Color32)vv;
			
		}else if(vv is Color){
			
			attType = 6;
			
			dataColor = (Color)vv;
			
		}else if(vv is Vector4){
			
			attType = 7;
			
			dataVector4 = (Vector4)vv;
			
		}else if(vv is LayerMask){
			
			attType = 8;
			
			dataLayerMask = (LayerMask)vv;
			
		}else if(vv is AnimationCurve){
			
			attType = 9;
			
			dataAnimationCurve = (AnimationCurve)vv;
			
		}else if(vv is UnityEngine.Object){
			
			attType = 10;
			
			dataObj = (UnityEngine.Object)vv;
			
		}





		else if(vv is Int32[]){
			
			attType = 101;
			
			dataInts = (int[])vv;
			
		}else if(vv is String[]){
			
			attType = 102;
			
			dataStrings = (string[])vv;
			
		}else if(vv is Boolean[]){
			
			attType = 103;
			
			dataBools = (bool[])vv;
			
		}else if(vv is Single[]){
			
			attType = 104;
			
			dataFloats = (float[])vv;
			
		}else if(vv is Color32[]){
			
			attType = 105;
			
			dataColor32s = (Color32[])vv;
			
		}else if(vv is Color[]){
			
			attType = 106;
			
			dataColors = (Color[])vv;
			
		}else if(vv is Vector4[]){
			
			attType = 107;
			
			dataVector4s = (Vector4[])vv;
			
		}else if(vv is LayerMask[]){
			
			attType = 108;
			
			dataLayerMasks = (LayerMask[])vv;
			
		}else if(vv is AnimationCurve[]){
			
			attType = 109;
			
			dataAnimationCurves = (AnimationCurve[])vv;
			
		}else if(vv is UnityEngine.Object[]){
			
			attType = 110;
			
			dataObjs = (UnityEngine.Object[])vv;

		}



		else{

			throw new Exception("AddAtt error! Type not found:" + vv.GetType());
		}
	}

	// Use this for initialization
	public void Init (Component component,Type type,Dictionary<int,Component> _dic) {
	
//		SuperDebug.Log("SetAtt:" + att);

		FieldInfo fieldInfo = type.GetField(att,BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

		switch(attType){

		case 0:

			fieldInfo.SetValue(component,_dic[dataMonoBehaviourInstanceID]);

			break;

		case 1:

			fieldInfo.SetValue(component,dataInt);

			break;

		case 2:
			
			fieldInfo.SetValue(component,dataString);
			
			break;

		case 3:
			
			fieldInfo.SetValue(component,dataBool);
			
			break;

		case 4:
			
			fieldInfo.SetValue(component,dataFloat);
			
			break;

		case 5:

			fieldInfo.SetValue(component,dataColor32);
			
			break;

		case 6:

			fieldInfo.SetValue(component,dataColor);
			
			break;

		case 7:

			fieldInfo.SetValue(component,dataVector4);
			
			break;

		case 8:

			fieldInfo.SetValue(component,dataLayerMask);
			
			break;

		case 9:

			fieldInfo.SetValue(component,dataAnimationCurve);
			
			break;

		case 10:

			if(dataObj != null){
				
				fieldInfo.SetValue(component,dataObj);
			}

			break;





		case 100:

			Type unitType = fieldInfo.FieldType.GetElementType();

			Array arr = Array.CreateInstance(unitType,dataMonoBehaviourInstanceIDs.Length);
			
			for(int i = 0 ; i < dataMonoBehaviourInstanceIDs.Length ; i++){
				
				arr.SetValue(_dic[dataMonoBehaviourInstanceIDs[i]],i);
			}
			
			fieldInfo.SetValue(component,arr);

			break;

		case 101:

			fieldInfo.SetValue(component,dataInts);

			break;

		case 102:

			fieldInfo.SetValue(component,dataStrings);

			break;

		case 103:

			fieldInfo.SetValue(component,dataBools);

			break;

		case 104:

			fieldInfo.SetValue(component,dataFloats);

			break;

		case 105:

			fieldInfo.SetValue(component,dataColor32s);

			break;

		case 106:

			fieldInfo.SetValue(component,dataColors);

			break;

		case 107:

			fieldInfo.SetValue(component,dataVector4s);

			break;

		case 108:

			fieldInfo.SetValue(component,dataLayerMasks);

			break;

		case 109:

			fieldInfo.SetValue(component,dataAnimationCurves);

			break;

		case 110:

			if(dataObjs != null){

				unitType = fieldInfo.FieldType.GetElementType();

				arr = Array.CreateInstance(unitType,dataObjs.Length);

				for(int i = 0 ; i < dataObjs.Length ; i++){

					arr.SetValue(dataObjs[i],i);
				}

				fieldInfo.SetValue(component,arr);
			}
			
			break;
		}
	}
}
