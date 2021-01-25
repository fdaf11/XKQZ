using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200026C RID: 620
	public class UpgradeManager : TextDataManager
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x00008ED1 File Offset: 0x000070D1
		public UpgradeManager()
		{
			this.m_UpgradeNodeList = new List<UpgradeNode>();
			this.m_LoadFileName = "Upgrade";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00008F0D File Offset: 0x0000710D
		public static UpgradeManager Singleton
		{
			get
			{
				return UpgradeManager.instance;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0005D8E4 File Offset: 0x0005BAE4
		protected override void LoadFile(string filePath)
		{
			this.m_UpgradeNodeList.Clear();
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
						if (text.get_Chars(0) != '\r')
						{
							try
							{
								string[] array3 = text.Trim().Split(new char[]
								{
									"\t".get_Chars(0)
								});
								UpgradeNode upgradeNode = new UpgradeNode();
								upgradeNode.iCharID = int.Parse(array3[0]);
								string[] array4 = array3[1].Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text2 in array4)
								{
									int num = int.Parse(text2);
									upgradeNode.m_HidePassiveNodeList.Add(num);
								}
								PassiveTreeNode.GenerateList(upgradeNode.m_PassiveTreeNodeList, array3[2]);
								this.m_UpgradeNodeList.Add(upgradeNode);
							}
							catch (Exception ex)
							{
								Debug.LogError(string.Concat(new string[]
								{
									"解析 ",
									filePath,
									" 時發生錯誤 : ",
									text,
									"   "
								}));
								Debug.LogError(ex.Message);
							}
						}
					}
				}
			}
			this.m_UpgradeNodeList.Sort((UpgradeNode A, UpgradeNode B) => A.iCharID.CompareTo(B.iCharID));
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0005DA8C File Offset: 0x0005BC8C
		public UpgradeNode GetUpgradeNode(int iCharID)
		{
			for (int i = 0; i < this.m_UpgradeNodeList.Count; i++)
			{
				if (this.m_UpgradeNodeList[i].iCharID == iCharID)
				{
					return this.m_UpgradeNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000D31 RID: 3377
		private static readonly UpgradeManager instance = new UpgradeManager();

		// Token: 0x04000D32 RID: 3378
		private List<UpgradeNode> m_UpgradeNodeList;
	}
}
