using System;
using SWS;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
public class PathInputDemo : MonoBehaviour
{
	// Token: 0x06002933 RID: 10547 RVA: 0x0001B1DB File Offset: 0x000193DB
	private void Start()
	{
		this.animator = base.GetComponent<Animator>();
		this.move = base.GetComponent<minimalMove>();
		this.move.StartMove();
		this.move.Pause();
		this.progress = 0f;
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x001469F8 File Offset: 0x00144BF8
	private void Update()
	{
		float num = this.speedMultiplier / 100f;
		if (Input.GetKey("right"))
		{
			this.progress += Time.deltaTime * num;
			this.progress = Mathf.Clamp01(this.progress);
			base.transform.position = this.move.tween.GetPointOnPath(this.progress);
			if (this.move.orientToPath)
			{
				float num2 = (this.move.lookAhead <= 0.01f) ? 0.01f : this.move.lookAhead;
				base.transform.LookAt(this.move.tween.GetPointOnPath(Mathf.Clamp01(this.progress + num2)));
			}
		}
		if (Input.GetKey("left"))
		{
			this.progress -= Time.deltaTime * num;
			this.progress = Mathf.Clamp01(this.progress);
			base.transform.position = this.move.tween.GetPointOnPath(this.progress);
			if (this.move.orientToPath)
			{
				float num3 = (this.move.lookAhead <= 0.01f) ? 0.01f : this.move.lookAhead;
				num3 = -num3;
				base.transform.LookAt(this.move.tween.GetPointOnPath(Mathf.Clamp01(this.progress + num3)));
			}
		}
		if ((Input.GetKey("right") || Input.GetKey("left")) && this.progress != 0f && this.progress != 1f)
		{
			this.animator.SetFloat("Speed", this.move.speed);
		}
		else
		{
			this.animator.SetFloat("Speed", 0f);
		}
	}

	// Token: 0x04003422 RID: 13346
	public float speedMultiplier = 10f;

	// Token: 0x04003423 RID: 13347
	public float progress;

	// Token: 0x04003424 RID: 13348
	private minimalMove move;

	// Token: 0x04003425 RID: 13349
	private Animator animator;
}
