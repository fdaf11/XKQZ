using System;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000009 RID: 9
	public class ExampleDefendScript : MonoBehaviour
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000025D1 File Offset: 0x000007D1
		public void OnStartDefending(DefendState defendState)
		{
			Debug.Log("Start Defending");
			base.GetComponent<AIBehaviors>().damageMultiplier = defendState.defensiveBonus;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025EE File Offset: 0x000007EE
		public void OnStopDefending(DefendState defendState)
		{
			Debug.Log("Stop Defending");
			base.GetComponent<AIBehaviors>().damageMultiplier = 1f;
		}
	}
}
