using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	// Token: 0x06001AB4 RID: 6836 RVA: 0x000D3CF4 File Offset: 0x000D1EF4
	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, ref num))
			{
				this.mRayPos = currentRay.GetPoint(num);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000D3DB4 File Offset: 0x000D1FB4
	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, ref num))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 vector = currentRay.GetPoint(num) - this.mRayPos;
				cachedTransform.position += vector;
				Vector3 vector2 = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector2.x, vector2.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
			}
		}
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x00011995 File Offset: 0x0000FB95
	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	// Token: 0x04001F7A RID: 8058
	public UIWidget target;

	// Token: 0x04001F7B RID: 8059
	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	// Token: 0x04001F7C RID: 8060
	public int minWidth = 100;

	// Token: 0x04001F7D RID: 8061
	public int minHeight = 100;

	// Token: 0x04001F7E RID: 8062
	public int maxWidth = 100000;

	// Token: 0x04001F7F RID: 8063
	public int maxHeight = 100000;

	// Token: 0x04001F80 RID: 8064
	private Plane mPlane;

	// Token: 0x04001F81 RID: 8065
	private Vector3 mRayPos;

	// Token: 0x04001F82 RID: 8066
	private Vector3 mLocalPos;

	// Token: 0x04001F83 RID: 8067
	private int mWidth;

	// Token: 0x04001F84 RID: 8068
	private int mHeight;

	// Token: 0x04001F85 RID: 8069
	private bool mDragging;
}
