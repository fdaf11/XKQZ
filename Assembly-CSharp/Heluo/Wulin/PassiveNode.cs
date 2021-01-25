using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000211 RID: 529
	public class PassiveNode
	{
		// Token: 0x06000A48 RID: 2632 RVA: 0x00008309 File Offset: 0x00006509
		public PassiveNode(string[] args)
		{
			this.pNodeType = (PassiveNodeType)int.Parse(args[0]);
			this.iValue1 = int.Parse(args[1]);
			this.iValue2 = int.Parse(args[2]);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00057408 File Offset: 0x00055608
		public static void GenerateList(List<PassiveNode> list, string data)
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
					PassiveNode passiveNode = new PassiveNode(array2);
					list.Add(passiveNode);
				}
			}
		}

		// Token: 0x04000AF2 RID: 2802
		public PassiveNodeType pNodeType;

		// Token: 0x04000AF3 RID: 2803
		public int iValue1;

		// Token: 0x04000AF4 RID: 2804
		public int iValue2;
	}
}
