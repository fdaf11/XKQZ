using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000353 RID: 851
	public class WgPracticeTwoForm : Widget
	{
		// Token: 0x0600137B RID: 4987 RVA: 0x000A85C4 File Offset: 0x000A67C4
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgPracticeTwoForm.<>f__switch$map42 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("PracticeTwoForm", 0);
					dictionary.Add("PracticeScrollBar", 1);
					dictionary.Add("PracticeScrollView", 2);
					dictionary.Add("GridPractice", 3);
					dictionary.Add("IconCloseTwoForm", 4);
					WgPracticeTwoForm.<>f__switch$map42 = dictionary;
				}
				int num;
				if (WgPracticeTwoForm.<>f__switch$map42.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_PracticeTwoForm = sender;
						break;
					case 1:
						this.m_PracticeScrollBar = sender;
						break;
					case 2:
						this.m_PracticeScrollView = sender;
						break;
					case 3:
						this.m_GridPractice = sender;
						break;
					case 4:
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

		// Token: 0x0600137C RID: 4988 RVA: 0x000A86EC File Offset: 0x000A68EC
		public void UpdatePracticeTwoForm(List<NpcRoutine> routineList, List<NpcNeigong> neigongList)
		{
			this.firstOpen = !this.Obj.activeSelf;
			this.Obj.SetActive(true);
			int num = routineList.Count + neigongList.Count;
			this.m_iMaxCount = num;
			if (this.m_WgPracticeNode.Count < num)
			{
				this.CreatPractice(num - this.m_WgPracticeNode.Count);
			}
			for (int i = 0; i < this.m_WgPracticeNode.Count; i++)
			{
				this.m_WgPracticeNode[i].Obj.SetActive(false);
				if (i < routineList.Count + neigongList.Count)
				{
					this.m_WgPracticeNode[i].Obj.SetActive(true);
					this.m_WgPracticeNode[i].SetScrollBar = new Action<GameObject>(this.SetScrollBar);
					if (i < routineList.Count)
					{
						this.m_WgPracticeNode[i].SetWgPracticeNode(routineList[i]);
					}
					else
					{
						this.m_WgPracticeNode[i].SetWgPracticeNode(neigongList[i - routineList.Count]);
					}
				}
			}
			this.m_GridPractice.GetComponent<UIGrid>().Reposition();
			if (this.firstOpen)
			{
				this.m_iLastSelect = 0;
				this.firstOpen = false;
				this.m_PracticeScrollBar.sliderValue = 0f;
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000A8850 File Offset: 0x000A6A50
		private void CreatPractice(int max)
		{
			this.ParentLayer.CreateUIWidget<WgPracticeNode>("PracticeNode", this.m_GridPractice.GameObject, this.m_PracticeNode, this.m_WgPracticeNode, max);
			this.m_GridPractice.GetComponent<UIGrid>().Reposition();
			this.m_PracticeScrollBar.sliderValue = 0f;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000A88A8 File Offset: 0x000A6AA8
		public void SetScrollBar(GameObject go)
		{
			int num = this.m_WgPracticeNode.FindIndex((WgPracticeNode x) => x.Obj == go);
			if (num > this.m_iLastSelect)
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_iMaxCount, false, this.m_PracticeScrollBar);
			}
			else
			{
				base.SetScrollBar(num, this.m_iTwoFormMaxAmount, this.m_iMaxCount, true, this.m_PracticeScrollBar);
			}
			this.m_iLastSelect = num;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000A8928 File Offset: 0x000A6B28
		private void Update()
		{
			if (this.m_PracticeTwoForm != null && this.m_PracticeTwoForm.GameObject.activeSelf)
			{
				this.CheckMouseWheel(this.m_WgPracticeNode.Count, this.m_iTwoFormMaxAmount, this.m_PracticeScrollBar, this.m_PracticeScrollView, false);
				this.SetMouseWheel(this.m_PracticeScrollBar);
			}
		}

		// Token: 0x0400178A RID: 6026
		private Control m_PracticeTwoForm;

		// Token: 0x0400178B RID: 6027
		private Control m_GridPractice;

		// Token: 0x0400178C RID: 6028
		public GameObject m_PracticeNode;

		// Token: 0x0400178D RID: 6029
		private List<WgPracticeNode> m_WgPracticeNode = new List<WgPracticeNode>();

		// Token: 0x0400178E RID: 6030
		private Control m_PracticeScrollBar;

		// Token: 0x0400178F RID: 6031
		private Control m_PracticeScrollView;

		// Token: 0x04001790 RID: 6032
		private int m_iTwoFormMaxAmount = 7;

		// Token: 0x04001791 RID: 6033
		private int m_iMaxCount;

		// Token: 0x04001792 RID: 6034
		private int m_iLastSelect;

		// Token: 0x04001793 RID: 6035
		private bool firstOpen;
	}
}
