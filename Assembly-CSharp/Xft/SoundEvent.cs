using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000591 RID: 1425
	public class SoundEvent : XftEvent
	{
		// Token: 0x060023BE RID: 9150 RVA: 0x00017CA0 File Offset: 0x00015EA0
		public SoundEvent(XftEventComponent owner) : base(XEventType.Sound, owner)
		{
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00117628 File Offset: 0x00115828
		protected AudioSource PlaySound(AudioClip clip, float volume, float pitch)
		{
			volume *= GlobalConfig.SoundVolume;
			if (clip != null)
			{
				if (SoundEvent.m_Listener == null)
				{
					SoundEvent.m_Listener = (Object.FindObjectOfType(typeof(AudioListener)) as AudioListener);
					if (SoundEvent.m_Listener == null)
					{
						Camera camera = Camera.main;
						if (camera == null)
						{
							camera = (Object.FindObjectOfType(typeof(Camera)) as Camera);
						}
						if (camera != null)
						{
							SoundEvent.m_Listener = camera.gameObject.AddComponent<AudioListener>();
						}
					}
				}
				if (SoundEvent.m_Listener != null)
				{
					AudioSource audioSource = SoundEvent.m_Listener.audio;
					if (audioSource == null)
					{
						audioSource = SoundEvent.m_Listener.gameObject.AddComponent<AudioSource>();
					}
					audioSource.pitch = pitch;
					audioSource.loop = this.m_owner.IsSoundLoop;
					if (!this.m_owner.IsSoundLoop)
					{
						audioSource.PlayOneShot(clip, volume);
					}
					else
					{
						audioSource.clip = clip;
						audioSource.volume = volume;
						audioSource.Play();
					}
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0011774C File Offset: 0x0011594C
		public override void Reset()
		{
			base.Reset();
			if (SoundEvent.m_Listener != null && SoundEvent.m_Listener.audio != null && this.m_owner.IsSoundLoop)
			{
				SoundEvent.m_Listener.audio.Stop();
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x00017CAA File Offset: 0x00015EAA
		public override void OnBegin()
		{
			base.OnBegin();
			this.PlaySound(this.m_owner.Clip, this.m_owner.Volume, this.m_owner.Pitch);
		}

		// Token: 0x04002B18 RID: 11032
		private static AudioListener m_Listener;
	}
}
