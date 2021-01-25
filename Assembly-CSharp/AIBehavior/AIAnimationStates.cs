using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000017 RID: 23
	public class AIAnimationStates : MonoBehaviour
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00022C84 File Offset: 0x00020E84
		public AIAnimationState GetStateWithName(string stateName)
		{
			if (this.statesDictionary.ContainsKey(stateName))
			{
				return this.statesDictionary[stateName];
			}
			for (int i = 0; i < this.states.Length; i++)
			{
				if (this.states[i].name == stateName)
				{
					this.statesDictionary[stateName] = this.states[i];
					return this.states[i];
				}
			}
			return null;
		}

		// Token: 0x0400004A RID: 74
		public AnimationType animationType;

		// Token: 0x0400004B RID: 75
		public AIAnimationState[] states = new AIAnimationState[1];

		// Token: 0x0400004C RID: 76
		public GameObject animationStatesGameObject;

		// Token: 0x0400004D RID: 77
		private Dictionary<string, AIAnimationState> statesDictionary = new Dictionary<string, AIAnimationState>();
	}
}
