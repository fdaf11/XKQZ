using System;
using System.Collections.Generic;
using Heluo.Wulin;

// Token: 0x02000131 RID: 305
public class ConditionManager
{
	// Token: 0x0600062B RID: 1579 RVA: 0x0004421C File Offset: 0x0004241C
	private static bool ConditionValue(int iCount, int iValue, eCompareType type)
	{
		bool result = false;
		if (type == eCompareType.Equal)
		{
			result = (iCount == iValue);
		}
		else if (type == eCompareType.Greater)
		{
			result = (iCount >= iValue);
		}
		else if (type == eCompareType.Lass)
		{
			result = (iCount < iValue);
		}
		return result;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0004425C File Offset: 0x0004245C
	public static bool CheckCondition(List<Condition> _ConditionList, bool bAnd = true, int iNpcID = 0, string unitGid = "")
	{
		bool flag = false;
		if (_ConditionList.Count != 0)
		{
			foreach (Condition condition in _ConditionList)
			{
				if (condition._iType == ConditionKind.ToTrue)
				{
					return !flag;
				}
				if (condition._iType == ConditionKind.Item || condition._iType == ConditionKind.OnlyCheckItem)
				{
					int iID = condition.m_Parameter;
					int iCount = BackpackStatus.m_Instance.CheclItemAmount(iID);
					flag = ConditionManager.ConditionValue(iCount, condition.m_iValue, (eCompareType)condition.m_iAmount);
				}
				else if (condition._iType == ConditionKind.TeamMoney)
				{
					int type = condition.m_Parameter;
					int money = BackpackStatus.m_Instance.GetMoney();
					flag = ConditionManager.ConditionValue(money, condition.m_iAmount, (eCompareType)type);
				}
				else if (condition._iType == ConditionKind.PartyMember)
				{
					int iNpcID2 = condition.m_Parameter;
					flag = TeamStatus.m_Instance.CheckTeamMember(iNpcID2);
					if (condition.m_iAmount == 1)
					{
						flag = !flag;
					}
				}
				else if (condition._iType == ConditionKind.RoundRange)
				{
					int startRound = condition.m_Parameter;
					flag = YoungHeroTime.m_instance.CheackRoundRange(startRound, condition.m_iAmount);
					if (condition.m_iValue == 1)
					{
						flag = !flag;
					}
				}
				else if (condition._iType == ConditionKind.CollectionQuest)
				{
					flag = MissionStatus.m_instance.CheckCollectionQuest(condition.m_Parameter);
					if (condition.m_iAmount == 1)
					{
						flag = !flag;
					}
				}
				else if (condition._iType == ConditionKind.ProcessingQuest)
				{
					flag = MissionStatus.m_instance.CheckQuest(condition.m_Parameter);
					if (condition.m_iAmount == 1)
					{
						flag = !flag;
					}
				}
				else if (condition._iType == ConditionKind.TriggeredEvent)
				{
					flag = TeamStatus.m_Instance.m_EventList.Contains(int.Parse(condition.m_Parameter));
					if (condition.m_iAmount == 1)
					{
						flag = !flag;
					}
				}
				else if (condition._iType == ConditionKind.MoreNpcRoutine || condition._iType == ConditionKind.LessNpcRoutine)
				{
					int num = condition.m_Parameter;
					if (num == 0)
					{
						num = iNpcID;
					}
					int routineLv = NPC.m_instance.GetRoutineLv(num, condition.m_iAmount);
					if (condition._iType == ConditionKind.MoreNpcRoutine)
					{
						flag = (routineLv >= condition.m_iValue);
					}
					else
					{
						flag = (routineLv < condition.m_iValue);
					}
				}
				else if (condition._iType == ConditionKind.MoreNpcNeigong || condition._iType == ConditionKind.LessNpcNeigong)
				{
					int num2 = condition.m_Parameter;
					if (num2 == 0)
					{
						num2 = iNpcID;
					}
					int neigongLv = NPC.m_instance.GetNeigongLv(num2, condition.m_iAmount);
					if (condition._iType == ConditionKind.MoreNpcNeigong)
					{
						flag = (neigongLv >= condition.m_iValue);
					}
					else
					{
						flag = (neigongLv < condition.m_iValue);
					}
				}
				else if (condition._iType == ConditionKind.MoreNpcProperty)
				{
					int num3 = condition.m_Parameter;
					if (num3 == 0)
					{
						num3 = iNpcID;
					}
					int characterProperty = NPC.m_instance.getCharacterProperty(num3, condition.m_iAmount, null);
					flag = (characterProperty >= condition.m_iValue);
				}
				else if (condition._iType == ConditionKind.LessNpcProperty)
				{
					int num4 = condition.m_Parameter;
					if (num4 == 0)
					{
						num4 = iNpcID;
					}
					int characterProperty2 = NPC.m_instance.getCharacterProperty(num4, condition.m_iAmount, null);
					if (characterProperty2 > 0)
					{
						flag = (characterProperty2 < condition.m_iValue);
					}
				}
				else if (condition._iType == ConditionKind.Friendliness)
				{
					int iID2 = condition.m_Parameter;
					int npcFriendly = Game.NpcData.GetNpcFriendly(iID2);
					if (condition.m_iValue == 0)
					{
						flag = (npcFriendly >= condition.m_iAmount);
					}
					else
					{
						flag = (npcFriendly < condition.m_iAmount);
					}
				}
				else if (condition._iType == ConditionKind.TeammateFriendliness)
				{
					int num5 = condition.m_Parameter;
					if (TeamStatus.m_Instance.CheckTeamMember(num5))
					{
						int npcFriendly2 = Game.NpcData.GetNpcFriendly(num5);
						if (condition.m_iValue == 0)
						{
							flag = (npcFriendly2 >= condition.m_iAmount);
						}
						else
						{
							flag = (npcFriendly2 < condition.m_iAmount);
						}
					}
				}
				else if (condition._iType != ConditionKind.NpcRoutineAbility)
				{
					if (condition._iType == ConditionKind.TimeCheck)
					{
						int round = condition.m_Parameter;
						flag = YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, round);
					}
					else if (condition._iType == ConditionKind.TeamMemberAmount)
					{
						int teamMemberAmount = TeamStatus.m_Instance.GetTeamMemberAmount();
						int type2 = condition.m_Parameter;
						flag = ConditionManager.ConditionValue(teamMemberAmount, condition.m_iAmount, (eCompareType)type2);
					}
					else if (condition._iType == ConditionKind.Flag)
					{
						int iCount2 = Game.Variable[condition.m_Parameter];
						flag = ConditionManager.ConditionValue(iCount2, condition.m_iValue, (eCompareType)condition.m_iAmount);
					}
					else if (condition._iType == ConditionKind.RegionalStability)
					{
						string regional = BigMapController.GetRegional();
						flag = (condition.m_Parameter == regional);
					}
					else if (condition._iType == ConditionKind.FailQuest)
					{
						flag = MissionStatus.m_instance.CheckFailQuest(condition.m_Parameter);
						if (condition.m_iAmount == 1)
						{
							flag = !flag;
						}
					}
					else if (condition._iType == ConditionKind.MoreNpcItem)
					{
						int npcitemCount = NPC.m_instance.GetNPCItemCount(condition.m_Parameter, condition.m_iAmount);
						flag = ConditionManager.ConditionValue(npcitemCount, condition.m_iValue, eCompareType.Greater);
					}
					else if (condition._iType == ConditionKind.LessNpcItem)
					{
						int npcitemCount2 = NPC.m_instance.GetNPCItemCount(condition.m_Parameter, condition.m_iAmount);
						flag = ConditionManager.ConditionValue(npcitemCount2, condition.m_iValue, eCompareType.Lass);
					}
					else if (condition._iType == ConditionKind.NpcMoney)
					{
						int npcMoney = NPC.m_instance.GetNpcMoney(condition.m_Parameter);
						flag = ConditionManager.ConditionValue(npcMoney, condition.m_iValue, (eCompareType)condition.m_iAmount);
					}
					else if (condition._iType == ConditionKind.NpcQuest)
					{
						flag = NPC.m_instance.CheckRendomQuest(condition.m_iValue, condition.m_Parameter);
						if (condition.m_iAmount == 1)
						{
							flag = !flag;
						}
					}
					else if (condition._iType == ConditionKind.CheckSkillMax)
					{
						flag = NPC.m_instance.CheckSkill(condition.m_Parameter, condition.m_iAmount);
					}
					else if (condition._iType == ConditionKind.DLC_Level)
					{
						if (condition.m_iValue != 0)
						{
							flag = !TeamStatus.m_Instance.CheckDLCMissionLevel(condition.m_Parameter.m_iVal, condition.m_iAmount);
						}
						else
						{
							flag = TeamStatus.m_Instance.CheckDLCMissionLevel(condition.m_Parameter.m_iVal, condition.m_iAmount);
						}
					}
					else if (condition._iType == ConditionKind.DLC_Gold)
					{
						flag = TeamStatus.m_Instance.CheckDLCGold(condition.m_Parameter.m_iVal, condition.m_iAmount);
					}
					else if (condition._iType == ConditionKind.DLC_Fame)
					{
						flag = TeamStatus.m_Instance.CheckDLCFame(condition.m_Parameter.m_iVal, condition.m_iAmount);
					}
					else if (condition._iType == ConditionKind.DLC_PassiveNode)
					{
						int num6 = condition.m_Parameter;
						if (num6 == 0)
						{
							num6 = iNpcID;
						}
						bool flag2 = NPC.m_instance.HaveThisPassive(num6, condition.m_iValue);
						flag = ((condition.m_iAmount == 0 && !flag2) || (condition.m_iAmount != 0 && flag2));
					}
					else if (condition._iType == ConditionKind.DLC_CharLevel)
					{
						int num7 = condition.m_Parameter;
						if (num7 == 0)
						{
							num7 = iNpcID;
						}
						flag = NPC.m_instance.CheckDLCCharLevel(num7, condition.m_iAmount, condition.m_iValue);
					}
					else if (condition._iType == ConditionKind.DLC_UnitLevel)
					{
						flag = TeamStatus.m_Instance.CheckDLCUnitLevel(unitGid, condition.m_iAmount, condition.m_iValue);
					}
				}
				if (!flag && bAnd)
				{
					return flag;
				}
				if (flag && !bAnd)
				{
					return flag;
				}
			}
			return flag;
		}
		return true;
	}
}
