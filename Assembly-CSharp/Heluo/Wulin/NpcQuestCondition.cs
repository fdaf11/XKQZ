using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000237 RID: 567
	public class NpcQuestCondition
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x000087D1 File Offset: 0x000069D1
		public NpcQuestCondition()
		{
			this.m_Type = NpcConditionType.None;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0005968C File Offset: 0x0005788C
		public NpcQuestCondition(string[] aString)
		{
			this.m_Type = (NpcConditionType)int.Parse(aString[0]);
			NpcConditionType type = this.m_Type;
			if (type != NpcConditionType.NpcQuest)
			{
				this.m_Parameter = int.Parse(aString[1]);
			}
			else
			{
				this.m_Parameter = aString[1];
			}
			this.m_iValue = int.Parse(aString[2]);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000596FC File Offset: 0x000578FC
		public static void GenerateList(List<NpcQuestCondition> list, string data)
		{
			string text = data.Replace(")*(", "*");
			if (text.Length > 2)
			{
				text = text.Substring(1, text.Length - 2);
			}
			string[] array = text.Split(new char[]
			{
				"*".get_Chars(0)
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					",".get_Chars(0)
				});
				if (array2.Length > 2)
				{
					NpcQuestCondition npcQuestCondition = new NpcQuestCondition(array2);
					list.Add(npcQuestCondition);
				}
			}
		}

		// Token: 0x04000BD7 RID: 3031
		public NpcConditionType m_Type;

		// Token: 0x04000BD8 RID: 3032
		public CParaValue m_Parameter;

		// Token: 0x04000BD9 RID: 3033
		public int m_iValue;
	}
}
