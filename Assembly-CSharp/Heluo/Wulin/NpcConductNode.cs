using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001CD RID: 461
	public class NpcConductNode
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x00007C25 File Offset: 0x00005E25
		public NpcConductNode()
		{
			this.m_ConductNodeList = new List<ConductNode>();
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000512A4 File Offset: 0x0004F4A4
		public ConductNode GetConductNode(float fTime)
		{
			for (int i = 0; i < this.m_ConductNodeList.Count; i++)
			{
				ConductNode conductNode = this.m_ConductNodeList[i];
				float num;
				if (conductNode.m_fCloseTime < conductNode.m_fStartTime)
				{
					num = conductNode.m_fCloseTime + 24f;
				}
				else
				{
					num = conductNode.m_fCloseTime;
				}
				if (fTime >= conductNode.m_fStartTime && fTime < num)
				{
					return this.m_ConductNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000970 RID: 2416
		public int m_iNpcID;

		// Token: 0x04000971 RID: 2417
		public List<ConductNode> m_ConductNodeList;

		// Token: 0x04000972 RID: 2418
		public int m_iType;

		// Token: 0x04000973 RID: 2419
		public int m_iTimeType;

		// Token: 0x04000974 RID: 2420
		public GameObject m_go_Npc;

		// Token: 0x04000975 RID: 2421
		public float m_fClickTime;
	}
}
