using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000562 RID: 1378
	public class TurbulenceFieldAffector : Affector
	{
		// Token: 0x0600228D RID: 8845 RVA: 0x00017123 File Offset: 0x00015323
		public TurbulenceFieldAffector(Transform obj, float atten, bool useMax, float maxDist, EffectNode node) : base(node, AFFECTORTYPE.TurbulenceAffector)
		{
			this.TurbulenceObj = obj;
			this.UseMaxDistance = useMax;
			this.MaxDistance = maxDist;
			this.MaxDistanceSqr = this.MaxDistance * this.MaxDistance;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0010E8BC File Offset: 0x0010CABC
		protected void UpateNoAttenuation(float deltaTime)
		{
			float sqrMagnitude = (this.Node.GetOriginalPos() - this.TurbulenceObj.position).sqrMagnitude;
			float num;
			if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Fixed)
			{
				num = this.Node.Owner.TurbulenceMagnitude;
			}
			else if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.Node.Owner.TurbulenceMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				num = this.Node.Owner.TurbulenceMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (!this.UseMaxDistance || sqrMagnitude <= this.MaxDistanceSqr)
			{
				Vector3 vector;
				vector.x = Random.Range(-1f, 1f);
				vector.y = Random.Range(-1f, 1f);
				vector.z = Random.Range(-1f, 1f);
				vector *= num;
				vector = Vector3.Scale(vector, this.Node.Owner.TurbulenceForce);
				this.Node.Velocity += vector;
			}
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x0010EA0C File Offset: 0x0010CC0C
		public override void Update(float deltaTime)
		{
			if ((double)this.Attenuation < 1E-06)
			{
				this.UpateNoAttenuation(deltaTime);
				return;
			}
			float magnitude = (this.Node.GetOriginalPos() - this.TurbulenceObj.position).magnitude;
			float num;
			if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Fixed)
			{
				num = this.Node.Owner.TurbulenceMagnitude;
			}
			else if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.Node.Owner.TurbulenceMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				num = this.Node.Owner.TurbulenceMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (!this.UseMaxDistance || magnitude <= this.MaxDistance)
			{
				Vector3 vector;
				vector.x = Random.Range(-1f, 1f);
				vector.y = Random.Range(-1f, 1f);
				vector.z = Random.Range(-1f, 1f);
				vector *= num / (1f + magnitude * this.Attenuation);
				this.Node.Velocity += vector;
			}
		}

		// Token: 0x040028EF RID: 10479
		protected Transform TurbulenceObj;

		// Token: 0x040028F0 RID: 10480
		protected float Attenuation;

		// Token: 0x040028F1 RID: 10481
		protected bool UseMaxDistance;

		// Token: 0x040028F2 RID: 10482
		protected float MaxDistance;

		// Token: 0x040028F3 RID: 10483
		protected float MaxDistanceSqr;
	}
}
