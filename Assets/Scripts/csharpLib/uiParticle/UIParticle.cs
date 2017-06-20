using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticle : Graphic {

	private static readonly Vector4 uv = new Vector4(0, 0, 1, 1);

	private ParticleSystem ps;

	private ParticleSystemRenderer psr;

	private ParticleSystem.Particle[] p;

	private float fix;

	private UIVertex[] vertex;

	private Camera m_camera;

	private Vector2 canvasRect;

	void Start(){

		canvasRect = (canvas.transform as RectTransform).sizeDelta;

		Vector2 pos0 = WorldPositionToCanvasPosition (new Vector3(0,0,0));

		Vector2 pos1 = WorldPositionToCanvasPosition (new Vector3(1,0,0));

		fix = Mathf.Abs (pos0.x - pos1.x) * 0.5f;
	}

	protected override void Awake ()
	{
		base.Awake ();

		m_camera = canvas.worldCamera;

		ps = GetComponent<ParticleSystem> ();

		psr = GetComponent<ParticleSystemRenderer> ();

		p = new ParticleSystem.Particle[ps.maxParticles];

		vertex = new UIVertex[4];

		base.raycastTarget = false;

		psr.enabled = false;
	}

	public override Material material {

		get {

			if (Application.isPlaying) {

				return psr.sharedMaterial;

			} else {

				return base.material;
			}
		}
	}

	public override Texture mainTexture {

		get {

			if (Application.isPlaying) {

				return psr.sharedMaterial.mainTexture;

			} else {

				return material.mainTexture;
			}
		}
	}

	void Update(){

		if (Application.isPlaying) {

//			ps.Simulate (Time.unscaledDeltaTime, false, false);

			SetVerticesDirty ();
		}
	}

	protected override void OnPopulateMesh (VertexHelper vh){
		
		vh.Clear ();

		int num = ps.GetParticles (p);

		for (int i = 0; i < num; i++) {

			ParticleSystem.Particle pp = p [i];

			float size = pp.GetCurrentSize (ps);

			Color color = pp.GetCurrentColor (ps);

			Vector2 pos = WorldPositionToCanvasPosition (pp.position);

			UIVertex v = UIVertex.simpleVert;

			v.color = color;

			v.position = new Vector2 (pos.x - size * fix, pos.y - size * fix);

			v.uv0 = new Vector2 (uv.x, uv.y);

			vertex [0] = v;

			v = UIVertex.simpleVert;

			v.color = color;

			v.position = new Vector2 (pos.x - size * fix, pos.y + size * fix);

			v.uv0 = new Vector2 (uv.x, uv.w);

			vertex [1] = v;

			v = UIVertex.simpleVert;

			v.color = color;

			v.position = new Vector2 (pos.x + size * fix, pos.y + size * fix);

			v.uv0 = new Vector2 (uv.z, uv.w);

			vertex [2] = v;

			v = UIVertex.simpleVert;

			v.color = color;

			v.position = new Vector2 (pos.x + size * fix, pos.y - size * fix);

			v.uv0 = new Vector2 (uv.z, uv.y);

			vertex [3] = v;

			vh.AddUIVertexQuad (vertex);
		}
	}

	private Vector3 WorldPositionToCanvasPosition(Vector3 _worldPosition){

		Vector3 screenPos = m_camera.WorldToViewportPoint(_worldPosition);

		return new Vector3((screenPos.x * canvasRect.x) - (canvasRect.x * 0.5f),(screenPos.y * canvasRect.y) - (canvasRect.y * 0.5f),screenPos.z);
	}
}
