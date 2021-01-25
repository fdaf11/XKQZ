using System;
using System.Linq.Expressions;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000171 RID: 369
	[Description("數學運算/加法")]
	[GenericTypeConstraint(TypeConstraint.AllPrimitive)]
	public class AddNode<T> : OperatorNode<T>
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x000066D7 File Offset: 0x000048D7
		public AddNode() : base(new Func<Expression, Expression, Expression>(Expression.Add))
		{
		}
	}
}
