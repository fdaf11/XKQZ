using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034A RID: 842
	public class WgCharaProperty : Widget
	{
		// Token: 0x06001334 RID: 4916 RVA: 0x0000C76F File Offset: 0x0000A96F
		public void SetPropertyText(string title, string value)
		{
			this.PropertyTitle.Text = title;
			this.PropertyValue.Text = value;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000A6F50 File Offset: 0x000A5150
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgCharaProperty.<>f__switch$map38 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("LblPropertyTitle", 0);
					dictionary.Add("LblPropertyValue", 1);
					WgCharaProperty.<>f__switch$map38 = dictionary;
				}
				int num;
				if (WgCharaProperty.<>f__switch$map38.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.PropertyValue = sender;
						}
					}
					else
					{
						this.PropertyTitle = sender;
					}
				}
			}
		}

		// Token: 0x04001741 RID: 5953
		public Control PropertyTitle;

		// Token: 0x04001742 RID: 5954
		public Control PropertyValue;
	}
}
