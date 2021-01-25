using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000453 RID: 1107
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	// Token: 0x06001A8F RID: 6799 RVA: 0x00011784 File Offset: 0x0000F984
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mCollider = base.collider;
		this.mCollider2D = base.collider2D;
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000117C2 File Offset: 0x0000F9C2
	protected void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
			this.mPressed = true;
		}
		else
		{
			this.mPressed = false;
		}
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000117EF File Offset: 0x0000F9EF
	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000D2F58 File Offset: 0x000D1158
	protected void OnDragStart()
	{
		if (!base.enabled || this.mTouchID != -2147483648)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000D3014 File Offset: 0x000D1214
	protected virtual void StartDragging()
	{
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				GameObject gameObject = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
				gameObject.transform.localPosition = base.transform.localPosition;
				gameObject.transform.localRotation = base.transform.localRotation;
				gameObject.transform.localScale = base.transform.localScale;
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				UICamera.currentTouch.dragged = gameObject;
				UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
				component2.mDragging = true;
				component2.Start();
				component2.OnDragDropStart();
			}
			else
			{
				this.mDragging = true;
				this.OnDragDropStart();
			}
		}
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x00011829 File Offset: 0x0000FA29
	protected void OnDrag(Vector2 delta)
	{
		if (!this.mDragging || !base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x00011869 File Offset: 0x0000FA69
	protected void OnDragEnd()
	{
		if (!base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x00011892 File Offset: 0x0000FA92
	public void StopDragging(GameObject go)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000D30F0 File Offset: 0x000D12F0
	protected virtual void OnDragDropStart()
	{
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.enabled = false;
		}
		this.mTouchID = UICamera.currentTouchID;
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000118AD File Offset: 0x0000FAAD
	protected virtual void OnDragDropMove(Vector2 delta)
	{
		this.mTrans.localPosition += delta;
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000D3288 File Offset: 0x000D1488
	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			this.mTouchID = int.MinValue;
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.enabled = true;
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.enabled = true;
			}
			UIDragDropContainer uidragDropContainer = (!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
			if (uidragDropContainer != null)
			{
				this.mTrans.parent = ((!(uidragDropContainer.reparentTarget != null)) ? uidragDropContainer.transform : uidragDropContainer.reparentTarget);
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				this.mTrans.parent = this.mParent;
			}
			this.mParent = this.mTrans.parent;
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.StartCoroutine(this.EnableDragScrollView());
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
			this.OnDragDropEnd();
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void OnDragDropEnd()
	{
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000D3444 File Offset: 0x000D1644
	protected IEnumerator EnableDragScrollView()
	{
		yield return new WaitForEndOfFrame();
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
		yield break;
	}

	// Token: 0x04001F4B RID: 8011
	public UIDragDropItem.Restriction restriction;

	// Token: 0x04001F4C RID: 8012
	public bool cloneOnDrag;

	// Token: 0x04001F4D RID: 8013
	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	// Token: 0x04001F4E RID: 8014
	protected Transform mTrans;

	// Token: 0x04001F4F RID: 8015
	protected Transform mParent;

	// Token: 0x04001F50 RID: 8016
	protected Collider mCollider;

	// Token: 0x04001F51 RID: 8017
	protected Collider2D mCollider2D;

	// Token: 0x04001F52 RID: 8018
	protected UIButton mButton;

	// Token: 0x04001F53 RID: 8019
	protected UIRoot mRoot;

	// Token: 0x04001F54 RID: 8020
	protected UIGrid mGrid;

	// Token: 0x04001F55 RID: 8021
	protected UITable mTable;

	// Token: 0x04001F56 RID: 8022
	protected int mTouchID = int.MinValue;

	// Token: 0x04001F57 RID: 8023
	protected float mDragStartTime;

	// Token: 0x04001F58 RID: 8024
	protected UIDragScrollView mDragScrollView;

	// Token: 0x04001F59 RID: 8025
	protected bool mPressed;

	// Token: 0x04001F5A RID: 8026
	protected bool mDragging;

	// Token: 0x02000454 RID: 1108
	public enum Restriction
	{
		// Token: 0x04001F5C RID: 8028
		None,
		// Token: 0x04001F5D RID: 8029
		Horizontal,
		// Token: 0x04001F5E RID: 8030
		Vertical,
		// Token: 0x04001F5F RID: 8031
		PressAndHold
	}
}
