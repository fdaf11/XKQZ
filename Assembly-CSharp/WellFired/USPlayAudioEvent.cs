using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000863 RID: 2147
	[USequencerEvent("Audio/Play Audio")]
	[USequencerFriendlyName("Play Audio")]
	public class USPlayAudioEvent : USEventBase
	{
		// Token: 0x060033E5 RID: 13285 RVA: 0x00020A46 File Offset: 0x0001EC46
		public void Update()
		{
			if (!this.loop && this.audioClip)
			{
				base.Duration = this.audioClip.length;
			}
			else
			{
				base.Duration = -1f;
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0018EAC4 File Offset: 0x0018CCC4
		public override void FireEvent()
		{
			AudioSource audioSource = base.AffectedObject.GetComponent<AudioSource>();
			if (!audioSource)
			{
				audioSource = base.AffectedObject.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
			}
			if (audioSource.clip != this.audioClip)
			{
				audioSource.clip = this.audioClip;
			}
			audioSource.time = 0f;
			audioSource.loop = this.loop;
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			audioSource.Play();
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0018EB4C File Offset: 0x0018CD4C
		public override void ProcessEvent(float deltaTime)
		{
			AudioSource audioSource = base.AffectedObject.GetComponent<AudioSource>();
			if (!audioSource)
			{
				audioSource = base.AffectedObject.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
			}
			if (audioSource.clip != this.audioClip)
			{
				audioSource.clip = this.audioClip;
			}
			if (audioSource.isPlaying)
			{
				return;
			}
			audioSource.time = deltaTime;
			if (base.Sequence.IsPlaying && !audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0018EBDC File Offset: 0x0018CDDC
		public override void ManuallySetTime(float deltaTime)
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				return;
			}
			component.time = deltaTime;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0018EC08 File Offset: 0x0018CE08
		public override void ResumeEvent()
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				return;
			}
			component.time = base.Sequence.RunningTime - base.FireTime;
			if (this.wasPlaying)
			{
				component.Play();
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0018EC58 File Offset: 0x0018CE58
		public override void PauseEvent()
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			this.wasPlaying = false;
			if (component && component.isPlaying)
			{
				this.wasPlaying = true;
			}
			if (component)
			{
				component.Pause();
			}
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x00020A84 File Offset: 0x0001EC84
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x00020A84 File Offset: 0x0001EC84
		public override void EndEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x0018ECA8 File Offset: 0x0018CEA8
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (component)
			{
				component.Stop();
			}
		}

		// Token: 0x04003FF9 RID: 16377
		public AudioClip audioClip;

		// Token: 0x04003FFA RID: 16378
		public bool loop;

		// Token: 0x04003FFB RID: 16379
		private bool wasPlaying;
	}
}
