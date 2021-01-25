using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000578 RID: 1400
	public class Cone : RenderObject
	{
		// Token: 0x060022EF RID: 8943 RVA: 0x001121E0 File Offset: 0x001103E0
		public Cone(VertexPool.VertexSegment segment, Vector2 size, int numseg, float angle, Vector3 dir, int uvStretch, float maxFps)
		{
			this.Vertexsegment = segment;
			this.Size = size;
			this.Direction = dir;
			this.SetColor(Color.white);
			this.NumSegment = numseg;
			this.SpreadAngle = angle;
			this.OriSpreadAngle = angle;
			this.InitVerts();
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x00112274 File Offset: 0x00110474
		public override void ApplyShaderParam(float x, float y)
		{
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumVerts; i++)
			{
				pool.UVs2[vertStart + i] = one;
			}
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x001122EC File Offset: 0x001104EC
		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			this.SetColor(this.Node.Color);
			this.SetRotation((float)this.Node.OriRotateAngle);
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x00112340 File Offset: 0x00110540
		public override void Reset()
		{
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetColor(Color.clear);
			this.SetPosition(this.Node.Position);
			this.ResetAngle();
			this.MyUpdate(true, 0f);
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x00112390 File Offset: 0x00110590
		public override void Update(float deltaTime)
		{
			this.SetScale(this.Node.Scale.x * this.Node.OriScaleX, this.Node.Scale.y * this.Node.OriScaleY);
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			this.SetRotation((float)this.Node.OriRotateAngle + this.Node.RotateAngle);
			this.SetPosition(this.Node.CurWorldPos);
			this.MyUpdate(false, deltaTime);
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00112480 File Offset: 0x00110680
		public void SetUVCoord(Vector2 topleft, Vector2 dimensions)
		{
			this.LowerLeftUV = topleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.LowerLeftUV.y = this.LowerLeftUV.y - dimensions.y;
			this.UVDimensions.y = -this.UVDimensions.y;
			this.UVChanged = true;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000172D1 File Offset: 0x000154D1
		public void SetColor(Color c)
		{
			this.Color = c;
			this.ColorChanged = true;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000172E1 File Offset: 0x000154E1
		public void SetPosition(Vector3 pos)
		{
			this.Position = pos;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000172EA File Offset: 0x000154EA
		public void SetScale(float width, float height)
		{
			this.Scale.x = width;
			this.Scale.y = height;
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x00017304 File Offset: 0x00015504
		public void SetRotation(float angle)
		{
			this.OriRotAngle = angle;
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0001730D File Offset: 0x0001550D
		public void ResetAngle()
		{
			this.SpreadAngle = this.OriSpreadAngle;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x001124E4 File Offset: 0x001106E4
		protected void UpdateRotAngle(float deltaTime)
		{
			if (!this.UseDeltaAngle)
			{
				return;
			}
			this.SpreadAngle = this.CurveAngle.Evaluate(this.Node.GetElapsedTime());
			for (int i = this.NumVerts / 2; i < this.NumVerts; i++)
			{
				this.Verts[i] = this.Verts[i - this.NumVerts / 2] + Vector3.up * this.Size.y;
				this.Verts[i] = Vector3.RotateTowards(this.Verts[i], this.Verts[i - this.NumVerts / 2], this.SpreadAngle * 0.017453292f, 0f);
			}
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x001125D0 File Offset: 0x001107D0
		public void UpdateVerts()
		{
			Vector3 vector = Vector3.forward * this.Size.x;
			for (int i = 0; i < this.NumVerts / 2; i++)
			{
				this.Verts[i] = Quaternion.Euler(0f, this.OriRotAngle + this.SegmentAngle * (float)i, 0f) * vector;
			}
			for (int j = this.NumVerts / 2; j < this.NumVerts; j++)
			{
				this.Verts[j] = this.Verts[j - this.NumVerts / 2] + Vector3.up * this.Size.y;
				this.Verts[j] = Vector3.RotateTowards(this.Verts[j], this.Verts[j - this.NumVerts / 2], this.SpreadAngle * 0.017453292f, 0f);
			}
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x001126F8 File Offset: 0x001108F8
		public void InitVerts()
		{
			this.NumVerts = (this.NumSegment + 1) * 2;
			this.SegmentAngle = 360f / (float)this.NumSegment;
			this.Verts = new Vector3[this.NumVerts];
			this.VertsTemp = new Vector3[this.NumVerts];
			this.UpdateVerts();
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumSegment; i++)
			{
				int num = indexStart + i * 6;
				int num2 = vertStart + i;
				pool.Indices[num] = num2 + this.NumSegment + 1;
				pool.Indices[num + 1] = num2 + this.NumSegment + 2;
				pool.Indices[num + 2] = num2;
				pool.Indices[num + 3] = num2 + this.NumSegment + 2;
				pool.Indices[num + 4] = num2 + 1;
				pool.Indices[num + 5] = num2;
				pool.Vertices[num2 + this.NumSegment + 1] = Vector3.zero;
				pool.Vertices[num2 + this.NumSegment + 2] = Vector3.zero;
				pool.Vertices[num2] = Vector3.zero;
				pool.Vertices[num2 + 1] = Vector3.zero;
			}
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x00112870 File Offset: 0x00110A70
		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			float num = this.UVDimensions.x / (float)this.NumSegment;
			for (int i = 0; i < this.NumSegment + 1; i++)
			{
				pool.UVs[vertStart + i] = this.LowerLeftUV;
				Vector2[] uvs = pool.UVs;
				int num2 = vertStart + i;
				uvs[num2].x = uvs[num2].x + (float)i * num;
			}
			for (int j = this.NumSegment + 1; j < this.NumVerts; j++)
			{
				pool.UVs[vertStart + j] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				Vector2[] uvs2 = pool.UVs;
				int num3 = vertStart + j;
				uvs2[num3].x = uvs2[num3].x + (float)(j - this.NumSegment - 1) * num;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0011298C File Offset: 0x00110B8C
		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumVerts; i++)
			{
				pool.Colors[vertStart + i] = this.Color;
			}
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x001129F4 File Offset: 0x00110BF4
		public void Transform()
		{
			if (this.Node.Owner.RotAffectorEnable || this.OriRotAngle != 0f)
			{
				this.UpdateVerts();
			}
			Quaternion quaternion;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				quaternion = Quaternion.FromToRotation(Vector3.up, this.Node.Owner.transform.rotation * this.Direction);
			}
			else
			{
				quaternion = Quaternion.FromToRotation(Vector3.up, this.Direction);
			}
			for (int i = 0; i < this.NumSegment + 1; i++)
			{
				this.VertsTemp[i] = this.Verts[i] * this.Scale.x;
				this.VertsTemp[i] = quaternion * this.VertsTemp[i];
				this.VertsTemp[i] = this.VertsTemp[i] + this.Position;
			}
			for (int j = this.NumSegment + 1; j < this.NumVerts; j++)
			{
				this.VertsTemp[j] = this.Verts[j] * this.Scale.x;
				this.VertsTemp[j].y = this.Verts[j].y * this.Scale.y;
				this.VertsTemp[j] = quaternion * this.VertsTemp[j];
				this.VertsTemp[j] = this.VertsTemp[j] + this.Position;
			}
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int k = 0; k < this.NumVerts; k++)
			{
				pool.Vertices[vertStart + k] = this.VertsTemp[k];
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x00112C50 File Offset: 0x00110E50
		public void MyUpdate(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				this.UpdateRotAngle(deltaTime);
				this.Transform();
				if (this.UVChanged)
				{
					this.UpdateUV();
				}
				if (this.ColorChanged)
				{
					this.UpdateColor();
				}
				this.UVChanged = (this.ColorChanged = false);
				if (!force)
				{
					this.ElapsedTime -= base.Fps;
				}
			}
		}

		// Token: 0x04002A61 RID: 10849
		public Vector2 Size;

		// Token: 0x04002A62 RID: 10850
		public Vector3 Direction;

		// Token: 0x04002A63 RID: 10851
		public int NumSegment = 4;

		// Token: 0x04002A64 RID: 10852
		public float SpreadAngle;

		// Token: 0x04002A65 RID: 10853
		public float OriSpreadAngle;

		// Token: 0x04002A66 RID: 10854
		public float OriRotAngle = 45f;

		// Token: 0x04002A67 RID: 10855
		public bool UseDeltaAngle;

		// Token: 0x04002A68 RID: 10856
		public AnimationCurve CurveAngle;

		// Token: 0x04002A69 RID: 10857
		protected int NumVerts;

		// Token: 0x04002A6A RID: 10858
		protected float SegmentAngle;

		// Token: 0x04002A6B RID: 10859
		protected VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002A6C RID: 10860
		protected Vector3[] Verts;

		// Token: 0x04002A6D RID: 10861
		protected Vector3[] VertsTemp;

		// Token: 0x04002A6E RID: 10862
		protected float ElapsedTime;

		// Token: 0x04002A6F RID: 10863
		protected bool UVChanged = true;

		// Token: 0x04002A70 RID: 10864
		protected bool ColorChanged = true;

		// Token: 0x04002A71 RID: 10865
		protected float OriUVX;

		// Token: 0x04002A72 RID: 10866
		public Vector3 Position = Vector3.zero;

		// Token: 0x04002A73 RID: 10867
		public Color Color;

		// Token: 0x04002A74 RID: 10868
		public Vector2 Scale;

		// Token: 0x04002A75 RID: 10869
		protected Vector2 LowerLeftUV = Vector2.zero;

		// Token: 0x04002A76 RID: 10870
		protected Vector2 UVDimensions = Vector2.one;
	}
}
