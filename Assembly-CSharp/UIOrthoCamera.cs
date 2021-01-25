using System;
using UnityEngine;

// Token: 0x020004FF RID: 1279
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Orthographic Camera")]
[ExecuteInEditMode]
public class UIOrthoCamera : MonoBehaviour
{
	// Token: 0x0600209E RID: 8350 RVA: 0x00015E12 File Offset: 0x00014012
	private void Start()
	{
		this.mCam = base.camera;
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x000F6728 File Offset: 0x000F4928
	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = this.mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num3))
		{
			this.mCam.orthographicSize = num3;
		}
	}

	// Token: 0x040023F5 RID: 9205
	private Camera mCam;

	// Token: 0x040023F6 RID: 9206
	private Transform mTrans;
}
