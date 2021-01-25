using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000229 RID: 553
	public class NeigongNewDataNode
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x00008603 File Offset: 0x00006803
		public NeigongNewDataNode()
		{
			this.m_iConditionList = new List<int>();
			this.m_ConditionEffectList = new List<ConditionEffect>();
			this.m_LevelUP = new List<LevelUp>();
			this.m_MaxLevelUP = new List<LevelUp>();
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00008637 File Offset: 0x00006837
		public ConditionEffect GetConditionEffect(int iIdx)
		{
			return this.m_ConditionEffectList[iIdx];
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000589B0 File Offset: 0x00056BB0
		public bool ReplaceBattleConditionID(int origID, int newID)
		{
			for (int i = 0; i < this.m_ConditionEffectList.Count; i++)
			{
				if (this.m_ConditionEffectList[i].m_iBattleConditionID == origID)
				{
					this.m_ConditionEffectList[i].m_iBattleConditionID = newID;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00058A08 File Offset: 0x00056C08
		public void AddBattleConditionID(int newID)
		{
			bool flag = false;
			for (int i = 0; i < this.m_ConditionEffectList.Count; i++)
			{
				if (this.m_ConditionEffectList[i].m_iBattleConditionID == newID)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return;
			}
			ConditionEffect conditionEffect = new ConditionEffect();
			conditionEffect.bOpen = true;
			conditionEffect.m_iBattleConditionID = newID;
			this.m_ConditionEffectList.Add(conditionEffect);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00058A78 File Offset: 0x00056C78
		public void CheckConditionEffectOpen()
		{
			this.m_iConditionList.Clear();
			for (int i = 0; i < this.m_ConditionEffectList.Count; i++)
			{
				this.m_ConditionEffectList[i].bOpen = ConditionManager.CheckCondition(this.m_ConditionEffectList[i].m_ConditionList, true, 0, string.Empty);
				if (this.m_ConditionEffectList[i].bOpen && this.m_ConditionEffectList[i].m_iBattleConditionID != 0)
				{
					this.m_iConditionList.Add(this.m_ConditionEffectList[i].m_iBattleConditionID);
				}
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00058B24 File Offset: 0x00056D24
		public NeigongNewDataNode Clone()
		{
			NeigongNewDataNode neigongNewDataNode = new NeigongNewDataNode();
			neigongNewDataNode.m_iNeigongID = this.m_iNeigongID;
			neigongNewDataNode.m_strNeigongName = this.m_strNeigongName;
			neigongNewDataNode.m_strNeigongTip = this.m_strNeigongTip;
			neigongNewDataNode.m_strUpgradeNotes = this.m_strUpgradeNotes;
			neigongNewDataNode.m_LevelUP = this.m_LevelUP;
			neigongNewDataNode.m_MaxLevelUP = this.m_MaxLevelUP;
			neigongNewDataNode.m_iExpType = this.m_iExpType;
			neigongNewDataNode.sIconImage = this.sIconImage;
			foreach (ConditionEffect conditionEffect in this.m_ConditionEffectList)
			{
				neigongNewDataNode.m_ConditionEffectList.Add(conditionEffect.Clone());
			}
			return neigongNewDataNode;
		}

		// Token: 0x04000B79 RID: 2937
		public int m_iNeigongID;

		// Token: 0x04000B7A RID: 2938
		public string m_strNeigongName;

		// Token: 0x04000B7B RID: 2939
		public string m_strNeigongTip;

		// Token: 0x04000B7C RID: 2940
		public string m_strUpgradeNotes;

		// Token: 0x04000B7D RID: 2941
		public List<LevelUp> m_LevelUP;

		// Token: 0x04000B7E RID: 2942
		public List<LevelUp> m_MaxLevelUP;

		// Token: 0x04000B7F RID: 2943
		public int m_iExpType;

		// Token: 0x04000B80 RID: 2944
		public string sIconImage;

		// Token: 0x04000B81 RID: 2945
		public List<ConditionEffect> m_ConditionEffectList;

		// Token: 0x04000B82 RID: 2946
		public List<int> m_iConditionList;

		// Token: 0x0200022A RID: 554
		public enum eMember
		{
			// Token: 0x04000B84 RID: 2948
			NeigongID,
			// Token: 0x04000B85 RID: 2949
			NeigongName,
			// Token: 0x04000B86 RID: 2950
			NeigongTip,
			// Token: 0x04000B87 RID: 2951
			UpgradeNotes,
			// Token: 0x04000B88 RID: 2952
			LevelUP,
			// Token: 0x04000B89 RID: 2953
			MaxLevelUP,
			// Token: 0x04000B8A RID: 2954
			ExpType,
			// Token: 0x04000B8B RID: 2955
			IconImage,
			// Token: 0x04000B8C RID: 2956
			ConditionEffect,
			// Token: 0x04000B8D RID: 2957
			Condition
		}
	}
}
