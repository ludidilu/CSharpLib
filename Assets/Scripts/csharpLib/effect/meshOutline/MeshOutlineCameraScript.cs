using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MeshOutlineCameraScript : MonoBehaviour {

	private Material mat;
	
	// Use this for initialization
	void Start () {

		mat = new Material(Shader.Find("Custom/Mesh_Outline"));
		
		Camera rc = GetComponent<Camera>();
		
		rc.clearFlags = CameraClearFlags.SolidColor;
		
		rc.backgroundColor = new Color(0,0,0,0);
		
		rc.depthTextureMode = DepthTextureMode.Depth;
	}
	
	void OnRenderImage(RenderTexture src, RenderTexture dest){
		
		Graphics.Blit(src,null,mat);
	}
}
