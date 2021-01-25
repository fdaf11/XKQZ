using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000176 RID: 374
	[Description("亂數/Float")]
	public class FloatRandomNode : OutputNode<float>
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x0000675C File Offset: 0x0000495C
		public override float GetValue()
		{
			if (this.maxNode == null || this.minNode == null)
			{
				return 0f;
			}
			return Random.Range(this.minNode.GetValue(), this.maxNode.GetValue());
		}

		// Token: 0x040007C5 RID: 1989
		[Argument("上限")]
		public OutputNode<float> maxNode;

		// Token: 0x040007C6 RID: 1990
		[Argument("下限")]
		public OutputNode<float> minNode;
	}
}
