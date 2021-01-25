using System;
using Assets.PigeonCoopUtil;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B5 RID: 1461
	public class PCTrail : IDisposable
	{
		// Token: 0x0600247B RID: 9339 RVA: 0x0011D9C0 File Offset: 0x0011BBC0
		public PCTrail(int numPoints)
		{
			this.Mesh = new Mesh();
			this.Mesh.MarkDynamic();
			this.verticies = new Vector3[2 * numPoints];
			this.normals = new Vector3[2 * numPoints];
			this.uvs = new Vector2[2 * numPoints];
			this.colors = new Color[2 * numPoints];
			this.indicies = new int[2 * numPoints * 3];
			this.Points = new CircularBuffer<PCTrailPoint>(numPoints);
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0011DA40 File Offset: 0x0011BC40
		public void Dispose()
		{
			if (this.Mesh != null)
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(this.Mesh, true);
				}
				else
				{
					Object.Destroy(this.Mesh);
				}
			}
			this.Points.Clear();
			this.Points = null;
		}

		// Token: 0x04002C3F RID: 11327
		public CircularBuffer<PCTrailPoint> Points;

		// Token: 0x04002C40 RID: 11328
		public Mesh Mesh;

		// Token: 0x04002C41 RID: 11329
		public Vector3[] verticies;

		// Token: 0x04002C42 RID: 11330
		public Vector3[] normals;

		// Token: 0x04002C43 RID: 11331
		public Vector2[] uvs;

		// Token: 0x04002C44 RID: 11332
		public Color[] colors;

		// Token: 0x04002C45 RID: 11333
		public int[] indicies;

		// Token: 0x04002C46 RID: 11334
		public int activePointCount;
	}
}
