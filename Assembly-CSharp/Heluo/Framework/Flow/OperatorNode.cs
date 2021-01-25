using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000170 RID: 368
	public abstract class OperatorNode<T> : OutputNode<T>
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x00048CE0 File Offset: 0x00046EE0
		public OperatorNode(Func<Expression, Expression, Expression> expr)
		{
			ParameterExpression parameterExpression;
			ParameterExpression parameterExpression2;
			this.func = (Expression.Lambda(expr.Invoke(parameterExpression, parameterExpression2), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile() as Func<T, T, T>);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00048D48 File Offset: 0x00046F48
		public override T GetValue()
		{
			if (this.func != null)
			{
				try
				{
					return this.func.Invoke(this.leftNode.GetValue(), this.rightNode.GetValue());
				}
				catch (Exception)
				{
					return default(T);
				}
			}
			return default(T);
		}

		// Token: 0x040007C0 RID: 1984
		[Argument("運算元 A")]
		public OutputNode<T> leftNode;

		// Token: 0x040007C1 RID: 1985
		[Argument("運算元 B")]
		public OutputNode<T> rightNode;

		// Token: 0x040007C2 RID: 1986
		[JsonIgnore]
		private Func<T, T, T> func;
	}
}
