using System;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
public class DemoMainMenu : MonoBehaviour
{
	// Token: 0x0600302E RID: 12334 RVA: 0x00175F18 File Offset: 0x00174118
	private void Start()
	{
		this.tooltip = new string[8];
		this.tooltip[0] = "Series of continous levels in random order that carry allow unit selection before each battle\nThe unit and point earns in each level are carried forth to next level.";
		this.tooltip[1] = "A simple level, each unit takes turn to move based on their stats in each round\nUnits gain AP over each round which is then use to perform special ability\nThe level is procedurally generated in every loading";
		this.tooltip[2] = "A space theme level, each faction takes turn to move all units in each round. Unit switching is not allowed.\nUnits gain AP over each round which is then use to perform special ability";
		this.tooltip[3] = "A tropic theme level, each faction take turn to move a single unit, a round is completed when all unit is moved\nUnits is given full AP at each turn but every movement, attack, and ability usage will cost AP.";
		this.tooltip[4] = "A test level showcase the XCom style cover system with fog-of-war system\nEach faction takes turn to move all the units.\nUnits gain AP over each round which is then use to perform special ability";
		this.tooltip[5] = "A test level with square grid\nEach faction takes turn to move all the units.\nUnits gain AP over each round which is then use to perform special ability";
		this.loadPara = ((!this.useNGUIScene) ? string.Empty : "NGUI");
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x00175FA0 File Offset: 0x001741A0
	private void OnGUI()
	{
		float num = (float)(Screen.height / 2 - 170);
		float num2 = 50f;
		GUIContent guicontent = new GUIContent("Mini\nCampaign", "1");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num += num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExPreBattleSetup" + this.loadPara);
		}
		guicontent = new GUIContent("Basic\n(single level)", "2");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num += num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExSingleBasic" + this.loadPara);
		}
		guicontent = new GUIContent("Space\n(single level)", "3");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num += num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExSingleSpace" + this.loadPara);
		}
		guicontent = new GUIContent("Tropic\n(single level)", "4");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num += num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExSingleTropic" + this.loadPara);
		}
		guicontent = new GUIContent("Cover System\n(single level)", "5");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num += num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExCoverSystem" + this.loadPara);
		}
		guicontent = new GUIContent("Square Grid\n(single level)", "5");
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), num + num2, 120f, 40f), guicontent))
		{
			Application.LoadLevel("ExSquare" + this.loadPara);
		}
		if (GUI.tooltip != string.Empty)
		{
			int num3 = int.Parse(GUI.tooltip) - 1;
			GUIStyle guistyle = new GUIStyle();
			guistyle.alignment = 1;
			guistyle.fontStyle = 1;
			guistyle.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, (float)Screen.height * 0.75f + 30f, (float)Screen.width, 200f), this.tooltip[num3], guistyle);
		}
	}

	// Token: 0x04003BE1 RID: 15329
	private string[] tooltip;

	// Token: 0x04003BE2 RID: 15330
	public bool useNGUIScene;

	// Token: 0x04003BE3 RID: 15331
	private string loadPara = string.Empty;
}
