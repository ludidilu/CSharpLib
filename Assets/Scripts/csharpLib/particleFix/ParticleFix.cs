using UnityEngine;
using System.Collections;

public class ParticleFix : MonoBehaviour {

	private ParticleSystemRenderer particleRenderer;

	private Camera renderCamera;

	public void Init(Camera _camera){

		renderCamera = _camera;
	}

	void Start(){

		particleRenderer = GetComponent<ParticleSystemRenderer>();
	}

	// Use this for initialization
	void Update () {

		particleRenderer.material.SetVector("_Scaling",transform.lossyScale);
			
		if(particleRenderer.renderMode == ParticleSystemRenderMode.Billboard || particleRenderer.renderMode == ParticleSystemRenderMode.Stretch){
				
			particleRenderer.material.SetVector("_Center", GetComponent<Renderer>().gameObject.transform.position); 

			if(renderCamera != null){

				particleRenderer.material.SetMatrix("_Camera", renderCamera.worldToCameraMatrix);  
				particleRenderer.material.SetMatrix("_CameraInv", renderCamera.worldToCameraMatrix.inverse);  
			}
		}
	}
}
