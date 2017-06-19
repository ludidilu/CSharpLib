using UnityEngine;
using System.Collections;
using gameObjectFactory;
using publicTools;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using superFunction;
using animatorFactoty;
using localData;

namespace heroFactory{

	//这个组件是绑在英雄身上的
	public class HeroController : MonoBehaviour
	{
		private const string CENTER_BONE_NAME = "Bip01";

		private Animator[] animators;

		public GameObject body;
		private Material bodyMaterial;

		public GameObject horse;
		private Material horseMaterial;

		public GameObject wing;
		private Material wingMaterial;

		public GameObject mainHandWeaponContainer;
		public GameObject offHandWeaponContainer;

		public GameObject[] particles;

		public GameObject mainHandWeapon;
		private Material mainHandWeaponMaterial;
		public GameObject[] mainHandWeaponParticles;

		public GameObject offHandWeapon;
		private Material offHandWeaponMaterial;
		public GameObject[] offHandWeaponParticles;

        private GameObject shadow;
        private Material shadowMat;

		private Transform centerBone;

        public GameObject Shadow
        {
            get { return shadow; }
            set { 
                shadow = value;

                if (shadow != null)
                {
                    shadowMat = shadow.GetComponent<Renderer>().material;
                }
                else
                {
                    shadowMat = null;
                }

            }
        }

        private bool isRandomHead = true;

		public string state = "wait";

        public string playOverState = "wait";

        private float curSpeed;

        public List<string> stateList = new List<string>();

		private bool notPlayWaitWhenPoseEnd;

		private float m_Alpha = 1;

		private Color m_Color = Color.white;

		public float Alpha
		{
			get {

				return m_Alpha;
			}

			set { 

				SetMaterialAlpha(bodyMaterial,value);

				if(horseMaterial != null){

					SetMaterialAlpha(horseMaterial,value);
				}

				if(wingMaterial != null){
					
					SetMaterialAlpha(wingMaterial,value);
				}

				if(mainHandWeaponMaterial != null){
					
					SetMaterialAlpha(mainHandWeaponMaterial,value);
				}

				if(offHandWeaponMaterial != null){
					
					SetMaterialAlpha(offHandWeaponMaterial,value);
				}

                if (shadowMat != null)
                {
                    shadowMat.SetFloat("_Alpha", value);
                }

				SetAlpha(value);

				m_Alpha = value;
			}
		}

		public Color Color
		{
			get {
				
				return m_Color;
			}
			
			set { 
				
				SetMaterialColor(bodyMaterial,value);
				
				if(horseMaterial != null){
					
					SetMaterialColor(horseMaterial,value);
				}
				
				if(wingMaterial != null){
					
					SetMaterialColor(wingMaterial,value);
				}
				
				if(mainHandWeaponMaterial != null){
					
					SetMaterialColor(mainHandWeaponMaterial,value);
				}
				
				if(offHandWeaponMaterial != null){
					
					SetMaterialColor(offHandWeaponMaterial,value);
				}

				m_Color = value;
			}
		}

		private const string showOutlineKey = "CharOutline";

		//0:关，1：开，-1：未知
		static int s_outlineOpen = -1;

		public static int OutlineOpen{

			get{

				if(s_outlineOpen == -1){

					if(LocalSettingData.HasKey(showOutlineKey)){

						s_outlineOpen = 1;

						LocalSettingData.SetInt(showOutlineKey,s_outlineOpen);

					}else{

						s_outlineOpen = LocalSettingData.GetInt(showOutlineKey);
					}
				}

				return s_outlineOpen;
			}

			set{

				s_outlineOpen = value;

				LocalSettingData.SetInt(showOutlineKey,s_outlineOpen);
			}
		}

		static Shader shaderHero;
		static Shader shaderHero2;
		static Shader shaderWeapon;
		static Shader shaderWeapon2;

		public void SetOutlineToggle(bool show){
			//TODO 删除本行
			show = false;

			Material[] mats = new Material[5];
			mats [0] = bodyMaterial;
			mats [1] = horseMaterial;
			mats [2] = wingMaterial;
			mats [3] = mainHandWeaponMaterial;
			mats [4] = offHandWeaponMaterial;
			for (int i = 0; i < mats.Length; i++) {
				if(mats[i] == null)
					continue;

				if(show){
					if(mats[i].shader.name == "Custom/Hero"){
						if(shaderHero2 == null)
							shaderHero2 = Shader.Find ("Custom/Hero2");
						mats[i].shader = shaderHero2;
					}
					else if(mats[i].shader.name == "Custom/WeaponWithLightningMove"){
						if(shaderWeapon2 == null)
							shaderWeapon2 = Shader.Find ("Custom/WeaponWithLightningMove2");
						mats[i].shader = shaderWeapon2;
					}
				}
				else{
					if(mats[i].shader.name == "Custom/Hero2"){
						if(shaderHero == null)
							shaderHero = Shader.Find ("Custom/Hero");
						mats[i].shader = shaderHero;
					}
					else if(mats[i].shader.name == "Custom/WeaponWithLightningMove2"){
						if(shaderWeapon == null)
							shaderWeapon = Shader.Find ("Custom/WeaponWithLightningMove");
						mats[i].shader = shaderWeapon;
					}
				}
			}
		}

		private void SetMaterialAlpha(Material _material,float _alpha){

			if (_material.shader.name == "Custom/Hero2") {
				if (shaderHero == null)
					shaderHero = Shader.Find ("Custom/Hero");
				_material.shader = shaderHero;
			} else if (_material.shader.name == "Custom/WeaponWithLightningMove2") {
				if (shaderWeapon == null)
					shaderWeapon = Shader.Find ("Custom/WeaponWithLightningMove");
				_material.shader = shaderWeapon;
			}

			_material.SetFloat("_AlphaXXX", _alpha);
			
			if (_alpha < 1 && Alpha == 1)
			{
				_material.renderQueue = 3000;
				_material.SetInt("_ZWrite", 0);
				
				_material.SetInt("_SrcAlpha", (int)BlendMode.SrcAlpha);
				_material.SetInt("_TarAlpha", (int)BlendMode.OneMinusSrcAlpha);
			}
			else if(_alpha == 1 && Alpha < 1)
			{
				_material.renderQueue = 2000;
				_material.SetInt("_ZWrite", 1);
				
				_material.SetInt("_SrcAlpha", (int)BlendMode.One);
				_material.SetInt("_TarAlpha", (int)BlendMode.Zero);
			}
		}

		private void SetMaterialColor(Material _material,Color _color){

			_material.SetColor("_Color",_color);
		}

		protected virtual void SetAlpha(float _alpha){


		}
		
		public void Init(){

			GameObject tmpGo = PublicTools.FindChildForce(gameObject,CENTER_BONE_NAME);

			if(tmpGo != null){

				centerBone = tmpGo.transform;

			}else{

				SuperDebug.Log("HeroController can not find " + CENTER_BONE_NAME + ".   GameObject name:" + gameObject.name);
			}
			
			animators = gameObject.GetComponentsInChildren<Animator>();

			for (int i = 0; i < animators.Length; i++) {
				
				if(animators[i].runtimeAnimatorController != null){
					
					AnimatorFactory.Instance.AddUseNum(animators[i].runtimeAnimatorController.name);
				}
			}
			
			bodyMaterial = body.GetComponent<Renderer>().material;
			
			if(horse != null){
				
				horseMaterial = horse.GetComponent<Renderer>().material;
			}
			
			if(wing != null){
				
				wingMaterial = wing.GetComponent<Renderer>().material;
			}
			
			if(mainHandWeapon != null){
				
				mainHandWeaponMaterial = mainHandWeapon.GetComponent<Renderer>().material;
			}
			
			if(offHandWeapon != null){
				
				offHandWeaponMaterial = offHandWeapon.GetComponent<Renderer>().material;
			}
			
			SetPartIndex(2);
			
			SetWeaponVisible(true);

			AddShadow();

			SetOutlineToggle (OutlineOpen == 1);
		}
		
		public void AddShadow()
		{
			GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/ShadowOne.prefab", ShadowLoadOK); 
		}
		
		private void ShadowLoadOK(GameObject _shadow){
			
			Shadow = _shadow;
			Shadow.transform.eulerAngles = new Vector3(90, 0, 0);
			Shadow.transform.SetParent(gameObject.transform, false);

			shadow.layer = gameObject.layer;
			//Shadow.layer = LayerMask.NameToLayer("UI");
		}

		private void SetPartIndex(int _index){

			bodyMaterial.SetInt("_PartIndex",_index);
		}

        public void ChangeHead(int _index)
        {
            isRandomHead = false;
            SetPartIndex(_index);
        }

        private void ChangeHeadRandom()
        {
            if (isRandomHead)
            {
                float ram = UnityEngine.Random.value;

                if (ram < 0.85f)
                {
                    SetPartIndex(2);
                    ResetTigger("zhucheng_wait01");
                    PlayAnim("zhucheng_wait", true);
                }
                else
                {
                    SetPartIndex(3);
                    ResetTigger("zhucheng_wait");
                    PlayAnim("zhucheng_wait01", true);
                }
            }
        }

		public void SetWeaponVisible(bool _visible){

			if(mainHandWeaponContainer != null){

				mainHandWeaponContainer.SetActive(_visible);
			}

			if(offHandWeaponContainer != null){

				offHandWeaponContainer.SetActive(_visible);
			}
		}

        public void ResetTigger(string name)
        {
			for (int i = 0; i < animators.Length; i++) {

				Animator animator = animators[i];

				if(animator.runtimeAnimatorController != null){
					
					animator.ResetTrigger(name);
				}
			}
        }

        public void ResetAllTrigger()
        {
			for (int i = 0; i < animators.Length; i++) {
				
				Animator animator = animators[i];
				
				if(animator.runtimeAnimatorController != null){

					for(int m = 0 ; m < stateList.Count ; m++){

						string _oldState = stateList[m];
	                
	                    animator.ResetTrigger(_oldState);
	                }
				}
            }

            stateList.Clear();
        }

		public void PlayAnim(string _state,bool _notPlayWaitWhenPoseEnd)
	    {
			ResetAllTrigger();

			state = _state;

            if (!stateList.Contains(state))
            {
                stateList.Add(state);
            }

			for(int i = 0 ; i < animators.Length ; i++){

				Animator animator = animators[i];

				animator.SetTrigger(state);
			}

			notPlayWaitWhenPoseEnd = _notPlayWaitWhenPoseEnd;
		}

		private void PlayAnimOver(){

            PlayAnim(playOverState, true);
		}

	    public void SetSpeed(float speed)
	    {
            curSpeed = speed;

			for(int i = 0 ; i < animators.Length ; i++){

				Animator animator = animators[i];

                animator.speed = speed;
			}
	    }

        public void IsPlay(bool b)
        {
			for(int i = 0 ; i < animators.Length ; i++){

				Animator animator = animators[i];

                if (b)
                {
                    if (curSpeed <= 0)
                    {
                        animator.speed = 1;
                    }
                    else
                    {
                        animator.speed = curSpeed;
                    }
                }
                else
                {
                    animator.speed = 0;
                }
            }
        }

		public void DispatchAnimationEventHandler(string[] _strs)
	    {
			string eventName = _strs[1];

			switch(eventName){

			case "poseStop":

				if(!notPlayWaitWhenPoseEnd){

					PlayAnimOver();
				}

				DispatchAnimationEventUpwards(eventName,_strs);

				break;

			case "SetPartIndex":

				SetPartIndex(int.Parse(_strs[2]));

				break;

			case "ChangeHeadRandom":

				ChangeHeadRandom();

				break;

			default:

				DispatchAnimationEventUpwards(eventName,_strs);

				break;
			}
	    }

		public void DispatchAnimationEventUpwards(string _eventName,string[] _strs){

			if(_strs.Length > 2){
				
				object[] tempData = new object[_strs.Length - 2];
				
				for (int i = 2; i < _strs.Length; i++)
				{
					tempData[i - 2] = _strs[i];
				}

				SuperFunction.Instance.DispatchEvent (gameObject, _eventName, tempData);

			}else{

				SuperFunction.Instance.DispatchEvent (gameObject, _eventName);
			}
		}

		void Update(){

			if(shadow != null && centerBone != null){

				shadow.transform.position = new Vector3(centerBone.position.x,shadow.transform.position.y,centerBone.position.z);
			}
		}

		void OnDestroy(){
			
			for (int i = 0; i < animators.Length; i++) {

				if(animators[i].runtimeAnimatorController != null){

					AnimatorFactory.Instance.DelUseNum(animators[i].runtimeAnimatorController.name);
				}
			}
		}
	}
}