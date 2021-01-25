using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : MonoBehaviour
{
	// Token: 0x06001A02 RID: 6658 RVA: 0x00010F3C File Offset: 0x0000F13C
	private void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x000D0490 File Offset: 0x000CE690
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (this.range < 0.1f)
		{
			this.range = 0.1f;
		}
		float num3 = Mathf.Clamp((mousePosition.x - num) / num / this.range, -1f, 1f);
		float num4 = Mathf.Clamp((mousePosition.y - num2) / num2 / this.range, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(num3, num4), deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0f);
	}

	// Token: 0x04001EC6 RID: 7878
	public Vector2 degrees = new Vector2(5f, 3f);

	// Token: 0x04001EC7 RID: 7879
	public float range = 1f;

	// Token: 0x04001EC8 RID: 7880
	private Transform mTrans;

	// Token: 0x04001EC9 RID: 7881
	private Quaternion mStart;

	// Token: 0x04001ECA RID: 7882
	private Vector2 mRot = Vector2.zero;
}
