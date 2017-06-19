using UnityEngine;
using System.Collections;
using superFunction;

namespace superFunction{

	public class SuperFunctionControl : MonoBehaviour {

		public bool isDestroy = false;

		void OnDestroy(){

			if(!isDestroy){

				SuperFunction.Instance.DestroyGameObject(gameObject);
			}
		}
	}
}