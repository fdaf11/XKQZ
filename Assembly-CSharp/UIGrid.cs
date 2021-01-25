using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045E RID: 1118
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	// Token: 0x17000205 RID: 517
	// (set) Token: 0x06001ADE RID: 6878 RVA: 0x00011DE6 File Offset: 0x0000FFE6
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

	// Token: 0x06001ADF RID: 6879 RVA: 0x000D4644 File Offset: 0x000D2844
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
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
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

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000D4750 File Offset: 0x000D2950
	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		return (index >= childList.Count) ? null : childList[index];
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x00011DFC File Offset: 0x0000FFFC
	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x00011E0A File Offset: 0x0001000A
	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x00011E14 File Offset: 0x00010014
	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000D4780 File Offset: 0x000D2980
	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00011E3A File Offset: 0x0001003A
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000D47AC File Offset: 0x000D29AC
	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x00011E54 File Offset: 0x00010054
	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x00011E63 File Offset: 0x00010063
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x00011E80 File Offset: 0x00010080
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000D47EC File Offset: 0x000D29EC
	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x000D481C File Offset: 0x000D2A1C
	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void Sort(List<Transform> list)
	{
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000D484C File Offset: 0x000D2A4C
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000D48E4 File Offset: 0x000D2AE4
	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000D4934 File Offset: 0x000D2B34
	protected void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			Transform transform2 = list[i];
			Vector3 vector = transform2.localPosition;
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z));
			}
			if (this.animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(transform2.localPosition - vector) >= 0.0001f)
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition2 = component;
					springPosition2.target.x = springPosition2.target.x - num5;
					SpringPosition springPosition3 = component;
					springPosition3.target.y = springPosition3.target.y - num6;
				}
				else
				{
					Vector3 localPosition = child.localPosition;
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.localPosition = localPosition;
				}
			}
		}
	}

	// Token: 0x04001FB2 RID: 8114
	public UIGrid.Arrangement arrangement;

	// Token: 0x04001FB3 RID: 8115
	public UIGrid.Sorting sorting;

	// Token: 0x04001FB4 RID: 8116
	public UIWidget.Pivot pivot;

	// Token: 0x04001FB5 RID: 8117
	public int maxPerLine;

	// Token: 0x04001FB6 RID: 8118
	public float cellWidth = 200f;

	// Token: 0x04001FB7 RID: 8119
	public float cellHeight = 200f;

	// Token: 0x04001FB8 RID: 8120
	public bool animateSmoothly;

	// Token: 0x04001FB9 RID: 8121
	public bool hideInactive;

	// Token: 0x04001FBA RID: 8122
	public bool keepWithinPanel;

	// Token: 0x04001FBB RID: 8123
	public UIGrid.OnReposition onReposition;

	// Token: 0x04001FBC RID: 8124
	public Comparison<Transform> onCustomSort;

	// Token: 0x04001FBD RID: 8125
	[HideInInspector]
	[SerializeField]
	private bool sorted;

	// Token: 0x04001FBE RID: 8126
	protected bool mReposition;

	// Token: 0x04001FBF RID: 8127
	protected UIPanel mPanel;

	// Token: 0x04001FC0 RID: 8128
	protected bool mInitDone;

	// Token: 0x0200045F RID: 1119
	public enum Arrangement
	{
		// Token: 0x04001FC2 RID: 8130
		Horizontal,
		// Token: 0x04001FC3 RID: 8131
		Vertical,
		// Token: 0x04001FC4 RID: 8132
		CellSnap
	}

	// Token: 0x02000460 RID: 1120
	public enum Sorting
	{
		// Token: 0x04001FC6 RID: 8134
		None,
		// Token: 0x04001FC7 RID: 8135
		Alphabetic,
		// Token: 0x04001FC8 RID: 8136
		Horizontal,
		// Token: 0x04001FC9 RID: 8137
		Vertical,
		// Token: 0x04001FCA RID: 8138
		Custom
	}

	// Token: 0x02000461 RID: 1121
	// (Invoke) Token: 0x06001AF1 RID: 6897
	public delegate void OnReposition();
}
