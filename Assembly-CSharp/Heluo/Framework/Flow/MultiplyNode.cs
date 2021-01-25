using System;
using System.Linq.Expressions;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000173 RID: 371
	[GenericTypeConstraint(TypeConstraint.AllPrimitive)]
	[Description("數學運算/乘法")]
	public class MultiplyNode<T> : OperatorNode<T>
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x000066FF File Offset: 0x000048FF
		public MultiplyNode() : base(new Func<Expression, Expression, Expression>(Expression.Multiply))
		{
		}
	}
}
