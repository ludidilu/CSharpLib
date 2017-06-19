using UnityEngine;
using System.Collections;
using System;
using superFunction;

[Serializable]
public class EventToTrigger
{
    public string eventName;
    public string trigger;
}


public class AnimationEventReceiver : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private EventToTrigger[] binders;

	// Use this for initialization
	void Awake () {
	    
        for(int i = 0; i < binders.Length; i++)
        {
            EventToTrigger ett = binders[i];

			SuperFunction.SuperFunctionCallBack0 del = delegate (int _index)
            {
                Reset();

                animator.SetTrigger(ett.trigger);
            };

            SuperFunction.Instance.AddEventListener(target, ett.eventName, del);
        }
	}

    private void Reset()
    {
        for(int i = 0; i < binders.Length; i++)
        {
            animator.ResetTrigger(binders[i].trigger);
        }
    }
}
