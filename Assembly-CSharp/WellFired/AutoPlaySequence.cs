using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000850 RID: 2128
	public class AutoPlaySequence : MonoBehaviour
	{
		// Token: 0x0600338F RID: 13199 RVA: 0x0002079A File Offset: 0x0001E99A
		private void Start()
		{
			if (!this.sequence)
			{
				Debug.LogError("You have added an AutoPlaySequence, however you haven't assigned it a sequence", base.gameObject);
				return;
			}
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0018DB04 File Offset: 0x0018BD04
		private void Update()
		{
			if (this.hasPlayed)
			{
				return;
			}
			this.currentTime += Time.deltaTime;
			if (this.currentTime >= this.delay && this.sequence)
			{
				this.sequence.Play();
				this.hasPlayed = true;
			}
		}

		// Token: 0x04003FC0 RID: 16320
		public USSequencer sequence;

		// Token: 0x04003FC1 RID: 16321
		public float delay = 1f;

		// Token: 0x04003FC2 RID: 16322
		private float currentTime;

		// Token: 0x04003FC3 RID: 16323
		private bool hasPlayed;
	}
}
