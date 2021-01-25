using System;
using UnityEngine;

// Token: 0x020004CC RID: 1228
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	// Token: 0x06001EAA RID: 7850 RVA: 0x000EC734 File Offset: 0x000EA934
	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			return;
		}
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mSr != null)
		{
			return;
		}
		Renderer renderer = base.renderer;
		if (renderer != null)
		{
			this.mMat = renderer.material;
			return;
		}
		this.mLight = base.light;
		if (this.mLight == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06001EAB RID: 7851 RVA: 0x0001451A File Offset: 0x0001271A
	// (set) Token: 0x06001EAC RID: 7852 RVA: 0x00014522 File Offset: 0x00012722
	[Obsolete("Use 'value' instead")]
	public Color color
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

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06001EAD RID: 7853 RVA: 0x000EC7D0 File Offset: 0x000EA9D0
	// (set) Token: 0x06001EAE RID: 7854 RVA: 0x000EC868 File Offset: 0x000EAA68
	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			if (this.mSr != null)
			{
				return this.mSr.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			return Color.black;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			else if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			else if (this.mSr != null)
			{
				this.mSr.color = value;
			}
			else if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x0001452B File Offset: 0x0001272B
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000EC934 File Offset: 0x000EAB34
	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x00014545 File Offset: 0x00012745
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00014553 File Offset: 0x00012753
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x00014561 File Offset: 0x00012761
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x0001456F File Offset: 0x0001276F
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400227D RID: 8829
	public Color from = Color.white;

	// Token: 0x0400227E RID: 8830
	public Color to = Color.white;

	// Token: 0x0400227F RID: 8831
	private bool mCached;

	// Token: 0x04002280 RID: 8832
	private UIWidget mWidget;

	// Token: 0x04002281 RID: 8833
	private Material mMat;

	// Token: 0x04002282 RID: 8834
	private Light mLight;

	// Token: 0x04002283 RID: 8835
	private SpriteRenderer mSr;
}
