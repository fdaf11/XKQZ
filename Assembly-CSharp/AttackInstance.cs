using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000780 RID: 1920
public class AttackInstance
{
	// Token: 0x06002DEF RID: 11759 RVA: 0x00160F64 File Offset: 0x0015F164
	private bool ProcessHitOrMiss()
	{
		float abilityToTargetHitRate = this.srcUnit.GetAbilityToTargetHitRate(this.targetUnit, this.unitAbility, this.srcUnit.occupiedTile);
		return Random.Range(0f, 1f) <= abilityToTargetHitRate;
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x00160FAC File Offset: 0x0015F1AC
	private bool ProcessCritical()
	{
		if (this.srcUnit.bStealth)
		{
			return true;
		}
		float abilityToTargetCriticalRate = this.srcUnit.GetAbilityToTargetCriticalRate(this.targetUnit, this.unitAbility, this.srcUnit.occupiedTile);
		return Random.Range(0f, 1f) <= abilityToTargetCriticalRate;
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x00161008 File Offset: 0x0015F208
	public bool ProcessCounterAttack()
	{
		if (this.protect || this.knockback)
		{
			return false;
		}
		float abilityToTargetCounterRate = this.targetUnit.GetAbilityToTargetCounterRate(this.srcUnit, this.unitAbility);
		return Random.Range(0f, 1f) <= abilityToTargetCounterRate;
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x00161060 File Offset: 0x0015F260
	private void ProcessHeal(UnitTB unit)
	{
		this.plusDamage = 0;
		this.targetUnit = unit;
		int num = GridManager.Distance(this.srcUnit.occupiedTile, this.targetUnit.occupiedTile);
		if (!this.srcUnit.bCharge && !this.srcUnit.bSpeicalAction)
		{
			Tile tile = this.srcUnit.CheckChargeTile(this.targetUnit, this.unitAbility);
			if (tile != null)
			{
				this.srcUnit.bCharge = true;
				this.srcUnit.bSpeicalAction = true;
				this.srcUnit.tChargeToTile = tile;
				tile.bUnitOrder = true;
			}
		}
		this.moveToTile = this.srcUnit.CheckKnockBackTile(this.targetUnit, this.unitAbility);
		if (this.moveToTile != null && !this.targetUnit.bSpeicalAction)
		{
			this.knockback = true;
			this.targetUnit.bSpeicalAction = true;
			this.moveToTile.bUnitOrder = true;
		}
		else
		{
			this.moveToTile = this.srcUnit.CheckPullCloseTile(this.targetUnit, this.unitAbility);
			if (this.moveToTile != null && !this.targetUnit.bSpeicalAction)
			{
				this.pullclose = true;
				this.targetUnit.bSpeicalAction = true;
				this.moveToTile.bUnitOrder = true;
			}
		}
		int routineLv = this.srcUnit.GetRoutineLv(this.unitAbility.ID);
		if (this.srcUnit.GetUnitAbilityAbsoluteBuff(_EffectPartType.Cleanup, this.unitAbility))
		{
			this.cleanDebuffCount = 99;
		}
		else if (this.srcUnit.GetUnitAbilityValue(_EffectPartType.Cleanup, this.unitAbility, 0, true) >= 1f)
		{
			this.cleanDebuffCount = Mathf.RoundToInt(this.srcUnit.GetUnitAbilityValue(_EffectPartType.Cleanup, this.unitAbility, 0, true));
		}
		float num2 = 0.1f * this.unitAbility.damageMin;
		float num3 = num2 * (float)this.srcUnit.GetAbilityCost(this.unitAbility, true);
		float num4 = num3;
		if (this.outofAP)
		{
			num4 *= 0.1f;
		}
		float num5 = this.srcUnit.GetEffectPartValue(_EffectPartType.Damage, this.unitAbility, num);
		if (this.srcUnit.occupiedTile != null)
		{
			num5 += this.srcUnit.occupiedTile.GetEffectPartValue(_EffectPartType.Damage, this.srcUnit);
		}
		float num6 = this.srcUnit.GetEffectPartValuePercent(_EffectPartType.Damage, this.unitAbility, num);
		num6 += this.srcUnit.occupiedTile.GetEffectPartValuePercent(_EffectPartType.Damage, this.srcUnit);
		num6 *= 0.01f;
		num6 += 1f;
		if (!GameGlobal.m_bDLCMode)
		{
			num6 += 0.1f * (float)routineLv;
		}
		num6 += this.srcUnit.GetTalentDamage(this.unitAbility) + this.srcUnit.occupiedTile.GetTalentDamagePercent(this.srcUnit);
		num6 += this.fTalentSkillRate;
		if (GameGlobal.m_iBattleDifficulty > 8)
		{
			num6 = Mathf.Clamp(num6, 0.75f, 32f);
		}
		else if (GameGlobal.m_iBattleDifficulty > 5)
		{
			num6 = Mathf.Clamp(num6, 0.5f, 32f);
		}
		else
		{
			num6 = Mathf.Clamp(num6, 0.1f, 32f);
		}
		num4 *= num6;
		num4 += num5;
		float abilityCriticalRate = this.srcUnit.GetAbilityCriticalRate(this.unitAbility, num, this.srcUnit.occupiedTile);
		if (Random.Range(0f, 1f) <= abilityCriticalRate)
		{
			float num7 = 1.5f;
			num4 *= num7;
			this.critical = true;
		}
		float num8 = this.srcUnit.GetUnitAbilityValue(_EffectPartType.HitPoint, this.unitAbility, num, false);
		num4 += num8;
		num8 = 0.01f * (float)this.targetUnit.fullHP * this.srcUnit.GetUnitAbilityValuePercent(_EffectPartType.HitPoint, this.unitAbility, num, false);
		num4 += num8;
		if (this.unitAbility.bItemSkill)
		{
			float num9 = 1f + this.srcUnit.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, true);
			num4 *= num9;
		}
		this.damageDone = Mathf.RoundToInt(num4);
		this.heal = true;
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x001614A0 File Offset: 0x0015F6A0
	public void Process(UnitTB unit)
	{
		if (this.unitAbility == null)
		{
			return;
		}
		if (this.unitAbility.effectType == _EffectType.Heal || this.unitAbility.effectType == _EffectType.Buff)
		{
			this.ProcessHeal(unit);
			return;
		}
		this.srcUnit.CheckTalentPosition(unit, this.unitAbility, this.srcUnit.occupiedTile, true);
		this.plusDamage = 0;
		this.targetUnit = unit;
		int iRange = GridManager.Distance(this.srcUnit.occupiedTile, this.targetUnit.occupiedTile);
		if (this.srcUnit.GetUnitAbilityAbsoluteBuff(_EffectPartType.Capture, this.unitAbility))
		{
			if (this.targetUnit.ProcessBeCapture())
			{
				this.captureTarget = true;
				return;
			}
			this.missed = true;
			this.counterattack = true;
			return;
		}
		else
		{
			if (this.type == _AttackType.Skill_Counter)
			{
				this.isCounterAttack = true;
			}
			if (!this.srcUnit.bCharge && !this.srcUnit.bSpeicalAction)
			{
				Tile tile = this.srcUnit.CheckChargeTile(this.targetUnit, this.unitAbility);
				if (tile != null)
				{
					this.srcUnit.bCharge = true;
					this.srcUnit.bSpeicalAction = true;
					this.srcUnit.tChargeToTile = tile;
					tile.bUnitOrder = true;
				}
			}
			if (!this.ProcessHitOrMiss())
			{
				this.missed = true;
				return;
			}
			bool flag = false;
			int routineLv = this.srcUnit.GetRoutineLv(this.unitAbility.ID);
			float num = 0.1f * this.unitAbility.damageMin;
			this.protect = this.targetUnit.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.Protect, this.targetUnit);
			UnitTB unitTB;
			if (this.protect && this.type != _AttackType.Skill_AssistAttack)
			{
				this.protectUnit = this.targetUnit.occupiedTile.GetEffectPartUnit(_EffectPartType.Protect);
				if (this.protectUnit != null && !this.protectUnit.bSpeicalAction && !this.protectUnit.bBeTarget)
				{
					this.protectUnit.bSpeicalAction = true;
					unitTB = this.protectUnit;
				}
				else
				{
					this.protectUnit = null;
					this.protect = false;
					unitTB = this.targetUnit;
				}
			}
			else
			{
				this.protectUnit = null;
				this.protect = false;
				unitTB = this.targetUnit;
			}
			this.srcUnit.ClearnupSelfOrTargetCondition(this.unitAbility, unitTB);
			float num2 = this.srcUnit.GetEffectPartValue(_EffectPartType.Damage, this.unitAbility, iRange);
			if (this.srcUnit.occupiedTile != null)
			{
				num2 += this.srcUnit.occupiedTile.GetEffectPartValue(_EffectPartType.Damage, this.srcUnit);
			}
			if (num2 != 0f)
			{
				flag = true;
			}
			float num3 = this.srcUnit.GetEffectPartValuePercent(_EffectPartType.Damage, this.unitAbility, iRange);
			num3 += this.srcUnit.occupiedTile.GetEffectPartValuePercent(_EffectPartType.Damage, this.srcUnit);
			num3 *= 0.01f;
			num3 += 1f;
			if (!GameGlobal.m_bDLCMode)
			{
				num3 += 0.1f * (float)routineLv;
			}
			num3 += this.srcUnit.GetTalentDamage(this.unitAbility) + this.srcUnit.occupiedTile.GetTalentDamagePercent(this.srcUnit);
			num3 += this.fTalentSkillRate;
			if (num3 != 1f)
			{
				if (GameGlobal.m_iBattleDifficulty > 8)
				{
					num3 = Mathf.Clamp(num3, 0.75f, 32f);
				}
				else if (GameGlobal.m_iBattleDifficulty > 5)
				{
					num3 = Mathf.Clamp(num3, 0.5f, 32f);
				}
				else
				{
					num3 = Mathf.Clamp(num3, 0.1f, 32f);
				}
				flag = true;
			}
			if (!this.protect && this.type != _AttackType.Skill_AssistAttack)
			{
				this.moveToTile = this.srcUnit.CheckKnockBackTile(this.targetUnit, this.unitAbility);
				if (this.moveToTile != null && !this.targetUnit.bSpeicalAction)
				{
					this.knockback = true;
					this.targetUnit.bSpeicalAction = true;
					this.moveToTile.bUnitOrder = true;
				}
				else
				{
					this.moveToTile = this.srcUnit.CheckPullCloseTile(this.targetUnit, this.unitAbility);
					if (this.moveToTile != null && !this.targetUnit.bSpeicalAction)
					{
						this.pullclose = true;
						this.targetUnit.bSpeicalAction = true;
						this.moveToTile.bUnitOrder = true;
					}
				}
			}
			float num4 = num * (float)this.srcUnit.GetAbilityCost(this.unitAbility, true);
			if (this.outofAP)
			{
				num4 *= 0.1f;
			}
			num4 *= num3;
			float num5 = num4 * (num3 - 1f);
			float num6 = (float)this.srcUnit.GetRoutineMartialAttack(this.unitAbility.ID);
			float num7 = unitTB.GetDamageReduc((float)unitTB.GetRoutineMartialDef(this.unitAbility.ID));
			if (this.srcUnit.GetUnitTileAbsoluteDebuff(_EffectPartType.DamageReduction, this.unitAbility))
			{
				if (GameGlobal.m_bDLCMode)
				{
					if (num7 > 0f)
					{
						num7 = 0f;
					}
				}
				else
				{
					num7 = 0f;
				}
			}
			float num8;
			if (GameGlobal.m_bDLCMode)
			{
				num7 = 100f - num7;
				if (num7 < 5f)
				{
					num7 = 5f;
				}
				num8 = num4 * num7 * 0.01f;
			}
			else if (num6 + num7 > 0f)
			{
				float num9 = (num6 + num6) / (num6 + num7);
				num8 = num4 * num9;
			}
			else
			{
				num8 = 1f;
			}
			int num10 = this.srcUnit.characterData._TotalProperty.Get(CharacterData.PropertyType.Attack);
			num8 += (float)num10 + num2;
			float num11 = (float)num10 + num2;
			if (this.ProcessCritical())
			{
				float num12 = 1.5f;
				num8 *= num12;
				num11 *= num12;
				num5 *= num12;
				this.critical = true;
			}
			float num13 = unitTB.fDamageReduc;
			float num14 = 1f - unitTB.GetEffectPartValuePercent(_EffectPartType.RealDmgReduc, null, 0) * 0.01f;
			if (unitTB.occupiedTile != null)
			{
				num14 -= 0.01f * unitTB.occupiedTile.GetEffectPartValuePercent(_EffectPartType.RealDmgReduc, unitTB);
			}
			num13 = Mathf.Clamp01(num13);
			num14 = Mathf.Clamp01(num14);
			num13 *= num14;
			num8 *= num13;
			num11 *= num13;
			num5 *= num13;
			float num15 = this.srcUnit.GetUnitAbilityValue(_EffectPartType.HitPoint, this.unitAbility, iRange, false);
			num8 -= num15;
			num15 = 0.01f * (float)unitTB.fullHP * this.srcUnit.GetUnitAbilityValuePercent(_EffectPartType.HitPoint, this.unitAbility, iRange, false);
			num8 -= num15;
			if (this.unitAbility.bItemSkill)
			{
				float num16 = 1f + this.srcUnit.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, true);
				num8 *= num16;
				num11 *= num16;
				num5 *= num16;
			}
			int num17 = Mathf.RoundToInt(num8);
			if (flag)
			{
				this.plusDamage = Mathf.RoundToInt(num11 + num5);
			}
			this.damageDone = num17;
			if (this.srcUnit.factionID == 0)
			{
				this.damageDone = (int)((float)this.damageDone / (0.5f + (float)GameGlobal.mod_Difficulty * 0.5f));
			}
			else
			{
				this.damageDone = (int)((float)this.damageDone * (0.5f + (float)GameGlobal.mod_Difficulty * 0.5f));
			}
			this.counterattack = this.ProcessCounterAttack();
			if (!GameGlobal.m_bDLCMode)
			{
				float num18;
				if (num6 >= num7)
				{
					num18 = 0.1f * Mathf.Sqrt((float)this.damageDone);
					num18 = 1f + num18;
					num18 *= num6;
				}
				else
				{
					num18 = num7 - num6;
					num18 = Mathf.Clamp(num18, 1f, 10f);
					num18 = num6 / num18;
				}
				this.iAbilityType = this.srcUnit.GetRoutineMartialType(this.unitAbility.ID);
				this.iDefPlusLv = unitTB.characterData._MartialDef.PlusExp(this.iAbilityType + (CharacterData.PropertyType)40, Mathf.RoundToInt(num18));
			}
			if (!this.protect && (this.type == _AttackType.Skill_Normal || this.type == _AttackType.Skill_DoubleAttack))
			{
				this.moveFlower = this.targetUnit.Calculate4Measurements(CharacterData.PropertyType.Dexterity, ref this.targetUnit.iRemoveFlowerValue);
			}
			if (this.moveFlower)
			{
				this.missed = true;
				this.counterattack = true;
			}
			else
			{
				this.damageAbsorbToSP = unitTB.Calculate4Measurements(CharacterData.PropertyType.Constitution, ref unitTB.iDamageAbsorbToSPValue);
			}
			if (this.unitAbility != null && (this.unitAbility.effectType == _EffectType.Damage || this.unitAbility.effectType == _EffectType.Debuff) && this.targetUnit != null && this.targetUnit != this.srcUnit)
			{
				this.targetUnit.RotateToUnit(this.srcUnit);
			}
			return;
		}
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x00161CEC File Offset: 0x0015FEEC
	public AttackInstance Clone()
	{
		AttackInstance attackInstance = new AttackInstance();
		attackInstance.type = this.type;
		if (this.unitAbility != null)
		{
			attackInstance.unitAbility = this.unitAbility.Clone();
		}
		else
		{
			attackInstance.unitAbility = null;
		}
		attackInstance.srcUnit = this.srcUnit;
		attackInstance.targetUnit = this.targetUnit;
		attackInstance.protectUnit = this.protectUnit;
		attackInstance.moveToTile = this.moveToTile;
		attackInstance.charge = this.charge;
		attackInstance.protect = this.protect;
		attackInstance.knockback = this.knockback;
		attackInstance.pullclose = this.pullclose;
		attackInstance.destroyed = this.destroyed;
		attackInstance.missed = this.missed;
		attackInstance.critical = this.critical;
		attackInstance.counterattack = this.counterattack;
		attackInstance.counterAttacking = this.counterAttacking;
		attackInstance.damageDone = this.damageDone;
		attackInstance.outofAP = this.outofAP;
		attackInstance.plusDamage = this.plusDamage;
		attackInstance.heal = this.heal;
		attackInstance.counterAbility = this.counterAbility;
		attackInstance.shock = this.shock;
		attackInstance.fTalentSkillRate = this.fTalentSkillRate;
		return attackInstance;
	}

	// Token: 0x04003A0F RID: 14863
	public _AttackType type;

	// Token: 0x04003A10 RID: 14864
	public bool isCounterAttack;

	// Token: 0x04003A11 RID: 14865
	public UnitAbility unitAbility;

	// Token: 0x04003A12 RID: 14866
	public UnitTB srcUnit;

	// Token: 0x04003A13 RID: 14867
	public UnitTB targetUnit;

	// Token: 0x04003A14 RID: 14868
	public UnitTB protectUnit;

	// Token: 0x04003A15 RID: 14869
	public Tile moveToTile;

	// Token: 0x04003A16 RID: 14870
	public bool charge;

	// Token: 0x04003A17 RID: 14871
	public bool knockback;

	// Token: 0x04003A18 RID: 14872
	public bool pullclose;

	// Token: 0x04003A19 RID: 14873
	public bool protect;

	// Token: 0x04003A1A RID: 14874
	public bool destroyed;

	// Token: 0x04003A1B RID: 14875
	public bool missed;

	// Token: 0x04003A1C RID: 14876
	public bool critical;

	// Token: 0x04003A1D RID: 14877
	public bool counterattack;

	// Token: 0x04003A1E RID: 14878
	public bool counterAttacking;

	// Token: 0x04003A1F RID: 14879
	public bool counterAbility;

	// Token: 0x04003A20 RID: 14880
	public bool captureTarget;

	// Token: 0x04003A21 RID: 14881
	public int damageDone;

	// Token: 0x04003A22 RID: 14882
	public int damageSPDone;

	// Token: 0x04003A23 RID: 14883
	public int cleanDebuffCount;

	// Token: 0x04003A24 RID: 14884
	public bool outofAP;

	// Token: 0x04003A25 RID: 14885
	public int plusDamage;

	// Token: 0x04003A26 RID: 14886
	public bool heal;

	// Token: 0x04003A27 RID: 14887
	public bool shock;

	// Token: 0x04003A28 RID: 14888
	public bool moveFlower;

	// Token: 0x04003A29 RID: 14889
	public bool damageAbsorbToSP;

	// Token: 0x04003A2A RID: 14890
	public int iAbilityType;

	// Token: 0x04003A2B RID: 14891
	public int iDefPlusLv;

	// Token: 0x04003A2C RID: 14892
	public float animationLength;

	// Token: 0x04003A2D RID: 14893
	public float lastEmitHitTime;

	// Token: 0x04003A2E RID: 14894
	public float fTalentSkillRate;

	// Token: 0x04003A2F RID: 14895
	public int totalTarget;
}
