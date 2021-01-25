using System;
using UnityEngine;

// Token: 0x02000697 RID: 1687
public class shadowDistance : MonoBehaviour
{
	// Token: 0x060028ED RID: 10477 RVA: 0x0001AF5B File Offset: 0x0001915B
	private void Start()
	{
		if (QualitySettings.GetQualityLevel() == 5)
		{
			QualitySettings.shadowDistance = (float)this.ShadowDistance;
		}
	}

	// Token: 0x040033C5 RID: 13253
	public int ShadowDistance = 100;
}
