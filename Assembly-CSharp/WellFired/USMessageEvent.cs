using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000865 RID: 2149
	[USequencerEvent("Debug/Log Message")]
	[USequencerFriendlyName("Log Message")]
	public class USMessageEvent : USEventBase
	{
		// Token: 0x060033F2 RID: 13298 RVA: 0x00020A9F File Offset: 0x0001EC9F
		public override void FireEvent()
		{
			Debug.Log(this.message);
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04003FFC RID: 16380
		public string message = "Default Message";
	}
}
