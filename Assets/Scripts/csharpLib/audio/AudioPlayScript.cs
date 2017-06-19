using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayScript : MonoBehaviour {

	private List<AudioSource> effectSources = new List<AudioSource>();

	private float pitch = 1;

	private float volume;

	public void PlayEffect(AudioClip _clip){

		AudioSource source = gameObject.AddComponent<AudioSource>();

		source.clip = _clip;

		source.pitch = pitch;

		source.volume = volume;

		source.Play();

		effectSources.Add(source);
	}

    public void SetPlayEffectVol(float _vol){

		volume = _vol;

		for(int i = 0; i < effectSources.Count ; i++){

			effectSources[i].volume = volume;
        }
    }

	public void SetPlayEffectSpeed(float _speed){

		pitch = _speed;

		for(int i = 0 ; i < effectSources.Count ; i++){

			effectSources[i].pitch = pitch;
		}
	}

	// Update is called once per frame
	void Update () {
	
		for(int i = effectSources.Count - 1 ; i > -1 ; i--){

			if(!effectSources[i].isPlaying){

				GameObject.Destroy(effectSources[i]);

				effectSources.RemoveAt(i);
			}
		}
	}
}
