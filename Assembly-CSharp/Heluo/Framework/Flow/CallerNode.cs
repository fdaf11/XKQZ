using System;
using System.Reflection;
using Newtonsoft.Json;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200015C RID: 348
	[Description("呼叫")]
	public class CallerNode : ActionNode
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x000064F0 File Offset: 0x000046F0
		public CallerNode()
		{
			this.info = typeof(IOutput).GetMethod("GetValue");
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00006512 File Offset: 0x00004712
		public override bool GetValue()
		{
			if (this.info != null)
			{
				this.info.Invoke(this.callNode, null);
			}
			return base.GetValue();
		}

		// Token: 0x04000783 RID: 1923
		[Argument("呼叫", "單純呼叫用，可以塞任何東西...")]
		public OutputNode callNode;

		// Token: 0x04000784 RID: 1924
		[JsonIgnore]
		private MethodInfo info;
	}
}
