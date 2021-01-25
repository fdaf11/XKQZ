using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000144B5 File Offset: 0x000126B5
	// (set) Token: 0x06001EA1 RID: 7841 RVA: 0x000144BD File Offset: 0x000126BD
	[Obsolete("Use 'value' instead")]
	public float alpha
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

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000EC528 File Offset: 0x000EA728
	private void Cache()
	{
		this.mCached = true;
		this.mRect = base.GetComponent<UIRect>();
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mRect == null && this.mSr == null)
		{
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				this.mMat = component.material;
			}
			if (this.mMat == null)
			{
				this.mRect = base.GetComponentInChildren<UIRect>();
			}
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x000EC5B4 File Offset: 0x000EA7B4
	// (set) Token: 0x06001EA4 RID: 7844 RVA: 0x000EC644 File Offset: 0x000EA844
	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				return this.mRect.alpha;
			}
			if (this.mSr != null)
			{
				return this.mSr.color.a;
			}
			return (!(this.mMat != null)) ? 1f : this.mMat.color.a;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				this.mRect.alpha = value;
			}
			else if (this.mSr != null)
			{
				Color color = this.mSr.color;
				color.a = value;
				this.mSr.color = color;
			}
			else if (this.mMat != null)
			{
				Color color2 = this.mMat.color;
				color2.a = value;
				this.mMat.color = color2;
			}
		}
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000144C6 File Offset: 0x000126C6
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000EC6EC File Offset: 0x000EA8EC
	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000144E0 File Offset: 0x000126E0
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000144EE File Offset: 0x000126EE
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04002277 RID: 8823
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04002278 RID: 8824
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04002279 RID: 8825
	private bool mCached;

	// Token: 0x0400227A RID: 8826
	private UIRect mRect;

	// Token: 0x0400227B RID: 8827
	private Material mMat;

	// Token: 0x0400227C RID: 8828
	private SpriteRenderer mSr;
}
