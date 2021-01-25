using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000348 RID: 840
	public class WgCharaDefType : Widget
	{
		// Token: 0x0600132A RID: 4906 RVA: 0x000A6878 File Offset: 0x000A4A78
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgCharaDefType.<>f__switch$map36 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("TypeTitle", 0);
					dictionary.Add("TypeValue", 1);
					WgCharaDefType.<>f__switch$map36 = dictionary;
				}
				int num;
				if (WgCharaDefType.<>f__switch$map36.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.TypeValue = sender;
						}
					}
					else
					{
						this.TypeTitle = sender;
					}
				}
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000A6908 File Offset: 0x000A4B08
		public void SetDefTypeText(CharacterData.PropertyType type, int value)
		{
			string text = string.Empty;
			text = value.ToString();
			this.TypeTitle.Text = Game.StringTable.GetString((int)(110100 + type));
			this.TypeValue.Text = text;
		}

		// Token: 0x04001737 RID: 5943
		public Control TypeTitle;

		// Token: 0x04001738 RID: 5944
		public Control TypeValue;
	}
}
