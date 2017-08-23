using UnityEngine;
using System.Collections;
using publicTools;

namespace scene{

	public class Scene : MonoBehaviour {

		[SerializeField] public Texture2D[] farTextures;
		[SerializeField] public Texture2D[] nearTextures;
		
		[SerializeField] public float fieldOfView;
		
		[SerializeField] public Color ambientLight;
		[SerializeField] public float ambientIntensity;
		
		[SerializeField] public bool fog;
		[SerializeField] public Color fogColor;
		[SerializeField] public float fogStartDistance;
		[SerializeField] public float fogEndDistance;

		[HideInInspector] public bool resetWhenDisable = true;

		void OnEnable(){

			LightmapData[] lightmaps = new LightmapData[farTextures.Length];

			for(int i = 0 ; i < lightmaps.Length ; i++){

				lightmaps[i] = new LightmapData();

				lightmaps[i].lightmapColor = farTextures[i];
				lightmaps[i].lightmapDir = nearTextures[i];
			}

			LightmapSettings.lightmaps = lightmaps;

			RenderSettings.ambientLight = ambientLight;
			RenderSettings.ambientIntensity = ambientIntensity;

			if(fog){

				RenderSettings.fog = true;
				RenderSettings.fogMode = FogMode.Linear;
				RenderSettings.fogColor = fogColor;
				RenderSettings.fogStartDistance = fogStartDistance;
				RenderSettings.fogEndDistance = fogEndDistance;
			}
		}

		void OnDisable(){

			if(resetWhenDisable){
			
				LightmapSettings.lightmaps = new LightmapData[0];

				RenderSettings.ambientLight = Color.white;
				RenderSettings.ambientIntensity = 1;

				RenderSettings.fog = false;
			}
		}
	}
}