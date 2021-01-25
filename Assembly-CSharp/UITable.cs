using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000483 RID: 1155
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	// Token: 0x1700022D RID: 557
	// (set) Token: 0x06001BE7 RID: 7143 RVA: 0x000129D9 File Offset: 0x00010BD9
	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000DA5DC File Offset: 0x000D87DC
	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UITable.Sorting.None)
		{
			if (this.sorting == UITable.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UITable.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UITable.Sorting.Vertical)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000129EF File Offset: 0x00010BEF
	protected virtual void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(UIGrid.SortByName));
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00012A03 File Offset: 0x00010C03
	protected virtual void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00012A18 File Offset: 0x00010C18
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00012A32 File Offset: 0x00010C32
	protected virtual void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x00012A4C File Offset: 0x00010C4C
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x000DA6DC File Offset: 0x000D88DC
	protected void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = (this.columns <= 0) ? 1 : (children.Count / this.columns + 1);
		int num4 = (this.columns <= 0) ? children.Count : this.columns;
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int count = children.Count;
		while (i < count)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, !this.hideInactive);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
			}
			i++;
		}
		num5 = 0;
		num6 = 0;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.cellAlignment);
		int j = 0;
		int count2 = children.Count;
		while (j < count2)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x -= Mathf.Lerp(0f, bounds2.max.x - bounds2.min.x - bounds3.max.x + bounds3.min.x, pivotOffset.x) - this.padding.x;
			if (this.direction == UITable.Direction.Down)
			{
				localPosition.y = -num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += Mathf.Lerp(bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, 0f, pivotOffset.y) - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + bounds2.extents.y - bounds2.center.y;
				localPosition.y -= Mathf.Lerp(0f, bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, pivotOffset.y) - this.padding.y;
			}
			num += bounds3.size.x + this.padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
			j++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			Bounds bounds5 = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float num7 = Mathf.Lerp(0f, bounds5.size.x, pivotOffset.x);
			float num8 = Mathf.Lerp(-bounds5.size.y, 0f, pivotOffset.y);
			Transform transform3 = base.transform;
			for (int k = 0; k < transform3.childCount; k++)
			{
				Transform child = transform3.GetChild(k);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition = component;
					springPosition.target.x = springPosition.target.x - num7;
					SpringPosition springPosition2 = component;
					springPosition2.target.y = springPosition2.target.y - num8;
				}
				else
				{
					Vector3 localPosition2 = child.localPosition;
					localPosition2.x -= num7;
					localPosition2.y -= num8;
					child.localPosition = localPosition2;
				}
			}
		}
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x000DABF4 File Offset: 0x000D8DF4
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		List<Transform> childList = this.GetChildList();
		if (childList.Count > 0)
		{
			this.RepositionVariableSize(childList);
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x040020BC RID: 8380
	public int columns;

	// Token: 0x040020BD RID: 8381
	public UITable.Direction direction;

	// Token: 0x040020BE RID: 8382
	public UITable.Sorting sorting;

	// Token: 0x040020BF RID: 8383
	public UIWidget.Pivot pivot;

	// Token: 0x040020C0 RID: 8384
	public UIWidget.Pivot cellAlignment;

	// Token: 0x040020C1 RID: 8385
	public bool hideInactive = true;

	// Token: 0x040020C2 RID: 8386
	public bool keepWithinPanel;

	// Token: 0x040020C3 RID: 8387
	public Vector2 padding = Vector2.zero;

	// Token: 0x040020C4 RID: 8388
	public UITable.OnReposition onReposition;

	// Token: 0x040020C5 RID: 8389
	public Comparison<Transform> onCustomSort;

	// Token: 0x040020C6 RID: 8390
	protected UIPanel mPanel;

	// Token: 0x040020C7 RID: 8391
	protected bool mInitDone;

	// Token: 0x040020C8 RID: 8392
	protected bool mReposition;

	// Token: 0x02000484 RID: 1156
	public enum Direction
	{
		// Token: 0x040020CA RID: 8394
		Down,
		// Token: 0x040020CB RID: 8395
		Up
	}

	// Token: 0x02000485 RID: 1157
	public enum Sorting
	{
		// Token: 0x040020CD RID: 8397
		None,
		// Token: 0x040020CE RID: 8398
		Alphabetic,
		// Token: 0x040020CF RID: 8399
		Horizontal,
		// Token: 0x040020D0 RID: 8400
		Vertical,
		// Token: 0x040020D1 RID: 8401
		Custom
	}

	// Token: 0x02000486 RID: 1158
	// (Invoke) Token: 0x06001BF1 RID: 7153
	public delegate void OnReposition();
}
