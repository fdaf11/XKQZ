using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000355 RID: 853
	public class WgPracticeUp : Widget
	{
		// Token: 0x06001383 RID: 4995 RVA: 0x000A8988 File Offset: 0x000A6B88
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgPracticeUp.<>f__switch$map43 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("Title", 0);
					dictionary.Add("Value", 1);
					WgPracticeUp.<>f__switch$map43 = dictionary;
				}
				int num;
				if (WgPracticeUp.<>f__switch$map43.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.Value = sender;
						}
					}
					else
					{
						this.Title = sender;
					}
				}
			}
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000A8A18 File Offset: 0x000A6C18
		public void SetPracticeUpGroupText(CharacterData.PropertyType type, int value)
		{
			string text = string.Empty;
			text = value.ToString();
			this.Title.Text = Game.StringTable.GetString((int)(110100 + type));
			this.Value.Text = text;
			this.Obj.SetActive(true);
		}

		// Token: 0x04001796 RID: 6038
		public Control Title;

		// Token: 0x04001797 RID: 6039
		public Control Value;
	}
}
