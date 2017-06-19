using UnityEngine;
using System.Collections;

public class UIParticleFix : MonoBehaviour {

    private ParticleSystemRenderer particleRenderer;

    private Camera uiCamera;

	// Use this for initialization
	void Start () {

		particleRenderer = GetComponent<ParticleSystemRenderer>();

		if (particleRenderer.renderMode == ParticleSystemRenderMode.Billboard)
        {
            uiCamera = gameObject.GetComponentInParent<Canvas>().worldCamera;
        }
    }

    void Update(){

		float scale = transform.lossyScale.x / 0.015625f;
		
		if(scale > 1){
			
			scale = 1;
		}

		particleRenderer.material.SetFloat("_Scaling",scale);
			
		if(particleRenderer.renderMode == ParticleSystemRenderMode.Billboard){

			particleRenderer.material.SetVector("_Center", particleRenderer.gameObject.transform.position);  
			particleRenderer.material.SetMatrix("_Camera", uiCamera.worldToCameraMatrix);  
			particleRenderer.material.SetMatrix("_CameraInv", uiCamera.worldToCameraMatrix.inverse);  
		}
	}
}
