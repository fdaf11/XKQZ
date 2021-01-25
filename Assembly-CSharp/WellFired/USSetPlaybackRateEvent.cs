using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000876 RID: 2166
	[USequencerEvent("Sequence/Set Playback Rate")]
	[USequencerFriendlyName("Set uSequence Playback Rate")]
	public class USSetPlaybackRateEvent : USEventBase
	{
		// Token: 0x06003444 RID: 13380 RVA: 0x0018FCF4 File Offset: 0x0018DEF4
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				Debug.LogWarning("No sequence for USSetPlaybackRate : " + base.name, this);
			}
			if (this.sequence)
			{
				this.prevPlaybackRate = this.sequence.PlaybackRate;
				this.sequence.PlaybackRate = this.playbackRate;
			}
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00020CA4 File Offset: 0x0001EEA4
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00020CAC File Offset: 0x0001EEAC
		public override void UndoEvent()
		{
			if (this.sequence)
			{
				this.sequence.PlaybackRate = this.prevPlaybackRate;
			}
		}

		// Token: 0x0400402D RID: 16429
		public USSequencer sequence;

		// Token: 0x0400402E RID: 16430
		public float playbackRate = 1f;

		// Token: 0x0400402F RID: 16431
		private float prevPlaybackRate = 1f;
	}
}
