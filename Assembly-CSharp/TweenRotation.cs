using System;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x00014865 File Offset: 0x00012A65
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

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0001488A File Offset: 0x00012A8A
	// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x00014892 File Offset: 0x00012A92
	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
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

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x0001489B File Offset: 0x00012A9B
	// (set) Token: 0x06001EEA RID: 7914 RVA: 0x000148A8 File Offset: 0x00012AA8
	public Quaternion value
	{
		get
		{
			return this.cachedTransform.localRotation;
		}
		set
		{
			this.cachedTransform.localRotation = value;
		}
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x000ECC08 File Offset: 0x000EAE08
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor)));
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x000ECC7C File Offset: 0x000EAE7C
	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.value.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.enabled = false;
		}
		return tweenRotation;
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x000ECCD4 File Offset: 0x000EAED4
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.eulerAngles;
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x000ECCF8 File Offset: 0x000EAEF8
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.eulerAngles;
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000148B6 File Offset: 0x00012AB6
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x000148C9 File Offset: 0x00012AC9
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}

	// Token: 0x04002294 RID: 8852
	public Vector3 from;

	// Token: 0x04002295 RID: 8853
	public Vector3 to;

	// Token: 0x04002296 RID: 8854
	private Transform mTrans;
}
