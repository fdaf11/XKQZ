using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200026A RID: 618
	public class PassiveTreeNode
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00008E8F File Offset: 0x0000708F
		public PassiveTreeNode(string[] args)
		{
			this.iPos = int.Parse(args[0]);
			this.iPassiveID = int.Parse(args[1]);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0005D844 File Offset: 0x0005BA44
		public static void GenerateList(List<PassiveTreeNode> list, string data)
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
					PassiveTreeNode passiveTreeNode = new PassiveTreeNode(array2);
					list.Add(passiveTreeNode);
				}
			}
		}

		// Token: 0x04000D2C RID: 3372
		public int iPos;

		// Token: 0x04000D2D RID: 3373
		public int iPassiveID;
	}
}
