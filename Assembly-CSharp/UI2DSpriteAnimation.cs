using System;
using UnityEngine;

// Token: 0x020004DA RID: 1242
public class UI2DSpriteAnimation : MonoBehaviour
{
	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06001F42 RID: 8002 RVA: 0x0000DB01 File Offset: 0x0000BD01
	public bool isPlaying
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06001F43 RID: 8003 RVA: 0x00014DD7 File Offset: 0x00012FD7
	// (set) Token: 0x06001F44 RID: 8004 RVA: 0x00014DDF File Offset: 0x00012FDF
	public int framesPerSecond
	{
		get
		{
			return this.framerate;
		}
		set
		{
			this.framerate = value;
		}
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000EE10C File Offset: 0x000EC30C
	public void Play()
	{
		if (this.frames != null && this.frames.Length > 0)
		{
			if (!base.enabled && !this.loop)
			{
				int num = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (num < 0 || num >= this.frames.Length)
				{
					this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
				}
			}
			base.enabled = true;
			this.UpdateSprite();
		}
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x00014DE8 File Offset: 0x00012FE8
	public void Pause()
	{
		base.enabled = false;
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x00014DF1 File Offset: 0x00012FF1
	public void ResetToBeginning()
	{
		this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
		this.UpdateSprite();
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x00014E1B File Offset: 0x0001301B
	private void Start()
	{
		this.Play();
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x000EE1B0 File Offset: 0x000EC3B0
	private void Update()
	{
		if (this.frames == null || this.frames.Length == 0)
		{
			base.enabled = false;
		}
		else if (this.framerate != 0)
		{
			float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
			if (this.mUpdate < num)
			{
				this.mUpdate = num;
				int num2 = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (!this.loop && (num2 < 0 || num2 >= this.frames.Length))
				{
					base.enabled = false;
					return;
				}
				this.mIndex = NGUIMath.RepeatIndex(num2, this.frames.Length);
				this.UpdateSprite();
			}
		}
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000EE280 File Offset: 0x000EC480
	private void UpdateSprite()
	{
		if (this.mUnitySprite == null && this.mNguiSprite == null)
		{
			this.mUnitySprite = base.GetComponent<SpriteRenderer>();
			this.mNguiSprite = base.GetComponent<UI2DSprite>();
			if (this.mUnitySprite == null && this.mNguiSprite == null)
			{
				base.enabled = false;
				return;
			}
		}
		float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (this.framerate != 0)
		{
			this.mUpdate = num + Mathf.Abs(1f / (float)this.framerate);
		}
		if (this.mUnitySprite != null)
		{
			this.mUnitySprite.sprite = this.frames[this.mIndex];
		}
		else if (this.mNguiSprite != null)
		{
			this.mNguiSprite.nextSprite = this.frames[this.mIndex];
		}
	}

	// Token: 0x040022D0 RID: 8912
	[SerializeField]
	protected int framerate = 20;

	// Token: 0x040022D1 RID: 8913
	public bool ignoreTimeScale = true;

	// Token: 0x040022D2 RID: 8914
	public bool loop = true;

	// Token: 0x040022D3 RID: 8915
	public Sprite[] frames;

	// Token: 0x040022D4 RID: 8916
	private SpriteRenderer mUnitySprite;

	// Token: 0x040022D5 RID: 8917
	private UI2DSprite mNguiSprite;

	// Token: 0x040022D6 RID: 8918
	private int mIndex;

	// Token: 0x040022D7 RID: 8919
	private float mUpdate;
}
