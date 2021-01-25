using System;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class MartialDef : PropertyBase
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x00053DB0 File Offset: 0x00051FB0
	public MartialDef()
	{
		foreach (object obj in Enum.GetValues(typeof(MartialDef.eDataField)))
		{
			base.Add((CharacterData.PropertyType)((int)obj), 500);
		}
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00053E28 File Offset: 0x00052028
	public new bool SetPlus(CharacterData.PropertyType type, int val)
	{
		int num = this.Get(type);
		num += val;
		return this.Set(type, num);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00053E4C File Offset: 0x0005204C
	public override bool Set(CharacterData.PropertyType type, int val)
	{
		int num = val;
		if (type >= CharacterData.PropertyType.DefSword && type <= CharacterData.PropertyType.DefPike)
		{
			if (GameGlobal.m_bDLCMode)
			{
				num = Mathf.Clamp(num, -500, 500);
			}
			else
			{
				num = Mathf.Clamp(num, 0, 500);
			}
		}
		return base.Set(type, num);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00053EA4 File Offset: 0x000520A4
	public void Rest()
	{
		for (int i = 41; i <= 48; i++)
		{
			this.Set((CharacterData.PropertyType)i, 0);
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00053ED0 File Offset: 0x000520D0
	public void getdata(int idx, int ivalue)
	{
		CharacterData.PropertyType type = idx + CharacterData.PropertyType.DefSword;
		this.Set(type, ivalue);
		type = idx + CharacterData.PropertyType.DefSwordExp;
		this.Set(type, 0);
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00053EFC File Offset: 0x000520FC
	public int PlusExp(CharacterData.PropertyType type, int val)
	{
		int num = 0;
		CharacterData.PropertyType type2 = type + 100;
		int num2 = this.Get(type);
		int i = this.Get(type2);
		i += val;
		int num3 = (num2 / 10 + 1) * 100;
		while (i > num3)
		{
			i -= num3;
			num2++;
			num3 = (num2 / 10 + 1) * 100;
			num++;
		}
		this.Set(type, num2);
		this.Set(type2, i);
		return num;
	}

	// Token: 0x020001F3 RID: 499
	public enum eDataField
	{
		// Token: 0x04000A34 RID: 2612
		DefBlade = 42,
		// Token: 0x04000A35 RID: 2613
		DefSword = 41,
		// Token: 0x04000A36 RID: 2614
		DefArrow = 43,
		// Token: 0x04000A37 RID: 2615
		DefFist,
		// Token: 0x04000A38 RID: 2616
		DefGas,
		// Token: 0x04000A39 RID: 2617
		DefRope,
		// Token: 0x04000A3A RID: 2618
		DefWhip,
		// Token: 0x04000A3B RID: 2619
		DefPike,
		// Token: 0x04000A3C RID: 2620
		DefBladeExp = 142,
		// Token: 0x04000A3D RID: 2621
		DefSwordExp = 141,
		// Token: 0x04000A3E RID: 2622
		DefArrowExp = 143,
		// Token: 0x04000A3F RID: 2623
		DefFistExp,
		// Token: 0x04000A40 RID: 2624
		DefGasExp,
		// Token: 0x04000A41 RID: 2625
		DefRopeExp,
		// Token: 0x04000A42 RID: 2626
		DefWhipExp,
		// Token: 0x04000A43 RID: 2627
		DefPikeExp
	}
}
