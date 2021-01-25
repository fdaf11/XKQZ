using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class NsSharedManager : MonoBehaviour
{
	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060016C2 RID: 5826 RVA: 0x0000E99F File Offset: 0x0000CB9F
	public static NsSharedManager inst
	{
		get
		{
			if (NsSharedManager._inst == null)
			{
				NsSharedManager._inst = NcEffectBehaviour.GetRootInstanceEffect().AddComponent<NsSharedManager>();
			}
			return NsSharedManager._inst;
		}
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x000BC9D0 File Offset: 0x000BABD0
	public GameObject GetSharedParticleGameObject(GameObject originalParticlePrefab)
	{
		int num = this.m_SharedPrefabs.IndexOf(originalParticlePrefab);
		if (num >= 0 && !(this.m_SharedGameObjects[num] == null))
		{
			return this.m_SharedGameObjects[num];
		}
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(originalParticlePrefab);
		gameObject.transform.parent = NcEffectBehaviour.GetRootInstanceEffect().transform;
		if (0 <= num)
		{
			this.m_SharedGameObjects[num] = gameObject;
		}
		else
		{
			this.m_SharedPrefabs.Add(originalParticlePrefab);
			this.m_SharedGameObjects.Add(gameObject);
		}
		NcParticleSystem component = gameObject.GetComponent<NcParticleSystem>();
		if (component)
		{
			component.enabled = false;
		}
		if (gameObject.particleEmitter)
		{
			gameObject.particleEmitter.emit = false;
			gameObject.particleEmitter.useWorldSpace = true;
			ParticleAnimator component2 = gameObject.GetComponent<ParticleAnimator>();
			if (component2)
			{
				component2.autodestruct = false;
			}
		}
		NcParticleSystem component3 = gameObject.GetComponent<NcParticleSystem>();
		if (component3)
		{
			component3.m_bBurst = false;
		}
		ParticleSystem component4 = gameObject.GetComponent<ParticleSystem>();
		if (component4)
		{
			component4.enableEmission = false;
		}
		return gameObject;
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x000BCB08 File Offset: 0x000BAD08
	public void EmitSharedParticleSystem(GameObject originalParticlePrefab, int nEmitCount, Vector3 worldPos)
	{
		GameObject sharedParticleGameObject = this.GetSharedParticleGameObject(originalParticlePrefab);
		if (sharedParticleGameObject == null)
		{
			return;
		}
		sharedParticleGameObject.transform.position = worldPos;
		if (sharedParticleGameObject.particleEmitter != null)
		{
			sharedParticleGameObject.particleEmitter.Emit(nEmitCount);
		}
		else
		{
			ParticleSystem component = sharedParticleGameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Emit(nEmitCount);
			}
		}
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x000BCB74 File Offset: 0x000BAD74
	public AudioSource GetSharedAudioSource(AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		int num = this.m_SharedAudioClip.IndexOf(audioClip);
		if (num >= 0)
		{
			foreach (AudioSource audioSource in this.m_SharedAudioSources[num])
			{
				if (audioSource.volume == fVolume && audioSource.pitch == fPitch && audioSource.loop == bLoop && audioSource.priority == nPriority)
				{
					return audioSource;
				}
			}
			return this.AddAudioSource(this.m_SharedAudioSources[num], audioClip, nPriority, bLoop, fVolume, fPitch);
		}
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		List<AudioSource> list = new List<AudioSource>();
		this.m_SharedAudioClip.Add(audioClip);
		this.m_SharedAudioSources.Add(list);
		return this.AddAudioSource(list, audioClip, nPriority, bLoop, fVolume, fPitch);
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x000BCC70 File Offset: 0x000BAE70
	private AudioSource AddAudioSource(List<AudioSource> sourceList, AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		sourceList.Add(audioSource);
		audioSource.clip = audioClip;
		audioSource.priority = nPriority;
		audioSource.loop = bLoop;
		audioSource.volume = fVolume;
		audioSource.pitch = fPitch;
		audioSource.playOnAwake = false;
		return audioSource;
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x000BCCC0 File Offset: 0x000BAEC0
	public void PlaySharedAudioSource(bool bUniquePlay, AudioClip audioClip, int nPriority, bool bLoop, float fVolume, float fPitch)
	{
		AudioSource sharedAudioSource = this.GetSharedAudioSource(audioClip, nPriority, bLoop, fVolume, fPitch);
		if (sharedAudioSource == null)
		{
			return;
		}
		if (sharedAudioSource.isPlaying)
		{
			if (bUniquePlay)
			{
				return;
			}
			sharedAudioSource.Stop();
		}
		sharedAudioSource.Play();
	}

	// Token: 0x04001AF5 RID: 6901
	protected static NsSharedManager _inst;

	// Token: 0x04001AF6 RID: 6902
	protected List<GameObject> m_SharedPrefabs = new List<GameObject>();

	// Token: 0x04001AF7 RID: 6903
	protected List<GameObject> m_SharedGameObjects = new List<GameObject>();

	// Token: 0x04001AF8 RID: 6904
	protected List<AudioClip> m_SharedAudioClip = new List<AudioClip>();

	// Token: 0x04001AF9 RID: 6905
	protected List<List<AudioSource>> m_SharedAudioSources = new List<List<AudioSource>>();
}
