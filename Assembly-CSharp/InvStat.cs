using System;

// Token: 0x02000428 RID: 1064
[Serializable]
public class InvStat
{
	// Token: 0x060019D9 RID: 6617 RVA: 0x00010D58 File Offset: 0x0000EF58
	public static string GetName(InvStat.Identifier i)
	{
		return i.ToString();
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000CFCA4 File Offset: 0x000CDEA4
	public static string GetDescription(InvStat.Identifier i)
	{
		switch (i)
		{
		case InvStat.Identifier.Strength:
			return "Strength increases melee damage";
		case InvStat.Identifier.Constitution:
			return "Constitution increases health";
		case InvStat.Identifier.Agility:
			return "Agility increases armor";
		case InvStat.Identifier.Intelligence:
			return "Intelligence increases mana";
		case InvStat.Identifier.Damage:
			return "Damage adds to the amount of damage done in combat";
		case InvStat.Identifier.Crit:
			return "Crit increases the chance of landing a critical strike";
		case InvStat.Identifier.Armor:
			return "Armor protects from damage";
		case InvStat.Identifier.Health:
			return "Health prolongs life";
		case InvStat.Identifier.Mana:
			return "Mana increases the number of spells that can be cast";
		default:
			return null;
		}
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x000CFD1C File Offset: 0x000CDF1C
	public static int CompareArmor(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Armor)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Damage)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000CFDF0 File Offset: 0x000CDFF0
	public static int CompareWeapon(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Damage)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Armor)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x04001E93 RID: 7827
	public InvStat.Identifier id;

	// Token: 0x04001E94 RID: 7828
	public InvStat.Modifier modifier;

	// Token: 0x04001E95 RID: 7829
	public int amount;

	// Token: 0x02000429 RID: 1065
	public enum Identifier
	{
		// Token: 0x04001E97 RID: 7831
		Strength,
		// Token: 0x04001E98 RID: 7832
		Constitution,
		// Token: 0x04001E99 RID: 7833
		Agility,
		// Token: 0x04001E9A RID: 7834
		Intelligence,
		// Token: 0x04001E9B RID: 7835
		Damage,
		// Token: 0x04001E9C RID: 7836
		Crit,
		// Token: 0x04001E9D RID: 7837
		Armor,
		// Token: 0x04001E9E RID: 7838
		Health,
		// Token: 0x04001E9F RID: 7839
		Mana,
		// Token: 0x04001EA0 RID: 7840
		Other
	}

	// Token: 0x0200042A RID: 1066
	public enum Modifier
	{
		// Token: 0x04001EA2 RID: 7842
		Added,
		// Token: 0x04001EA3 RID: 7843
		Percent
	}
}
