using System;
using System.Collections;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200018F RID: 399
	internal class ListItemResolver : Resolver
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x00006DF6 File Offset: 0x00004FF6
		public ListItemResolver(IList list, int reference) : base(reference)
		{
			this.list = list;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00049F6C File Offset: 0x0004816C
		public override void Resolve(Dictionary<int, OutputNode> map)
		{
			OutputNode outputNode = map[this.reference];
			this.list.Add(outputNode);
		}

		// Token: 0x040007F8 RID: 2040
		private IList list;
	}
}
