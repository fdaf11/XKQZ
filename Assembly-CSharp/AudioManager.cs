using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200072F RID: 1839
[AddComponentMenu("TDTK/Optional/AudioManager")]
public class AudioManager : MonoBehaviour
{
	// Token: 0x06002B7A RID: 11130 RVA: 0x0001C078 File Offset: 0x0001A278
	public static void PlayGameWonSound()
	{
		if (AudioManager.instance == null)
		{
			return;
		}
		if (AudioManager.instance.gameWonSound != null)
		{
			AudioManager.PlaySound(AudioManager.instance.gameWonSound);
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x0001C0B0 File Offset: 0x0001A2B0
	public static void PlayGameLostSound()
	{
		if (AudioManager.instance == null)
		{
			return;
		}
		if (AudioManager.instance.gameLostSound != null)
		{
			AudioManager.PlaySound(AudioManager.instance.gameLostSound);
		}
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
	public static void PlayNewRoundSound(int round)
	{
		if (AudioManager.instance == null)
		{
			return;
		}
		if (AudioManager.instance.newRoundSound != null)
		{
			AudioManager.PlaySound(AudioManager.instance.newRoundSound);
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x0001C120 File Offset: 0x0001A320
	public static void PlayActionFailedSound()
	{
		if (AudioManager.instance == null)
		{
			return;
		}
		if (AudioManager.instance.actionFailedSound != null)
		{
			AudioManager.PlaySound(AudioManager.instance.actionFailedSound);
		}
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x00153888 File Offset: 0x00151A88
	private void Awake()
	{
		this.thisObj = base.gameObject;
		this.thisT = base.transform;
		AudioManager.camT = Camera.main.transform;
		if (this.playMusic && this.musicList != null && this.musicList.Length > 0)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "MusicSource";
			gameObject.transform.position = AudioManager.camT.position;
			gameObject.transform.parent = AudioManager.camT;
			this.musicSource = gameObject.AddComponent<AudioSource>();
			this.musicSource.loop = false;
			this.musicSource.playOnAwake = false;
			this.musicSource.volume = GameGlobal.m_fSoundValue;
			this.musicSource.ignoreListenerVolume = true;
			if (this.listenerT != null)
			{
				gameObject.transform.parent = this.listenerT;
				gameObject.transform.localPosition = Vector3.zero;
			}
			base.StartCoroutine(this.MusicRoutine());
		}
		AudioManager.audioObject = new AudioObject[20];
		for (int i = 0; i < AudioManager.audioObject.Length; i++)
		{
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "AudioSource" + i;
			AudioSource audioSource = gameObject2.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.minDistance = this.minFallOffRange;
			Transform transform = gameObject2.transform;
			transform.parent = this.thisObj.transform;
			AudioManager.audioObject[i] = new AudioObject(audioSource, transform);
		}
		AudioListener.volume = this.initialSFXVolume;
		if (AudioManager.instance == null)
		{
			AudioManager.instance = this;
		}
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x00153A40 File Offset: 0x00151C40
	public static void Init()
	{
		if (AudioManager.instance == null)
		{
			AudioManager.instance = new GameObject
			{
				name = "AudioManager"
			}.AddComponent<AudioManager>();
		}
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x00153A7C File Offset: 0x00151C7C
	public IEnumerator MusicRoutine()
	{
		for (;;)
		{
			if (this.shuffle)
			{
				this.musicSource.clip = this.musicList[Random.Range(0, this.musicList.Length)];
			}
			else
			{
				this.musicSource.clip = this.musicList[this.currentTrackID];
				this.currentTrackID++;
				if (this.currentTrackID == this.musicList.Length)
				{
					this.currentTrackID = 0;
				}
			}
			this.musicSource.Play();
			yield return new WaitForSeconds(this.musicSource.clip.length);
		}
		yield break;
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x0001C158 File Offset: 0x0001A358
	private void Start()
	{
		if (this.themeMusic != null)
		{
			this.themeID = AudioManager.PlaySoundLoop(this.themeMusic);
		}
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x0001C17C File Offset: 0x0001A37C
	private void OnEnable()
	{
		GameControlTB.onBattleEndE += this.OnGameOver;
		GameControlTB.onNewRoundE += AudioManager.PlayNewRoundSound;
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x0001C1A0 File Offset: 0x0001A3A0
	private void OnDisable()
	{
		GameControlTB.onBattleEndE -= this.OnGameOver;
		GameControlTB.onNewRoundE -= AudioManager.PlayNewRoundSound;
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x00153A98 File Offset: 0x00151C98
	private void OnGameOver(int vicFactionID)
	{
		if (this.themeID >= 0)
		{
			AudioManager.StopSound(this.themeID);
			this.themeID = -1;
		}
		if (GameControlTB.playerFactionExisted)
		{
			if (GameControlTB.IsPlayerFaction(vicFactionID))
			{
				AudioManager.PlayGameWonSound();
			}
			else
			{
				AudioManager.PlayGameLostSound();
			}
		}
		else
		{
			AudioManager.PlayGameWonSound();
		}
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x00153AF4 File Offset: 0x00151CF4
	private static int GetUnusedAudioObject()
	{
		for (int i = 0; i < AudioManager.audioObject.Length; i++)
		{
			if (!AudioManager.audioObject[i].inUse)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x00153B30 File Offset: 0x00151D30
	public static int PlaySound(AudioClip clip, Vector3 pos)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].thisT.position = pos;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = false;
		AudioManager.audioObject[unusedAudioObject].source.Play();
		float length = AudioManager.audioObject[unusedAudioObject].source.clip.length;
		AudioManager.instance.StartCoroutine(AudioManager.instance.ClearAudioObject(unusedAudioObject, length));
		return unusedAudioObject;
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x00153BDC File Offset: 0x00151DDC
	public static int PlaySound(AudioClip clip, Transform srcT)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].thisT.parent = srcT;
		AudioManager.audioObject[unusedAudioObject].thisT.localPosition = Vector3.zero;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = false;
		AudioManager.audioObject[unusedAudioObject].source.Play();
		float length = AudioManager.audioObject[unusedAudioObject].source.clip.length;
		AudioManager.instance.StartCoroutine(AudioManager.instance.ClearAudioObject(unusedAudioObject, length));
		return unusedAudioObject;
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x00153CA0 File Offset: 0x00151EA0
	public static int PlaySoundDelay(AudioClip clip, Transform srcT, float fdelay)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].thisT.parent = srcT;
		AudioManager.audioObject[unusedAudioObject].thisT.localPosition = Vector3.zero;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = false;
		AudioManager.audioObject[unusedAudioObject].source.PlayDelayed(fdelay);
		float duration = AudioManager.audioObject[unusedAudioObject].source.clip.length + fdelay;
		AudioManager.instance.StartCoroutine(AudioManager.instance.ClearAudioObject(unusedAudioObject, duration));
		return unusedAudioObject;
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x00153D64 File Offset: 0x00151F64
	public static int PlaySoundLoop(AudioClip clip, Transform srcT)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].thisT.parent = srcT;
		AudioManager.audioObject[unusedAudioObject].thisT.localPosition = Vector3.zero;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = true;
		AudioManager.audioObject[unusedAudioObject].source.Play();
		return unusedAudioObject;
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x00153DF8 File Offset: 0x00151FF8
	public static int PlaySound(AudioClip clip)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = false;
		AudioManager.audioObject[unusedAudioObject].source.Play();
		AudioManager.audioObject[unusedAudioObject].thisT.position = AudioManager.camT.position;
		float length = AudioManager.audioObject[unusedAudioObject].source.clip.length;
		AudioManager.instance.StartCoroutine(AudioManager.instance.ClearAudioObject(unusedAudioObject, length));
		return unusedAudioObject;
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x00153EAC File Offset: 0x001520AC
	public static int PlaySoundLoop(AudioClip clip)
	{
		if (AudioManager.instance == null)
		{
			AudioManager.Init();
		}
		int unusedAudioObject = AudioManager.GetUnusedAudioObject();
		AudioManager.audioObject[unusedAudioObject].inUse = true;
		AudioManager.audioObject[unusedAudioObject].source.clip = clip;
		AudioManager.audioObject[unusedAudioObject].source.loop = true;
		AudioManager.audioObject[unusedAudioObject].source.Play();
		return unusedAudioObject;
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x00153F18 File Offset: 0x00152118
	public static void StopSound(int ID)
	{
		AudioManager.audioObject[ID].inUse = false;
		AudioManager.audioObject[ID].source.Stop();
		AudioManager.audioObject[ID].source.clip = null;
		AudioManager.audioObject[ID].thisT.parent = AudioManager.instance.thisT;
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x00153F70 File Offset: 0x00152170
	private static IEnumerator SoundRoutine2D(int ID, float duration)
	{
		while (duration > 0f)
		{
			AudioManager.audioObject[ID].thisT.position = AudioManager.camT.position;
			yield return null;
		}
		AudioManager.instance.StartCoroutine(AudioManager.instance.ClearAudioObject(ID, 0f));
		yield break;
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x00153FA0 File Offset: 0x001521A0
	private IEnumerator ClearAudioObject(int ID, float duration)
	{
		yield return new WaitForSeconds(duration);
		AudioManager.audioObject[ID].inUse = false;
		AudioManager.audioObject[ID].thisT.parent = this.thisT;
		yield break;
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x0001C1C4 File Offset: 0x0001A3C4
	public static void SetSFXVolume(float val)
	{
		AudioListener.volume = val;
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0001C1CC File Offset: 0x0001A3CC
	public static void SetMusicVolume(float val)
	{
		if (AudioManager.instance && AudioManager.instance.musicSource)
		{
			AudioManager.instance.musicSource.volume = val;
		}
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x0001C201 File Offset: 0x0001A401
	public static float GetSFXVolume()
	{
		return AudioListener.volume;
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x0001C208 File Offset: 0x0001A408
	public static float GetMusicVolume()
	{
		if (AudioManager.instance && AudioManager.instance.musicSource)
		{
			return AudioManager.instance.musicSource.volume;
		}
		return AudioManager.instance.initialMusicVolume;
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x00153FD8 File Offset: 0x001521D8
	public static void SetMusicSourceToListener(Transform lisT)
	{
		AudioManager.instance.listenerT = lisT;
		if (AudioManager.instance.musicSource != null)
		{
			AudioManager.instance.musicSource.transform.parent = AudioManager.instance.listenerT;
			AudioManager.instance.musicSource.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x04003826 RID: 14374
	private static AudioObject[] audioObject;

	// Token: 0x04003827 RID: 14375
	public static AudioManager instance;

	// Token: 0x04003828 RID: 14376
	private static Transform camT;

	// Token: 0x04003829 RID: 14377
	public float minFallOffRange = 10f;

	// Token: 0x0400382A RID: 14378
	public AudioClip[] musicList;

	// Token: 0x0400382B RID: 14379
	public bool playMusic = true;

	// Token: 0x0400382C RID: 14380
	public bool shuffle;

	// Token: 0x0400382D RID: 14381
	public float initialMusicVolume = 0.5f;

	// Token: 0x0400382E RID: 14382
	private int currentTrackID;

	// Token: 0x0400382F RID: 14383
	private AudioSource musicSource;

	// Token: 0x04003830 RID: 14384
	private Transform listenerT;

	// Token: 0x04003831 RID: 14385
	public float initialSFXVolume = 0.75f;

	// Token: 0x04003832 RID: 14386
	public AudioClip gameWonSound;

	// Token: 0x04003833 RID: 14387
	public AudioClip gameLostSound;

	// Token: 0x04003834 RID: 14388
	public AudioClip newRoundSound;

	// Token: 0x04003835 RID: 14389
	public AudioClip themeMusic;

	// Token: 0x04003836 RID: 14390
	private int themeID = -1;

	// Token: 0x04003837 RID: 14391
	public AudioClip actionFailedSound;

	// Token: 0x04003838 RID: 14392
	private GameObject thisObj;

	// Token: 0x04003839 RID: 14393
	private Transform thisT;
}
