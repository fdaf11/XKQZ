using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000256 RID: 598
	public class RoutineNewManager : TextDataManager
	{
		// Token: 0x06000AFE RID: 2814 RVA: 0x00008B44 File Offset: 0x00006D44
		public RoutineNewManager()
		{
			this.m_RoutineDataNodeList = new List<RoutineNewDataNode>();
			this.m_LoadFileName = "RoutineNewData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00008B86 File Offset: 0x00006D86
		public static RoutineNewManager Singleton
		{
			get
			{
				return RoutineNewManager.instance;
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0005BC18 File Offset: 0x00059E18
		protected override void LoadFile(string filePath)
		{
			this.m_RoutineDataNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			if (array == null)
			{
				return;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '\r')
					{
						if (text.get_Chars(0) != '#')
						{
							try
							{
								string[] array3 = text.Trim().Split(new char[]
								{
									"\t".get_Chars(0)
								});
								RoutineNewDataNode routineNewDataNode = new RoutineNewDataNode();
								routineNewDataNode.m_iRoutineID = int.Parse(array3[0]);
								routineNewDataNode.m_strRoutineName = array3[1];
								routineNewDataNode.m_strRoutineTip = array3[2].Replace("<br>", "\n");
								routineNewDataNode.m_strUpgradeNotes = array3[3];
								routineNewDataNode.m_strSkillIconName = array3[4];
								int num = 0;
								if (int.TryParse(array3[5], ref num))
								{
									if (num > 8)
									{
										Debug.LogError(routineNewDataNode.m_iRoutineID + "have error" + array3[5]);
										routineNewDataNode.m_RoutineType = WeaponType.None;
									}
									else
									{
										routineNewDataNode.m_RoutineType = (WeaponType)num;
									}
								}
								int num2 = 0;
								if (int.TryParse(array3[6], ref num2))
								{
									routineNewDataNode.m_AdditonType = num2 + CharacterData.PropertyType.Strength;
								}
								routineNewDataNode.m_iExpType = int.Parse(array3[7]);
								if (array3[8].Length > 1)
								{
									LevelUp.GreatData(routineNewDataNode.m_iLevelUP, array3[8]);
								}
								routineNewDataNode._NeigonLinkID = int.Parse(array3[9]);
								if (array3[10] == "0")
								{
									routineNewDataNode.m_bNeedToSelectTarget = false;
								}
								else
								{
									routineNewDataNode.m_bNeedToSelectTarget = true;
								}
								routineNewDataNode.m_iSkillType = int.Parse(array3[11]);
								routineNewDataNode.m_iTargetType = int.Parse(array3[12]);
								routineNewDataNode.m_iTargetArea = int.Parse(array3[13]);
								routineNewDataNode.m_iRange = int.Parse(array3[14]);
								routineNewDataNode.m_iAOE = int.Parse(array3[15]);
								routineNewDataNode.m_iDamage = int.Parse(array3[16]);
								routineNewDataNode.m_iRequestSP = int.Parse(array3[17]);
								routineNewDataNode.m_iCD = int.Parse(array3[18]);
								string[] array4 = array3[19].Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text2 in array4)
								{
									int num3 = int.Parse(text2);
									if (num3 != 0)
									{
										routineNewDataNode.m_iConditionIDList.Add(num3);
									}
								}
								string text3 = array3[20].Replace("\r", string.Empty);
								array4 = text3.Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text4 in array4)
								{
									routineNewDataNode.m_iSkillEffectIDList.Add(int.Parse(text4));
								}
								this.m_RoutineDataNodeList.Add(routineNewDataNode);
							}
							catch
							{
								Debug.LogError("解析 RoutineNewData 時發生錯誤 : " + text);
							}
						}
					}
				}
			}
			this.m_RoutineDataNodeList.Sort((RoutineNewDataNode A, RoutineNewDataNode B) => A.m_iRoutineID.CompareTo(B.m_iRoutineID));
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0005BF8C File Offset: 0x0005A18C
		public string GetGetRoutineName(int RoutineId)
		{
			int num = this.BinarySearch(RoutineId);
			if (num >= 0)
			{
				return this.m_RoutineDataNodeList[num].m_strRoutineName;
			}
			return string.Empty;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0005BFC0 File Offset: 0x0005A1C0
		public RoutineNewDataNode GetRoutineNewData(int RoutineId)
		{
			int num = this.BinarySearch(RoutineId);
			if (num >= 0)
			{
				return this.m_RoutineDataNodeList[num];
			}
			return null;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00008B8D File Offset: 0x00006D8D
		public List<RoutineNewDataNode> GetRoutineList()
		{
			return this.m_RoutineDataNodeList;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0005BFEC File Offset: 0x0005A1EC
		private int BinarySearch(int id)
		{
			List<RoutineNewDataNode> routineDataNodeList = this.m_RoutineDataNodeList;
			int i = 0;
			int num = routineDataNodeList.Count - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				if (routineDataNodeList[num2].m_iRoutineID == id)
				{
					return num2;
				}
				if (routineDataNodeList[num2].m_iRoutineID < id)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return -1;
		}

		// Token: 0x04000C83 RID: 3203
		private static readonly RoutineNewManager instance = new RoutineNewManager();

		// Token: 0x04000C84 RID: 3204
		private List<RoutineNewDataNode> m_RoutineDataNodeList;
	}
}
