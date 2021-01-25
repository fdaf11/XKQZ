using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class LogicBreakableRopes : MonoBehaviour
{
	// Token: 0x06002726 RID: 10022 RVA: 0x00019DD4 File Offset: 0x00017FD4
	private void Start()
	{
		this.bBroken1 = false;
		this.bBroken2 = false;
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x0012F298 File Offset: 0x0012D498
	private void OnGUI()
	{
		LogicGlobal.GlobalGUI();
		GUILayout.Label("Breakable rope test (procedural rope and linkedobjects rope with breakable properties and notifications set)", new GUILayoutOption[0]);
		GUILayout.Label("Move the mouse while holding down the left button to move the camera", new GUILayoutOption[0]);
		GUILayout.Label("Use the spacebar to shoot balls and aim for the ropes to break them", new GUILayoutOption[0]);
		Color color = GUI.color;
		GUI.color = new Color(255f, 0f, 0f);
		if (this.bBroken1)
		{
			GUILayout.Label("Rope 1 was broken", new GUILayoutOption[0]);
		}
		if (this.bBroken2)
		{
			GUILayout.Label("Rope 2 was broken", new GUILayoutOption[0]);
		}
		GUI.color = color;
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x00019DE4 File Offset: 0x00017FE4
	private void OnRopeBreak(UltimateRope.RopeBreakEventInfo breakInfo)
	{
		if (breakInfo.rope == this.Rope1)
		{
			this.bBroken1 = true;
		}
		if (breakInfo.rope == this.Rope2)
		{
			this.bBroken2 = true;
		}
	}

	// Token: 0x04003060 RID: 12384
	public UltimateRope Rope1;

	// Token: 0x04003061 RID: 12385
	public UltimateRope Rope2;

	// Token: 0x04003062 RID: 12386
	private bool bBroken1;

	// Token: 0x04003063 RID: 12387
	private bool bBroken2;
}
