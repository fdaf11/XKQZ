using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200011E RID: 286
	public class BigMapBillBoard : MonoBehaviour
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x000055EC File Offset: 0x000037EC
		private void Start()
		{
			if (this.m_Camera == null)
			{
				this.m_Camera = Camera.main;
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000560A File Offset: 0x0000380A
		public void SetBillBoard(string text, string icon)
		{
			this.m_Text.text = text;
			this.m_Icon.spriteName = icon;
			this.m_Icon.MarkAsChanged();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000562F File Offset: 0x0000382F
		public void SetPosition(Vector3 pos)
		{
			base.transform.position = pos;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00042804 File Offset: 0x00040A04
		private void Update()
		{
			if (this.m_Camera == null)
			{
				this.m_Camera = Camera.main;
				return;
			}
			this.m_Scaling = Vector3.Distance(base.transform.position, this.m_Camera.transform.position) / 200f;
			base.transform.localScale = Vector3.one * this.m_Scaling;
			base.transform.LookAt(base.transform.position + this.m_Camera.transform.rotation * Vector3.forward, this.m_Camera.transform.rotation * Vector3.up);
		}

		// Token: 0x0400064A RID: 1610
		public UILabel m_Text;

		// Token: 0x0400064B RID: 1611
		public UISprite m_Icon;

		// Token: 0x0400064C RID: 1612
		private Camera m_Camera;

		// Token: 0x0400064D RID: 1613
		private float m_Scaling;
	}
}
