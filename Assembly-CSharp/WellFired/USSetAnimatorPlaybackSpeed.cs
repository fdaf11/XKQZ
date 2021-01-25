using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000856 RID: 2134
	[USequencerFriendlyName("Set Playback Speed")]
	[USequencerEvent("Animation (Mecanim)/Animator/Set Playback Speed")]
	public class USSetAnimatorPlaybackSpeed : USEventBase
	{
		// Token: 0x060033A8 RID: 13224 RVA: 0x0018DF10 File Offset: 0x0018C110
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevPlaybackSpeed = component.speed;
			component.speed = this.playbackSpeed;
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float runningTime)
		{
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x0002084A File Offset: 0x0001EA4A
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x0018DF58 File Offset: 0x0018C158
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			component.speed = this.prevPlaybackSpeed;
		}

		// Token: 0x04003FD8 RID: 16344
		public float playbackSpeed = 1f;

		// Token: 0x04003FD9 RID: 16345
		private float prevPlaybackSpeed;
	}
}
