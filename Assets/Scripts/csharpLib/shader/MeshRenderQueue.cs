using UnityEngine;
using System.Collections;

namespace shader{

	public class MeshRenderQueue : MonoBehaviour {

		[SerializeField]
		private int renderQueue;

		// Use this for initialization
		void Awake () {
		
			GetComponent<Renderer>().material.renderQueue = renderQueue;

			GameObject.Destroy(this);
		}
	}
}