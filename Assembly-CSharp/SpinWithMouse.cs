using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	// Token: 0x06001A11 RID: 6673 RVA: 0x00011021 File Offset: 0x0000F221
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x000D09EC File Offset: 0x000CEBEC
	private void OnDrag(Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		if (this.target != null)
		{
			this.target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.target.localRotation;
		}
		else
		{
			this.mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.mTrans.localRotation;
		}
	}

	// Token: 0x04001ED7 RID: 7895
	public Transform target;

	// Token: 0x04001ED8 RID: 7896
	public float speed = 1f;

	// Token: 0x04001ED9 RID: 7897
	private Transform mTrans;
}
