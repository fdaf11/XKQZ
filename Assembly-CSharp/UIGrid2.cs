using System;
using UnityEngine;

// Token: 0x02000462 RID: 1122
[AddComponentMenu("NGUI/Interaction/Grid2")]
public class UIGrid2 : UIWidgetContainer
{
	// Token: 0x17000206 RID: 518
	// (set) Token: 0x06001AF5 RID: 6901 RVA: 0x00011EB8 File Offset: 0x000100B8
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

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000D4C1C File Offset: 0x000D2E1C
	public BetterList<Transform> GetChildList()
	{
		Transform transform = base.transform;
		BetterList<Transform> betterList = new BetterList<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				betterList.Add(child);
			}
		}
		return betterList;
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000D4C84 File Offset: 0x000D2E84
	public Transform GetChild(int index)
	{
		BetterList<Transform> childList = this.GetChildList();
		return (index >= childList.size) ? null : childList[index];
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00011ECE File Offset: 0x000100CE
	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x00011EDC File Offset: 0x000100DC
	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x000D4CB4 File Offset: 0x000D2EB4
	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			BetterList<Transform> childList = this.GetChildList();
			childList.Add(trans);
			this.ResetPosition(childList);
		}
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x000D4CE4 File Offset: 0x000D2EE4
	public void AddChild(Transform trans, int index)
	{
		if (trans != null)
		{
			if (this.sorting != UIGrid2.Sorting.None)
			{
				Debug.LogWarning("The Grid has sorting enabled, so AddChild at index may not work as expected.", this);
			}
			BetterList<Transform> childList = this.GetChildList();
			childList.Insert(index, trans);
			this.ResetPosition(childList);
		}
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x000D4D2C File Offset: 0x000D2F2C
	public Transform RemoveChild(int index)
	{
		BetterList<Transform> childList = this.GetChildList();
		if (index < childList.size)
		{
			Transform result = childList[index];
			childList.RemoveAt(index);
			this.ResetPosition(childList);
			return result;
		}
		return null;
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x000D4D68 File Offset: 0x000D2F68
	public bool RemoveChild(Transform t)
	{
		BetterList<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	// Token: 0x06001AFE RID: 6910 RVA: 0x00011EE6 File Offset: 0x000100E6
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000D4D94 File Offset: 0x000D2F94
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

	// Token: 0x06001B00 RID: 6912 RVA: 0x00011F00 File Offset: 0x00010100
	protected virtual void Update()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x00011E80 File Offset: 0x00010080
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000D47EC File Offset: 0x000D29EC
	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000D481C File Offset: 0x000D2A1C
	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void Sort(BetterList<Transform> list)
	{
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x000D4DD4 File Offset: 0x000D2FD4
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.mReposition = true;
			return;
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid2.Sorting.None)
			{
				this.sorting = UIGrid2.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		if (!this.mInitDone)
		{
			this.Init();
		}
		BetterList<Transform> childList = this.GetChildList();
		if (this.sorting != UIGrid2.Sorting.None)
		{
			if (this.sorting == UIGrid2.Sorting.Alphabetic)
			{
				childList.Sort(new BetterList<Transform>.CompareFunc(UIGrid2.SortByName));
			}
			else if (this.sorting == UIGrid2.Sorting.Horizontal)
			{
				childList.Sort(new BetterList<Transform>.CompareFunc(UIGrid2.SortHorizontal));
			}
			else if (this.sorting == UIGrid2.Sorting.Vertical)
			{
				childList.Sort(new BetterList<Transform>.CompareFunc(UIGrid2.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				childList.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(childList);
			}
		}
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

	// Token: 0x06001B06 RID: 6918 RVA: 0x00011F1A File Offset: 0x0001011A
	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
		}
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x000D4F10 File Offset: 0x000D3110
	protected void ResetPosition(BetterList<Transform> list)
	{
		this.mReposition = false;
		int i = 0;
		int size = list.size;
		while (i < size)
		{
			list[i].parent = null;
			i++;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int j = 0;
		int size2 = list.size;
		while (j < size2)
		{
			Transform transform2 = list[j];
			transform2.parent = transform;
			float z = transform2.localPosition.z;
			Vector3 vector = (this.arrangement != UIGrid2.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z);
			if (this.animateSmoothly && Application.isPlaying)
			{
				SpringPosition.Begin(transform2.gameObject, vector, 15f).updateScrollView = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (num2 < 3)
			{
				num2++;
			}
			else
			{
				num2 = 0;
				if (++num >= this.maxPerLine && this.maxPerLine > 0)
				{
					num = 0;
					num2++;
				}
			}
			j++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid2.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child = transform.GetChild(k);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition = component;
					springPosition.target.x = springPosition.target.x - num5;
					SpringPosition springPosition2 = component;
					springPosition2.target.y = springPosition2.target.y - num6;
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

	// Token: 0x04001FCB RID: 8139
	public UIGrid2.Arrangement arrangement;

	// Token: 0x04001FCC RID: 8140
	public UIGrid2.Sorting sorting;

	// Token: 0x04001FCD RID: 8141
	public UIWidget.Pivot pivot;

	// Token: 0x04001FCE RID: 8142
	public int maxPerLine;

	// Token: 0x04001FCF RID: 8143
	public float cellWidth = 200f;

	// Token: 0x04001FD0 RID: 8144
	public float cellHeight = 200f;

	// Token: 0x04001FD1 RID: 8145
	public bool animateSmoothly;

	// Token: 0x04001FD2 RID: 8146
	public bool hideInactive = true;

	// Token: 0x04001FD3 RID: 8147
	public bool keepWithinPanel;

	// Token: 0x04001FD4 RID: 8148
	public UIGrid2.OnReposition onReposition;

	// Token: 0x04001FD5 RID: 8149
	public BetterList<Transform>.CompareFunc onCustomSort;

	// Token: 0x04001FD6 RID: 8150
	[SerializeField]
	[HideInInspector]
	private bool sorted;

	// Token: 0x04001FD7 RID: 8151
	protected bool mReposition;

	// Token: 0x04001FD8 RID: 8152
	protected UIPanel mPanel;

	// Token: 0x04001FD9 RID: 8153
	protected bool mInitDone;

	// Token: 0x02000463 RID: 1123
	public enum Arrangement
	{
		// Token: 0x04001FDB RID: 8155
		Horizontal,
		// Token: 0x04001FDC RID: 8156
		Vertical
	}

	// Token: 0x02000464 RID: 1124
	public enum Sorting
	{
		// Token: 0x04001FDE RID: 8158
		None,
		// Token: 0x04001FDF RID: 8159
		Alphabetic,
		// Token: 0x04001FE0 RID: 8160
		Horizontal,
		// Token: 0x04001FE1 RID: 8161
		Vertical,
		// Token: 0x04001FE2 RID: 8162
		Custom
	}

	// Token: 0x02000465 RID: 1125
	// (Invoke) Token: 0x06001B09 RID: 6921
	public delegate void OnReposition();
}
