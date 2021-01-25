using System;
using UnityEngine;

// Token: 0x020005CC RID: 1484
public class LazyLoad : MonoBehaviour
{
	// Token: 0x060024E4 RID: 9444 RVA: 0x0001871F File Offset: 0x0001691F
	private void Awake()
	{
		this.GO.SetActive(false);
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x0001872D File Offset: 0x0001692D
	private void LazyEnable()
	{
		this.GO.SetActive(true);
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x0001873B File Offset: 0x0001693B
	private void OnEnable()
	{
		base.Invoke("LazyEnable", this.TimeDelay);
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x0001874E File Offset: 0x0001694E
	private void OnDisable()
	{
		base.CancelInvoke("LazyEnable");
		this.GO.SetActive(false);
	}

	// Token: 0x04002CED RID: 11501
	public GameObject GO;

	// Token: 0x04002CEE RID: 11502
	public float TimeDelay = 0.3f;
}
