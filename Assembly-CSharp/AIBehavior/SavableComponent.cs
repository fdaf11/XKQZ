using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000023 RID: 35
	public class SavableComponent : MonoBehaviour
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00002A56 File Offset: 0x00000C56
		public int GetSaveID()
		{
			this.saveId = SaveIdDistributor.GetId(this.saveId);
			return this.saveId;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002A6F File Offset: 0x00000C6F
		public void SetSaveID(int id)
		{
			this.saveId = id;
			SaveIdDistributor.SetId(id);
		}

		// Token: 0x04000073 RID: 115
		public int saveId = -1;
	}
}
