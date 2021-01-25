using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000220 RID: 544
	public class MissionLevelNode
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x000084CD File Offset: 0x000066CD
		public MissionLevelNode()
		{
			this.m_DisplayConditionList = new List<Condition>();
			this.m_EnterConditionList = new List<Condition>();
			this.m_CloseConditionList = new List<Condition>();
			this.m_ShaqList = new List<int>();
			this.m_ItemList = new List<int>();
		}

		// Token: 0x04000B42 RID: 2882
		public int iLevelID;

		// Token: 0x04000B43 RID: 2883
		public string strName;

		// Token: 0x04000B44 RID: 2884
		public int iBattleAreaID;

		// Token: 0x04000B45 RID: 2885
		public int iType;

		// Token: 0x04000B46 RID: 2886
		public int iGroup;

		// Token: 0x04000B47 RID: 2887
		public int iRepeat;

		// Token: 0x04000B48 RID: 2888
		public int iWeights;

		// Token: 0x04000B49 RID: 2889
		public List<Condition> m_DisplayConditionList;

		// Token: 0x04000B4A RID: 2890
		public List<Condition> m_EnterConditionList;

		// Token: 0x04000B4B RID: 2891
		public List<Condition> m_CloseConditionList;

		// Token: 0x04000B4C RID: 2892
		public int iRound;

		// Token: 0x04000B4D RID: 2893
		public int iMissionPosition;

		// Token: 0x04000B4E RID: 2894
		public string strDesc;

		// Token: 0x04000B4F RID: 2895
		public List<int> m_ShaqList;

		// Token: 0x04000B50 RID: 2896
		public int iExp;

		// Token: 0x04000B51 RID: 2897
		public int iFame;

		// Token: 0x04000B52 RID: 2898
		public int iMoney;

		// Token: 0x04000B53 RID: 2899
		public List<int> m_ItemList;

		// Token: 0x04000B54 RID: 2900
		public string strTalkID;

		// Token: 0x02000221 RID: 545
		public enum eMember
		{
			// Token: 0x04000B56 RID: 2902
			LevelID,
			// Token: 0x04000B57 RID: 2903
			MissionName,
			// Token: 0x04000B58 RID: 2904
			BattleAreaID,
			// Token: 0x04000B59 RID: 2905
			Type,
			// Token: 0x04000B5A RID: 2906
			Group,
			// Token: 0x04000B5B RID: 2907
			Repeat,
			// Token: 0x04000B5C RID: 2908
			Weights,
			// Token: 0x04000B5D RID: 2909
			DisplayCondition,
			// Token: 0x04000B5E RID: 2910
			EnterCondition,
			// Token: 0x04000B5F RID: 2911
			CloseCondition,
			// Token: 0x04000B60 RID: 2912
			Round,
			// Token: 0x04000B61 RID: 2913
			Position,
			// Token: 0x04000B62 RID: 2914
			Desc,
			// Token: 0x04000B63 RID: 2915
			Shaq,
			// Token: 0x04000B64 RID: 2916
			Exp,
			// Token: 0x04000B65 RID: 2917
			Fame,
			// Token: 0x04000B66 RID: 2918
			Money,
			// Token: 0x04000B67 RID: 2919
			Item,
			// Token: 0x04000B68 RID: 2920
			TalkID
		}
	}
}
