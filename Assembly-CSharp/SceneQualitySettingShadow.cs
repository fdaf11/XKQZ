using System;
using UnityEngine;

// Token: 0x02000693 RID: 1683
public class SceneQualitySettingShadow : MonoBehaviour
{
	// Token: 0x060028E1 RID: 10465 RVA: 0x0001AE6C File Offset: 0x0001906C
	private void Start()
	{
		QualitySettings.shadowDistance = (float)this.ShadowDistance;
		Debug.Log("SceneQualitySettingShadow Distance = " + this.ShadowDistance.ToString());
	}

	// Token: 0x040033BC RID: 13244
	public int ShadowDistance = 500;
}
