using UnityEngine;
using System.Collections;
using superTween;

namespace effect{

	public class DelayShow : MonoBehaviour {
		
		[SerializeField]
		private float delayTime = 1.0f;

        private int index = -1;

		// Use this for initialization
		void Start () {		

			gameObject.SetActive(false);

            index = SuperTween.Instance.DelayCall(delayTime, DelayFunc);
		}
		
		void DelayFunc()
		{
			index = -1;

			gameObject.SetActive(true);
		}

        void OnDestroy()
        {
            if (index != -1)
            {
                SuperTween.Instance.Remove(index);
            }
        }
	}
}