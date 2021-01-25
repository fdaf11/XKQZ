using System;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class LogicLamp : MonoBehaviour
{
	// Token: 0x0600272E RID: 10030 RVA: 0x00019E3C File Offset: 0x0001803C
	private void OnGUI()
	{
		LogicGlobal.GlobalGUI();
		GUILayout.Label("Lamp rope physics test (procedural rope linked to a dynamic object)", new GUILayoutOption[0]);
		GUILayout.Label("Move the mouse while holding down the left button to move the camera", new GUILayoutOption[0]);
		GUILayout.Label("Use the spacebar to shoot balls and aim for the lamp to test the physics", new GUILayoutOption[0]);
	}
}
