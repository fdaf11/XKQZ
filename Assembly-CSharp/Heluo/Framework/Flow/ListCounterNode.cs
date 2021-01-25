using System;
using System.Collections;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000167 RID: 359
	[Description("計算陣列大小")]
	public class ListCounterNode : OutputNode<int>
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x0004896C File Offset: 0x00046B6C
		public override int GetValue()
		{
			if (this.inputListNode != null)
			{
				IList value = this.inputListNode.GetValue();
				if (value != null)
				{
					return value.Count;
				}
			}
			return 0;
		}

		// Token: 0x040007A7 RID: 1959
		[Argument]
		public OutputNode<IList> inputListNode;
	}
}
