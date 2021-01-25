using System;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000161 RID: 353
	[Description("流程控制/Switch")]
	public class SwitchAction : ActionNode
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x000486D4 File Offset: 0x000468D4
		public override bool GetValue()
		{
			if (this.indexNode == null)
			{
				return false;
			}
			int value = this.indexNode.GetValue();
			if (value < this.actionNodeList.Count && value >= 0)
			{
				ActionNode actionNode = this.actionNodeList[value];
				if (actionNode != null)
				{
					return actionNode.GetValue();
				}
			}
			return false;
		}

		// Token: 0x04000791 RID: 1937
		[Argument("選項")]
		public OutputNode<int> indexNode;

		// Token: 0x04000792 RID: 1938
		[Argument("動作")]
		public List<ActionNode> actionNodeList = new List<ActionNode>();
	}
}
