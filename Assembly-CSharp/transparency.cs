using System;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class transparency : MonoBehaviour
{
	// Token: 0x0600223F RID: 8767 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x0010C4FC File Offset: 0x0010A6FC
	private void OnGUI()
	{
		GUI.Label(new Rect(300f, 25f, 200f, 20f), "Clouds Density:");
		transparency.density = GUI.HorizontalSlider(new Rect(300f, 45f, 130f, 20f), transparency.density, 0.5f, 1.5f);
		GUI.Label(new Rect(600f, 25f, 200f, 20f), "Clouds Darkness:");
		transparency.darkness = GUI.HorizontalSlider(new Rect(600f, 45f, 130f, 20f), transparency.darkness, 0f, 0.4f);
		GUI.Label(new Rect(760f, 25f, 200f, 80f), "It takes time to reduce cloudes density (old particle have to die). Due to the particle based system, total number of unique clouds is unlimited");
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002866 RID: 10342
	private Transform cl1;

	// Token: 0x04002867 RID: 10343
	private Transform cl2;

	// Token: 0x04002868 RID: 10344
	private Transform cl3;

	// Token: 0x04002869 RID: 10345
	private Transform cl4;

	// Token: 0x0400286A RID: 10346
	private Transform cl5;

	// Token: 0x0400286B RID: 10347
	public static float darkness;

	// Token: 0x0400286C RID: 10348
	public static float density = 1f;
}
