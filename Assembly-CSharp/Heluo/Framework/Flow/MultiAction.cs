using System;
using System.Collections.Generic;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015B RID: 347
	[Description("執行動作/複數動作")]
	public class MultiAction : ActionNode
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x00048550 File Offset: 0x00046750
		public override bool GetValue()
		{
			foreach (ActionNode actionNode in this.actionListNode)
			{
				actionNode.GetValue();
			}
			return base.GetValue();
		}

		// Token: 0x04000782 RID: 1922
		[Argument("動作")]
		public List<ActionNode> actionListNode = new List<ActionNode>();
	}
}
