using System;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class LogicRopeWithCoil : MonoBehaviour
{
	// Token: 0x06002730 RID: 10032 RVA: 0x00019E73 File Offset: 0x00018073
	private void Start()
	{
		this.m_fRopeExtension = ((!(this.Rope != null)) ? 0f : this.Rope.m_fCurrentExtension);
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x00019EA1 File Offset: 0x000180A1
	private void OnGUI()
	{
		LogicGlobal.GlobalGUI();
		GUILayout.Label("Rope test (Procedural rope with additional coil)", new GUILayoutOption[0]);
		GUILayout.Label("Use the keypad + and - to extend the rope", new GUILayoutOption[0]);
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x0012F390 File Offset: 0x0012D590
	private void Update()
	{
		if (Input.GetKey(270))
		{
			this.m_fRopeExtension += Time.deltaTime * this.RopeExtensionSpeed;
		}
		if (Input.GetKey(269))
		{
			this.m_fRopeExtension -= Time.deltaTime * this.RopeExtensionSpeed;
		}
		if (this.Rope != null)
		{
			this.m_fRopeExtension = Mathf.Clamp(this.m_fRopeExtension, 0f, this.Rope.ExtensibleLength);
			this.Rope.ExtendRope(UltimateRope.ERopeExtensionMode.LinearExtensionIncrement, this.m_fRopeExtension - this.Rope.m_fCurrentExtension);
		}
	}

	// Token: 0x04003064 RID: 12388
	public UltimateRope Rope;

	// Token: 0x04003065 RID: 12389
	public float RopeExtensionSpeed;

	// Token: 0x04003066 RID: 12390
	private float m_fRopeExtension;
}
