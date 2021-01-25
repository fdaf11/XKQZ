using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200055E RID: 1374
	public class DragAffector : Affector
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x0010E398 File Offset: 0x0010C598
		public DragAffector(Transform dragObj, bool useDir, Vector3 dir, float mag, bool useMaxDist, float maxDist, float atten, EffectNode node) : base(node, AFFECTORTYPE.DragAffector)
		{
			this.DragObj = dragObj;
			this.UseDirection = useDir;
			this.Direction = dir;
			this.Magnitude = mag;
			this.UseMaxDistance = useMaxDist;
			this.MaxDistance = maxDist;
			this.Attenuation = atten;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x0010E3E4 File Offset: 0x0010C5E4
		protected void UpdateNoAttenuationNoDir(float deltaTime)
		{
			float sqrMagnitude = (this.Node.GetOriginalPos() - this.DragObj.position).sqrMagnitude;
			float num = this.Magnitude * deltaTime;
			if (num != 0f && sqrMagnitude <= this.MaxDistance * this.MaxDistance)
			{
				if (num < 1f)
				{
					this.Node.Velocity *= 1f - num;
				}
				else
				{
					this.Node.Velocity = Vector3.zero;
				}
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x0010E47C File Offset: 0x0010C67C
		protected void UpdateNoAttenuationNoDirNoDist(float deltaTime)
		{
			float num = this.Magnitude * deltaTime;
			if (num < 1f)
			{
				this.Node.Velocity *= 1f - num;
			}
			else
			{
				this.Node.Velocity = Vector3.zero;
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x0010E4D0 File Offset: 0x0010C6D0
		public override void Update(float deltaTime)
		{
			if (!this.UseDirection && this.Attenuation == 0f)
			{
				if (this.UseMaxDistance)
				{
					this.UpdateNoAttenuationNoDir(deltaTime);
				}
				else
				{
					this.UpdateNoAttenuationNoDirNoDist(deltaTime);
				}
				return;
			}
			Vector3 vector = Vector3.one;
			if (this.UseDirection && this.Direction != Vector3.zero)
			{
				vector = this.DragObj.rotation * this.Direction;
				vector.Normalize();
			}
			float magnitude = (this.Node.GetOriginalPos() - this.DragObj.position).magnitude;
			if (!this.UseMaxDistance || magnitude <= this.MaxDistance)
			{
				float num = 1f;
				if (this.UseDirection)
				{
					Vector3 velocity = this.Node.Velocity;
					velocity.Normalize();
					num = Vector3.Dot(velocity, vector);
				}
				float num2 = this.Magnitude * deltaTime * num / (1f + magnitude * this.Attenuation);
				if (num2 < 1f)
				{
					this.Node.Velocity -= num2 * this.Node.Velocity;
				}
				else
				{
					this.Node.Velocity = Vector3.zero;
				}
			}
		}

		// Token: 0x040028E1 RID: 10465
		protected Transform DragObj;

		// Token: 0x040028E2 RID: 10466
		protected bool UseDirection;

		// Token: 0x040028E3 RID: 10467
		protected Vector3 Direction;

		// Token: 0x040028E4 RID: 10468
		protected float Magnitude;

		// Token: 0x040028E5 RID: 10469
		protected bool UseMaxDistance;

		// Token: 0x040028E6 RID: 10470
		protected float MaxDistance;

		// Token: 0x040028E7 RID: 10471
		protected float Attenuation;
	}
}
