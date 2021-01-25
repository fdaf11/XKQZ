using System;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class LogicSimpleRopes : MonoBehaviour
{
	// Token: 0x06002734 RID: 10036 RVA: 0x00019EC8 File Offset: 0x000180C8
	private void OnGUI()
	{
		LogicGlobal.GlobalGUI();
		GUILayout.Label("Simple persistent rope test (procedural rope and linkedobjects rope)", new GUILayoutOption[0]);
		GUILayout.Label("Move the mouse while holding down the left button to move the camera", new GUILayoutOption[0]);
		GUILayout.Label("Use the spacebar to shoot balls and aim for the ropes to test the physics", new GUILayoutOption[0]);
	}
}
