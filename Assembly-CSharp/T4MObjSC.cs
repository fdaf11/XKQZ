using System;
using UnityEngine;

// Token: 0x02000715 RID: 1813
[ExecuteInEditMode]
public class T4MObjSC : MonoBehaviour
{
	// Token: 0x06002B0B RID: 11019 RVA: 0x0014ED28 File Offset: 0x0014CF28
	public void Awake()
	{
		if (this.Master == 1)
		{
			if (this.PlayerCamera == null && Camera.main)
			{
				this.PlayerCamera = Camera.main.transform;
			}
			else if (this.PlayerCamera == null && !Camera.main)
			{
				Camera[] array = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].GetComponent<AudioListener>())
					{
						this.PlayerCamera = array[i].transform;
					}
				}
			}
			if (this.enabledLayerCul)
			{
				this.distances[26] = this.CloseView;
				this.distances[27] = this.NormalView;
				this.distances[28] = this.FarView;
				this.distances[29] = this.BackGroundView;
				this.PlayerCamera.camera.layerCullDistances = this.distances;
			}
			if (this.EnabledLODSystem && this.ObjPosition.Length > 0 && this.Mode == 1)
			{
				if (this.ObjLodScript[0].gameObject != null)
				{
					if (this.LODbasedOnScript)
					{
						base.InvokeRepeating("LODScript", Random.Range(0f, this.Interval), this.Interval);
					}
					else
					{
						base.InvokeRepeating("LODLay", Random.Range(0f, this.Interval), this.Interval);
					}
				}
			}
			else if (this.EnabledLODSystem && this.ObjPosition.Length > 0 && this.Mode == 2 && this.ObjLodScript[0] != null)
			{
				for (int j = 0; j < this.ObjPosition.Length; j++)
				{
					if (this.ObjLodScript[j] != null)
					{
						if (this.LODbasedOnScript)
						{
							this.ObjLodScript[j].ActivateLODScrpt();
						}
						else
						{
							this.ObjLodScript[j].ActivateLODLay();
						}
					}
				}
			}
			if (this.enabledBillboard && this.BillboardPosition.Length > 0 && this.BillScript[0] != null)
			{
				if (this.BilBbasedOnScript)
				{
					base.InvokeRepeating("BillScrpt", Random.Range(0f, this.BillInterval), this.BillInterval);
				}
				else
				{
					base.InvokeRepeating("BillLay", Random.Range(0f, this.BillInterval), this.BillInterval);
				}
			}
		}
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x0014EFDC File Offset: 0x0014D1DC
	private void OnGUI()
	{
		if (!Application.isPlaying && this.Master == 1)
		{
			if (this.LayerCullPreview && this.enabledLayerCul)
			{
				GUI.color = Color.green;
				GUI.Label(new Rect(0f, 0f, 200f, 200f), "LayerCull Preview ON");
			}
			else
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(0f, 0f, 200f, 200f), "LayerCull Preview OFF");
			}
			if (this.LODPreview && this.ObjPosition.Length > 0)
			{
				GUI.color = Color.green;
				GUI.Label(new Rect(0f, 20f, 200f, 200f), "LOD Preview ON");
			}
			else if (this.LODPreview && this.ObjPosition.Length == 0)
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(0f, 20f, 200f, 200f), "Activate the LOD First");
			}
			else
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(0f, 20f, 200f, 200f), "LOD Preview OFF");
			}
			if (this.BillboardPreview && this.BillboardPosition.Length > 0)
			{
				GUI.color = Color.green;
				GUI.Label(new Rect(0f, 40f, 200f, 200f), "Billboard Preview ON");
			}
			else if (this.BillboardPreview && this.BillboardPosition.Length == 0)
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(0f, 40f, 200f, 200f), "Activate the Billboard First");
			}
			else
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(0f, 40f, 200f, 200f), "Billboard Preview OFF");
			}
		}
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x0014F1F8 File Offset: 0x0014D3F8
	private void LateUpdate()
	{
		if (this.ActiveWind)
		{
			Color color = this.Wind * Mathf.Sin(Time.realtimeSinceStartup * this.WindFrequency);
			color.a = this.Wind.w;
			Color color2 = this.Wind * Mathf.Sin(Time.realtimeSinceStartup * this.GrassWindFrequency);
			color2.a = this.Wind.w;
			Shader.SetGlobalColor("_Wind", color);
			Shader.SetGlobalColor("_GrassWind", color2);
			Shader.SetGlobalColor("_TranslucencyColor", this.TranslucencyColor);
			Shader.SetGlobalFloat("_TranslucencyViewDependency;", 0.65f);
		}
		if (this.PlayerCamera && !Application.isPlaying && this.Master == 1)
		{
			if (this.LayerCullPreview && this.enabledLayerCul)
			{
				this.distances[26] = this.CloseView;
				this.distances[27] = this.NormalView;
				this.distances[28] = this.FarView;
				this.distances[29] = this.BackGroundView;
				this.PlayerCamera.camera.layerCullDistances = this.distances;
			}
			else
			{
				this.distances[26] = this.PlayerCamera.camera.farClipPlane;
				this.distances[27] = this.PlayerCamera.camera.farClipPlane;
				this.distances[28] = this.PlayerCamera.camera.farClipPlane;
				this.distances[29] = this.PlayerCamera.camera.farClipPlane;
				this.PlayerCamera.camera.layerCullDistances = this.distances;
			}
			if (this.LODPreview)
			{
				if (this.EnabledLODSystem && this.ObjPosition.Length > 0 && this.Mode == 1)
				{
					if (this.ObjLodScript[0].gameObject != null)
					{
						if (this.LODbasedOnScript)
						{
							this.LODScript();
						}
						else
						{
							this.LODLay();
						}
					}
				}
				else if (this.EnabledLODSystem && this.ObjPosition.Length > 0 && this.Mode == 2 && this.ObjLodScript[0] != null)
				{
					for (int i = 0; i < this.ObjPosition.Length; i++)
					{
						if (this.ObjLodScript[i] != null)
						{
							if (this.LODbasedOnScript)
							{
								this.ObjLodScript[i].AFLODScrpt();
							}
							else
							{
								this.ObjLodScript[i].AFLODLay();
							}
						}
					}
				}
			}
			if (this.BillboardPreview && this.enabledBillboard && this.BillboardPosition.Length > 0 && this.BillScript[0] != null)
			{
				if (this.BilBbasedOnScript)
				{
					this.BillScrpt();
				}
				else
				{
					this.BillLay();
				}
			}
		}
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x0014F504 File Offset: 0x0014D704
	private void BillScrpt()
	{
		for (int i = 0; i < this.BillboardPosition.Length; i++)
		{
			if (Vector3.Distance(this.BillboardPosition[i], this.PlayerCamera.position) <= this.BillMaxViewDistance)
			{
				if (this.BillStatus[i] != 1)
				{
					this.BillScript[i].Render.enabled = true;
					this.BillStatus[i] = 1;
				}
				if (this.Axis == 0)
				{
					this.BillScript[i].Transf.LookAt(new Vector3(this.PlayerCamera.position.x, this.BillScript[i].Transf.position.y, this.PlayerCamera.position.z), Vector3.up);
				}
				else
				{
					this.BillScript[i].Transf.LookAt(this.PlayerCamera.position, Vector3.up);
				}
			}
			else if (this.BillStatus[i] != 0 && !this.BillScript[i].Render.enabled)
			{
				this.BillScript[i].Render.enabled = false;
				this.BillStatus[i] = 0;
			}
		}
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x0014F654 File Offset: 0x0014D854
	private void BillLay()
	{
		for (int i = 0; i < this.BillboardPosition.Length; i++)
		{
			int layer = this.BillScript[i].gameObject.layer;
			if (Vector3.Distance(this.BillboardPosition[i], this.PlayerCamera.position) <= this.distances[layer])
			{
				if (this.Axis == 0)
				{
					this.BillScript[i].Transf.LookAt(new Vector3(this.PlayerCamera.position.x, this.BillScript[i].Transf.position.y, this.PlayerCamera.position.z), Vector3.up);
				}
				else
				{
					this.BillScript[i].Transf.LookAt(this.PlayerCamera.position, Vector3.up);
				}
			}
		}
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x0014F74C File Offset: 0x0014D94C
	private void LODScript()
	{
		if (this.OldPlayerPos == this.PlayerCamera.position)
		{
			return;
		}
		this.OldPlayerPos = this.PlayerCamera.position;
		for (int i = 0; i < this.ObjPosition.Length; i++)
		{
			float num = Vector3.Distance(new Vector3(this.ObjPosition[i].x, this.PlayerCamera.position.y, this.ObjPosition[i].z), this.PlayerCamera.position);
			if (num <= this.MaxViewDistance)
			{
				if (num < this.LOD2Start && this.ObjLodStatus[i] != 1)
				{
					Renderer lod = this.ObjLodScript[i].LOD2;
					bool flag = false;
					this.ObjLodScript[i].LOD3.enabled = flag;
					lod.enabled = flag;
					this.ObjLodScript[i].LOD1.enabled = true;
					this.ObjLodStatus[i] = 1;
				}
				else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus[i] != 2)
				{
					Renderer lod2 = this.ObjLodScript[i].LOD1;
					bool flag = false;
					this.ObjLodScript[i].LOD3.enabled = flag;
					lod2.enabled = flag;
					this.ObjLodScript[i].LOD2.enabled = true;
					this.ObjLodStatus[i] = 2;
				}
				else if (num >= this.LOD3Start && this.ObjLodStatus[i] != 3)
				{
					Renderer lod3 = this.ObjLodScript[i].LOD2;
					bool flag = false;
					this.ObjLodScript[i].LOD1.enabled = flag;
					lod3.enabled = flag;
					this.ObjLodScript[i].LOD3.enabled = true;
					this.ObjLodStatus[i] = 3;
				}
			}
			else if (this.ObjLodStatus[i] != 0)
			{
				Renderer lod4 = this.ObjLodScript[i].LOD1;
				bool flag = false;
				this.ObjLodScript[i].LOD3.enabled = flag;
				flag = flag;
				this.ObjLodScript[i].LOD2.enabled = flag;
				lod4.enabled = flag;
				this.ObjLodStatus[i] = 0;
			}
		}
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x0014F980 File Offset: 0x0014DB80
	private void LODLay()
	{
		if (this.OldPlayerPos == this.PlayerCamera.position)
		{
			return;
		}
		this.OldPlayerPos = this.PlayerCamera.position;
		for (int i = 0; i < this.ObjPosition.Length; i++)
		{
			float num = Vector3.Distance(new Vector3(this.ObjPosition[i].x, this.PlayerCamera.position.y, this.ObjPosition[i].z), this.PlayerCamera.position);
			int layer = this.ObjLodScript[i].gameObject.layer;
			if (num <= this.distances[layer] + 5f)
			{
				if (num < this.LOD2Start && this.ObjLodStatus[i] != 1)
				{
					Renderer lod = this.ObjLodScript[i].LOD2;
					bool enabled = false;
					this.ObjLodScript[i].LOD3.enabled = enabled;
					lod.enabled = enabled;
					this.ObjLodScript[i].LOD1.enabled = true;
					this.ObjLodStatus[i] = 1;
				}
				else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus[i] != 2)
				{
					Renderer lod2 = this.ObjLodScript[i].LOD1;
					bool enabled = false;
					this.ObjLodScript[i].LOD3.enabled = enabled;
					lod2.enabled = enabled;
					this.ObjLodScript[i].LOD2.enabled = true;
					this.ObjLodStatus[i] = 2;
				}
				else if (num >= this.LOD3Start && this.ObjLodStatus[i] != 3)
				{
					Renderer lod3 = this.ObjLodScript[i].LOD2;
					bool enabled = false;
					this.ObjLodScript[i].LOD1.enabled = enabled;
					lod3.enabled = enabled;
					this.ObjLodScript[i].LOD3.enabled = true;
					this.ObjLodStatus[i] = 3;
				}
			}
		}
	}

	// Token: 0x0400374F RID: 14159
	[HideInInspector]
	public string ConvertType = string.Empty;

	// Token: 0x04003750 RID: 14160
	[HideInInspector]
	public bool EnabledLODSystem = true;

	// Token: 0x04003751 RID: 14161
	[HideInInspector]
	public Vector3[] ObjPosition;

	// Token: 0x04003752 RID: 14162
	[HideInInspector]
	public T4MLodObjSC[] ObjLodScript;

	// Token: 0x04003753 RID: 14163
	[HideInInspector]
	public int[] ObjLodStatus;

	// Token: 0x04003754 RID: 14164
	[HideInInspector]
	public float MaxViewDistance = 60f;

	// Token: 0x04003755 RID: 14165
	[HideInInspector]
	public float LOD2Start = 20f;

	// Token: 0x04003756 RID: 14166
	[HideInInspector]
	public float LOD3Start = 40f;

	// Token: 0x04003757 RID: 14167
	[HideInInspector]
	public float Interval = 0.5f;

	// Token: 0x04003758 RID: 14168
	[HideInInspector]
	public Transform PlayerCamera;

	// Token: 0x04003759 RID: 14169
	private Vector3 OldPlayerPos;

	// Token: 0x0400375A RID: 14170
	[HideInInspector]
	public int Mode = 1;

	// Token: 0x0400375B RID: 14171
	[HideInInspector]
	public int Master;

	// Token: 0x0400375C RID: 14172
	[HideInInspector]
	public bool enabledBillboard = true;

	// Token: 0x0400375D RID: 14173
	[HideInInspector]
	public Vector3[] BillboardPosition;

	// Token: 0x0400375E RID: 14174
	[HideInInspector]
	public float BillInterval = 0.05f;

	// Token: 0x0400375F RID: 14175
	[HideInInspector]
	public int[] BillStatus;

	// Token: 0x04003760 RID: 14176
	[HideInInspector]
	public float BillMaxViewDistance = 30f;

	// Token: 0x04003761 RID: 14177
	[HideInInspector]
	public T4MBillBObjSC[] BillScript;

	// Token: 0x04003762 RID: 14178
	[HideInInspector]
	public bool enabledLayerCul = true;

	// Token: 0x04003763 RID: 14179
	[HideInInspector]
	public float BackGroundView = 1000f;

	// Token: 0x04003764 RID: 14180
	[HideInInspector]
	public float FarView = 200f;

	// Token: 0x04003765 RID: 14181
	[HideInInspector]
	public float NormalView = 60f;

	// Token: 0x04003766 RID: 14182
	[HideInInspector]
	public float CloseView = 30f;

	// Token: 0x04003767 RID: 14183
	private float[] distances = new float[32];

	// Token: 0x04003768 RID: 14184
	[HideInInspector]
	public int Axis;

	// Token: 0x04003769 RID: 14185
	[HideInInspector]
	public bool LODbasedOnScript = true;

	// Token: 0x0400376A RID: 14186
	[HideInInspector]
	public bool BilBbasedOnScript = true;

	// Token: 0x0400376B RID: 14187
	public Material T4MMaterial;

	// Token: 0x0400376C RID: 14188
	public MeshFilter T4MMesh;

	// Token: 0x0400376D RID: 14189
	[HideInInspector]
	public Color TranslucencyColor = new Color(0.73f, 0.85f, 0.4f, 1f);

	// Token: 0x0400376E RID: 14190
	[HideInInspector]
	public Vector4 Wind = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);

	// Token: 0x0400376F RID: 14191
	[HideInInspector]
	public float WindFrequency = 0.75f;

	// Token: 0x04003770 RID: 14192
	[HideInInspector]
	public float GrassWindFrequency = 1.5f;

	// Token: 0x04003771 RID: 14193
	[HideInInspector]
	public bool ActiveWind;

	// Token: 0x04003772 RID: 14194
	public bool LayerCullPreview;

	// Token: 0x04003773 RID: 14195
	public bool LODPreview;

	// Token: 0x04003774 RID: 14196
	public bool BillboardPreview;

	// Token: 0x04003775 RID: 14197
	public Texture2D T4MMaskTex2d;

	// Token: 0x04003776 RID: 14198
	public Texture2D T4MMaskTexd;
}
