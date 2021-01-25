using System;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
public class Instruction : MonoBehaviour
{
	// Token: 0x06003032 RID: 12338 RVA: 0x0017621C File Offset: 0x0017441C
	private void Start()
	{
		this.camInst = string.Empty;
		this.camInst += "- 'w','a','s','d' key to pan the camera\n";
		this.camInst += "- 'q' and 'e' key to rotate the view angle\n";
		this.camInst += "- mouse wheel to zoom\n";
		this.camInst += "\n'Space' to Toggle unit's overlay\n";
		this.camInst += "\nRight click on any of the unit to show unit stats\n";
		this.camInst += "\nWhan an ability is selected, Right click or escape key to cancel an ability\n";
		this.camInst += "\nDuring unit placement, click on the green tile to place unit, click on placed unit to reposition it";
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x001762D0 File Offset: 0x001744D0
	private void OnGUI()
	{
		string text = "Show Instruction";
		if (this.show)
		{
			text = "Hide Instruction";
		}
		if (GUI.Button(new Rect((float)(Screen.width - 220), 10f, 120f, 30f), text))
		{
			this.show = !this.show;
		}
		if (this.show)
		{
			GUI.Label(new Rect((float)(Screen.width - 310), 45f, 300f, 500f), this.camInst);
		}
	}

	// Token: 0x04003BE4 RID: 15332
	private string camInst = string.Empty;

	// Token: 0x04003BE5 RID: 15333
	private bool show;
}
