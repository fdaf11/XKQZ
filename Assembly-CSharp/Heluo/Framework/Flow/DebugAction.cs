using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015A RID: 346
	[Description("執行動作/除錯訊息")]
	public class DebugAction : ActionNode
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x000064D5 File Offset: 0x000046D5
		public override bool GetValue()
		{
			return base.GetValue();
		}

		// Token: 0x04000781 RID: 1921
		[InputField(null, false)]
		public string debugMessage;
	}
}
