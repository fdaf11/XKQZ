using System;
using UnityEngine;

// Token: 0x02000819 RID: 2073
[ExecuteInEditMode]
[AddComponentMenu("Time of Day/Camera Main Script")]
[RequireComponent(typeof(Camera))]
public class TOD_Camera : MonoBehaviour
{
	// Token: 0x060032D4 RID: 13012 RVA: 0x00187EDC File Offset: 0x001860DC
	protected void OnEnable()
	{
		this.cameraComponent = base.GetComponent<Camera>();
		this.cameraTransform = base.GetComponent<Transform>();
		if (!this.sky)
		{
			this.sky = (Object.FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky);
		}
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x0001FDBC File Offset: 0x0001DFBC
	protected void OnPreCull()
	{
		if (!this.sky)
		{
			return;
		}
		if (this.DomeScaleToFarClip)
		{
			this.DoDomeScaleToFarClip();
		}
		if (this.DomePosToCamera)
		{
			this.DoDomePosToCamera();
		}
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x00187F2C File Offset: 0x0018612C
	public void DoDomeScaleToFarClip()
	{
		float num = this.DomeScaleFactor * this.cameraComponent.farClipPlane;
		Vector3 localScale;
		localScale..ctor(num, num, num);
		this.sky.Components.DomeTransform.localScale = localScale;
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00187F6C File Offset: 0x0018616C
	public void DoDomePosToCamera()
	{
		Vector3 position = this.cameraTransform.position;
		this.sky.Components.DomeTransform.position = position;
	}

	// Token: 0x04003E45 RID: 15941
	public TOD_Sky sky;

	// Token: 0x04003E46 RID: 15942
	public bool DomePosToCamera = true;

	// Token: 0x04003E47 RID: 15943
	public bool DomeScaleToFarClip = true;

	// Token: 0x04003E48 RID: 15944
	public float DomeScaleFactor = 0.95f;

	// Token: 0x04003E49 RID: 15945
	private Camera cameraComponent;

	// Token: 0x04003E4A RID: 15946
	private Transform cameraTransform;
}
