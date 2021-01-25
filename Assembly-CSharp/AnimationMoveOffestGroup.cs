using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class AnimationMoveOffestGroup : MonoBehaviour
{
	// Token: 0x060004CA RID: 1226 RVA: 0x0000501F File Offset: 0x0000321F
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0400047C RID: 1148
	public List<AnimationMoveOffest> animationMoveOffectList;
}
