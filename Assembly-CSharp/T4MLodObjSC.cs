using System;
using UnityEngine;

// Token: 0x02000714 RID: 1812
public class T4MLodObjSC : MonoBehaviour
{
	// Token: 0x06002B06 RID: 11014 RVA: 0x0014E7EC File Offset: 0x0014C9EC
	public void ActivateLODScrpt()
	{
		if (this.Mode != 2)
		{
			return;
		}
		if (this.PlayerCamera == null)
		{
			this.PlayerCamera = Camera.main.transform;
		}
		base.InvokeRepeating("AFLODScrpt", Random.Range(0f, this.Interval), this.Interval);
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x0014E848 File Offset: 0x0014CA48
	public void ActivateLODLay()
	{
		if (this.Mode != 2)
		{
			return;
		}
		if (this.PlayerCamera == null)
		{
			this.PlayerCamera = Camera.main.transform;
		}
		base.InvokeRepeating("AFLODLay", Random.Range(0f, this.Interval), this.Interval);
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x0014E8A4 File Offset: 0x0014CAA4
	public void AFLODLay()
	{
		if (this.OldPlayerPos == this.PlayerCamera.position)
		{
			return;
		}
		this.OldPlayerPos = this.PlayerCamera.position;
		float num = Vector3.Distance(new Vector3(base.transform.position.x, this.PlayerCamera.position.y, base.transform.position.z), this.PlayerCamera.position);
		int layer = base.gameObject.layer;
		if (num <= this.PlayerCamera.camera.layerCullDistances[layer] + 5f)
		{
			if (num < this.LOD2Start && this.ObjLodStatus != 1)
			{
				Renderer lod = this.LOD3;
				bool enabled = false;
				this.LOD2.enabled = enabled;
				lod.enabled = enabled;
				this.LOD1.enabled = true;
				this.ObjLodStatus = 1;
			}
			else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus != 2)
			{
				Renderer lod2 = this.LOD1;
				bool enabled = false;
				this.LOD3.enabled = enabled;
				lod2.enabled = enabled;
				this.LOD2.enabled = true;
				this.ObjLodStatus = 2;
			}
			else if (num >= this.LOD3Start && this.ObjLodStatus != 3)
			{
				Renderer lod3 = this.LOD1;
				bool enabled = false;
				this.LOD2.enabled = enabled;
				lod3.enabled = enabled;
				this.LOD3.enabled = true;
				this.ObjLodStatus = 3;
			}
		}
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x0014EA48 File Offset: 0x0014CC48
	public void AFLODScrpt()
	{
		if (this.OldPlayerPos == this.PlayerCamera.position)
		{
			return;
		}
		this.OldPlayerPos = this.PlayerCamera.position;
		float num = Vector3.Distance(new Vector3(base.transform.position.x, this.PlayerCamera.position.y, base.transform.position.z), this.PlayerCamera.position);
		if (num <= this.MaxViewDistance)
		{
			if (num < this.LOD2Start && this.ObjLodStatus != 1)
			{
				Renderer lod = this.LOD3;
				bool flag = false;
				this.LOD2.enabled = flag;
				lod.enabled = flag;
				this.LOD1.enabled = true;
				this.ObjLodStatus = 1;
			}
			else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus != 2)
			{
				Renderer lod2 = this.LOD1;
				bool flag = false;
				this.LOD3.enabled = flag;
				lod2.enabled = flag;
				this.LOD2.enabled = true;
				this.ObjLodStatus = 2;
			}
			else if (num >= this.LOD3Start && this.ObjLodStatus != 3)
			{
				Renderer lod3 = this.LOD1;
				bool flag = false;
				this.LOD2.enabled = flag;
				lod3.enabled = flag;
				this.LOD3.enabled = true;
				this.ObjLodStatus = 3;
			}
		}
		else if (this.ObjLodStatus != 0)
		{
			Renderer lod4 = this.LOD1;
			bool flag = false;
			this.LOD3.enabled = flag;
			flag = flag;
			this.LOD2.enabled = flag;
			lod4.enabled = flag;
			this.ObjLodStatus = 0;
		}
	}

	// Token: 0x04003744 RID: 14148
	public Renderer LOD1;

	// Token: 0x04003745 RID: 14149
	public Renderer LOD2;

	// Token: 0x04003746 RID: 14150
	public Renderer LOD3;

	// Token: 0x04003747 RID: 14151
	[HideInInspector]
	public float Interval = 0.5f;

	// Token: 0x04003748 RID: 14152
	[HideInInspector]
	public Transform PlayerCamera;

	// Token: 0x04003749 RID: 14153
	[HideInInspector]
	public int Mode;

	// Token: 0x0400374A RID: 14154
	private Vector3 OldPlayerPos;

	// Token: 0x0400374B RID: 14155
	[HideInInspector]
	public int ObjLodStatus;

	// Token: 0x0400374C RID: 14156
	[HideInInspector]
	public float MaxViewDistance = 60f;

	// Token: 0x0400374D RID: 14157
	[HideInInspector]
	public float LOD2Start = 20f;

	// Token: 0x0400374E RID: 14158
	[HideInInspector]
	public float LOD3Start = 40f;
}
