using System;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000008 RID: 8
	public class ExampleCallbacks : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00021E88 File Offset: 0x00020088
		private void OnEnable()
		{
			AIBehaviors component = base.GetComponent<AIBehaviors>();
			AIBehaviors aibehaviors = component;
			aibehaviors.onStateChanged = (AIBehaviors.StateChangedDelegate)Delegate.Combine(aibehaviors.onStateChanged, new AIBehaviors.StateChangedDelegate(this.OnStateChanged));
			AIBehaviors aibehaviors2 = component;
			aibehaviors2.onPlayAnimation = (AIBehaviors.AnimationCallbackDelegate)Delegate.Combine(aibehaviors2.onPlayAnimation, new AIBehaviors.AnimationCallbackDelegate(this.OnPlayAnimation));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00021EE0 File Offset: 0x000200E0
		private void OnDisable()
		{
			AIBehaviors component = base.GetComponent<AIBehaviors>();
			AIBehaviors aibehaviors = component;
			aibehaviors.onStateChanged = (AIBehaviors.StateChangedDelegate)Delegate.Remove(aibehaviors.onStateChanged, new AIBehaviors.StateChangedDelegate(this.OnStateChanged));
			AIBehaviors aibehaviors2 = component;
			aibehaviors2.onPlayAnimation = (AIBehaviors.AnimationCallbackDelegate)Delegate.Remove(aibehaviors2.onPlayAnimation, new AIBehaviors.AnimationCallbackDelegate(this.OnPlayAnimation));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002598 File Offset: 0x00000798
		private void OnStateChanged(BaseState newState, BaseState previousState)
		{
			Debug.Log("Changed from " + previousState.name + " to " + newState.name);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025BA File Offset: 0x000007BA
		private void OnPlayAnimation(AIAnimationState animationState)
		{
			Debug.Log("Play " + animationState.name);
		}
	}
}
