using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000338 RID: 824
	public class WgBackPackTip : Widget
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x000A167C File Offset: 0x0009F87C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgBackPackTip.<>f__switch$map2C == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("ItemName", 0);
					dictionary.Add("ItemExplain", 1);
					dictionary.Add("ItemTexture", 2);
					dictionary.Add("ItemEffect", 3);
					dictionary.Add("ItemAppend", 4);
					dictionary.Add("ItemLimit", 5);
					WgBackPackTip.<>f__switch$map2C = dictionary;
				}
				int num;
				if (WgBackPackTip.<>f__switch$map2C.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.ItemName = sender;
						break;
					case 1:
						this.ItemExplain = sender;
						break;
					case 2:
						this.ItemTexture = sender;
						break;
					case 3:
						this.ItemEffectList.Add(sender);
						break;
					case 4:
						this.ItemAppend = sender;
						break;
					case 5:
						this.ItemLimit = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x000A1798 File Offset: 0x0009F998
		public void SetItemTip(TipData tipData)
		{
			this.ReSetTip();
			this.ItemName.Text = tipData.name;
			this.ItemExplain.Text = tipData.explain;
			if (tipData.texture != null)
			{
				this.ItemTexture.Texture = tipData.texture;
			}
			else
			{
				this.ItemTexture.Texture = null;
			}
			string text = string.Empty;
			for (int i = 0; i < tipData.limitList.Count; i++)
			{
				text += tipData.limitList[i];
				if (i < tipData.limitList.Count - 1)
				{
					text += "，";
				}
			}
			string text2 = string.Empty;
			for (int j = 0; j < tipData.limitNPCList.Count; j++)
			{
				text2 += tipData.limitNPCList[j];
				if (j < tipData.limitNPCList.Count - 1)
				{
					text2 += "，";
				}
			}
			string text3 = string.Empty;
			for (int k = 0; k < tipData.appendList.Count; k++)
			{
				text3 += tipData.appendList[k];
				if (k < tipData.appendList.Count - 1)
				{
					text3 += "\n";
				}
			}
			if (text.Length != 0 || text2.Length != 0 || text3.Length != 0)
			{
				this.ItemAppend.GameObject.SetActive(true);
				string text4 = string.Empty;
				if (text.Length != 0)
				{
					text4 = text4 + text + "\n";
				}
				if (text2.Length != 0)
				{
					text4 = text4 + text2 + "\n";
				}
				if (text3.Length != 0)
				{
					text4 += text3;
				}
				this.ItemAppend.Text = text4;
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0000BFD0 File Offset: 0x0000A1D0
		public void ReSetTip()
		{
			this.ItemAppend.GameObject.SetActive(false);
			this.ItemLimit.GameObject.SetActive(false);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0000BE19 File Offset: 0x0000A019
		public void SetActive(bool bactive)
		{
			this.Obj.SetActive(bactive);
		}

		// Token: 0x0400166F RID: 5743
		private Control ItemName;

		// Token: 0x04001670 RID: 5744
		private Control ItemExplain;

		// Token: 0x04001671 RID: 5745
		private Control ItemTexture;

		// Token: 0x04001672 RID: 5746
		private Control ItemAppend;

		// Token: 0x04001673 RID: 5747
		private Control ItemLimit;

		// Token: 0x04001674 RID: 5748
		private List<Control> ItemEffectList = new List<Control>();
	}
}
