using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200035D RID: 861
	public class WgUnitOrder : Widget
	{
		// Token: 0x060013AA RID: 5034 RVA: 0x0000CB06 File Offset: 0x0000AD06
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			this.m_UIDLCCharacter = (layer as UIDLCCharacter);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000A9F24 File Offset: 0x000A8124
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgUnitOrder.<>f__switch$map47 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("defend", 0);
					dictionary.Add("disband", 1);
					dictionary.Add("training", 2);
					WgUnitOrder.<>f__switch$map47 = dictionary;
				}
				int num;
				if (WgUnitOrder.<>f__switch$map47.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_defend = control;
						this.m_defend.OnClick += this.OnClick;
						break;
					case 1:
						this.m_disband = control;
						this.m_disband.OnClick += this.OnClick;
						break;
					case 2:
						this.m_training = control;
						this.m_training.OnClick += this.OnClick;
						break;
					}
				}
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x000AA014 File Offset: 0x000A8214
		public void OnTrainingOK()
		{
			UIDLCCharacter uidlccharacter = this.ParentLayer as UIDLCCharacter;
			CtrlDLCCharacter ctrlCharacter = uidlccharacter.m_CtrlCharacter;
			CharacterData curCaraData = ctrlCharacter.m_CurCaraData;
			int num = curCaraData.iPrice * curCaraData.iLevel * 2;
			int money = BackpackStatus.m_Instance.GetMoney();
			if (money < num)
			{
				string @string = Game.StringTable.GetString(200081);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				return;
			}
			BackpackStatus.m_Instance.ChangeMoney(-num);
			TeamStatus.m_Instance.DLCUnitLevelUP(ctrlCharacter.NpcID);
			ctrlCharacter.UpdateCaracterData();
			this.m_UIDLCCharacter.CloseCheckUI();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0000CB1B File Offset: 0x0000AD1B
		public void OnCancel()
		{
			this.m_UIDLCCharacter.CloseCheckUI();
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000AA0B0 File Offset: 0x000A82B0
		public void OnDisbandOK()
		{
			UIDLCCharacter uidlccharacter = this.ParentLayer as UIDLCCharacter;
			CtrlDLCCharacter ctrlCharacter = uidlccharacter.m_CtrlCharacter;
			TeamStatus.m_Instance.DeleteDLCUnit(ctrlCharacter.NpcID);
			BackpackStatus.m_Instance.ChangeMoney(ctrlCharacter.m_CurCaraData.iPrice * ctrlCharacter.m_CurCaraData.iLevel);
			if (TeamStatus.m_Instance.GetDLCUnitList().Count <= 0)
			{
				Game.UI.Hide<UIDLCCharacter>();
				string @string = Game.StringTable.GetString(990077);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				this.m_UIDLCCharacter.CloseCheckUI();
				return;
			}
			string dlcunitGID = TeamStatus.m_Instance.GetDLCUnitGID(null);
			ctrlCharacter.SetCurNpcID(dlcunitGID);
			this.m_UIDLCCharacter.CloseCheckUI();
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000AA16C File Offset: 0x000A836C
		public void OnDefendOK()
		{
			UIDLCCharacter uidlccharacter = this.ParentLayer as UIDLCCharacter;
			CtrlDLCCharacter ctrlCharacter = uidlccharacter.m_CtrlCharacter;
			TeamStatus.m_Instance.DeleteDLCUnit(ctrlCharacter.NpcID);
			TeamStatus.m_Instance.AddPrestigePoints(ctrlCharacter.m_CurCaraData.iHonor * ctrlCharacter.m_CurCaraData.iLevel);
			if (TeamStatus.m_Instance.GetDLCUnitList().Count <= 0)
			{
				Game.UI.Hide<UIDLCCharacter>();
				string @string = Game.StringTable.GetString(990077);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				this.m_UIDLCCharacter.CloseCheckUI();
				return;
			}
			string dlcunitGID = TeamStatus.m_Instance.GetDLCUnitGID(null);
			ctrlCharacter.SetCurNpcID(dlcunitGID);
			this.m_UIDLCCharacter.CloseCheckUI();
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000AA228 File Offset: 0x000A8428
		public void OnClick(GameObject obj)
		{
			string title = string.Empty;
			string text = string.Empty;
			string lblOK = string.Empty;
			string lblCancel = string.Empty;
			UIDLCCharacter uidlccharacter = this.ParentLayer as UIDLCCharacter;
			CtrlDLCCharacter ctrlCharacter = uidlccharacter.m_CtrlCharacter;
			CharacterData curCaraData = ctrlCharacter.m_CurCaraData;
			string name = obj.name;
			if (name != null)
			{
				if (WgUnitOrder.<>f__switch$map48 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("defend", 0);
					dictionary.Add("disband", 1);
					dictionary.Add("training", 2);
					WgUnitOrder.<>f__switch$map48 = dictionary;
				}
				int num;
				if (WgUnitOrder.<>f__switch$map48.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						title = Game.StringTable.GetString(990081);
						text = Game.StringTable.GetString(990103);
						lblOK = Game.StringTable.GetString(100009);
						lblCancel = Game.StringTable.GetString(100005);
						text = string.Format(text, curCaraData.iHonor * curCaraData.iLevel);
						this.m_UIDLCCharacter.ShowWgCheckUpgradeUnit(title, text, lblOK, lblCancel, new Action(this.OnDefendOK), new Action(this.OnCancel));
						break;
					case 1:
						title = Game.StringTable.GetString(990080);
						text = Game.StringTable.GetString(990102);
						lblOK = Game.StringTable.GetString(100009);
						lblCancel = Game.StringTable.GetString(100005);
						text = string.Format(text, curCaraData.iPrice * curCaraData.iLevel);
						this.m_UIDLCCharacter.ShowWgCheckUpgradeUnit(title, text, lblOK, lblCancel, new Action(this.OnDisbandOK), new Action(this.OnCancel));
						break;
					case 2:
						if (curCaraData.iLevel >= 4)
						{
							string @string = Game.StringTable.GetString(990108);
							Game.UI.Get<UIMapMessage>().SetMsg(@string);
							return;
						}
						title = Game.StringTable.GetString(990078);
						text = Game.StringTable.GetString(990100);
						lblOK = Game.StringTable.GetString(100009);
						lblCancel = Game.StringTable.GetString(100005);
						text = string.Format(text, curCaraData.iPrice * curCaraData.iLevel * 2);
						this.m_UIDLCCharacter.ShowWgCheckUpgradeUnit(title, text, lblOK, lblCancel, new Action(this.OnTrainingOK), new Action(this.OnCancel));
						break;
					}
				}
			}
		}

		// Token: 0x040017CF RID: 6095
		private UIDLCCharacter m_UIDLCCharacter;

		// Token: 0x040017D0 RID: 6096
		private Control m_defend;

		// Token: 0x040017D1 RID: 6097
		private Control m_disband;

		// Token: 0x040017D2 RID: 6098
		private Control m_training;

		// Token: 0x040017D3 RID: 6099
		public Action OnDisbandClick;

		// Token: 0x040017D4 RID: 6100
		public Action OnTrainingClick;
	}
}
