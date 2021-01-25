using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085B RID: 2139
	[USequencerEvent("Animation (Legacy)/Blend Animation No Scrub")]
	[USequencerFriendlyName("Blend Animation No Scrub (Legacy)")]
	public class USBlendAnimNoScrubEvent : USEventBase
	{
		// Token: 0x060033C1 RID: 13249 RVA: 0x000208CF File Offset: 0x0001EACF
		public void Update()
		{
			if (base.Duration < 0f)
			{
				base.Duration = this.blendedAnimation.length;
			}
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0018E2CC File Offset: 0x0018C4CC
		public override void FireEvent()
		{
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (!component)
			{
				Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component[this.blendedAnimation.name].wrapMode = 1;
			component[this.blendedAnimation.name].layer = 1;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000208F2 File Offset: 0x0001EAF2
		public override void ProcessEvent(float deltaTime)
		{
			base.animation.CrossFade(this.blendedAnimation.name);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0018E290 File Offset: 0x0018C490
		public override void StopEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (component)
			{
				component.Stop();
			}
		}

		// Token: 0x04003FE3 RID: 16355
		public AnimationClip blendedAnimation;
	}
}
