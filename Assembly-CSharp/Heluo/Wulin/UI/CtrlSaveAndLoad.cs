using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F4 RID: 756
	public class CtrlSaveAndLoad
	{
		// Token: 0x0600100D RID: 4109 RVA: 0x000880C8 File Offset: 0x000862C8
		public void UpdateSaveLoadData(int title)
		{
			if (GameGlobal.m_bDLCMode)
			{
				if (title == 1)
				{
					this.saveLoadDateList = TeamStatus.m_Instance.m_DLCSaveTitleDataList;
				}
				else
				{
					this.saveLoadDateList = TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList;
				}
			}
			else if (title == 1)
			{
				this.saveLoadDateList = TeamStatus.m_Instance.m_SaveTitleDataList;
			}
			else
			{
				this.saveLoadDateList = TeamStatus.m_Instance.m_AutoSaveTitleDataList;
			}
			this.SetSaveLoadData();
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00088144 File Offset: 0x00086344
		public void SetTipData(int index)
		{
			if (this.saveLoadDateList[index].m_bHaveData)
			{
				string strMissionID = this.saveLoadDateList[index].m_strMissionID;
				QuestNode questNode = Game.QuestData.GetQuestNode(strMissionID);
				if (questNode != null)
				{
					this.setTipView.Invoke(questNode.m_strQuestName, questNode.m_strQuestTip, this.saveLoadDateList[index].m_Texture);
				}
				return;
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x000881BC File Offset: 0x000863BC
		public void SetSaveLoadData()
		{
			for (int i = 0; i < this.saveLoadDateList.Count; i++)
			{
				bool bHaveData = this.saveLoadDateList[i].m_bHaveData;
				this.setViewActive.Invoke(i, bHaveData);
				if (bHaveData)
				{
					string strMissionID = this.saveLoadDateList[i].m_strMissionID;
					QuestNode questNode = Game.QuestData.GetQuestNode(strMissionID);
					string text = Game.MapID.GetMapName(this.saveLoadDateList[i].m_strPlaceName);
					string strTrueYear = this.saveLoadDateList[i].m_strTrueYear;
					string @string = Game.StringTable.GetString(100304);
					string text2 = @string + this.saveLoadDateList[i].m_PlayGameTime.TimeFormt();
					string text3;
					if (questNode == null)
					{
						GameDebugTool.Log("沒有這個任務ID" + strMissionID);
						text3 = string.Empty;
					}
					else
					{
						text3 = questNode.m_strQuestName;
					}
					if (GameGlobal.m_bDLCMode)
					{
						MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(strMissionID);
						if (missionLevelNode != null)
						{
							text3 = missionLevelNode.strName;
						}
						text = Game.StringTable.GetString(110305);
					}
					if (text == null)
					{
						GameDebugTool.Log("沒有這個MapID");
						text = string.Empty;
					}
					this.setSaveLoadView.Invoke(i, new string[]
					{
						text3,
						text,
						strTrueYear,
						text2
					});
				}
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0000A9D3 File Offset: 0x00008BD3
		public bool HaveData(int index)
		{
			return this.saveLoadDateList[index].m_bHaveData;
		}

		// Token: 0x04001303 RID: 4867
		private List<SaveTitleDataNode> saveLoadDateList;

		// Token: 0x04001304 RID: 4868
		public Action<int, bool> setViewActive;

		// Token: 0x04001305 RID: 4869
		public Action<int, string[]> setSaveLoadView;

		// Token: 0x04001306 RID: 4870
		public Action<string, string, Texture2D> setTipView;
	}
}
