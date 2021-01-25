using System;
using System.Collections.Generic;

// Token: 0x020001EE RID: 494
public class EquipProperty : PropertyBase
{
	// Token: 0x060009EE RID: 2542 RVA: 0x00053A14 File Offset: 0x00051C14
	public EquipProperty()
	{
		this._MartialArts = new MartialArts();
		this._MartialDef = new MartialDef();
		this.iConditionList = new List<int>();
		foreach (object obj in Enum.GetValues(typeof(EquipProperty.eDataField)))
		{
			base.Add((CharacterData.PropertyType)((int)obj), 0);
		}
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00053AA8 File Offset: 0x00051CA8
	public void Reset()
	{
		this.iConditionList.Clear();
		foreach (object obj in Enum.GetValues(typeof(EquipProperty.eDataField)))
		{
			this.Set((CharacterData.PropertyType)((int)obj), 0);
		}
		this._MartialArts.Rest();
		this._MartialDef.Rest();
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00053B38 File Offset: 0x00051D38
	public void DirectSetPlus(CharacterData.PropertyType type, int val)
	{
		switch (type)
		{
		case CharacterData.PropertyType.MaxHP:
		case CharacterData.PropertyType.MaxSP:
		case CharacterData.PropertyType.MoveStep:
		case CharacterData.PropertyType.Strength:
		case CharacterData.PropertyType.Constitution:
		case CharacterData.PropertyType.Intelligence:
		case CharacterData.PropertyType.Dexterity:
		case CharacterData.PropertyType.StrengthMax:
		case CharacterData.PropertyType.ConstitutionMax:
		case CharacterData.PropertyType.IntelligenceMax:
		case CharacterData.PropertyType.DexterityMax:
		case CharacterData.PropertyType.Dodge:
		case CharacterData.PropertyType.Counter:
		case CharacterData.PropertyType.Critical:
		case CharacterData.PropertyType.Combo:
		case CharacterData.PropertyType.DefendDodge:
		case CharacterData.PropertyType.DefendCounter:
		case CharacterData.PropertyType.DefendCritical:
		case CharacterData.PropertyType.DefendCombo:
			break;
		default:
			switch (type)
			{
			case CharacterData.PropertyType.DefSwordExp:
			case CharacterData.PropertyType.DefBladeExp:
			case CharacterData.PropertyType.DefArrowExp:
			case CharacterData.PropertyType.DefFistExp:
			case CharacterData.PropertyType.DefGasExp:
			case CharacterData.PropertyType.DefRopeExp:
			case CharacterData.PropertyType.DefWhipExp:
			case CharacterData.PropertyType.DefPikeExp:
				goto IL_14C;
			default:
				if (type != CharacterData.PropertyType.Attack)
				{
					return;
				}
				break;
			}
			break;
		case CharacterData.PropertyType.UseSword:
		case CharacterData.PropertyType.UseBlade:
		case CharacterData.PropertyType.UseArrow:
		case CharacterData.PropertyType.UseFist:
		case CharacterData.PropertyType.UseGas:
		case CharacterData.PropertyType.UseRope:
		case CharacterData.PropertyType.UseWhip:
		case CharacterData.PropertyType.UsePike:
		case CharacterData.PropertyType.UseSwordMax:
		case CharacterData.PropertyType.UseBladeMax:
		case CharacterData.PropertyType.UseArrowMax:
		case CharacterData.PropertyType.UseFistMax:
		case CharacterData.PropertyType.UseGasMax:
		case CharacterData.PropertyType.UseRopeMax:
		case CharacterData.PropertyType.UseWhipMax:
		case CharacterData.PropertyType.UsePikeMax:
			this._MartialArts.SetPlus(type, val);
			return;
		case CharacterData.PropertyType.DefSword:
		case CharacterData.PropertyType.DefBlade:
		case CharacterData.PropertyType.DefArrow:
		case CharacterData.PropertyType.DefFist:
		case CharacterData.PropertyType.DefGas:
		case CharacterData.PropertyType.DefRope:
		case CharacterData.PropertyType.DefWhip:
		case CharacterData.PropertyType.DefPike:
			goto IL_14C;
		}
		this.SetPlus(type, val);
		return;
		IL_14C:
		this._MartialDef.SetPlus(type, val);
	}

	// Token: 0x04000A0A RID: 2570
	public MartialArts _MartialArts;

	// Token: 0x04000A0B RID: 2571
	public MartialDef _MartialDef;

	// Token: 0x04000A0C RID: 2572
	public List<int> iConditionList;

	// Token: 0x020001EF RID: 495
	public enum eDataField
	{
		// Token: 0x04000A0E RID: 2574
		MaxHP = 1,
		// Token: 0x04000A0F RID: 2575
		MaxSP = 3,
		// Token: 0x04000A10 RID: 2576
		Attack = 99,
		// Token: 0x04000A11 RID: 2577
		Critical = 53,
		// Token: 0x04000A12 RID: 2578
		Counter = 52,
		// Token: 0x04000A13 RID: 2579
		Combo = 54,
		// Token: 0x04000A14 RID: 2580
		Dodge = 51,
		// Token: 0x04000A15 RID: 2581
		DefendCritical = 57,
		// Token: 0x04000A16 RID: 2582
		DefendCounter = 56,
		// Token: 0x04000A17 RID: 2583
		DefendCombo = 58,
		// Token: 0x04000A18 RID: 2584
		DefendDodge = 55,
		// Token: 0x04000A19 RID: 2585
		MoveStep = 4,
		// Token: 0x04000A1A RID: 2586
		Strength = 11,
		// Token: 0x04000A1B RID: 2587
		Intelligence = 13,
		// Token: 0x04000A1C RID: 2588
		Dexterity,
		// Token: 0x04000A1D RID: 2589
		Constitution = 12,
		// Token: 0x04000A1E RID: 2590
		StrengthMax = 15,
		// Token: 0x04000A1F RID: 2591
		ConstitutionMax,
		// Token: 0x04000A20 RID: 2592
		IntelligenceMax,
		// Token: 0x04000A21 RID: 2593
		DexterityMax
	}
}
