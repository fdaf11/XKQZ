using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015E RID: 350
	[Description("流程控制/For 迴圈")]
	public class ForLoopAction : ActionNode
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x00048610 File Offset: 0x00046810
		public override bool GetValue()
		{
			if (this.countNode == null || this.actionNode == null)
			{
				return false;
			}
			for (int i = 0; i < this.countNode.GetValue(); i++)
			{
				this.counterNode.value = i;
				this.actionNode.GetValue();
			}
			return true;
		}

		// Token: 0x04000788 RID: 1928
		[Argument("執行次數")]
		public OutputNode<int> countNode;

		// Token: 0x04000789 RID: 1929
		[Argument("<計數器")]
		public ValueNode<int> counterNode;

		// Token: 0x0400078A RID: 1930
		[Argument("動作")]
		public ActionNode actionNode;
	}
}
