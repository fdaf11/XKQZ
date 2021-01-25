using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000874 RID: 2164
	[USequencerEvent("Sequence/Pause uSequence")]
	[USequencerFriendlyName("Pause uSequence")]
	public class USPauseSequenceEvent : USEventBase
	{
		// Token: 0x0600343E RID: 13374 RVA: 0x0018FC24 File Offset: 0x0018DE24
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				Debug.LogWarning("No sequence for USPauseSequenceEvent : " + base.name, this);
			}
			if (this.sequence)
			{
				this.sequence.Pause();
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x0400402A RID: 16426
		public USSequencer sequence;
	}
}
