using System;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class MartialArts : PropertyBase
{
	// Token: 0x060009F1 RID: 2545 RVA: 0x00053CA4 File Offset: 0x00051EA4
	public MartialArts()
	{
		foreach (object obj in Enum.GetValues(typeof(MartialArts.eDataField)))
		{
			base.Add((CharacterData.PropertyType)((int)obj), 500);
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00053D1C File Offset: 0x00051F1C
	public void Rest()
	{
		for (int i = 21; i <= 28; i++)
		{
			this.Set((CharacterData.PropertyType)i, 0);
		}
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00053D48 File Offset: 0x00051F48
	public void getdata(int idx, int ivalue)
	{
		CharacterData.PropertyType type = idx + CharacterData.PropertyType.UseSword;
		CharacterData.PropertyType type2 = idx + CharacterData.PropertyType.UseSwordMax;
		this.Set(type2, 500);
		this.Set(type, ivalue);
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00053D78 File Offset: 0x00051F78
	public override bool Set(CharacterData.PropertyType type, int val)
	{
		int num = val;
		if (type >= CharacterData.PropertyType.UseSwordMax && type <= CharacterData.PropertyType.UsePikeMax)
		{
			num = Mathf.Clamp(num, 0, 999);
		}
		return base.Set(type, num);
	}

	// Token: 0x020001F1 RID: 497
	public enum eDataField
	{
		// Token: 0x04000A23 RID: 2595
		Blade = 22,
		// Token: 0x04000A24 RID: 2596
		Sword = 21,
		// Token: 0x04000A25 RID: 2597
		Arrow = 23,
		// Token: 0x04000A26 RID: 2598
		Fist,
		// Token: 0x04000A27 RID: 2599
		Gas,
		// Token: 0x04000A28 RID: 2600
		Rope,
		// Token: 0x04000A29 RID: 2601
		Whip,
		// Token: 0x04000A2A RID: 2602
		Pike,
		// Token: 0x04000A2B RID: 2603
		BladeMax = 32,
		// Token: 0x04000A2C RID: 2604
		SwordMax = 31,
		// Token: 0x04000A2D RID: 2605
		ArrowMax = 33,
		// Token: 0x04000A2E RID: 2606
		FistMax,
		// Token: 0x04000A2F RID: 2607
		GasMax,
		// Token: 0x04000A30 RID: 2608
		RopeMax,
		// Token: 0x04000A31 RID: 2609
		WhipMax,
		// Token: 0x04000A32 RID: 2610
		PikeMax
	}
}
