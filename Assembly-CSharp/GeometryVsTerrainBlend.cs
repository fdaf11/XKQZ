using System;
using UnityEngine;

// Token: 0x02000539 RID: 1337
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("Relief Terrain/Geometry Blend")]
[SelectionBase]
public class GeometryVsTerrainBlend : MonoBehaviour
{
	// Token: 0x06002203 RID: 8707 RVA: 0x00016C00 File Offset: 0x00014E00
	private void Start()
	{
		this.SetupValues();
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x00102914 File Offset: 0x00100B14
	public void SetupValues()
	{
		if (this.blendedObject && (this.blendedObject.GetComponent(typeof(MeshRenderer)) != null || this.blendedObject.GetComponent(typeof(Terrain)) != null))
		{
			if (this.underlying_transform == null)
			{
				this.underlying_transform = base.transform.FindChild("RTP_blend_underlying");
			}
			if (this.underlying_transform != null)
			{
				GameObject gameObject = this.underlying_transform.gameObject;
				this.underlying_renderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			}
			if (this.underlying_renderer != null && this.underlying_renderer.sharedMaterial != null)
			{
				ReliefTerrain reliefTerrain = (ReliefTerrain)this.blendedObject.GetComponent(typeof(ReliefTerrain));
				if (reliefTerrain)
				{
					Material sharedMaterial = this.underlying_renderer.sharedMaterial;
					reliefTerrain.RefreshTextures(sharedMaterial, false);
					reliefTerrain.globalSettingsHolder.Refresh(sharedMaterial, null);
					if (sharedMaterial.HasProperty("RTP_DeferredAddPassSpec"))
					{
						sharedMaterial.SetFloat("RTP_DeferredAddPassSpec", this._DeferredBlendGloss);
					}
					if (reliefTerrain.controlA)
					{
						sharedMaterial.SetTexture("_Control", reliefTerrain.controlA);
					}
					if (reliefTerrain.ColorGlobal)
					{
						sharedMaterial.SetTexture("_Splat0", reliefTerrain.ColorGlobal);
					}
					if (reliefTerrain.NormalGlobal)
					{
						sharedMaterial.SetTexture("_Splat1", reliefTerrain.NormalGlobal);
					}
					if (reliefTerrain.TreesGlobal)
					{
						sharedMaterial.SetTexture("_Splat2", reliefTerrain.TreesGlobal);
					}
					if (reliefTerrain.BumpGlobalCombined)
					{
						sharedMaterial.SetTexture("_Splat3", reliefTerrain.BumpGlobalCombined);
					}
				}
				Terrain terrain = (Terrain)this.blendedObject.GetComponent(typeof(Terrain));
				if (terrain)
				{
					this.underlying_renderer.lightmapIndex = terrain.lightmapIndex;
				}
				else
				{
					this.underlying_renderer.lightmapIndex = this.blendedObject.GetComponent<Renderer>().lightmapIndex;
				}
				this.underlying_renderer.lightmapTilingOffset = new Vector4(1f, 1f, 0f, 0f);
				if (this.Sticked)
				{
					if (terrain)
					{
						base.GetComponent<Renderer>().lightmapIndex = terrain.lightmapIndex;
					}
					else
					{
						base.GetComponent<Renderer>().lightmapIndex = this.blendedObject.GetComponent<Renderer>().lightmapIndex;
					}
					base.GetComponent<Renderer>().lightmapTilingOffset = new Vector4(1f, 1f, 0f, 0f);
				}
			}
		}
	}

	// Token: 0x04002604 RID: 9732
	private const int progress_granulation = 1000;

	// Token: 0x04002605 RID: 9733
	public double UpdTim;

	// Token: 0x04002606 RID: 9734
	private int progress_count_max;

	// Token: 0x04002607 RID: 9735
	private int progress_count_current;

	// Token: 0x04002608 RID: 9736
	private string progress_description = string.Empty;

	// Token: 0x04002609 RID: 9737
	public float blend_distance = 0.1f;

	// Token: 0x0400260A RID: 9738
	public GameObject blendedObject;

	// Token: 0x0400260B RID: 9739
	public bool VoxelBlendedObject;

	// Token: 0x0400260C RID: 9740
	public float _DeferredBlendGloss = 0.8f;

	// Token: 0x0400260D RID: 9741
	[HideInInspector]
	public bool undo_flag;

	// Token: 0x0400260E RID: 9742
	[HideInInspector]
	public bool paint_flag;

	// Token: 0x0400260F RID: 9743
	[HideInInspector]
	public int paint_mode;

	// Token: 0x04002610 RID: 9744
	[HideInInspector]
	public float paint_size = 0.5f;

	// Token: 0x04002611 RID: 9745
	[HideInInspector]
	public float paint_smoothness;

	// Token: 0x04002612 RID: 9746
	[HideInInspector]
	public float paint_opacity = 1f;

	// Token: 0x04002613 RID: 9747
	[HideInInspector]
	public RTPColorChannels vertex_paint_channel = RTPColorChannels.A;

	// Token: 0x04002614 RID: 9748
	[HideInInspector]
	public int addTrisSubdivision;

	// Token: 0x04002615 RID: 9749
	[HideInInspector]
	public float addTrisMinAngle;

	// Token: 0x04002616 RID: 9750
	[HideInInspector]
	public float addTrisMaxAngle = 90f;

	// Token: 0x04002617 RID: 9751
	private Vector3[] paint_vertices;

	// Token: 0x04002618 RID: 9752
	private Vector3[] paint_normals;

	// Token: 0x04002619 RID: 9753
	private int[] paint_tris;

	// Token: 0x0400261A RID: 9754
	private Transform underlying_transform;

	// Token: 0x0400261B RID: 9755
	private MeshRenderer underlying_renderer;

	// Token: 0x0400261C RID: 9756
	[HideInInspector]
	public RaycastHit paintHitInfo;

	// Token: 0x0400261D RID: 9757
	[HideInInspector]
	public bool paintHitInfo_flag;

	// Token: 0x0400261E RID: 9758
	[HideInInspector]
	private Texture2D tmp_globalColorMap;

	// Token: 0x0400261F RID: 9759
	[HideInInspector]
	public Vector3[] normals_orig;

	// Token: 0x04002620 RID: 9760
	[HideInInspector]
	public Vector4[] tangents_orig;

	// Token: 0x04002621 RID: 9761
	[HideInInspector]
	public bool baked_normals;

	// Token: 0x04002622 RID: 9762
	[HideInInspector]
	public Mesh orig_mesh;

	// Token: 0x04002623 RID: 9763
	[HideInInspector]
	public Mesh pmesh;

	// Token: 0x04002624 RID: 9764
	[HideInInspector]
	public bool shader_global_blend_capabilities;

	// Token: 0x04002625 RID: 9765
	[HideInInspector]
	public float StickOffset = 0.03f;

	// Token: 0x04002626 RID: 9766
	[HideInInspector]
	public bool Sticked;

	// Token: 0x04002627 RID: 9767
	[HideInInspector]
	public bool StickedOptimized = true;

	// Token: 0x04002628 RID: 9768
	[HideInInspector]
	public bool ModifyTris;

	// Token: 0x04002629 RID: 9769
	[HideInInspector]
	public bool BuildMeshFlag;

	// Token: 0x0400262A RID: 9770
	[HideInInspector]
	public bool RealizePaint_Flag;

	// Token: 0x0400262B RID: 9771
	[HideInInspector]
	public string save_path = string.Empty;

	// Token: 0x0400262C RID: 9772
	[HideInInspector]
	public bool isBatched;
}
