using System;
using System.Linq.Expressions;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000172 RID: 370
	[Description("數學運算/減法")]
	[GenericTypeConstraint(TypeConstraint.AllPrimitive)]
	public class SubtractNode<T> : OperatorNode<T>
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x000066EB File Offset: 0x000048EB
		public SubtractNode() : base(new Func<Expression, Expression, Expression>(Expression.Subtract))
		{
		}
	}
}
