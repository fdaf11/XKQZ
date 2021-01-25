using System;

namespace Heluo.Wulin
{
	// Token: 0x020000EC RID: 236
	[Serializable]
	public class AnimatorStateChangeEventArg : EventArgs
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000514C File Offset: 0x0000334C
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00005154 File Offset: 0x00003354
		public string FromState { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000515D File Offset: 0x0000335D
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00005165 File Offset: 0x00003365
		public string ToState { get; set; }
	}
}
