using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class DieTimer : MonoBehaviour
{
	// Token: 0x06002720 RID: 10016 RVA: 0x00019D4B File Offset: 0x00017F4B
	private void Start()
	{
		this.m_fTimer = 0f;
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x00019D58 File Offset: 0x00017F58
	private void Update()
	{
		this.m_fTimer += Time.deltaTime;
		if (this.m_fTimer > this.SecondsToDie)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04003057 RID: 12375
	public float SecondsToDie = 10f;

	// Token: 0x04003058 RID: 12376
	private float m_fTimer;
}
