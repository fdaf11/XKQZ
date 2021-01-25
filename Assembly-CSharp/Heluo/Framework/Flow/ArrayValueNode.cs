using System;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000163 RID: 355
	[Description("Test/陣列資料輸出")]
	[GenericTypeConstraint(TypeConstraint.All, false, new Type[]
	{

	})]
	public class ArrayValueNode<T> : ValueNode<List<T>>
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x000065C5 File Offset: 0x000047C5
		public ArrayValueNode()
		{
			this.value = new List<T>();
		}
	}
}
