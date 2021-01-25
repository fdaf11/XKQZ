using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B2 RID: 1458
	public class SmokeTrailPoint : PCTrailPoint
	{
		// Token: 0x0600245E RID: 9310 RVA: 0x00018316 File Offset: 0x00016516
		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			this.Position += this.RandomVec * deltaTime;
		}

		// Token: 0x04002C30 RID: 11312
		public Vector3 RandomVec;
	}
}
