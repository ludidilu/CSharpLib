using UnityEngine;
using System.Collections;
using assetManager;
using gameObjectFactory;

namespace sceneStart{

	public class SceneStart : MonoBehaviour {

		[SerializeField]
		private string prefabPath;

		// Use this for initialization
		void Awake () {
		
			GameObjectFactory.Instance.GetGameObject(prefabPath,GetGo);
		}

		private void GetGo(GameObject _go){

			GameObject.Destroy(gameObject);
		}
	}
}