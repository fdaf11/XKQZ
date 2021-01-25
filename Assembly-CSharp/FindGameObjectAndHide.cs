using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class FindGameObjectAndHide : MonoBehaviour
{
	// Token: 0x06000709 RID: 1801 RVA: 0x000482D8 File Offset: 0x000464D8
	private void Start()
	{
		GameObject gameObject = GameObject.Find(this.strParent);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (this.listGameObject.Contains(gameObject.transform.GetChild(i).name))
			{
				gameObject.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04000769 RID: 1897
	public string strParent;

	// Token: 0x0400076A RID: 1898
	public List<string> listGameObject = new List<string>();
}
