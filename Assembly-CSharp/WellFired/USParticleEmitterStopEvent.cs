using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200086E RID: 2158
	[USequencerEvent("Particle System/Stop Emitter")]
	[USequencerFriendlyName("Stop Emitter (Legacy)")]
	public class USParticleEmitterStopEvent : USEventBase
	{
		// Token: 0x06003421 RID: 13345 RVA: 0x0018F780 File Offset: 0x0018D980
		public override void FireEvent()
		{
			ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
			if (!component)
			{
				Debug.Log("Attempting to emit particles, but the object has no particleSystem USParticleEmitterStartEvent::FireEvent");
				return;
			}
			component.Stop();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}
	}
}
