using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034F RID: 847
	public class WgGroupSkill : Widget
	{
		// Token: 0x06001355 RID: 4949 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x000A798C File Offset: 0x000A5B8C
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgGroupSkill.<>f__switch$map3E == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Name", 0);
					dictionary.Add("Level", 1);
					dictionary.Add("IconUsing", 2);
					dictionary.Add("OnLbl", 3);
					WgGroupSkill.<>f__switch$map3E = dictionary;
				}
				int num;
				if (WgGroupSkill.<>f__switch$map3E.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.Name = control;
						this.Name.Collider.enabled = false;
						control.OnClick += this.LblOnClick;
						control.OnHover += this.LelOnHover;
						control.OnKeySelect += this.LelKeySelect;
						this.ParentLayer.SetInputButton(1, control.Listener);
						break;
					case 1:
						this.Level = control;
						break;
					case 2:
						this.Using = control;
						break;
					case 3:
						this.OnLbl = control;
						break;
					}
				}
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000A7AAC File Offset: 0x000A5CAC
		private void LelKeySelect(GameObject go, bool isSelect)
		{
			this.OnLbl.GameObject.SetActive(isSelect);
			if (isSelect)
			{
				this.ParentLayer.ShowKeySelect(go, new Vector3(-30f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
			}
			else
			{
				this.ParentLayer.HideKeySelect();
			}
			if (this.GroupSkillOnHover != null)
			{
				this.GroupSkillOnHover.Invoke(go, isSelect);
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000A7B20 File Offset: 0x000A5D20
		private void LelOnHover(GameObject go, bool isOver)
		{
			this.OnLbl.GameObject.SetActive(isOver);
			if (this.GroupSkillOnHover != null)
			{
				this.GroupSkillOnHover.Invoke(go, isOver);
			}
			if (isOver && this.SetCurrent != null)
			{
				this.SetCurrent.Invoke(go);
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000A7B74 File Offset: 0x000A5D74
		private void LblOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			PracticeDataNode component = go.transform.parent.GetComponent<PracticeDataNode>();
			if (component == null)
			{
				return;
			}
			if (this.GroupSkillOnClick != null)
			{
				this.GroupSkillOnClick.Invoke(component);
			}
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		public void ReSetGroupSkill()
		{
			this.Obj.SetActive(false);
			this.Name.Collider.enabled = false;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x000A7BC4 File Offset: 0x000A5DC4
		public void SetGroupSkillText(NpcRoutine routine)
		{
			this.Obj.SetActive(true);
			this.Name.Collider.enabled = true;
			this.Obj.GetComponent<PracticeDataNode>().SkillData = routine;
			if (routine.m_Routine.m_iSkillType == 3 || routine.m_Routine.m_iSkillType == 1)
			{
				this.Name.Text = "[40FF40]" + routine.m_Routine.m_strRoutineName + "[-]";
			}
			else
			{
				this.Name.Text = routine.m_Routine.m_strRoutineName;
			}
			this.Level.Text = routine.iLevel.ToString();
			this.SetUsing(false);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000A7C80 File Offset: 0x000A5E80
		public void SetGroupSkillText(NpcNeigong neigong)
		{
			this.Obj.SetActive(true);
			this.Name.Collider.enabled = true;
			this.Name.Text = neigong.m_Neigong.m_strNeigongName;
			this.Obj.GetComponent<PracticeDataNode>().SkillData = neigong;
			this.Level.Text = neigong.iLevel.ToString();
			this.SetUsing(false);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000A7CF0 File Offset: 0x000A5EF0
		public void SetGroupSkillText(int talentID)
		{
			this.Obj.SetActive(true);
			this.Name.Collider.enabled = true;
			string talentName = Game.TalentNewData.GetTalentName(talentID);
			this.Name.Text = talentName;
			this.Obj.GetComponent<PracticeDataNode>().SkillData = null;
			this.Obj.GetComponent<PracticeDataNode>().TalentID = talentID;
			this.Level.GameObject.SetActive(false);
			this.SetUsing(false);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0000C8BF File Offset: 0x0000AABF
		public void SetUsing(bool bUsing)
		{
			this.Using.GameObject.SetActive(bUsing);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0000C8D2 File Offset: 0x0000AAD2
		public bool GetUsing()
		{
			return this.Using.GameObject.activeSelf;
		}

		// Token: 0x04001767 RID: 5991
		public Control Name;

		// Token: 0x04001768 RID: 5992
		public Control Level;

		// Token: 0x04001769 RID: 5993
		public Control Using;

		// Token: 0x0400176A RID: 5994
		public Control OnLbl;

		// Token: 0x0400176B RID: 5995
		public Action<PracticeDataNode> GroupSkillOnClick;

		// Token: 0x0400176C RID: 5996
		public Action<GameObject, bool> GroupSkillOnHover;

		// Token: 0x0400176D RID: 5997
		public Action<GameObject> SetCurrent;
	}
}
