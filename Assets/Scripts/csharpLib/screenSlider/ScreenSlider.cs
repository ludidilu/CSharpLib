using UnityEngine;
using System.Collections;
using superFunction;

namespace screenSlider{

	public enum SliderDirection
	{
		LEFT_TO_RIGHT,
		RIGHT_TO_LEFT,
	}

	public class ScreenSlider : MonoBehaviour {

		public const string SCREEN_SLIDER_EVENT = "SCREEN_SLIDER_EVENT";

		private const int CHECK_DISTANCE = 80;

		private static ScreenSlider _Instance;

		public static ScreenSlider Instance{

			get{

				if(_Instance == null){

					GameObject go = new GameObject("ScreenSliderGameObject");

					_Instance = go.AddComponent<ScreenSlider>();
				}

				return _Instance;
			}
		}

		private int m_isOpen = 1;

		public bool isOpen{

			set{

				m_isOpen += value ? 1 : -1;
			}

			get{

				return m_isOpen > 0;
			}
		}

		private float x;
		
		// Update is called once per frame
		void Update () {

			if(isOpen){
		
				if(Input.GetMouseButtonDown(0)){

					x = Input.mousePosition.x;
				}

				if(Input.GetMouseButtonUp(0)){

					float xOffset = Input.mousePosition.x - x;

					if (Mathf.Abs(xOffset) > CHECK_DISTANCE)
					{
						SliderDirection dir = xOffset > 0 ? SliderDirection.LEFT_TO_RIGHT : SliderDirection.RIGHT_TO_LEFT;

						SuperFunction.Instance.DispatchEvent(gameObject,SCREEN_SLIDER_EVENT,dir);
					}         
				}
			}
		}
	}
}