using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002E8 RID: 744
	public class UIBigMapEnter : UILayer
	{
		// Token: 0x06000F4C RID: 3916 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00080530 File Offset: 0x0007E730
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIBigMapEnter.<>f__switch$map6 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("BigMapEnter", 0);
					UIBigMapEnter.<>f__switch$map6 = dictionary;
				}
				int num;
				if (UIBigMapEnter.<>f__switch$map6.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						this.m_BigMapEnter = sender;
						this.m_BigMapEnter.OnClick += this.OnBigMapEnterClick;
					}
				}
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0000A50B File Offset: 0x0000870B
		public void SetUI(BigMapNode node)
		{
			this.currentNode = node;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000805B4 File Offset: 0x0007E7B4
		private void OnBigMapEnterClick(GameObject go)
		{
			if (GameGlobal.m_bCFormOpen)
			{
				return;
			}
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.currentNode != null)
			{
				if (this.currentNode.MapIDNode != null)
				{
					GameGlobal.m_TransferPos = this.currentNode.MapIDNode.Pos;
					GameGlobal.m_fDir = this.currentNode.MapIDNode.AngleY;
				}
				Game.LoadScene(this.currentNode.NodeID);
				this.Hide();
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0000A514 File Offset: 0x00008714
		public void Enter()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.OnBigMapEnterClick(null);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0000A529 File Offset: 0x00008729
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			this.m_BigMapEnter.GameObject.SetActive(true);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0000A54F File Offset: 0x0000874F
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			this.m_BigMapEnter.GameObject.SetActive(false);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0008063C File Offset: 0x0007E83C
		private void Update()
		{
			if (!this.m_bShow)
			{
				return;
			}
			if (GameGlobal.m_bCFormOpen || GameGlobal.m_bPlayerTalk)
			{
				this.m_BigMapEnter.GameObject.SetActive(false);
			}
			else
			{
				this.m_BigMapEnter.GameObject.SetActive(true);
			}
		}

		// Token: 0x0400124B RID: 4683
		private Control m_BigMapEnter;

		// Token: 0x0400124C RID: 4684
		private BigMapNode currentNode;
	}
}
