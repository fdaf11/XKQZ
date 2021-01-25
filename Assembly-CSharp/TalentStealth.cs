using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class TalentStealth : MonoBehaviour
{
	// Token: 0x060005CA RID: 1482 RVA: 0x0004272C File Offset: 0x0004092C
	private void Start()
	{
		this.origMaterialList = new List<Material>();
		this.smrList = new List<Renderer>();
		Renderer[] componentsInChildren = base.transform.parent.gameObject.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.smrList.Add(componentsInChildren[i]);
			this.origMaterialList.Add(this.smrList[i].material);
			this.smrList[i].material = this.steathMateral;
		}
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x000427BC File Offset: 0x000409BC
	private void OnDestroy()
	{
		for (int i = 0; i < this.smrList.Count; i++)
		{
			this.smrList[i].material = this.origMaterialList[i];
		}
	}

	// Token: 0x04000647 RID: 1607
	public Material steathMateral;

	// Token: 0x04000648 RID: 1608
	private List<Material> origMaterialList;

	// Token: 0x04000649 RID: 1609
	private List<Renderer> smrList;
}
