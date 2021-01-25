using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085A RID: 2138
	[USequencerFriendlyName("Blend Animation (Legacy)")]
	[USequencerEvent("Animation (Legacy)/Blend Animation")]
	public class USBlendAnimEvent : USEventBase
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x000208AA File Offset: 0x0001EAAA
		public void Update()
		{
			if (base.Duration < 0f)
			{
				base.Duration = 2f;
			}
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0018E100 File Offset: 0x0018C300
		public override void FireEvent()
		{
			Animation component = base.AffectedObject.GetComponent<Animation>();
			if (!component)
			{
				Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
				return;
			}
			component.wrapMode = 2;
			component.Play(this.animationClipSource.name);
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x0018E148 File Offset: 0x0018C348
		public override void ProcessEvent(float deltaTime)
		{
			Animation animation = base.AffectedObject.GetComponent<Animation>();
			if (!animation)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Trying to play an animation : ",
					this.animationClipSource.name,
					" but : ",
					base.AffectedObject,
					" doesn't have an animation component, we will add one, this time, though you should add it manually"
				}));
				animation = base.AffectedObject.AddComponent<Animation>();
			}
			if (animation[this.animationClipSource.name] == null)
			{
				Debug.LogError("Trying to play an animation : " + this.animationClipSource.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClipSource, this.animationClipSource.name);
			}
			if (animation[this.animationClipDest.name] == null)
			{
				Debug.LogError("Trying to play an animation : " + this.animationClipDest.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
				animation.AddClip(this.animationClipDest, this.animationClipDest.name);
			}
			if (deltaTime < this.blendPoint)
			{
				animation.CrossFade(this.animationClipSource.name);
			}
			else
			{
				animation.CrossFade(this.animationClipDest.name);
			}
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x0018E290 File Offset: 0x0018C490
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

		// Token: 0x04003FE0 RID: 16352
		public AnimationClip animationClipSource;

		// Token: 0x04003FE1 RID: 16353
		public AnimationClip animationClipDest;

		// Token: 0x04003FE2 RID: 16354
		public float blendPoint = 1f;
	}
}
