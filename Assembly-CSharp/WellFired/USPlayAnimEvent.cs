using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085D RID: 2141
	[USequencerFriendlyName("Play Animation (Legacy)")]
	[USequencerEvent("Animation (Legacy)/Play Animation")]
	public class USPlayAnimEvent : USEventBase
	{
		// Token: 0x060033CC RID: 13260 RVA: 0x00020980 File Offset: 0x0001EB80
		public void Update()
		{
			if (this.wrapMode != 2 && this.animationClip)
			{
				base.Duration = this.animationClip.length / this.playbackSpeed;
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x0018E580 File Offset: 0x0018C780
		public override void FireEvent()
		{
			if (!this.animationClip)
			{
				Debug.Log("Attempting to play an animation on a GameObject but you haven't given the event an animation clip from USPlayAnimEvent::FireEvent");
				return;
			}
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (!component)
			{
				Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component.wrapMode = this.wrapMode;
			component.Play(this.animationClip.name);
			AnimationState animationState = component[this.animationClip.name];
			if (!animationState)
			{
				return;
			}
			animationState.speed = this.playbackSpeed;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x0018E614 File Offset: 0x0018C814
		public override void ProcessEvent(float deltaTime)
		{
			Animation animation = base.AffectedObject.GetComponent<Animation>();
			if (!this.animationClip)
			{
				return;
			}
			if (!animation)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Trying to play an animation : ",
					this.animationClip.name,
					" but : ",
					base.AffectedObject,
					" doesn't have an animation component, we will add one, this time, though you should add it manually"
				}));
				animation = base.AffectedObject.AddComponent<Animation>();
			}
			if (animation[this.animationClip.name] == null)
			{
				Debug.LogError("Trying to play an animation : " + this.animationClip.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClip, this.animationClip.name);
			}
			AnimationState animationState = animation[this.animationClip.name];
			if (!animation.IsPlaying(this.animationClip.name))
			{
				animation.wrapMode = this.wrapMode;
				animation.Play(this.animationClip.name);
			}
			animationState.speed = this.playbackSpeed;
			animationState.time = deltaTime * this.playbackSpeed;
			animationState.enabled = true;
			animation.Sample();
			animationState.enabled = false;
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0018E290 File Offset: 0x0018C490
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

		// Token: 0x04003FEB RID: 16363
		public AnimationClip animationClip;

		// Token: 0x04003FEC RID: 16364
		public WrapMode wrapMode;

		// Token: 0x04003FED RID: 16365
		public float playbackSpeed = 1f;
	}
}
