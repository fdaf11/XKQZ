using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200059D RID: 1437
	public class RopeData
	{
		// Token: 0x060023F1 RID: 9201 RVA: 0x00118ECC File Offset: 0x001170CC
		public void Init(EffectLayer owner)
		{
			this.Owner = owner;
			this.Vertexsegment = owner.GetVertexPool().GetRopeVertexSeg(owner.MaxENodes);
			this.dummyNode = new EffectNode(0, owner.ClientTransform, false, owner);
			List<Affector> affectorList = owner.InitAffectors(this.dummyNode);
			this.dummyNode.SetAffectorList(affectorList);
			this.dummyNode.SetRenderType(4);
			this.dummyNode.Init(Vector3.zero, 0f, -1f, 0, 1f, 1f, Color.clear, Vector2.zero, Vector2.one);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x00118F64 File Offset: 0x00117164
		protected void RefreshData()
		{
			this.NodeList.Clear();
			for (int i = 0; i < this.Owner.MaxENodes; i++)
			{
				EffectNode effectNode = this.Owner.ActiveENodes[i];
				if (effectNode != null)
				{
					this.NodeList.Add(effectNode);
				}
			}
			this.NodeList.Sort();
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x00017E79 File Offset: 0x00016079
		public void Update(float deltaTime)
		{
			this.RefreshData();
			if (this.NodeList.Count < 2)
			{
				return;
			}
			this.dummyNode.Update(deltaTime);
			this.ClearDeadVerts();
			this.UpdateVertices();
			this.UpdateIndices();
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x00118FC8 File Offset: 0x001171C8
		protected void ClearNodeVert(EffectNode node)
		{
			int num = this.Vertexsegment.VertStart + node.Index * 2;
			VertexPool pool = this.Vertexsegment.Pool;
			pool.Vertices[num] = this.Owner.ClientTransform.position;
			pool.Colors[num] = Color.clear;
			pool.Vertices[num + 1] = this.Owner.ClientTransform.position;
			pool.Colors[num + 1] = Color.clear;
			pool.VertChanged = true;
			pool.ColorChanged = true;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00119078 File Offset: 0x00117278
		public void ClearDeadVerts()
		{
			for (int i = 0; i < this.Owner.MaxENodes; i++)
			{
				EffectNode effectNode = this.Owner.AvailableENodes[i];
				if (effectNode != null)
				{
					this.ClearNodeVert(effectNode);
				}
			}
			this.Vertexsegment.ClearIndices();
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x001190CC File Offset: 0x001172CC
		public void UpdateIndices()
		{
			int num = 0;
			VertexPool pool = this.Vertexsegment.Pool;
			for (int i = this.NodeList.Count - 1; i >= 0; i--)
			{
				EffectNode effectNode = this.NodeList[i];
				EffectNode effectNode2 = (i - 1 < 0) ? null : this.NodeList[i - 1];
				if (effectNode2 == null)
				{
					break;
				}
				int num2 = this.Vertexsegment.VertStart + effectNode.Index * 2;
				int num3 = this.Vertexsegment.VertStart + effectNode2.Index * 2;
				int num4 = this.Vertexsegment.IndexStart + num * 6;
				pool.Indices[num4] = num2;
				pool.Indices[num4 + 1] = num2 + 1;
				pool.Indices[num4 + 2] = num3;
				pool.Indices[num4 + 3] = num2 + 1;
				pool.Indices[num4 + 4] = num3 + 1;
				pool.Indices[num4 + 5] = num3;
				num++;
			}
			pool.IndiceChanged = true;
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x001191D8 File Offset: 0x001173D8
		public void UpdateVertices()
		{
			float num = 0f;
			Vector2 lowerLeftUV = this.dummyNode.LowerLeftUV;
			Vector2 uvdimensions = this.dummyNode.UVDimensions;
			uvdimensions.y = -uvdimensions.y;
			lowerLeftUV.y = 1f - lowerLeftUV.y;
			float num2 = this.Owner.RopeUVLen;
			if (this.Owner.RopeFixUVLen)
			{
				float num3 = 0f;
				for (int i = 0; i < this.NodeList.Count - 1; i++)
				{
					num3 += (this.NodeList[i + 1].GetWorldPos() - this.NodeList[i].GetWorldPos()).magnitude;
				}
				num2 = num3;
			}
			for (int j = this.NodeList.Count - 1; j >= 0; j--)
			{
				EffectNode effectNode = this.NodeList[j];
				EffectNode effectNode2 = (j + 1 >= this.NodeList.Count) ? null : this.NodeList[j + 1];
				EffectNode effectNode3 = (j - 1 < 0) ? null : this.NodeList[j - 1];
				Vector3 vector;
				if (effectNode3 == null)
				{
					vector = effectNode.GetWorldPos() - effectNode2.GetWorldPos();
				}
				else if (effectNode2 == null)
				{
					vector = effectNode3.GetWorldPos() - effectNode.GetWorldPos();
				}
				else
				{
					vector = effectNode3.GetWorldPos() - effectNode2.GetWorldPos();
				}
				Vector3 position = this.Owner.MyCamera.transform.position;
				Vector3 vector2 = position - effectNode.GetWorldPos();
				Vector3 vector3 = Vector3.Cross(vector, vector2);
				vector3.Normalize();
				vector3 *= this.Owner.RopeWidth * 0.5f * effectNode.Scale.x;
				Vector3 vector4 = effectNode.GetWorldPos() - vector3;
				Vector3 vector5 = effectNode.GetWorldPos() + vector3;
				VertexPool pool = this.Vertexsegment.Pool;
				float num4 = num / num2 * Mathf.Abs(uvdimensions.y);
				Vector2 zero = Vector2.zero;
				int num5 = this.Vertexsegment.VertStart + effectNode.Index * 2;
				pool.Vertices[num5] = vector4;
				pool.Colors[num5] = effectNode.Color;
				zero.x = lowerLeftUV.x + uvdimensions.x;
				zero.y = lowerLeftUV.y - num4;
				pool.UVs[num5] = zero;
				pool.Vertices[num5 + 1] = vector5;
				pool.Colors[num5 + 1] = effectNode.Color;
				zero.x = lowerLeftUV.x;
				zero.y = lowerLeftUV.y - num4;
				pool.UVs[num5 + 1] = zero;
				if (effectNode3 != null)
				{
					num += (effectNode3.GetWorldPos() - effectNode.GetWorldPos()).magnitude;
				}
				else
				{
					num += (effectNode.GetWorldPos() - effectNode2.GetWorldPos()).magnitude;
				}
			}
			this.Vertexsegment.Pool.UVChanged = true;
			this.Vertexsegment.Pool.VertChanged = true;
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		// Token: 0x04002B9C RID: 11164
		public List<EffectNode> NodeList = new List<EffectNode>();

		// Token: 0x04002B9D RID: 11165
		public VertexPool.VertexSegment Vertexsegment;

		// Token: 0x04002B9E RID: 11166
		public EffectLayer Owner;

		// Token: 0x04002B9F RID: 11167
		public EffectNode dummyNode;
	}
}
