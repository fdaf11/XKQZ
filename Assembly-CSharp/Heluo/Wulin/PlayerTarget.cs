using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000274 RID: 628
	public class PlayerTarget : MonoBehaviour
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x00008FEC File Offset: 0x000071EC
		protected void AddBattleNpcList(PlayerTarget ta)
		{
			if (PlayerTarget.BattleNPClist.Contains(ta))
			{
				return;
			}
			PlayerTarget.BattleNPClist.Add(ta);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0000900A File Offset: 0x0000720A
		protected void RemoveBattleNpcList(PlayerTarget ta)
		{
			if (PlayerTarget.BattleNPClist.Contains(ta))
			{
				PlayerTarget.BattleNPClist.Remove(ta);
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00009028 File Offset: 0x00007228
		protected void AddPlayerTargetList(PlayerTarget ta)
		{
			if (PlayerTarget.PlayerTargetList.Contains(ta))
			{
				return;
			}
			PlayerTarget.PlayerTargetList.Add(ta);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00009046 File Offset: 0x00007246
		protected void RemovePlayerTargetList(PlayerTarget ta)
		{
			if (PlayerTarget.PlayerTargetList.Contains(ta))
			{
				PlayerTarget.PlayerTargetList.Remove(ta);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00009064 File Offset: 0x00007264
		private void OnDestroy()
		{
			this.RemovePlayerTargetList(this);
			this.RemoveBattleNpcList(this);
		}

		// Token: 0x04000D51 RID: 3409
		[HideInInspector]
		public PlayerTarget.eTargetType m_TargetType;

		// Token: 0x04000D52 RID: 3410
		public static List<PlayerTarget> PlayerTargetList = new List<PlayerTarget>();

		// Token: 0x04000D53 RID: 3411
		public static List<PlayerTarget> BattleNPClist = new List<PlayerTarget>();

		// Token: 0x02000275 RID: 629
		public enum eTargetType
		{
			// Token: 0x04000D55 RID: 3413
			BigMapNode,
			// Token: 0x04000D56 RID: 3414
			NpcCollider,
			// Token: 0x04000D57 RID: 3415
			MouseEventCube,
			// Token: 0x04000D58 RID: 3416
			SmallGame,
			// Token: 0x04000D59 RID: 3417
			TreasureBox
		}
	}
}
