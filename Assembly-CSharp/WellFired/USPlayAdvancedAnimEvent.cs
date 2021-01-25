using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085C RID: 2140
	[USequencerFriendlyName("Play Advanced Animation (Legacy)")]
	[USequencerEvent("Animation (Legacy)/Play Animation Advanced")]
	public class USPlayAdvancedAnimEvent : USEventBase
	{
		// Token: 0x060033C6 RID: 13254 RVA: 0x0002092F File Offset: 0x0001EB2F
		public void Update()
		{
			if (this.wrapMode != 2 && this.animationClip)
			{
				base.Duration = this.animationClip.length / this.playbackSpeed;
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0018E32C File Offset: 0x0018C52C
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
			if (this.crossFadeAnimation)
			{
				component.CrossFade(this.animationClip.name);
			}
			else
			{
				component.Play(this.animationClip.name);
			}
			AnimationState animationState = component[this.animationClip.name];
			if (!animationState)
			{
				return;
			}
			animationState.enabled = true;
			animationState.weight = this.animationWeight;
			animationState.blendMode = this.blendMode;
			animationState.layer = this.animationLayer;
			animationState.speed = this.playbackSpeed;
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0018E40C File Offset: 0x0018C60C
		public override void ProcessEvent(float deltaTime)
		{
			Animation animation = base.AffectedObject.GetComponent<Animation>();
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
				if (this.crossFadeAnimation)
				{
					animation.CrossFade(this.animationClip.name);
				}
				else
				{
					animation.Play(this.animationClip.name);
				}
			}
			animationState.weight = this.animationWeight;
			animationState.blendMode = this.blendMode;
			animationState.layer = this.animationLayer;
			animationState.speed = this.playbackSpeed;
			animationState.time = deltaTime * this.playbackSpeed;
			animationState.enabled = true;
			animation.Sample();
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0018E290 File Offset: 0x0018C490
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

		// Token: 0x060033CA RID: 13258 RVA: 0x00020965 File Offset: 0x0001EB65
		public override void EndEvent()
		{
			this.StopEvent();
		}

		// Token: 0x04003FE4 RID: 16356
		public AnimationClip animationClip;

		// Token: 0x04003FE5 RID: 16357
		public WrapMode wrapMode;

		// Token: 0x04003FE6 RID: 16358
		public AnimationBlendMode blendMode;

		// Token: 0x04003FE7 RID: 16359
		public float playbackSpeed = 1f;

		// Token: 0x04003FE8 RID: 16360
		public float animationWeight = 1f;

		// Token: 0x04003FE9 RID: 16361
		public int animationLayer = 1;

		// Token: 0x04003FEA RID: 16362
		public bool crossFadeAnimation;
	}
}
