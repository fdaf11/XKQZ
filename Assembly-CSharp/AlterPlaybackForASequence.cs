using System;
using UnityEngine;
using WellFired;

// Token: 0x0200084C RID: 2124
public class AlterPlaybackForASequence : MonoBehaviour
{
	// Token: 0x06003387 RID: 13191 RVA: 0x0018D9DC File Offset: 0x0018BBDC
	private void Update()
	{
		this.runningTime += Time.deltaTime;
		if (!this.sequence || this.runningTime > 5f)
		{
			return;
		}
		this.sequence.PlaybackRate -= Time.deltaTime * 1f;
	}

	// Token: 0x04003FBB RID: 16315
	public USSequencer sequence;

	// Token: 0x04003FBC RID: 16316
	private float runningTime;
}
