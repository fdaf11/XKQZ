using System;
using System.Collections;
using SWS;
using UnityEngine;

// Token: 0x020006A3 RID: 1699
public class RapidInputDemo : MonoBehaviour
{
	// Token: 0x06002936 RID: 10550 RVA: 0x00146BF8 File Offset: 0x00144DF8
	private void Start()
	{
		this.move = base.GetComponent<minimalMove>();
		if (!this.move)
		{
			Debug.LogWarning(base.gameObject.name + " missing movement script!");
			return;
		}
		this.move.speed = 0.01f;
		this.move.StartMove();
		this.move.Pause();
		this.move.speed = 0f;
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x00146C74 File Offset: 0x00144E74
	private void Update()
	{
		if (this.move.tween == null || this.move.tween.isComplete)
		{
			return;
		}
		if (Input.GetKeyDown(273))
		{
			if (this.move.tween.isPaused)
			{
				this.move.Resume();
			}
			float num = this.currentSpeed + this.addSpeed;
			if (num >= this.topSpeed)
			{
				num = this.topSpeed;
			}
			this.move.ChangeSpeed(num);
			base.StopAllCoroutines();
			base.StartCoroutine("SlowDown");
		}
		this.speedDisplay.text = "YOUR SPEED: " + Mathf.Round(this.move.speed * 100f) / 100f;
		this.timeCounter += Time.deltaTime;
		this.timeDisplay.text = "YOUR TIME: " + Mathf.Round(this.timeCounter * 100f) / 100f;
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x00146D90 File Offset: 0x00144F90
	private IEnumerator SlowDown()
	{
		yield return new WaitForSeconds(this.delay);
		float t = 0f;
		float rate = 1f / this.slowTime;
		float speed = this.move.speed;
		while (t < 1f)
		{
			t += Time.deltaTime * rate;
			this.currentSpeed = Mathf.Lerp(speed, 0f, t);
			this.move.ChangeSpeed(this.currentSpeed);
			float pitchFactor = this.maxPitch - this.minPitch;
			float pitch = this.minPitch + this.move.speed / this.topSpeed * pitchFactor;
			if (base.audio)
			{
				base.audio.pitch = Mathf.SmoothStep(base.audio.pitch, pitch, 0.2f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003426 RID: 13350
	public TextMesh speedDisplay;

	// Token: 0x04003427 RID: 13351
	public TextMesh timeDisplay;

	// Token: 0x04003428 RID: 13352
	public float topSpeed = 15f;

	// Token: 0x04003429 RID: 13353
	public float addSpeed = 2f;

	// Token: 0x0400342A RID: 13354
	public float delay = 0.05f;

	// Token: 0x0400342B RID: 13355
	public float slowTime = 0.5f;

	// Token: 0x0400342C RID: 13356
	public float minPitch;

	// Token: 0x0400342D RID: 13357
	public float maxPitch = 2f;

	// Token: 0x0400342E RID: 13358
	private minimalMove move;

	// Token: 0x0400342F RID: 13359
	private float currentSpeed;

	// Token: 0x04003430 RID: 13360
	private float timeCounter;
}
