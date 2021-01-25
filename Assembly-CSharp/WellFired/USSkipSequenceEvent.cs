using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000877 RID: 2167
	[USequencerFriendlyName("Skip uSequence")]
	[USequencerEvent("Sequence/Skip uSequence")]
	public class USSkipSequenceEvent : USEventBase
	{
		// Token: 0x06003449 RID: 13385 RVA: 0x0018FD5C File Offset: 0x0018DF5C
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				Debug.LogWarning("No sequence for USSkipSequenceEvent : " + base.name, this);
				return;
			}
			if (!this.skipToEnd && this.skipToTime < 0f && this.skipToTime > this.sequence.Duration)
			{
				Debug.LogWarning("You haven't set the properties correctly on the Sequence for this USSkipSequenceEvent, either the skipToTime is invalid, or you haven't flagged it to skip to the end", this);
				return;
			}
			if (this.skipToEnd)
			{
				this.sequence.SkipTimelineTo(this.sequence.Duration);
			}
			else
			{
				this.sequence.SkipTimelineTo(this.skipToTime);
			}
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04004030 RID: 16432
		public USSequencer sequence;

		// Token: 0x04004031 RID: 16433
		public bool skipToEnd = true;

		// Token: 0x04004032 RID: 16434
		public float skipToTime = -1f;
	}
}
