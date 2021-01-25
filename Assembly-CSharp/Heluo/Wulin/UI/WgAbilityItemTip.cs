using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000335 RID: 821
	public class WgAbilityItemTip : Widget
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x0009FE00 File Offset: 0x0009E000
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgAbilityItemTip.<>f__switch$map27 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("LblName", 0);
					dictionary.Add("LblAddEffectTip", 1);
					dictionary.Add("LblEffectTip", 2);
					dictionary.Add("LblMaterial", 3);
					WgAbilityItemTip.<>f__switch$map27 = dictionary;
				}
				int num;
				if (WgAbilityItemTip.<>f__switch$map27.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_LblName = sender;
						break;
					case 1:
						this.m_LblAddEffectTip = sender;
						break;
					case 2:
						this.m_LblEffectTip = sender;
						break;
					case 3:
						this.m_LblMaterial = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0009FED4 File Offset: 0x0009E0D4
		public void SetTip(string name, string effectTip, string addEffectTip, string material)
		{
			this.m_LblName.GameObject.SetActive(false);
			this.m_LblEffectTip.GameObject.SetActive(false);
			this.m_LblAddEffectTip.GameObject.SetActive(false);
			this.m_LblMaterial.GameObject.SetActive(false);
			if (name.Length > 0)
			{
				this.m_LblName.GameObject.SetActive(true);
				this.m_LblName.Text = name;
			}
			if (effectTip.Length > 0)
			{
				this.m_LblEffectTip.GameObject.SetActive(true);
				this.m_LblEffectTip.Text = effectTip;
			}
			if (addEffectTip.Length > 0)
			{
				this.m_LblAddEffectTip.GameObject.SetActive(true);
				this.m_LblAddEffectTip.Text = addEffectTip;
			}
			if (material.Length > 0)
			{
				this.m_LblMaterial.GameObject.SetActive(true);
				this.m_LblMaterial.Text = material;
			}
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0000BE19 File Offset: 0x0000A019
		public void SetActive(bool bactive)
		{
			this.Obj.SetActive(bactive);
		}

		// Token: 0x04001638 RID: 5688
		private Control m_LblName;

		// Token: 0x04001639 RID: 5689
		private Control m_LblAddEffectTip;

		// Token: 0x0400163A RID: 5690
		private Control m_LblEffectTip;

		// Token: 0x0400163B RID: 5691
		private Control m_LblMaterial;
	}
}
