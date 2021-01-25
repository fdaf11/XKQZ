using System;
using UnityEngine;
using WellFired;

// Token: 0x0200084E RID: 2126
public class BasicCharacterControls : MonoBehaviour
{
	// Token: 0x0600338B RID: 13195 RVA: 0x0018DA3C File Offset: 0x0018BC3C
	private void Update()
	{
		if (this.cutscene && this.cutscene.IsPlaying)
		{
			return;
		}
		float num = this.strength * Time.deltaTime;
		if (Input.GetKey(119))
		{
			base.rigidbody.AddRelativeForce(-num, 0f, 0f);
		}
		if (Input.GetKey(115))
		{
			base.rigidbody.AddRelativeForce(num, 0f, 0f);
		}
		if (Input.GetKey(97))
		{
			base.rigidbody.AddRelativeForce(0f, 0f, -num);
		}
		if (Input.GetKey(100))
		{
			base.rigidbody.AddRelativeForce(0f, 0f, num);
		}
	}

	// Token: 0x04003FBD RID: 16317
	public USSequencer cutscene;

	// Token: 0x04003FBE RID: 16318
	public float strength = 10f;
}
