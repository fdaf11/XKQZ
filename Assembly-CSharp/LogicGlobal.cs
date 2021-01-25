using System;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class LogicGlobal : MonoBehaviour
{
	// Token: 0x0600272A RID: 10026 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x00019E20 File Offset: 0x00018020
	public static void GlobalGUI()
	{
		GUILayout.Label("Press 1-4 to select different sample scenes", new GUILayoutOption[0]);
		GUILayout.Space(20f);
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x0012F338 File Offset: 0x0012D538
	private void Update()
	{
		if (Input.GetKeyDown(49))
		{
			Application.LoadLevel(0);
		}
		if (Input.GetKeyDown(50))
		{
			Application.LoadLevel(1);
		}
		if (Input.GetKeyDown(51))
		{
			Application.LoadLevel(2);
		}
		if (Input.GetKeyDown(52))
		{
			Application.LoadLevel(3);
		}
	}
}
