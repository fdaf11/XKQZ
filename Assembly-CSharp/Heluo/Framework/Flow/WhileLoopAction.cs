using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000160 RID: 352
	[Description("流程控制/While 迴圈")]
	public class WhileLoopAction : ActionNode
	{
		// Token: 0x06000743 RID: 1859 RVA: 0x00006538 File Offset: 0x00004738
		public override bool GetValue()
		{
			if (this.conditionNode == null)
			{
				return false;
			}
			if (this.actionNode == null)
			{
				return false;
			}
			while (this.conditionNode.GetValue())
			{
				this.actionNode.GetValue();
			}
			return true;
		}

		// Token: 0x0400078F RID: 1935
		[Argument]
		public OutputNode<bool> conditionNode;

		// Token: 0x04000790 RID: 1936
		[Argument]
		public ActionNode actionNode;
	}
}
