using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000208 RID: 520
	public class UseLimitNode
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x00056858 File Offset: 0x00054A58
		public UseLimitNode(string[] args)
		{
			this.m_Type = (UseLimitType)int.Parse(args[0]);
			UseLimitType type = this.m_Type;
			if (type == UseLimitType.MoreNpcProperty || type == UseLimitType.LessNpcProperty)
			{
				this.m_iInde = int.Parse(args[1]);
			}
			this.m_iValue = int.Parse(args[2]);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000568B4 File Offset: 0x00054AB4
		public static void GreateData(List<UseLimitNode> ULNList, string data)
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
					UseLimitNode useLimitNode = new UseLimitNode(array2);
					ULNList.Add(useLimitNode);
				}
			}
		}

		// Token: 0x04000AB3 RID: 2739
		public UseLimitType m_Type;

		// Token: 0x04000AB4 RID: 2740
		public int m_iInde;

		// Token: 0x04000AB5 RID: 2741
		public int m_iValue;
	}
}
