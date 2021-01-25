using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001C1 RID: 449
	public class MapIconManager
	{
		// Token: 0x06000953 RID: 2387 RVA: 0x00007AC5 File Offset: 0x00005CC5
		public MapIconManager()
		{
			this.m_MapNpcDataList = new List<MapNpcDataNode>();
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00007AE4 File Offset: 0x00005CE4
		public static MapIconManager Singleton
		{
			get
			{
				return MapIconManager.instance;
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00050854 File Offset: 0x0004EA54
		public void LoadMapIconFile(string strMapNumber)
		{
			this.m_MapNpcDataList.Clear();
			string text = "MapIcon/Map_icon_" + strMapNumber;
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
								MapNpcDataNode mapNpcDataNode = new MapNpcDataNode();
								mapNpcDataNode.m_iNpcID = int.Parse(array3[1]);
								string text3 = array3[2].Replace(")*(", "*");
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
									PointNode pointNode = new PointNode();
									pointNode.m_fX = float.Parse(array5[0]);
									pointNode.m_fY = float.Parse(array5[1]);
									pointNode.m_fZ = float.Parse(array5[2]);
									pointNode.m_fPointTime = float.Parse(array5[3]);
									pointNode.m_strPointAni = array5[4];
									pointNode.m_iDirAngle = int.Parse(array5[5]);
									pointNode.m_strMoodID = array5[6];
									mapNpcDataNode.m_PointNodeList.Add(pointNode);
								}
								mapNpcDataNode.m_iNpcType = int.Parse(array3[3]);
								if (array3[4].Length > 1)
								{
									Condition.GenerateList(mapNpcDataNode.m_ShowSCondition, array3[4]);
								}
								if (array3[5].Length > 1)
								{
									text3 = array3[5].Replace(")*(", "*");
									text3 = text3.Substring(1, text3.Length - 2);
									array4 = text3.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int k = 0; k < array4.Length; k++)
									{
										string[] paraForMapIcon = array4[k].Split(new char[]
										{
											",".get_Chars(0)
										});
										Condition condition = new Condition();
										condition.SetParaForMapIcon(paraForMapIcon);
										mapNpcDataNode.m_CloseSCondition.Add(condition);
									}
								}
								mapNpcDataNode.m_iMoveType = int.Parse(array3[6]);
								mapNpcDataNode.m_strPointTalkID = array3[7];
								mapNpcDataNode.m_strMoveTalkID = array3[8];
								if (array3[9].Length > 1)
								{
									text3 = array3[9].Replace(")*(", "*");
									text3 = text3.Substring(1, text3.Length - 2);
									array4 = text3.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int k = 0; k < array4.Length; k++)
									{
										string[] array6 = array4[k].Split(new char[]
										{
											",".get_Chars(0)
										});
										SpecialTalkNode specialTalkNode = new SpecialTalkNode();
										specialTalkNode.m_Type = (SpecialTalkNode.SpecialType)int.Parse(array6[0]);
										specialTalkNode.m_strValue = array6[1];
										specialTalkNode.m_strMsgID = array6[2];
										mapNpcDataNode.m_SpecialTalkNodeList.Add(specialTalkNode);
									}
								}
								mapNpcDataNode.m_iTalkSteer = int.Parse(array3[10]);
								mapNpcDataNode.m_iStoreID = int.Parse(array3[11]);
								mapNpcDataNode.m_iMaxTalk = int.Parse(array3[12]);
								if (array3[13].Length > 1)
								{
									string[] array7 = array3[13].Split(new char[]
									{
										",".get_Chars(0)
									});
									mapNpcDataNode.m_iCheakPlayer = int.Parse(array7[0]);
									mapNpcDataNode.m_strCheakPlayerTalkID = array7[1];
								}
								else
								{
									mapNpcDataNode.m_iCheakPlayer = 0;
								}
								string text4 = array3[14].Replace("\r", string.Empty);
								if (text4.Length > 1)
								{
									string[] array8 = text4.Split(new char[]
									{
										",".get_Chars(0)
									});
									if (int.Parse(array8[0]) == 1)
									{
										mapNpcDataNode.m_bIsBattle = true;
									}
									else
									{
										mapNpcDataNode.m_bIsBattle = false;
									}
									mapNpcDataNode.m_iBattleArea = int.Parse(array8[1]);
									mapNpcDataNode.m_iStayAffterBattle = int.Parse(array8[2]);
								}
								else
								{
									mapNpcDataNode.m_bIsBattle = false;
								}
								this.m_MapNpcDataList.Add(mapNpcDataNode);
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

		// Token: 0x06000957 RID: 2391 RVA: 0x00050D28 File Offset: 0x0004EF28
		public MapNpcDataNode GetMapNpcDataNode(int iNpcID)
		{
			for (int i = 0; i < this.m_MapNpcDataList.Count; i++)
			{
				if (this.m_MapNpcDataList[i].m_iNpcID == iNpcID)
				{
					return this.m_MapNpcDataList[i];
				}
			}
			return null;
		}

		// Token: 0x0400093B RID: 2363
		private static readonly MapIconManager instance = new MapIconManager();

		// Token: 0x0400093C RID: 2364
		public List<MapNpcDataNode> m_MapNpcDataList;
	}
}
