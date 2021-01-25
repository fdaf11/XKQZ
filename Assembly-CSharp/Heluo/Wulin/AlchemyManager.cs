using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001DF RID: 479
	public class AlchemyManager : TextDataManager
	{
		// Token: 0x060009CA RID: 2506 RVA: 0x00007E76 File Offset: 0x00006076
		public AlchemyManager()
		{
			this.m_AlchemyProduceNodeList = new List<AlchemyProduceNode>();
			this.m_AlchemySceneList = new List<AlchemyScene>();
			this.LoadProduceNode("AlchemyProduceData");
			this.LoadSceneFile("AlchemyScene");
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x00007EBC File Offset: 0x000060BC
		public static AlchemyManager Singleton
		{
			get
			{
				return AlchemyManager.instance;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00007EC3 File Offset: 0x000060C3
		protected override void LoadFile(string filePath)
		{
			this.LoadProduceNode("AlchemyProduceData");
			this.LoadSceneFile("AlchemyScene");
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00052FAC File Offset: 0x000511AC
		private void LoadSceneFile(string filePath)
		{
			this.m_AlchemySceneList.Clear();
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
							AlchemyScene alchemyScene = new AlchemyScene();
							alchemyScene.m_iAbilityBookID = int.Parse(array3[0]);
							alchemyScene.m_iSkilllevel = int.Parse(array3[1]);
							alchemyScene.m_iMoveCount = int.Parse(array3[2]);
							alchemyScene.m_iMarkTarget = int.Parse(array3[3]);
							alchemyScene.m_iMarkTargetCount = int.Parse(array3[4]);
							alchemyScene.m_iSuccessItemID = int.Parse(array3[5]);
							alchemyScene.m_iMaxItemCount = int.Parse(array3[6]);
							alchemyScene.m_iWidth = int.Parse(array3[7]);
							alchemyScene.m_iHeight = int.Parse(array3[8]);
							for (int j = 9; j < array3.Length; j++)
							{
								alchemyScene.m_iTileList.Add(int.Parse(array3[j]));
							}
							this.m_AlchemySceneList.Add(alchemyScene);
						}
						catch
						{
							Debug.LogError("解析 AlchemyScene 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00053124 File Offset: 0x00051324
		public AlchemyScene GetAlchemyScene(int iID, int iNowSkill)
		{
			AlchemyScene result = null;
			int num = -1;
			foreach (AlchemyScene alchemyScene in this.m_AlchemySceneList)
			{
				if (alchemyScene.m_iAbilityBookID == iID)
				{
					if (alchemyScene.m_iSkilllevel <= iNowSkill)
					{
						if (num < alchemyScene.m_iSkilllevel)
						{
							result = alchemyScene;
							num = alchemyScene.m_iSkilllevel;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x000531B4 File Offset: 0x000513B4
		private void LoadProduceNode(string filePath)
		{
			this.m_AlchemyProduceNodeList.Clear();
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
							AlchemyProduceNode alchemyProduceNode = new AlchemyProduceNode();
							alchemyProduceNode.m_iAbilityBookID = int.Parse(array3[0]);
							alchemyProduceNode.m_iType = int.Parse(array3[1]);
							for (int j = 2; j < 12; j += 2)
							{
								if (int.Parse(array3[j]) != 0)
								{
									MaterialNode materialNode = new MaterialNode();
									materialNode.m_iMaterialID = int.Parse(array3[j]);
									materialNode.m_iAmount = int.Parse(array3[j + 1]);
									alchemyProduceNode.m_MaterialNodeList.Add(materialNode);
								}
							}
							if (array3.Length < 13)
							{
								alchemyProduceNode.m_iRequestSkill = 0;
							}
							else
							{
								alchemyProduceNode.m_iRequestSkill = int.Parse(array3[12]);
							}
							if (array3.Length < 14)
							{
								alchemyProduceNode.m_strIcon = string.Empty;
							}
							else
							{
								alchemyProduceNode.m_strIcon = array3[13].Trim();
							}
							this.m_AlchemyProduceNodeList.Add(alchemyProduceNode);
						}
						catch
						{
							Debug.LogError("解析 AlchemyProduceData 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00053350 File Offset: 0x00051550
		public AlchemyProduceNode GetAlchemyProduceNode(int iID)
		{
			for (int i = 0; i < this.m_AlchemyProduceNodeList.Count; i++)
			{
				if (this.m_AlchemyProduceNodeList[i].m_iAbilityBookID == iID)
				{
					return this.m_AlchemyProduceNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x040009CB RID: 2507
		private static readonly AlchemyManager instance = new AlchemyManager();

		// Token: 0x040009CC RID: 2508
		private List<AlchemyProduceNode> m_AlchemyProduceNodeList;

		// Token: 0x040009CD RID: 2509
		private List<AlchemyScene> m_AlchemySceneList;
	}
}
