using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000179 RID: 377
	[GenericTypeConstraint(TypeConstraint.All, false, new Type[]
	{

	})]
	[Description("資料輸出")]
	public class ValueNode<T> : OutputNode<T>, IInput, IInput<T>
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x000067A3 File Offset: 0x000049A3
		void IInput.SetValue(object obj)
		{
			this.value = (T)((object)obj);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000067B1 File Offset: 0x000049B1
		void IInput<!0>.SetValue(T obj)
		{
			this.value = obj;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000067BA File Offset: 0x000049BA
		public override T GetValue()
		{
			return this.value;
		}

		// Token: 0x040007C7 RID: 1991
		[InputField("資料值", false)]
		public T value;
	}
}
