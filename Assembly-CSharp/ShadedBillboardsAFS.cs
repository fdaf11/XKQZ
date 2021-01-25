using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000644 RID: 1604
[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
[AddComponentMenu("AFS/Trees/AFS Shaded Billboards")]
public class ShadedBillboardsAFS : MonoBehaviour
{
	// Token: 0x0600278D RID: 10125 RVA: 0x0001A114 File Offset: 0x00018314
	private void Awake()
	{
		this.terrainComp = (base.gameObject.GetComponent(typeof(Terrain)) as Terrain);
		this.treeInstances = this.terrainComp.terrainData.treeInstances;
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x0001A14C File Offset: 0x0001834C
	private void Update()
	{
		if (Application.isPlaying)
		{
			this.ShadeTrees();
		}
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x00139584 File Offset: 0x00137784
	public void UpdateTrees()
	{
		if (this.treeInstances == null)
		{
			this.treeInstances = this.terrainComp.terrainData.treeInstances;
		}
		Vector3 size = this.terrainComp.terrainData.size;
		Vector3 position = this.terrainComp.GetPosition();
		this.treesPos = new Vector3[this.treeInstances.Length];
		this.treesShadowed = new bool[this.treeInstances.Length];
		this.treesStates = new float[this.treeInstances.Length];
		this.curIdx = 0;
		for (int i = 0; i < this.treeInstances.Length; i++)
		{
			this.treesPos[i] = new Vector3(this.treeInstances[i].position.x * size.x, this.treeInstances[i].position.y * size.y, this.treeInstances[i].position.z * size.z) + position + Vector3.up;
			this.treesShadowed[i] = false;
			this.treesStates[i] = 0f;
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x001396CC File Offset: 0x001378CC
	public void RemoveLightMap()
	{
		if (this.treeInstances == null)
		{
			this.treeInstances = this.terrainComp.terrainData.treeInstances;
		}
		for (int i = 0; i < this.treeInstances.Length; i++)
		{
			this.treeInstances[i].lightmapColor = new Color(1f, 1f, 1f, 1f);
		}
		this.terrainComp.terrainData.treeInstances = this.treeInstances;
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x00139754 File Offset: 0x00137954
	public void RemoveShadowing()
	{
		if (this.treeInstances == null)
		{
			this.treeInstances = this.terrainComp.terrainData.treeInstances;
		}
		for (int i = 0; i < this.treeInstances.Length; i++)
		{
			this.treeInstances[i].color = new Color(this.treeInstances[i].color.r, this.treeInstances[i].color.g, this.treeInstances[i].color.b, 1f);
		}
		this.terrainComp.terrainData.treeInstances = this.treeInstances;
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x00139818 File Offset: 0x00137A18
	public void ShadeTrees()
	{
		if (this.lightRef == null)
		{
			return;
		}
		if (this.shadowCasters == null)
		{
			return;
		}
		if (this.treesPos == null || this.treesPos.Length == 0)
		{
			this.UpdateTrees();
		}
		float deltaTime = Time.deltaTime;
		if (this.treeInstances == null)
		{
			this.treeInstances = this.terrainComp.terrainData.treeInstances;
		}
		int num = this.treeInstances.Length;
		if (this.lightRef.light.shadows != null)
		{
			for (int i = 0; i < this.treesPerFrame; i++)
			{
				int num2 = (this.curIdx + i) % num;
				Vector3 vector = this.treesPos[num2];
				this.treesShadowed[num2] = false;
				for (int j = 0; j < this.shadowCasters.Count; j++)
				{
					if (this.shadowCasters[j].collider)
					{
						RaycastHit raycastHit;
						if (this.shadowCasters[j].collider.Raycast(new Ray(vector + new Vector3(0f, this.treeYOffset, 0f), this.lightRef.transform.rotation * new Vector3(0f, 0f, -1f)), ref raycastHit, float.PositiveInfinity))
						{
							this.treesShadowed[num2] = true;
							break;
						}
					}
					else
					{
						for (int k = 0; k < this.shadowCasters[j].transform.childCount; k++)
						{
							Collider collider = this.shadowCasters[j].transform.GetChild(k).collider;
							RaycastHit raycastHit;
							if (collider && this.shadowCasters[j].collider.Raycast(new Ray(vector + new Vector3(0f, this.treeYOffset, 0f), this.lightRef.transform.rotation * new Vector3(0f, 0f, -1f)), ref raycastHit, float.PositiveInfinity))
							{
								this.treesShadowed[num2] = true;
								break;
							}
						}
						if (this.treesShadowed[num2])
						{
							break;
						}
					}
				}
			}
			this.curIdx += this.treesPerFrame;
			if (this.curIdx >= num)
			{
				this.curIdx -= num;
			}
			for (int l = 0; l < num; l++)
			{
				Vector3 vector2 = this.treesPos[l];
				float num3 = Vector3.Distance(Camera.main.transform.position, vector2);
				bool flag = num3 > QualitySettings.shadowDistance;
				if (this.shadeOnlyWithinShadowRange && flag)
				{
					this.treeInstances[l].color = new Color(this.treeInstances[l].color.r, this.treeInstances[l].color.g, this.treeInstances[l].color.b, 1f);
				}
				else
				{
					if (this.treesShadowed[l] && this.treesStates[l] < 1f)
					{
						this.treesStates[l] += this.changeStateSpeed * deltaTime;
					}
					else if (!this.treesShadowed[l] && this.treesStates[l] > 0f)
					{
						this.treesStates[l] -= this.changeStateSpeed * deltaTime;
					}
					this.treesStates[l] = Mathf.Clamp01(this.treesStates[l]);
					this.treeInstances[l].color = Color.Lerp(new Color(this.treeInstances[l].color.r, this.treeInstances[l].color.g, this.treeInstances[l].color.b, 1f), new Color(this.treeInstances[l].color.r, this.treeInstances[l].color.g, this.treeInstances[l].color.b, 0f), this.treesStates[l]);
				}
			}
		}
		else
		{
			for (int m = 0; m < num; m++)
			{
				this.treeInstances[m].color = new Color(this.treeInstances[m].color.r, this.treeInstances[m].color.g, this.treeInstances[m].color.b, 1f);
			}
		}
		this.terrainComp.terrainData.treeInstances = this.treeInstances;
	}

	// Token: 0x04003154 RID: 12628
	public bool showRays;

	// Token: 0x04003155 RID: 12629
	public bool shadeOnlyWithinShadowRange;

	// Token: 0x04003156 RID: 12630
	public GameObject lightRef;

	// Token: 0x04003157 RID: 12631
	public List<GameObject> shadowCasters;

	// Token: 0x04003158 RID: 12632
	public int treesPerFrame = 10;

	// Token: 0x04003159 RID: 12633
	public float changeStateSpeed = 4f;

	// Token: 0x0400315A RID: 12634
	public float treeYOffset;

	// Token: 0x0400315B RID: 12635
	public Vector3[] treesPos;

	// Token: 0x0400315C RID: 12636
	public bool[] treesShadowed;

	// Token: 0x0400315D RID: 12637
	public float[] treesStates;

	// Token: 0x0400315E RID: 12638
	public int curIdx;

	// Token: 0x0400315F RID: 12639
	private Terrain terrainComp;

	// Token: 0x04003160 RID: 12640
	private TreeInstance[] treeInstances;
}
