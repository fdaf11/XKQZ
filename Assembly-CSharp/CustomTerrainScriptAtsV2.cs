using System;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class CustomTerrainScriptAtsV2 : MonoBehaviour
{
	// Token: 0x060027D4 RID: 10196 RVA: 0x0013B738 File Offset: 0x00139938
	private void Start()
	{
		Terrain terrain = (Terrain)base.GetComponent(typeof(Terrain));
		if (this.Bump0)
		{
			Shader.SetGlobalTexture("_BumpMap0", this.Bump0);
		}
		if (this.Bump1)
		{
			Shader.SetGlobalTexture("_BumpMap1", this.Bump1);
		}
		if (this.Bump2)
		{
			Shader.SetGlobalTexture("_BumpMap2", this.Bump2);
		}
		if (this.Bump3)
		{
			Shader.SetGlobalTexture("_BumpMap3", this.Bump3);
		}
		Shader.SetGlobalFloat("_Tile0", this.Tile0);
		Shader.SetGlobalFloat("_Tile1", this.Tile1);
		Shader.SetGlobalFloat("_Tile2", this.Tile2);
		Shader.SetGlobalFloat("_Tile3", this.Tile3);
		this.terrainSizeX = terrain.terrainData.size.x;
		this.terrainSizeZ = terrain.terrainData.size.z;
		Shader.SetGlobalFloat("_TerrainX", this.terrainSizeX);
		Shader.SetGlobalFloat("_TerrainZ", this.terrainSizeZ);
	}

	// Token: 0x040031CC RID: 12748
	public Texture2D Bump0;

	// Token: 0x040031CD RID: 12749
	public Texture2D Bump1;

	// Token: 0x040031CE RID: 12750
	public Texture2D Bump2;

	// Token: 0x040031CF RID: 12751
	public Texture2D Bump3;

	// Token: 0x040031D0 RID: 12752
	public float Tile0;

	// Token: 0x040031D1 RID: 12753
	public float Tile1;

	// Token: 0x040031D2 RID: 12754
	public float Tile2;

	// Token: 0x040031D3 RID: 12755
	public float Tile3;

	// Token: 0x040031D4 RID: 12756
	public float terrainSizeX;

	// Token: 0x040031D5 RID: 12757
	public float terrainSizeZ;
}
