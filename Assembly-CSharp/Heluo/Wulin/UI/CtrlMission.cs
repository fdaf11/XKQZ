using System;
using System.Collections.Generic;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F3 RID: 755
	public class CtrlMission
	{
		// Token: 0x06001006 RID: 4102 RVA: 0x00087F60 File Offset: 0x00086160
		public void SetMissionData(int type)
		{
			this.resetMission.Invoke();
			this.resetUIMission.Invoke();
			this.sortList = MissionStatus.m_instance.ReturnQuest(type);
			this.complementAmount.Invoke(this.sortList.Count);
			for (int i = 0; i < this.sortList.Count; i++)
			{
				QuestNode questNode = this.sortList[i];
				QuestStatus questStatusNode = MissionStatus.m_instance.GetQuestStatusNode(this.sortList[i].m_strQuestID);
				this.setMissionView.Invoke(i, questNode.m_strQuestName, questStatusNode.bfinish);
			}
			this.setMouseActive.Invoke(this.sortList.Count, this.maxAmount, true);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00088024 File Offset: 0x00086224
		public void SetTipData(int index)
		{
			QuestNode questNode = this.sortList[index];
			this.clickID = questNode.m_strQuestID;
			this.setTipView.Invoke(questNode.m_strQuestName, questNode.m_strQuestTip);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00088064 File Offset: 0x00086264
		public void SetRecordData()
		{
			if (this.clickID != null)
			{
				for (int i = 0; i < this.sortList.Count; i++)
				{
					QuestNode questNode = this.sortList[i];
					if (questNode.m_strQuestID == this.clickID)
					{
						this.setRecordView.Invoke(i);
					}
				}
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0000A996 File Offset: 0x00008B96
		public void CloseView()
		{
			MissionStatus.m_instance.setNewQuestCount(0);
			Game.UI.Get<UIMainSelect>().SetMissionAmount(0);
			this.resetUIMission.Invoke();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0000A9BE File Offset: 0x00008BBE
		public int GetSortCount()
		{
			return this.sortList.Count;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0000A9CB File Offset: 0x00008BCB
		public int GetMaxAmount()
		{
			return this.maxAmount;
		}

		// Token: 0x040012F9 RID: 4857
		private List<QuestNode> sortList;

		// Token: 0x040012FA RID: 4858
		private string clickID;

		// Token: 0x040012FB RID: 4859
		private int maxAmount = 18;

		// Token: 0x040012FC RID: 4860
		public Action<int> complementAmount;

		// Token: 0x040012FD RID: 4861
		public Action<int, string, bool> setMissionView;

		// Token: 0x040012FE RID: 4862
		public Action resetMission;

		// Token: 0x040012FF RID: 4863
		public Action<string, string> setTipView;

		// Token: 0x04001300 RID: 4864
		public Action resetUIMission;

		// Token: 0x04001301 RID: 4865
		public Action<int> setRecordView;

		// Token: 0x04001302 RID: 4866
		public Action<int, int, bool> setMouseActive;
	}
}
