using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000555 RID: 1365
	public class ScaleAffector : Affector
	{
		// Token: 0x0600226E RID: 8814 RVA: 0x00016F92 File Offset: 0x00015192
		public ScaleAffector(RSTYPE type, EffectNode node) : base(node, AFFECTORTYPE.ScaleAffector)
		{
			this.SType = type;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00016FAA File Offset: 0x000151AA
		public ScaleAffector(float x, float y, EffectNode node) : base(node, AFFECTORTYPE.ScaleAffector)
		{
			this.SType = RSTYPE.SIMPLE;
			this.DeltaX = x;
			this.DeltaY = y;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00016FD0 File Offset: 0x000151D0
		public override void Reset()
		{
			this.IsFirst = true;
			this.Node.Scale = Vector3.one;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0010D63C File Offset: 0x0010B83C
		public override void Update(float deltaTime)
		{
			if (this.IsFirst)
			{
				if (this.SType == RSTYPE.RANDOM)
				{
					this.DeltaX = Random.Range(this.Node.Owner.DeltaScaleX, this.Node.Owner.DeltaScaleXMax);
					this.DeltaY = Random.Range(this.Node.Owner.DeltaScaleY, this.Node.Owner.DeltaScaleYMax);
				}
				else
				{
					this.DeltaX = this.Node.Owner.DeltaScaleX;
					this.DeltaY = this.Node.Owner.DeltaScaleY;
				}
				this.IsFirst = false;
			}
			float elapsedTime = this.Node.GetElapsedTime();
			if (this.SType == RSTYPE.CURVE)
			{
				if (this.Node.Owner.UseSameScaleCurve)
				{
					float num = this.Node.Owner.ScaleXCurve.Evaluate(elapsedTime);
					this.Node.Scale.x = num;
					this.Node.Scale.y = num;
				}
				else
				{
					this.Node.Scale.x = this.Node.Owner.ScaleXCurve.Evaluate(elapsedTime);
					this.Node.Scale.y = this.Node.Owner.ScaleYCurve.Evaluate(elapsedTime);
				}
			}
			else if (this.SType == RSTYPE.RANDOM)
			{
				float num2 = this.Node.Scale.x + this.DeltaX * deltaTime;
				float num3 = this.Node.Scale.y + this.DeltaY * deltaTime;
				if (num2 > 0f)
				{
					this.Node.Scale.x = num2;
				}
				if (num3 > 0f)
				{
					this.Node.Scale.y = num3;
				}
			}
			else if (this.SType == RSTYPE.CURVE01)
			{
				float num4 = this.Node.Owner.ScaleCurveTime;
				if (num4 < 0f)
				{
					num4 = this.Node.GetLifeTime();
				}
				float num5 = elapsedTime / num4;
				if (num5 > 1f)
				{
					if (this.Node.Owner.ScaleWrapMode == WRAP_TYPE.CLAMP)
					{
						num5 = 1f;
					}
					else if (this.Node.Owner.ScaleWrapMode == WRAP_TYPE.LOOP)
					{
						int num6 = Mathf.FloorToInt(num5);
						num5 -= (float)num6;
					}
					else
					{
						int num7 = Mathf.CeilToInt(num5);
						int num8 = Mathf.FloorToInt(num5);
						if (num7 % 2 == 0)
						{
							num5 = (float)num7 - num5;
						}
						else
						{
							num5 -= (float)num8;
						}
					}
				}
				if (this.Node.Owner.UseSameScaleCurve)
				{
					float num9 = this.Node.Owner.ScaleXCurveNew.Evaluate(num5);
					num9 *= this.Node.Owner.MaxScaleCalue;
					this.Node.Scale.x = num9;
					this.Node.Scale.y = num9;
				}
				else
				{
					this.Node.Scale.x = this.Node.Owner.ScaleXCurveNew.Evaluate(num5) * this.Node.Owner.MaxScaleCalue;
					this.Node.Scale.y = this.Node.Owner.ScaleYCurveNew.Evaluate(num5) * this.Node.Owner.MaxScaleValueY;
				}
			}
			else if (this.SType == RSTYPE.SIMPLE)
			{
				float num10 = this.Node.Scale.x + this.DeltaX * deltaTime;
				float num11 = this.Node.Scale.y + this.DeltaY * deltaTime;
				if (num10 * this.Node.Scale.x > 0f)
				{
					this.Node.Scale.x = num10;
				}
				if (num11 * this.Node.Scale.y > 0f)
				{
					this.Node.Scale.y = num11;
				}
			}
		}

		// Token: 0x040028B7 RID: 10423
		protected RSTYPE SType;

		// Token: 0x040028B8 RID: 10424
		protected float DeltaX;

		// Token: 0x040028B9 RID: 10425
		protected float DeltaY;

		// Token: 0x040028BA RID: 10426
		protected bool IsFirst = true;
	}
}
