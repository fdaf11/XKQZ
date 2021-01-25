using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000878 RID: 2168
	[USequencerFriendlyName("stop uSequence")]
	[USequencerEvent("Sequence/Stop uSequence")]
	public class USStopSequenceEvent : USEventBase
	{
		// Token: 0x0600344C RID: 13388 RVA: 0x0018FE04 File Offset: 0x0018E004
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				Debug.LogWarning("No sequence for USstopSequenceEvent : " + base.name, this);
			}
			if (this.sequence)
			{
				this.sequence.Stop();
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04004033 RID: 16435
		public USSequencer sequence;
	}
}
