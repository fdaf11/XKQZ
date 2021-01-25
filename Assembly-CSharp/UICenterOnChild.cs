using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044E RID: 1102
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06001A78 RID: 6776 RVA: 0x000115FD File Offset: 0x0000F7FD
	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x00011605 File Offset: 0x0000F805
	private void Start()
	{
		this.Recenter();
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x0001160D File Offset: 0x0000F80D
	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x00011631 File Offset: 0x0000F831
	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x0001164F File Offset: 0x0000F84F
	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x00011662 File Offset: 0x0000F862
	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000D2938 File Offset: 0x000D0B38
	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				this.mScrollView.onDragFinished = new UIScrollView.OnDragNotification(this.OnDragFinished);
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				this.mScrollView.horizontalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				this.mScrollView.verticalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 vector3 = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 vector4 = vector - vector3 * 0.01f;
		float num = float.MaxValue;
		Transform target = null;
		int num2 = 0;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.activeInHierarchy)
			{
				float num3 = Vector3.SqrMagnitude(child.position - vector4);
				if (num3 < num)
				{
					num = num3;
					target = child;
					num2 = i;
				}
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transform.GetChild(num2))
		{
			Vector2 totalDelta = UICamera.currentTouch.totalDelta;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num4;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num4 = totalDelta.magnitude;
				}
				else
				{
					num4 = totalDelta.y;
				}
			}
			else
			{
				num4 = totalDelta.x;
			}
			if (Mathf.Abs(num4) > this.nextPageThreshold)
			{
				UIGrid component = base.GetComponent<UIGrid>();
				if (component != null && component.sorting != UIGrid.Sorting.None)
				{
					List<Transform> childList = component.GetChildList();
					if (num4 > this.nextPageThreshold)
					{
						if (num2 > 0)
						{
							target = childList[num2 - 1];
						}
						else
						{
							target = childList[0];
						}
					}
					else if (num4 < -this.nextPageThreshold)
					{
						if (num2 < childList.Count - 1)
						{
							target = childList[num2 + 1];
						}
						else
						{
							target = childList[childList.Count - 1];
						}
					}
				}
				else
				{
					Debug.LogWarning("Next Page Threshold requires a sorted UIGrid in order to work properly", this);
				}
			}
		}
		this.CenterOn(target, vector);
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000D2CE0 File Offset: 0x000D0EE0
	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
			Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 vector3 = vector - vector2;
			if (!this.mScrollView.canMoveHorizontally)
			{
				vector3.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				vector3.y = 0f;
			}
			vector3.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - vector3, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000D2DF8 File Offset: 0x000D0FF8
	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}

	// Token: 0x04001F43 RID: 8003
	public float springStrength = 8f;

	// Token: 0x04001F44 RID: 8004
	public float nextPageThreshold;

	// Token: 0x04001F45 RID: 8005
	public SpringPanel.OnFinished onFinished;

	// Token: 0x04001F46 RID: 8006
	public UICenterOnChild.OnCenterCallback onCenter;

	// Token: 0x04001F47 RID: 8007
	private UIScrollView mScrollView;

	// Token: 0x04001F48 RID: 8008
	private GameObject mCenteredObject;

	// Token: 0x0200044F RID: 1103
	// (Invoke) Token: 0x06001A82 RID: 6786
	public delegate void OnCenterCallback(GameObject centeredObject);
}
