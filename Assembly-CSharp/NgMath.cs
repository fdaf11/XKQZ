using System;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class NgMath
{
	// Token: 0x06001682 RID: 5762 RVA: 0x000BB4C8 File Offset: 0x000B96C8
	public static NgMath.EasingFunction GetEasingFunction(NgMath.EaseType easeType)
	{
		switch (easeType)
		{
		case NgMath.EaseType.linear:
			return new NgMath.EasingFunction(NgMath.linear);
		case NgMath.EaseType.spring:
			return new NgMath.EasingFunction(NgMath.spring);
		case NgMath.EaseType.easeInQuad:
			return new NgMath.EasingFunction(NgMath.easeInQuad);
		case NgMath.EaseType.easeInCubic:
			return new NgMath.EasingFunction(NgMath.easeInCubic);
		case NgMath.EaseType.easeInQuart:
			return new NgMath.EasingFunction(NgMath.easeInQuart);
		case NgMath.EaseType.easeInQuint:
			return new NgMath.EasingFunction(NgMath.easeInQuint);
		case NgMath.EaseType.easeInSine:
			return new NgMath.EasingFunction(NgMath.easeInSine);
		case NgMath.EaseType.easeInExpo:
			return new NgMath.EasingFunction(NgMath.easeInExpo);
		case NgMath.EaseType.easeInCirc:
			return new NgMath.EasingFunction(NgMath.easeInCirc);
		case NgMath.EaseType.easeInBack:
			return new NgMath.EasingFunction(NgMath.easeInBack);
		case NgMath.EaseType.easeInElastic:
			return new NgMath.EasingFunction(NgMath.easeInElastic);
		case NgMath.EaseType.easeInBounce:
			return new NgMath.EasingFunction(NgMath.easeInBounce);
		case NgMath.EaseType.easeOutQuad:
			return new NgMath.EasingFunction(NgMath.easeOutQuad);
		case NgMath.EaseType.easeOutCubic:
			return new NgMath.EasingFunction(NgMath.easeOutCubic);
		case NgMath.EaseType.easeOutQuart:
			return new NgMath.EasingFunction(NgMath.easeOutQuart);
		case NgMath.EaseType.easeOutQuint:
			return new NgMath.EasingFunction(NgMath.easeOutQuint);
		case NgMath.EaseType.easeOutSine:
			return new NgMath.EasingFunction(NgMath.easeOutSine);
		case NgMath.EaseType.easeOutExpo:
			return new NgMath.EasingFunction(NgMath.easeOutExpo);
		case NgMath.EaseType.easeOutCirc:
			return new NgMath.EasingFunction(NgMath.easeOutCirc);
		case NgMath.EaseType.easeOutBack:
			return new NgMath.EasingFunction(NgMath.easeOutBack);
		case NgMath.EaseType.easeOutElastic:
			return new NgMath.EasingFunction(NgMath.easeOutElastic);
		case NgMath.EaseType.easeOutBounce:
			return new NgMath.EasingFunction(NgMath.easeOutBounce);
		case NgMath.EaseType.easeInOutQuad:
			return new NgMath.EasingFunction(NgMath.easeInOutQuad);
		case NgMath.EaseType.easeInOutCubic:
			return new NgMath.EasingFunction(NgMath.easeInOutCubic);
		case NgMath.EaseType.easeInOutQuart:
			return new NgMath.EasingFunction(NgMath.easeInOutQuart);
		case NgMath.EaseType.easeInOutQuint:
			return new NgMath.EasingFunction(NgMath.easeInOutQuint);
		case NgMath.EaseType.easeInOutSine:
			return new NgMath.EasingFunction(NgMath.easeInOutSine);
		case NgMath.EaseType.easeInOutExpo:
			return new NgMath.EasingFunction(NgMath.easeInOutExpo);
		case NgMath.EaseType.easeInOutCirc:
			return new NgMath.EasingFunction(NgMath.easeInOutCirc);
		case NgMath.EaseType.easeInOutBounce:
			return new NgMath.EasingFunction(NgMath.easeInOutBounce);
		case NgMath.EaseType.easeInOutBack:
			return new NgMath.EasingFunction(NgMath.easeInOutBack);
		case NgMath.EaseType.easeInOutElastic:
			return new NgMath.EasingFunction(NgMath.easeInOutElastic);
		}
		return null;
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0000E747 File Offset: 0x0000C947
	public static float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x000BB70C File Offset: 0x000B990C
	public static float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) * 0.5f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x000BB784 File Offset: 0x000B9984
	public static float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x0000E751 File Offset: 0x0000C951
	public static float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x0000E75F File Offset: 0x0000C95F
	public static float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x000BB7E8 File Offset: 0x000B99E8
	public static float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return -end * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0000E774 File Offset: 0x0000C974
	public static float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0000E784 File Offset: 0x0000C984
	public static float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x000BB840 File Offset: 0x000B9A40
	public static float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0000E7A3 File Offset: 0x0000C9A3
	public static float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0000E7B5 File Offset: 0x0000C9B5
	public static float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x000BB894 File Offset: 0x000B9A94
	public static float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return -end * 0.5f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0000E7D7 File Offset: 0x0000C9D7
	public static float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0000E7EB File Offset: 0x0000C9EB
	public static float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x000BB8F0 File Offset: 0x000B9AF0
	public static float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0000E80E File Offset: 0x0000CA0E
	public static float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value * 1.5707964f) + end + start;
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0000E828 File Offset: 0x0000CA28
	public static float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * 1.5707964f) + start;
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0000E83F File Offset: 0x0000CA3F
	public static float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(3.1415927f * value) - 1f) + start;
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0000E863 File Offset: 0x0000CA63
	public static float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0000E885 File Offset: 0x0000CA85
	public static float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000BB94C File Offset: 0x000B9B4C
	public static float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
	public static float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
	public static float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x000BB9C0 File Offset: 0x000B9BC0
	public static float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x000BBA30 File Offset: 0x000B9C30
	public static float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - NgMath.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x000BBA5C File Offset: 0x000B9C5C
	public static float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x000BBB04 File Offset: 0x000B9D04
	public static float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return NgMath.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return NgMath.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x000BBB68 File Offset: 0x000B9D68
	public static float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x000BBB9C File Offset: 0x000B9D9C
	public static float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x000BBBD8 File Offset: 0x000B9DD8
	public static float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x000BBC58 File Offset: 0x000B9E58
	public static float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x000BBCD0 File Offset: 0x000B9ED0
	public static float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x000BBD88 File Offset: 0x000B9F88
	public static float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x000BBE38 File Offset: 0x000BA038
	public static float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x020003B9 RID: 953
	public enum EaseType
	{
		// Token: 0x04001AD2 RID: 6866
		None,
		// Token: 0x04001AD3 RID: 6867
		linear,
		// Token: 0x04001AD4 RID: 6868
		spring,
		// Token: 0x04001AD5 RID: 6869
		punch,
		// Token: 0x04001AD6 RID: 6870
		easeInQuad,
		// Token: 0x04001AD7 RID: 6871
		easeInCubic,
		// Token: 0x04001AD8 RID: 6872
		easeInQuart,
		// Token: 0x04001AD9 RID: 6873
		easeInQuint,
		// Token: 0x04001ADA RID: 6874
		easeInSine,
		// Token: 0x04001ADB RID: 6875
		easeInExpo,
		// Token: 0x04001ADC RID: 6876
		easeInCirc,
		// Token: 0x04001ADD RID: 6877
		easeInBack,
		// Token: 0x04001ADE RID: 6878
		easeInElastic,
		// Token: 0x04001ADF RID: 6879
		easeInBounce,
		// Token: 0x04001AE0 RID: 6880
		easeOutQuad,
		// Token: 0x04001AE1 RID: 6881
		easeOutCubic,
		// Token: 0x04001AE2 RID: 6882
		easeOutQuart,
		// Token: 0x04001AE3 RID: 6883
		easeOutQuint,
		// Token: 0x04001AE4 RID: 6884
		easeOutSine,
		// Token: 0x04001AE5 RID: 6885
		easeOutExpo,
		// Token: 0x04001AE6 RID: 6886
		easeOutCirc,
		// Token: 0x04001AE7 RID: 6887
		easeOutBack,
		// Token: 0x04001AE8 RID: 6888
		easeOutElastic,
		// Token: 0x04001AE9 RID: 6889
		easeOutBounce,
		// Token: 0x04001AEA RID: 6890
		easeInOutQuad,
		// Token: 0x04001AEB RID: 6891
		easeInOutCubic,
		// Token: 0x04001AEC RID: 6892
		easeInOutQuart,
		// Token: 0x04001AED RID: 6893
		easeInOutQuint,
		// Token: 0x04001AEE RID: 6894
		easeInOutSine,
		// Token: 0x04001AEF RID: 6895
		easeInOutExpo,
		// Token: 0x04001AF0 RID: 6896
		easeInOutCirc,
		// Token: 0x04001AF1 RID: 6897
		easeInOutBounce,
		// Token: 0x04001AF2 RID: 6898
		easeInOutBack,
		// Token: 0x04001AF3 RID: 6899
		easeInOutElastic
	}

	// Token: 0x020003BA RID: 954
	// (Invoke) Token: 0x060016A6 RID: 5798
	public delegate float EasingFunction(float start, float end, float Value);
}
