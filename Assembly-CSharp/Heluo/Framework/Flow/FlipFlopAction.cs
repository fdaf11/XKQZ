using System;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015D RID: 349
	[Description("流程控制/正反器")]
	public class FlipFlopAction : ActionNode
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x000485B0 File Offset: 0x000467B0
		public override bool GetValue()
		{
			if (this.isA)
			{
				if (this.actionANode != null)
				{
					this.actionANode.GetValue();
				}
				this.isA = false;
			}
			else
			{
				if (this.actionBNode != null)
				{
					this.actionBNode.GetValue();
				}
				this.isA = true;
			}
			return base.GetValue();
		}

		// Token: 0x04000785 RID: 1925
		[Argument("動作 1")]
		public ActionNode actionANode;

		// Token: 0x04000786 RID: 1926
		[Argument("動作 2")]
		public ActionNode actionBNode;

		// Token: 0x04000787 RID: 1927
		[JsonIgnore]
		private bool isA;
	}
}
