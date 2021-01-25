using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200025A RID: 602
	public class SettingDataManager : TextDataManager
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0005C27C File Offset: 0x0005A47C
		public SettingDataManager()
		{
			this.m_SettingDataNodeList = new List<SettingDataNode>();
			this.m_LoadFileName = "SettingData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			this.m_SettingDataNodeList.Sort((SettingDataNode A, SettingDataNode B) => A.m_iQualityLevel.CompareTo(B.m_iQualityLevel));
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00008BF7 File Offset: 0x00006DF7
		public static SettingDataManager Singleton
		{
			get
			{
				return SettingDataManager.instance;
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0005C2E0 File Offset: 0x0005A4E0
		protected override void LoadFile(string filePath)
		{
			this.m_SettingDataNodeList.Clear();
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
							SettingDataNode settingDataNode = new SettingDataNode();
							settingDataNode.m_iQualityLevel = int.Parse(array3[0]);
							settingDataNode.m_bShadow = this.StrToBool(array3[2]);
							settingDataNode.m_fShadowDistance = float.Parse(array3[3]);
							settingDataNode.m_iVSync = int.Parse(array3[4]);
							settingDataNode.m_iAntiAliasing = int.Parse(array3[5]);
							settingDataNode.m_iImageQuality = int.Parse(array3[6]);
							settingDataNode.m_bSSAO = this.StrToBool(array3[7]);
							settingDataNode.m_bLensEffects = this.StrToBool(array3[8]);
							settingDataNode.m_bBloom = this.StrToBool(array3[9]);
							settingDataNode.m_iResourcesCount = int.Parse(array3[10]);
							this.m_SettingDataNodeList.Add(settingDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00008BFE File Offset: 0x00006DFE
		public bool StrToBool(string strInt)
		{
			return strInt == "1";
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0005C448 File Offset: 0x0005A648
		public SettingDataNode GetSettingDataNode(int qualityLevel)
		{
			for (int i = 0; i < this.m_SettingDataNodeList.Count; i++)
			{
				if (this.m_SettingDataNodeList[i].m_iQualityLevel == qualityLevel)
				{
					return this.m_SettingDataNodeList[i].Clone();
				}
			}
			return null;
		}

		// Token: 0x04000C95 RID: 3221
		private static readonly SettingDataManager instance = new SettingDataManager();

		// Token: 0x04000C96 RID: 3222
		private List<SettingDataNode> m_SettingDataNodeList;
	}
}
