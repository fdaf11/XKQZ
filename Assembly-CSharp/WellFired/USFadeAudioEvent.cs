using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000861 RID: 2145
	[USequencerEvent("Audio/Fade Audio")]
	[USequencerFriendlyName("Fade Audio")]
	public class USFadeAudioEvent : USEventBase
	{
		// Token: 0x060033DC RID: 13276 RVA: 0x00020A1B File Offset: 0x0001EC1B
		public void Update()
		{
			base.Duration = (float)this.fadeCurve.length;
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x0018E960 File Offset: 0x0018CB60
		public override void FireEvent()
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			this.previousVolume = component.volume;
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x0018E99C File Offset: 0x0018CB9C
		public override void ProcessEvent(float deltaTime)
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			component.volume = this.fadeCurve.Evaluate(deltaTime);
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x00020A2F File Offset: 0x0001EC2F
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0018E9E0 File Offset: 0x0018CBE0
		public override void UndoEvent()
		{
			AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
			if (!component)
			{
				Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
				return;
			}
			component.volume = this.previousVolume;
		}

		// Token: 0x04003FF6 RID: 16374
		private float previousVolume = 1f;

		// Token: 0x04003FF7 RID: 16375
		public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});
	}
}
