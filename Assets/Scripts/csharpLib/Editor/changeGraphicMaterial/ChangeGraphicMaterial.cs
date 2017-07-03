using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ChangeGraphicMaterial {

    [MenuItem("Change Graphic material/Do")]
	public static void Start()
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/Prafab/base/UIDefault.mat");

        GameObject go = Selection.activeGameObject;

        Graphic[] images = go.GetComponentsInChildren<Graphic>(true);

        for (int i = 0; i < images.Length; i++)
        {
            if(images[i].material.shader.name == "UI/Default")
            {
                images[i].material = mat;
            }
        }

        EditorUtility.SetDirty(go);

        AssetDatabase.SaveAssets();
    }
}
