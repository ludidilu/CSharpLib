using UnityEngine;
using System.Collections;
using superFunction;

public class AnimationEventDispatcher : MonoBehaviour {

    public void Dispatcher(string msg)
    {
        SuperFunction.Instance.DispatchEvent(gameObject, msg);
    }
	
}
