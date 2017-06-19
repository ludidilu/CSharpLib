using UnityEngine;
using System.Collections;
using System;
using superTween;
using publicTools;

public class BallisticControl : MonoBehaviour {

	[SerializeField]
	private AnimationCurve curve;
	
	[SerializeField]
	private float flyTime;

	private Vector3 start;

	private Vector3 end;

	private float startTime;

	private Action callBack;

	private bool show;

	public void Fly(Vector3 _start,Vector3 _end,Action _callBack){

		start = _start;

		end = _end;

		startTime = Time.time;

		callBack = _callBack;

		PublicTools.SetGameObjectVisible(gameObject,false);
	}
	
	// Update is called once per frame
	void Update () {
	
		float percent = (Time.time - startTime) / flyTime;

		if(percent > 1){

			callBack();
			
			GameObject.Destroy(gameObject);

		}else{

			if(!show){

				show = true;

				PublicTools.SetGameObjectVisible(gameObject,true);
			}

			Vector3 v = Vector3.Lerp(start,end,percent);

			v.y += curve.Evaluate(percent);

			transform.LookAt(v);

			transform.position = v;
		}
	}
}
