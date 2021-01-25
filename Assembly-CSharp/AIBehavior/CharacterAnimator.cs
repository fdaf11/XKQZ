using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000018 RID: 24
	public class CharacterAnimator : MonoBehaviour
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00022D00 File Offset: 0x00020F00
		private void Awake()
		{
			this.anim = base.GetComponentInChildren<Animation>();
			this.hasAnimationComponent = (this.anim != null);
			if (!this.hasAnimationComponent)
			{
				Debug.LogWarning("No animation component found for the '" + base.gameObject.name + "' object or child objects");
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00022D58 File Offset: 0x00020F58
		public void OnAnimationState(AIAnimationState animState)
		{
			if (this.hasAnimationComponent && animState != null)
			{
				string name = animState.name;
				if (this.anim[name] != null)
				{
					this.anim[name].wrapMode = animState.animationWrapMode;
					this.anim[name].speed = animState.speed;
					this.anim.CrossFade(name);
				}
				else
				{
					Debug.LogWarning("The animation state \"" + name + "\" couldn't be found.");
				}
			}
		}

		// Token: 0x0400004E RID: 78
		private Animation anim;

		// Token: 0x0400004F RID: 79
		private bool hasAnimationComponent;
	}
}
