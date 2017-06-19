using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	private Transform renderCamera;

	[SerializeField]
	private Transform target;

	[SerializeField]
	private float rotateSpeed = 2f;

	[SerializeField]
	private float scrollSpeed = 0.1f;

	[SerializeField]
	private float xAngle = -1;//允许绕x轴转动的角度 小于0代表无限制

	[SerializeField]
	private float yAngle = -1;//允许绕y轴转动的角度 小于0代表无限制

	private float distance;

	private float nowXAngle;

	private float xAngleChange;

	private float nowYAngle;

	private float yAngleChange;

	// Use this for initialization
	void OnEnable () {
	
		target.LookAt(renderCamera,Vector3.up);

		nowXAngle = target.localEulerAngles.x;

		nowYAngle = target.localEulerAngles.y;

		xAngleChange = yAngleChange = 0;

		distance = Vector3.Distance(target.position,renderCamera.position);
	}
	
	// Update is called once per frame
	void Update () {
		
		bool hasChange = false;

		if(!Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)){

			RefreshCameraAngle();

			hasChange = true;
		}

		if(Input.mouseScrollDelta.y != 0){

			RefreshCameraDistance();

			hasChange = true;
		}

		if(hasChange){

			RefreshCameraPosition();
		}
	}

	private void RefreshCameraAngle(){

		Vector3 q = target.localEulerAngles;

		if(xAngle < 0){

			q.x += GetInputDeltaX() * rotateSpeed;

		}else{

			xAngleChange += GetInputDeltaX() * rotateSpeed;

			if(xAngleChange > xAngle){

				xAngleChange = xAngle;

			}else if(xAngleChange < -xAngle){

				xAngleChange = -xAngle;
			}

			q.x = nowXAngle + xAngleChange;
		}

		if(yAngle < 0){

			q.y += GetInputDeltaY() * rotateSpeed;

		}else{

			yAngleChange += GetInputDeltaY() * rotateSpeed;

			if(yAngleChange > yAngle){

				yAngleChange = yAngle;

			}else if(yAngleChange < -yAngle){

				yAngleChange = -yAngle;
			}

			q.y = nowYAngle + yAngleChange;
		}

		target.localEulerAngles = q;
	}

	private void RefreshCameraDistance(){

		distance -= Input.mouseScrollDelta.y * scrollSpeed;
	}

	private void RefreshCameraPosition(){

		renderCamera.localPosition = target.forward * distance;

		renderCamera.LookAt (target);
	}

	private float GetInputDeltaX(){

		#if PLATFORM_PC || UNITY_EDITOR
		
		return Input.GetAxis("Mouse Y");
		
		#else
		
		return Input.GetTouch(0).deltaPosition.y;
		
		#endif
	}

	private float GetInputDeltaY(){
		
		#if PLATFORM_PC || UNITY_EDITOR
		
		return Input.GetAxis("Mouse X");
		
		#else

		return Input.GetTouch(0).deltaPosition.x;
		
		#endif
	}
}
