using System;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class PanWM : MonoBehaviour
{
	// Token: 0x0600223B RID: 8763 RVA: 0x00016D98 File Offset: 0x00014F98
	private void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x0010C350 File Offset: 0x0010A550
	private void Update()
	{
		this.t += PanWM.shake_speed * Time.deltaTime;
		this.shake = new Vector2(Mathf.Sin(this.t * 5f) * PanWM.shake_value, Mathf.Sin(this.t * 3f) * PanWM.shake_value);
		float deltaTime = Time.deltaTime;
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (this.range < 0.1f)
		{
			this.range = 0.1f;
		}
		float num3 = Mathf.Clamp((mousePosition.x - num) / num / this.range, -1f, 1f);
		float num4 = Mathf.Clamp((mousePosition.y - num2) / num2 / this.range, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(num3, num4), deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y + this.shake.y, this.mRot.x * this.degrees.x + this.shake.x, 0f);
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f);
	}

	// Token: 0x0400285D RID: 10333
	public Vector2 degrees = new Vector2(5f, 3f);

	// Token: 0x0400285E RID: 10334
	private Vector2 shake = new Vector2(5f, 3f);

	// Token: 0x0400285F RID: 10335
	public static float shake_value;

	// Token: 0x04002860 RID: 10336
	public static float shake_speed = 10f;

	// Token: 0x04002861 RID: 10337
	public float range = 1f;

	// Token: 0x04002862 RID: 10338
	private float t;

	// Token: 0x04002863 RID: 10339
	private Transform mTrans;

	// Token: 0x04002864 RID: 10340
	private Quaternion mStart;

	// Token: 0x04002865 RID: 10341
	private Vector2 mRot = Vector2.zero;
}
