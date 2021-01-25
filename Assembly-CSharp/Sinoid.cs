using System;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class Sinoid : MonoBehaviour
{
	// Token: 0x06002849 RID: 10313 RVA: 0x0001A863 File Offset: 0x00018A63
	public void Start()
	{
		this.startPos = base.transform.position;
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x0013EFF4 File Offset: 0x0013D1F4
	public void Update()
	{
		this.accuTime += Time.deltaTime;
		base.transform.position = this.startPos + Vector3.up * this.Amplitude * Mathf.Sin(this.accuTime * 2f * 3.1415927f * this.SineFreq);
		base.transform.Rotate((Vector3.up + Vector3.forward) * this.AngularVelocity * Time.deltaTime);
	}

	// Token: 0x0400328A RID: 12938
	public float AngularVelocity = 2f;

	// Token: 0x0400328B RID: 12939
	public float SineFreq = 0.2f;

	// Token: 0x0400328C RID: 12940
	public float Amplitude = 0.25f;

	// Token: 0x0400328D RID: 12941
	private float accuTime;

	// Token: 0x0400328E RID: 12942
	private Vector3 startPos;
}
