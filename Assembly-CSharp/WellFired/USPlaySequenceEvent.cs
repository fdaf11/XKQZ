using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000875 RID: 2165
	[USequencerFriendlyName("Play uSequence")]
	[USequencerEvent("Sequence/Play uSequence")]
	public class USPlaySequenceEvent : USEventBase
	{
		// Token: 0x06003441 RID: 13377 RVA: 0x0018FC74 File Offset: 0x0018DE74
		public override void FireEvent()
		{
			if (!this.sequence)
			{
				Debug.LogWarning("No sequence for USPlaySequenceEvent : " + base.name, this);
				return;
			}
			if (!Application.isPlaying)
			{
				Debug.LogWarning("Sequence playback controls are not supported in the editor, but will work in game, just fine.");
				return;
			}
			if (!this.restartSequencer)
			{
				this.sequence.Play();
			}
			else
			{
				this.sequence.RunningTime = 0f;
				this.sequence.Play();
			}
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x0400402B RID: 16427
		public USSequencer sequence;

		// Token: 0x0400402C RID: 16428
		public bool restartSequencer;
	}
}
