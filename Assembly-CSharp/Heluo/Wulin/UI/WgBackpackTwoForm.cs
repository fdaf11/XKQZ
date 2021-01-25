using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000345 RID: 837
	public class WgBackpackTwoForm : Widget
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x000A62C0 File Offset: 0x000A44C0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgBackpackTwoForm.<>f__switch$map34 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("BackpackTwoForm", 0);
					dictionary.Add("BaclpackScrollBar", 1);
					dictionary.Add("BackpackScrollView", 2);
					dictionary.Add("GridBackpackEquip", 3);
					dictionary.Add("IconCloseTwoForm", 4);
					WgBackpackTwoForm.<>f__switch$map34 = dictionary;
				}
				int num;
				if (WgBackpackTwoForm.<>f__switch$map34.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_BackpackTwoForm = sender;
						break;
					case 1:
						this.m_BaclpackScrollBar = sender;
						break;
					case 2:
						this.m_BackpackScrollView = sender;
						break;
					case 3:
						this.m_GridBackpackEquip = sender;
						break;
					case 4:
					{
						Control control = sender;
						this.m_IconCloseTwoForm = control;
						this.m_IconCloseTwoForm.OnClick += this.IconCloseTwoFormOnClick;
						this.m_IconCloseTwoForm.OnHover += this.IconCloseTwoFormOnHover;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					}
				}
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		public void IconCloseTwoFormOnClick(GameObject go)
		{
			if (this.OnCloseClick != null)
			{
				this.OnCloseClick.Invoke(go);
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0000C709 File Offset: 0x0000A909
		public void IconCloseTwoFormOnHover(GameObject go, bool hover)
		{
			if (this.OnCloseHover != null)
			{
				this.OnCloseHover.Invoke(go, hover);
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x000A63EC File Offset: 0x000A45EC
		public void SetBackpackEquipData(CharacterData cdata, List<BackpackNewDataNode> sortList, int iType)
		{
			this.firstOpen = !this.Obj.activeSelf;
			this.Obj.SetActive(true);
			this.m_ShowList.Clear();
			for (int i = 0; i < sortList.Count; i++)
			{
				bool flag = BackpackStatus.m_Instance.CheckItemUse(cdata, sortList[i]._ItemDataNode);
				if (flag)
				{
					for (int j = 0; j < sortList[i].m_iAmount; j++)
					{
						this.m_ShowList.Add(sortList[i]);
					}
				}
			}
			int num = this.m_ShowList.Count + 1;
			if (this.m_WgBackpackEquip.Count < num)
			{
				this.CreatBackpackEquip(num - this.m_WgBackpackEquip.Count);
			}
			int num2 = 0;
			for (int k = 0; k < this.m_WgBackpackEquip.Count; k++)
			{
				if (k == 0)
				{
					this.m_WgBackpackEquip[k].SetRemoveEquip(iType);
					this.m_WgBackpackEquip[k].SetScrollBar = new Action<GameObject>(this.SetScrollBar);
				}
				else
				{
					this.m_WgBackpackEquip[k].Obj.SetActive(false);
					if (num2 < this.m_ShowList.Count)
					{
						this.m_WgBackpackEquip[k].SetWgBackpackEquip(this.m_ShowList[num2]);
						this.m_WgBackpackEquip[k].SetScrollBar = new Action<GameObject>(this.SetScrollBar);
						num2++;
					}
				}
			}
			this.m_GridBackpackEquip.GetComponent<UIGrid>().Reposition();
			if (this.firstOpen)
			{
				this.m_iLastSelect = 0;
				this.m_BaclpackScrollBar.sliderValue = 0f;
				this.firstOpen = false;
			}
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x000A65C0 File Offset: 0x000A47C0
		private void CreatBackpackEquip(int max)
		{
			this.ParentLayer.CreateUIWidget<WgBackpackEquip>("BackpackEquip", this.m_GridBackpackEquip.GameObject, this.m_BackpackEquip, this.m_WgBackpackEquip, max);
			for (int i = 0; i < this.m_WgBackpackEquip.Count; i++)
			{
				this.m_WgBackpackEquip[i].SetCurrent = this.SetCurrent;
				this.m_WgBackpackEquip[i].UseEquip = this.UseEquip;
				this.m_WgBackpackEquip[i].RemoveEqupip = this.RemoveEqupip;
			}
			this.m_GridBackpackEquip.GetComponent<UIGrid>().Reposition();
			this.m_BaclpackScrollBar.sliderValue = 0f;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x000A6678 File Offset: 0x000A4878
		public void SetScrollBar(GameObject go)
		{
			int num = this.m_WgBackpackEquip.FindIndex((WgBackpackEquip x) => x.Obj == go);
			if (num > this.m_iLastSelect)
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_ShowList.Count + 1, false, this.m_BaclpackScrollBar);
			}
			else
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_ShowList.Count + 1, true, this.m_BaclpackScrollBar);
			}
			this.m_iLastSelect = num;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000A6704 File Offset: 0x000A4904
		private void Update()
		{
			if (this.m_BackpackTwoForm != null && this.m_BackpackTwoForm.GameObject.activeSelf)
			{
				this.CheckMouseWheel(this.m_ShowList.Count, this.m_iTwoFormMaxAmount, this.m_BaclpackScrollBar, this.m_BackpackScrollView, false);
				this.SetMouseWheel(this.m_BaclpackScrollBar);
			}
		}

		// Token: 0x04001720 RID: 5920
		private Control m_BackpackTwoForm;

		// Token: 0x04001721 RID: 5921
		private Control m_GridBackpackEquip;

		// Token: 0x04001722 RID: 5922
		public GameObject m_BackpackEquip;

		// Token: 0x04001723 RID: 5923
		private List<WgBackpackEquip> m_WgBackpackEquip = new List<WgBackpackEquip>();

		// Token: 0x04001724 RID: 5924
		private Control m_BaclpackScrollBar;

		// Token: 0x04001725 RID: 5925
		private Control m_BackpackScrollView;

		// Token: 0x04001726 RID: 5926
		private Control m_BackpackEquipOnHover;

		// Token: 0x04001727 RID: 5927
		private Control m_IconCloseTwoForm;

		// Token: 0x04001728 RID: 5928
		private int m_iTwoFormMaxAmount = 7;

		// Token: 0x04001729 RID: 5929
		private int m_iLastSelect;

		// Token: 0x0400172A RID: 5930
		private bool firstOpen;

		// Token: 0x0400172B RID: 5931
		private List<BackpackNewDataNode> m_ShowList = new List<BackpackNewDataNode>();

		// Token: 0x0400172C RID: 5932
		public Action<GameObject> OnCloseClick;

		// Token: 0x0400172D RID: 5933
		public Action<GameObject, bool> OnCloseHover;

		// Token: 0x0400172E RID: 5934
		public Action<GameObject> SetCurrent;

		// Token: 0x0400172F RID: 5935
		public Action<GameObject> UseEquip;

		// Token: 0x04001730 RID: 5936
		public Action<GameObject, int> RemoveEqupip;
	}
}
