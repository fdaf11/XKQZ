using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class PartyMemberInfo : MonoBehaviour
{
	// Token: 0x06000F1E RID: 3870 RVA: 0x0007E380 File Offset: 0x0007C580
	private void Awake()
	{
		for (int i = 0; i < this.abilityItems.Count; i++)
		{
			this.abilityItems[i].Init();
			UIEventListener uieventListener = UIEventListener.Get(this.abilityItems[i].rootObj);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityButton));
		}
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0000A32E File Offset: 0x0000852E
	public void Show(int iID)
	{
		if (iID == 10001401)
		{
			this.ShowPlayer();
			return;
		}
		this.ShowMember(iID);
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0000264F File Offset: 0x0000084F
	private void ShowMember(int i8ID)
	{
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0000264F File Offset: 0x0000084F
	private void ShowPlayer()
	{
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0007E3F4 File Offset: 0x0007C5F4
	public void OnHoverAbilityButton(GameObject butObj, bool state)
	{
		string text = string.Empty;
		if (state)
		{
			if (butObj.GetComponent<BtnData>() == null)
			{
				return;
			}
			AbilityNode abilityNode = Game.g_BattleControl.m_battleAbility.GetAbilityNode(butObj.GetComponent<BtnData>().m_iBtnType);
			this.lbAbilityName.text = abilityNode.m_strName;
			this.lbAbilityDamage.text = abilityNode.m_iValue1.ToString("0") + " - " + abilityNode.m_iValue2.ToString("0");
			if (abilityNode.m_iConditionIDList.Count <= 0)
			{
				this.lbAbilityStatus.text = Game.StringTable.GetString(263041);
			}
			else
			{
				for (int i = 0; i < abilityNode.m_iConditionIDList.Count; i++)
				{
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(abilityNode.m_iConditionIDList[i]);
					if (conditionNode == null)
					{
						Debug.LogError(abilityNode.m_iConditionIDList[i].ToString() + " 狀態找不到");
					}
					else if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.StackBuff)
					{
						text = text + "[40FF40]" + conditionNode.m_strName + " [-]";
					}
					else
					{
						text = text + "[FF3030]" + conditionNode.m_strName + " [-]";
					}
				}
				this.lbAbilityStatus.text = text;
			}
			string text2 = string.Empty;
			_AbilityTargetType iTargetType = (_AbilityTargetType)abilityNode.m_iTargetType;
			if (iTargetType == _AbilityTargetType.Hostile)
			{
				text2 = Game.StringTable.GetString(263030);
			}
			if (iTargetType == _AbilityTargetType.Friendly)
			{
				text2 = Game.StringTable.GetString(263031);
			}
			if (iTargetType == _AbilityTargetType.AllUnits)
			{
				text2 = Game.StringTable.GetString(263032);
			}
			if (iTargetType == _AbilityTargetType.EmptyTile)
			{
				text2 = Game.StringTable.GetString(263033);
			}
			if (iTargetType == _AbilityTargetType.AllTile)
			{
				text2 = Game.StringTable.GetString(263034);
			}
			string text3 = string.Empty;
			_TargetArea iTargetArea = (_TargetArea)abilityNode.m_iTargetArea;
			if (iTargetArea == _TargetArea.Default)
			{
				if (abilityNode.m_bNeedToSelectTarget)
				{
					if (abilityNode.m_iAOE == 0)
					{
						text = Game.StringTable.GetString(263035).Replace("<br>", "\n");
						text3 = string.Format(text, abilityNode.m_iRange, text2);
					}
					else
					{
						text = Game.StringTable.GetString(263036).Replace("<br>", "\n");
						text3 = string.Format(text, abilityNode.m_iRange, abilityNode.m_iAOE, text2);
					}
				}
				else
				{
					text = Game.StringTable.GetString(263037).Replace("<br>", "\n");
					text3 = string.Format(text, abilityNode.m_iAOE, text2);
				}
			}
			else if (iTargetArea == _TargetArea.Line)
			{
				text = Game.StringTable.GetString(263038).Replace("<br>", "\n");
				text3 = string.Format(text, abilityNode.m_iRange, text2);
			}
			else if (iTargetArea == _TargetArea.Cone)
			{
				text = Game.StringTable.GetString(263039).Replace("<br>", "\n");
				text3 = string.Format(text, abilityNode.m_iRange, text2);
			}
			else if (iTargetArea == _TargetArea.Fan1)
			{
				text = Game.StringTable.GetString(263040).Replace("<br>", "\n");
				text3 = string.Format(text, abilityNode.m_iRange, text2);
			}
			this.lbAbilityDesp.text = text3;
			this.lbAbilityCost.text = abilityNode.m_iRequestSP.ToString() + Game.StringTable.GetString(263042);
			Utility.SetActive(this.InfoBoxObj, true);
		}
		else
		{
			Utility.SetActive(this.InfoBoxObj, false);
		}
	}

	// Token: 0x040011D8 RID: 4568
	public UITexture texPlayer;

	// Token: 0x040011D9 RID: 4569
	public UILabel lbName;

	// Token: 0x040011DA RID: 4570
	public UILabel lbHP;

	// Token: 0x040011DB RID: 4571
	public UILabel lbHPVal;

	// Token: 0x040011DC RID: 4572
	public UILabel lbSP;

	// Token: 0x040011DD RID: 4573
	public UILabel lbSPVal;

	// Token: 0x040011DE RID: 4574
	public UILabel lbOffense;

	// Token: 0x040011DF RID: 4575
	public UILabel lbDamage;

	// Token: 0x040011E0 RID: 4576
	public UILabel lbDamageVal;

	// Token: 0x040011E1 RID: 4577
	public UILabel lbHit;

	// Token: 0x040011E2 RID: 4578
	public UILabel lbHitVal;

	// Token: 0x040011E3 RID: 4579
	public UILabel lbCrit;

	// Token: 0x040011E4 RID: 4580
	public UILabel lbCritVal;

	// Token: 0x040011E5 RID: 4581
	public UILabel lbDefence;

	// Token: 0x040011E6 RID: 4582
	public UILabel lbArmor;

	// Token: 0x040011E7 RID: 4583
	public UILabel lbArmorVal;

	// Token: 0x040011E8 RID: 4584
	public UILabel lbDodge;

	// Token: 0x040011E9 RID: 4585
	public UILabel lbDodgeVal;

	// Token: 0x040011EA RID: 4586
	public UILabel lbCritDef;

	// Token: 0x040011EB RID: 4587
	public UILabel lbCritDefVal;

	// Token: 0x040011EC RID: 4588
	public UILabel lbCounter;

	// Token: 0x040011ED RID: 4589
	public UILabel lbCounterVal;

	// Token: 0x040011EE RID: 4590
	public UILabel lbMisc;

	// Token: 0x040011EF RID: 4591
	public UILabel lbMoveRange;

	// Token: 0x040011F0 RID: 4592
	public UILabel lbMoveRangeVal;

	// Token: 0x040011F1 RID: 4593
	public UILabel lbCounterDef;

	// Token: 0x040011F2 RID: 4594
	public UILabel lbCounterDefVal;

	// Token: 0x040011F3 RID: 4595
	public UILabel lbAbility;

	// Token: 0x040011F4 RID: 4596
	public List<EffectItem> abilityItems = new List<EffectItem>();

	// Token: 0x040011F5 RID: 4597
	public UILabel lbNeigong;

	// Token: 0x040011F6 RID: 4598
	public UILabel lbNeigongStr;

	// Token: 0x040011F7 RID: 4599
	public UISprite spfaction;

	// Token: 0x040011F8 RID: 4600
	public UILabel lbfaction;

	// Token: 0x040011F9 RID: 4601
	public UILabel lbTitle;

	// Token: 0x040011FA RID: 4602
	public UILabel lbIntroduction;

	// Token: 0x040011FB RID: 4603
	public GameObject InfoBoxObj;

	// Token: 0x040011FC RID: 4604
	public UILabel lbAbilityName;

	// Token: 0x040011FD RID: 4605
	public UILabel lbAbilityDesp;

	// Token: 0x040011FE RID: 4606
	public UILabel lbAbilityCost;

	// Token: 0x040011FF RID: 4607
	public UILabel lbAbilityDamage;

	// Token: 0x04001200 RID: 4608
	public UILabel lbAbilityStatus;
}
