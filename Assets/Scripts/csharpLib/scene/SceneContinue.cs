using UnityEngine;
using System.Collections;
using System;

namespace scene{

	public class SceneContinue : MonoBehaviour {

		private Camera renderCamera;

		private Bounds[] bounds = new Bounds[3];

		private GameObject copy;

		private Vector3 recPos;

		public void SetCopy(GameObject _go){

			copy = _go;
		}

		public void SetCamera(Camera _camera){

			renderCamera = _camera;
		}

		// Use this for initialization
		void Awake(){

			MeshRenderer[] rs = gameObject.GetComponentsInChildren<MeshRenderer>();

			Bounds b = rs[0].bounds;

			for(int i = 1 ; i < rs.Length ; i++){

				b.Encapsulate(rs[i].bounds);
			}

			bounds[0] = b;

			bounds[1] = new Bounds(new Vector3(b.center.x + b.size.x,b.center.y,b.center.z),b.size);
			
			bounds[2] = new Bounds(new Vector3(b.center.x - b.size.x,b.center.y,b.center.z),b.size);

			recPos = transform.position;
		}

		void Update() {

			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(renderCamera);

			if(!GeometryUtility.TestPlanesAABB(planes, GetBounds(bounds[0]))){//看不到中间那个了

				if (GeometryUtility.TestPlanesAABB(planes, GetBounds(bounds[1]))){//看得到右面那个  那我把中间那个移动到右面 把右面那个隐藏

					transform.position = transform.position + new Vector3(bounds[0].size.x,0,0);

					copy.SetActive(false);

				}else if (GeometryUtility.TestPlanesAABB(planes, GetBounds(bounds[2]))){//看得到左面那个  那我把中间那个移动到左面 把左面那个隐藏

					transform.position = transform.position - new Vector3(bounds[0].size.x,0,0);

					copy.SetActive(false);

				}else{//左右都看不到了  怎么玩

					SuperDebug.LogError("See nothing!");
				}

			}else{//看得到中间那个

				bool getRight = GeometryUtility.TestPlanesAABB(planes, GetBounds(bounds[1]));

				bool getLeft = GeometryUtility.TestPlanesAABB(planes, GetBounds(bounds[2]));

				if(getRight && getLeft){

					SuperDebug.LogError("See both left and right!");

				}else if (getRight){//看得到右面那个

					if(!copy.activeSelf){

						copy.transform.position = transform.position + new Vector3(bounds[0].size.x,0,0);

						copy.SetActive(true);
					}
					
				}else if (getLeft){//看得到左面那个

					if(!copy.activeSelf){

						copy.transform.position = transform.position - new Vector3(bounds[0].size.x,0,0);
						
						copy.SetActive(true);
					}

				}else{//左右都看不到

					if(copy.activeSelf){

						copy.SetActive(false);
					}
				}
			}
		}

		void OnDestroy(){

			GameObject.Destroy(copy);
		}

		private Bounds GetBounds(Bounds _bounds){

			return new Bounds(_bounds.center + transform.position - recPos,_bounds.size);
		}

		public float mapWidth{

			get{

				return bounds[0].size.x;
			}
		}
	}
}
