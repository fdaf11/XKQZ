using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000879 RID: 2169
	[USequencerFriendlyName("Send Message")]
	[USequencerEvent("Signal/Send Message")]
	public class USSendMessageEvent : USEventBase
	{
		// Token: 0x0600344F RID: 13391 RVA: 0x0018FE54 File Offset: 0x0018E054
		public override void FireEvent()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.receiver)
			{
				this.receiver.SendMessage(this.action);
			}
			else
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"No receiver of signal \"",
					this.action,
					"\" on object ",
					this.receiver.name,
					" (",
					this.receiver.GetType().Name,
					")"
				}), this.receiver);
			}
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04004034 RID: 16436
		public GameObject receiver;

		// Token: 0x04004035 RID: 16437
		public string action = "OnSignal";
	}
}
