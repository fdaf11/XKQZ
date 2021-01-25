using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200055A RID: 1370
	public class BombAffector : Affector
	{
		// Token: 0x06002277 RID: 8823 RVA: 0x0010DE3C File Offset: 0x0010C03C
		public BombAffector(Transform obj, BOMBTYPE gtype, BOMBDECAYTYPE dtype, float mag, float decay, Vector3 axis, EffectNode node) : base(node, AFFECTORTYPE.BombAffector)
		{
			this.BombType = gtype;
			this.DecayType = dtype;
			this.Magnitude = mag;
			this.Decay = decay;
			this.BombAxis = axis;
			this.BombAxis.Normalize();
			this.BombObj = obj;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00017021 File Offset: 0x00015221
		public override void Reset()
		{
			this.ElapsedTime = 0f;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0010DE8C File Offset: 0x0010C08C
		public override void Update(float deltaTime)
		{
			deltaTime = 0.01666f;
			float num = this.Magnitude;
			Vector3 vector = this.BombObj.rotation * this.BombAxis;
			Vector3 vector2 = this.Node.GetOriginalPos() - this.BombObj.position;
			float num2 = vector2.magnitude;
			Vector3 vector3 = Vector3.zero;
			if (vector2 == Vector3.zero)
			{
			}
			if (this.DecayType == BOMBDECAYTYPE.None || num2 <= this.Decay)
			{
				switch (this.BombType)
				{
				case BOMBTYPE.Planar:
					num2 = Vector3.Dot(vector, vector2);
					if (num2 < 0f)
					{
						num2 = -num2;
						vector3 = -vector;
					}
					else
					{
						vector3 = vector;
					}
					break;
				case BOMBTYPE.Spherical:
					vector3 = vector2 / num2;
					break;
				case BOMBTYPE.Cylindrical:
					num2 = Vector3.Dot(vector, vector2);
					vector3 = vector2 - num2 * vector;
					num2 = vector3.magnitude;
					if (num2 != 0f)
					{
						vector3 /= num2;
					}
					break;
				default:
					Debug.LogError("wrong bomb type!");
					break;
				}
				float num3 = 1f;
				if (this.DecayType == BOMBDECAYTYPE.Linear)
				{
					num3 = (this.Decay - num2) / this.Decay;
				}
				else if (this.DecayType == BOMBDECAYTYPE.Exponential)
				{
					num3 = Mathf.Exp(-num2 / this.Decay);
				}
				this.ElapsedTime += deltaTime;
				num /= this.ElapsedTime * this.ElapsedTime;
				this.Node.Velocity += num3 * num * deltaTime * vector3;
			}
		}

		// Token: 0x040028D1 RID: 10449
		protected BOMBTYPE BombType;

		// Token: 0x040028D2 RID: 10450
		protected BOMBDECAYTYPE DecayType;

		// Token: 0x040028D3 RID: 10451
		protected float Magnitude;

		// Token: 0x040028D4 RID: 10452
		protected float Decay;

		// Token: 0x040028D5 RID: 10453
		protected Vector3 BombAxis;

		// Token: 0x040028D6 RID: 10454
		protected Transform BombObj;

		// Token: 0x040028D7 RID: 10455
		protected float ElapsedTime;
	}
}
