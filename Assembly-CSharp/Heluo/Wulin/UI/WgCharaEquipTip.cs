using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000349 RID: 841
	public class WgCharaEquipTip : Widget
	{
		// Token: 0x0600132D RID: 4909 RVA: 0x000A694C File Offset: 0x000A4B4C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name == "EquipTip")
			{
				this.m_EquipTip = sender;
				return;
			}
			if (name == "LblEquipTip")
			{
				this.m_LblEquipTip = sender;
				return;
			}
			if (name == "LblEquipEffectTip")
			{
				this.m_LblEquipEffectTip = sender;
				return;
			}
			if (name == "LblEquipAddEffectTip")
			{
				this.m_LblEquipAddEffectTip = sender;
				return;
			}
			if (!(name == "LblEquipNone"))
			{
				return;
			}
			this.m_LblEquipNone = sender;
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x0000C762 File Offset: 0x0000A962
		// (set) Token: 0x0600132E RID: 4910 RVA: 0x0000C754 File Offset: 0x0000A954
		public string SpriteName
		{
			get
			{
				return this.m_EquipTip.SpriteName;
			}
			set
			{
				this.m_EquipTip.SpriteName = value;
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000A69E4 File Offset: 0x000A4BE4
		public void SetEquipTipNone()
		{
			this.m_LblEquipNone.GameObject.SetActive(true);
			this.m_LblEquipTip.GameObject.SetActive(false);
			this.m_LblEquipEffectTip.GameObject.SetActive(false);
			this.m_LblEquipAddEffectTip.GameObject.SetActive(false);
			for (int i = 0; i < this.mod_GemIconList.Count; i++)
			{
				this.mod_GemIconList[i].SetActive(false);
				this.mod_GemEffectList[i].SetActive(false);
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000A6A70 File Offset: 0x000A4C70
		public void SetEquipTip(string equipTip, string effectTip, string addEffectTip)
		{
			this.m_EquipTip.UISprite.SetRect(this.m_EquipTip.GameObject.transform.localPosition.x - 100f, this.m_EquipTip.GameObject.transform.localPosition.y - 80f, 760f, 500f);
			this.m_LblEquipNone.GameObject.SetActive(false);
			this.m_LblEquipTip.GameObject.SetActive(false);
			this.m_LblEquipEffectTip.GameObject.SetActive(false);
			this.m_LblEquipAddEffectTip.GameObject.SetActive(false);
			for (int i = 0; i < this.mod_GemIconList.Count; i++)
			{
				this.mod_GemIconList[i].SetActive(false);
				this.mod_GemEffectList[i].SetActive(false);
			}
			if (equipTip.Length > 0)
			{
				this.m_LblEquipTip.GameObject.SetActive(true);
				this.m_LblEquipTip.GameObject.transform.position = this.m_EquipTip.GameObject.transform.position + new Vector3(0.1f, 0.3f, 0f);
				this.m_LblEquipTip.Text = equipTip;
			}
			if (effectTip.Length > 0)
			{
				this.m_LblEquipEffectTip.GameObject.SetActive(true);
				this.m_LblEquipEffectTip.Text = effectTip;
				this.m_LblEquipEffectTip.UILabel.maxLineCount = 0;
				this.m_LblEquipEffectTip.UILabel.fontSize = 25;
				this.m_LblEquipEffectTip.UILabel.height = 400;
				this.m_LblEquipEffectTip.UILabel.spacingY = 10;
				string[] array = effectTip.Split(new char[]
				{
					'\n'
				});
				this.m_LblEquipEffectTip.GameObject.transform.localPosition = this.m_LblEquipTip.GameObject.transform.localPosition - new Vector3(0f, (float)array.Length * 35f, 0f);
				List<string> list = new List<string>();
				list.Add("测试1");
				list.Add("测试2");
				list.Add("测试3");
				list.Add("测试4");
				list.Add("测试5");
				list.Add("测试6");
				List<string> list2 = list;
				if (this.mod_GemIconList.Count > 0)
				{
					for (int j = 0; j < this.mod_GemIconList.Count; j++)
					{
						Texture2D mainTexture = Game.mod_Load((j + 11).ToString());
						this.mod_GemIconList[j].SetActive(true);
						this.mod_GemIconList[j].GetComponent<UITexture>().mainTexture = mainTexture;
						this.mod_GemIconList[j].transform.SetParent(this.m_EquipTip.GameObject.transform);
						this.mod_GemIconList[j].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
						this.mod_GemIconList[j].transform.localPosition = this.m_LblEquipTip.GameObject.transform.localPosition + new Vector3(220f, -(float)j * 45f, 0f);
						this.mod_GemEffectList[j].SetActive(true);
						this.mod_GemEffectList[j].GetComponent<UILabel>().text = list2[j];
						this.mod_GemEffectList[j].GetComponent<UILabel>().transform.SetParent(this.m_EquipTip.GameObject.transform);
						this.mod_GemEffectList[j].GetComponent<UILabel>().transform.localPosition = this.m_LblEquipTip.GameObject.transform.localPosition + new Vector3(250f, -(float)j * 45f, 0f);
					}
				}
			}
			if (addEffectTip.Length > 0)
			{
				this.m_LblEquipAddEffectTip.GameObject.SetActive(true);
				this.m_LblEquipAddEffectTip.UILabel.height = 400;
				this.m_LblEquipAddEffectTip.UILabel.maxLineCount = 0;
				this.m_LblEquipAddEffectTip.UILabel.fontSize = 25;
				this.m_LblEquipAddEffectTip.UILabel.spacingY = 10;
				string[] array2 = addEffectTip.Split(new char[]
				{
					'\n'
				});
				this.m_LblEquipAddEffectTip.GameObject.transform.localPosition = this.m_LblEquipTip.GameObject.transform.localPosition + new Vector3(500f, -(float)array2.Length * 15f, 0f);
				this.m_LblEquipAddEffectTip.Text = addEffectTip;
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0000BE19 File Offset: 0x0000A019
		public void SetActive(bool bactive)
		{
			this.Obj.SetActive(bactive);
		}

		// Token: 0x0400173A RID: 5946
		private Control m_EquipTip;

		// Token: 0x0400173B RID: 5947
		private Control m_LblEquipTip;

		// Token: 0x0400173C RID: 5948
		private Control m_LblEquipEffectTip;

		// Token: 0x0400173D RID: 5949
		private Control m_LblEquipAddEffectTip;

		// Token: 0x0400173E RID: 5950
		private Control m_LblEquipNone;

		// Token: 0x0400173F RID: 5951
		public List<GameObject> mod_GemIconList = new List<GameObject>();

		// Token: 0x04001740 RID: 5952
		public List<GameObject> mod_GemEffectList = new List<GameObject>();
	}
}
