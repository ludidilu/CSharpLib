using UnityEngine;
using System.Collections;

namespace cameraEffect{ 

	[RequireComponent(typeof(Camera))]
	public class CameraEffect : MonoBehaviour {

		[SerializeField]
		public Material material;

		// Use this for initialization
		void Awake () {

			material = Material.Instantiate<Material>(material);
		}

		// Update is called once per frame
		void OnRenderImage  (RenderTexture _s,RenderTexture _t) {

			Graphics.Blit(_s,null,material);
		}

		void Update () {
			
			if(Input.GetMouseButton(0)){
				
				Vector4 v = new Vector4(Input.mousePosition.x / Screen.width,Input.mousePosition.y / Screen.height,0,0);
				
				material.SetVector("_Center",v);
			}
		}
	}
}