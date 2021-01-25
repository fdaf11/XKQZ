using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000159 RID: 345
	[Description("執行動作/結束動作")]
	public class FinishNode : ActionNode
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x000064B7 File Offset: 0x000046B7
		public override bool GetValue()
		{
			if (this.timedActionNode != null)
			{
				this.timedActionNode.OnFinish();
			}
			return base.GetValue();
		}

		// Token: 0x04000780 RID: 1920
		[Argument("結束動作")]
		public TimedActionNode timedActionNode;
	}
}
