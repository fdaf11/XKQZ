using System;
using PigeonCoopToolkit.Utillities;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public class PitchShifter : MonoBehaviour
{
	// Token: 0x060024B3 RID: 9395 RVA: 0x0001854A File Offset: 0x0001674A
	private void Start()
	{
		this.src.pitch = Random.Range(this.pitchRange.Min, this.pitchRange.Max);
	}

	// Token: 0x04002C7B RID: 11387
	public Range pitchRange;

	// Token: 0x04002C7C RID: 11388
	public AudioSource src;
}
