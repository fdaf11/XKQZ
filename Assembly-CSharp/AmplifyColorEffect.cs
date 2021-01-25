using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[AddComponentMenu("Image Effects/Amplify Color")]
[ExecuteInEditMode]
[ImageEffectTransformsToLDR]
public sealed class AmplifyColorEffect : AmplifyColorBase
{
	// Token: 0x06000172 RID: 370 RVA: 0x00003328 File Offset: 0x00001528
	private void Start()
	{
		if (!GameGlobal.m_bLensEffects)
		{
			base.SendMessage("DisableToneMapping", 1);
			base.enabled = false;
			return;
		}
	}
}
