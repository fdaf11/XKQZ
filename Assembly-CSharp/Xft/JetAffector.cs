using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000556 RID: 1366
	public class JetAffector : Affector
	{
		// Token: 0x06002272 RID: 8818 RVA: 0x00016FEE File Offset: 0x000151EE
		public JetAffector(float mag, MAGTYPE type, AnimationCurve curve, EffectNode node) : base(node, AFFECTORTYPE.JetAffector)
		{
			this.Mag = mag;
			this.MType = type;
			this.MagCurve = curve;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x0010DA70 File Offset: 0x0010BC70
		public override void Update(float deltaTime)
		{
			Vector3 vector = Vector3.zero;
			if (this.MType == MAGTYPE.Fixed)
			{
				vector = this.Node.Velocity.normalized * this.Mag * deltaTime;
			}
			else if (this.MType == MAGTYPE.Curve_OBSOLETE)
			{
				vector = this.Node.Velocity.normalized * this.MagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				vector = this.Node.Velocity.normalized * this.Node.Owner.JetCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			Vector3 vector2 = this.Node.Velocity + vector;
			if (Vector3.Dot(vector2, this.Node.Velocity) <= 0f)
			{
				this.Node.Velocity = Vector3.zero;
				return;
			}
			this.Node.Velocity = vector2;
		}

		// Token: 0x040028BB RID: 10427
		protected float Mag;

		// Token: 0x040028BC RID: 10428
		protected MAGTYPE MType;

		// Token: 0x040028BD RID: 10429
		protected AnimationCurve MagCurve;
	}
}
