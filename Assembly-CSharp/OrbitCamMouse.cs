using System;
using UnityEngine;

// Token: 0x02000521 RID: 1313
[AddComponentMenu("Camera-Control/OrbitCam-Mouse")]
public class OrbitCamMouse : MonoBehaviour
{
	// Token: 0x060021AB RID: 8619 RVA: 0x000FDE08 File Offset: 0x000FC008
	protected void Reset()
	{
		this.MouseOrbitButton = 324;
		this.AlwaysOrbit = false;
		this.LockCursorOnMouseDown = true;
		this.AllowRotate = true;
		this.RotateSpeed = 200f;
		this.AllowTilt = true;
		this.TiltSpeed = 100f;
		this.AllowZoom = true;
		this.ZoomSpeed = 500f;
		this.RotateInputAxis = "Mouse X";
		this.TiltInputAxis = "Mouse Y";
		this.ZoomInputAxis = "Mouse ScrollWheel";
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x00016961 File Offset: 0x00014B61
	protected void Start()
	{
		this._orbitCamera = base.gameObject.GetComponent<OrbitCam>();
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000FDE88 File Offset: 0x000FC088
	protected void Update()
	{
		if (this._orbitCamera == null)
		{
			return;
		}
		if (GameGlobal.m_bCFormOpen)
		{
			return;
		}
		if (this.AllowZoom)
		{
			float axisRaw = Input.GetAxisRaw(this.ZoomInputAxis);
			this._orbitCamera.Distance -= axisRaw * this.ZoomSpeed * Time.deltaTime;
		}
		if (this.AlwaysOrbit || Input.GetKey(this.MouseOrbitButton))
		{
			if (this.LockCursorOnMouseDown)
			{
				Screen.lockCursor = true;
			}
			if (this.AllowTilt)
			{
				float axisRaw2 = Input.GetAxisRaw(this.TiltInputAxis);
				this._orbitCamera.Tilt -= axisRaw2 * this.TiltSpeed * Time.deltaTime;
			}
			if (this.AllowRotate)
			{
				float axisRaw3 = Input.GetAxisRaw(this.RotateInputAxis);
				this._orbitCamera.Rotation += axisRaw3 * this.RotateSpeed * Time.deltaTime;
			}
		}
		else if (this.LockCursorOnMouseDown)
		{
			Screen.lockCursor = false;
		}
	}

	// Token: 0x040024F9 RID: 9465
	public KeyCode MouseOrbitButton;

	// Token: 0x040024FA RID: 9466
	public bool AlwaysOrbit;

	// Token: 0x040024FB RID: 9467
	public bool LockCursorOnMouseDown = true;

	// Token: 0x040024FC RID: 9468
	public bool AllowRotate = true;

	// Token: 0x040024FD RID: 9469
	public float RotateSpeed;

	// Token: 0x040024FE RID: 9470
	public bool AllowTilt = true;

	// Token: 0x040024FF RID: 9471
	public float TiltSpeed;

	// Token: 0x04002500 RID: 9472
	public bool AllowZoom = true;

	// Token: 0x04002501 RID: 9473
	public float ZoomSpeed;

	// Token: 0x04002502 RID: 9474
	public string RotateInputAxis = "Mouse X";

	// Token: 0x04002503 RID: 9475
	public string TiltInputAxis = "Mouse Y";

	// Token: 0x04002504 RID: 9476
	public string ZoomInputAxis = "Mouse ScrollWheel";

	// Token: 0x04002505 RID: 9477
	private OrbitCam _orbitCamera;
}
