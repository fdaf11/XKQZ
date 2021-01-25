using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class BattleArea
{
	// Token: 0x0600051C RID: 1308 RVA: 0x0003BAA0 File Offset: 0x00039CA0
	public void LoadText(string filePath)
	{
		this.m_BattleAreaList.Clear();
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
						string[] array3 = text.Trim().Split(new char[]
						{
							"\t".get_Chars(0)
						});
						BattleAreaNode battleAreaNode = new BattleAreaNode();
						battleAreaNode.m_iID = int.Parse(array3[0]);
						battleAreaNode.m_sMapName = array3[1];
						string text2 = array3[2].Replace(")*(", "*");
						if (text2.Length > 3)
						{
							text2 = text2.Substring(1, text2.Length - 2);
						}
						string[] array4 = text2.Split(new char[]
						{
							"*".get_Chars(0)
						});
						for (int j = 0; j < array4.Length; j++)
						{
							string[] array5 = array4[j].Split(new char[]
							{
								",".get_Chars(0)
							});
							if (array5.Length >= 3)
							{
								BattleJoinCharacterNode battleJoinCharacterNode = new BattleJoinCharacterNode();
								battleJoinCharacterNode.m_iTurn = int.Parse(array5[0]);
								battleJoinCharacterNode.m_iCharID = int.Parse(array5[1]);
								battleJoinCharacterNode.m_iTile = int.Parse(array5[2]);
								battleJoinCharacterNode.m_iFaction = 0;
								battleAreaNode.m_TeamList.Add(battleJoinCharacterNode);
							}
						}
						text2 = array3[3].Replace(")*(", "*");
						if (text2.Length > 3)
						{
							text2 = text2.Substring(1, text2.Length - 2);
						}
						array4 = text2.Split(new char[]
						{
							"*".get_Chars(0)
						});
						for (int k = 0; k < array4.Length; k++)
						{
							string[] array6 = array4[k].Split(new char[]
							{
								",".get_Chars(0)
							});
							if (array6.Length >= 3)
							{
								BattleJoinCharacterNode battleJoinCharacterNode2 = new BattleJoinCharacterNode();
								battleJoinCharacterNode2.m_iTurn = int.Parse(array6[0]);
								battleJoinCharacterNode2.m_iCharID = int.Parse(array6[1]);
								battleJoinCharacterNode2.m_iTile = int.Parse(array6[2]);
								if (array6.Length >= 4)
								{
									battleJoinCharacterNode2.m_iFaction = int.Parse(array6[3]);
								}
								else
								{
									battleJoinCharacterNode2.m_iFaction = 1;
								}
								battleAreaNode.m_EnemyList.Add(battleJoinCharacterNode2);
							}
						}
						text2 = array3[4].Replace(")*(", "*");
						if (text2.Length > 3)
						{
							text2 = text2.Substring(1, text2.Length - 2);
						}
						array4 = text2.Split(new char[]
						{
							"*".get_Chars(0)
						});
						for (int l = 0; l < array4.Length; l++)
						{
							string[] array7 = array4[l].Split(new char[]
							{
								",".get_Chars(0)
							});
							if (array7.Length >= 3)
							{
								WinLoseRequirement winLoseRequirement = new WinLoseRequirement();
								winLoseRequirement.m_iType = int.Parse(array7[0]);
								winLoseRequirement.m_iValue1 = int.Parse(array7[1]);
								winLoseRequirement.m_iValue2 = int.Parse(array7[2]);
								battleAreaNode.m_VicReqList.Add(winLoseRequirement);
							}
						}
						text2 = array3[5].Replace(")*(", "*");
						if (text2.Length > 3)
						{
							text2 = text2.Substring(1, text2.Length - 2);
						}
						array4 = text2.Split(new char[]
						{
							"*".get_Chars(0)
						});
						for (int m = 0; m < array4.Length; m++)
						{
							string[] array8 = array4[m].Split(new char[]
							{
								",".get_Chars(0)
							});
							if (array8.Length >= 3)
							{
								WinLoseRequirement winLoseRequirement2 = new WinLoseRequirement();
								winLoseRequirement2.m_iType = int.Parse(array8[0]);
								winLoseRequirement2.m_iValue1 = int.Parse(array8[1]);
								winLoseRequirement2.m_iValue2 = int.Parse(array8[2]);
								battleAreaNode.m_FailReqList.Add(winLoseRequirement2);
							}
						}
						battleAreaNode.m_iRewardID = int.Parse(array3[6]);
						battleAreaNode.m_iFailResultID = int.Parse(array3[7]);
						battleAreaNode.m_iTime = int.Parse(array3[8]);
						battleAreaNode.m_sSoundName = array3[9];
						battleAreaNode.m_iInside = int.Parse(array3[10]);
						battleAreaNode.m_iSave = int.Parse(array3[11]);
						int num = 12;
						while (num + 2 <= array3.Length)
						{
							DropItem dropItem = new DropItem();
							dropItem.ItemID = int.Parse(array3[num]);
							dropItem.DropRate = int.Parse(array3[num + 1]);
							battleAreaNode.m_DropItemList.Add(dropItem);
							num += 2;
						}
						this.m_BattleAreaList.Add(battleAreaNode);
					}
					catch (Exception ex)
					{
						Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						Debug.Log(ex.Message);
					}
				}
			}
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0003BFDC File Offset: 0x0003A1DC
	public BattleAreaNode GetBattleAreaNode(int ID)
	{
		foreach (BattleAreaNode battleAreaNode in this.m_BattleAreaList)
		{
			if (battleAreaNode.m_iID == ID)
			{
				return battleAreaNode;
			}
		}
		return null;
	}

	// Token: 0x0400054D RID: 1357
	private List<BattleAreaNode> m_BattleAreaList = new List<BattleAreaNode>();
}
