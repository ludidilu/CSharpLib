using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

public class CreateIOSDummyScene {

	private static IOSDummyDic dic;

	private const string TAG = "IOSDummy";

	[MenuItem("打包前准备工作/构造ios空场景")]
	public static void Start(){

        EditorSceneManager.OpenScene("Assets/scenes/iosDummy.unity");

		GameObject[] gos = GameObject.FindGameObjectsWithTag(TAG);

		for(int i = 0 ; i < gos.Length ; i++){

			GameObject.DestroyImmediate(gos[i]);
		}

		GameObject go = new GameObject("dic");

		go.tag = TAG;

		dic = go.AddComponent<IOSDummyDic>();

		dic.list.Clear();

		string[] strs = AssetDatabase.FindAssets("t:prefab");
		
		for(int i = 0 ; i < strs.Length ; i++){
			
			string path = AssetDatabase.GUIDToAssetPath(strs[i]);
			
			bool skip = false;
			
			foreach(string kk in CodeHotFixTools.ignorePaths){
				
				if(path.IndexOf(kk) != -1){
					
					skip = true;
					
					break;
				}
			}
			
			if(!skip){
				
				go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				
				FixPrefab(go);
			}
		}

        EditorSceneManager.SaveOpenScenes();

		Debug.Log("IOSDummy场景构造完毕!!!");
	}

	private static void FixPrefab(GameObject _go){

		Component[] components = _go.GetComponents<Component>();

		for(int i = 0 ; i < components.Length ; i++){

			Component component = components[i];

			if(component == null){

				continue;
			}

			if(!(component is MonoBehaviour) && !(component is Transform)){

				Type type = component.GetType();

				if(!Check(type)){

					GameObject go = new GameObject(type.ToString());

					go.tag = TAG;

					Component c = go.AddComponent(type);

					dic.list.Add(c);

					if(component is Animator){

						(c as Animator).runtimeAnimatorController = GetAnimatorController();
					}
				}


			}
		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			FixPrefab(_go.transform.GetChild(i).gameObject);
		}
	}

	private static bool Check(Type _type){

		for(int i = 0 ; i < dic.list.Count ; i++){

			if(dic.list[i].GetType().Equals(_type)){

				return true;
			}
		}

		return false;
	}

	private static RuntimeAnimatorController GetAnimatorController(){

		string[] strs = AssetDatabase.FindAssets("t:animatorcontroller");

		string path = AssetDatabase.GUIDToAssetPath(strs[0]);
		
		RuntimeAnimatorController animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);

		return animator;
	}
}
