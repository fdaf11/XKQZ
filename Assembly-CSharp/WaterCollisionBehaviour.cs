using System;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
public class WaterCollisionBehaviour : MonoBehaviour
{
	// Token: 0x060024FD RID: 9469 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x00120DD0 File Offset: 0x0011EFD0
	private void OnTriggerEnter(Collider myTrigger)
	{
		if (this.WaterWave != null)
		{
			GameObject gameObject = Object.Instantiate(this.WaterWave) as GameObject;
			Transform transform = gameObject.transform;
			transform.parent = base.transform;
			float num = base.transform.localScale.x * this.scaleWave;
			transform.localScale = new Vector3(num, num, num);
			transform.localPosition = new Vector3(0f, 0.001f, 0f);
			transform.LookAt(myTrigger.transform.position);
		}
	}

	// Token: 0x04002D29 RID: 11561
	public GameObject WaterWave;

	// Token: 0x04002D2A RID: 11562
	public float scaleWave = 0.97f;
}
