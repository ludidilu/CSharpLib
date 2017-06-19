using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Events;
using superList;

public class AddScript : MonoBehaviour {

	public string scriptName;

	public int monoBehaviourInstanceID;

	[HideInInspector] public AddAtt[] atts;

	[HideInInspector] public Button[] buttons;

	[HideInInspector] public string[] buttonMethodNames;

	[HideInInspector] public RectTransform superScrollRectContent;

	[HideInInspector] public bool superScrollRectIsVertical;

	[HideInInspector] public bool superScrollRectIsHorizontal;

	[HideInInspector] public ScrollRect.MovementType superScrollRectMovementType;

	private Component script;

	private Type type;

	public Component Init(){
		
		type = AssemblyManager.Instance.assembly.GetType(scriptName);

		script = gameObject.AddComponent(type) as MonoBehaviour;

		if(scriptName == "effect.Gradient"){

			Outline outline = gameObject.GetComponent<Outline>();

			if(outline != null){

				Color effectColor = outline.effectColor;
				Vector2 effectDistance = outline.effectDistance;
				bool useGraphicAlpha = outline.useGraphicAlpha;

				GameObject.Destroy(outline);

				outline = gameObject.AddComponent<Outline>();

				outline.effectColor = effectColor;
				outline.effectDistance = effectDistance;
				outline.useGraphicAlpha = useGraphicAlpha;
			}

			Shadow shadow = gameObject.GetComponent<Shadow>();

			if(shadow != null){

				Color effectColor = shadow.effectColor;
				Vector2 effectDistance = shadow.effectDistance;
				bool useGraphicAlpha = shadow.useGraphicAlpha;
				
				GameObject.Destroy(shadow);
				
				shadow = gameObject.AddComponent<Shadow>();
				
				shadow.effectColor = effectColor;
				shadow.effectDistance = effectDistance;
				shadow.useGraphicAlpha = useGraphicAlpha;
			}
		}

		if(buttons != null){

			for(int i = 0 ; i < buttons.Length ; i++){

				UnityAction callBack = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction),script,buttonMethodNames[i]);
				
				buttons[i].onClick.AddListener(callBack);
			}
		}

		if(superScrollRectContent != null){

			PropertyInfo pi = type.GetProperty("content");
			
			pi.SetValue(script,superScrollRectContent,null);
			
			pi = type.GetProperty("vertical");
			
			pi.SetValue(script,superScrollRectIsVertical,null);
			
			pi = type.GetProperty("horizontal");
			
			pi.SetValue(script,superScrollRectIsHorizontal,null);
			
			pi = type.GetProperty("movementType");
			
			pi.SetValue(script,superScrollRectMovementType,null);
		}

		return script;
	}

	public void InitAtt(Dictionary<int,Component> _dic){
		
		for(int i = 0 ; i < atts.Length ; i++){
			
			atts[i].Init(script,type,_dic);
		}

		GameObject.Destroy(this);
	}
}
