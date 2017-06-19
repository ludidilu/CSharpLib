using UnityEngine;
using System.Collections;
using UnityEditor;

public class ClearLocalData{

	[MenuItem("本地数据/清空本地数据 清空后弱引导会重置 本地设置也会被清除")]
	public static void Start(){

		PlayerPrefs.DeleteAll();

		PlayerPrefs.Save();
	}
}
