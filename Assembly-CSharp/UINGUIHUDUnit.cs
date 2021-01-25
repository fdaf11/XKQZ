using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
public class UINGUIHUDUnit : MonoBehaviour
{
	// Token: 0x06003151 RID: 12625 RVA: 0x0017D464 File Offset: 0x0017B664
	private void Awake()
	{
		this.m_currentUnit = null;
		for (int i = 0; i < 8; i++)
		{
			float num;
			if (this.m_bAttacker)
			{
				num = -55f * (float)i;
			}
			else
			{
				num = 55f * (float)i;
			}
			if (i == 0)
			{
				this.conditionItems[i].Init();
			}
			else
			{
				this.conditionItems.Add(this.conditionItems[0].CloneItem("Icon" + (i + 1).ToString(), new Vector3(num, 0f, 0f)));
			}
			if (this.m_bAttacker)
			{
				UIEventListener uieventListener = UIEventListener.Get(this.conditionItems[i].rootObj);
				uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(this.OnHoverEffectItem));
			}
		}
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x0017D54C File Offset: 0x0017B74C
	public void UpdateUnit(UnitTB unit)
	{
		this.m_currentUnit = unit;
		this.m_face.mainTexture = unit.icon;
		this.m_labName.text = unit.unitName;
		this.m_Hp.text = unit.HP.ToString();
		this.m_Mp.text = unit.SP.ToString();
		List<ConditionNode> allCondition = unit.GetAllCondition();
		for (int i = 0; i < this.conditionItems.Count; i++)
		{
			Utility.SetActive(this.conditionItems[i].rootObj, false);
			if (i < allCondition.Count)
			{
				this.conditionItems[i].spIcon.spriteName = allCondition[i].m_strIconName;
				Utility.SetActive(this.conditionItems[i].rootObj, true);
			}
		}
		this.m_labAbilityDamage.text = string.Empty;
		this.m_labAbilityName.text = string.Empty;
		this.m_labAbilityLv.text = string.Empty;
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x0017D660 File Offset: 0x0017B860
	public void UpdateAttack(UnitTB unitSrc, UnitAbility uab, UnitTB unitDesc)
	{
		if (uab == null || unitSrc == null)
		{
			this.m_labAbilityName.text = string.Empty;
			this.m_labAbilityDamage.text = string.Empty;
			this.m_labAbilityLv.text = string.Empty;
		}
		else
		{
			this.m_labAbilityName.text = uab.name;
			this.m_labAbilityLv.text = unitSrc.GetRoutineLv(uab.ID).ToString();
			if (unitDesc != null)
			{
				int abilityToTargetDamage = unitSrc.GetAbilityToTargetDamage(unitDesc, uab, unitSrc.occupiedTile);
				int num = Mathf.RoundToInt(unitSrc.GetAbilityToTargetHitRate(unitDesc, uab, unitSrc.occupiedTile) * 100f);
				this.m_labAbilityDamage.text = abilityToTargetDamage.ToString() + " " + num.ToString() + "%";
			}
			else
			{
				int abilityDamage = unitSrc.GetAbilityDamage(uab);
				this.m_labAbilityDamage.text = abilityDamage.ToString();
			}
		}
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x0017D760 File Offset: 0x0017B960
	public void UpdateDefense(UnitTB unitSrc, UnitAbility uab, UnitTB unitDesc)
	{
		if (unitDesc == null)
		{
			this.m_labAbilityName.text = string.Empty;
			this.m_labAbilityDamage.text = string.Empty;
			this.m_labAbilityLv.text = string.Empty;
		}
		else if (unitDesc.GetCounterAttackDisabled())
		{
			this.m_labAbilityName.text = string.Empty;
			this.m_labAbilityDamage.text = string.Empty;
			this.m_labAbilityLv.text = string.Empty;
		}
		else
		{
			UnitAbility countAttackAbility = unitDesc.GetCountAttackAbility();
			if (countAttackAbility == null)
			{
				this.m_labAbilityName.text = string.Empty;
				this.m_labAbilityDamage.text = string.Empty;
				this.m_labAbilityLv.text = string.Empty;
			}
			else if (countAttackAbility.effectType == _EffectType.Heal)
			{
				this.m_labAbilityName.text = string.Empty;
				this.m_labAbilityDamage.text = string.Empty;
				this.m_labAbilityLv.text = string.Empty;
			}
			else
			{
				List<Tile> abilityAttackAbleTiles = unitDesc.GetAbilityAttackAbleTiles(unitDesc.occupiedTile, countAttackAbility);
				if (abilityAttackAbleTiles.Contains(unitSrc.occupiedTile))
				{
					int abilityToTargetDamage = unitDesc.GetAbilityToTargetDamage(unitSrc, countAttackAbility, unitDesc.occupiedTile);
					int num = Mathf.RoundToInt(unitDesc.GetAbilityToTargetCounterRate(unitSrc, uab) * 100f);
					this.m_labAbilityName.text = countAttackAbility.name;
					this.m_labAbilityDamage.text = abilityToTargetDamage.ToString() + " " + num.ToString() + "%";
					this.m_labAbilityLv.text = unitDesc.GetRoutineLv(countAttackAbility.ID).ToString();
				}
				else
				{
					this.m_labAbilityName.text = string.Empty;
					this.m_labAbilityDamage.text = string.Empty;
					this.m_labAbilityLv.text = string.Empty;
				}
			}
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x0001F0BD File Offset: 0x0001D2BD
	public void HideTooltip()
	{
		this.m_tooltipObj.SetActive(false);
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x0017D944 File Offset: 0x0017BB44
	private void OnHoverEffectItem(GameObject butObj, bool state)
	{
		if (state && this.m_currentUnit != null)
		{
			int num = 0;
			for (int i = 0; i < this.conditionItems.Count; i++)
			{
				if (this.conditionItems[i].rootObj == butObj)
				{
					num = i;
				}
			}
			List<ConditionNode> allCondition = this.m_currentUnit.GetAllCondition();
			if (num < allCondition.Count)
			{
				ConditionNode conditionNode = allCondition[num];
				string text = string.Empty;
				if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff)
				{
					if (conditionNode.m_iRemoveByAttack > 0)
					{
						text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
					}
					else if (conditionNode.m_iRemoveOnHit > 0)
					{
						text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveOnHit);
					}
					else
					{
						text = string.Format("[40FF40]{0}[-]", conditionNode.m_strName);
					}
				}
				else if (conditionNode.m_iCondType == _ConditionType.StackBuff)
				{
					text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
				}
				else if (conditionNode.m_iCondType == _ConditionType.Debuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff)
				{
					if (conditionNode.m_iRemoveByAttack > 0)
					{
						text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
					}
					else if (conditionNode.m_iRemoveOnHit > 0)
					{
						text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveOnHit);
					}
					else
					{
						text = string.Format("[FF3030]{0}[-]", conditionNode.m_strName);
					}
				}
				else if (conditionNode.m_iCondType == _ConditionType.StackDebuff)
				{
					text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
				}
				text = text + "\n[FFFFFF]" + conditionNode.m_strDesp + "[-]";
				this.m_lbTooltip.text = text;
				Utility.SetActive(this.m_tooltipObj, true);
			}
		}
		else
		{
			Utility.SetActive(this.m_tooltipObj, false);
		}
	}

	// Token: 0x04003CD2 RID: 15570
	public bool m_bAttacker;

	// Token: 0x04003CD3 RID: 15571
	public UITexture m_face;

	// Token: 0x04003CD4 RID: 15572
	public UILabel m_labName;

	// Token: 0x04003CD5 RID: 15573
	public UILabel m_Hp;

	// Token: 0x04003CD6 RID: 15574
	public UILabel m_Mp;

	// Token: 0x04003CD7 RID: 15575
	public GameObject m_tooltipObj;

	// Token: 0x04003CD8 RID: 15576
	public UILabel m_lbTooltip;

	// Token: 0x04003CD9 RID: 15577
	public UILabel m_labAbilityName;

	// Token: 0x04003CDA RID: 15578
	public UILabel m_labAbilityDamage;

	// Token: 0x04003CDB RID: 15579
	public UILabel m_labAbilityLv;

	// Token: 0x04003CDC RID: 15580
	private UnitTB m_currentUnit;

	// Token: 0x04003CDD RID: 15581
	public List<EffectItem> conditionItems = new List<EffectItem>();
}
