using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000560 RID: 1376
	public class GravityAffector : Affector
	{
		// Token: 0x06002288 RID: 8840 RVA: 0x000170D5 File Offset: 0x000152D5
		public GravityAffector(Transform obj, GAFTTYPE gtype, bool isacc, Vector3 dir, EffectNode node) : base(node, AFFECTORTYPE.GravityAffector)
		{
			this.GType = gtype;
			this.Dir = dir;
			this.Dir.Normalize();
			this.GravityObj = obj;
			this.IsAccelerate = isacc;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0001710F File Offset: 0x0001530F
		public void SetAttraction(Transform goal)
		{
			this.GravityObj = goal;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0010E628 File Offset: 0x0010C828
		public override void Update(float deltaTime)
		{
			float num;
			if (this.Node.Owner.GravityMagType == MAGTYPE.Fixed)
			{
				num = this.Node.Owner.GravityMag;
			}
			else if (this.Node.Owner.GravityMagType == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.Node.Owner.GravityCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				num = this.Node.Owner.GravityCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (this.GType == GAFTTYPE.Planar)
			{
				Vector3 vector = this.Node.Owner.ClientTransform.rotation * this.Dir;
				if (this.IsAccelerate)
				{
					this.Node.Velocity += vector * num * deltaTime;
				}
				else
				{
					this.Node.Position += vector * num * deltaTime;
				}
			}
			else if (this.GType == GAFTTYPE.Spherical)
			{
				Vector3 vector2 = this.GravityObj.position - this.Node.GetOriginalPos();
				if (this.IsAccelerate)
				{
					this.Node.Velocity += vector2 * num * deltaTime;
				}
				else
				{
					this.Node.Position += vector2.normalized * num * deltaTime;
				}
			}
		}

		// Token: 0x040028EB RID: 10475
		protected GAFTTYPE GType;

		// Token: 0x040028EC RID: 10476
		protected Vector3 Dir;

		// Token: 0x040028ED RID: 10477
		protected Transform GravityObj;

		// Token: 0x040028EE RID: 10478
		protected bool IsAccelerate = true;
	}
}
