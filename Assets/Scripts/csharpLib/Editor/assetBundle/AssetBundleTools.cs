using UnityEngine;
using System.Collections;
using UnityEditor;

using System;
using assetManager;
using System.Collections.Generic;
using assetBundleManager;
using System.IO;
using System.Reflection;
using UnityEngine.Rendering;
using System.Threading;

public class AssetBundleTools{

	private static readonly BuildAssetBundleOptions BUILD_OPTION = BuildAssetBundleOptions.ForceRebuildAssetBundle;

//	private static readonly BuildAssetBundleOptions BUILD_OPTION = BuildAssetBundleOptions.UncompressedAssetBundle;

	[MenuItem("AssetBundle/清除所有选中对象的AssetBundle名字")]
	public static void ClearSelectedAssetBundleName(){

		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);

			SetAssetBundleName(path,null);
		}

		AssetDatabase.RemoveUnusedAssetBundleNames();
	}

	[MenuItem("AssetBundle/设置所有选中对象的AssetBundle名字")]
	public static void SetSelectedAssetBundleName(){
		
		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);

			SetAssetBundleName(path,obj.name);
		}
	}

	public static string GetAssetBundleName(string _path){

		AssetImporter importer = AssetImporter.GetAtPath(_path);

		return importer.assetBundleName;
	}

	public static void SetAssetBundleName(string _path,string _name){

		AssetImporter importer = AssetImporter.GetAtPath(_path);

		importer.assetBundleName = _name;
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:PC")]
	public static void CreateAssetBundlePC(){

//		CreateAssetBundleDat(null);

		AssetBundleManifest manifest = CreateAssetBundle(BUILD_OPTION,BuildTarget.StandaloneWindows64);
		
		CreateAssetBundleDat(manifest,BUILD_OPTION,BuildTarget.StandaloneWindows64);

		Debug.Log("AssetBundle生成成功！PC");
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:IOS")]
	public static void CreateAssetBundleIOS(){
		
		AssetBundleManifest manifest = CreateAssetBundle(BUILD_OPTION,BuildTarget.iOS);
		
		CreateAssetBundleDat(manifest,BUILD_OPTION,BuildTarget.iOS);

		Debug.Log("AssetBundle生成成功！IOS");
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:Android")]
	public static void CreateAssetBundleAndroid(){

		AssetBundleManifest manifest = CreateAssetBundle(BUILD_OPTION,BuildTarget.Android);

		CreateAssetBundleDat(manifest,BUILD_OPTION,BuildTarget.Android);

		Debug.Log("AssetBundle生成成功！Android");
	}

	private static void PrepareToBuildAssetBundle(){

		DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/assetbundle");

		if(!directoryInfo.Exists){

			directoryInfo.Create();

		}else{

			FileInfo[] fileInfos = directoryInfo.GetFiles();

			foreach(FileInfo fileInfo in fileInfos){

				fileInfo.Delete();
			}
		}
	}

	private static AssetBundleManifest CreateAssetBundle(BuildAssetBundleOptions _option,BuildTarget _buildTarget){
		
		PrepareToBuildAssetBundle();

		RenderSettings.fog = true;
		
		RenderSettings.fogMode = FogMode.Linear;

		LightmapData[] lightMaps = new LightmapData[1];

		lightMaps[0] = new LightmapData();

		lightMaps[0].lightmapLight = new Texture2D(100,100);

		LightmapSettings.lightmaps = lightMaps;

		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,_option,_buildTarget);

		RenderSettings.fog = false;

		LightmapSettings.lightmaps = new LightmapData[0];

		return manifest;
	}

	private static void CreateAssetBundleDat(AssetBundleManifest manifest,BuildAssetBundleOptions _buildOptions,BuildTarget _buildTarget){

		if(manifest == null){

			return;
		}

		string[] abs = manifest.GetAllAssetBundles ();
		
		AssetBundle[] aaaa = new AssetBundle[abs.Length];
		
		try{
			
			List<string> assetNames = new List<string> ();
			
			List<string> assetBundleNames = new List<string> ();

			Dictionary<string,List<string>> result = new Dictionary<string, List<string>> ();
			
			for(int i = 0 ; i < abs.Length ; i++){

                //AssetBundle ab = LoadAssetBundle("file:///" + Application.streamingAssetsPath + "/" + AssetBundleManager.path + abs[i]);

                AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + AssetBundleManager.path + abs[i]);

                aaaa[i] = ab;
				
				string[] strs = ab.GetAllAssetNames();

                for (int m = 0; m < strs.Length; m++)
                {
                    string str = strs[m];
					
					if(assetNames.Contains(str)){
						
						SuperDebug.LogError("error!");
						
					}else{
						
						assetNames.Add(str);
						
						assetBundleNames.Add(abs[i]);

						List<string> ll = new List<string>();
						
						result.Add(str,ll);
					}
				}
			}
			
			for (int i = 0; i < assetNames.Count; i++) {
				
				string assetName = assetNames[i];
				string abName = assetBundleNames[i];
				List<string> list = result[assetName];

                string[] strs = AssetDatabase.GetDependencies(assetName);

                for (int m = 0; m < strs.Length; m++)
                {
                    string tmpAssetName = strs[m].ToLower();

                    if(tmpAssetName != assetName)
                    {
                        int index = assetNames.IndexOf(tmpAssetName);

                        if(index != -1)
                        {
                            string assetBundleName = assetBundleNames[index];

                            if (assetBundleName != abName && !list.Contains(assetBundleName))
                            {

                                list.Add(assetBundleName);
                            }
                        }
                    }
                }
			}

			FileInfo fi = new FileInfo(Application.streamingAssetsPath + "/" + AssetManager.dataName);

			if(fi.Exists){

				fi.Delete();
			}

			FileStream fs = fi.Create();

			BinaryWriter bw = new BinaryWriter(fs);

			AssetManagerDataFactory.SetData(bw,assetNames,assetBundleNames,result);

			fs.Flush();

			bw.Close();

			fs.Close();

			fs.Dispose();

		}catch(Exception e){
			
			Debug.Log("error:" + e.Message);
			
		}finally{
			
			foreach (AssetBundle aaa in aaaa) {
				
				aaa.Unload (true);
			}
		}
	}

	private static AssetBundle LoadAssetBundle(string _path){

		using(WWW www = new WWW(_path)){
		
			while(www.progress < 1){
				
				Thread.Sleep(0);
			}
			
			AssetBundle ab = www.assetBundle;

			return ab;
		}
	}

	[MenuItem("AssetBundle/清除所有AssetBundle设置  千万不要乱点！！！")]
	public static void ClearAllAssetBundleName(){
		
		string[] names = AssetDatabase.GetAllAssetBundleNames();
		
		foreach(string assetBundleName in names){
			
			AssetDatabase.RemoveAssetBundleName(assetBundleName,true);
		}

		AssetDatabase.RemoveUnusedAssetBundleNames();
	}
}
