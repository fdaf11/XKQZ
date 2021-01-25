using System;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class RotateAround : MonoBehaviour
{
	// Token: 0x060025E8 RID: 9704 RVA: 0x0012516C File Offset: 0x0012336C
	private void Start()
	{
		if (this.UseCollision)
		{
			this.EffectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.EffectSettings_CollisionEnter);
		}
		if (this.TimeDelay > 0f)
		{
			base.Invoke("ChangeUpdate", this.TimeDelay);
		}
		else
		{
			this.canUpdate = true;
		}
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x00019411 File Offset: 0x00017611
	private void OnEnable()
	{
		this.canUpdate = true;
		this.allTime = 0f;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x00019425 File Offset: 0x00017625
	private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.canUpdate = false;
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x0001942E File Offset: 0x0001762E
	private void ChangeUpdate()
	{
		this.canUpdate = true;
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x001251C8 File Offset: 0x001233C8
	private void Update()
	{
		if (!this.canUpdate)
		{
			return;
		}
		this.allTime += Time.deltaTime;
		if (this.allTime >= this.LifeTime && this.LifeTime > 0.0001f)
		{
			return;
		}
		if (this.SpeedFadeInTime > 0.001f)
		{
			if (this.currentSpeedFadeIn < this.Speed)
			{
				this.currentSpeedFadeIn += Time.deltaTime / this.SpeedFadeInTime * this.Speed;
			}
			else
			{
				this.currentSpeedFadeIn = this.Speed;
			}
		}
		else
		{
			this.currentSpeedFadeIn = this.Speed;
		}
		base.transform.Rotate(Vector3.forward * Time.deltaTime * this.currentSpeedFadeIn);
	}

	// Token: 0x04002E79 RID: 11897
	public float Speed = 1f;

	// Token: 0x04002E7A RID: 11898
	public float LifeTime = 1f;

	// Token: 0x04002E7B RID: 11899
	public float TimeDelay;

	// Token: 0x04002E7C RID: 11900
	public float SpeedFadeInTime;

	// Token: 0x04002E7D RID: 11901
	public bool UseCollision;

	// Token: 0x04002E7E RID: 11902
	public EffectSettings EffectSettings;

	// Token: 0x04002E7F RID: 11903
	private bool canUpdate;

	// Token: 0x04002E80 RID: 11904
	private float currentSpeedFadeIn;

	// Token: 0x04002E81 RID: 11905
	private float allTime;
}
