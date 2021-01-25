using System;
using UnityEngine;

// Token: 0x02000431 RID: 1073
[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : MonoBehaviour
{
	// Token: 0x060019F1 RID: 6641 RVA: 0x00010DFA File Offset: 0x0000EFFA
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000D027C File Offset: 0x000CE47C
	private void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			Vector3 vector = parent.position + parent.rotation * this.mRelative;
			this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(delta * this.speed.x));
			this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(delta * this.speed.y));
			this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(delta * this.speed.z));
			this.mTrans.position = this.mAbsolute;
		}
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x00010E07 File Offset: 0x0000F007
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mAbsolute = this.mTrans.position;
		this.mRelative = this.mTrans.localPosition;
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00010E37 File Offset: 0x0000F037
	private void Update()
	{
		this.Interpolate((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
	}

	// Token: 0x04001EB7 RID: 7863
	public Vector3 speed = new Vector3(10f, 10f, 10f);

	// Token: 0x04001EB8 RID: 7864
	public bool ignoreTimeScale;

	// Token: 0x04001EB9 RID: 7865
	private Transform mTrans;

	// Token: 0x04001EBA RID: 7866
	private Vector3 mRelative;

	// Token: 0x04001EBB RID: 7867
	private Vector3 mAbsolute;
}
