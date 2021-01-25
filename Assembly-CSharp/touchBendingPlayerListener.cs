using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000647 RID: 1607
[AddComponentMenu("AFS/Touch Bending/Player Listener")]
public class touchBendingPlayerListener : MonoBehaviour
{
	// Token: 0x060027A8 RID: 10152 RVA: 0x0001A2A6 File Offset: 0x000184A6
	private void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x0001A2B4 File Offset: 0x000184B4
	private void Start()
	{
		this.Player_Position = base.transform.position;
		this.Player_OldPosition = this.Player_Position;
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x0001A2D3 File Offset: 0x000184D3
	private void Update()
	{
		base.StartCoroutine(this.AfsPlayerDataUpdate());
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x0013B0A8 File Offset: 0x001392A8
	private IEnumerator AfsPlayerDataUpdate()
	{
		yield return new WaitForEndOfFrame();
		this.Player_Position = this.myTransform.position;
		this.Player_NewSpeed = (this.Player_Position - this.Player_OldPosition).magnitude / Time.deltaTime / this.maxSpeed;
		float dampDecelerate = 1f - Mathf.Exp(-20f * Time.deltaTime);
		float dampAccelerate = 0.25f * dampDecelerate;
		dampDecelerate *= 0.125f;
		if (this.Player_NewSpeed < this.Player_Speed)
		{
			this.Player_Speed = Mathf.Lerp(this.Player_Speed, this.Player_NewSpeed, dampDecelerate * this.Player_DampSpeed);
		}
		else
		{
			this.Player_Speed = Mathf.Lerp(this.Player_Speed, this.Player_NewSpeed, dampAccelerate * this.Player_DampSpeed);
		}
		if (this.Player_Position != this.Player_OldPosition)
		{
			this.Player_Direction = Vector3.Normalize(this.Player_Position - this.Player_OldPosition);
		}
		this.Player_OldPosition = this.Player_Position;
		yield break;
	}

	// Token: 0x040031A7 RID: 12711
	public float maxSpeed = 8f;

	// Token: 0x040031A8 RID: 12712
	public float Player_DampSpeed = 0.75f;

	// Token: 0x040031A9 RID: 12713
	private Transform myTransform;

	// Token: 0x040031AA RID: 12714
	private Vector3 Player_Position;

	// Token: 0x040031AB RID: 12715
	private Vector3 Player_OldPosition;

	// Token: 0x040031AC RID: 12716
	public float Player_Speed;

	// Token: 0x040031AD RID: 12717
	private float Player_NewSpeed;

	// Token: 0x040031AE RID: 12718
	public Vector3 Player_Direction;
}
