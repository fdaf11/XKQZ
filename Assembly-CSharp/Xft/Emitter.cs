using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200057D RID: 1405
	public class Emitter
	{
		// Token: 0x0600232C RID: 9004 RVA: 0x001140D4 File Offset: 0x001122D4
		public Emitter(EffectLayer owner)
		{
			this.Layer = owner;
			this.EmitLoop = (float)this.Layer.EmitLoop;
			this.EmitDist = this.Layer.DiffDistance;
			this.LastClientPos = this.Layer.ClientTransform.position;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00114144 File Offset: 0x00112344
		public void Reset()
		{
			this.EmitterElapsedTime = 0f;
			this.IsFirstEmit = true;
			this.EmitLoop = (float)this.Layer.EmitLoop;
			this.EmitDist = this.Layer.DiffDistance;
			this.m_emitCount = 0;
			this.CurveEmitDone = false;
			this.m_curveCountTime = 0f;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x00017499 File Offset: 0x00015699
		public void StopEmit()
		{
			this.EmitLoop = 0f;
			this.EmitterElapsedTime = 999999f;
			this.EmitDist = 9999999f;
			this.mElapsedTime = 0f;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x001141A0 File Offset: 0x001123A0
		protected int EmitByCurve(float deltaTime)
		{
			XCurveParam emitterCurveX = this.Layer.EmitterCurveX;
			if (emitterCurveX == null)
			{
				Debug.LogWarning("emitter hasn't set a curve yet!");
				return 0;
			}
			this.EmitterElapsedTime += deltaTime;
			int num = (int)emitterCurveX.Evaluate(this.EmitterElapsedTime) - this.m_emitCount;
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.m_emitCount += num2;
			if (num2 == 0)
			{
				this.m_curveCountTime += deltaTime;
				if (this.m_curveCountTime > 1f)
				{
					this.CurveEmitDone = true;
				}
			}
			else
			{
				this.m_curveCountTime = 0f;
			}
			if (this.CurveEmitDone)
			{
				return 0;
			}
			return num2;
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x00114274 File Offset: 0x00112474
		protected int EmitByDistance()
		{
			if (this.Layer.mStopped)
			{
				return 0;
			}
			if ((this.Layer.ClientTransform.position - this.LastClientPos).magnitude >= this.EmitDist)
			{
				this.LastClientPos = this.Layer.ClientTransform.position;
				return 1;
			}
			return 0;
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x001142DC File Offset: 0x001124DC
		protected int EmitByBurst(float deltaTime)
		{
			if (!this.IsFirstEmit)
			{
				this.EmitLoop = 0f;
				return 0;
			}
			this.IsFirstEmit = false;
			int num = (int)this.Layer.EmitRate;
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 <= 0)
			{
				return 0;
			}
			return num2;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x00114348 File Offset: 0x00112548
		protected int EmitByRate(float deltaTime)
		{
			if (this.Layer.IsBurstEmit)
			{
				return this.EmitByBurst(deltaTime);
			}
			this.EmitterElapsedTime += deltaTime;
			if (this.Layer.EmitDuration > 0f && this.EmitterElapsedTime >= this.Layer.EmitDuration)
			{
				if (this.EmitLoop > 0f)
				{
					this.EmitLoop -= 1f;
				}
				this.m_emitCount = 0;
				this.EmitterElapsedTime = 0f;
				this.IsFirstEmit = false;
				this.mElapsedTime = 0f;
			}
			if (this.EmitLoop == 0f)
			{
				return 0;
			}
			if (this.Layer.AvailableNodeCount == 0)
			{
				return 0;
			}
			this.mElapsedTime += deltaTime;
			int num = (int)(this.mElapsedTime * this.Layer.EmitRate);
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 <= 0)
			{
				return 0;
			}
			this.mElapsedTime = 0f;
			this.m_emitCount += num2;
			return num2;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0011447C File Offset: 0x0011267C
		public Vector3 GetEmitRotation(EffectNode node)
		{
			Vector3 vector = Vector3.zero;
			if (this.Layer.DirType == DIRECTION_TYPE.Sphere)
			{
				vector = node.GetOriginalPos() - this.Layer.ClientTransform.position;
				if (vector == Vector3.zero)
				{
					Vector3 up = Vector3.up;
					Quaternion quaternion = Quaternion.Euler((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
					vector = quaternion * up;
				}
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Planar)
			{
				vector = this.Layer.OriVelocityAxis;
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Cone)
			{
				if (this.Layer.EmitType == 3 && this.Layer.EmitUniform)
				{
					Vector3 vector2;
					if (!this.Layer.SyncClient)
					{
						vector2 = node.Position - (node.GetRealClientPos() + this.Layer.EmitPoint);
					}
					else
					{
						vector2 = node.Position - this.Layer.EmitPoint;
					}
					int num = this.Layer.AngleAroundAxis;
					if (this.Layer.UseRandomDirAngle)
					{
						num = Random.Range(this.Layer.AngleAroundAxis, this.Layer.AngleAroundAxisMax);
					}
					Vector3 vector3 = Vector3.RotateTowards(vector2, this.Layer.CircleDir, (float)(90 - num) * 0.017453292f, 1f);
					Quaternion quaternion2 = Quaternion.FromToRotation(vector2, vector3);
					vector = quaternion2 * vector2;
				}
				else
				{
					Quaternion quaternion3 = Quaternion.Euler(0f, 0f, (float)this.Layer.AngleAroundAxis);
					Quaternion quaternion4 = Quaternion.Euler(0f, (float)Random.Range(0, 360), 0f);
					Quaternion quaternion5 = Quaternion.FromToRotation(Vector3.up, this.Layer.OriVelocityAxis);
					vector = quaternion5 * quaternion4 * quaternion3 * Vector3.up;
				}
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Cylindrical)
			{
				Vector3 vector4 = node.GetOriginalPos() - this.Layer.ClientTransform.position;
				if (vector4 == Vector3.zero)
				{
					Vector3 up2 = Vector3.up;
					Quaternion quaternion6 = Quaternion.Euler((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
					vector4 = quaternion6 * up2;
				}
				float num2 = Vector3.Dot(this.Layer.OriVelocityAxis, vector4);
				vector = vector4 - num2 * this.Layer.OriVelocityAxis.normalized;
			}
			return vector;
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x00114744 File Offset: 0x00112944
		public void SetEmitPosition(EffectNode node)
		{
			Vector3 vector = Vector3.zero;
			Vector3 realClientPos = node.GetRealClientPos();
			if (this.Layer.EmitType == 1)
			{
				Vector3 emitPoint = this.Layer.EmitPoint;
				float x = Random.Range(emitPoint.x - this.Layer.BoxSize.x / 2f, emitPoint.x + this.Layer.BoxSize.x / 2f);
				float y = Random.Range(emitPoint.y - this.Layer.BoxSize.y / 2f, emitPoint.y + this.Layer.BoxSize.y / 2f);
				float z = Random.Range(emitPoint.z - this.Layer.BoxSize.z / 2f, emitPoint.z + this.Layer.BoxSize.z / 2f);
				vector.x = x;
				vector.y = y;
				vector.z = z;
				if (!this.Layer.SyncClient)
				{
					vector = this.Layer.ClientTransform.rotation * vector + realClientPos;
				}
				else
				{
					vector = this.Layer.ClientTransform.rotation * vector;
				}
			}
			else if (this.Layer.EmitType == 0)
			{
				vector = this.Layer.EmitPoint;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + this.Layer.EmitPoint;
				}
			}
			else if (this.Layer.EmitType == 2)
			{
				vector = this.Layer.EmitPoint;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + this.Layer.EmitPoint;
				}
				Vector3 vector2 = Vector3.up * this.Layer.Radius;
				Quaternion quaternion = Quaternion.Euler((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
				vector = quaternion * vector2 + vector;
			}
			else if (this.Layer.EmitType == 4)
			{
				Vector3 position = this.Layer.LineStartObj.position;
				Vector3 position2 = this.Layer.LineEndObj.position;
				Vector3 vector3 = position2 - position;
				float num2;
				if (this.Layer.EmitUniform)
				{
					float num = (float)(node.Index + 1) / (float)this.Layer.MaxENodes;
					num2 = vector3.magnitude * num;
				}
				else
				{
					num2 = Random.Range(0f, vector3.magnitude);
				}
				vector = position + vector3.normalized * num2 - realClientPos;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + vector;
				}
			}
			else if (this.Layer.EmitType == 3)
			{
				float num4;
				if (this.Layer.EmitUniform)
				{
					int index = node.Index;
					float num3 = (float)(index + 1) / (float)this.Layer.MaxENodes;
					num4 = 360f * num3;
				}
				else
				{
					num4 = (float)Random.Range(0, 360);
				}
				float num5 = this.Layer.Radius;
				if (this.Layer.UseRandomCircle)
				{
					num5 = Random.Range(this.Layer.CircleRadiusMin, this.Layer.CircleRadiusMax);
				}
				Quaternion quaternion2 = Quaternion.Euler(0f, num4, 0f);
				Vector3 vector4 = quaternion2 * (Vector3.right * num5);
				Quaternion quaternion3 = Quaternion.FromToRotation(Vector3.up, this.Layer.ClientTransform.rotation * this.Layer.CircleDir);
				vector = quaternion3 * vector4;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + vector + this.Layer.EmitPoint;
				}
				else
				{
					vector += this.Layer.EmitPoint;
				}
			}
			else if (this.Layer.EmitType == 5)
			{
				if (this.Layer.EmitMesh == null)
				{
					Debug.LogWarning("please set a mesh to the emitter.");
					return;
				}
				if (this.Layer.EmitMeshType == 0)
				{
					int vertexCount = this.Layer.EmitMesh.vertexCount;
					int num6;
					if (this.Layer.EmitUniform)
					{
						num6 = node.Index % (vertexCount - 1);
					}
					else
					{
						num6 = Random.Range(0, vertexCount - 1);
					}
					vector = this.Layer.EmitMesh.vertices[num6];
					if (!this.Layer.SyncClient)
					{
						vector = realClientPos + vector + this.Layer.EmitPoint;
					}
					else
					{
						vector += this.Layer.EmitPoint;
					}
				}
				else if (this.Layer.EmitMeshType == 1)
				{
					Vector3[] vertices = this.Layer.EmitMesh.vertices;
					int num7 = this.Layer.EmitMesh.triangles.Length / 3;
					int num6;
					if (this.Layer.EmitUniform)
					{
						num6 = node.Index % (num7 - 1);
					}
					else
					{
						num6 = Random.Range(0, num7 - 1);
					}
					int num8 = this.Layer.EmitMesh.triangles[num6 * 3];
					int num9 = this.Layer.EmitMesh.triangles[num6 * 3 + 1];
					int num10 = this.Layer.EmitMesh.triangles[num6 * 3 + 2];
					vector = (vertices[num8] + vertices[num9] + vertices[num10]) / 3f;
					if (!this.Layer.SyncClient)
					{
						vector = realClientPos + vector + this.Layer.EmitPoint;
					}
					else
					{
						vector += this.Layer.EmitPoint;
					}
				}
			}
			node.SetLocalPosition(vector);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000174C7 File Offset: 0x000156C7
		public int GetNodes(float deltaTime)
		{
			if (this.Layer.EmitWay == EEmitWay.ByRate)
			{
				return this.EmitByRate(deltaTime);
			}
			if (this.Layer.EmitWay == EEmitWay.ByCurve)
			{
				return this.EmitByCurve(deltaTime);
			}
			return this.EmitByDistance();
		}

		// Token: 0x04002AAC RID: 10924
		public EffectLayer Layer;

		// Token: 0x04002AAD RID: 10925
		public float EmitterElapsedTime;

		// Token: 0x04002AAE RID: 10926
		protected float mElapsedTime;

		// Token: 0x04002AAF RID: 10927
		private bool IsFirstEmit = true;

		// Token: 0x04002AB0 RID: 10928
		public float EmitLoop;

		// Token: 0x04002AB1 RID: 10929
		public float EmitDist = 1f;

		// Token: 0x04002AB2 RID: 10930
		public bool CurveEmitDone;

		// Token: 0x04002AB3 RID: 10931
		private Vector3 LastClientPos = Vector3.zero;

		// Token: 0x04002AB4 RID: 10932
		protected int m_emitCount;

		// Token: 0x04002AB5 RID: 10933
		protected float m_curveCountTime;
	}
}
