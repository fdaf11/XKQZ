using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000579 RID: 1401
	public class CustomMesh : RenderObject
	{
		// Token: 0x06002301 RID: 8961 RVA: 0x00112CDC File Offset: 0x00110EDC
		public CustomMesh(VertexPool.VertexSegment segment, Mesh mesh, Vector3 dir, float maxFps)
		{
			this.MyMesh = mesh;
			this.MeshVerts = new Vector3[mesh.vertices.Length];
			mesh.vertices.CopyTo(this.MeshVerts, 0);
			this.Vertexsegment = segment;
			this.MyDirection = dir;
			this.SetPosition(Vector3.zero);
			this.InitVerts();
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x00112D5C File Offset: 0x00110F5C
		public override void ApplyShaderParam(float x, float y)
		{
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.Vertexsegment.VertCount; i++)
			{
				pool.UVs2[vertStart + i] = one;
			}
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x00112DD8 File Offset: 0x00110FD8
		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetColor(this.Node.Color);
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetScale(this.Node.OriScaleX, this.Node.OriScaleY);
			this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			this.SetDirection(this.Node.OriDirection);
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0001731B File Offset: 0x0001551B
		public override void Reset()
		{
			this.SetColor(Color.clear);
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.Update(true, 0f);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x00112E58 File Offset: 0x00111058
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
			this.Update(false, deltaTime);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x00017346 File Offset: 0x00015546
		public void SetDirection(Vector3 dir)
		{
			this.MyDirection = dir;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0001734F File Offset: 0x0001554F
		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.UVChanged = true;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x00017377 File Offset: 0x00015577
		public void SetColor(Color c)
		{
			this.MyColor = c;
			this.ColorChanged = true;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x00017387 File Offset: 0x00015587
		public void SetPosition(Vector3 pos)
		{
			this.MyPosition = pos;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x00017390 File Offset: 0x00015590
		public void SetScale(float width, float height)
		{
			this.MyScale.x = width;
			this.MyScale.y = height;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000173AA File Offset: 0x000155AA
		public void SetRotation(float angle)
		{
			this.MyRotation = Quaternion.AngleAxis(angle, this.Node.Owner.MeshRotateAxis);
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x00112F48 File Offset: 0x00111148
		public void InitVerts()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.MeshVerts.Length; i++)
			{
				pool.Vertices[vertStart + i] = Vector3.zero;
			}
			int[] triangles = this.MyMesh.triangles;
			for (int j = 0; j < this.Vertexsegment.IndexCount; j++)
			{
				pool.Indices[j + indexStart] = triangles[j] + vertStart;
			}
			this.m_oriUvs = this.MyMesh.uv;
			for (int k = 0; k < this.m_oriUvs.Length; k++)
			{
				pool.UVs[k + vertStart] = this.m_oriUvs[k];
			}
			Color[] colors = this.MyMesh.colors;
			for (int l = 0; l < colors.Length; l++)
			{
				pool.Colors[l + vertStart] = Color.clear;
			}
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0011307C File Offset: 0x0011127C
		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.m_oriUvs.Length; i++)
			{
				Vector2 vector = this.LowerLeftUV + Vector2.Scale(this.m_oriUvs[i], this.UVDimensions);
				if (vector.x > this.UVDimensions.x + this.LowerLeftUV.x)
				{
					float num = vector.x - this.UVDimensions.x - this.LowerLeftUV.x;
					num = Mathf.Repeat(num, this.UVDimensions.x + this.LowerLeftUV.x);
					vector.x = this.LowerLeftUV.x + num * this.UVDimensions.x;
				}
				pool.UVs[i + vertStart] = vector;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0011318C File Offset: 0x0011138C
		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.Vertexsegment.VertCount; i++)
			{
				pool.Colors[vertStart + i] = this.MyColor;
			}
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x001131F8 File Offset: 0x001113F8
		public void Transform()
		{
			Quaternion quaternion;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				quaternion = Quaternion.FromToRotation(Vector3.up, this.Node.Owner.transform.rotation * this.MyDirection);
			}
			else
			{
				quaternion = Quaternion.FromToRotation(Vector3.up, this.MyDirection);
			}
			Vector3 one = Vector3.one;
			one.x = (one.z = this.MyScale.x);
			one.y = this.MyScale.y;
			this.LocalMat.SetTRS(Vector3.zero, quaternion * this.MyRotation, one);
			this.WorldMat.SetTRS(this.MyPosition, Quaternion.identity, Vector3.one);
			Matrix4x4 matrix4x = this.WorldMat * this.LocalMat;
			VertexPool pool = this.Vertexsegment.Pool;
			for (int i = this.Vertexsegment.VertStart; i < this.Vertexsegment.VertStart + this.Vertexsegment.VertCount; i++)
			{
				pool.Vertices[i] = matrix4x.MultiplyPoint3x4(this.MeshVerts[i - this.Vertexsegment.VertStart]);
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x00113354 File Offset: 0x00111554
		public void Update(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				this.Transform();
				if (this.ColorChanged)
				{
					this.UpdateColor();
				}
				if (this.UVChanged)
				{
					this.UpdateUV();
				}
				this.ColorChanged = (this.UVChanged = false);
				if (!force)
				{
					this.ElapsedTime -= base.Fps;
				}
			}
		}

		// Token: 0x04002A77 RID: 10871
		protected VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002A78 RID: 10872
		public Mesh MyMesh;

		// Token: 0x04002A79 RID: 10873
		public Vector3[] MeshVerts;

		// Token: 0x04002A7A RID: 10874
		public Color MyColor;

		// Token: 0x04002A7B RID: 10875
		public Vector3 MyPosition = Vector3.zero;

		// Token: 0x04002A7C RID: 10876
		public Vector2 MyScale = Vector2.one;

		// Token: 0x04002A7D RID: 10877
		public Quaternion MyRotation = Quaternion.identity;

		// Token: 0x04002A7E RID: 10878
		public Vector3 MyDirection;

		// Token: 0x04002A7F RID: 10879
		private Matrix4x4 LocalMat;

		// Token: 0x04002A80 RID: 10880
		private Matrix4x4 WorldMat;

		// Token: 0x04002A81 RID: 10881
		private float ElapsedTime;

		// Token: 0x04002A82 RID: 10882
		public bool ColorChanged;

		// Token: 0x04002A83 RID: 10883
		public bool UVChanged;

		// Token: 0x04002A84 RID: 10884
		protected Vector2 LowerLeftUV;

		// Token: 0x04002A85 RID: 10885
		protected Vector2 UVDimensions;

		// Token: 0x04002A86 RID: 10886
		protected Vector2[] m_oriUvs;
	}
}
