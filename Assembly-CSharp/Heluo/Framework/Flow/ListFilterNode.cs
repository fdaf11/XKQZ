using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200016F RID: 367
	[GenericTypeConstraint(TypeConstraint.All, false, new Type[]
	{

	})]
	[Description("篩選陣列")]
	public class ListFilterNode<T> : ListNode<T>
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x00048C20 File Offset: 0x00046E20
		public override IList GetValue()
		{
			if (this.inputListNode == null || this.outputNode == null || this.comparerNode == null)
			{
				return null;
			}
			this.list.Clear();
			foreach (object obj in this.inputListNode.GetValue())
			{
				T t = (T)((object)obj);
				this.outputNode.value = t;
				if (this.comparerNode.GetValue())
				{
					this.list.Add(t);
				}
			}
			return this.list;
		}

		// Token: 0x040007BC RID: 1980
		[JsonIgnore]
		private List<T> list = new List<T>();

		// Token: 0x040007BD RID: 1981
		[Argument("輸入陣列")]
		public ListNode<T> inputListNode;

		// Token: 0x040007BE RID: 1982
		[Argument("< 暫存值")]
		public ValueNode<T> outputNode;

		// Token: 0x040007BF RID: 1983
		[Argument("篩選條件")]
		public OutputNode<bool> comparerNode;
	}
}
