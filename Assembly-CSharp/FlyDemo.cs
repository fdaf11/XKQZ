using System;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public class FlyDemo : MonoBehaviour
{
	// Token: 0x060025AC RID: 9644 RVA: 0x000191EA File Offset: 0x000173EA
	private void Start()
	{
		this.t = base.transform;
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x00124260 File Offset: 0x00122460
	private void Update()
	{
		this.time += Time.deltaTime;
		float num = Mathf.Cos(this.time / this.Speed);
		this.t.localPosition = new Vector3(0f, 0f, num * this.Height);
	}

	// Token: 0x04002E32 RID: 11826
	public float Speed = 1f;

	// Token: 0x04002E33 RID: 11827
	public float Height = 1f;

	// Token: 0x04002E34 RID: 11828
	private Transform t;

	// Token: 0x04002E35 RID: 11829
	private float time;
}
