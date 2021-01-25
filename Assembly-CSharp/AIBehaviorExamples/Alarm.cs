using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000002 RID: 2
	public class Alarm : MonoBehaviour
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000248C File Offset: 0x0000068C
		public void OnGetHelp()
		{
			base.audio.PlayOneShot(this.alarmSound);
		}

		// Token: 0x04000001 RID: 1
		public AudioClip alarmSound;
	}
}
