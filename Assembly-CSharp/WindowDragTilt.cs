using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	// Token: 0x06001A23 RID: 6691 RVA: 0x00011134 File Offset: 0x0000F334
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mLastPos = this.mTrans.position;
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000D128C File Offset: 0x000CF48C
	private void Update()
	{
		Vector3 vector = this.mTrans.position - this.mLastPos;
		this.mLastPos = this.mTrans.position;
		this.mAngle += vector.x * this.degrees;
		this.mAngle = NGUIMath.SpringLerp(this.mAngle, 0f, 20f, Time.deltaTime);
		this.mTrans.localRotation = Quaternion.Euler(0f, 0f, -this.mAngle);
	}

	// Token: 0x04001EF4 RID: 7924
	public int updateOrder;

	// Token: 0x04001EF5 RID: 7925
	public float degrees = 30f;

	// Token: 0x04001EF6 RID: 7926
	private Vector3 mLastPos;

	// Token: 0x04001EF7 RID: 7927
	private Transform mTrans;

	// Token: 0x04001EF8 RID: 7928
	private float mAngle;
}
