using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace effect{

	[AddComponentMenu("UI/Effects/LightningMove")]
	public class LightningMove : MonoBehaviour {

		[SerializeField]
		private bool uMove;

		[SerializeField]
		private float moveSpeed = 1f;

		[SerializeField]
		private float strongFix = 1f;

		[SerializeField]
		private float delay;

		private Image image;
		private float fix;

		private bool isMoving;
		private float waitStartTime;

		private Sprite sp;

		// Use this for initialization
		void Start () {
		
			image = gameObject.GetComponentInChildren<Image> ();

			image.material = GameObject.Instantiate<Material> (image.material);

			image.material.SetFloat ("_StrongFix", strongFix);

			Init ();
		}

		private void Init(){

			if (image.sprite != null && image.sprite != sp) {//sprite一变就要重新计算一次  否则uv会不对

				sp = image.sprite;

				float uOffset = image.sprite.textureRect.x / image.sprite.texture.width;
				
				float vOffset = image.sprite.textureRect.y / image.sprite.texture.height;
				
				float uScale = image.sprite.texture.width / image.sprite.textureRect.width;
				
				float vScale = image.sprite.texture.height / image.sprite.textureRect.height;

				fix = 0.5f;

				if (uMove) {

					uScale = uScale * 0.5f;
					image.material.SetFloat ("_UFix", fix);

				} else {

					vScale = vScale * 0.5f;
					image.material.SetFloat ("_VFix", fix);
				}

				
				image.material.SetFloat ("_UOffset", uOffset);
				image.material.SetFloat ("_VOffset", vOffset);
				image.material.SetFloat ("_UScale", uScale);
				image.material.SetFloat ("_VScale", vScale);
			}
		}
		
		// Update is called once per frame
		void Update () {

			Init ();

			if(isMoving){
		
				fix = fix - Time.deltaTime * moveSpeed;

				if(uMove){

					image.material.SetFloat("_UFix",fix);

				}else{

					image.material.SetFloat("_VFix",fix);
				}

				if(fix < -0.5f){

					isMoving = false;

					waitStartTime = Time.time;
				}

			}else{

				if(Time.time - waitStartTime > delay){

					isMoving = true;

					fix = 0.5f;				
				}
			}

//			SuperDebug.Log("fix:" + fix + "  isMoving" + isMoving);
		}
	}
}
