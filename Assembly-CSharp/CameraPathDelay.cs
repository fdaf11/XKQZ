using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class CameraPathDelay : CameraPathPoint
{
	// Token: 0x040001D3 RID: 467
	public float time;

	// Token: 0x040001D4 RID: 468
	public float introStartEasePercentage = 0.1f;

	// Token: 0x040001D5 RID: 469
	public AnimationCurve introCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	// Token: 0x040001D6 RID: 470
	public float outroEndEasePercentage = 0.1f;

	// Token: 0x040001D7 RID: 471
	public AnimationCurve outroCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
}
