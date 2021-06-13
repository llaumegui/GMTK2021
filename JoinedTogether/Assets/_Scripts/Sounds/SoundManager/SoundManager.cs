using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager
{

	static GameObject _oneShotGameObject;
	static AudioSource _oneShotAudioSource;


	public enum Sound //donner les catégories des sons (ex. Jump; Wind; Punch...)
	{
		Explosion,
		Checkpoint,
		Walk,
		Projection,
		Momentum,
		OnLand,
	}

	public static void PlayLoop(string name, Sound sound, float volume = 1f, bool isMusic = false, bool spacialized = false, Vector3 position = new Vector3(), float maxDistance = 100f)
	{
		GameObject loop = new GameObject(name);
		AudioSource sourceLoop = loop.AddComponent<AudioSource>();

		if (isMusic)
		{
			sourceLoop.outputAudioMixerGroup = SoundAssets.i.MusicGroup;
		}
		else
		{
			sourceLoop.outputAudioMixerGroup = SoundAssets.i.SfxGroup;
		}
		sourceLoop.loop = true;
		sourceLoop.volume = volume;
		sourceLoop.clip = GetAudioClip(sound);

		SoundAssets.i.AudioLoops.Add(loop);


		if(spacialized)
		{
			sourceLoop.spatialBlend = 1;
			sourceLoop.rolloffMode = AudioRolloffMode.Custom;
			sourceLoop.SetCustomCurve(AudioSourceCurveType.CustomRolloff, SoundAssets.i.SpacializedCurve);
			sourceLoop.transform.position = position;
			sourceLoop.maxDistance = maxDistance;

			SoundAssets.i.spacializedGizmos.Add(position, maxDistance);
		}

		sourceLoop.Play();
	}


	public static void PlaySound(Sound sound, float volume = 1f) //not Spacialized
	{
		if (_oneShotGameObject == null)
		{
			_oneShotGameObject = new GameObject("OneShotSound");
			_oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();

			_oneShotAudioSource.outputAudioMixerGroup = SoundAssets.i.SfxGroup;
		}

		_oneShotAudioSource.bypassEffects = false;
		_oneShotAudioSource.PlayOneShot(GetAudioClip(sound), volume);
	}

	public static AudioClip GetAudioClip(Sound sound)
	{
		List<AudioClip> audioClips = new List<AudioClip>();
		foreach(SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClips)
		{
			if(soundAudioClip.sound == sound)
			{
				audioClips.Add(soundAudioClip.audioClip);
			}
		}
		return audioClips[Random.Range(0,audioClips.Count)];
	}
}
