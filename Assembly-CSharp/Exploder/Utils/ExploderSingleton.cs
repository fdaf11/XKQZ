using System;
using UnityEngine;

namespace Exploder.Utils
{
	// Token: 0x020000BB RID: 187
	public class ExploderSingleton : MonoBehaviour
	{
		// Token: 0x060003E9 RID: 1001 RVA: 0x00004B02 File Offset: 0x00002D02
		private void Start()
		{
			ExploderSingleton.ExploderInstance = base.gameObject.GetComponent<ExploderObject>();
		}

		// Token: 0x04000315 RID: 789
		public static ExploderObject ExploderInstance;
	}
}
