using System;
using System.Collections;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200016A RID: 362
	[Description("Test/字串陣列輸出")]
	public class StringListNode : ListNode<string>
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x00048A08 File Offset: 0x00046C08
		public StringListNode()
		{
			List<string> list = new List<string>();
			list.Add("Apple");
			list.Add("Orange");
			list.Add("Lemon");
			this.list = list;
			base..ctor();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00006653 File Offset: 0x00004853
		public override IList GetValue()
		{
			return this.list;
		}

		// Token: 0x040007A9 RID: 1961
		private List<string> list;
	}
}
