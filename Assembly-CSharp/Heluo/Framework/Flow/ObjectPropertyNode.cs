using System;
using System.Reflection;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017D RID: 381
	[GenericTypeConstraint(TypeConstraint.All)]
	[Description("內部使用/PropertyNode")]
	public class ObjectPropertyNode<T> : ObjectMemberNode<T>
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00048F0C File Offset: 0x0004710C
		public override T GetValue()
		{
			object value = this.objectNode.GetValue();
			if (value == null || this.memberInfo == null)
			{
				return default(T);
			}
			PropertyInfo propertyInfo = this.memberInfo as PropertyInfo;
			object obj;
			try
			{
				obj = propertyInfo.GetValue(value, null);
			}
			catch (Exception)
			{
				obj = default(T);
			}
			return (T)((object)obj);
		}
	}
}
