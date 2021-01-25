using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class PlayCtrlDebug : MonoBehaviour
{
	// Token: 0x0600063E RID: 1598 RVA: 0x00045248 File Offset: 0x00043448
	private void OnGUI()
	{
		if (this.m_PlayerFSM == null || !this.ShowDebug)
		{
			return;
		}
		GUI.Box(new Rect(610f, 10f, 400f, 200f), string.Empty);
		this.m_PlayerFSM.m_IsOnNaveMesh = GUI.Toggle(new Rect(610f, 10f, 150f, 30f), this.m_PlayerFSM.m_IsOnNaveMesh, "在NavMesh行走");
		GUI.Label(new Rect(610f, 40f, 100f, 30f), "基本速度 : ");
		this.baseSpeed = GUI.TextField(new Rect(710f, 40f, 150f, 20f), this.baseSpeed);
		GUI.Label(new Rect(610f, 70f, 100f, 30f), "加速度 : ");
		this.addSpeed = GUI.TextField(new Rect(710f, 70f, 150f, 20f), this.addSpeed);
		if (GUI.Button(new Rect(860f, 40f, 100f, 60f), "設定"))
		{
			float fbase = 5f;
			float fadd = 0.5f;
			float.TryParse(this.baseSpeed, ref fbase);
			float.TryParse(this.addSpeed, ref fadd);
			this.m_PlayerFSM.SetSpeed(fbase, fadd);
		}
		GUI.Label(new Rect(610f, 100f, 200f, 30f), "注意 : 按鍵盤K可以加速~~");
	}

	// Token: 0x040006EF RID: 1775
	public PlayerFSM m_PlayerFSM;

	// Token: 0x040006F0 RID: 1776
	private string baseSpeed = "5.0";

	// Token: 0x040006F1 RID: 1777
	private string addSpeed = "0.5";

	// Token: 0x040006F2 RID: 1778
	public bool ShowDebug;
}
