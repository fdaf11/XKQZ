using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class SwarmCenter : MonoBehaviour
{
	// Token: 0x0600225B RID: 8795 RVA: 0x00016EA6 File Offset: 0x000150A6
	private void Start()
	{
		base.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x00016ED1 File Offset: 0x000150D1
	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, this.speed * Time.deltaTime, 0f));
	}

	// Token: 0x04002894 RID: 10388
	public float speed;
}
