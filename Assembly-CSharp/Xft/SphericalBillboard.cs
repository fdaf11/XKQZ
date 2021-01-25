using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200059E RID: 1438
	public class SphericalBillboard : RenderObject
	{
		// Token: 0x060023F8 RID: 9208 RVA: 0x00119590 File Offset: 0x00117790
		public SphericalBillboard(VertexPool.VertexSegment segment)
		{
			this.UVChanged = (this.ColorChanged = false);
			this.Vertexsegment = segment;
			this.ResetSegment();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00017EB1 File Offset: 0x000160B1
		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.UVChanged = true;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00017ED9 File Offset: 0x000160D9
		public void SetColor(Color c)
		{
			this.Color = c;
			this.ColorChanged = true;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00017EE9 File Offset: 0x000160E9
		public void SetPosition(Vector3 pos)
		{
			this.mCenterPos = pos;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x001195F8 File Offset: 0x001177F8
		public void ResetSegment()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Indices[indexStart] = vertStart;
			pool.Indices[indexStart + 1] = vertStart + 3;
			pool.Indices[indexStart + 2] = vertStart + 1;
			pool.Indices[indexStart + 3] = vertStart + 3;
			pool.Indices[indexStart + 4] = vertStart + 2;
			pool.Indices[indexStart + 5] = vertStart + 1;
			pool.Vertices[vertStart] = Vector3.zero;
			pool.Vertices[vertStart + 1] = Vector3.zero;
			pool.Vertices[vertStart + 2] = Vector3.zero;
			pool.Vertices[vertStart + 3] = Vector3.zero;
			pool.Colors[vertStart] = Color.white;
			pool.Colors[vertStart + 1] = Color.white;
			pool.Colors[vertStart + 2] = Color.white;
			pool.Colors[vertStart + 3] = Color.white;
			pool.UVs[vertStart] = Vector2.zero;
			pool.UVs[vertStart + 1] = Vector2.zero;
			pool.UVs[vertStart + 2] = Vector2.zero;
			pool.UVs[vertStart + 3] = Vector2.zero;
			pool.UVChanged = (pool.IndiceChanged = (pool.ColorChanged = (pool.VertChanged = true)));
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x001197B0 File Offset: 0x001179B0
		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			if (this.UVDimensions.y > 0f)
			{
				pool.UVs[vertStart] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				pool.UVs[vertStart + 1] = this.LowerLeftUV;
				pool.UVs[vertStart + 2] = this.LowerLeftUV + Vector2.right * this.UVDimensions.x;
				pool.UVs[vertStart + 3] = this.LowerLeftUV + this.UVDimensions;
			}
			else
			{
				pool.UVs[vertStart] = this.LowerLeftUV;
				pool.UVs[vertStart + 1] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				pool.UVs[vertStart + 2] = this.LowerLeftUV + this.UVDimensions;
				pool.UVs[vertStart + 3] = this.LowerLeftUV + Vector2.right * this.UVDimensions.x;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x00119944 File Offset: 0x00117B44
		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Colors[vertStart] = this.Color;
			pool.Colors[vertStart + 1] = this.Color;
			pool.Colors[vertStart + 2] = this.Color;
			pool.Colors[vertStart + 3] = this.Color;
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x001199DC File Offset: 0x00117BDC
		public void UpdateCenterPos()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Vertices[vertStart] = this.mCenterPos;
			pool.Vertices[vertStart + 1] = this.mCenterPos;
			pool.Vertices[vertStart + 2] = this.mCenterPos;
			pool.Vertices[vertStart + 3] = this.mCenterPos;
			this.Vertexsegment.Pool.VertChanged = true;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x00119A74 File Offset: 0x00117C74
		public override void Update(float deltaTime)
		{
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			this.SetPosition(this.Node.CurWorldPos);
			if (this.UVChanged)
			{
				this.UpdateUV();
			}
			if (this.ColorChanged)
			{
				this.UpdateColor();
			}
			this.UVChanged = (this.ColorChanged = false);
			this.UpdateCenterPos();
			this.mRadius = this.Node.Scale.x * this.Node.Owner.SpriteWidth * this.Node.OriScaleX;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector2 one = Vector2.one;
			one.x = this.mRadius;
			one.y = ((float)this.Node.OriRotateAngle + this.Node.RotateAngle) * 3.1415927f / 180f;
			pool.UVs2[vertStart] = one;
			pool.UVs2[vertStart + 1] = one;
			pool.UVs2[vertStart + 2] = one;
			pool.UVs2[vertStart + 3] = one;
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x00119C20 File Offset: 0x00117E20
		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(node.LowerLeftUV, node.UVDimensions);
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.MyCamera.depthTextureMode == null)
			{
				this.Node.Owner.MyCamera.depthTextureMode = 1;
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x00017EF2 File Offset: 0x000160F2
		public override void Reset()
		{
			this.SetPosition(this.Node.Position);
			this.SetColor(Color.clear);
			this.mRadius = 0f;
			this.Update(0f);
		}

		// Token: 0x04002BA0 RID: 11168
		protected Vector2 LowerLeftUV;

		// Token: 0x04002BA1 RID: 11169
		protected Vector2 UVDimensions;

		// Token: 0x04002BA2 RID: 11170
		protected Vector3 v1 = Vector3.zero;

		// Token: 0x04002BA3 RID: 11171
		protected Vector3 v2 = Vector3.zero;

		// Token: 0x04002BA4 RID: 11172
		protected Vector3 v3 = Vector3.zero;

		// Token: 0x04002BA5 RID: 11173
		protected Vector3 v4 = Vector3.zero;

		// Token: 0x04002BA6 RID: 11174
		protected VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002BA7 RID: 11175
		public Color Color;

		// Token: 0x04002BA8 RID: 11176
		protected bool UVChanged;

		// Token: 0x04002BA9 RID: 11177
		protected bool ColorChanged;

		// Token: 0x04002BAA RID: 11178
		protected Vector3 mCenterPos = Vector3.zero;

		// Token: 0x04002BAB RID: 11179
		protected float mRadius;
	}
}
