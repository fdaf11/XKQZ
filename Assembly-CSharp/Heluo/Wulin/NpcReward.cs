using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000238 RID: 568
	public class NpcReward
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x000087E0 File Offset: 0x000069E0
		public NpcReward()
		{
			this.m_Type = NpcRewardType.None;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000087EF File Offset: 0x000069EF
		public NpcReward(string[] aString)
		{
			this.m_Type = (NpcRewardType)int.Parse(aString[0]);
			this.m_iID = int.Parse(aString[1]);
			this.m_iValue = int.Parse(aString[2]);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0005979C File Offset: 0x0005799C
		public static void GenerateList(List<NpcReward> list, string data)
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
					NpcReward npcReward = new NpcReward(array2);
					list.Add(npcReward);
				}
			}
		}

		// Token: 0x04000BDA RID: 3034
		public NpcRewardType m_Type;

		// Token: 0x04000BDB RID: 3035
		public int m_iID;

		// Token: 0x04000BDC RID: 3036
		public int m_iValue;
	}
}
