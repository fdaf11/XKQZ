using System;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class NcAttachSound : NcEffectBehaviour
{
	// Token: 0x060016DE RID: 5854 RVA: 0x000BD2A0 File Offset: 0x000BB4A0
	public override int GetAnimationState()
	{
		if ((base.enabled && NcEffectBehaviour.IsActive(base.gameObject)) || (this.m_AudioSource != null && (this.m_AudioSource.isPlaying || NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)))
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
	public void Replay()
	{
		this.m_bStartAttach = false;
		this.m_bEnable = true;
		base.enabled = true;
		this.m_nCreateCount = 0;
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x0000EBE2 File Offset: 0x0000CDE2
	private void OnEnable()
	{
		if (this.m_bPlayOnActive)
		{
			this.Replay();
		}
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x000BD304 File Offset: 0x000BB504
	private void Update()
	{
		if (this.m_AudioClip == null)
		{
			base.enabled = false;
			return;
		}
		if (!this.m_bEnable)
		{
			return;
		}
		if (!this.m_bStartAttach)
		{
			this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
			this.m_bStartAttach = true;
		}
		if (this.m_fStartTime + ((this.m_nCreateCount != 0) ? this.m_fRepeatTime : this.m_fDelayTime) <= NcEffectBehaviour.GetEngineTime())
		{
			this.CreateAttachSound();
			if (0f < this.m_fRepeatTime && (this.m_nRepeatCount == 0 || this.m_nCreateCount < this.m_nRepeatCount))
			{
				this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
			}
			else
			{
				this.m_bEnable = false;
			}
		}
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000BD3CC File Offset: 0x000BB5CC
	public void CreateAttachSound()
	{
		if (this.m_PlayType == NcAttachSound.PLAY_TYPE.MultiPlay || !this.m_bSharedAudioSource)
		{
			if (this.m_AudioSource == null)
			{
				this.m_AudioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this.m_AudioSource.clip = this.m_AudioClip;
			this.m_AudioSource.priority = this.m_nPriority;
			this.m_AudioSource.loop = this.m_bLoop;
			this.m_AudioSource.volume = this.m_fVolume;
			this.m_AudioSource.pitch = this.m_fPitch;
			this.m_AudioSource.playOnAwake = false;
			this.m_AudioSource.Play();
		}
		else
		{
			NsSharedManager.inst.PlaySharedAudioSource(this.m_PlayType == NcAttachSound.PLAY_TYPE.UniquePlay, this.m_AudioClip, this.m_nPriority, this.m_bLoop, this.m_fVolume, this.m_fPitch);
		}
		this.m_nCreateCount++;
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x0000EBF5 File Offset: 0x0000CDF5
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fRepeatTime /= fSpeedRate;
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x0000EC13 File Offset: 0x0000CE13
	public override void OnSetReplayState()
	{
		base.OnSetReplayState();
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x0000EC1B File Offset: 0x0000CE1B
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.Replay();
	}

	// Token: 0x04001B12 RID: 6930
	public NcAttachSound.PLAY_TYPE m_PlayType;

	// Token: 0x04001B13 RID: 6931
	public bool m_bSharedAudioSource = true;

	// Token: 0x04001B14 RID: 6932
	public bool m_bPlayOnActive;

	// Token: 0x04001B15 RID: 6933
	public float m_fDelayTime;

	// Token: 0x04001B16 RID: 6934
	public float m_fRepeatTime;

	// Token: 0x04001B17 RID: 6935
	public int m_nRepeatCount;

	// Token: 0x04001B18 RID: 6936
	public AudioClip m_AudioClip;

	// Token: 0x04001B19 RID: 6937
	public int m_nPriority = 128;

	// Token: 0x04001B1A RID: 6938
	public bool m_bLoop;

	// Token: 0x04001B1B RID: 6939
	public float m_fVolume = 1f;

	// Token: 0x04001B1C RID: 6940
	public float m_fPitch = 1f;

	// Token: 0x04001B1D RID: 6941
	protected AudioSource m_AudioSource;

	// Token: 0x04001B1E RID: 6942
	protected float m_fStartTime;

	// Token: 0x04001B1F RID: 6943
	protected int m_nCreateCount;

	// Token: 0x04001B20 RID: 6944
	protected bool m_bStartAttach;

	// Token: 0x04001B21 RID: 6945
	protected bool m_bEnable = true;

	// Token: 0x020003C2 RID: 962
	public enum PLAY_TYPE
	{
		// Token: 0x04001B23 RID: 6947
		StopAndPlay,
		// Token: 0x04001B24 RID: 6948
		UniquePlay,
		// Token: 0x04001B25 RID: 6949
		MultiPlay
	}
}
