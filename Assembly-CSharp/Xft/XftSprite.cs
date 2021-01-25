using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A6 RID: 1446
	public class XftSprite : RenderObject
	{
		// Token: 0x06002421 RID: 9249 RVA: 0x0011A7D8 File Offset: 0x001189D8
		public XftSprite(VertexPool.VertexSegment segment, float width, float height, STYPE type, ORIPOINT oripoint, float maxFps, bool simple)
		{
			this.UVChanged = (this.ColorChanged = false);
			this.MyTransform.position = Vector3.zero;
			this.MyTransform.rotation = Quaternion.identity;
			this.LocalMat = (this.WorldMat = Matrix4x4.identity);
			this.Vertexsegment = segment;
			this.LastMat = Matrix4x4.identity;
			this.ElapsedTime = 0f;
			this.Simple = simple;
			this.OriPoint = oripoint;
			this.RotateAxis = Vector3.zero;
			this.SetSizeXZ(width, height);
			this.RotateAxis.y = 1f;
			this.Type = type;
			this.ResetSegment();
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0011A8BC File Offset: 0x00118ABC
		public override void ApplyShaderParam(float x, float y)
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
			pool.UVs2[vertStart] = one;
			pool.UVs2[vertStart + 1] = one;
			pool.UVs2[vertStart + 2] = one;
			pool.UVs2[vertStart + 3] = one;
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x0011A958 File Offset: 0x00118B58
		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(node.LowerLeftUV, node.UVDimensions);
			this.SetColor(this.Node.Color);
			if (this.Simple)
			{
				node.Update(0f);
				this.Transform();
			}
			if (node.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.SetRotationTo(node.Owner.ClientTransform.rotation * node.OriDirection);
			}
			else
			{
				this.SetRotationTo(node.OriDirection);
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x00017FD7 File Offset: 0x000161D7
		public override void Reset()
		{
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetPosition(this.Node.Position);
			this.SetColor(Color.clear);
			this.Update(true, 0f);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0011A9F0 File Offset: 0x00118BF0
		public override void Update(float deltaTime)
		{
			if (this.Node.Owner.AlwaysSyncRotation && this.Node.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.SetRotationTo(this.Node.Owner.ClientTransform.rotation * this.Node.OriDirection);
			}
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

		// Token: 0x06002426 RID: 9254 RVA: 0x0011AB34 File Offset: 0x00118D34
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

		// Token: 0x06002427 RID: 9255 RVA: 0x00018013 File Offset: 0x00016213
		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.UVChanged = true;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0001803B File Offset: 0x0001623B
		public void SetPosition(Vector3 pos)
		{
			this.MyTransform.position = pos;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00018049 File Offset: 0x00016249
		public void SetRotation(Quaternion q)
		{
			this.MyTransform.rotation = q;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x00018057 File Offset: 0x00016257
		public void SetRotationFaceTo(Vector3 dir)
		{
			this.MyTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0011ACEC File Offset: 0x00118EEC
		public void SetRotationTo(Vector3 dir)
		{
			if (dir == Vector3.zero)
			{
				return;
			}
			Quaternion rotation = Quaternion.identity;
			Vector3 vector = dir;
			vector.y = 0f;
			if (vector == Vector3.zero)
			{
				vector = Vector3.up;
			}
			if (this.OriPoint == ORIPOINT.CENTER)
			{
				Quaternion quaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), vector);
				Quaternion quaternion2 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion2 * quaternion;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_UP)
			{
				Quaternion quaternion3 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v3), vector);
				Quaternion quaternion4 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion4 * quaternion3;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
			{
				Quaternion quaternion5 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v4), vector);
				Quaternion quaternion6 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion6 * quaternion5;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
			{
				Quaternion quaternion7 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v1), vector);
				Quaternion quaternion8 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion8 * quaternion7;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_UP)
			{
				Quaternion quaternion9 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v2), vector);
				Quaternion quaternion10 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion10 * quaternion9;
			}
			else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
			{
				Quaternion quaternion11 = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), vector);
				Quaternion quaternion12 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion12 * quaternion11;
			}
			else if (this.OriPoint == ORIPOINT.TOP_CENTER)
			{
				Quaternion quaternion13 = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), vector);
				Quaternion quaternion14 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion14 * quaternion13;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
			{
				Quaternion quaternion15 = Quaternion.FromToRotation(new Vector3(-1f, 0f, 0f), vector);
				Quaternion quaternion16 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion16 * quaternion15;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
			{
				Quaternion quaternion17 = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), vector);
				Quaternion quaternion18 = Quaternion.FromToRotation(vector, dir);
				rotation = quaternion18 * quaternion17;
			}
			this.MyTransform.rotation = rotation;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0011AF6C File Offset: 0x0011916C
		public void SetSizeXZ(float width, float height)
		{
			this.v1 = new Vector3(-width / 2f, 0f, height / 2f);
			this.v2 = new Vector3(-width / 2f, 0f, -height / 2f);
			this.v3 = new Vector3(width / 2f, 0f, -height / 2f);
			this.v4 = new Vector3(width / 2f, 0f, height / 2f);
			Vector3 zero = Vector3.zero;
			if (this.OriPoint == ORIPOINT.LEFT_UP)
			{
				zero = this.v3;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
			{
				zero = this.v4;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
			{
				zero = this.v1;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_UP)
			{
				zero = this.v2;
			}
			else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
			{
				zero..ctor(0f, 0f, height / 2f);
			}
			else if (this.OriPoint == ORIPOINT.TOP_CENTER)
			{
				zero..ctor(0f, 0f, -height / 2f);
			}
			else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
			{
				zero..ctor(width / 2f, 0f, 0f);
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
			{
				zero..ctor(-width / 2f, 0f, 0f);
			}
			this.v1 += zero;
			this.v2 += zero;
			this.v3 += zero;
			this.v4 += zero;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0011B144 File Offset: 0x00119344
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

		// Token: 0x0600242E RID: 9262 RVA: 0x0011B2D8 File Offset: 0x001194D8
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

		// Token: 0x0600242F RID: 9263 RVA: 0x0001806F File Offset: 0x0001626F
		public void SetCustomHeight(float[] h)
		{
			this.UseCustomHeight = true;
			this.h1 = h[0];
			this.h2 = h[1];
			this.h3 = h[2];
			this.h4 = h[3];
			this.Transform();
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0011B370 File Offset: 0x00119570
		public void Transform()
		{
			this.LocalMat.SetTRS(Vector3.zero, this.Rotation, this.ScaleVector);
			Transform transform = this.Node.MyCamera.transform;
			if (this.Type == STYPE.BILLBOARD)
			{
				this.MyTransform.LookAt(transform.rotation * Vector3.up, transform.rotation * Vector3.back);
			}
			else if (this.Type == STYPE.BILLBOARD_Y)
			{
				Vector3 up = transform.position - this.MyTransform.position;
				up.y = 0f;
				this.MyTransform.LookAt(Vector3.up, up);
			}
			this.WorldMat.SetTRS(this.MyTransform.position, this.MyTransform.rotation, Vector3.one);
			Matrix4x4 matrix4x = this.WorldMat * this.LocalMat;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector3 vector = matrix4x.MultiplyPoint3x4(this.v1);
			Vector3 vector2 = matrix4x.MultiplyPoint3x4(this.v2);
			Vector3 vector3 = matrix4x.MultiplyPoint3x4(this.v3);
			Vector3 vector4 = matrix4x.MultiplyPoint3x4(this.v4);
			if (this.Type == STYPE.BILLBOARD_SELF)
			{
				Vector3 vector5 = Vector3.zero;
				Vector3 vector6 = Vector3.zero;
				Vector3 vector7 = Vector3.one * this.Node.Owner.Owner.Scale;
				float magnitude;
				if (this.Node.Owner.SpriteUVStretch == UV_STRETCH.Vertical)
				{
					vector5 = (vector + vector4) / 2f;
					vector6 = (vector2 + vector3) / 2f;
					magnitude = (vector4 - vector).magnitude;
				}
				else
				{
					vector5 = (vector + vector2) / 2f;
					vector6 = (vector4 + vector3) / 2f;
					magnitude = (vector2 - vector).magnitude;
				}
				Vector3 vector8 = vector5 - vector6;
				Vector3 vector9 = this.Node.MyCamera.transform.position - Vector3.Scale(vector5, vector7);
				Vector3 vector10 = Vector3.Cross(vector8, vector9);
				vector10.Normalize();
				vector10 *= magnitude * 0.5f;
				Vector3 vector11 = this.Node.MyCamera.transform.position - Vector3.Scale(vector6, vector7);
				Vector3 vector12 = Vector3.Cross(vector8, vector11);
				vector12.Normalize();
				vector12 *= magnitude * 0.5f;
				if (this.Node.Owner.SpriteUVStretch == UV_STRETCH.Vertical)
				{
					vector = vector5 - vector10;
					vector4 = vector5 + vector10;
					vector2 = vector6 - vector12;
					vector3 = vector6 + vector12;
				}
				else
				{
					vector = vector5 - vector10;
					vector2 = vector5 + vector10;
					vector4 = vector6 - vector12;
					vector3 = vector6 + vector12;
				}
			}
			pool.Vertices[vertStart] = vector;
			pool.Vertices[vertStart + 1] = vector2;
			pool.Vertices[vertStart + 2] = vector3;
			pool.Vertices[vertStart + 3] = vector4;
			if (this.UseCustomHeight)
			{
				pool.Vertices[vertStart].y = this.h1;
				pool.Vertices[vertStart + 1].y = this.h2;
				pool.Vertices[vertStart + 2].y = this.h3;
				pool.Vertices[vertStart + 3].y = this.h4;
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000180A2 File Offset: 0x000162A2
		public void SetRotation(float angle)
		{
			this.Rotation = Quaternion.AngleAxis(angle, this.RotateAxis);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000180B6 File Offset: 0x000162B6
		public void SetScale(float width, float height)
		{
			this.ScaleVector.x = width;
			this.ScaleVector.z = height;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0011B760 File Offset: 0x00119960
		public void Update(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				if (!this.Simple || force)
				{
					this.Transform();
				}
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

		// Token: 0x06002434 RID: 9268 RVA: 0x000180D0 File Offset: 0x000162D0
		public void SetColor(Color c)
		{
			this.Color = c;
			this.ColorChanged = true;
		}

		// Token: 0x04002BE4 RID: 11236
		protected Vector2 LowerLeftUV;

		// Token: 0x04002BE5 RID: 11237
		protected Vector2 UVDimensions;

		// Token: 0x04002BE6 RID: 11238
		public XTransform MyTransform;

		// Token: 0x04002BE7 RID: 11239
		public Vector3 v1 = Vector3.zero;

		// Token: 0x04002BE8 RID: 11240
		public Vector3 v2 = Vector3.zero;

		// Token: 0x04002BE9 RID: 11241
		public Vector3 v3 = Vector3.zero;

		// Token: 0x04002BEA RID: 11242
		public Vector3 v4 = Vector3.zero;

		// Token: 0x04002BEB RID: 11243
		protected VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002BEC RID: 11244
		public Color Color;

		// Token: 0x04002BED RID: 11245
		private Vector3 ScaleVector;

		// Token: 0x04002BEE RID: 11246
		private Quaternion Rotation;

		// Token: 0x04002BEF RID: 11247
		private Matrix4x4 LocalMat;

		// Token: 0x04002BF0 RID: 11248
		private Matrix4x4 WorldMat;

		// Token: 0x04002BF1 RID: 11249
		protected float ElapsedTime;

		// Token: 0x04002BF2 RID: 11250
		protected bool UVChanged;

		// Token: 0x04002BF3 RID: 11251
		protected bool ColorChanged;

		// Token: 0x04002BF4 RID: 11252
		protected Matrix4x4 LastMat;

		// Token: 0x04002BF5 RID: 11253
		protected Vector3 RotateAxis;

		// Token: 0x04002BF6 RID: 11254
		private STYPE Type;

		// Token: 0x04002BF7 RID: 11255
		private ORIPOINT OriPoint;

		// Token: 0x04002BF8 RID: 11256
		private bool Simple;

		// Token: 0x04002BF9 RID: 11257
		public bool UseCustomHeight;

		// Token: 0x04002BFA RID: 11258
		public float h1;

		// Token: 0x04002BFB RID: 11259
		public float h2;

		// Token: 0x04002BFC RID: 11260
		public float h3;

		// Token: 0x04002BFD RID: 11261
		public float h4;
	}
}
