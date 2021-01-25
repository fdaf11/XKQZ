using System;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
public class Billboard : MonoBehaviour
{
	// Token: 0x06002523 RID: 9507 RVA: 0x00121BCC File Offset: 0x0011FDCC
	private void Awake()
	{
		if (this.AutoInitCamera)
		{
			this.Camera = Camera.main;
			this.Active = true;
		}
		this.t = base.transform;
		this.camT = this.Camera.transform;
		Transform parent = this.t.parent;
		this.myContainer = new GameObject
		{
			name = "Billboard_" + this.t.gameObject.name
		};
		this.contT = this.myContainer.transform;
		this.contT.position = this.t.position;
		this.t.parent = this.myContainer.transform;
		this.contT.parent = parent;
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x00121C98 File Offset: 0x0011FE98
	private void Update()
	{
		if (this.Active)
		{
			this.contT.LookAt(this.contT.position + this.camT.rotation * Vector3.back, this.camT.rotation * Vector3.up);
		}
	}

	// Token: 0x04002D5B RID: 11611
	public Camera Camera;

	// Token: 0x04002D5C RID: 11612
	public bool Active = true;

	// Token: 0x04002D5D RID: 11613
	public bool AutoInitCamera = true;

	// Token: 0x04002D5E RID: 11614
	private GameObject myContainer;

	// Token: 0x04002D5F RID: 11615
	private Transform t;

	// Token: 0x04002D60 RID: 11616
	private Transform camT;

	// Token: 0x04002D61 RID: 11617
	private Transform contT;
}
