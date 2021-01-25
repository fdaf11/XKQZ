using System;
using System.Reflection;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017E RID: 382
	[Description("內部使用/FieldNode")]
	[GenericTypeConstraint(TypeConstraint.All)]
	public class ObjectFieldNode<T> : ObjectMemberNode<T>
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x00048F88 File Offset: 0x00047188
		public override T GetValue()
		{
			object value = this.objectNode.GetValue();
			if (value == null || this.memberInfo == null)
			{
				return default(T);
			}
			FieldInfo fieldInfo = this.memberInfo as FieldInfo;
			object obj;
			try
			{
				obj = fieldInfo.GetValue(value);
			}
			catch (Exception)
			{
				obj = default(T);
			}
			return (T)((object)obj);
		}
	}
}
