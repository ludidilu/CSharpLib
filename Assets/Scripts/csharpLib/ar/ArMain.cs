using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using publicTools;

public class ArMain : MonoBehaviour {
	
	private const float TARGET_DISTANCE = 5.0f;
	
	private const float TARGET_DISTANCE_FIX = 1.0f;
	
	private const float TARGET_Y_FIX = 1.0f;

	[SerializeField]
	private Transform container;

	[SerializeField]
	private Camera renderCamera;
	
	[SerializeField]
	private RawImage ri;
	
	[SerializeField]
	private Transform cameraTrans;
	
	private WebCamTexture webCamTexture;

	private int layerIndex;

	private int rotationFix;

	private void Init(){

		layerIndex = container.gameObject.layer;
	
		//设置陀螺仪的更新检索时间，即隔 0.1秒更新一次  
		Input.gyro.updateInterval = 0.05f;  

		StartCoroutine(webCam());
	}

	void OnEnable(){

		Input.gyro.enabled = true;  

		if(webCamTexture == null){

			Init();

		}else{

			webCamTexture.Play();
		}

		gameObject.SetActive(true);
	}

	public void AddChild(GameObject _go){

		PublicTools.SetLayer(_go,layerIndex);

		_go.transform.SetParent(container,false);
	}

	void OnDisable(){

		Input.gyro.enabled = false;  

		if(webCamTexture != null){
		
			webCamTexture.Stop();
		}

		gameObject.SetActive(false);
	}
	
	IEnumerator webCam(){
		
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))  
		{  
			WebCamDevice[] devices = WebCamTexture.devices;  
			
			if(devices.Length > 0){
				
				string deviceName = devices[0].name;  

				WebCamTexture tmpWebCamTexture = new WebCamTexture(deviceName, Screen.height / 2, Screen.width / 2, 12); 

				tmpWebCamTexture.Play();

				while(tmpWebCamTexture.width < 100){

					yield return null;
				}

				webCamTexture = tmpWebCamTexture;

//				SuperDebug.Log("Screen.width:" + Screen.width + "   Screen.height:" + Screen.height);
//
//				SuperDebug.Log("canvasX:" + (ri.canvas.transform as RectTransform).sizeDelta.x + "   canvasY:" + (ri.canvas.transform as RectTransform).sizeDelta.y);
//
//				SuperDebug.Log("webCamTexture.width:" + webCamTexture.width + "   webCamTexture.height:" + webCamTexture.height);
//
//				SuperDebug.Log("webCamTexture.requestedWidth:" + webCamTexture.requestedWidth + "   webCamTexture.requestedHeight:" + webCamTexture.requestedHeight);
//
				SuperDebug.Log("webCamTexture.videoRotationAngle:" + webCamTexture.videoRotationAngle);

				SuperDebug.Log("webCamTexture.videoVerticallyMirrored:" + webCamTexture.videoVerticallyMirrored);

				rotationFix = webCamTexture.videoVerticallyMirrored ? -1 : 1;

				ri.transform.localScale = new Vector3(1, rotationFix, 1);

				ri.texture = webCamTexture;

				if((float)webCamTexture.height / webCamTexture.width > (ri.canvas.transform as RectTransform).sizeDelta.x / (ri.canvas.transform as RectTransform).sizeDelta.y){

					ri.rectTransform.sizeDelta = new Vector2((ri.canvas.transform as RectTransform).sizeDelta.y,(ri.canvas.transform as RectTransform).sizeDelta.y * webCamTexture.height / webCamTexture.width);

				}else{

					ri.rectTransform.sizeDelta = new Vector2((ri.canvas.transform as RectTransform).sizeDelta.x * webCamTexture.width / webCamTexture.height,(ri.canvas.transform as RectTransform).sizeDelta.x);
				}

//				SuperDebug.Log("ri.rectTransform.sizeDelta:" + ri.rectTransform.sizeDelta);
			}
		}  
	}
	
	// Update is called once per frame
	void Update () {

		if(webCamTexture != null){
		
			UpdateCamera();
		}
	}
	
	private void UpdateCamera(){
		
		Quaternion q = Input.gyro.attitude;
		
		q.eulerAngles = new Vector3(-q.eulerAngles.x,-q.eulerAngles.y,q.eulerAngles.z);
		
		cameraTrans.rotation = q;//这里一定不能用localRotation!
	}

	public Vector3 GetCameraRotation(){

		return Input.gyro.attitude.eulerAngles;
	}

	public float GetAngle(Vector3 _pos){

		Vector3 v = _pos - cameraTrans.transform.localPosition;

		return Vector3.Angle(v,cameraTrans.transform.forward);
	}

	public void SetTargetPos(GameObject _go){
		
		Vector3 v = new Vector3(0.5f - UnityEngine.Random.value,0,0.5f - UnityEngine.Random.value);
		
		_go.transform.localPosition = cameraTrans.localPosition + v.normalized * (TARGET_DISTANCE + (0.5f - UnityEngine.Random.value) * TARGET_DISTANCE_FIX);
		
		_go.transform.localPosition = new Vector3(_go.transform.localPosition.x,_go.transform.localPosition.y + (0.5f - UnityEngine.Random.value) * TARGET_Y_FIX,_go.transform.localPosition.z);

		SetTargetRotation(_go);
	}
	
	private void SetTargetRotation(GameObject _go){
		
		float r = Mathf.Atan2(_go.transform.localPosition.x - cameraTrans.localPosition.x,_go.transform.localPosition.z - cameraTrans.localPosition.z) * Mathf.Rad2Deg - 180;
		
		Quaternion q = new Quaternion();
		
		q.eulerAngles = new Vector3(0,r,0);
		
		_go.transform.localRotation = q;
	}
}
