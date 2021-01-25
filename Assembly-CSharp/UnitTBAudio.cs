using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public class UnitTBAudio : MonoBehaviour
{
	// Token: 0x0600300A RID: 12298 RVA: 0x0001E572 File Offset: 0x0001C772
	public void Awake()
	{
		this.thisT = base.transform;
	}

	// Token: 0x0600300B RID: 12299 RVA: 0x0001E580 File Offset: 0x0001C780
	public void AddSelectSound(AudioClip clip)
	{
		if (!this.selectSoundList.Contains(clip))
		{
			this.selectSoundList.Add(clip);
		}
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x00175664 File Offset: 0x00173864
	public void PlaySelect()
	{
		if (this.selectSoundList.Count > 0)
		{
			if (this.goSelectAudio != null)
			{
				Object.Destroy(this.goSelectAudio);
			}
			int num = Random.Range(0, this.selectSoundList.Count);
			this.goSelectAudio = this.PlaySound(this.selectSoundList[num]);
		}
	}

	// Token: 0x0600300D RID: 12301 RVA: 0x001756C8 File Offset: 0x001738C8
	public void PlayMove()
	{
		if (this.moveAudioID >= 0)
		{
			return;
		}
		if (this.moveSound != null)
		{
			if (this.loopMoveSound)
			{
				this.moveAudioID = AudioManager.PlaySoundLoop(this.moveSound, this.thisT);
			}
			else
			{
				this.moveAudioID = AudioManager.PlaySound(this.moveSound, this.thisT);
			}
		}
	}

	// Token: 0x0600300E RID: 12302 RVA: 0x0001E59F File Offset: 0x0001C79F
	public void PlayMeleeAttack()
	{
		if (this.meleeAttackSound != null)
		{
			this.PlaySound(this.meleeAttackSound);
		}
	}

	// Token: 0x0600300F RID: 12303 RVA: 0x0001E5BF File Offset: 0x0001C7BF
	public void PlayRangeAttack()
	{
		if (this.rangeAttackSound != null)
		{
			this.PlaySound(this.rangeAttackSound);
		}
	}

	// Token: 0x06003010 RID: 12304 RVA: 0x0001E5DF File Offset: 0x0001C7DF
	public void PlayHit()
	{
		if (this.hitSound != null)
		{
			this.PlaySound(this.hitSound);
		}
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x0001E5FF File Offset: 0x0001C7FF
	public void PlayMissedRange()
	{
		if (this.missedSoundRange != null)
		{
			this.PlaySound(this.missedSoundRange);
		}
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x0001E61F File Offset: 0x0001C81F
	public void PlayMissedMelee()
	{
		if (this.missedSoundMelee != null)
		{
			this.PlaySound(this.missedSoundMelee);
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x0001E63F File Offset: 0x0001C83F
	public void PlayDestroy()
	{
		if (this.destroySound != null)
		{
			this.PlaySound(this.destroySound);
		}
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x0001E65F File Offset: 0x0001C85F
	public void StopMove()
	{
		if (this.moveAudioID < 0)
		{
			return;
		}
		AudioManager.StopSound(this.moveAudioID);
		this.moveAudioID = -1;
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x00175734 File Offset: 0x00173934
	public GameObject PlaySound(AudioClip clip)
	{
		float length = clip.length;
		GameObject gameObject = new GameObject("PlaySound");
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		gameObject.transform.position = this.thisT.position;
		audioSource.rolloffMode = 1;
		audioSource.minDistance = 20f;
		audioSource.playOnAwake = false;
		audioSource.volume = GameGlobal.m_fSoundValue;
		audioSource.ignoreListenerVolume = true;
		audioSource.clip = clip;
		audioSource.loop = false;
		audioSource.Play();
		Object.Destroy(gameObject, length);
		return gameObject;
	}

	// Token: 0x04003BD1 RID: 15313
	public AudioClip selectSound;

	// Token: 0x04003BD2 RID: 15314
	public bool loopMoveSound;

	// Token: 0x04003BD3 RID: 15315
	public AudioClip moveSound;

	// Token: 0x04003BD4 RID: 15316
	public AudioClip meleeAttackSound;

	// Token: 0x04003BD5 RID: 15317
	public AudioClip rangeAttackSound;

	// Token: 0x04003BD6 RID: 15318
	public AudioClip hitSound;

	// Token: 0x04003BD7 RID: 15319
	public AudioClip missedSoundRange;

	// Token: 0x04003BD8 RID: 15320
	public AudioClip missedSoundMelee;

	// Token: 0x04003BD9 RID: 15321
	public AudioClip destroySound;

	// Token: 0x04003BDA RID: 15322
	private Transform thisT;

	// Token: 0x04003BDB RID: 15323
	private GameObject goSelectAudio;

	// Token: 0x04003BDC RID: 15324
	private List<AudioClip> selectSoundList = new List<AudioClip>();

	// Token: 0x04003BDD RID: 15325
	private int moveAudioID = -1;
}
