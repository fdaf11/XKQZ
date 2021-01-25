using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200055D RID: 1373
	public class ColorAffector : Affector
	{
		// Token: 0x0600227F RID: 8831 RVA: 0x00017081 File Offset: 0x00015281
		public ColorAffector(EffectLayer owner, EffectNode node) : base(node, AFFECTORTYPE.ColorAffector)
		{
			this.mOwner = owner;
			if (owner.ColorGradualTimeLength < 0f)
			{
				this.IsNodeLife = true;
			}
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000170A9 File Offset: 0x000152A9
		public void Fade(float time)
		{
			this.mFade = true;
			this.ElapsedTime = 0f;
			this.mFadeStartColor = this.Node.Color;
			this.mFadingTime = time;
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x0010E164 File Offset: 0x0010C364
		private void UpdateFading()
		{
			float num = this.ElapsedTime / this.mFadingTime;
			this.Node.Color = Color.Lerp(this.mFadeStartColor, Color.clear, num);
			if (num >= 1f)
			{
				this.Node.Stop();
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0010E1B4 File Offset: 0x0010C3B4
		public override void Reset()
		{
			this.mFade = false;
			this.ElapsedTime = 0f;
			if (this.IsNodeLife && this.mOwner.IsNodeLifeLoop && !this.mOwner.mStopped)
			{
				Debug.LogWarning("invalid color gradual time, loop node can't be gradient by 'gradual time':" + this.mOwner.ColorGradualTimeLength + this.mOwner.Owner.name);
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x0010E230 File Offset: 0x0010C430
		public override void Update(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.mFade)
			{
				this.UpdateFading();
				return;
			}
			float num = this.mOwner.ColorGradualTimeLength;
			if (this.IsNodeLife)
			{
				num = this.Node.GetLifeTime();
			}
			if (num <= 0f)
			{
				return;
			}
			float num2;
			if (this.mOwner.ColorGradualType == COLOR_GRADUAL_TYPE.CURVE)
			{
				num2 = this.mOwner.ColorGradualCurve.Evaluate(this.ElapsedTime);
			}
			else
			{
				num2 = this.ElapsedTime / num;
			}
			if (num2 > 1f)
			{
				if (this.mOwner.ColorGradualType == COLOR_GRADUAL_TYPE.CLAMP)
				{
					this.Node.Color = this.Node.StartColor * this.mOwner.ColorGradient.Evaluate(1f);
					return;
				}
				if (this.mOwner.ColorGradualType == COLOR_GRADUAL_TYPE.LOOP)
				{
					this.ElapsedTime = 0f;
					return;
				}
				if (this.mOwner.ColorGradualType == COLOR_GRADUAL_TYPE.REVERSE)
				{
					int num3 = Mathf.CeilToInt(num2);
					int num4 = Mathf.FloorToInt(num2);
					if (num3 % 2 == 0)
					{
						num2 = (float)num3 - num2;
					}
					else
					{
						num2 -= (float)num4;
					}
				}
			}
			this.Node.Color = this.Node.StartColor * this.mOwner.ColorGradient.Evaluate(num2);
		}

		// Token: 0x040028DB RID: 10459
		protected float ElapsedTime;

		// Token: 0x040028DC RID: 10460
		protected bool IsNodeLife;

		// Token: 0x040028DD RID: 10461
		protected EffectLayer mOwner;

		// Token: 0x040028DE RID: 10462
		protected bool mFade;

		// Token: 0x040028DF RID: 10463
		protected float mFadingTime;

		// Token: 0x040028E0 RID: 10464
		protected Color mFadeStartColor;
	}
}
