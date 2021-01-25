using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x00014796 File Offset: 0x00012996
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

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000147BB File Offset: 0x000129BB
	// (set) Token: 0x06001EDA RID: 7898 RVA: 0x000147C3 File Offset: 0x000129C3
	[Obsolete("Use 'value' instead")]
	public Vector3 position
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

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000147CC File Offset: 0x000129CC
	// (set) Token: 0x06001EDC RID: 7900 RVA: 0x000ECADC File Offset: 0x000EACDC
	public Vector3 value
	{
		get
		{
			return (!this.worldSpace) ? this.cachedTransform.localPosition : this.cachedTransform.position;
		}
		set
		{
			if (this.mRect == null || !this.mRect.isAnchored || this.worldSpace)
			{
				if (this.worldSpace)
				{
					this.cachedTransform.position = value;
				}
				else
				{
					this.cachedTransform.localPosition = value;
				}
			}
			else
			{
				value -= this.cachedTransform.localPosition;
				NGUIMath.MoveRect(this.mRect, value.x, value.y);
			}
		}
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x000147F4 File Offset: 0x000129F4
	private void Awake()
	{
		this.mRect = base.GetComponent<UIRect>();
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x00014802 File Offset: 0x00012A02
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x000ECB70 File Offset: 0x000EAD70
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x000ECBB8 File Offset: 0x000EADB8
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.worldSpace = worldSpace;
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x0001482D File Offset: 0x00012A2D
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x0001483B File Offset: 0x00012A3B
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x00014849 File Offset: 0x00012A49
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x00014857 File Offset: 0x00012A57
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400228F RID: 8847
	public Vector3 from;

	// Token: 0x04002290 RID: 8848
	public Vector3 to;

	// Token: 0x04002291 RID: 8849
	[HideInInspector]
	public bool worldSpace;

	// Token: 0x04002292 RID: 8850
	private Transform mTrans;

	// Token: 0x04002293 RID: 8851
	private UIRect mRect;
}
