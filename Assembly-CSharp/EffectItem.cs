using System;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
[Serializable]
public class EffectItem : NGUIItem
{
	// Token: 0x060031EE RID: 12782 RVA: 0x0001EDCC File Offset: 0x0001CFCC
	public EffectItem()
	{
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x0001F5F9 File Offset: 0x0001D7F9
	public EffectItem(GameObject obj)
	{
		Utility.SetActive(obj, true);
		this.rootObj = obj;
		this.Init();
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x0001F615 File Offset: 0x0001D815
	public void Init()
	{
		this.rootT = this.rootObj.transform;
		this.spIcon = this.rootObj.GetComponent<UISprite>();
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x00183F78 File Offset: 0x00182178
	public EffectItem CloneItem(string name, Vector3 posOffset)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(this.rootObj);
		gameObject.name = name;
		return new EffectItem(gameObject)
		{
			rootT = 
			{
				parent = this.rootT.parent,
				localPosition = this.rootT.localPosition + posOffset,
				localScale = this.rootT.localScale
			}
		};
	}

	// Token: 0x04003DA0 RID: 15776
	[HideInInspector]
	public UISprite spIcon;
}
