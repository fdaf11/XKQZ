using System;

namespace AIBehavior
{
	// Token: 0x0200004E RID: 78
	public class WithinDistanceTrigger : DistanceTrigger
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00003210 File Offset: 0x00001410
		protected override bool Compare(float sqrMagnitude, float sqrThreshold)
		{
			return sqrMagnitude < sqrThreshold;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00002C2D File Offset: 0x00000E2D
		protected override bool ResultForNoTaggedObjectsFound()
		{
			return false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override DistanceNegotiation GetDefaultNegotiationMode()
		{
			return DistanceNegotiation.Any;
		}
	}
}
