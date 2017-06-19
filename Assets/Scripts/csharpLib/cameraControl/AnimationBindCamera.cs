using UnityEngine;
using System.Collections;
using System;

public class AnimationBindCamera : MonoBehaviour {
	
	private static Action bindCallBack;
	
	private static Action unbindCallBack;
	
	public static void Init(Action _bindCallBack,Action _unbindCallBack){

		bindCallBack = _bindCallBack;

		unbindCallBack = _unbindCallBack;
	}

	private Transform recTrans;

	private Vector3 recPos;

	private Quaternion recRotation;

	public void BindCamera(){

		recTrans = Camera.main.transform.parent;

		recPos = Camera.main.transform.localPosition;

		recRotation = Camera.main.transform.localRotation;

		Camera.main.transform.SetParent (transform, false);

		Camera.main.transform.localPosition = Vector3.zero;

		Camera.main.transform.localRotation = Quaternion.identity;

		if(bindCallBack != null){

			bindCallBack();
		}
	}

	public void UnbindCamera(){

		Camera.main.transform.SetParent (recTrans, false);

		Camera.main.transform.localPosition = recPos;

		Camera.main.transform.localRotation = recRotation;

		if(unbindCallBack != null){

			unbindCallBack();
		}
	}
}
