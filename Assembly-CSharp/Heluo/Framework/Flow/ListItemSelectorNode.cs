using System;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200016C RID: 364
	[Description("從陣列選一個輸出")]
	[GenericTypeConstraint(TypeConstraint.All, false, new Type[]
	{

	})]
	public class ListItemSelectorNode<T> : OutputNode<T>
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x00048AA0 File Offset: 0x00046CA0
		public override T GetValue()
		{
			if (this.inputListNode == null || this.indexNode == null)
			{
				return default(T);
			}
			List<T> list = this.inputListNode.GetValue() as List<T>;
			int num = this.indexNode.GetValue();
			if (list.Count == 0)
			{
				return default(T);
			}
			if (num < 0 || num >= this.inputListNode.GetValue().Count)
			{
				num = 0;
			}
			list.Sort(delegate(T a, T b)
			{
				Comparison<float> comparison = (this.sortMode != SortMode.Max) ? ListItemSelectorNode<T>.Desc : ListItemSelectorNode<T>.Asc;
				this.tempValueNode.value = a;
				this.sortValueNode.OnArgumentChange();
				float value = this.sortValueNode.GetValue();
				this.tempValueNode.value = b;
				this.sortValueNode.OnArgumentChange();
				float value2 = this.sortValueNode.GetValue();
				return comparison.Invoke(value, value2);
			});
			return list[num];
		}

		// Token: 0x040007AD RID: 1965
		private static readonly Comparison<float> Asc = (float a, float b) => (a != b) ? ((a <= b) ? 1 : -1) : 0;

		// Token: 0x040007AE RID: 1966
		private static readonly Comparison<float> Desc = (float a, float b) => (a != b) ? ((a >= b) ? 1 : -1) : 0;

		// Token: 0x040007AF RID: 1967
		[Argument("輸入陣列")]
		public ListNode<T> inputListNode;

		// Token: 0x040007B0 RID: 1968
		[Argument("索引值")]
		public OutputNode<int> indexNode;

		// Token: 0x040007B1 RID: 1969
		[Argument("< 暫存值")]
		public ValueNode<T> tempValueNode;

		// Token: 0x040007B2 RID: 1970
		[Argument("依此值排序")]
		public OutputNode<float> sortValueNode;

		// Token: 0x040007B3 RID: 1971
		[InputField("排序方式", false)]
		public SortMode sortMode;
	}
}
