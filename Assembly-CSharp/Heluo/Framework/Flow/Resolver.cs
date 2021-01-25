using System;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200018D RID: 397
	internal class Resolver
	{
		// Token: 0x060007EF RID: 2031 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public Resolver(int reference)
		{
			this.reference = reference;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Resolve(Dictionary<int, OutputNode> map)
		{
		}

		// Token: 0x040007F5 RID: 2037
		protected int reference;
	}
}
