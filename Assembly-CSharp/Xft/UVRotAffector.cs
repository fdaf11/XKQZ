using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000552 RID: 1362
	public class UVRotAffector : Affector
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x00016F5F File Offset: 0x0001515F
		public UVRotAffector(float rotXSpeed, float rotYSpeed, EffectNode node) : base(node, AFFECTORTYPE.UVRotAffector)
		{
			this.RotXSpeed = rotXSpeed;
			this.RotYSpeed = rotYSpeed;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00016F7E File Offset: 0x0001517E
		public override void Reset()
		{
			this.FirstUpdate = true;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x0010D364 File Offset: 0x0010B564
		public override void Update(float deltaTime)
		{
			if (this.FirstUpdate)
			{
				if (this.Node.Owner.RandomStartFrame)
				{
					EffectNode node = this.Node;
					node.LowerLeftUV.x = node.LowerLeftUV.x + Random.Range(-1f, 1f);
					EffectNode node2 = this.Node;
					node2.LowerLeftUV.y = node2.LowerLeftUV.y + Random.Range(-1f, 1f);
				}
				else
				{
					this.Node.LowerLeftUV = this.Node.Owner.UVRotStartOffset;
				}
				this.FirstUpdate = false;
			}
			Vector2 lowerLeftUV = this.Node.LowerLeftUV;
			lowerLeftUV.x += this.RotXSpeed * deltaTime;
			lowerLeftUV.y += this.RotYSpeed * deltaTime;
			this.Node.LowerLeftUV = lowerLeftUV;
		}

		// Token: 0x040028AF RID: 10415
		protected float RotXSpeed;

		// Token: 0x040028B0 RID: 10416
		protected float RotYSpeed;

		// Token: 0x040028B1 RID: 10417
		protected bool FirstUpdate = true;
	}
}
