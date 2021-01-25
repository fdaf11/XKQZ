using System;
using UnityEngine;

// Token: 0x02000544 RID: 1348
[ExecuteInEditMode]
[AddComponentMenu("Relief Terrain/Helpers/MIP solver for standalone shader")]
public class ReliefTerrainVertexBlendTriplanar : MonoBehaviour
{
	// Token: 0x06002234 RID: 8756 RVA: 0x0010BE80 File Offset: 0x0010A080
	public void SetupMIPOffsets()
	{
		if (base.GetComponent<Renderer>() == null && this.material == null)
		{
			return;
		}
		Material sharedMaterial;
		if (base.GetComponent<Renderer>() != null)
		{
			sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
		else
		{
			sharedMaterial = this.material;
		}
		if (sharedMaterial == null)
		{
			return;
		}
		if (sharedMaterial.HasProperty("_SplatAtlasA"))
		{
			this.SetupTex("_SplatAtlasA", "rtp_mipoffset_color", 1f, -1f);
		}
		if (sharedMaterial.HasProperty("_SplatA0"))
		{
			this.SetupTex("_SplatA0", "rtp_mipoffset_color", 1f, 0f);
		}
		this.SetupTex("_BumpMap01", "rtp_mipoffset_bump", 1f, 0f);
		this.SetupTex("_TERRAIN_HeightMap", "rtp_mipoffset_height", 1f, 0f);
		if (sharedMaterial.HasProperty("_BumpMapGlobal"))
		{
			this.SetupTex("_BumpMapGlobal", "rtp_mipoffset_globalnorm", sharedMaterial.GetFloat("_BumpMapGlobalScale"), sharedMaterial.GetFloat("rtp_mipoffset_globalnorm_offset"));
			if (sharedMaterial.HasProperty("_SuperDetailTiling"))
			{
				this.SetupTex("_BumpMapGlobal", "rtp_mipoffset_superdetail", sharedMaterial.GetFloat("_SuperDetailTiling"), 0f);
			}
			if (sharedMaterial.HasProperty("TERRAIN_FlowScale"))
			{
				this.SetupTex("_BumpMapGlobal", "rtp_mipoffset_flow", sharedMaterial.GetFloat("TERRAIN_FlowScale"), sharedMaterial.GetFloat("TERRAIN_FlowMipOffset"));
			}
		}
		if (sharedMaterial.HasProperty("TERRAIN_RippleMap"))
		{
			this.SetupTex("TERRAIN_RippleMap", "rtp_mipoffset_ripple", sharedMaterial.GetFloat("TERRAIN_RippleScale"), 0f);
		}
		if (sharedMaterial.HasProperty("TERRAIN_CausticsTex"))
		{
			this.SetupTex("TERRAIN_CausticsTex", "rtp_mipoffset_caustics", sharedMaterial.GetFloat("TERRAIN_CausticsTilingScale"), 0f);
		}
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x0010C06C File Offset: 0x0010A26C
	private void SetupTex(string tex_name, string param_name, float _mult = 1f, float _add = 0f)
	{
		if (base.GetComponent<Renderer>() == null && this.material == null)
		{
			return;
		}
		Material sharedMaterial;
		if (base.GetComponent<Renderer>() != null)
		{
			sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
		else
		{
			sharedMaterial = this.material;
		}
		if (sharedMaterial == null)
		{
			return;
		}
		if (sharedMaterial.GetTexture(tex_name) != null)
		{
			int width = sharedMaterial.GetTexture(tex_name).width;
			sharedMaterial.SetFloat(param_name, -Mathf.Log(1024f / ((float)width * _mult)) / Mathf.Log(2f) + _add);
		}
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x0010C118 File Offset: 0x0010A318
	public void SetTopPlanarUVBounds()
	{
		MeshFilter meshFilter = base.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (meshFilter == null)
		{
			return;
		}
		Mesh sharedMesh = meshFilter.sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		if (vertices.Length == 0)
		{
			return;
		}
		Vector3 vector = base.transform.TransformPoint(vertices[0]);
		Vector4 vector2;
		vector2.x = vector.x;
		vector2.y = vector.z;
		vector2.z = vector.x;
		vector2.w = vector.z;
		for (int i = 1; i < vertices.Length; i++)
		{
			vector = base.transform.TransformPoint(vertices[i]);
			if (vector.x < vector2.x)
			{
				vector2.x = vector.x;
			}
			if (vector.z < vector2.y)
			{
				vector2.y = vector.z;
			}
			if (vector.x > vector2.z)
			{
				vector2.z = vector.x;
			}
			if (vector.z > vector2.w)
			{
				vector2.w = vector.z;
			}
		}
		vector2.z -= vector2.x;
		vector2.w -= vector2.y;
		if (base.GetComponent<Renderer>() == null && this.material == null)
		{
			return;
		}
		Material sharedMaterial;
		if (base.GetComponent<Renderer>() != null)
		{
			sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
		else
		{
			sharedMaterial = this.material;
		}
		if (sharedMaterial == null)
		{
			return;
		}
		sharedMaterial.SetVector("_TERRAIN_PosSize", vector2);
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x00016D61 File Offset: 0x00014F61
	private void Awake()
	{
		this.SetupMIPOffsets();
		if (Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x00016D7A File Offset: 0x00014F7A
	private void Update()
	{
		if (!Application.isPlaying)
		{
			this.SetupMIPOffsets();
		}
	}

	// Token: 0x04002859 RID: 10329
	public Texture2D tmp_globalColorMap;

	// Token: 0x0400285A RID: 10330
	public string save_path_colormap = string.Empty;

	// Token: 0x0400285B RID: 10331
	public GeometryVsTerrainBlend painterInstance;

	// Token: 0x0400285C RID: 10332
	public Material material;
}
