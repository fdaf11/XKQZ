using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000563 RID: 1379
	public class VortexAffector : Affector
	{
		// Token: 0x06002290 RID: 8848 RVA: 0x0010EB70 File Offset: 0x0010CD70
		public VortexAffector(Transform obj, Vector3 dir, bool inhRot, EffectNode node) : base(node, AFFECTORTYPE.VortexAffector)
		{
			this.Direction = dir;
			this.InheritRotation = inhRot;
			this.VortexObj = obj;
			if (node.Owner.IsRandomVortexDir)
			{
				this.Direction.x = Random.Range(-1f, 1f);
				this.Direction.y = Random.Range(-1f, 1f);
				this.Direction.z = Random.Range(-1f, 1f);
			}
			this.Direction.Normalize();
			this.IsFirst = true;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x0010EC14 File Offset: 0x0010CE14
		public override void Reset()
		{
			this.IsFirst = true;
			this.OriginalRadius = 0f;
			if (this.Node.Owner.IsRandomVortexDir)
			{
				this.Direction.x = Random.Range(-1f, 1f);
				this.Direction.y = Random.Range(-1f, 1f);
				this.Direction.z = Random.Range(-1f, 1f);
			}
			this.Direction.Normalize();
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x0010ECA4 File Offset: 0x0010CEA4
		public override void Update(float deltaTime)
		{
			Vector3 vector = this.Node.GetOriginalPos() - this.VortexObj.position;
			if (this.Node.Owner.IsRandomVortexDir && this.IsFirst)
			{
				this.Direction = Vector3.Cross(vector, this.Direction);
			}
			Vector3 vector2 = this.Direction;
			if (this.InheritRotation)
			{
				vector2 = this.Node.Owner.ClientTransform.rotation * vector2;
			}
			if (this.IsFirst)
			{
				this.IsFirst = false;
				this.OriginalRadius = (vector - Vector3.Project(vector, vector2)).magnitude;
			}
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return;
			}
			if (!this.Node.Owner.UseVortexMaxDistance || sqrMagnitude <= this.Node.Owner.VortexMaxDistance * this.Node.Owner.VortexMaxDistance)
			{
				float num = Vector3.Dot(vector2, vector);
				vector -= num * vector2;
				Vector3 vector3 = Vector3.zero;
				if (vector == Vector3.zero)
				{
					vector3 = vector;
				}
				else
				{
					vector3 = Vector3.Cross(vector2, vector).normalized;
				}
				float elapsedTime = this.Node.GetElapsedTime();
				float num2;
				if (this.Node.Owner.VortexMagType == MAGTYPE.Curve_OBSOLETE)
				{
					num2 = this.Node.Owner.VortexCurve.Evaluate(elapsedTime);
				}
				else if (this.Node.Owner.VortexMagType == MAGTYPE.Fixed)
				{
					num2 = this.Node.Owner.VortexMag;
				}
				else
				{
					num2 = this.Node.Owner.VortexCurveX.Evaluate(elapsedTime);
				}
				if (this.Node.Owner.VortexAttenuation < 0.0001f)
				{
					vector3 *= num2 * deltaTime;
				}
				else
				{
					vector3 *= num2 * deltaTime / Mathf.Pow(Mathf.Sqrt(sqrMagnitude), this.Node.Owner.VortexAttenuation);
				}
				if (this.Node.Owner.IsVortexAccelerate)
				{
					this.Node.Velocity += vector3;
				}
				else if (this.Node.Owner.IsFixedCircle)
				{
					Vector3 vector4 = this.Node.GetOriginalPos() + vector3 - this.VortexObj.position;
					Vector3 vector5 = Vector3.Project(vector4, vector2);
					Vector3 vector6 = vector4 - vector5;
					if (this.Node.Owner.SyncClient)
					{
						this.Node.Position = vector6.normalized * this.OriginalRadius + vector5;
					}
					else
					{
						this.Node.Position = this.Node.GetRealClientPos() + vector6.normalized * this.OriginalRadius + vector5;
					}
				}
				else
				{
					this.Node.Position += vector3;
				}
			}
		}

		// Token: 0x040028F4 RID: 10484
		protected Vector3 Direction;

		// Token: 0x040028F5 RID: 10485
		protected Transform VortexObj;

		// Token: 0x040028F6 RID: 10486
		protected bool InheritRotation;

		// Token: 0x040028F7 RID: 10487
		protected bool IsFirst = true;

		// Token: 0x040028F8 RID: 10488
		protected float OriginalRadius;
	}
}
