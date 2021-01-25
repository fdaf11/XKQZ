using System;
using System.Collections.Generic;

// Token: 0x0200072D RID: 1837
[Serializable]
public class UnitAbility : Effect
{
	// Token: 0x06002B70 RID: 11120 RVA: 0x00153304 File Offset: 0x00151504
	public UnitAbility Clone()
	{
		UnitAbility unitAbility = new UnitAbility();
		unitAbility.ID = this.ID;
		unitAbility.name = this.name;
		unitAbility.desp = this.desp;
		unitAbility.icon = this.icon;
		unitAbility.iconName = this.iconName;
		unitAbility.factionID = this.factionID;
		unitAbility.targetArea = this.targetArea;
		unitAbility.targetType = this.targetType;
		unitAbility.requireTargetSelection = this.requireTargetSelection;
		unitAbility.enableMovementAfter = this.enableMovementAfter;
		unitAbility.enableAttackAfter = this.enableAttackAfter;
		unitAbility.effectType = this.effectType;
		unitAbility.aoeRange = this.aoeRange;
		unitAbility.range = this.range;
		unitAbility.cost = this.cost;
		unitAbility.totalCost = this.totalCost;
		unitAbility.cdDuration = this.cdDuration;
		unitAbility.cooldown = this.cooldown;
		unitAbility.useLimit = this.useLimit;
		unitAbility.useCount = this.useCount;
		unitAbility.damageMin = this.damageMin;
		unitAbility.damageMax = this.damageMax;
		unitAbility.canFail = this.canFail;
		unitAbility.failChance = this.failChance;
		unitAbility.chainedAbilityIDList.AddRange(this.chainedAbilityIDList.ToArray());
		unitAbility.skillID = this.skillID;
		unitAbility.skillIDList.AddRange(this.skillIDList.ToArray());
		unitAbility.minDuration = this.minDuration;
		unitAbility.maxDuration = this.maxDuration;
		unitAbility.removeChance = this.removeChance;
		unitAbility.removeByAttackChance = this.removeByAttackChance;
		unitAbility.bItemSkill = this.bItemSkill;
		return unitAbility;
	}

	// Token: 0x0400380F RID: 14351
	public _AbilityTargetType targetType;

	// Token: 0x04003810 RID: 14352
	public bool requireTargetSelection = true;

	// Token: 0x04003811 RID: 14353
	public bool enableMovementAfter = true;

	// Token: 0x04003812 RID: 14354
	public bool enableAttackAfter;

	// Token: 0x04003813 RID: 14355
	public int factionID;

	// Token: 0x04003814 RID: 14356
	public bool canFail;

	// Token: 0x04003815 RID: 14357
	public float failChance = 0.15f;

	// Token: 0x04003816 RID: 14358
	public List<int> chainedAbilityIDList = new List<int>();

	// Token: 0x04003817 RID: 14359
	public List<int> skillIDList = new List<int>();

	// Token: 0x04003818 RID: 14360
	public _TargetArea targetArea;

	// Token: 0x04003819 RID: 14361
	public int aoeRange = 1;

	// Token: 0x0400381A RID: 14362
	public int range = 3;

	// Token: 0x0400381B RID: 14363
	public int cost = 2;

	// Token: 0x0400381C RID: 14364
	public int totalCost;

	// Token: 0x0400381D RID: 14365
	public int cdDuration = 2;

	// Token: 0x0400381E RID: 14366
	public int cooldown;

	// Token: 0x0400381F RID: 14367
	public int useLimit = -1;

	// Token: 0x04003820 RID: 14368
	public int useCount;

	// Token: 0x04003821 RID: 14369
	public float damageMin;

	// Token: 0x04003822 RID: 14370
	public float damageMax;

	// Token: 0x04003823 RID: 14371
	public int skillID;

	// Token: 0x04003824 RID: 14372
	public bool bItemSkill;
}
