using UnityEngine;
using System.Collections;
using System;
using superTween;

namespace heroFactory{

	public class HeroControllerDispatcher : MonoBehaviour {

		public void DispatchAnimationEvent(string _eventName)
		{
			string[] strs = _eventName.Split('$');

			SendMessageUpwards("DispatchAnimationEventHandler",strs,SendMessageOptions.DontRequireReceiver);
		}

		public void DispatchAnimationEventInstant(string _eventName){

			DispatchAnimationEvent(_eventName);
		}

//		void Update()
//		{
//			AnimatorClipInfo[] infos = animator.GetCurrentAnimatorClipInfo(0);
//
//			if(infos.Length == 0){
//				
//				return;
//			}
//
//			AnimationClip clip = infos[0].clip;
//				
//			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
//
//			AnimationEvent[] events = clip.events;
//
//			for(int i = 0 ; i < events.Length ; i++){
//
//				AnimationEvent ev = events[i];
//
//				if(ev.functionName.Equals("DispatchAnimationEventInstant")){
//
//					float time0 = info.normalizedTime * info.length;
//
//					float time1 = time0 + Time.deltaTime * info.speed;
//
//					if(ev.time > time0 && ev.time <= time1){
//
//						DispatchAnimationEvent(ev.stringParameter);
//					}
//				}
//			}
//		}
	}
}