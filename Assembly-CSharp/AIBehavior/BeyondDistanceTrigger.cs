using System;

namespace AIBehavior
{
	// Token: 0x0200003D RID: 61
	public class BeyondDistanceTrigger : DistanceTrigger
	{
		// Token: 0x06000118 RID: 280 RVA: 0x0000301D File Offset: 0x0000121D
		protected override bool Compare(float sqrMagnitude, float sqrThreshold)
		{
			return sqrMagnitude > sqrThreshold;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool ResultForNoTaggedObjectsFound()
		{
			return true;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003023 File Offset: 0x00001223
		protected override DistanceNegotiation GetDefaultNegotiationMode()
		{
			return DistanceNegotiation.All;
		}
	}
}
