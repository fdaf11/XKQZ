using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200086D RID: 2157
	[USequencerEvent("Particle System/Start Emitter")]
	[USequencerFriendlyName("Start Emitter (Legacy)")]
	public class USParticleEmitterStartEvent : USEventBase
	{
		// Token: 0x0600341B RID: 13339 RVA: 0x0018F67C File Offset: 0x0018D87C
		public void Update()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
			if (component)
			{
				base.Duration = component.duration + component.startLifetime;
			}
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x0018F6C4 File Offset: 0x0018D8C4
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
			if (!component)
			{
				Debug.Log("Attempting to emit particles, but the object has no particleSystem USParticleEmitterStartEvent::FireEvent");
				return;
			}
			if (!Application.isPlaying)
			{
				return;
			}
			component.Play();
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x0018F718 File Offset: 0x0018D918
		public override void ProcessEvent(float deltaTime)
		{
			if (Application.isPlaying)
			{
				return;
			}
			ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
			component.Simulate(deltaTime);
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x00020C31 File Offset: 0x0001EE31
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x0018F744 File Offset: 0x0018D944
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
			if (component)
			{
				component.Stop();
			}
		}
	}
}
