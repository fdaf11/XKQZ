using System;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Height")]
public class TweenHeight : UITweener
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x0001465B File Offset: 0x0001285B
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

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x00014680 File Offset: 0x00012880
	// (set) Token: 0x06001EC4 RID: 7876 RVA: 0x00014688 File Offset: 0x00012888
	[Obsolete("Use 'value' instead")]
	public int height
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

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x00014691 File Offset: 0x00012891
	// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x0001469E File Offset: 0x0001289E
	public int value
	{
		get
		{
			return this.cachedWidget.height;
		}
		set
		{
			this.cachedWidget.height = value;
		}
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000EC9C4 File Offset: 0x000EABC4
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

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000ECA48 File Offset: 0x000EAC48
	public static TweenHeight Begin(UIWidget widget, float duration, int height)
	{
		TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration);
		tweenHeight.from = widget.height;
		tweenHeight.to = height;
		if (duration <= 0f)
		{
			tweenHeight.Sample(1f, true);
			tweenHeight.enabled = false;
		}
		return tweenHeight;
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x000146AC File Offset: 0x000128AC
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000146BA File Offset: 0x000128BA
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000146C8 File Offset: 0x000128C8
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x000146D6 File Offset: 0x000128D6
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04002287 RID: 8839
	public int from = 100;

	// Token: 0x04002288 RID: 8840
	public int to = 100;

	// Token: 0x04002289 RID: 8841
	public bool updateTable;

	// Token: 0x0400228A RID: 8842
	private UIWidget mWidget;

	// Token: 0x0400228B RID: 8843
	private UITable mTable;
}
