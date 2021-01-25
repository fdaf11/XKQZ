using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
	// Token: 0x06001A1F RID: 6687 RVA: 0x000110DA File Offset: 0x0000F2DA
	private void OnDisable()
	{
		this.mTrans.localRotation = Quaternion.identity;
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x000110EC File Offset: 0x0000F2EC
	private void OnEnable()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mTrans = base.transform;
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000D1220 File Offset: 0x000CF420
	private void Update()
	{
		if (this.uiCamera != null)
		{
			Vector3 vector = this.uiCamera.WorldToViewportPoint(this.mTrans.position);
			this.mTrans.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * this.yawAmount, 0f);
		}
	}

	// Token: 0x04001EF0 RID: 7920
	public int updateOrder;

	// Token: 0x04001EF1 RID: 7921
	public Camera uiCamera;

	// Token: 0x04001EF2 RID: 7922
	public float yawAmount = 20f;

	// Token: 0x04001EF3 RID: 7923
	private Transform mTrans;
}
