using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200048B RID: 1163
[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
	// Token: 0x06001C09 RID: 7177 RVA: 0x000DB328 File Offset: 0x000D9528
	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
		}
		this.mFirstTime = false;
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x00012B3E File Offset: 0x00010D3E
	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x000DB378 File Offset: 0x000D9578
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
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
		}
		else
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortVertical));
		}
		this.ResetChildPositions();
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000DB414 File Offset: 0x000D9614
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
		this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortByName));
		this.ResetChildPositions();
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x000DB488 File Offset: 0x000D9688
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

	// Token: 0x06001C0E RID: 7182 RVA: 0x000DB514 File Offset: 0x000D9714
	private void ResetChildPositions()
	{
		int i = 0;
		int count = this.mChildren.Count;
		while (i < count)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = ((!this.mHorizontal) ? new Vector3(0f, (float)(-(float)i * this.itemSize), 0f) : new Vector3((float)(i * this.itemSize), 0f, 0f));
			i++;
		}
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000DB594 File Offset: 0x000D9794
	public void WrapContent()
	{
		float num = (float)(this.itemSize * this.mChildren.Count) * 0.5f;
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		bool flag = true;
		float num2 = num * 2f;
		if (this.mHorizontal)
		{
			float num3 = worldCorners[0].x - (float)this.itemSize;
			float num4 = worldCorners[2].x + (float)this.itemSize;
			int j = 0;
			int count = this.mChildren.Count;
			while (j < count)
			{
				Transform transform = this.mChildren[j];
				float num5 = transform.localPosition.x - vector2.x;
				if (num5 < -num)
				{
					Vector3 localPosition = transform.localPosition;
					localPosition.x += num2;
					num5 = localPosition.x - vector2.x;
					int num6 = Mathf.RoundToInt(localPosition.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num6 && num6 <= this.maxIndex))
					{
						transform.localPosition = localPosition;
						this.UpdateItem(transform, j);
						transform.name = num6.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (num5 > num)
				{
					Vector3 localPosition2 = transform.localPosition;
					localPosition2.x -= num2;
					num5 = localPosition2.x - vector2.x;
					int num7 = Mathf.RoundToInt(localPosition2.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num7 && num7 <= this.maxIndex))
					{
						transform.localPosition = localPosition2;
						this.UpdateItem(transform, j);
						transform.name = num7.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform, j);
				}
				if (this.cullContent)
				{
					num5 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
					if (!UICamera.IsPressed(transform.gameObject))
					{
						NGUITools.SetActive(transform.gameObject, num5 > num3 && num5 < num4, false);
					}
				}
				j++;
			}
		}
		else
		{
			float num8 = worldCorners[0].y - (float)this.itemSize;
			float num9 = worldCorners[2].y + (float)this.itemSize;
			int k = 0;
			int count2 = this.mChildren.Count;
			while (k < count2)
			{
				Transform transform2 = this.mChildren[k];
				float num10 = transform2.localPosition.y - vector2.y;
				if (num10 < -num)
				{
					Vector3 localPosition3 = transform2.localPosition;
					localPosition3.y += num2;
					num10 = localPosition3.y - vector2.y;
					int num11 = Mathf.RoundToInt(localPosition3.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num11 && num11 <= this.maxIndex))
					{
						transform2.localPosition = localPosition3;
						this.UpdateItem(transform2, k);
						transform2.name = num11.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (num10 > num)
				{
					Vector3 localPosition4 = transform2.localPosition;
					localPosition4.y -= num2;
					num10 = localPosition4.y - vector2.y;
					int num12 = Mathf.RoundToInt(localPosition4.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num12 && num12 <= this.maxIndex))
					{
						transform2.localPosition = localPosition4;
						this.UpdateItem(transform2, k);
						transform2.name = num12.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform2, k);
				}
				if (this.cullContent)
				{
					num10 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
					if (!UICamera.IsPressed(transform2.gameObject))
					{
						NGUITools.SetActive(transform2.gameObject, num10 > num8 && num10 < num9, false);
					}
				}
				k++;
			}
		}
		this.mScroll.restrictWithinPanel = !flag;
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x00012B46 File Offset: 0x00010D46
	private void OnValidate()
	{
		if (this.maxIndex < this.minIndex)
		{
			this.maxIndex = this.minIndex;
		}
		if (this.minIndex > this.maxIndex)
		{
			this.maxIndex = this.minIndex;
		}
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x000DBAC4 File Offset: 0x000D9CC4
	protected virtual void UpdateItem(Transform item, int index)
	{
		if (this.onInitializeItem != null)
		{
			int realIndex = (this.mScroll.movement != UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.x / (float)this.itemSize) : Mathf.RoundToInt(item.localPosition.y / (float)this.itemSize);
			this.onInitializeItem(item.gameObject, index, realIndex);
		}
	}

	// Token: 0x040020EA RID: 8426
	public int itemSize = 100;

	// Token: 0x040020EB RID: 8427
	public bool cullContent = true;

	// Token: 0x040020EC RID: 8428
	public int minIndex;

	// Token: 0x040020ED RID: 8429
	public int maxIndex;

	// Token: 0x040020EE RID: 8430
	public UIWrapContent.OnInitializeItem onInitializeItem;

	// Token: 0x040020EF RID: 8431
	private Transform mTrans;

	// Token: 0x040020F0 RID: 8432
	private UIPanel mPanel;

	// Token: 0x040020F1 RID: 8433
	private UIScrollView mScroll;

	// Token: 0x040020F2 RID: 8434
	private bool mHorizontal;

	// Token: 0x040020F3 RID: 8435
	private bool mFirstTime = true;

	// Token: 0x040020F4 RID: 8436
	private List<Transform> mChildren = new List<Transform>();

	// Token: 0x0200048C RID: 1164
	// (Invoke) Token: 0x06001C13 RID: 7187
	public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}
