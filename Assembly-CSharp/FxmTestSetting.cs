using System;
using UnityEngine;

// Token: 0x02000407 RID: 1031
public class FxmTestSetting : MonoBehaviour
{
	// Token: 0x06001907 RID: 6407 RVA: 0x000CB350 File Offset: 0x000C9550
	public string[] GetPlayContents()
	{
		int num = 3;
		GUIContent[] hcEffectControls_Play = FxmTestControls.GetHcEffectControls_Play(0f, 0f, this.m_fPlayToolbarTimes[1], this.m_fPlayToolbarTimes[num], this.m_fPlayToolbarTimes[num + 1], this.m_fPlayToolbarTimes[num + 2], this.m_fPlayToolbarTimes[num + 3], this.m_fPlayToolbarTimes[num + 4]);
		return NgConvert.ContentsToStrings(hcEffectControls_Play);
	}

	// Token: 0x04001D66 RID: 7526
	public int m_nPlayIndex;

	// Token: 0x04001D67 RID: 7527
	public int m_nTransIndex;

	// Token: 0x04001D68 RID: 7528
	public FxmTestControls.AXIS m_nTransAxis = FxmTestControls.AXIS.Z;

	// Token: 0x04001D69 RID: 7529
	public float m_fTransRate = 1f;

	// Token: 0x04001D6A RID: 7530
	public float m_fStartPosition;

	// Token: 0x04001D6B RID: 7531
	public float m_fDistPerTime = 10f;

	// Token: 0x04001D6C RID: 7532
	public int m_nRotateIndex;

	// Token: 0x04001D6D RID: 7533
	public int m_nMultiShotCount = 1;

	// Token: 0x04001D6E RID: 7534
	protected float[] m_fPlayToolbarTimes = new float[]
	{
		1f,
		1f,
		1f,
		0.3f,
		0.6f,
		1f,
		2f,
		3f
	};
}
