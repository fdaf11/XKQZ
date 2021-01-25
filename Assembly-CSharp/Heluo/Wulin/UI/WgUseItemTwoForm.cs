using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000364 RID: 868
	public class WgUseItemTwoForm : Widget
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x000AAE0C File Offset: 0x000A900C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgUseItemTwoForm.<>f__switch$map4B == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("UseItemTwoForm", 0);
					dictionary.Add("LblUseItem", 1);
					dictionary.Add("BaclpackScrollBar", 2);
					dictionary.Add("BackpackScrollView", 3);
					dictionary.Add("GridBackpackItem", 4);
					dictionary.Add("IconCloseTwoForm", 5);
					WgUseItemTwoForm.<>f__switch$map4B = dictionary;
				}
				int num;
				if (WgUseItemTwoForm.<>f__switch$map4B.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_UseItemTwoForm = sender;
						break;
					case 1:
						this.m_LblUseItem = sender;
						break;
					case 2:
						this.m_BaclpackScrollBar = sender;
						break;
					case 3:
						this.m_BackpackScrollView = sender;
						break;
					case 4:
						this.m_GridBackpackItem = sender;
						break;
					case 5:
					{
						Control control = sender;
						UICharacter @object = this.ParentLayer as UICharacter;
						control.OnClick += @object.IconCloseTwoFormOnClick;
						control.OnHover += @object.IconCloseTwoFormOnHover;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					}
				}
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x000AAF54 File Offset: 0x000A9154
		public void SetBackpackItemData(CharacterData cdata, List<BackpackNewDataNode> sortList, ItemDataNode.ItemType Type)
		{
			this.firstOpen = !this.Obj.activeSelf;
			this.Obj.SetActive(true);
			if (Type == ItemDataNode.ItemType.TipsBook)
			{
				this.m_LblUseItem.Text = Game.StringTable.GetString(100157);
				this.SetSecretBook(cdata, sortList);
			}
			else
			{
				this.m_LblUseItem.Text = Game.StringTable.GetString(100158);
				this.SetMedicine(sortList);
			}
			this.m_GridBackpackItem.GetComponent<UIGrid>().Reposition();
			if (this.firstOpen)
			{
				this.m_iLastSelect = 0;
				this.firstOpen = false;
				this.m_BaclpackScrollBar.sliderValue = 0f;
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x000AB00C File Offset: 0x000A920C
		public void SetMedicine(List<BackpackNewDataNode> sortList)
		{
			this.m_ShowList.Clear();
			for (int i = 0; i < sortList.Count; i++)
			{
				if (sortList[i]._ItemDataNode.m_iUseTime == 1)
				{
					this.m_ShowList.Add(sortList[i]);
				}
			}
			int count = this.m_ShowList.Count;
			if (this.m_WgBackpackItem.Count < count)
			{
				this.CreatBackpackItem(count - this.m_WgBackpackItem.Count);
			}
			int num = 0;
			for (int j = 0; j < this.m_WgBackpackItem.Count; j++)
			{
				this.m_WgBackpackItem[j].Obj.SetActive(false);
				if (num < this.m_ShowList.Count)
				{
					this.m_WgBackpackItem[j].SetScrollBar = new Action<GameObject>(this.SetScrollBar);
					this.m_WgBackpackItem[j].SetMedicine(this.m_ShowList[num]);
					num++;
				}
			}
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000AB118 File Offset: 0x000A9318
		public bool CheckSecretBook(CharacterData cdata, ItemDataNode _ItemDataNode)
		{
			bool result = false;
			if (_ItemDataNode.m_CanUseiNpcIDList.Count == 0)
			{
				result = true;
			}
			for (int i = 0; i < _ItemDataNode.m_CanUseiNpcIDList.Count; i++)
			{
				if (cdata.iNpcID == _ItemDataNode.m_CanUseiNpcIDList[i])
				{
					result = true;
				}
				if (_ItemDataNode.m_CanUseiNpcIDList[i] == 99 && cdata._NpcDataNode.m_Gender == GenderType.Female)
				{
					result = true;
				}
				else if (_ItemDataNode.m_CanUseiNpcIDList[i] == 98 && cdata._NpcDataNode.m_Gender == GenderType.Male)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000AB1C0 File Offset: 0x000A93C0
		public void SetSecretBook(CharacterData cdata, List<BackpackNewDataNode> sortList)
		{
			this.m_ShowList.Clear();
			for (int i = 0; i < sortList.Count; i++)
			{
				bool flag = this.CheckSecretBook(cdata, sortList[i]._ItemDataNode);
				if (flag)
				{
					this.m_ShowList.Add(sortList[i]);
				}
			}
			int count = this.m_ShowList.Count;
			if (this.m_WgBackpackItem.Count < count)
			{
				this.CreatBackpackItem(count - this.m_WgBackpackItem.Count);
			}
			int num = 0;
			for (int j = 0; j < this.m_WgBackpackItem.Count; j++)
			{
				this.m_WgBackpackItem[j].Obj.SetActive(false);
				if (num < this.m_ShowList.Count)
				{
					this.m_WgBackpackItem[j].SetScrollBar = new Action<GameObject>(this.SetScrollBar);
					this.m_WgBackpackItem[j].SetSecretBook(this.m_ShowList[num]);
					num++;
				}
			}
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x000AB2D8 File Offset: 0x000A94D8
		private void CreatBackpackItem(int max)
		{
			this.ParentLayer.CreateUIWidget<WgBackpackItem>("BackpackItem", this.m_GridBackpackItem.GameObject, this.m_Item, this.m_WgBackpackItem, max);
			this.m_GridBackpackItem.GetComponent<UIGrid>().Reposition();
			this.m_BaclpackScrollBar.sliderValue = 0f;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000AB330 File Offset: 0x000A9530
		public void SetScrollBar(GameObject go)
		{
			int num = this.m_WgBackpackItem.FindIndex((WgBackpackItem x) => x.Obj == go);
			if (num > this.m_iLastSelect)
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_ShowList.Count, false, this.m_BaclpackScrollBar);
			}
			else
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_ShowList.Count, true, this.m_BaclpackScrollBar);
			}
			this.m_iLastSelect = num;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x000AB3B8 File Offset: 0x000A95B8
		private void Update()
		{
			if (this.m_UseItemTwoForm != null && this.m_UseItemTwoForm.GameObject.activeSelf)
			{
				this.CheckMouseWheel(this.m_ShowList.Count, this.m_iTwoFormMaxAmount, this.m_BaclpackScrollBar, this.m_BackpackScrollView, false);
				this.SetMouseWheel(this.m_BaclpackScrollBar);
			}
		}

		// Token: 0x040017F6 RID: 6134
		private Control m_UseItemTwoForm;

		// Token: 0x040017F7 RID: 6135
		private Control m_LblUseItem;

		// Token: 0x040017F8 RID: 6136
		private Control m_GridBackpackItem;

		// Token: 0x040017F9 RID: 6137
		public GameObject m_Item;

		// Token: 0x040017FA RID: 6138
		private List<WgBackpackItem> m_WgBackpackItem = new List<WgBackpackItem>();

		// Token: 0x040017FB RID: 6139
		private Control m_BaclpackScrollBar;

		// Token: 0x040017FC RID: 6140
		private Control m_BackpackScrollView;

		// Token: 0x040017FD RID: 6141
		private Control m_BackpackEquipOnHover;

		// Token: 0x040017FE RID: 6142
		private int m_iTwoFormMaxAmount = 6;

		// Token: 0x040017FF RID: 6143
		private int m_iLastSelect;

		// Token: 0x04001800 RID: 6144
		private List<BackpackNewDataNode> m_ShowList = new List<BackpackNewDataNode>();

		// Token: 0x04001801 RID: 6145
		private bool firstOpen;
	}
}
