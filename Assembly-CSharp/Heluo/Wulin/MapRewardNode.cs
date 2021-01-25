using System;

namespace Heluo.Wulin
{
	// Token: 0x0200024C RID: 588
	public class MapRewardNode
	{
		// Token: 0x06000AE3 RID: 2787 RVA: 0x00002672 File Offset: 0x00000872
		public MapRewardNode()
		{
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0005A82C File Offset: 0x00058A2C
		public MapRewardNode(string[] args)
		{
			this._RewardType = (RewardType)int.Parse(args[0]);
			switch (this._RewardType)
			{
			case RewardType.PropertyType:
			case RewardType.AddRoutine:
			case RewardType.AddNeigong:
			case RewardType.ToBattle:
			case RewardType.AddItem:
			case RewardType.AddTeamMember:
			case RewardType.LessTeamMember:
			case RewardType.PlayMovie:
			case RewardType.AddTalent:
			case RewardType.PersonalExp:
			case RewardType.LessItem:
			case RewardType.EquipItem:
			case RewardType.AddDLCUnit:
			case RewardType.ChangeNpcID:
			case RewardType.DLCUnitLimit:
			case RewardType.StoreLimit:
			case RewardType.DLCInfoLimit:
			case RewardType.StoreRenewTurn:
				this.m_Parameter = int.Parse(args[1]);
				goto IL_CF;
			}
			this.m_Parameter = args[1];
			IL_CF:
			this.m_iAmount = int.Parse(args[2]);
			this.m_iMsgID = int.Parse(args[3]);
			string strGType = args[4].Replace("\r", string.Empty);
			this.m_strGType = strGType;
		}

		// Token: 0x04000C2D RID: 3117
		public RewardType _RewardType;

		// Token: 0x04000C2E RID: 3118
		public string m_strGType;

		// Token: 0x04000C2F RID: 3119
		public CParaValue m_Parameter;

		// Token: 0x04000C30 RID: 3120
		public int m_iAmount;

		// Token: 0x04000C31 RID: 3121
		public int m_iMsgID;
	}
}
