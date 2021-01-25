using System;
using UnityEngine;

// Token: 0x02000520 RID: 1312
[AddComponentMenu("Camera-Control/OrbitCam-Keyboard")]
public class OrbitCamKeys : MonoBehaviour
{
	// Token: 0x060021A7 RID: 8615 RVA: 0x00016916 File Offset: 0x00014B16
	protected void Reset()
	{
		this.AllowRotate = true;
		this.RotateSpeed = 100f;
		this.AllowZoom = true;
		this.ZoomSpeed = 10f;
		this.AllowTilt = true;
		this.TiltSpeed = 50f;
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x0001694E File Offset: 0x00014B4E
	protected void Start()
	{
		this._orbitCamera = base.gameObject.GetComponent<OrbitCam>();
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000FDC58 File Offset: 0x000FBE58
	protected void Update()
	{
		if (this._orbitCamera == null)
		{
			return;
		}
		if (this.AllowRotate)
		{
			float axisRaw = Input.GetAxisRaw(this.RotateInputAxis);
			if (Mathf.Abs(axisRaw) > 0.001f)
			{
				this._orbitCamera.Rotation += axisRaw * this.RotateSpeed * Time.deltaTime;
			}
		}
		if (this.AllowZoom)
		{
			if (this.ZoomUsesInputAxis)
			{
				float axisRaw2 = Input.GetAxisRaw(this.ZoomInputAxis);
				if (Mathf.Abs(axisRaw2) > 0.001f)
				{
					this._orbitCamera.Distance += axisRaw2 * this.ZoomSpeed * Time.deltaTime;
				}
			}
			else
			{
				if (Input.GetKey(this.ZoomOutKey))
				{
					this._orbitCamera.Distance += this.ZoomSpeed * Time.deltaTime;
				}
				if (Input.GetKey(this.ZoomInKey))
				{
					this._orbitCamera.Distance -= this.ZoomSpeed * Time.deltaTime;
				}
			}
		}
		if (this.AllowTilt)
		{
			float axisRaw3 = Input.GetAxisRaw(this.TiltInputAxis);
			if (Mathf.Abs(axisRaw3) > 0.001f)
			{
				this._orbitCamera.Tilt += axisRaw3 * this.TiltSpeed * Time.deltaTime;
			}
		}
	}

	// Token: 0x040024EC RID: 9452
	public bool AllowRotate;

	// Token: 0x040024ED RID: 9453
	public float RotateSpeed;

	// Token: 0x040024EE RID: 9454
	public bool AllowZoom;

	// Token: 0x040024EF RID: 9455
	public float ZoomSpeed;

	// Token: 0x040024F0 RID: 9456
	public bool AllowTilt;

	// Token: 0x040024F1 RID: 9457
	public float TiltSpeed;

	// Token: 0x040024F2 RID: 9458
	public string RotateInputAxis = "Horizontal";

	// Token: 0x040024F3 RID: 9459
	public bool ZoomUsesInputAxis;

	// Token: 0x040024F4 RID: 9460
	public string ZoomInputAxis = "KbCameraZoom";

	// Token: 0x040024F5 RID: 9461
	public KeyCode ZoomOutKey = 102;

	// Token: 0x040024F6 RID: 9462
	public KeyCode ZoomInKey = 114;

	// Token: 0x040024F7 RID: 9463
	public string TiltInputAxis = "Vertical";

	// Token: 0x040024F8 RID: 9464
	private OrbitCam _orbitCamera;
}
