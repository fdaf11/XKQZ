using System;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public class MultiTerrainBoost : MonoBehaviour
{
	// Token: 0x06003221 RID: 12833 RVA: 0x001851BC File Offset: 0x001833BC
	private void Start()
	{
		this.MainCamera = base.GetComponent<Camera>();
		this.terrains = (Resources.FindObjectsOfTypeAll(typeof(Terrain)) as Terrain[]);
		this.bounds = new Bounds[this.terrains.Length];
		this.active1 = new bool[this.terrains.Length];
		this.calcBounds();
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x0018521C File Offset: 0x0018341C
	private void LateUpdate()
	{
		this.calcFrustrum();
		this.count_terrain = 0;
		while (this.count_terrain < this.bounds.Length)
		{
			if (this.IsRenderedFrom(this.bounds[this.count_terrain]))
			{
				if (!this.active1[this.count_terrain])
				{
					this.terrains[this.count_terrain].enabled = true;
					this.active1[this.count_terrain] = true;
				}
			}
			else if (this.active1[this.count_terrain])
			{
				this.terrains[this.count_terrain].enabled = false;
				this.active1[this.count_terrain] = false;
			}
			this.count_terrain++;
		}
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x001852E8 File Offset: 0x001834E8
	private void calcBounds()
	{
		this.count_terrain = 0;
		while (this.count_terrain < this.terrains.Length)
		{
			this.bounds[this.count_terrain].size = this.terrains[this.count_terrain].terrainData.size;
			this.bounds[this.count_terrain].center = new Vector3(this.terrains[this.count_terrain].transform.position.x + this.bounds[this.count_terrain].size.x / 2f, this.terrains[this.count_terrain].transform.position.y + this.bounds[this.count_terrain].size.y / 2f, this.terrains[this.count_terrain].transform.position.z + this.bounds[this.count_terrain].size.z / 2f);
			this.active1[this.count_terrain] = true;
			this.count_terrain++;
		}
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x0001F85B File Offset: 0x0001DA5B
	private void calcFrustrum()
	{
		this.planes = GeometryUtility.CalculateFrustumPlanes(this.MainCamera);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x0001F86E File Offset: 0x0001DA6E
	private bool IsRenderedFrom(Bounds bound)
	{
		return GeometryUtility.TestPlanesAABB(this.planes, bound);
	}

	// Token: 0x04003DE5 RID: 15845
	private Camera MainCamera;

	// Token: 0x04003DE6 RID: 15846
	private bool[] active1;

	// Token: 0x04003DE7 RID: 15847
	private Terrain[] terrains;

	// Token: 0x04003DE8 RID: 15848
	private Bounds[] bounds;

	// Token: 0x04003DE9 RID: 15849
	private Plane[] planes;

	// Token: 0x04003DEA RID: 15850
	private float distance;

	// Token: 0x04003DEB RID: 15851
	private int count_terrain;
}
