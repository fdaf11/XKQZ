using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000175 RID: 373
	[Description("亂數/Integer")]
	public class IntegerRandomNode : OutputNode<int>
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00006727 File Offset: 0x00004927
		public override int GetValue()
		{
			if (this.maxNode == null || this.minNode == null)
			{
				return 0;
			}
			return Random.Range(this.minNode.GetValue(), this.maxNode.GetValue());
		}

		// Token: 0x040007C3 RID: 1987
		[Argument("上限")]
		public OutputNode<int> maxNode;

		// Token: 0x040007C4 RID: 1988
		[Argument("下限")]
		public OutputNode<int> minNode;
	}
}
