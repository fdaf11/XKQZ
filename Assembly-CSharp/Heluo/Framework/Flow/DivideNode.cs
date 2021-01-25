using System;
using System.Linq.Expressions;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000174 RID: 372
	[Description("數學運算/除法")]
	[GenericTypeConstraint(TypeConstraint.AllPrimitive)]
	public class DivideNode<T> : OperatorNode<T>
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x00006713 File Offset: 0x00004913
		public DivideNode() : base(new Func<Expression, Expression, Expression>(Expression.Divide))
		{
		}
	}
}
