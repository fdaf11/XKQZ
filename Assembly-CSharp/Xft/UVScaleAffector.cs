using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000553 RID: 1363
	public class UVScaleAffector : Affector
	{
		// Token: 0x06002269 RID: 8809 RVA: 0x00016F87 File Offset: 0x00015187
		public UVScaleAffector(EffectNode node) : base(node, AFFECTORTYPE.UVScaleAffector)
		{
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0010D44C File Offset: 0x0010B64C
		public override void Update(float deltaTime)
		{
			Vector2 uvdimensions = this.Node.UVDimensions;
			uvdimensions.x += this.Node.Owner.UVScaleXSpeed * deltaTime;
			uvdimensions.y += this.Node.Owner.UVScaleYSpeed * deltaTime;
			this.Node.UVDimensions = uvdimensions;
		}
	}
}
