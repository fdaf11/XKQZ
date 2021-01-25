using System;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015F RID: 351
	[Description("流程控制/If Else")]
	public class IfElseAction : ActionNode
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0004866C File Offset: 0x0004686C
		public override bool GetValue()
		{
			if (this.conditionNode == null)
			{
				return false;
			}
			this.result = this.conditionNode.GetValue();
			if (this.result)
			{
				return this.trueNode != null && this.trueNode.GetValue();
			}
			return this.falseNode != null && this.falseNode.GetValue();
		}

		// Token: 0x0400078B RID: 1931
		[Argument("條件")]
		public OutputNode<bool> conditionNode;

		// Token: 0x0400078C RID: 1932
		[Argument("成立")]
		public ActionNode trueNode;

		// Token: 0x0400078D RID: 1933
		[Argument("不成立")]
		public ActionNode falseNode;

		// Token: 0x0400078E RID: 1934
		[JsonIgnore]
		private bool result;
	}
}
