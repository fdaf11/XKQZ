using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Viewport Camera")]
public class UIViewport : MonoBehaviour
{
	// Token: 0x06002164 RID: 8548 RVA: 0x0001670E File Offset: 0x0001490E
	private void Start()
	{
		this.mCam = base.camera;
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000FBA48 File Offset: 0x000F9C48
	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
			Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
			Rect rect;
			rect..ctor(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
			float num = this.fullSize * rect.height;
			if (rect != this.mCam.rect)
			{
				this.mCam.rect = rect;
			}
			if (this.mCam.orthographicSize != num)
			{
				this.mCam.orthographicSize = num;
			}
		}
	}

	// Token: 0x0400248F RID: 9359
	public Camera sourceCamera;

	// Token: 0x04002490 RID: 9360
	public Transform topLeft;

	// Token: 0x04002491 RID: 9361
	public Transform bottomRight;

	// Token: 0x04002492 RID: 9362
	public float fullSize = 1f;

	// Token: 0x04002493 RID: 9363
	private Camera mCam;
}
