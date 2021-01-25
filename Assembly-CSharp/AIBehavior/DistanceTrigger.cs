using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200003F RID: 63
	public abstract class DistanceTrigger : BaseTrigger
	{
		// Token: 0x0600011F RID: 287 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0002464C File Offset: 0x0002284C
		protected override bool Evaluate(AIBehaviors fsm)
		{
			Transform[] transforms = this.objectFinder.GetTransforms();
			bool result = false;
			if (transforms.Length > 0)
			{
				Vector3 position = fsm.transform.position;
				float sqrThreshold = this.distanceThreshold * this.distanceThreshold;
				DistanceNegotiation distanceNegotiation = this.GetNegotiationMode();
				for (int i = 0; i < transforms.Length; i++)
				{
					if (this.Compare((transforms[i].position - position).sqrMagnitude, sqrThreshold))
					{
						if (distanceNegotiation == DistanceNegotiation.Any)
						{
							return true;
						}
						result = true;
					}
					else if (distanceNegotiation == DistanceNegotiation.All)
					{
						return false;
					}
				}
			}
			else
			{
				result = this.ResultForNoTaggedObjectsFound();
			}
			return result;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003041 File Offset: 0x00001241
		public DistanceNegotiation GetNegotiationMode()
		{
			if (this.negotiationMode == DistanceNegotiation.Default)
			{
				return this.GetDefaultNegotiationMode();
			}
			return this.negotiationMode;
		}

		// Token: 0x06000122 RID: 290
		protected abstract bool ResultForNoTaggedObjectsFound();

		// Token: 0x06000123 RID: 291
		protected abstract DistanceNegotiation GetDefaultNegotiationMode();

		// Token: 0x06000124 RID: 292
		protected abstract bool Compare(float sqrMagnitude, float sqrThreshold);

		// Token: 0x040000F5 RID: 245
		public float distanceThreshold = 1f;

		// Token: 0x040000F6 RID: 246
		public DistanceNegotiation negotiationMode;
	}
}
