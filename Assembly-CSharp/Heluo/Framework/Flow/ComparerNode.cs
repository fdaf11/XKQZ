using System;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000165 RID: 357
	[GenericTypeConstraint(TypeConstraint.Primitive)]
	[Description("邏輯比較")]
	public class ComparerNode<T> : OutputNode<bool> where T : IComparable<T>, IEquatable<T>
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x000487CC File Offset: 0x000469CC
		public override bool GetValue()
		{
			if (this.argNode1 == null || this.argNode2 == null)
			{
				return false;
			}
			T value = this.argNode1.GetValue();
			T value2 = this.argNode2.GetValue();
			int num = value.CompareTo(value2);
			return this.f.Invoke(num);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00048824 File Offset: 0x00046A24
		private void OnOperatorChange(Operator op)
		{
			switch (op)
			{
			case Operator.GreaterThen:
				this.f = ((int result) => result > 0);
				break;
			case Operator.Equal:
				this.f = ((int result) => result == 0);
				break;
			case Operator.LessThen:
				this.f = ((int result) => result < 0);
				break;
			case Operator.NotEqual:
				this.f = ((int result) => result != 0);
				break;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00006603 File Offset: 0x00004803
		internal override void OnAfterDeserialize(NodeGraph cond)
		{
			base.OnAfterDeserialize(cond);
			this.OnOperatorChange(this.op);
		}

		// Token: 0x0400079C RID: 1948
		[Argument("參數 1")]
		public OutputNode<T> argNode1;

		// Token: 0x0400079D RID: 1949
		[InputField("運算元", false)]
		public Operator op;

		// Token: 0x0400079E RID: 1950
		[Argument("參數 2")]
		public OutputNode<T> argNode2;

		// Token: 0x0400079F RID: 1951
		[JsonIgnore]
		private Func<int, bool> f = (int result) => result > 0;
	}
}
