using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000508 RID: 1288
[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06002119 RID: 8473 RVA: 0x00016282 File Offset: 0x00014482
	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x0600211A RID: 8474 RVA: 0x0001628F File Offset: 0x0001448F
	// (set) Token: 0x0600211B RID: 8475 RVA: 0x00016297 File Offset: 0x00014497
	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x0600211C RID: 8476 RVA: 0x000162A0 File Offset: 0x000144A0
	// (set) Token: 0x0600211D RID: 8477 RVA: 0x000162A8 File Offset: 0x000144A8
	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x0600211E RID: 8478 RVA: 0x000162C8 File Offset: 0x000144C8
	// (set) Token: 0x0600211F RID: 8479 RVA: 0x000162D0 File Offset: 0x000144D0
	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06002120 RID: 8480 RVA: 0x000162D9 File Offset: 0x000144D9
	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000162E1 File Offset: 0x000144E1
	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000F9CDC File Offset: 0x000F7EDC
	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.mFPS;
			if (num < this.mDelta)
			{
				this.mDelta = ((num <= 0f) ? 0f : (this.mDelta - num));
				if (++this.mIndex >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000F9DDC File Offset: 0x000F7FDC
	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uispriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uispriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uispriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000162E9 File Offset: 0x000144E9
	public void Play()
	{
		this.mActive = true;
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000162F2 File Offset: 0x000144F2
	public void Pause()
	{
		this.mActive = false;
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000F9EAC File Offset: 0x000F80AC
	public void ResetToBeginning()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x0400243E RID: 9278
	[SerializeField]
	[HideInInspector]
	protected int mFPS = 30;

	// Token: 0x0400243F RID: 9279
	[SerializeField]
	[HideInInspector]
	protected string mPrefix = string.Empty;

	// Token: 0x04002440 RID: 9280
	[SerializeField]
	[HideInInspector]
	protected bool mLoop = true;

	// Token: 0x04002441 RID: 9281
	[SerializeField]
	[HideInInspector]
	protected bool mSnap = true;

	// Token: 0x04002442 RID: 9282
	protected UISprite mSprite;

	// Token: 0x04002443 RID: 9283
	protected float mDelta;

	// Token: 0x04002444 RID: 9284
	protected int mIndex;

	// Token: 0x04002445 RID: 9285
	protected bool mActive = true;

	// Token: 0x04002446 RID: 9286
	protected List<string> mSpriteNames = new List<string>();
}
