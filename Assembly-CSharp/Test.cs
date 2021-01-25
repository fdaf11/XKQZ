using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class Test : MonoBehaviour
{
	// Token: 0x06000F2A RID: 3882 RVA: 0x0000A3AE File Offset: 0x000085AE
	private void Start()
	{
		this.nullList.Add("Line 1");
		this.nullList.Add("Line 2");
		this.nullList.Add("Line 3");
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0007E994 File Offset: 0x0007CB94
	private void Update()
	{
		this.ftime += Time.deltaTime;
		if (this.ftime > 0.5f)
		{
			List<string> list = new List<string>();
			list.AddRange(this.nullList);
			Debug.Log(list.Count);
			this.nullList[0] = "Line 11";
			this.nullList[1] = "Line 12";
			this.nullList[2] = "Line 13";
			Debug.Log(list.Count);
			this.ftime = 0f;
		}
	}

	// Token: 0x0400120A RID: 4618
	private List<string> nullList = new List<string>();

	// Token: 0x0400120B RID: 4619
	private float ftime;
}
