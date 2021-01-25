using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000186 RID: 390
	public abstract class OutputNode<T> : OutputNode, IOutput
	{
		// Token: 0x060007A6 RID: 1958 RVA: 0x00006907 File Offset: 0x00004B07
		object IOutput.GetValue()
		{
			return this.GetValue();
		}

		// Token: 0x060007A7 RID: 1959
		public abstract T GetValue();

		// Token: 0x060007A8 RID: 1960 RVA: 0x00049498 File Offset: 0x00047698
		public override string ToString()
		{
			if (this.GetValue() == null)
			{
				return "NULL !!";
			}
			T value = this.GetValue();
			return value.ToString();
		}
	}
}
