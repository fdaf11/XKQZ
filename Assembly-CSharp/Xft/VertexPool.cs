using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A3 RID: 1443
	public class VertexPool
	{
		// Token: 0x0600240E RID: 9230 RVA: 0x0011A25C File Offset: 0x0011845C
		public VertexPool(Mesh mesh, Material material)
		{
			this.VertexTotal = (this.VertexUsed = 0);
			this.VertCountChanged = false;
			this.Mesh = mesh;
			this.Material = material;
			this.InitArrays();
			this.IndiceChanged = (this.ColorChanged = (this.UVChanged = (this.UV2Changed = (this.VertChanged = true))));
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x00017F6E File Offset: 0x0001616E
		public void RecalculateBounds()
		{
			this.Mesh.RecalculateBounds();
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x0011A2D8 File Offset: 0x001184D8
		public CustomMesh AddCustomMesh(Mesh mesh, Vector3 dir, float maxFps)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(mesh.vertices.Length, mesh.triangles.Length);
			return new CustomMesh(vertices, mesh, dir, maxFps);
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0011A308 File Offset: 0x00118508
		public Cone AddCone(Vector2 size, int numSegment, float angle, Vector3 dir, int uvStretch, float maxFps, bool usedelta, AnimationCurve deltaAngle)
		{
			VertexPool.VertexSegment vertices = this.GetVertices((numSegment + 1) * 2, numSegment * 6);
			return new Cone(vertices, size, numSegment, angle, dir, uvStretch, maxFps)
			{
				UseDeltaAngle = usedelta,
				CurveAngle = deltaAngle
			};
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0011A348 File Offset: 0x00118548
		public XftSprite AddSprite(float width, float height, STYPE type, ORIPOINT ori, float maxFps, bool simple)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(4, 6);
			return new XftSprite(vertices, width, height, type, ori, maxFps, simple);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0011A370 File Offset: 0x00118570
		public RibbonTrail AddRibbonTrail(bool useFaceObj, Transform faceobj, float width, int maxelemnt, float len, Vector3 pos, float maxFps)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(maxelemnt * 2, (maxelemnt - 1) * 6);
			return new RibbonTrail(vertices, useFaceObj, faceobj, width, maxelemnt, len, pos, maxFps);
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x0011A3A4 File Offset: 0x001185A4
		public VertexPool.VertexSegment GetRopeVertexSeg(int maxcount)
		{
			return this.GetVertices(maxcount * 2, (maxcount - 1) * 6);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0011A3C4 File Offset: 0x001185C4
		public Rope AddRope()
		{
			return new Rope();
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0011A3D8 File Offset: 0x001185D8
		public SphericalBillboard AddSphericalBillboard()
		{
			VertexPool.VertexSegment vertices = this.GetVertices(4, 6);
			return new SphericalBillboard(vertices);
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x00017F7B File Offset: 0x0001617B
		public Material GetMaterial()
		{
			return this.Material;
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0011A3F8 File Offset: 0x001185F8
		public VertexPool.VertexSegment GetVertices(int vcount, int icount)
		{
			int num = 0;
			int num2 = 0;
			if (this.VertexUsed + vcount >= this.VertexTotal)
			{
				num = (vcount / 108 + 1) * 108;
			}
			if (this.IndexUsed + icount >= this.IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
			}
			this.VertexUsed += vcount;
			this.IndexUsed += icount;
			if (num != 0 || num2 != 0)
			{
				this.EnlargeArrays(num, num2);
				this.VertexTotal += num;
				this.IndexTotal += num2;
			}
			return new VertexPool.VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0011A4A8 File Offset: 0x001186A8
		private void InitDefaultShaderParam(Vector2[] uv2)
		{
			for (int i = 0; i < uv2.Length; i++)
			{
				uv2[i].x = 1f;
				uv2[i].y = 0f;
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0011A4EC File Offset: 0x001186EC
		protected void InitArrays()
		{
			this.Vertices = new Vector3[4];
			this.UVs = new Vector2[4];
			this.UVs2 = new Vector2[4];
			this.Colors = new Color[4];
			this.Indices = new int[6];
			this.VertexTotal = 4;
			this.IndexTotal = 6;
			this.InitDefaultShaderParam(this.UVs2);
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x0011A550 File Offset: 0x00118750
		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = this.Vertices;
			this.Vertices = new Vector3[this.Vertices.Length + count];
			vertices.CopyTo(this.Vertices, 0);
			Vector2[] uvs = this.UVs;
			this.UVs = new Vector2[this.UVs.Length + count];
			uvs.CopyTo(this.UVs, 0);
			Vector2[] uvs2 = this.UVs2;
			this.UVs2 = new Vector2[this.UVs2.Length + count];
			uvs2.CopyTo(this.UVs2, 0);
			this.InitDefaultShaderParam(this.UVs2);
			Color[] colors = this.Colors;
			this.Colors = new Color[this.Colors.Length + count];
			colors.CopyTo(this.Colors, 0);
			int[] indices = this.Indices;
			this.Indices = new int[this.Indices.Length + icount];
			indices.CopyTo(this.Indices, 0);
			this.VertCountChanged = true;
			this.IndiceChanged = true;
			this.ColorChanged = true;
			this.UVChanged = true;
			this.VertChanged = true;
			this.UV2Changed = true;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0011A664 File Offset: 0x00118864
		public void LateUpdate()
		{
			if (this.VertCountChanged)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.UVChanged)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.UV2Changed)
			{
				this.Mesh.uv2 = this.UVs2;
			}
			if (this.ColorChanged)
			{
				this.Mesh.colors = this.Colors;
			}
			if (this.IndiceChanged)
			{
				this.Mesh.triangles = this.Indices;
			}
			this.ElapsedTime += Time.deltaTime;
			if (this.ElapsedTime > this.BoundsScheduleTime || this.FirstUpdate)
			{
				this.RecalculateBounds();
				this.ElapsedTime = 0f;
			}
			if (this.ElapsedTime > this.BoundsScheduleTime)
			{
				this.FirstUpdate = false;
			}
			this.VertCountChanged = false;
			this.IndiceChanged = false;
			this.ColorChanged = false;
			this.UVChanged = false;
			this.UV2Changed = false;
			this.VertChanged = false;
		}

		// Token: 0x04002BC8 RID: 11208
		public const int BlockSize = 108;

		// Token: 0x04002BC9 RID: 11209
		public Vector3[] Vertices;

		// Token: 0x04002BCA RID: 11210
		public int[] Indices;

		// Token: 0x04002BCB RID: 11211
		public Vector2[] UVs;

		// Token: 0x04002BCC RID: 11212
		public Color[] Colors;

		// Token: 0x04002BCD RID: 11213
		public Vector2[] UVs2;

		// Token: 0x04002BCE RID: 11214
		public bool IndiceChanged;

		// Token: 0x04002BCF RID: 11215
		public bool ColorChanged;

		// Token: 0x04002BD0 RID: 11216
		public bool UVChanged;

		// Token: 0x04002BD1 RID: 11217
		public bool VertChanged;

		// Token: 0x04002BD2 RID: 11218
		public bool UV2Changed;

		// Token: 0x04002BD3 RID: 11219
		public Mesh Mesh;

		// Token: 0x04002BD4 RID: 11220
		public Material Material;

		// Token: 0x04002BD5 RID: 11221
		protected int VertexTotal;

		// Token: 0x04002BD6 RID: 11222
		protected int VertexUsed;

		// Token: 0x04002BD7 RID: 11223
		protected int IndexTotal;

		// Token: 0x04002BD8 RID: 11224
		protected int IndexUsed;

		// Token: 0x04002BD9 RID: 11225
		public bool FirstUpdate = true;

		// Token: 0x04002BDA RID: 11226
		protected bool VertCountChanged;

		// Token: 0x04002BDB RID: 11227
		public float BoundsScheduleTime = 1f;

		// Token: 0x04002BDC RID: 11228
		public float ElapsedTime;

		// Token: 0x020005A4 RID: 1444
		public class VertexSegment
		{
			// Token: 0x0600241D RID: 9245 RVA: 0x00017F83 File Offset: 0x00016183
			public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
			{
				this.VertStart = start;
				this.VertCount = count;
				this.IndexCount = icount;
				this.IndexStart = istart;
				this.Pool = pool;
			}

			// Token: 0x0600241E RID: 9246 RVA: 0x0011A78C File Offset: 0x0011898C
			public void ClearIndices()
			{
				for (int i = this.IndexStart; i < this.IndexStart + this.IndexCount; i++)
				{
					this.Pool.Indices[i] = 0;
				}
				this.Pool.IndiceChanged = true;
			}

			// Token: 0x04002BDD RID: 11229
			public int VertStart;

			// Token: 0x04002BDE RID: 11230
			public int IndexStart;

			// Token: 0x04002BDF RID: 11231
			public int VertCount;

			// Token: 0x04002BE0 RID: 11232
			public int IndexCount;

			// Token: 0x04002BE1 RID: 11233
			public VertexPool Pool;
		}
	}
}
