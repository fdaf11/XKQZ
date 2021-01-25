using System;
using System.Collections;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000169 RID: 361
	[Description("Test/數字陣列輸出")]
	public class IntegerListNode : ListNode<int>
	{
		// Token: 0x06000758 RID: 1880 RVA: 0x000489C4 File Offset: 0x00046BC4
		public IntegerListNode()
		{
			List<int> list = new List<int>();
			list.Add(5);
			list.Add(4);
			list.Add(3);
			list.Add(2);
			list.Add(1);
			this.list = list;
			base..ctor();
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000664B File Offset: 0x0000484B
		public override IList GetValue()
		{
			return this.list;
		}

		// Token: 0x040007A8 RID: 1960
		private List<int> list;
	}
}
