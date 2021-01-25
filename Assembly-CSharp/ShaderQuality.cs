using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
[AddComponentMenu("NGUI/Examples/Shader Quality")]
[ExecuteInEditMode]
public class ShaderQuality : MonoBehaviour
{
	// Token: 0x06001A0A RID: 6666 RVA: 0x000D093C File Offset: 0x000CEB3C
	private void Update()
	{
		int num = (QualitySettings.GetQualityLevel() + 1) * 100;
		if (this.mCurrent != num)
		{
			this.mCurrent = num;
			Shader.globalMaximumLOD = this.mCurrent;
		}
	}

	// Token: 0x04001ED2 RID: 7890
	private int mCurrent = 600;
}
