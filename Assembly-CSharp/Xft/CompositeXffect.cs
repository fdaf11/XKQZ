using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000564 RID: 1380
	public class CompositeXffect : MonoBehaviour
	{
		// Token: 0x06002294 RID: 8852 RVA: 0x0001716B File Offset: 0x0001536B
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0010EFDC File Offset: 0x0010D1DC
		public void Initialize()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				XffectComponent component = transform.GetComponent<XffectComponent>();
				if (component == null)
				{
					XffectCache component2 = transform.GetComponent<XffectCache>();
					if (component2 != null)
					{
						component2.Init();
					}
				}
				else
				{
					component.Initialize();
					this.XffectList.Add(component);
				}
			}
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x0010F080 File Offset: 0x0010D280
		public void Active()
		{
			base.gameObject.SetActive(true);
			foreach (XffectComponent xffectComponent in this.XffectList)
			{
				xffectComponent.Active();
			}
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0010F0E8 File Offset: 0x0010D2E8
		public void DeActive()
		{
			base.gameObject.SetActive(false);
			foreach (XffectComponent xffectComponent in this.XffectList)
			{
				xffectComponent.DeActive();
			}
		}

		// Token: 0x040028F9 RID: 10489
		private List<XffectComponent> XffectList = new List<XffectComponent>();
	}
}
