using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class FPSObjectShooter : MonoBehaviour
{
	// Token: 0x06002723 RID: 10019 RVA: 0x00019DC7 File Offset: 0x00017FC7
	private void Start()
	{
		this.m_v3MousePosition = Input.mousePosition;
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x0012F13C File Offset: 0x0012D33C
	private void Update()
	{
		if (this.Element != null && Input.GetKeyDown(32))
		{
			GameObject gameObject = Object.Instantiate(this.Element) as GameObject;
			gameObject.transform.position = base.transform.position;
			gameObject.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
			gameObject.rigidbody.mass = this.Mass;
			gameObject.rigidbody.solverIterationCount = 255;
			gameObject.rigidbody.AddForce(base.transform.forward * this.InitialSpeed, 2);
			DieTimer dieTimer = gameObject.AddComponent<DieTimer>();
			dieTimer.SecondsToDie = this.Life;
		}
		if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
		{
			base.transform.Rotate(-(Input.mousePosition.y - this.m_v3MousePosition.y) * this.MouseSpeed, 0f, 0f);
			base.transform.RotateAround(base.transform.position, Vector3.up, (Input.mousePosition.x - this.m_v3MousePosition.x) * this.MouseSpeed);
		}
		this.m_v3MousePosition = Input.mousePosition;
	}

	// Token: 0x04003059 RID: 12377
	public GameObject Element;

	// Token: 0x0400305A RID: 12378
	public float InitialSpeed = 1f;

	// Token: 0x0400305B RID: 12379
	public float MouseSpeed = 0.3f;

	// Token: 0x0400305C RID: 12380
	public float Scale = 1f;

	// Token: 0x0400305D RID: 12381
	public float Mass = 1f;

	// Token: 0x0400305E RID: 12382
	public float Life = 10f;

	// Token: 0x0400305F RID: 12383
	private Vector3 m_v3MousePosition;
}
