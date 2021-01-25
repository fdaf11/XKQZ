using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200031A RID: 794
	public class UIPopUI : UILayer
	{
		// Token: 0x0600113A RID: 4410 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x000947A4 File Offset: 0x000929A4
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			this.m_Group.GameObject.SetActive(true);
			Game.g_InputManager.Push(this);
			GameGlobal.m_bCFormOpen = true;
			this.current = UIEventListener.Get(this.m_Btn01.GameObject);
			this.current.onKeySelect(this.current.gameObject, true);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00094818 File Offset: 0x00092A18
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			this.m_Group.GameObject.SetActive(false);
			GameGlobal.m_bCFormOpen = false;
			this.m_title.Text = string.Empty;
			this.m_msg.Text = string.Empty;
			this.btnCallBack01 = null;
			this.btnCallBack02 = null;
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00094888 File Offset: 0x00092A88
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIPopUI.<>f__switch$map14 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
					dictionary.Add("Group", 0);
					dictionary.Add("Title", 1);
					dictionary.Add("Msg", 2);
					dictionary.Add("Btn01", 3);
					dictionary.Add("Btn02", 4);
					dictionary.Add("BtnCancel", 5);
					dictionary.Add("BtnSelect", 6);
					UIPopUI.<>f__switch$map14 = dictionary;
				}
				int num;
				if (UIPopUI.<>f__switch$map14.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_title = sender;
						break;
					case 2:
						this.m_msg = sender;
						break;
					case 3:
					{
						this.m_Btn01 = sender;
						this.m_Btn01.OnClick += this.OnBtn01Click;
						this.m_Btn01.OnKeySelect += this.OnKeySelect;
						this.m_Btn01.OnHover += this.OnHover;
						UIEventListener listener = UIEventListener.Get(sender.gameObject);
						base.SetInputButton(0, listener);
						break;
					}
					case 4:
					{
						this.m_Btn02 = sender;
						this.m_Btn02.OnClick += this.OnBtn02Click;
						this.m_Btn02.OnKeySelect += this.OnKeySelect;
						this.m_Btn02.OnHover += this.OnHover;
						UIEventListener listener2 = UIEventListener.Get(sender.gameObject);
						base.SetInputButton(0, listener2);
						break;
					}
					case 5:
					{
						this.m_btnCancel = sender;
						this.m_btnCancel.OnClick += this.OnCancelClick;
						this.m_btnCancel.OnKeySelect += this.OnKeySelect;
						this.m_btnCancel.OnHover += this.OnHover;
						UIEventListener listener3 = UIEventListener.Get(sender.gameObject);
						base.SetInputButton(0, listener3);
						break;
					}
					case 6:
						this.m_btnSelect = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0000B3A3 File Offset: 0x000095A3
		private void OnBtn01Click(GameObject go)
		{
			if (this.btnCallBack01 != null)
			{
				this.btnCallBack01.Invoke();
			}
			this.Hide();
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0000B3C1 File Offset: 0x000095C1
		private void OnBtn02Click(GameObject go)
		{
			if (this.btnCallBack02 != null)
			{
				this.btnCallBack02.Invoke();
			}
			this.Hide();
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0000B3DF File Offset: 0x000095DF
		private void OnCancelClick(GameObject go)
		{
			this.Hide();
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0000B3E7 File Offset: 0x000095E7
		private void OnHover(GameObject go, bool bselt)
		{
			this.SetControl(this.m_btnSelect, go, 0f, 5f, 0f);
			this.m_btnSelect.GameObject.SetActive(bselt);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0000B416 File Offset: 0x00009616
		private void OnKeySelect(GameObject go, bool bselt)
		{
			if (bselt)
			{
				this.ShowKeySelect(go, new Vector3(0f, -40f, 0f), KeySelect.eSelectDir.Bottom, 64, 64);
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00094AD4 File Offset: 0x00092CD4
		public void ShowPopUI(string title, string msg, string btn1, string btn2, Action callback1, Action callback2)
		{
			this.m_title.Text = title;
			this.m_msg.Text = msg;
			this.m_Btn01.Text = btn1;
			this.m_Btn02.Text = btn2;
			this.btnCallBack01 = callback1;
			this.btnCallBack02 = callback2;
			this.Show();
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040014E8 RID: 5352
		private Control m_Group;

		// Token: 0x040014E9 RID: 5353
		private Control m_title;

		// Token: 0x040014EA RID: 5354
		private Control m_msg;

		// Token: 0x040014EB RID: 5355
		private Control m_Btn01;

		// Token: 0x040014EC RID: 5356
		private Control m_Btn02;

		// Token: 0x040014ED RID: 5357
		private Control m_btnCancel;

		// Token: 0x040014EE RID: 5358
		private Control m_btnSelect;

		// Token: 0x040014EF RID: 5359
		private Action btnCallBack01;

		// Token: 0x040014F0 RID: 5360
		private Action btnCallBack02;

		// Token: 0x0200031B RID: 795
		private enum eState
		{
			// Token: 0x040014F3 RID: 5363
			enterPopUI,
			// Token: 0x040014F4 RID: 5364
			none
		}
	}
}
