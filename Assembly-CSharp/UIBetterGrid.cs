using System;
using UnityEngine;

// Token: 0x02000443 RID: 1091
[AddComponentMenu("NGUI/Interaction/Better Grid")]
public class UIBetterGrid : MonoBehaviour
{
	// Token: 0x06001A29 RID: 6697 RVA: 0x000D13B4 File Offset: 0x000CF5B4
	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
			this.mScroll.restrictWithinPanel = true;
		}
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x00011186 File Offset: 0x0000F386
	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000D1408 File Offset: 0x000CF608
	[ContextMenu("Sort Based on Scroll Movement")]
	public void SortBasedOnScrollMovement()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		if (this.mHorizontal)
		{
			this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortHorizontal));
		}
		else
		{
			this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortVertical));
		}
		this.ResetChildPositions();
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x000D14A4 File Offset: 0x000CF6A4
	[ContextMenu("Sort Alphabetically")]
	public void SortAlphabetically()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortByName));
		this.ResetChildPositions();
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x000D1518 File Offset: 0x000CF718
	protected bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		if (this.mScroll == null)
		{
			return false;
		}
		if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
		{
			this.mHorizontal = true;
		}
		else
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				return false;
			}
			this.mHorizontal = false;
		}
		return true;
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x000D15A4 File Offset: 0x000CF7A4
	private void ResetChildPositions()
	{
		for (int i = 0; i < this.mChildren.size; i++)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = ((!this.mHorizontal) ? new Vector3(0f, (float)(-(float)i * this.itemSize), 0f) : new Vector3((float)(i * this.itemSize), 0f, 0f));
		}
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x000D1624 File Offset: 0x000CF824
	public void WrapContent()
	{
		float num = (float)(this.itemSize * this.mChildren.size) * 0.5f;
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		if (this.mHorizontal)
		{
			float num2 = worldCorners[0].x - (float)this.itemSize;
			float num3 = worldCorners[2].x + (float)this.itemSize;
			for (int j = 0; j < this.mChildren.size; j++)
			{
				Transform transform = this.mChildren[j];
				float num4 = transform.localPosition.x - vector2.x;
				if (this.cullContent)
				{
					num4 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
					if (!UICamera.IsPressed(transform.gameObject))
					{
						NGUITools.SetActive(transform.gameObject, num4 > num2 && num4 < num3, false);
					}
				}
			}
		}
		else
		{
			float num5 = worldCorners[0].y - (float)this.itemSize;
			float num6 = worldCorners[2].y + (float)this.itemSize;
			for (int k = 0; k < this.mChildren.size; k++)
			{
				Transform transform2 = this.mChildren[k];
				float num7 = transform2.localPosition.y - vector2.y;
				if (this.cullContent)
				{
					num7 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
					if (!UICamera.IsPressed(transform2.gameObject))
					{
						NGUITools.SetActive(transform2.gameObject, num7 > num5 && num7 < num6, false);
					}
				}
			}
		}
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void UpdateItem(Transform item, int index)
	{
	}

	// Token: 0x04001EFA RID: 7930
	public int itemSize = 100;

	// Token: 0x04001EFB RID: 7931
	public bool cullContent = true;

	// Token: 0x04001EFC RID: 7932
	private Transform mTrans;

	// Token: 0x04001EFD RID: 7933
	private UIPanel mPanel;

	// Token: 0x04001EFE RID: 7934
	private UIScrollView mScroll;

	// Token: 0x04001EFF RID: 7935
	private bool mHorizontal;

	// Token: 0x04001F00 RID: 7936
	private BetterList<Transform> mChildren = new BetterList<Transform>();
}
