using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000263 RID: 611
	public class TalentNewManager : TextDataManager
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x00008D2F File Offset: 0x00006F2F
		public TalentNewManager()
		{
			this.m_TalentDataNodeList = new List<TalentNewDataNode>();
			this.m_LoadFileName = "TalentNewData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00008D71 File Offset: 0x00006F71
		public static TalentNewManager Singleton
		{
			get
			{
				return TalentNewManager.instance;
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0005CB34 File Offset: 0x0005AD34
		protected override void LoadFile(string filePath)
		{
			this.m_TalentDataNodeList.Clear();
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
							string text2 = text.Replace("\r", string.Empty);
							string[] array3 = text2.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							TalentNewDataNode talentNewDataNode = new TalentNewDataNode();
							talentNewDataNode.m_iTalentID = int.Parse(array3[0]);
							talentNewDataNode.m_strTalentName = array3[1];
							talentNewDataNode.m_strTalentTip = array3[2];
							talentNewDataNode.m_strTalentImage = array3[3];
							int num = 4;
							while (num + 3 <= array3.Length)
							{
								int talentEffect;
								if (int.TryParse(array3[num], ref talentEffect))
								{
									if (int.TryParse(array3[num + 1], ref talentEffect))
									{
										if (int.TryParse(array3[num + 2], ref talentEffect))
										{
											TalentResultPart talentResultPart = new TalentResultPart();
											talentEffect = int.Parse(array3[num]);
											talentResultPart.m_TalentEffect = (TalentEffect)talentEffect;
											talentResultPart.iUpDown = int.Parse(array3[num + 1]);
											talentResultPart.iValue = int.Parse(array3[num + 2]);
											talentNewDataNode.m_cEffetPartList.Add(talentResultPart);
										}
									}
								}
								num += 3;
							}
							this.m_TalentDataNodeList.Add(talentNewDataNode);
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

		// Token: 0x06000B32 RID: 2866 RVA: 0x0005CD00 File Offset: 0x0005AF00
		public TalentNewDataNode GetTalentData(int iID)
		{
			foreach (TalentNewDataNode talentNewDataNode in this.m_TalentDataNodeList)
			{
				if (talentNewDataNode.m_iTalentID == iID)
				{
					return talentNewDataNode;
				}
			}
			return null;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0005CD6C File Offset: 0x0005AF6C
		public bool CheckTalemEffect(int iID, TalentEffect iType)
		{
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return false;
			}
			foreach (TalentResultPart talentResultPart in talentData.m_cEffetPartList)
			{
				if (talentResultPart.m_TalentEffect == iType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0005CDE8 File Offset: 0x0005AFE8
		public float GetTalentPercentValue(int iID, TalentEffect iType)
		{
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (TalentResultPart talentResultPart in talentData.m_cEffetPartList)
			{
				if (talentResultPart.m_TalentEffect == iType)
				{
					if (talentResultPart.iUpDown == 2 || talentResultPart.iUpDown == 3)
					{
						if (talentResultPart.iUpDown == 2)
						{
							num += 0.01f * (float)talentResultPart.iValue;
						}
						if (talentResultPart.iUpDown == 3)
						{
							num -= 0.01f * (float)talentResultPart.iValue;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0005CEBC File Offset: 0x0005B0BC
		public int GetTalentValue(int iID, TalentEffect iType)
		{
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return 0;
			}
			int num = 0;
			foreach (TalentResultPart talentResultPart in talentData.m_cEffetPartList)
			{
				if (talentResultPart.m_TalentEffect == iType)
				{
					if (talentResultPart.iUpDown == 0 || talentResultPart.iUpDown == 1)
					{
						if (talentResultPart.iUpDown == 0)
						{
							num += talentResultPart.iValue;
						}
						if (talentResultPart.iUpDown == 1)
						{
							num -= talentResultPart.iValue;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0005CF78 File Offset: 0x0005B178
		public List<TalentResultPart> GetTalnetResulePart(int iID, TalentEffect iType)
		{
			List<TalentResultPart> list = new List<TalentResultPart>();
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return list;
			}
			foreach (TalentResultPart talentResultPart in talentData.m_cEffetPartList)
			{
				if (talentResultPart.m_TalentEffect == iType)
				{
					list.Add(talentResultPart);
				}
			}
			return list;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0005CFFC File Offset: 0x0005B1FC
		public string GetTalentTip(int iID)
		{
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return string.Empty;
			}
			return talentData.m_strTalentTip;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0005D024 File Offset: 0x0005B224
		public string GetTalentName(int iID)
		{
			TalentNewDataNode talentData = this.GetTalentData(iID);
			if (talentData == null)
			{
				return string.Empty;
			}
			return talentData.m_strTalentName;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00008D78 File Offset: 0x00006F78
		public List<TalentNewDataNode> GetTalentList()
		{
			return this.m_TalentDataNodeList;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0005D04C File Offset: 0x0005B24C
		public void NotifyGame(TalentEffect Te)
		{
			if (Te == TalentEffect.Observant)
			{
				MouseEventCube.ResetAllMouseEvent();
			}
		}

		// Token: 0x04000D0F RID: 3343
		private static readonly TalentNewManager instance = new TalentNewManager();

		// Token: 0x04000D10 RID: 3344
		private List<TalentNewDataNode> m_TalentDataNodeList;
	}
}
