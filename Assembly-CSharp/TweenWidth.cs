using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
[AddComponentMenu("NGUI/Tween/Tween Width")]
[RequireComponent(typeof(UIWidget))]
public class TweenWidth : UITweener
{
	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001F0C RID: 7948 RVA: 0x00014A74 File Offset: 0x00012C74
	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06001F0D RID: 7949 RVA: 0x00014A99 File Offset: 0x00012C99
	// (set) Token: 0x06001F0E RID: 7950 RVA: 0x00014AA1 File Offset: 0x00012CA1
	[Obsolete("Use 'value' instead")]
	public int width
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

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001F0F RID: 7951 RVA: 0x00014AAA File Offset: 0x00012CAA
	// (set) Token: 0x06001F10 RID: 7952 RVA: 0x00014AB7 File Offset: 0x00012CB7
	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000ED094 File Offset: 0x000EB294
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
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

	// Token: 0x06001F12 RID: 7954 RVA: 0x000ED118 File Offset: 0x000EB318
	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x00014AC5 File Offset: 0x00012CC5
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x00014AD3 File Offset: 0x00012CD3
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x00014AE1 File Offset: 0x00012CE1
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x00014AEF File Offset: 0x00012CEF
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040022A6 RID: 8870
	public int from = 100;

	// Token: 0x040022A7 RID: 8871
	public int to = 100;

	// Token: 0x040022A8 RID: 8872
	public bool updateTable;

	// Token: 0x040022A9 RID: 8873
	private UIWidget mWidget;

	// Token: 0x040022AA RID: 8874
	private UITable mTable;
}
