using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public class TerrainHeight : MonoBehaviour
{
	// Token: 0x0600321C RID: 12828 RVA: 0x00184F94 File Offset: 0x00183194
	private void Awake()
	{
		Transform[] componentsInChildren = this.TerrainParent.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Terrain component = componentsInChildren[i].GetComponent<Terrain>();
			if (component)
			{
				this.terrains.Add(componentsInChildren[i]);
			}
		}
		this.terrainRects = new Rect[this.terrains.Count];
		for (int j = 0; j < this.terrains.Count; j++)
		{
			Terrain component = this.terrains[j].GetComponent<Terrain>();
			this.terrainRects[j].x = this.terrains[j].position.x;
			this.terrainRects[j].y = this.terrains[j].position.z;
			this.terrainRects[j].width = component.terrainData.size.x;
			this.terrainRects[j].height = component.terrainData.size.z;
		}
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x001850C8 File Offset: 0x001832C8
	private Terrain GetTerrainTile(Vector3 position)
	{
		for (int i = 0; i < this.terrains.Count; i++)
		{
			if (this.terrainRects[i].Contains(new Vector2(position.x, position.z)))
			{
				return this.terrains[i].GetComponent<Terrain>();
			}
		}
		return null;
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x00185130 File Offset: 0x00183330
	private float GetTerrainHeight(Vector3 position)
	{
		Terrain terrainTile = this.GetTerrainTile(position);
		if (terrainTile)
		{
			return terrainTile.SampleHeight(position);
		}
		return -1f;
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x00185160 File Offset: 0x00183360
	private void Update()
	{
		float terrainHeight = this.GetTerrainHeight(this.cube.position);
		Debug.Log(string.Concat(new object[]
		{
			"Position: ",
			this.cube.position,
			" height: ",
			terrainHeight
		}));
	}

	// Token: 0x04003DE1 RID: 15841
	public GameObject TerrainParent;

	// Token: 0x04003DE2 RID: 15842
	private List<Transform> terrains = new List<Transform>();

	// Token: 0x04003DE3 RID: 15843
	private Rect[] terrainRects;

	// Token: 0x04003DE4 RID: 15844
	public Transform cube;
}
