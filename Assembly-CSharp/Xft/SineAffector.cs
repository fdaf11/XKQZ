using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000561 RID: 1377
	public class SineAffector : Affector
	{
		// Token: 0x0600228B RID: 8843 RVA: 0x00017118 File Offset: 0x00015318
		public SineAffector(EffectNode node) : base(node, AFFECTORTYPE.SineAffector)
		{
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		public override void Update(float deltaTime)
		{
			float sineTime = this.Node.Owner.SineTime;
			float num = this.Node.Owner.SineMagnitude;
			if (this.Node.Owner.SineMagType != MAGTYPE.Fixed)
			{
				num = this.Node.Owner.SineMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			float num2 = this.Node.GetElapsedTime() / sineTime * 2f * 3.1415927f;
			Vector3 vector = Mathf.Sin(num2) * this.Node.Owner.SineForce * num;
			if (this.Node.Owner.SineIsAccelarate)
			{
				this.Node.Velocity += vector;
			}
			else
			{
				this.Node.Position += vector;
			}
		}
	}
}
