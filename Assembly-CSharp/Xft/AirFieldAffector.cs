using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000557 RID: 1367
	public class AirFieldAffector : Affector
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x0010DB74 File Offset: 0x0010BD74
		public AirFieldAffector(Transform airObj, Vector3 dir, float atten, bool useMaxdist, float maxDist, bool enableSpread, float spread, float inhV, bool inhRot, EffectNode node) : base(node, AFFECTORTYPE.AirFieldAffector)
		{
			this.AirObj = airObj;
			this.Direction = dir.normalized;
			this.Attenuation = atten;
			this.UseMaxDistance = useMaxdist;
			this.MaxDistance = maxDist;
			this.MaxDistanceSqr = this.MaxDistance * this.MaxDistance;
			this.EnableSpread = enableSpread;
			this.Spread = spread;
			this.InheritVelocity = inhV;
			this.InheritRotation = inhRot;
			this.LastFieldPos = this.AirObj.position;
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x0001700E File Offset: 0x0001520E
		public override void Reset()
		{
			this.LastFieldPos = this.AirObj.position;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0010DBFC File Offset: 0x0010BDFC
		public override void Update(float deltaTime)
		{
			Vector3 vector;
			if (this.InheritRotation)
			{
				vector = this.AirObj.rotation * this.Direction;
			}
			else
			{
				vector = this.Direction;
			}
			Vector3 vector2 = Vector3.zero;
			vector2 = (this.AirObj.position - this.LastFieldPos) * this.InheritVelocity / deltaTime;
			this.LastFieldPos = this.AirObj.position;
			float num;
			if (this.Node.Owner.AirMagType == MAGTYPE.Fixed)
			{
				num = this.Node.Owner.AirMagnitude;
			}
			else if (this.Node.Owner.AirMagType == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.Node.Owner.AirMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				num = this.Node.Owner.AirMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			vector2 += vector * num;
			float magnitude = vector2.magnitude;
			float num2 = (!this.EnableSpread) ? 0f : Mathf.Cos(1.5707964f * this.Spread);
			Vector3 vector3 = this.Node.GetOriginalPos() - this.AirObj.position;
			float sqrMagnitude = vector3.sqrMagnitude;
			if (!this.UseMaxDistance || sqrMagnitude < this.MaxDistanceSqr)
			{
				Vector3 vector4 = vector2;
				if (this.EnableSpread)
				{
					vector4 = vector3.normalized;
					if (Vector3.Dot(vector2, vector4) < num2)
					{
						return;
					}
					vector4 *= magnitude;
				}
				Vector3 vector5 = this.Node.Velocity;
				if (Vector3.Dot(vector4, vector5 - vector4) < 0f)
				{
					float num3 = deltaTime;
					if (this.UseMaxDistance && this.Attenuation > 1E-06f)
					{
						num3 *= Mathf.Pow(1f - Mathf.Sqrt(sqrMagnitude) / this.MaxDistance, this.Attenuation);
					}
					vector5 += vector4 * num3;
					this.Node.Velocity = vector5;
				}
			}
		}

		// Token: 0x040028BE RID: 10430
		protected Transform AirObj;

		// Token: 0x040028BF RID: 10431
		protected Vector3 Direction;

		// Token: 0x040028C0 RID: 10432
		protected float Attenuation;

		// Token: 0x040028C1 RID: 10433
		protected bool UseMaxDistance;

		// Token: 0x040028C2 RID: 10434
		protected float MaxDistance;

		// Token: 0x040028C3 RID: 10435
		protected float MaxDistanceSqr;

		// Token: 0x040028C4 RID: 10436
		protected bool EnableSpread;

		// Token: 0x040028C5 RID: 10437
		protected float Spread;

		// Token: 0x040028C6 RID: 10438
		protected float InheritVelocity;

		// Token: 0x040028C7 RID: 10439
		protected bool InheritRotation;

		// Token: 0x040028C8 RID: 10440
		protected Vector3 LastFieldPos;
	}
}
