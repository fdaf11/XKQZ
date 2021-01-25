using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000224 RID: 548
	public class MissionPositionManager : TextDataManager
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x00008557 File Offset: 0x00006757
		public MissionPositionManager()
		{
			this.m_MissionPositionNodeList = new List<MissionPositionNode>();
			this.m_LoadFileName = "MissionPosition";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00008593 File Offset: 0x00006793
		public static MissionPositionManager Singleton
		{
			get
			{
				return MissionPositionManager.instance;
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000585A4 File Offset: 0x000567A4
		protected override void LoadFile(string filePath)
		{
			this.m_MissionPositionNodeList.Clear();
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
							string[] array3 = text.Trim().Split(new char[]
							{
								"\t".get_Chars(0)
							});
							MissionPositionNode missionPositionNode = new MissionPositionNode();
							missionPositionNode.iPositionID = int.Parse(array3[0]);
							missionPositionNode.iOffsetX = int.Parse(array3[1]);
							missionPositionNode.iOffsetY = int.Parse(array3[2]);
							this.m_MissionPositionNodeList.Add(missionPositionNode);
						}
						catch (Exception ex)
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
							Debug.Log(ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000586AC File Offset: 0x000568AC
		public MissionPositionNode GetMissionPositionNode(int iD)
		{
			for (int i = 0; i < this.m_MissionPositionNodeList.Count; i++)
			{
				if (iD == this.m_MissionPositionNodeList[i].iPositionID)
				{
					return this.m_MissionPositionNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000B6E RID: 2926
		private static readonly MissionPositionManager instance = new MissionPositionManager();

		// Token: 0x04000B6F RID: 2927
		private List<MissionPositionNode> m_MissionPositionNodeList;
	}
}
