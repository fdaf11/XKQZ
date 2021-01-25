using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A5 RID: 165
	public class WeaponManager : MonoBehaviour
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0002D488 File Offset: 0x0002B688
		private void Update()
		{
			if (Input.GetKeyDown(49))
			{
				ExploderUtils.SetActiveRecursively(this.RPG, false);
				ExploderUtils.SetActiveRecursively(this.Shotgun, true);
				this.Shotgun.GetComponent<ShotgunController>().OnActivate();
			}
			if (Input.GetKeyDown(50))
			{
				ExploderUtils.SetActiveRecursively(this.RPG, true);
				ExploderUtils.SetActiveRecursively(this.Shotgun, false);
				this.RPG.GetComponent<RPGController>().OnActivate();
			}
		}

		// Token: 0x040002C3 RID: 707
		public GameObject Shotgun;

		// Token: 0x040002C4 RID: 708
		public GameObject RPG;
	}
}
