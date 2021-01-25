using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000573 RID: 1395
	[Serializable]
	public class XCurveParam
	{
		// Token: 0x06002299 RID: 8857 RVA: 0x0010F1C0 File Offset: 0x0010D3C0
		public float Evaluate(float time, EffectNode node)
		{
			float num = this.TimeLen;
			if (num < 0f)
			{
				num = node.GetLifeTime();
			}
			float num2 = time / num;
			if (num2 > 1f)
			{
				if (this.WrapType == WRAP_TYPE.CLAMP)
				{
					num2 = 1f;
				}
				else if (this.WrapType == WRAP_TYPE.LOOP)
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
			return this.Curve01.Evaluate(num2) * this.MaxValue;
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0010F264 File Offset: 0x0010D464
		public float Evaluate(float time)
		{
			float num = time / this.TimeLen;
			if (num > 1f)
			{
				if (this.WrapType == WRAP_TYPE.CLAMP)
				{
					num = 1f;
				}
				else if (this.WrapType == WRAP_TYPE.LOOP)
				{
					int num2 = Mathf.FloorToInt(num);
					num -= (float)num2;
				}
				else
				{
					int num3 = Mathf.CeilToInt(num);
					int num4 = Mathf.FloorToInt(num);
					if (num3 % 2 == 0)
					{
						num = (float)num3 - num;
					}
					else
					{
						num -= (float)num4;
					}
				}
			}
			return this.Curve01.Evaluate(num) * this.MaxValue;
		}

		// Token: 0x04002941 RID: 10561
		[SerializeField]
		public float MaxValue = 1f;

		// Token: 0x04002942 RID: 10562
		[SerializeField]
		public float TimeLen = 1f;

		// Token: 0x04002943 RID: 10563
		[SerializeField]
		public WRAP_TYPE WrapType;

		// Token: 0x04002944 RID: 10564
		[SerializeField]
		public AnimationCurve Curve01 = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});
	}
}
