using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020002CC RID: 716
	public class SaveGameDataNode
	{
		// Token: 0x04001074 RID: 4212
		public string m_strVersion;

		// Token: 0x04001075 RID: 4213
		public float m_fNowX;

		// Token: 0x04001076 RID: 4214
		public float m_fNowY;

		// Token: 0x04001077 RID: 4215
		public float m_fNowZ;

		// Token: 0x04001078 RID: 4216
		public float m_fDir;

		// Token: 0x04001079 RID: 4217
		public int m_iMoney;

		// Token: 0x0400107A RID: 4218
		public int m_iAttributePoints;

		// Token: 0x0400107B RID: 4219
		public string m_strSceneName;

		// Token: 0x0400107C RID: 4220
		public int m_iRound;

		// Token: 0x0400107D RID: 4221
		public int m_iDay;

		// Token: 0x0400107E RID: 4222
		public List<int> m_TeammateList = new List<int>();

		// Token: 0x0400107F RID: 4223
		public List<DLCUnitInfo> m_UnitInfoList = new List<DLCUnitInfo>();

		// Token: 0x04001080 RID: 4224
		public List<CharacterData> m_NpcList = new List<CharacterData>();

		// Token: 0x04001081 RID: 4225
		public List<LevelTurnNode> m_NowLevelList = new List<LevelTurnNode>();

		// Token: 0x04001082 RID: 4226
		public List<LevelTurnNode> m_FinishLevelList = new List<LevelTurnNode>();

		// Token: 0x04001083 RID: 4227
		public List<BackpackNewDataNode> m_BackpackList = new List<BackpackNewDataNode>();

		// Token: 0x04001084 RID: 4228
		public List<int> m_EventList = new List<int>();

		// Token: 0x04001085 RID: 4229
		public Dictionary<string, int> Variable = new Dictionary<string, int>();

		// Token: 0x04001086 RID: 4230
		public List<NpcIsFought> m_NpcIsFoughtList = new List<NpcIsFought>();

		// Token: 0x04001087 RID: 4231
		public List<QuestStatus> m_QuestList = new List<QuestStatus>();

		// Token: 0x04001088 RID: 4232
		public List<TimeQuestStatus> m_TimeQuestList = new List<TimeQuestStatus>();

		// Token: 0x04001089 RID: 4233
		public List<StoreDataNode> m_StroeChangedList = new List<StoreDataNode>();

		// Token: 0x0400108A RID: 4234
		public List<string> m_TreasureBoxIDList = new List<string>();

		// Token: 0x0400108B RID: 4235
		public List<string> m_GlobalNpcQuest = new List<string>();

		// Token: 0x0400108C RID: 4236
		public int m_iBattleArea;

		// Token: 0x0400108D RID: 4237
		public int m_iBigMapModel;

		// Token: 0x0400108E RID: 4238
		public int m_iAutomaticIndex;

		// Token: 0x0400108F RID: 4239
		public bool m_bHouseInside;

		// Token: 0x04001090 RID: 4240
		public string m_strHouseName;

		// Token: 0x04001091 RID: 4241
		public bool m_AffterMode;

		// Token: 0x04001092 RID: 4242
		public float m_fInSideAbLi_r;

		// Token: 0x04001093 RID: 4243
		public float m_fInSideAbLi_g;

		// Token: 0x04001094 RID: 4244
		public float m_fInSideAbLi_b;

		// Token: 0x04001095 RID: 4245
		public float m_fInSideAbLi_a;

		// Token: 0x04001096 RID: 4246
		public int m_iBattleDifficulty;

		// Token: 0x04001097 RID: 4247
		public CameraSaveDateNode _CameraSaveDateNode = new CameraSaveDateNode();

		// Token: 0x04001098 RID: 4248
		public List<int> m_wtfnpc = new List<int>();

		// Token: 0x04001099 RID: 4249
		public List<string> m_SaveVersionFix = new List<string>();

		// Token: 0x0400109A RID: 4250
		public List<SaveRumor> m_SaveRumor = new List<SaveRumor>();

		// Token: 0x0400109B RID: 4251
		public DLCShopInfo m_DLCShopInfo;

		// Token: 0x0400109C RID: 4252
		public int m_iPrestigePoints;

		// Token: 0x0400109D RID: 4253
		public int m_iDLCUnitLimit;

		// Token: 0x0400109E RID: 4254
		public int m_iDLCInfoLimit;

		// Token: 0x0400109F RID: 4255
		public int m_iDLCInfoRemain;

		// Token: 0x040010A0 RID: 4256
		public int m_iDLCStoreLimit;

		// Token: 0x040010A1 RID: 4257
		public int m_iDLCStoreRenewTurn;

		// Token: 0x040010A2 RID: 4258
		public int mod_Difficulty;

		// Token: 0x040010A3 RID: 4259
		public Dictionary<int, List<ItmeEffectNode>> mod_EquipDic = new Dictionary<int, List<ItmeEffectNode>>();
	}
}
