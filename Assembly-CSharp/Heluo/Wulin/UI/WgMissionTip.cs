using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000369 RID: 873
	public class WgMissionTip : Widget
	{
		// Token: 0x060013F7 RID: 5111 RVA: 0x000AC360 File Offset: 0x000AA560
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgMissionTip.<>f__switch$map51 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("ExplainName", 0);
					dictionary.Add("Explain", 1);
					WgMissionTip.<>f__switch$map51 = dictionary;
				}
				int num;
				if (WgMissionTip.<>f__switch$map51.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.m_Explain = sender;
						}
					}
					else
					{
						this.m_ExplainName = sender;
					}
				}
			}
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0000CDD3 File Offset: 0x0000AFD3
		public void SetMissionTip(string name, string explain)
		{
			this.m_ExplainName.Text = name;
			this.m_Explain.Text = explain;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0000BE19 File Offset: 0x0000A019
		public void SetActive(bool bactive)
		{
			this.Obj.SetActive(bactive);
		}

		// Token: 0x04001827 RID: 6183
		private Control m_ExplainName;

		// Token: 0x04001828 RID: 6184
		private Control m_Explain;
	}
}
