using System;
using System.Collections;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000168 RID: 360
	public abstract class ListNode<T> : OutputNode<IList>
	{
		// Token: 0x06000757 RID: 1879 RVA: 0x000489A0 File Offset: 0x00046BA0
		public override IList GetValue()
		{
			List<T> list = new List<T>();
			list.Add(default(T));
			return list;
		}
	}
}
