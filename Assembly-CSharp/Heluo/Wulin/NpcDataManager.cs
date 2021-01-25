using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000233 RID: 563
	public class NpcDataManager : TextDataManager
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x00008763 File Offset: 0x00006963
		public NpcDataManager()
		{
			this.m_NpcDataNodeList = new List<NpcDataNode>();
			this.m_NpcFriendlyNodeList = new List<NpcFriendlyNode>();
			this.m_LoadFileName = "NpcData";
			this.LoadFile("NpcData");
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x000087AF File Offset: 0x000069AF
		public static NpcDataManager Singleton
		{
			get
			{
				return NpcDataManager.instance;
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0005929C File Offset: 0x0005749C
		protected override void LoadFile(string filePath)
		{
			this.m_NpcDataNodeList.Clear();
			this.m_NpcFriendlyNodeList.Clear();
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
							NpcDataNode npcDataNode = new NpcDataNode();
							for (int j = 0; j < array3.Length; j++)
							{
								this.generateData(j, array3[j], npcDataNode);
							}
							this.m_NpcDataNodeList.Add(npcDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0005939C File Offset: 0x0005759C
		private void generateData(int idx, string data, NpcDataNode node)
		{
			switch (idx)
			{
			case 0:
			{
				int iNpcID = 0;
				if (!int.TryParse(data, ref iNpcID))
				{
					iNpcID = 0;
				}
				node.m_iNpcID = iNpcID;
				break;
			}
			case 1:
				node.m_str3DModel = data;
				break;
			case 2:
				node.m_strBigHeadImage = data;
				break;
			case 3:
				node.m_strHalfImage = data;
				break;
			case 4:
				node.m_strMemberImage = data;
				break;
			case 5:
				node.m_strSmallImage = data;
				break;
			case 6:
				if (data == "1")
				{
					NpcFriendlyNode npcFriendlyNode = new NpcFriendlyNode();
					npcFriendlyNode.m_iNpcID = node.m_iNpcID;
					npcFriendlyNode.m_iFriendly = -1;
					this.m_NpcFriendlyNodeList.Add(npcFriendlyNode);
				}
				break;
			case 7:
				node.m_strNpcName = data;
				break;
			case 8:
				node.m_strDescription = data;
				break;
			case 9:
			{
				int gender = 0;
				if (!int.TryParse(data, ref gender))
				{
					gender = 0;
				}
				node.m_Gender = (GenderType)gender;
				break;
			}
			case 10:
			{
				int iMartialType = 0;
				if (!int.TryParse(data, ref iMartialType))
				{
					iMartialType = 0;
				}
				node.m_iMartialType = iMartialType;
				break;
			}
			case 11:
				node.m_strTitle = data;
				break;
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000594E0 File Offset: 0x000576E0
		public NpcDataNode GetNpcData(int iNpcID)
		{
			for (int i = 0; i < this.m_NpcDataNodeList.Count; i++)
			{
				if (this.m_NpcDataNodeList[i].m_iNpcID == iNpcID)
				{
					return this.m_NpcDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000264F File Offset: 0x0000084F
		public void SetNpcFriendly(int iID, int iAmount)
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0000264F File Offset: 0x0000084F
		private void CheckNpcOpen(int iID, int iFriendly)
		{
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00059530 File Offset: 0x00057730
		public int GetNpcFriendly(int iID)
		{
			for (int i = 0; i < this.m_NpcFriendlyNodeList.Count; i++)
			{
				if (this.m_NpcFriendlyNodeList[i].m_iNpcID == iID)
				{
					return this.m_NpcFriendlyNodeList[i].m_iFriendly;
				}
			}
			return 0;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00059584 File Offset: 0x00057784
		public string GetNpcName(int iID)
		{
			for (int i = 0; i < this.m_NpcDataNodeList.Count; i++)
			{
				if (this.m_NpcDataNodeList[i].m_iNpcID == iID)
				{
					return this.m_NpcDataNodeList[i].m_strNpcName;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000087B6 File Offset: 0x000069B6
		public int GetNpcCount()
		{
			return this.m_NpcDataNodeList.Count;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000087C3 File Offset: 0x000069C3
		public NpcDataNode GetNpcNodePos(int Pos)
		{
			return this.m_NpcDataNodeList[Pos];
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000595DC File Offset: 0x000577DC
		public string GetImageName(int iID)
		{
			for (int i = 0; i < this.m_NpcDataNodeList.Count; i++)
			{
				if (this.m_NpcDataNodeList[i].m_iNpcID == iID)
				{
					return this.m_NpcDataNodeList[i].m_strHalfImage;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00059634 File Offset: 0x00057834
		public string GetBigHeadName(int iID)
		{
			for (int i = 0; i < this.m_NpcDataNodeList.Count; i++)
			{
				if (this.m_NpcDataNodeList[i].m_iNpcID == iID)
				{
					return this.m_NpcDataNodeList[i].m_strBigHeadImage;
				}
			}
			return string.Empty;
		}

		// Token: 0x04000BAE RID: 2990
		private static readonly NpcDataManager instance = new NpcDataManager();

		// Token: 0x04000BAF RID: 2991
		private List<NpcDataNode> m_NpcDataNodeList;

		// Token: 0x04000BB0 RID: 2992
		private List<NpcFriendlyNode> m_NpcFriendlyNodeList;

		// Token: 0x02000234 RID: 564
		private enum eMember
		{
			// Token: 0x04000BB2 RID: 2994
			ID,
			// Token: 0x04000BB3 RID: 2995
			_3DModel,
			// Token: 0x04000BB4 RID: 2996
			BigHeadImage,
			// Token: 0x04000BB5 RID: 2997
			HalfImage,
			// Token: 0x04000BB6 RID: 2998
			MemberImage,
			// Token: 0x04000BB7 RID: 2999
			SmallImage,
			// Token: 0x04000BB8 RID: 3000
			isFriendly,
			// Token: 0x04000BB9 RID: 3001
			NpcName,
			// Token: 0x04000BBA RID: 3002
			Introduction,
			// Token: 0x04000BBB RID: 3003
			GenderType,
			// Token: 0x04000BBC RID: 3004
			MartialType,
			// Token: 0x04000BBD RID: 3005
			Title,
			// Token: 0x04000BBE RID: 3006
			AffterNpcID
		}
	}
}
