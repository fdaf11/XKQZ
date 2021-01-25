using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000551 RID: 1361
	public class RotateAffector : Affector
	{
		// Token: 0x06002262 RID: 8802 RVA: 0x00016F0F File Offset: 0x0001510F
		public RotateAffector(RSTYPE type, EffectNode node) : base(node, AFFECTORTYPE.RotateAffector)
		{
			this.RType = type;
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00016F27 File Offset: 0x00015127
		public RotateAffector(float delta, EffectNode node) : base(node, AFFECTORTYPE.RotateAffector)
		{
			this.RType = RSTYPE.SIMPLE;
			this.Delta = delta;
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00016F46 File Offset: 0x00015146
		public override void Reset()
		{
			this.IsFirst = true;
			this.Node.RotateAngle = 0f;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x0010D170 File Offset: 0x0010B370
		public override void Update(float deltaTime)
		{
			if (this.IsFirst)
			{
				if (this.RType == RSTYPE.RANDOM)
				{
					this.Delta = Random.Range(this.Node.Owner.RotateSpeedMin, this.Node.Owner.RotateSpeedMax);
				}
				else
				{
					this.Delta = this.Node.Owner.DeltaRot;
				}
				this.IsFirst = false;
			}
			float elapsedTime = this.Node.GetElapsedTime();
			if (this.RType == RSTYPE.CURVE)
			{
				this.Node.RotateAngle = (float)((int)this.Node.Owner.RotateCurve.Evaluate(elapsedTime));
			}
			else if (this.RType == RSTYPE.SIMPLE)
			{
				float rotateAngle = this.Node.RotateAngle + this.Delta * deltaTime;
				this.Node.RotateAngle = rotateAngle;
			}
			else if (this.RType == RSTYPE.RANDOM)
			{
				this.Node.RotateAngle = this.Node.RotateAngle + this.Delta * deltaTime;
			}
			else
			{
				float num = this.Node.Owner.RotateCurveTime;
				if (num < 0f)
				{
					num = this.Node.GetLifeTime();
				}
				float num2 = elapsedTime / num;
				if (num2 > 1f)
				{
					if (this.Node.Owner.RotateCurveWrap == WRAP_TYPE.CLAMP)
					{
						num2 = 1f;
					}
					else if (this.Node.Owner.RotateCurveWrap == WRAP_TYPE.LOOP)
					{
						int num3 = Mathf.FloorToInt(num2);
						num2 -= (float)num3;
					}
					else
					{
						int num4 = Mathf.CeilToInt(num2);
						int num5 = Mathf.FloorToInt(num2);
						if (num4 % 2 == 0)
						{
							num2 = (float)num4 - num2;
						}
						else
						{
							num2 -= (float)num5;
						}
					}
				}
				this.Node.RotateAngle = (float)((int)(this.Node.Owner.RotateCurve01.Evaluate(num2) * this.Node.Owner.RotateCurveMaxValue));
			}
		}

		// Token: 0x040028AC RID: 10412
		protected RSTYPE RType;

		// Token: 0x040028AD RID: 10413
		protected float Delta;

		// Token: 0x040028AE RID: 10414
		protected bool IsFirst = true;
	}
}
