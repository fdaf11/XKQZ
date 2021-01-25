using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034B RID: 843
	public class WgCharacterList : Widget
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x0000C79C File Offset: 0x0000A99C
		private void Awake()
		{
			this.m_WgHeroBtn.InitWidget(this.ParentLayer);
			this.m_WgHeroBtn.OnClick = new Action<GameObject>(this.OnHeroClick);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000A6FE0 File Offset: 0x000A51E0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgCharacterList.<>f__switch$map39 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("Hero", 0);
					dictionary.Add("ForegroundImage", 1);
					dictionary.Add("Block", 2);
					WgCharacterList.<>f__switch$map39 = dictionary;
				}
				int num;
				if (WgCharacterList.<>f__switch$map39.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Hero = sender;
						break;
					case 1:
						this.m_ForegroundImage = sender;
						break;
					case 2:
						this.m_Block = sender;
						this.m_Block.GameObject.SetActive(false);
						break;
					}
				}
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000A70A4 File Offset: 0x000A52A4
		public void SetDLCUnitList(List<DLCUnitInfo> infos)
		{
			this.ClearBtnHero();
			for (int i = 0; i < infos.Count; i++)
			{
				GameObject heroButton = this.GetHeroButton();
				heroButton.SetActive(true);
				heroButton.name = infos[i].GID.ToString();
				heroButton.transform.parent = this.m_GridItem.transform;
				heroButton.transform.localPosition = Vector3.zero;
				heroButton.transform.localScale = Vector3.one;
				WgHeroBtn component = heroButton.GetComponent<WgHeroBtn>();
				component.SetCharaHead(infos[i], null);
			}
			this.m_GridItem.enabled = true;
			this.m_GridItem.Reposition();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x000A7158 File Offset: 0x000A5358
		public void ResetCharaList()
		{
			for (int i = 0; i < this.m_BtnHeorList.Count; i++)
			{
				WgHeroBtn component = this.m_BtnHeorList[i].GetComponent<WgHeroBtn>();
				component.ResetInfo();
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000A719C File Offset: 0x000A539C
		public void SetCharacterList(List<CharacterData> charas)
		{
			this.ClearBtnHero();
			for (int i = 0; i < charas.Count; i++)
			{
				GameObject heroButton = this.GetHeroButton();
				heroButton.SetActive(true);
				heroButton.name = charas[i].iNpcID.ToString();
				heroButton.transform.parent = this.m_GridItem.transform;
				heroButton.transform.localPosition = Vector3.zero;
				heroButton.transform.localScale = Vector3.one;
				WgHeroBtn component = heroButton.GetComponent<WgHeroBtn>();
				component.SetCharaHead(null, charas[i]);
			}
			this.m_GridItem.enabled = true;
			this.m_GridItem.Reposition();
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0000C7C6 File Offset: 0x0000A9C6
		public void OnHeroClick(GameObject go)
		{
			if (this.SetCurNpcID != null)
			{
				this.SetCurNpcID.Invoke(go.name);
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000A7250 File Offset: 0x000A5450
		public GameObject GetHeroButton()
		{
			for (int i = 0; i < this.m_BtnHeorList.Count; i++)
			{
				if (!this.m_BtnHeorList[i].activeSelf)
				{
					return this.m_BtnHeorList[i];
				}
			}
			GameObject gameObject = Object.Instantiate(this.m_Hero.GameObject) as GameObject;
			WgHeroBtn component = gameObject.GetComponent<WgHeroBtn>();
			component.InitWidget(this.ParentLayer);
			component.OnClick = new Action<GameObject>(this.OnHeroClick);
			component.UnSelectAll = new Action(this.UnSelectAll);
			this.m_BtnHeorList.Add(gameObject);
			return gameObject;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000A72F8 File Offset: 0x000A54F8
		public void UnSelectAll()
		{
			for (int i = 0; i < this.m_BtnHeorList.Count; i++)
			{
				WgHeroBtn component = this.m_BtnHeorList[i].GetComponent<WgHeroBtn>();
				component.UnSelect();
			}
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x000A733C File Offset: 0x000A553C
		public void ClearBtnHero()
		{
			for (int i = 0; i < this.m_BtnHeorList.Count; i++)
			{
				Object.Destroy(this.m_BtnHeorList[i]);
			}
			this.m_BtnHeorList.Clear();
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		public void Block(bool block)
		{
			this.m_Block.GameObject.SetActive(block);
		}

		// Token: 0x04001744 RID: 5956
		public WgHeroBtn m_WgHeroBtn;

		// Token: 0x04001745 RID: 5957
		public Control m_Hero;

		// Token: 0x04001746 RID: 5958
		public Control m_ForegroundImage;

		// Token: 0x04001747 RID: 5959
		public Control m_Label;

		// Token: 0x04001748 RID: 5960
		public Control m_Block;

		// Token: 0x04001749 RID: 5961
		public UIGrid m_GridItem;

		// Token: 0x0400174A RID: 5962
		private List<GameObject> m_BtnHeorList = new List<GameObject>();

		// Token: 0x0400174B RID: 5963
		public Action<string> SetCurNpcID;
	}
}
