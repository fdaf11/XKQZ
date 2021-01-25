using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002FF RID: 767
	public class UIDate : UILayer
	{
		// Token: 0x0600104D RID: 4173 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0008D6C0 File Offset: 0x0008B8C0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIDate.<>f__switch$map9 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Group", 0);
					dictionary.Add("LabelMonth", 1);
					dictionary.Add("LabelYear", 2);
					dictionary.Add("LabelPosition", 3);
					UIDate.<>f__switch$map9 = dictionary;
				}
				int num;
				if (UIDate.<>f__switch$map9.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_LabelMonth = sender;
						break;
					case 2:
						this.m_LabelYear = sender;
						break;
					case 3:
						this.m_LabelPosition = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0000AAFF File Offset: 0x00008CFF
		public override void Show()
		{
			this.m_Group.GameObject.SetActive(false);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0000AB12 File Offset: 0x00008D12
		public override void Hide()
		{
			this.SetYearMonth();
			this.SetScene();
			this.m_Group.GameObject.SetActive(true);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0008D794 File Offset: 0x0008B994
		public void SetYearMonth()
		{
			YoungHeroTimeNode youngHeroTimeNode = new YoungHeroTimeNode();
			youngHeroTimeNode = YoungHeroTime.m_instance.GetNowTime();
			this.m_iMonth = youngHeroTimeNode.iMonth;
			this.m_iYear = youngHeroTimeNode.iYear;
			this.m_iWeek = youngHeroTimeNode.iweek;
			int num = 110040;
			this.m_LabelYear.Text = Game.StringTable.GetString(110028);
			Control labelYear = this.m_LabelYear;
			labelYear.Text += Game.StringTable.GetString(num + this.m_iYear);
			Control labelYear2 = this.m_LabelYear;
			labelYear2.Text += Game.StringTable.GetString(110029);
			this.m_LabelMonth.Text = Game.StringTable.GetString(num + this.m_iMonth);
			Control labelMonth = this.m_LabelMonth;
			labelMonth.Text += Game.StringTable.GetString(110030);
			Control labelMonth2 = this.m_LabelMonth;
			labelMonth2.Text += Game.StringTable.GetString(110030 + this.m_iWeek);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0008D8B4 File Offset: 0x0008BAB4
		public void SetScene()
		{
			this.m_strScene = Application.loadedLevelName;
			string mapName = Game.MapID.GetMapName(this.m_strScene);
			if (mapName == null)
			{
				Debug.LogError("沒有這個MapID");
			}
			this.m_LabelPosition.Text = mapName;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x04001388 RID: 5000
		private int m_iMonth;

		// Token: 0x04001389 RID: 5001
		private int m_iYear;

		// Token: 0x0400138A RID: 5002
		private int m_iWeek;

		// Token: 0x0400138B RID: 5003
		private string m_strScene;

		// Token: 0x0400138C RID: 5004
		private Control m_LabelMonth;

		// Token: 0x0400138D RID: 5005
		private Control m_LabelYear;

		// Token: 0x0400138E RID: 5006
		private Control m_LabelPosition;

		// Token: 0x0400138F RID: 5007
		private Control m_Group;
	}
}
