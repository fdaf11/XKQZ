using System;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017B RID: 379
	[Description("變數輸出")]
	public class VariableNode<T> : OutputNode<T>, IInput
	{
		// Token: 0x0600077E RID: 1918 RVA: 0x000067DD File Offset: 0x000049DD
		public override T GetValue()
		{
			return this.value;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000067E5 File Offset: 0x000049E5
		public void SetValue(object obj)
		{
			this.value = (T)((object)obj);
		}

		// Token: 0x040007CA RID: 1994
		[InputField(null, false)]
		[JsonIgnore]
		public T value;
	}
}
