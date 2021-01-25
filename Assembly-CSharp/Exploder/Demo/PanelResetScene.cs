using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x0200009A RID: 154
	public class PanelResetScene : UseObject
	{
		// Token: 0x06000350 RID: 848 RVA: 0x00004566 File Offset: 0x00002766
		private void Start()
		{
			this.objectList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Exploder"));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0002C708 File Offset: 0x0002A908
		public override void Use()
		{
			base.Use();
			ExploderUtils.ClearLog();
			foreach (GameObject obj in this.objectList)
			{
				ExploderUtils.SetActiveRecursively(obj, true);
				ExploderUtils.SetVisible(obj, true);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000457D File Offset: 0x0000277D
		private void Update()
		{
			if (Input.GetKeyDown(114))
			{
				this.Use();
			}
		}

		// Token: 0x04000289 RID: 649
		private List<GameObject> objectList;
	}
}
