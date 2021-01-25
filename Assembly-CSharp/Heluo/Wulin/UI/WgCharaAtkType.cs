using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000347 RID: 839
	public class WgCharaAtkType : Widget
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x000A6764 File Offset: 0x000A4964
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgCharaAtkType.<>f__switch$map35 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("TypeIcon", 0);
					dictionary.Add("TypeTitle", 1);
					dictionary.Add("TypeValue", 2);
					WgCharaAtkType.<>f__switch$map35 = dictionary;
				}
				int num;
				if (WgCharaAtkType.<>f__switch$map35.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.TypeIcon = sender;
						break;
					case 1:
						this.TypeTitle = sender;
						break;
					case 2:
						this.TypeValue = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000A6818 File Offset: 0x000A4A18
		public void SetAtkTypeText(CharacterData.PropertyType type, int value)
		{
			string text = string.Empty;
			text = value.ToString();
			this.TypeTitle.Text = Game.StringTable.GetString((int)(110100 + type));
			this.TypeValue.Text = text;
			this.TypeIcon.SpriteName = string.Format("cdata_045_{0:00}", (int)type);
		}

		// Token: 0x04001733 RID: 5939
		public Control TypeIcon;

		// Token: 0x04001734 RID: 5940
		public Control TypeTitle;

		// Token: 0x04001735 RID: 5941
		public Control TypeValue;
	}
}
