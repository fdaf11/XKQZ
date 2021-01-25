using System;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class BattleSchedule
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x00005561 File Offset: 0x00003761
	public void LoadText(string str)
	{
		this.LoadClip(str);
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00041854 File Offset: 0x0003FA54
	private void LoadClip(string filePath)
	{
		this.m_ClipList.Clear();
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
					int num = 0;
					try
					{
						string[] array3 = text.Trim().Split(new char[]
						{
							"\t".get_Chars(0)
						});
						BattleTriggerClip battleTriggerClip = new BattleTriggerClip();
						battleTriggerClip.m_iAreaID = int.Parse(array3[0]);
						battleTriggerClip.m_iClipID = int.Parse(array3[1]);
						if (int.Parse(array3[2]) == 0)
						{
							battleTriggerClip.m_bStartClip = false;
						}
						else
						{
							battleTriggerClip.m_bStartClip = true;
						}
						battleTriggerClip.m_iNextID = int.Parse(array3[3]);
						battleTriggerClip.m_iLinkID = int.Parse(array3[4]);
						battleTriggerClip.m_TriggerType = (_BattleTriggerType)int.Parse(array3[5]);
						battleTriggerClip.m_iTriggerData = int.Parse(array3[6]);
						battleTriggerClip.m_RequireType = (_RequireType)int.Parse(array3[7]);
						battleTriggerClip.m_RequireEqual = (_EqualType)int.Parse(array3[8]);
						battleTriggerClip.m_strRequireValue1 = array3[9];
						if (int.TryParse(array3[9], ref num))
						{
							battleTriggerClip.m_iRequireValue1 = num;
						}
						else
						{
							battleTriggerClip.m_iRequireValue1 = 0;
						}
						battleTriggerClip.m_iRequireValue2 = int.Parse(array3[10]);
						battleTriggerClip.m_iTriggerPercent = int.Parse(array3[11]);
						battleTriggerClip.m_TriggerEvent = (_TriggerEvent)int.Parse(array3[12]);
						battleTriggerClip.m_strTriggerValue1 = array3[13].Trim();
						if (int.TryParse(array3[13], ref num))
						{
							battleTriggerClip.m_iTriggerValue1 = num;
						}
						else
						{
							battleTriggerClip.m_iTriggerValue1 = 0;
						}
						battleTriggerClip.m_iTriggerValue2 = int.Parse(array3[14]);
						battleTriggerClip.m_iTriggerValue3 = int.Parse(array3[15]);
						battleTriggerClip.m_iGroup = int.Parse(array3[16]);
						battleTriggerClip.m_iRepeat = int.Parse(array3[17]);
						battleTriggerClip.m_bUsed = false;
						this.m_ClipList.Add(battleTriggerClip);
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

	// Token: 0x060005A8 RID: 1448 RVA: 0x00041AB8 File Offset: 0x0003FCB8
	public void SaveText()
	{
		string text = Game.g_strAssetDataPath + "Resources/TextFiles/BattleSchedule";
		File.WriteAllText(text, this.SaveClip());
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00041AE4 File Offset: 0x0003FCE4
	private string SaveClip()
	{
		StringWriter stringWriter = new StringWriter();
		for (int i = 0; i < this.m_ClipList.Count; i++)
		{
			BattleTriggerClip battleTriggerClip = this.m_ClipList[i];
			string[] array = new string[29];
			array[0] = battleTriggerClip.m_iAreaID.ToString();
			array[1] = "\t";
			array[2] = battleTriggerClip.m_iClipID.ToString();
			array[3] = "\t";
			int num = 4;
			int triggerType = (int)battleTriggerClip.m_TriggerType;
			array[num] = triggerType.ToString();
			array[5] = "\t";
			array[6] = battleTriggerClip.m_iTriggerData.ToString();
			array[7] = "\t";
			int num2 = 8;
			int requireType = (int)battleTriggerClip.m_RequireType;
			array[num2] = requireType.ToString();
			array[9] = "\t";
			int num3 = 10;
			int requireEqual = (int)battleTriggerClip.m_RequireEqual;
			array[num3] = requireEqual.ToString();
			array[11] = "\t";
			array[12] = battleTriggerClip.m_iRequireValue1.ToString();
			array[13] = "\t";
			array[14] = battleTriggerClip.m_iRequireValue2.ToString();
			array[15] = "\t";
			array[16] = battleTriggerClip.m_iTriggerPercent.ToString();
			array[17] = "\t";
			int num4 = 18;
			int triggerEvent = (int)battleTriggerClip.m_TriggerEvent;
			array[num4] = triggerEvent.ToString();
			array[19] = "\t";
			array[20] = battleTriggerClip.m_iTriggerValue1.ToString();
			array[21] = "\t";
			array[22] = battleTriggerClip.m_iTriggerValue2.ToString();
			array[23] = "\t";
			array[24] = battleTriggerClip.m_iTriggerValue3.ToString();
			array[25] = "\t";
			array[26] = battleTriggerClip.m_iGroup.ToString();
			array[27] = "\t";
			array[28] = battleTriggerClip.m_iRepeat.ToString();
			string text = string.Concat(array);
			stringWriter.WriteLine(text);
		}
		return stringWriter.ToString();
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00041CA0 File Offset: 0x0003FEA0
	public List<BattleTriggerClip> GetAreaScheduleList(int iArea)
	{
		List<BattleTriggerClip> list = new List<BattleTriggerClip>();
		foreach (BattleTriggerClip battleTriggerClip in this.m_ClipList)
		{
			if (battleTriggerClip.m_iAreaID == iArea)
			{
				list.Add(battleTriggerClip);
			}
		}
		return list;
	}

	// Token: 0x04000617 RID: 1559
	public List<BattleTriggerClip> m_ClipList = new List<BattleTriggerClip>();
}
