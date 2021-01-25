using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200021F RID: 543
	public class MapTalkManager : TextDataManager
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x00008484 File Offset: 0x00006684
		public MapTalkManager()
		{
			this.m_MapTalkTypeNodeList = new List<MapTalkTypeNode>();
			this.m_LoadFileName = "MapTalkManager";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000084C6 File Offset: 0x000066C6
		public static MapTalkManager Singleton
		{
			get
			{
				return MapTalkManager.instance;
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00057B9C File Offset: 0x00055D9C
		protected override void LoadFile(string filePath)
		{
			this.m_MapTalkTypeNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			string text = string.Empty;
			if (array == null)
			{
				return;
			}
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2) && text2.get_Chars(0) != '#')
				{
					try
					{
						string[] array3 = text2.Trim().Split(new char[]
						{
							"\t".get_Chars(0)
						});
						MapTalkTypeNode mapTalkTypeNode = this.ClickMapTalkTypeNode(array3[0]);
						MapTalkNode mapTalkNode = new MapTalkNode();
						mapTalkNode.m_iOrder = int.Parse(array3[1]);
						string[] array4 = array3[2].Split(new char[]
						{
							','
						});
						if (array4.Length > 1)
						{
							mapTalkNode.m_iNpcID = int.Parse(array4[0]);
							mapTalkNode.m_iNpcIDEX = int.Parse(array4[1]);
						}
						else
						{
							mapTalkNode.m_iNpcID = int.Parse(array3[2]);
						}
						int imageDirection = 0;
						if (!int.TryParse(array3[3], ref imageDirection))
						{
							imageDirection = 0;
						}
						mapTalkNode.m_ImageDirection = (MapTalkNode.eImageDirectionType)imageDirection;
						if (array3[4].Length > 1)
						{
							string text3 = array3[4].Replace(")*(", "*");
							text3 = text3.Substring(1, text3.Length - 2);
							string[] array5 = text3.Split(new char[]
							{
								"*".get_Chars(0)
							});
							for (int j = 0; j < array5.Length; j++)
							{
								Condition condition = new Condition(array5[j].Split(new char[]
								{
									",".get_Chars(0)
								}));
								mapTalkNode.m_MapTalkMapTalkConditionList.Add(condition);
							}
						}
						mapTalkNode.m_strNpcVoice = array3[5];
						mapTalkNode.m_strActionID = array3[6];
						string strManager = array3[7].Replace("<br>", "\n");
						mapTalkNode.m_strManager = strManager;
						if (array3[8] == "1")
						{
							mapTalkNode.m_bInFields = true;
						}
						else
						{
							mapTalkNode.m_bInFields = false;
						}
						if (mapTalkNode.m_bInFields)
						{
							for (int k = 9; k < array3.Length - 3; k += 3)
							{
								if (!array3[k].Equals("0"))
								{
									MapTalkButtonNode mapTalkButtonNode = new MapTalkButtonNode();
									mapTalkButtonNode.m_strButtonName = array3[k];
									mapTalkButtonNode.m_iButtonType = int.Parse(array3[k + 1]);
									mapTalkButtonNode.m_strBArg = array3[k + 2];
									mapTalkNode.m_MapTalkButtonNodeList.Add(mapTalkButtonNode);
								}
							}
						}
						string strNextQuestID = array3[21].Replace("\r", string.Empty);
						mapTalkNode.m_strNextQuestID = strNextQuestID;
						mapTalkNode.m_fDestroyTime = float.Parse(array3[22]);
						mapTalkNode.m_iGiftID = int.Parse(array3[23]);
						mapTalkTypeNode.m_MapTalkNodeList.Add(mapTalkNode);
						text = text2;
					}
					catch
					{
						Debug.LogError(string.Concat(new string[]
						{
							"解析 ",
							filePath,
							" 時發生錯誤 : ",
							text2,
							" 上一行 : ",
							text
						}));
					}
				}
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00057EB8 File Offset: 0x000560B8
		public MapTalkTypeNode ClickMapTalkTypeNode(string strID)
		{
			MapTalkTypeNode mapTalkTypeNode;
			if (this.m_MapTalkTypeNodeList.Count == 0)
			{
				mapTalkTypeNode = new MapTalkTypeNode();
				mapTalkTypeNode.m_strTalkGroupID = strID;
				this.m_MapTalkTypeNodeList.Add(mapTalkTypeNode);
				return mapTalkTypeNode;
			}
			for (int i = 0; i < this.m_MapTalkTypeNodeList.Count; i++)
			{
				if (this.m_MapTalkTypeNodeList[i].m_strTalkGroupID.Equals(strID))
				{
					return this.m_MapTalkTypeNodeList[i];
				}
			}
			mapTalkTypeNode = new MapTalkTypeNode();
			mapTalkTypeNode.m_strTalkGroupID = strID;
			this.m_MapTalkTypeNodeList.Add(mapTalkTypeNode);
			return mapTalkTypeNode;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00057F50 File Offset: 0x00056150
		public MapTalkTypeNode GetMapTalkTypeNode(string strTalkID)
		{
			for (int i = 0; i < this.m_MapTalkTypeNodeList.Count; i++)
			{
				if (this.m_MapTalkTypeNodeList[i].m_strTalkGroupID.Equals(strTalkID))
				{
					return this.m_MapTalkTypeNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00057FA4 File Offset: 0x000561A4
		public string GetTalkString(int movieID, int Step)
		{
			if (Step < 0)
			{
				return string.Empty;
			}
			MapTalkTypeNode mapTalkTypeNode = this.GetMapTalkTypeNode(movieID.ToString());
			if (mapTalkTypeNode == null)
			{
				Debug.Log(string.Concat(new string[]
				{
					"MovieTalkNode no fund movieID = ",
					movieID.ToString(),
					" Step = ",
					Step.ToString(),
					" plz Kuro Check"
				}));
				return string.Empty;
			}
			if (Step >= mapTalkTypeNode.m_MapTalkNodeList.Count)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Step out of range movieID = ",
					movieID.ToString(),
					" Step = ",
					Step.ToString(),
					" plz Kuro Check"
				}));
				return string.Empty;
			}
			MapTalkNode mapTalkNode = mapTalkTypeNode.m_MapTalkNodeList[Step];
			return mapTalkNode.m_strManager;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0005807C File Offset: 0x0005627C
		public int GetTalkNpcId(int movieID, int Step)
		{
			if (Step < 0)
			{
				return 0;
			}
			MapTalkTypeNode mapTalkTypeNode = this.GetMapTalkTypeNode(movieID.ToString());
			if (mapTalkTypeNode == null)
			{
				Debug.Log(string.Concat(new string[]
				{
					"MovieTalkNode no fund movieID = ",
					movieID.ToString(),
					" Step = ",
					Step.ToString(),
					" plz Kuro Check"
				}));
				return 0;
			}
			if (Step >= mapTalkTypeNode.m_MapTalkNodeList.Count)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Step out of range movieID = ",
					movieID.ToString(),
					" Step = ",
					Step.ToString(),
					" plz Kuro Check"
				}));
				return 0;
			}
			MapTalkNode mapTalkNode = mapTalkTypeNode.m_MapTalkNodeList[Step];
			return mapTalkNode.m_iNpcID;
		}

		// Token: 0x04000B40 RID: 2880
		private static readonly MapTalkManager instance = new MapTalkManager();

		// Token: 0x04000B41 RID: 2881
		private List<MapTalkTypeNode> m_MapTalkTypeNodeList;
	}
}
