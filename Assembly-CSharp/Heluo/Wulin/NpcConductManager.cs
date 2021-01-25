using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001CE RID: 462
	public class NpcConductManager
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x00007C38 File Offset: 0x00005E38
		public NpcConductManager()
		{
			this.m_NpcConductNodeList = new List<NpcConductNode>();
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00007C57 File Offset: 0x00005E57
		public static NpcConductManager Singleton
		{
			get
			{
				return NpcConductManager.instance;
			}
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0005132C File Offset: 0x0004F52C
		public void LoadNpcConductFile(string strMapNumber)
		{
			string text = "MapIcon/NpcConduct_" + strMapNumber;
			this.m_NpcConductNodeList.Clear();
			string[] array = Game.ExtractTextFile(text);
			if (array == null)
			{
				return;
			}
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					if (text2.get_Chars(0) != '#')
					{
						if (text2.get_Chars(0) != '\r')
						{
							try
							{
								string[] array3 = text2.Split(new char[]
								{
									"\t".get_Chars(0)
								});
								NpcConductNode npcConductNode = new NpcConductNode();
								npcConductNode.m_iNpcID = int.Parse(array3[0]);
								string text3 = array3[1].Replace(")*(", "*");
								text3 = text3.Substring(1, text3.Length - 2);
								string[] array4 = text3.Split(new char[]
								{
									"*".get_Chars(0)
								});
								for (int j = 0; j < array4.Length; j++)
								{
									string[] array5 = array4[j].Split(new char[]
									{
										",".get_Chars(0)
									});
									ConductNode conductNode = new ConductNode();
									conductNode.m_fStartTime = float.Parse(array5[0]);
									conductNode.m_fCloseTime = float.Parse(array5[1]);
									conductNode.m_strModelName = array5[2];
									conductNode.m_ConductID = int.Parse(array5[3]);
									npcConductNode.m_ConductNodeList.Add(conductNode);
								}
								npcConductNode.m_iType = 0;
								npcConductNode.m_iTimeType = 0;
								this.m_NpcConductNodeList.Add(npcConductNode);
							}
							catch
							{
								Debug.LogError("解析 " + text + " 時發生錯誤 : " + text2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00051510 File Offset: 0x0004F710
		public NpcConductNode GetNpcConductNode(int iID)
		{
			for (int i = 0; i < this.m_NpcConductNodeList.Count; i++)
			{
				if (this.m_NpcConductNodeList[i].m_iNpcID == iID)
				{
					return this.m_NpcConductNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000976 RID: 2422
		private static readonly NpcConductManager instance = new NpcConductManager();

		// Token: 0x04000977 RID: 2423
		public List<NpcConductNode> m_NpcConductNodeList;
	}
}
