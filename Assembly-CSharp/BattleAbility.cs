using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class BattleAbility
{
	// Token: 0x06000510 RID: 1296 RVA: 0x000051DB File Offset: 0x000033DB
	public void LoadText(string str)
	{
		this.LoadCondition(str);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0003B200 File Offset: 0x00039400
	private void LoadAbility(string filePath)
	{
		string[] array = Game.ExtractTextFile(filePath);
		if (array == null)
		{
			return;
		}
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (text.get_Chars(0) != '#')
				{
					try
					{
						string[] array3 = text.Split(new char[]
						{
							"\t".get_Chars(0)
						});
						AbilityNode abilityNode = new AbilityNode();
						abilityNode.m_iAbilityID = int.Parse(array3[0]);
						abilityNode.m_strName = array3[1];
						if (array3[2] == "0")
						{
							abilityNode.m_bNeedToSelectTarget = false;
						}
						else
						{
							abilityNode.m_bNeedToSelectTarget = true;
						}
						abilityNode.m_iSkillType = int.Parse(array3[3]);
						abilityNode.m_iTargetType = int.Parse(array3[4]);
						abilityNode.m_iTargetArea = int.Parse(array3[5]);
						abilityNode.m_iRange = int.Parse(array3[6]);
						abilityNode.m_iAOE = int.Parse(array3[7]);
						abilityNode.m_iValue1 = int.Parse(array3[8]);
						abilityNode.m_iValue2 = int.Parse(array3[9]);
						abilityNode.m_iRequestSP = int.Parse(array3[10]);
						abilityNode.m_iCD = int.Parse(array3[11]);
						if (array3[12] == "0")
						{
							abilityNode.m_bUseSkillAfterMove = false;
						}
						else
						{
							abilityNode.m_bUseSkillAfterMove = true;
						}
						string[] array4 = array3[13].Split(new char[]
						{
							",".get_Chars(0)
						});
						foreach (string text2 in array4)
						{
							int num = int.Parse(text2);
							if (num != 0)
							{
								abilityNode.m_iConditionIDList.Add(num);
							}
						}
						array4 = array3[14].Split(new char[]
						{
							",".get_Chars(0)
						});
						foreach (string text3 in array4)
						{
							abilityNode.m_iSkillEffectIDList.Add(int.Parse(text3));
						}
						abilityNode.m_strSkillIconName = array3[15].Trim();
						this.m_AbilityNodeList.Add(abilityNode);
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
					}
				}
			}
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0003B494 File Offset: 0x00039694
	private void LoadCondition(string filePath)
	{
		this.m_ConditionList.Clear();
		string[] array = Game.ExtractTextFile(filePath);
		if (array == null)
		{
			return;
		}
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (text.get_Chars(0) != '#')
				{
					try
					{
						string[] array3 = text.Split(new char[]
						{
							"\t".get_Chars(0)
						});
						ConditionNode conditionNode = new ConditionNode();
						conditionNode.m_iConditionID = int.Parse(array3[0]);
						conditionNode.m_strName = array3[1];
						conditionNode.m_strDesp = array3[2].Replace("<br>", "\n");
						conditionNode.m_strIconName = array3[3];
						int num = int.Parse(array3[4]);
						conditionNode.m_iCondType = (_ConditionType)num;
						conditionNode.m_iCondTarget = int.Parse(array3[5]);
						conditionNode.m_iMinTurn = int.Parse(array3[6]);
						conditionNode.m_iMaxTurn = int.Parse(array3[7]);
						conditionNode.m_iRemoveByAttack = int.Parse(array3[8]);
						conditionNode.m_iRemoveOnHit = int.Parse(array3[9]);
						int num2 = array3.Length;
						int num3 = 10;
						while (num3 + 6 <= num2)
						{
							EffectPart effectPart = new EffectPart();
							if (int.TryParse(array3[num3], ref num))
							{
								effectPart.m_effectPartType = (_EffectPartType)num;
								num = int.Parse(array3[num3 + 1]);
								effectPart.m_effectAccumulateType = (_EffectAccumulateType)num;
								if (array3[num3 + 2] == "0")
								{
									effectPart.m_bPercent = false;
								}
								else
								{
									effectPart.m_bPercent = true;
								}
								effectPart.m_iValue1 = int.Parse(array3[num3 + 3]);
								effectPart.m_iValue2 = int.Parse(array3[num3 + 4]);
								effectPart.m_iValueLimit = int.Parse(array3[num3 + 5]);
								if (effectPart.m_iValue1 != 0 || effectPart.m_iValue2 != 0 || effectPart.m_iValueLimit != 0 || effectPart.m_effectPartType != _EffectPartType.HitPoint || effectPart.m_effectAccumulateType != _EffectAccumulateType.None)
								{
									conditionNode.m_effectPartList.Add(effectPart);
								}
							}
							num3 += 6;
						}
						this.m_ConditionList.Add(conditionNode);
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
					}
				}
			}
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0003B720 File Offset: 0x00039920
	private void LoadNeigong(string filePath)
	{
		string[] array = Game.ExtractTextFile(filePath);
		if (array == null)
		{
			return;
		}
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (text.get_Chars(0) != '#')
				{
					try
					{
						string[] array3 = text.Split(new char[]
						{
							"\t".get_Chars(0)
						});
						NeigongNode neigongNode = new NeigongNode();
						neigongNode.m_iNeigongID = int.Parse(array3[0]);
						neigongNode.m_strName = array3[1];
						neigongNode.m_strDesp = array3[2];
						neigongNode.m_iExp = int.Parse(array3[3]);
						neigongNode.m_iNeigongType = int.Parse(array3[4]);
						neigongNode.m_strSelectImage = array3[5];
						neigongNode.m_strStatusImage = array3[6];
						int num = array3.Length;
						int num2 = 7;
						while (num2 + 6 <= num)
						{
							EffectPart effectPart = new EffectPart();
							int num3;
							if (int.TryParse(array3[num2], ref num3))
							{
								effectPart.m_effectPartType = (_EffectPartType)num3;
								num3 = int.Parse(array3[num2 + 1]);
								effectPart.m_effectAccumulateType = (_EffectAccumulateType)num3;
								if (array3[num2 + 2] == "0")
								{
									effectPart.m_bPercent = false;
								}
								else
								{
									effectPart.m_bPercent = true;
								}
								effectPart.m_iValue1 = int.Parse(array3[num2 + 3]);
								effectPart.m_iValue2 = int.Parse(array3[num2 + 4]);
								effectPart.m_iValueLimit = int.Parse(array3[num2 + 5]);
								if (effectPart.m_iValue1 != 0 || effectPart.m_iValue2 != 0 || effectPart.m_iValueLimit != 0 || effectPart.m_effectPartType != _EffectPartType.HitPoint || effectPart.m_effectAccumulateType != _EffectAccumulateType.None)
								{
									neigongNode.m_effectPartList.Add(effectPart);
								}
							}
							num2 += 6;
						}
						this.m_NeigongList.Add(neigongNode);
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
					}
				}
			}
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0003B95C File Offset: 0x00039B5C
	public AbilityNode GetAbilityNode(int ID)
	{
		foreach (AbilityNode abilityNode in this.m_AbilityNodeList)
		{
			if (abilityNode.m_iAbilityID == ID)
			{
				return abilityNode;
			}
		}
		return null;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0003B9C8 File Offset: 0x00039BC8
	public ConditionNode GetConditionNode(int ID)
	{
		foreach (ConditionNode conditionNode in this.m_ConditionList)
		{
			if (conditionNode.m_iConditionID == ID)
			{
				return conditionNode;
			}
		}
		return null;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0003BA34 File Offset: 0x00039C34
	public NeigongNode GetNeigongNode(int ID)
	{
		foreach (NeigongNode neigongNode in this.m_NeigongList)
		{
			if (neigongNode.m_iNeigongID == ID)
			{
				return neigongNode;
			}
		}
		return null;
	}

	// Token: 0x0400052F RID: 1327
	public List<AbilityNode> m_AbilityNodeList = new List<AbilityNode>();

	// Token: 0x04000530 RID: 1328
	public List<ConditionNode> m_ConditionList = new List<ConditionNode>();

	// Token: 0x04000531 RID: 1329
	public List<NeigongNode> m_NeigongList = new List<NeigongNode>();
}
