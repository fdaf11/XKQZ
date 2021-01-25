using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000862 RID: 2146
	[USequencerEvent("Audio/Pause Or Resume Audio")]
	[USequencerFriendlyName("Pause Or Resume Audio")]
	public class USPauseResumeAudioEvent : USEventBase
	{
		// Token: 0x060033E2 RID: 13282 RVA: 0x0018EA1C File Offset: 0x0018CC1C
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				Debug.Log("USSequencer is trying to play an audio clip, but you didn't give it Audio To Play from USPauseAudioEvent::FireEvent");
				return;
			}
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPauseAudioEvent::FireEvent");
				return;
			}
			if (this.pause)
			{
				component.Pause();
			}
			if (!this.pause)
			{
				component.Play();
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0018EA88 File Offset: 0x0018CC88
		public override void ProcessEvent(float deltaTime)
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.Log("USSequencer is trying to play an audio source, but the GameObject doesn't contain an AudioClip from USPauseAudioEvent::FireEvent");
				return;
			}
			if (component.isPlaying)
			{
				return;
			}
		}

		// Token: 0x04003FF8 RID: 16376
		public bool pause = true;
	}
}
