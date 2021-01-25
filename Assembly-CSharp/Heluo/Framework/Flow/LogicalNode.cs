using System;
using System.Collections.Generic;
using System.Linq;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200016E RID: 366
	[Description("複數邏輯判斷")]
	public class LogicalNode : OutputNode<bool>
	{
		// Token: 0x06000763 RID: 1891 RVA: 0x00048BB0 File Offset: 0x00046DB0
		public override bool GetValue()
		{
			if (this.inputListNode == null)
			{
				return false;
			}
			if (this.inputListNode.Count == 0)
			{
				return false;
			}
			Func<OutputNode<bool>, bool> func = (OutputNode<bool> item) => item != null && item.GetValue();
			if (this.op == LogicalOperator.And)
			{
				return Enumerable.All<OutputNode<bool>>(this.inputListNode, func);
			}
			return Enumerable.Any<OutputNode<bool>>(this.inputListNode, func);
		}

		// Token: 0x040007B9 RID: 1977
		[Argument("Bool 陣列")]
		public List<OutputNode<bool>> inputListNode = new List<OutputNode<bool>>();

		// Token: 0x040007BA RID: 1978
		[InputField("條件", false)]
		public LogicalOperator op;
	}
}
