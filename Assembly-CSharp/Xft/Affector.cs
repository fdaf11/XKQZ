using System;

namespace Xft
{
	// Token: 0x02000550 RID: 1360
	public class Affector
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x00016EF9 File Offset: 0x000150F9
		public Affector(EffectNode node, AFFECTORTYPE type)
		{
			this.Node = node;
			this.Type = type;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Reset()
		{
		}

		// Token: 0x040028AA RID: 10410
		public EffectNode Node;

		// Token: 0x040028AB RID: 10411
		public AFFECTORTYPE Type;
	}
}
