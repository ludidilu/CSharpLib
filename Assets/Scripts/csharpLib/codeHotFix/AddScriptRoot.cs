using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddScriptRoot : MonoBehaviour {

	// Use this for initialization
	void Awake () {

		gameObject.SetActive(false);

		Dictionary<int,Component> dic = new Dictionary<int, Component>();
		
		FixGo(transform,dic);

		FixGoAtt(transform,dic);
		
		gameObject.SetActive(true);

		GameObject.Destroy(this);
	}
	
	private void FixGo(Transform _go,Dictionary<int,Component> _dic){

		AddScript[] scripts = _go.GetComponents<AddScript>();

		for(int i = 0 ; i < scripts.Length ; i++){

			Component mono = scripts[i].Init();

			_dic.Add(scripts[i].monoBehaviourInstanceID,mono);
		}
		
		for(int i = 0 ; i < _go.childCount ; i++){

			FixGo(_go.transform.GetChild(i),_dic);
		}
	}

	private void FixGoAtt(Transform _go,Dictionary<int,Component> _dic){

		AddScript[] scripts = _go.GetComponents<AddScript>();
		
		for(int i = 0 ; i < scripts.Length ; i++){

			scripts[i].InitAtt(_dic);
		}
		
		for(int i = 0 ; i < _go.childCount ; i++){
			
			FixGoAtt(_go.transform.GetChild(i),_dic);
		}
	}
}
