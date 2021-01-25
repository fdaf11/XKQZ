using System;

// Token: 0x0200068F RID: 1679
[Serializable]
public class FlareOcclusion
{
	// Token: 0x04003380 RID: 13184
	public bool occluded;

	// Token: 0x04003381 RID: 13185
	public float occlusionScale = 1f;

	// Token: 0x04003382 RID: 13186
	public FlareOcclusion.CullingState _CullingState;

	// Token: 0x04003383 RID: 13187
	public float CullTimer;

	// Token: 0x04003384 RID: 13188
	public float cullFader = 1f;

	// Token: 0x02000690 RID: 1680
	public enum CullingState
	{
		// Token: 0x04003386 RID: 13190
		Visible,
		// Token: 0x04003387 RID: 13191
		CullCountDown,
		// Token: 0x04003388 RID: 13192
		CanCull,
		// Token: 0x04003389 RID: 13193
		Culled,
		// Token: 0x0400338A RID: 13194
		NeverCull
	}
}
