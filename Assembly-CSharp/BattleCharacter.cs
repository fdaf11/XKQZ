using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class BattleCharacter
{
	// Token: 0x06000520 RID: 1312 RVA: 0x0003C048 File Offset: 0x0003A248
	public void LoadText(string filePath)
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
						string[] array3 = text.Trim().Split(new char[]
						{
							"\t".get_Chars(0)
						});
						BattleCharacterNode battleCharacterNode = new BattleCharacterNode();
						battleCharacterNode.m_iCharID = int.Parse(array3[0]);
						battleCharacterNode.m_strName = array3[1];
						battleCharacterNode.m_Rank = array3[2];
						battleCharacterNode.m_iNeigong = int.Parse(array3[3]);
						battleCharacterNode.m_iHp = int.Parse(array3[4]);
						battleCharacterNode.m_iSp = int.Parse(array3[5]);
						battleCharacterNode.m_fCritical = float.Parse(array3[6]);
						battleCharacterNode.m_fDodge = float.Parse(array3[7]);
						battleCharacterNode.m_fCounter = float.Parse(array3[8]);
						battleCharacterNode.m_iMobility = int.Parse(array3[9]);
						battleCharacterNode.m_iDefance = int.Parse(array3[10]);
						string[] array4 = array3[11].Split(new char[]
						{
							",".get_Chars(0)
						});
						foreach (string text2 in array4)
						{
							battleCharacterNode.m_iRoutineIDList.Add(int.Parse(text2));
						}
						if (array3.Length < 13)
						{
							battleCharacterNode.m_strVoice = battleCharacterNode.m_iCharID.ToString().Substring(0, 6);
						}
						else
						{
							battleCharacterNode.m_strVoice = array3[12];
						}
						this.m_BattleCharacterList.Add(battleCharacterNode);
					}
					catch (Exception ex)
					{
						Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						Debug.LogError(ex.Message);
					}
				}
			}
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0003C24C File Offset: 0x0003A44C
	public BattleCharacterNode GetBattleCharacterNode(int ID)
	{
		foreach (BattleCharacterNode battleCharacterNode in this.m_BattleCharacterList)
		{
			if (battleCharacterNode.m_iCharID == ID)
			{
				return battleCharacterNode;
			}
		}
		return null;
	}

	// Token: 0x0400055B RID: 1371
	private List<BattleCharacterNode> m_BattleCharacterList = new List<BattleCharacterNode>();
}
