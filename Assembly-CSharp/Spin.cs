using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
	// Token: 0x06001A0C RID: 6668 RVA: 0x00010FA3 File Offset: 0x0000F1A3
	private void Start()
	{
		this.mTrans = base.transform;
		this.mRb = base.rigidbody;
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x00010FBD File Offset: 0x0000F1BD
	private void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
		}
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x00010FF0 File Offset: 0x0000F1F0
	private void FixedUpdate()
	{
		if (this.mRb != null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x000D0974 File Offset: 0x000CEB74
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion quaternion = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			this.mTrans.rotation = this.mTrans.rotation * quaternion;
		}
		else
		{
			this.mRb.MoveRotation(this.mRb.rotation * quaternion);
		}
	}

	// Token: 0x04001ED3 RID: 7891
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04001ED4 RID: 7892
	public bool ignoreTimeScale;

	// Token: 0x04001ED5 RID: 7893
	private Rigidbody mRb;

	// Token: 0x04001ED6 RID: 7894
	private Transform mTrans;
}
