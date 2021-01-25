using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034D RID: 845
	public class WgDLCCharaDefType : Widget
	{
		// Token: 0x06001348 RID: 4936 RVA: 0x000A75B0 File Offset: 0x000A57B0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgDLCCharaDefType.<>f__switch$map3C == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("TypeTitle", 0);
					dictionary.Add("TypeValue", 1);
					dictionary.Add("Add", 2);
					WgDLCCharaDefType.<>f__switch$map3C = dictionary;
				}
				int num;
				if (WgDLCCharaDefType.<>f__switch$map3C.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.TypeTitle = sender;
						break;
					case 1:
						this.TypeValue = sender;
						break;
					case 2:
						this.Add = sender;
						this.Add.OnClick += this.BtnAddClick;
						break;
					}
				}
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x000A7678 File Offset: 0x000A5878
		public void SetDefTypeText(CharacterData.PropertyType type, int value)
		{
			string text = string.Empty;
			text = value.ToString();
			this.TypeTitle.Text = Game.StringTable.GetString((int)(110100 + type));
			this.TypeValue.Text = text;
			this.type = type;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0000C815 File Offset: 0x0000AA15
		public void IsShowAdd(bool isShow)
		{
			this.Add.GameObject.SetActive(isShow);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0000C828 File Offset: 0x0000AA28
		public void BtnAddClick(GameObject obj)
		{
			if (this.OnAddClick != null)
			{
				this.OnAddClick.Invoke(this.type);
			}
		}

		// Token: 0x04001757 RID: 5975
		public Control TypeTitle;

		// Token: 0x04001758 RID: 5976
		public Control TypeValue;

		// Token: 0x04001759 RID: 5977
		public Control Add;

		// Token: 0x0400175A RID: 5978
		private CharacterData.PropertyType type = CharacterData.PropertyType.DefSword;

		// Token: 0x0400175B RID: 5979
		public Action<CharacterData.PropertyType> OnAddClick;
	}
}
