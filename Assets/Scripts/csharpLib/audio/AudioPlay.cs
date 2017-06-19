using UnityEngine;
using System.Collections;
using System;
using assetManager;
using localData;

namespace audio
{

	public class AudioPlay : MonoBehaviour
	{
		private static AudioPlay _Instance;

		public static AudioPlay Instance {

			get {

				if (_Instance == null) {

					GameObject go = new GameObject("AudioPlayGameObject");

					GameObject.DontDestroyOnLoad(go);

					_Instance = go.AddComponent<AudioPlay> ();
				}

				return _Instance;
			}
		}

		private AudioPlayScript script;

		private AudioSource musicSource;

		private const float defaultMusicVolumn = 0.6f;
		private const float defaultEffectVolumn = 0.5f;

        private float musicVolumn;
        private float effectVolumn;

        private const string MUSIC_PLAY_KEY = "MUSIC_PLAY_KEY";
        private const string MUSIC_PLAY_KEY_VOL = "MUSIC_PLAY_KEY_VOL";

		public bool isMusicPlay = true;

        private const string EFFECT_PLAY_KEY = "EFFECT_PLAY_KEY";
        private const string EFFECT_PLAY_KEY_VOL = "EFFECT_PLAY_KEY_VOL";

		public bool isEffectPlay = true;

        public float musicVol
        {
            get
            {
                return musicVolumn;
            }
            set
            {
                value = Mathf.Clamp(value, 0, 1);

                if (value != musicVolumn)
                {
                    LocalSettingData.SetFloat(MUSIC_PLAY_KEY_VOL,value);

                    musicVolumn = musicSource.volume = value;
                }
            }
        }
        public float effectVol
        {
            get
            {
                return effectVolumn;
            }
            set
            {
                value = Mathf.Clamp(value, 0, 1);

                if (value != effectVolumn)
                {
                    LocalSettingData.SetFloat(EFFECT_PLAY_KEY_VOL, value);

					script.SetPlayEffectVol(value);

                    effectVolumn = value;
                }
            }
        }

		void Awake()
		{
			musicSource = gameObject.AddComponent<AudioSource> ();

			musicSource.loop = true;

			musicSource.volume = musicVol;

			script = gameObject.AddComponent<AudioPlayScript> ();

			if(LocalSettingData.HasKey(MUSIC_PLAY_KEY)){

				isMusicPlay = LocalSettingData.GetInt(MUSIC_PLAY_KEY) == 1;
            }

            if (LocalSettingData.HasKey(MUSIC_PLAY_KEY_VOL))
            {

                musicVol = LocalSettingData.GetFloat(MUSIC_PLAY_KEY_VOL);

            }else{

				musicVol = defaultMusicVolumn;
			}

			if(LocalSettingData.HasKey(EFFECT_PLAY_KEY)){

				isEffectPlay = LocalSettingData.GetInt(EFFECT_PLAY_KEY) == 1;
			}

            if (LocalSettingData.HasKey(EFFECT_PLAY_KEY_VOL))
            {

                effectVol = LocalSettingData.GetFloat(EFFECT_PLAY_KEY_VOL);

            }else{

				effectVol = defaultEffectVolumn;
			}
		}

		public void PlayMusic (string _path)
		{
			if(musicSource.clip != null){

				if(musicSource.clip.name.Equals(_path)){

					return;
				}

				AudioFactory.Instance.RemoveClip(musicSource.clip);
			}

			AudioFactory.Instance.GetClip (_path, GetMusicClip, false);
		}

		private void GetMusicClip (AudioClip _clip)
		{
			musicSource.clip = _clip;

			if (isMusicPlay) {

				musicSource.Play ();
			}
		}

		public void PlayEffect (string _path)
		{
			if (isEffectPlay) {

				AudioFactory.Instance.GetClip (_path, GetEffectClip);
			}
		}

		private void GetEffectClip (AudioClip _clip)
		{
			script.PlayEffect (_clip);
		}

		public void SetIsMusicPlay (bool _b)
		{
			isMusicPlay = _b;

			LocalSettingData.SetInt(MUSIC_PLAY_KEY,isMusicPlay ? 1 : 0);

			if (!isMusicPlay) {

				if (musicSource.isPlaying) {

					musicSource.Stop ();
				}

			} else {

				if (musicSource.clip != null) {

					musicSource.Play ();
				}
			}
		}

		public void SetIsEffectPlay (bool _b)
		{
			isEffectPlay = _b;

			LocalSettingData.SetInt(EFFECT_PLAY_KEY,isEffectPlay ? 1 : 0);
		}

		public void SetPlayEffectSpeed(float _speed){

			script.SetPlayEffectSpeed(_speed);
		}
	}
}