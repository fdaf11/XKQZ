using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200072E RID: 1838
public class AbilityManagerTB : MonoBehaviour
{
	// Token: 0x06002B73 RID: 11123 RVA: 0x001534AC File Offset: 0x001516AC
	public static void LoadUnitAbility()
	{
		GameObject gameObject = Resources.Load("PrefabList/UnitAbilityListPrefab", typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("load unit ability fail, make sure the resource file exists");
			return;
		}
		UnitAbilityListPrefab component = gameObject.GetComponent<UnitAbilityListPrefab>();
		AbilityManagerTB.unitAbilityList.AddRange(component.unitAbilityList.ToArray());
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x00153508 File Offset: 0x00151708
	public static void LoadBattleAbility2()
	{
		List<RoutineNewDataNode> routineList = Game.RoutineNewData.GetRoutineList();
		foreach (RoutineNewDataNode routineNewDataNode in routineList)
		{
			UnitAbility unitAbility = new UnitAbility();
			unitAbility.ID = routineNewDataNode.m_iRoutineID;
			unitAbility.name = routineNewDataNode.m_strRoutineName;
			unitAbility.requireTargetSelection = routineNewDataNode.m_bNeedToSelectTarget;
			unitAbility.effectType = (_EffectType)routineNewDataNode.m_iSkillType;
			unitAbility.targetType = (_AbilityTargetType)routineNewDataNode.m_iTargetType;
			unitAbility.targetArea = (_TargetArea)routineNewDataNode.m_iTargetArea;
			unitAbility.range = routineNewDataNode.m_iRange;
			unitAbility.aoeRange = routineNewDataNode.m_iAOE;
			unitAbility.damageMin = (float)routineNewDataNode.m_iDamage;
			unitAbility.damageMax = (float)routineNewDataNode.m_iDamage;
			unitAbility.cost = routineNewDataNode.m_iRequestSP;
			unitAbility.totalCost = routineNewDataNode.m_iRequestSP;
			unitAbility.cdDuration = routineNewDataNode.m_iCD;
			if (routineNewDataNode.m_iSkillEffectIDList.Count > 0)
			{
				unitAbility.skillID = routineNewDataNode.m_iSkillEffectIDList[0];
				unitAbility.skillIDList.AddRange(routineNewDataNode.m_iSkillEffectIDList.ToArray());
			}
			unitAbility.iconName = routineNewDataNode.m_strSkillIconName;
			if (routineNewDataNode.m_iConditionIDList.Count > 0)
			{
				unitAbility.chainedAbilityIDList.AddRange(routineNewDataNode.m_iConditionIDList.ToArray());
			}
			AbilityManagerTB.unitAbilityList.Add(unitAbility);
		}
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x00153688 File Offset: 0x00151888
	public static void LoadBattleAbility()
	{
		foreach (AbilityNode abilityNode in Game.g_BattleControl.m_battleAbility.m_AbilityNodeList)
		{
			UnitAbility unitAbility = new UnitAbility();
			unitAbility.ID = abilityNode.m_iAbilityID;
			unitAbility.name = abilityNode.m_strName;
			unitAbility.requireTargetSelection = abilityNode.m_bNeedToSelectTarget;
			unitAbility.effectType = (_EffectType)abilityNode.m_iSkillType;
			unitAbility.targetType = (_AbilityTargetType)abilityNode.m_iTargetType;
			unitAbility.targetArea = (_TargetArea)abilityNode.m_iTargetArea;
			unitAbility.range = abilityNode.m_iRange;
			unitAbility.aoeRange = abilityNode.m_iAOE;
			unitAbility.damageMin = (float)abilityNode.m_iValue1;
			unitAbility.damageMax = (float)abilityNode.m_iValue2;
			unitAbility.cost = abilityNode.m_iRequestSP;
			unitAbility.totalCost = abilityNode.m_iRequestSP;
			unitAbility.cdDuration = abilityNode.m_iCD;
			unitAbility.enableMovementAfter = abilityNode.m_bUseSkillAfterMove;
			if (abilityNode.m_iSkillEffectIDList.Count > 0)
			{
				unitAbility.skillID = abilityNode.m_iSkillEffectIDList[0];
				unitAbility.skillIDList.AddRange(abilityNode.m_iSkillEffectIDList.ToArray());
			}
			unitAbility.iconName = abilityNode.m_strSkillIconName;
			if (abilityNode.m_iConditionIDList.Count > 0)
			{
				unitAbility.chainedAbilityIDList.AddRange(abilityNode.m_iConditionIDList.ToArray());
			}
			AbilityManagerTB.unitAbilityList.Add(unitAbility);
		}
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x00153818 File Offset: 0x00151A18
	public static UnitAbility GetUnitAbility(int ID)
	{
		foreach (UnitAbility unitAbility in AbilityManagerTB.unitAbilityList)
		{
			if (unitAbility.ID == ID)
			{
				return unitAbility.Clone();
			}
		}
		return null;
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04003825 RID: 14373
	public static List<UnitAbility> unitAbilityList = new List<UnitAbility>();
}
