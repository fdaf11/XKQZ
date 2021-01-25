using System;
using UnityEngine;

// Token: 0x02000434 RID: 1076
[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x060019FD RID: 6653 RVA: 0x00010EFB File Offset: 0x0000F0FB
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000D03CC File Offset: 0x000CE5CC
	private void LateUpdate()
	{
		if (this.target != null)
		{
			Vector3 vector = this.target.position - this.mTrans.position;
			float magnitude = vector.magnitude;
			if (magnitude > 0.001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(vector);
				this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, quaternion, Mathf.Clamp01(this.speed * Time.deltaTime));
			}
		}
	}

	// Token: 0x04001EC2 RID: 7874
	public int level;

	// Token: 0x04001EC3 RID: 7875
	public Transform target;

	// Token: 0x04001EC4 RID: 7876
	public float speed = 8f;

	// Token: 0x04001EC5 RID: 7877
	private Transform mTrans;
}
