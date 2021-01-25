using System;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
[Serializable]
public class UnitOverlay : NGUIItem
{
	// Token: 0x060031A3 RID: 12707 RVA: 0x0001EDCC File Offset: 0x0001CFCC
	public UnitOverlay()
	{
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x0001F304 File Offset: 0x0001D504
	public UnitOverlay(GameObject obj, UnitTB unit)
	{
		Utility.SetActive(obj, true);
		this.rootObj = obj;
		this.Init(unit);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x00181014 File Offset: 0x0017F214
	public void Init(UnitTB un)
	{
		this.rootT = this.rootObj.transform;
		this.unit = un;
		UISlider[] componentsInChildren = this.rootObj.GetComponentsInChildren<UISlider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name == "FriendHP")
			{
				this.FriendBar = componentsInChildren[i];
			}
			else if (componentsInChildren[i].gameObject.name == "EnemyHP")
			{
				this.EnemyBar = componentsInChildren[i];
			}
			else if (componentsInChildren[i].gameObject.name == "OtherHP")
			{
				this.OtherBar = componentsInChildren[i];
			}
			else if (componentsInChildren[i].gameObject.name == "SpecialHP")
			{
				this.SpecialBar = componentsInChildren[i];
			}
		}
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x001810FC File Offset: 0x0017F2FC
	public UnitOverlay CloneItem(string name, UnitTB un)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(this.rootObj);
		gameObject.name = name;
		return new UnitOverlay(gameObject, un)
		{
			rootT = 
			{
				parent = this.rootT.parent,
				localPosition = this.rootT.localPosition,
				localScale = this.rootT.localScale
			}
		};
	}

	// Token: 0x04003D42 RID: 15682
	[HideInInspector]
	public UnitTB unit;

	// Token: 0x04003D43 RID: 15683
	[HideInInspector]
	public UISlider EnemyBar;

	// Token: 0x04003D44 RID: 15684
	[HideInInspector]
	public UISlider FriendBar;

	// Token: 0x04003D45 RID: 15685
	[HideInInspector]
	public UISlider OtherBar;

	// Token: 0x04003D46 RID: 15686
	[HideInInspector]
	public UISlider SpecialBar;
}
