using System;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
[AddComponentMenu("NGUI/Tween/Tween Volume")]
[RequireComponent(typeof(AudioSource))]
public class TweenVolume : UITweener
{
	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000ECFF8 File Offset: 0x000EB1F8
	public AudioSource audioSource
	{
		get
		{
			if (this.mSource == null)
			{
				this.mSource = base.audio;
				if (this.mSource == null)
				{
					this.mSource = base.GetComponent<AudioSource>();
					if (this.mSource == null)
					{
						Debug.LogError("TweenVolume needs an AudioSource to work with", this);
						base.enabled = false;
					}
				}
			}
			return this.mSource;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06001F03 RID: 7939 RVA: 0x000149AC File Offset: 0x00012BAC
	// (set) Token: 0x06001F04 RID: 7940 RVA: 0x000149B4 File Offset: 0x00012BB4
	[Obsolete("Use 'value' instead")]
	public float volume
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001F05 RID: 7941 RVA: 0x000149BD File Offset: 0x00012BBD
	// (set) Token: 0x06001F06 RID: 7942 RVA: 0x000149E5 File Offset: 0x00012BE5
	public float value
	{
		get
		{
			return (!(this.audioSource != null)) ? 0f : this.mSource.volume;
		}
		set
		{
			if (this.audioSource != null)
			{
				this.mSource.volume = value;
			}
		}
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x00014A04 File Offset: 0x00012C04
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		this.mSource.enabled = (this.mSource.volume > 0.01f);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x000ED068 File Offset: 0x000EB268
	public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
	{
		TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration);
		tweenVolume.from = tweenVolume.value;
		tweenVolume.to = targetVolume;
		return tweenVolume;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x00014A40 File Offset: 0x00012C40
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x00014A4E File Offset: 0x00012C4E
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x040022A3 RID: 8867
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x040022A4 RID: 8868
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x040022A5 RID: 8869
	private AudioSource mSource;
}
