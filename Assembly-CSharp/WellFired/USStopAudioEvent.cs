using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000864 RID: 2148
	[USequencerEvent("Audio/Stop Audio")]
	[USequencerFriendlyName("Stop Audio")]
	public class USStopAudioEvent : USEventBase
	{
		// Token: 0x060033EF RID: 13295 RVA: 0x0018ECE4 File Offset: 0x0018CEE4
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				Debug.Log("USSequencer is trying to play an audio clip, but you didn't give it Audio To Play from USPlayAudioEvent::FireEvent");
				return;
			}
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPlayAudioEvent::FireEvent");
				return;
			}
			component.Stop();
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
