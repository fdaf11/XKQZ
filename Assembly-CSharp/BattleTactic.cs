using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class BattleTactic
{
	// Token: 0x060005B0 RID: 1456 RVA: 0x00041D54 File Offset: 0x0003FF54
	public void LoadText(string filePath)
	{
		this.m_TacticNodeList.Clear();
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
						BattleTacticNode battleTacticNode = new BattleTacticNode();
						battleTacticNode.iNodeID = int.Parse(array3[0]);
						battleTacticNode.sName = array3[1];
						battleTacticNode.sDesc = array3[2];
						string text2 = array3[3].Replace(")*(", "*");
						text2 = text2.Substring(1, text2.Length - 2);
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
							BattleUnLockNode battleUnLockNode = new BattleUnLockNode();
							battleUnLockNode.iAbility = int.Parse(array5[0]);
							battleUnLockNode.iType = int.Parse(array5[1]);
							battleUnLockNode.iValue = int.Parse(array5[2]);
							battleTacticNode.lUnlockNodeList.Add(battleUnLockNode);
						}
						battleTacticNode.iTacticPoint = int.Parse(array3[4]);
						battleTacticNode.iTargetFaction = int.Parse(array3[5]);
						battleTacticNode.iTargetType = int.Parse(array3[6]);
						battleTacticNode.iTargetWeapon = int.Parse(array3[7]);
						string[] array6 = array3[8].Split(new char[]
						{
							",".get_Chars(0)
						});
						foreach (string text3 in array6)
						{
							battleTacticNode.lConditionIDList.Add(int.Parse(text3));
						}
						this.m_TacticNodeList.Add(battleTacticNode);
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

	// Token: 0x060005B1 RID: 1457 RVA: 0x00041FAC File Offset: 0x000401AC
	public BattleTacticNode GetTacticNode(int iD)
	{
		foreach (BattleTacticNode battleTacticNode in this.m_TacticNodeList)
		{
			if (battleTacticNode.iNodeID == iD)
			{
				return battleTacticNode;
			}
		}
		return null;
	}

	// Token: 0x04000624 RID: 1572
	public List<BattleTacticNode> m_TacticNodeList = new List<BattleTacticNode>();
}
