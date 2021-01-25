using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200022E RID: 558
	public class NeigongUpValueManager : TextDataManager
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x000086BC File Offset: 0x000068BC
		public NeigongUpValueManager()
		{
			this.m_NeigongUpValueList = new List<NeigongUpValueNode>();
			this.m_LoadFileName = "NeigongUpValue";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x000086F8 File Offset: 0x000068F8
		public static NeigongUpValueManager Singleton
		{
			get
			{
				return NeigongUpValueManager.instance;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00058F5C File Offset: 0x0005715C
		protected override void LoadFile(string filePath)
		{
			this.m_NeigongUpValueList.Clear();
			string[] array = base.ExtractTextFile(filePath);
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
						try
						{
							string[] array3 = text.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							NeigongUpValueNode neigongUpValueNode = new NeigongUpValueNode();
							neigongUpValueNode.m_iID = int.Parse(array3[0]);
							for (int j = 1; j < array3.Length; j++)
							{
								UpValueNode upValueNode = new UpValueNode();
								string[] array4 = array3[j].Split(new char[]
								{
									",".get_Chars(0)
								});
								upValueNode.m_iAttack = int.Parse(array4[0]);
								upValueNode.m_iDefense = int.Parse(array4[1]);
								upValueNode.m_iHp = int.Parse(array4[2]);
								upValueNode.m_iMp = int.Parse(array4[3]);
								upValueNode.m_iDodge = int.Parse(array4[4]);
								upValueNode.m_iPoison = int.Parse(array4[5]);
								neigongUpValueNode.m_UpValueList.Add(upValueNode);
							}
							this.m_NeigongUpValueList.Add(neigongUpValueNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000590E0 File Offset: 0x000572E0
		public NeigongUpValueNode GetNeigongUpValueNode(int iID)
		{
			for (int i = 0; i < this.m_NeigongUpValueList.Count; i++)
			{
				if (this.m_NeigongUpValueList[i].m_iID == iID)
				{
					return this.m_NeigongUpValueList[i];
				}
			}
			return null;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0000264F File Offset: 0x0000084F
		public void SetAddValue(int iID, int iLv)
		{
		}

		// Token: 0x04000B99 RID: 2969
		private static readonly NeigongUpValueManager instance = new NeigongUpValueManager();

		// Token: 0x04000B9A RID: 2970
		private List<NeigongUpValueNode> m_NeigongUpValueList;
	}
}
