using System;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	// Token: 0x06001EFE RID: 7934 RVA: 0x000ECDEC File Offset: 0x000EAFEC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x00014983 File Offset: 0x00012B83
	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000ECFB4 File Offset: 0x000EB1B4
	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	// Token: 0x0400229C RID: 8860
	public Transform from;

	// Token: 0x0400229D RID: 8861
	public Transform to;

	// Token: 0x0400229E RID: 8862
	public bool parentWhenFinished;

	// Token: 0x0400229F RID: 8863
	private Transform mTrans;

	// Token: 0x040022A0 RID: 8864
	private Vector3 mPos;

	// Token: 0x040022A1 RID: 8865
	private Quaternion mRot;

	// Token: 0x040022A2 RID: 8866
	private Vector3 mScale;
}
