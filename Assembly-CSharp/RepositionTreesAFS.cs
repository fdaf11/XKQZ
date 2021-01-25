using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000642 RID: 1602
[RequireComponent(typeof(Terrain))]
[AddComponentMenu("AFS/Trees/AFS Reposition Trees")]
public class RepositionTreesAFS : MonoBehaviour
{
	// Token: 0x06002779 RID: 10105 RVA: 0x0013818C File Offset: 0x0013638C
	public void UpdatePrototypes()
	{
		this.myterrainComp = (base.gameObject.GetComponent(typeof(Terrain)) as Terrain);
		if (this.ExcludetreePrototype == null)
		{
			this.ExcludetreePrototype = new bool[this.myterrainComp.terrainData.treePrototypes.Length];
			this.TreePrototypeName = new string[this.myterrainComp.terrainData.treePrototypes.Length];
			for (int i = 0; i < this.myterrainComp.terrainData.treePrototypes.Length; i++)
			{
				this.TreePrototypeName[i] = this.myterrainComp.terrainData.treePrototypes[i].prefab.name;
			}
		}
		else if (this.ExcludetreePrototype.Length != this.myterrainComp.terrainData.treePrototypes.Length)
		{
			this.ExcludetreePrototype = new bool[this.myterrainComp.terrainData.treePrototypes.Length];
			this.TreePrototypeName = new string[this.myterrainComp.terrainData.treePrototypes.Length];
			for (int j = 0; j < this.myterrainComp.terrainData.treePrototypes.Length; j++)
			{
				this.TreePrototypeName[j] = this.myterrainComp.terrainData.treePrototypes[j].prefab.name;
			}
		}
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x001382EC File Offset: 0x001364EC
	public void RepositionTrees()
	{
		Terrain terrain = base.gameObject.GetComponent(typeof(Terrain)) as Terrain;
		TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
		Vector3 size = terrain.terrainData.size;
		Vector3 position = terrain.GetPosition();
		this.treesPos = new Vector3[treeInstances.Length];
		for (int i = 0; i < treeInstances.Length; i++)
		{
			this.treesPos[i] = new Vector3(treeInstances[i].position.x * size.x, treeInstances[i].position.y * size.y, treeInstances[i].position.z * size.z) + position + Vector3.up;
		}
		for (int j = 0; j < treeInstances.Length; j++)
		{
			int prototypeIndex = treeInstances[j].prototypeIndex;
			if (!this.ExcludetreePrototype[prototypeIndex])
			{
				Vector3 vector;
				vector..ctor(this.treesPos[j].x, 2000f, this.treesPos[j].z);
				float num = treeInstances[j].position.y;
				float x = treeInstances[j].position.x;
				float z = treeInstances[j].position.z;
				RaycastHit raycastHit;
				if (terrain.collider.Raycast(new Ray(vector, new Vector3(0f, -1f, 0f)), ref raycastHit, float.PositiveInfinity))
				{
					num = raycastHit.point.y / size.y;
				}
				for (int k = 0; k < this.additionalTerrainObjects.Count; k++)
				{
					if (this.additionalTerrainObjects[k].collider)
					{
						RaycastHit raycastHit2;
						if (this.additionalTerrainObjects[k].collider.Raycast(new Ray(vector, new Vector3(0f, -1f, 0f)), ref raycastHit2, float.PositiveInfinity))
						{
							float num2 = raycastHit2.point.y / size.y;
							if (num < num2)
							{
								num = num2;
							}
						}
						Vector3 position2;
						position2..ctor(x, num, z);
						treeInstances[j].position = position2;
					}
				}
			}
		}
		terrain.terrainData.treeInstances = treeInstances;
	}

	// Token: 0x04003106 RID: 12550
	public bool[] ExcludetreePrototype;

	// Token: 0x04003107 RID: 12551
	public string[] TreePrototypeName;

	// Token: 0x04003108 RID: 12552
	public List<GameObject> additionalTerrainObjects;

	// Token: 0x04003109 RID: 12553
	public Vector3[] treesPos;

	// Token: 0x0400310A RID: 12554
	public Terrain myterrainComp;
}
