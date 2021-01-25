using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200059A RID: 1434
	public class RibbonTrail : RenderObject
	{
		// Token: 0x060023DC RID: 9180 RVA: 0x00117F74 File Offset: 0x00116174
		public RibbonTrail(VertexPool.VertexSegment segment, bool useFaceObj, Transform faceobj, float width, int maxelemnt, float len, Vector3 pos, float maxFps)
		{
			if (maxelemnt <= 2)
			{
				Debug.LogError("ribbon trail's maxelement should > 2!");
			}
			this.MaxElements = maxelemnt;
			this.Vertexsegment = segment;
			this.ElementArray = new RibbonTrail.Element[this.MaxElements];
			this.Head = (this.Tail = 99999);
			this.OriHeadPos = pos;
			this.SetTrailLen(len);
			this.UnitWidth = width;
			this.HeadPosition = pos;
			Vector3 vector;
			if (this.UseFaceObject)
			{
				vector = faceobj.position - this.HeadPosition;
			}
			else
			{
				vector = Vector3.zero;
			}
			RibbonTrail.Element element = new RibbonTrail.Element(this.HeadPosition, this.UnitWidth);
			element.Normal = vector.normalized;
			this.IndexDirty = false;
			this.AddElememt(element);
			this.AddElememt(new RibbonTrail.Element(this.HeadPosition, this.UnitWidth)
			{
				Normal = vector.normalized
			});
			for (int i = 0; i < this.MaxElements; i++)
			{
				this.ElementPool.Add(new RibbonTrail.Element(this.HeadPosition, this.UnitWidth));
			}
			this.UseFaceObject = useFaceObj;
			this.FaceObject = faceobj;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x001180C4 File Offset: 0x001162C4
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

		// Token: 0x060023DE RID: 9182 RVA: 0x00118140 File Offset: 0x00116340
		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(node.LowerLeftUV, node.UVDimensions);
			this.SetColor(node.Color);
			this.SetHeadPosition(node.GetRealClientPos() + node.Position);
			this.ResetElementsPos();
			this.mLastClientPos = node.Owner.ClientTransform.position;
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x001181A8 File Offset: 0x001163A8
		public override void Reset()
		{
			Vector3 headPosition;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				headPosition = this.Node.ClientTrans.rotation * (this.Node.GetRealClientPos() + this.Node.Owner.EmitPoint);
			}
			else
			{
				headPosition = this.Node.GetRealClientPos() + this.Node.Owner.EmitPoint;
			}
			this.SetHeadPosition(headPosition);
			this.ResetElementsPos();
			this.StretchCount = 0;
			this.SetColor(Color.clear);
			this.UpdateVertices(Vector3.zero);
			this.mLastClientPos = this.Node.Owner.ClientTransform.position;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0011826C File Offset: 0x0011646C
		public override void Update(float deltaTime)
		{
			this.SetHeadPosition(this.Node.CurWorldPos);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			if (this.Node.Owner.SyncTrailWithClient)
			{
				this.CheckUpdateSync();
			}
			this.SetColor(this.Node.Color);
			this.MyUpdate(deltaTime);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x00118318 File Offset: 0x00116518
		private void CheckUpdateSync()
		{
			Vector3 vector = this.Node.Owner.ClientTransform.position - this.mLastClientPos;
			this.mLastClientPos = this.Node.Owner.ClientTransform.position;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head + 1;
				for (;;)
				{
					int num2 = num;
					if (num2 == this.MaxElements)
					{
						num2 = 0;
					}
					this.ElementArray[num2].Position += vector;
					if (num2 == this.Tail)
					{
						break;
					}
					num = num2 + 1;
				}
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x001183D0 File Offset: 0x001165D0
		public void ResetElementsPos()
		{
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head;
				for (;;)
				{
					int num2 = num;
					if (num2 == this.MaxElements)
					{
						num2 = 0;
					}
					this.ElementArray[num2].Position = this.OriHeadPos;
					if (num2 == this.Tail)
					{
						break;
					}
					num = num2 + 1;
				}
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00017DDB File Offset: 0x00015FDB
		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00017DFC File Offset: 0x00015FFC
		public void SetColor(Color color)
		{
			this.Color = color;
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x00017E05 File Offset: 0x00016005
		public void SetTrailLen(float len)
		{
			this.TrailLength = len;
			this.ElemLength = this.TrailLength / (float)(this.MaxElements - 2);
			this.SquaredElemLength = this.ElemLength * this.ElemLength;
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x00017E37 File Offset: 0x00016037
		public void SetHeadPosition(Vector3 pos)
		{
			this.HeadPosition = pos;
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00017E40 File Offset: 0x00016040
		public int GetStretchCount()
		{
			return this.StretchCount;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00118444 File Offset: 0x00116644
		public void Smooth()
		{
			if (this.ElemCount <= 3)
			{
				return;
			}
			RibbonTrail.Element element = this.ElementArray[this.Head];
			int num = this.Head + 1;
			if (num == this.MaxElements)
			{
				num = 0;
			}
			int num2 = num + 1;
			if (num2 == this.MaxElements)
			{
				num2 = 0;
			}
			RibbonTrail.Element element2 = this.ElementArray[num];
			RibbonTrail.Element element3 = this.ElementArray[num2];
			Vector3 vector = element.Position - element2.Position;
			Vector3 vector2 = element2.Position - element3.Position;
			float num3 = Vector3.Angle(vector, vector2);
			if (num3 > 60f)
			{
				Vector3 vector3 = (element.Position + element3.Position) / 2f;
				Vector3 vector4 = vector3 - element2.Position;
				Vector3 zero = Vector3.zero;
				float num4 = 0.1f / (num3 / 60f);
				element2.Position = Vector3.SmoothDamp(element2.Position, element2.Position + vector4.normalized * element2.Width, ref zero, num4);
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00118560 File Offset: 0x00116760
		private void ScaleLength()
		{
			float num = this.Node.Scale.y;
			num = Mathf.Clamp(num, 0.1f, num);
			this.SetTrailLen(this.Node.Owner.RibbonLen * num);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x001185A4 File Offset: 0x001167A4
		public void MyUpdate(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime < base.Fps)
			{
				return;
			}
			this.ElapsedTime -= base.Fps;
			if (this.Node.Owner.ScaleAffectorEnable)
			{
				this.ScaleLength();
			}
			bool flag = false;
			Vector3 vector = Vector3.one * this.Node.Owner.Owner.Scale;
			while (!flag)
			{
				RibbonTrail.Element element = this.ElementArray[this.Head];
				int num = this.Head + 1;
				if (num == this.MaxElements)
				{
					num = 0;
				}
				RibbonTrail.Element element2 = this.ElementArray[num];
				Vector3 headPosition = this.HeadPosition;
				Vector3 vector2 = headPosition - element2.Position;
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude >= this.SquaredElemLength)
				{
					this.StretchCount++;
					Vector3 vector3 = vector2 * (this.ElemLength / vector2.magnitude);
					element.Position = element2.Position + vector3;
					RibbonTrail.Element element3 = this.ElementPool[0];
					this.ElementPool.RemoveAt(0);
					element3.Position = headPosition;
					element3.Width = this.UnitWidth;
					if (this.UseFaceObject)
					{
						element3.Normal = this.FaceObject.position - Vector3.Scale(headPosition, vector);
					}
					else
					{
						element3.Normal = Vector3.zero;
					}
					this.AddElememt(element3);
					vector2 = headPosition - element.Position;
					if (vector2.sqrMagnitude <= this.SquaredElemLength)
					{
						flag = true;
					}
				}
				else
				{
					element.Position = headPosition;
					flag = true;
				}
				if ((this.Tail + 1) % this.MaxElements == this.Head)
				{
					RibbonTrail.Element element4 = this.ElementArray[this.Tail];
					int num2;
					if (this.Tail == 0)
					{
						num2 = this.MaxElements - 1;
					}
					else
					{
						num2 = this.Tail - 1;
					}
					RibbonTrail.Element element5 = this.ElementArray[num2];
					Vector3 vector4 = element4.Position - element5.Position;
					float magnitude = vector4.magnitude;
					if ((double)magnitude > 1E-06)
					{
						float num3 = this.ElemLength - vector2.magnitude;
						vector4 *= num3 / magnitude;
						element4.Position = element5.Position + vector4;
					}
				}
			}
			Vector3 position = this.Node.MyCamera.transform.position;
			this.UpdateVertices(position);
			this.UpdateIndices();
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00118848 File Offset: 0x00116A48
		public void UpdateIndices()
		{
			if (!this.IndexDirty)
			{
				return;
			}
			VertexPool pool = this.Vertexsegment.Pool;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head;
				int num2 = 0;
				for (;;)
				{
					int num3 = num + 1;
					if (num3 == this.MaxElements)
					{
						num3 = 0;
					}
					if (num3 * 2 >= 65536)
					{
						Debug.LogError("Too many elements!");
					}
					int num4 = this.Vertexsegment.VertStart + num3 * 2;
					int num5 = this.Vertexsegment.VertStart + num * 2;
					int num6 = this.Vertexsegment.IndexStart + num2 * 6;
					pool.Indices[num6] = num5;
					pool.Indices[num6 + 1] = num5 + 1;
					pool.Indices[num6 + 2] = num4;
					pool.Indices[num6 + 3] = num5 + 1;
					pool.Indices[num6 + 4] = num4 + 1;
					pool.Indices[num6 + 5] = num4;
					if (num3 == this.Tail)
					{
						break;
					}
					num = num3;
					num2++;
				}
				pool.IndiceChanged = true;
			}
			this.IndexDirty = false;
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00118970 File Offset: 0x00116B70
		public void UpdateVertices(Vector3 eyePos)
		{
			float num = 0f;
			float num2 = this.ElemLength * (float)(this.MaxElements - 2);
			Vector3 vector = Vector3.one * this.Node.Owner.Owner.Scale;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num3 = this.Head;
				int num4 = this.Head;
				for (;;)
				{
					if (num4 == this.MaxElements)
					{
						num4 = 0;
					}
					RibbonTrail.Element element = this.ElementArray[num4];
					if (num4 * 2 >= 65536)
					{
						Debug.LogError("Too many elements!");
					}
					int num5 = this.Vertexsegment.VertStart + num4 * 2;
					int num6 = num4 + 1;
					if (num6 == this.MaxElements)
					{
						num6 = 0;
					}
					Vector3 vector2;
					if (num4 == this.Head)
					{
						vector2 = Vector3.Scale(this.ElementArray[num6].Position, vector) - Vector3.Scale(element.Position, vector);
					}
					else if (num4 == this.Tail)
					{
						vector2 = Vector3.Scale(element.Position, vector) - Vector3.Scale(this.ElementArray[num3].Position, vector);
					}
					else
					{
						vector2 = Vector3.Scale(this.ElementArray[num6].Position, vector) - Vector3.Scale(this.ElementArray[num3].Position, vector);
					}
					Vector3 vector3;
					if (!this.UseFaceObject)
					{
						vector3 = eyePos - Vector3.Scale(element.Position, vector);
					}
					else
					{
						vector3 = element.Normal;
					}
					Vector3 vector4 = Vector3.Cross(vector2, vector3);
					vector4.Normalize();
					vector4 *= element.Width * 0.5f * this.Node.Scale.x;
					Vector3 vector5 = element.Position - vector4;
					Vector3 vector6 = element.Position + vector4;
					VertexPool pool = this.Vertexsegment.Pool;
					float num7;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						num7 = num / num2 * Mathf.Abs(this.UVDimensions.y);
					}
					else
					{
						num7 = num / num2 * Mathf.Abs(this.UVDimensions.x);
					}
					Vector2 zero = Vector2.zero;
					pool.Vertices[num5] = vector5;
					pool.Colors[num5] = this.Color;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						zero.x = this.LowerLeftUV.x + this.UVDimensions.x;
						zero.y = this.LowerLeftUV.y - num7;
					}
					else
					{
						zero.x = this.LowerLeftUV.x + num7;
						zero.y = this.LowerLeftUV.y;
					}
					pool.UVs[num5] = zero;
					pool.Vertices[num5 + 1] = vector6;
					pool.Colors[num5 + 1] = this.Color;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						zero.x = this.LowerLeftUV.x;
						zero.y = this.LowerLeftUV.y - num7;
					}
					else
					{
						zero.x = this.LowerLeftUV.x + num7;
						zero.y = this.LowerLeftUV.y - Mathf.Abs(this.UVDimensions.y);
					}
					pool.UVs[num5 + 1] = zero;
					if (num4 == this.Tail)
					{
						break;
					}
					num3 = num4;
					num += (this.ElementArray[num6].Position - element.Position).magnitude;
					num4++;
				}
				this.Vertexsegment.Pool.UVChanged = true;
				this.Vertexsegment.Pool.VertChanged = true;
				this.Vertexsegment.Pool.ColorChanged = true;
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x00118DC0 File Offset: 0x00116FC0
		public void AddElememt(RibbonTrail.Element dtls)
		{
			if (this.Head == 99999)
			{
				this.Tail = this.MaxElements - 1;
				this.Head = this.Tail;
				this.IndexDirty = true;
				this.ElemCount++;
			}
			else
			{
				if (this.Head == 0)
				{
					this.Head = this.MaxElements - 1;
				}
				else
				{
					this.Head--;
				}
				if (this.Head == this.Tail)
				{
					if (this.Tail == 0)
					{
						this.Tail = this.MaxElements - 1;
					}
					else
					{
						this.Tail--;
					}
				}
				else
				{
					this.ElemCount++;
				}
			}
			if (this.ElementArray[this.Head] != null)
			{
				this.ElementPool.Add(this.ElementArray[this.Head]);
			}
			this.ElementArray[this.Head] = dtls;
			this.IndexDirty = true;
		}

		// Token: 0x04002B82 RID: 11138
		public const int CHAIN_EMPTY = 99999;

		// Token: 0x04002B83 RID: 11139
		protected List<RibbonTrail.Element> ElementPool = new List<RibbonTrail.Element>();

		// Token: 0x04002B84 RID: 11140
		protected VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002B85 RID: 11141
		public int MaxElements;

		// Token: 0x04002B86 RID: 11142
		public RibbonTrail.Element[] ElementArray;

		// Token: 0x04002B87 RID: 11143
		public int Head;

		// Token: 0x04002B88 RID: 11144
		public int Tail;

		// Token: 0x04002B89 RID: 11145
		protected Vector3 HeadPosition;

		// Token: 0x04002B8A RID: 11146
		protected float TrailLength;

		// Token: 0x04002B8B RID: 11147
		protected float ElemLength;

		// Token: 0x04002B8C RID: 11148
		public float SquaredElemLength;

		// Token: 0x04002B8D RID: 11149
		protected float UnitWidth;

		// Token: 0x04002B8E RID: 11150
		protected bool IndexDirty;

		// Token: 0x04002B8F RID: 11151
		protected Vector2 LowerLeftUV;

		// Token: 0x04002B90 RID: 11152
		protected Vector2 UVDimensions;

		// Token: 0x04002B91 RID: 11153
		protected Color Color = Color.white;

		// Token: 0x04002B92 RID: 11154
		public int ElemCount;

		// Token: 0x04002B93 RID: 11155
		protected float ElapsedTime;

		// Token: 0x04002B94 RID: 11156
		protected int StretchCount;

		// Token: 0x04002B95 RID: 11157
		public Vector3 OriHeadPos;

		// Token: 0x04002B96 RID: 11158
		private bool UseFaceObject;

		// Token: 0x04002B97 RID: 11159
		private Transform FaceObject;

		// Token: 0x04002B98 RID: 11160
		protected Vector3 mLastClientPos;

		// Token: 0x0200059B RID: 1435
		public class Element
		{
			// Token: 0x060023EE RID: 9198 RVA: 0x00017E48 File Offset: 0x00016048
			public Element(Vector3 position, float width)
			{
				this.Position = position;
				this.Width = width;
			}

			// Token: 0x04002B99 RID: 11161
			public Vector3 Position;

			// Token: 0x04002B9A RID: 11162
			public Vector3 Normal;

			// Token: 0x04002B9B RID: 11163
			public float Width;
		}
	}
}
