using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000251 RID: 593
	public class RoutineExpManager : TextDataManager
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x00008AA9 File Offset: 0x00006CA9
		public RoutineExpManager()
		{
			this.m_RoutineExpNodeList = new List<RoutineExpNode>();
			this.m_LoadFileName = "RoutineExp";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00008AEB File Offset: 0x00006CEB
		public static RoutineExpManager Singleton
		{
			get
			{
				return RoutineExpManager.instance;
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0005B798 File Offset: 0x00059998
		protected override void LoadFile(string filePath)
		{
			this.m_RoutineExpNodeList.Clear();
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
							RoutineExpNode routineExpNode = new RoutineExpNode();
							routineExpNode.m_iExpType = int.Parse(array3[0]);
							routineExpNode.m_iLV2Exp = int.Parse(array3[1]);
							routineExpNode.m_iLV3Exp = int.Parse(array3[2]);
							routineExpNode.m_iLV4Exp = int.Parse(array3[3]);
							routineExpNode.m_iLV5Exp = int.Parse(array3[4]);
							routineExpNode.m_iLV6Exp = int.Parse(array3[5]);
							routineExpNode.m_iLV7Exp = int.Parse(array3[6]);
							routineExpNode.m_iLV8Exp = int.Parse(array3[7]);
							routineExpNode.m_iLV9Exp = int.Parse(array3[8]);
							routineExpNode.m_iLV10Exp = int.Parse(array3[9]);
							this.m_RoutineExpNodeList.Add(routineExpNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0005B8F8 File Offset: 0x00059AF8
		public RoutineExpNode GetRoutineExp(int iType)
		{
			for (int i = 0; i < this.m_RoutineExpNodeList.Count; i++)
			{
				if (this.m_RoutineExpNodeList[i].m_iExpType == iType)
				{
					return this.m_RoutineExpNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000C63 RID: 3171
		private static readonly RoutineExpManager instance = new RoutineExpManager();

		// Token: 0x04000C64 RID: 3172
		public List<RoutineExpNode> m_RoutineExpNodeList;
	}
}
