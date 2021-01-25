using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001E5 RID: 485
	public class BiographiesNpcQHeadNode
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x00053590 File Offset: 0x00051790
		public BiographiesNpcQHeadNode(string[] data)
		{
			this.m_QHeadName = data[0];
			int dir = 0;
			int.TryParse(data[1], ref dir);
			this.m_Dir = (UIBasicSprite.Flip)dir;
			int ePos = 0;
			int.TryParse(data[2], ref ePos);
			this.m_ePos = (BiographiesNpcQHeadNode.ePos)ePos;
			this.generateAction(data[3]);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x000535E8 File Offset: 0x000517E8
		private void generateAction(string data)
		{
			string text = data.Replace("]+[", "+");
			text = text.Replace("\r", string.Empty);
			if (text.Length > 2)
			{
				text = text.Substring(1, text.Length - 2);
				string[] array = text.Split(new char[]
				{
					"+".get_Chars(0)
				});
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						"-".get_Chars(0)
					});
					if (array2.Length > 2)
					{
						BiographiesActionNode biographiesActionNode = new BiographiesActionNode(array2);
						this.m_ActionList.Add(biographiesActionNode);
					}
				}
			}
		}

		// Token: 0x040009E4 RID: 2532
		public string m_QHeadName;

		// Token: 0x040009E5 RID: 2533
		public UIBasicSprite.Flip m_Dir;

		// Token: 0x040009E6 RID: 2534
		public BiographiesNpcQHeadNode.ePos m_ePos;

		// Token: 0x040009E7 RID: 2535
		public List<BiographiesActionNode> m_ActionList = new List<BiographiesActionNode>();

		// Token: 0x020001E6 RID: 486
		public enum ePos
		{
			// Token: 0x040009E9 RID: 2537
			Left,
			// Token: 0x040009EA RID: 2538
			MediumLeft,
			// Token: 0x040009EB RID: 2539
			MediumRight,
			// Token: 0x040009EC RID: 2540
			Right
		}
	}
}
