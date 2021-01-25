using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001B3 RID: 435
	public class HuntDataManager
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x000077D1 File Offset: 0x000059D1
		public HuntDataManager()
		{
			this.m_AnimalDataNodeList = new List<AnimalDataNode>();
			this.LoadFile("HuntData");
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000077FB File Offset: 0x000059FB
		public static HuntDataManager Singleton
		{
			get
			{
				return HuntDataManager.instance;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0004F324 File Offset: 0x0004D524
		private void LoadFile(string filePath)
		{
			string[] array = Game.ExtractTextFile(filePath);
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
							AnimalDataNode animalDataNode = new AnimalDataNode();
							animalDataNode.m_iAnimalID = int.Parse(array3[0]);
							string[] array4 = array3[1].Split(new char[]
							{
								",".get_Chars(0)
							});
							for (int j = 0; j < array4.Length; j++)
							{
								animalDataNode.m_iProbabilityList.Add(int.Parse(array4[j]));
							}
							animalDataNode.m_iBodyType = int.Parse(array3[2]);
							animalDataNode.m_iAnimalType = int.Parse(array3[3]);
							animalDataNode.m_iRound = int.Parse(array3[4]);
							animalDataNode.m_fAtkDedSec = float.Parse(array3[5]);
							animalDataNode.m_iAnimalHp = int.Parse(array3[6]);
							animalDataNode.m_fSpeed = float.Parse(array3[7]);
							animalDataNode.m_ipoint = int.Parse(array3[8]);
							animalDataNode.m_iGift1Item = int.Parse(array3[9]);
							animalDataNode.m_iGift2Item = int.Parse(array3[11]);
							animalDataNode.m_iGift3Item = int.Parse(array3[13]);
							animalDataNode.m_iProbability1 = int.Parse(array3[10]);
							animalDataNode.m_iProbability2 = int.Parse(array3[12]);
							animalDataNode.m_iProbability3 = int.Parse(array3[14]);
							animalDataNode.m_strSoundID = array3[15];
							animalDataNode.m_strMapID = array3[16].Trim();
							if (array3.Length > 17)
							{
								animalDataNode.m_strIcon = array3[17].Trim();
							}
							else
							{
								animalDataNode.m_strIcon = animalDataNode.m_iAnimalID.ToString();
							}
							this.m_AnimalDataNodeList.Add(animalDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0004F560 File Offset: 0x0004D760
		public AnimalDataNode GetAnimalDataNode(int iLevel, int iIndex)
		{
			int i = 0;
			while (i < this.m_AnimalDataNodeList.Count)
			{
				if (this.m_AnimalDataNodeList[i].m_iProbabilityList[iLevel] != 0 && iIndex <= this.m_AnimalDataNodeList[i].m_iProbabilityList[iLevel])
				{
					if (this.m_AnimalDataNodeList[i].m_strMapID.Equals("0"))
					{
						return this.m_AnimalDataNodeList[i];
					}
					return null;
				}
				else
				{
					if (iIndex > 50)
					{
						return this.m_AnimalDataNodeList[0];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x040008C0 RID: 2240
		private static readonly HuntDataManager instance = new HuntDataManager();

		// Token: 0x040008C1 RID: 2241
		public List<AnimalDataNode> m_AnimalDataNodeList;
	}
}
