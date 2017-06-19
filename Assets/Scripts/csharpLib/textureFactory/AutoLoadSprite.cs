using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using superFunction;

namespace textureFactory
{

	public class AutoLoadSprite : MonoBehaviour
	{

		public const string LOAD_OVER_EVENT = "AutoLoadSpriteLoadOver";

		[SerializeField]
		private string
			spriteName;

		[SerializeField]
		private Image
			img;

		[SerializeField]
		private GameObject
			eventGo;

		// Use this for initialization
		void Start () {
		
			if (eventGo != null) {

				SpriteFactory.SetSprite (img, spriteName, false, true, false, LoadOver, LoadOver);

			} else {

				SpriteFactory.SetSprite (img, spriteName, false, true, false, null, null);
			}
		}

		void OnEnable(){
#if USE_ASSETBUNDLE

#else
			if(eventGo != null){
				
				LoadOver();
			}
#endif
		}

		private void LoadOver () {

			SuperFunction.Instance.DispatchEvent (eventGo, LOAD_OVER_EVENT);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}