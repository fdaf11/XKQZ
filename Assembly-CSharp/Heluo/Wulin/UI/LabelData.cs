using System;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000387 RID: 903
	public class LabelData : MonoBehaviour
	{
		// Token: 0x0600150B RID: 5387 RVA: 0x000B5224 File Offset: 0x000B3424
		private void Start()
		{
			if (this.m_iStringID == 0)
			{
				return;
			}
			if (Game.instance == null)
			{
				return;
			}
			if (base.gameObject.GetComponent<UILabel>() == null)
			{
				return;
			}
			string @string = Game.StringTable.GetString(this.m_iStringID);
			base.gameObject.GetComponent<UILabel>().text = @string;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040019A3 RID: 6563
		public int m_iIndex;

		// Token: 0x040019A4 RID: 6564
		public int m_iStringID;

		// Token: 0x040019A5 RID: 6565
		public int m_iHoverID;

		// Token: 0x040019A6 RID: 6566
		public bool m_bHave;

		// Token: 0x040019A7 RID: 6567
		public int m_iKind;
	}
}
