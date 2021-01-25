using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000269 RID: 617
	public class TreasureBoxManager : TextDataManager
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x00008E49 File Offset: 0x00007049
		public TreasureBoxManager()
		{
			this.m_TreasureBoxGroupList = new List<TreasureBoxGroup>();
			this.m_LoadFileName = "TreasureBox";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00008E85 File Offset: 0x00007085
		public static TreasureBoxManager Singleton
		{
			get
			{
				return TreasureBoxManager.instance;
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0005D54C File Offset: 0x0005B74C
		protected override void LoadFile(string filePath)
		{
			this.m_TreasureBoxGroupList.Clear();
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
							TreasureBoxGroup treasureBoxGroup = this.ClickStoreDataNode(array3[0]);
							TreasureBoxNode treasureBoxNode = new TreasureBoxNode();
							treasureBoxNode.m_strBoxID = array3[1];
							treasureBoxNode.m_ModelName = array3[2];
							if (array3[3].Length > 1)
							{
								string[] array4 = array3[3].Split(new char[]
								{
									",".get_Chars(0)
								});
								treasureBoxNode.m_fPosX = float.Parse(array4[0]);
								treasureBoxNode.m_fPosY = float.Parse(array4[1]);
								treasureBoxNode.m_fPosZ = float.Parse(array4[2]);
							}
							if (array3[4].Length > 1)
							{
								string[] array5 = array3[4].Split(new char[]
								{
									",".get_Chars(0)
								});
								treasureBoxNode.m_fRotX = float.Parse(array5[0]);
								treasureBoxNode.m_fRotY = float.Parse(array5[1]);
								treasureBoxNode.m_fRotZ = float.Parse(array5[2]);
							}
							treasureBoxNode.m_iBoxType = int.Parse(array3[5]);
							treasureBoxNode.m_strQuestID = array3[6];
							treasureBoxNode.m_iItemID = int.Parse(array3[7]);
							treasureBoxNode.m_iMoveCount = int.Parse(array3[8]);
							treasureBoxNode.m_iLevel = int.Parse(array3[9]);
							string text2 = array3[10].Replace("\r", string.Empty);
							treasureBoxNode.m_iRewardID = int.Parse(text2);
							treasureBoxGroup.m_SceneTBList.Add(treasureBoxNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00008E8C File Offset: 0x0000708C
		public TreasureBoxNode GetTreasureBoxNode(string strID)
		{
			return null;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0005D764 File Offset: 0x0005B964
		public TreasureBoxNode GetTreasureBox(string strSceneID, int index)
		{
			List<TreasureBoxNode> treasureBoxList = this.GetTreasureBoxList(strSceneID);
			return treasureBoxList[index];
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0005D780 File Offset: 0x0005B980
		public List<TreasureBoxNode> GetTreasureBoxList(string strSceneID)
		{
			for (int i = 0; i < this.m_TreasureBoxGroupList.Count; i++)
			{
				if (this.m_TreasureBoxGroupList[i].m_strSceneID.Equals(strSceneID))
				{
					return this.m_TreasureBoxGroupList[i].m_SceneTBList;
				}
			}
			return null;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0005D7D8 File Offset: 0x0005B9D8
		private TreasureBoxGroup ClickStoreDataNode(string strSceneID)
		{
			for (int i = 0; i < this.m_TreasureBoxGroupList.Count; i++)
			{
				if (this.m_TreasureBoxGroupList[i].m_strSceneID == strSceneID)
				{
					return this.m_TreasureBoxGroupList[i];
				}
			}
			TreasureBoxGroup treasureBoxGroup = new TreasureBoxGroup();
			treasureBoxGroup.m_strSceneID = strSceneID;
			this.m_TreasureBoxGroupList.Add(treasureBoxGroup);
			return treasureBoxGroup;
		}

		// Token: 0x04000D2A RID: 3370
		private static readonly TreasureBoxManager instance = new TreasureBoxManager();

		// Token: 0x04000D2B RID: 3371
		private List<TreasureBoxGroup> m_TreasureBoxGroupList;
	}
}
