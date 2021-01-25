using System;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x000148FA File Offset: 0x00012AFA
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x0001491F File Offset: 0x00012B1F
	// (set) Token: 0x06001EF4 RID: 7924 RVA: 0x0001492C File Offset: 0x00012B2C
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x0001493A File Offset: 0x00012B3A
	// (set) Token: 0x06001EF6 RID: 7926 RVA: 0x00014942 File Offset: 0x00012B42
	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000ECD1C File Offset: 0x000EAF1C
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000ECDA4 File Offset: 0x000EAFA4
	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x0001494B File Offset: 0x00012B4B
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x00014959 File Offset: 0x00012B59
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x00014967 File Offset: 0x00012B67
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x00014975 File Offset: 0x00012B75
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04002297 RID: 8855
	public Vector3 from = Vector3.one;

	// Token: 0x04002298 RID: 8856
	public Vector3 to = Vector3.one;

	// Token: 0x04002299 RID: 8857
	public bool updateTable;

	// Token: 0x0400229A RID: 8858
	private Transform mTrans;

	// Token: 0x0400229B RID: 8859
	private UITable mTable;
}
