using System;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class qb_Group : MonoBehaviour
{
	// Token: 0x060027B3 RID: 10163 RVA: 0x0001A2F3 File Offset: 0x000184F3
	public void AddObject(GameObject newObject)
	{
		newObject.transform.parent = base.transform;
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x0001A306 File Offset: 0x00018506
	public void Hide()
	{
		this.visible = false;
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x0001A30F File Offset: 0x0001850F
	public void Show()
	{
		this.visible = true;
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x0001A318 File Offset: 0x00018518
	public void Freeze()
	{
		this.frozen = true;
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x0001A321 File Offset: 0x00018521
	public void UnFreeze()
	{
		this.frozen = false;
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x0001A32A File Offset: 0x0001852A
	public void CleanUp()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	// Token: 0x040031B4 RID: 12724
	public string groupName;

	// Token: 0x040031B5 RID: 12725
	private bool visible;

	// Token: 0x040031B6 RID: 12726
	private bool frozen;
}
