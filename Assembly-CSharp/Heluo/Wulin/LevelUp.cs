using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000254 RID: 596
	public class LevelUp
	{
		// Token: 0x06000AFA RID: 2810 RVA: 0x0005B948 File Offset: 0x00059B48
		public LevelUp(string[] args)
		{
			int type;
			if (!int.TryParse(args[0], ref type))
			{
				type = 0;
			}
			int iValue;
			if (!int.TryParse(args[1], ref iValue))
			{
				iValue = 0;
			}
			this.m_Type = (CharacterData.PropertyType)type;
			this.m_iValue = iValue;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0005B98C File Offset: 0x00059B8C
		public static void GreatData(List<LevelUp> list, string data)
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
				if (array2.Length > 1)
				{
					LevelUp levelUp = new LevelUp(array2);
					list.Add(levelUp);
				}
			}
		}

		// Token: 0x04000C6B RID: 3179
		public CharacterData.PropertyType m_Type;

		// Token: 0x04000C6C RID: 3180
		public int m_iValue;
	}
}
