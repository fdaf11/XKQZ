using System;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
[Serializable]
public class NGUIButton : NGUIItem
{
	// Token: 0x060030D8 RID: 12504 RVA: 0x0001EDCC File Offset: 0x0001CFCC
	public NGUIButton()
	{
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
	public NGUIButton(GameObject obj)
	{
		Utility.SetActive(obj, true);
		this.rootObj = obj;
		this.Init();
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x001791BC File Offset: 0x001773BC
	public void Init()
	{
		this.rootT = this.rootObj.transform;
		this.uiButton = this.rootObj.GetComponent<UIButton>();
		UILabel[] componentsInChildren = this.rootObj.GetComponentsInChildren<UILabel>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name == "CD")
			{
				this.label = componentsInChildren[i];
			}
			else
			{
				this.labelName = componentsInChildren[i];
			}
		}
		UISprite[] componentsInChildren2 = this.rootObj.GetComponentsInChildren<UISprite>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j].gameObject.name == "Icon")
			{
				this.spriteIcon = componentsInChildren2[j];
			}
			else
			{
				this.spriteBG = componentsInChildren2[j];
			}
		}
		this.startingPos = this.rootT.localPosition;
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x001792A4 File Offset: 0x001774A4
	public NGUIButton CloneItem(string name, Vector3 posOffset)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(this.rootObj);
		gameObject.name = name;
		return new NGUIButton(gameObject)
		{
			rootT = 
			{
				parent = this.rootT.parent,
				localPosition = this.rootT.localPosition + posOffset,
				localScale = this.rootT.localScale
			}
		};
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x0001EDF0 File Offset: 0x0001CFF0
	public void ResetPosition()
	{
		this.rootT.localPosition = this.startingPos;
	}

	// Token: 0x04003C70 RID: 15472
	[HideInInspector]
	public UIButton uiButton;

	// Token: 0x04003C71 RID: 15473
	[HideInInspector]
	public UILabel label;

	// Token: 0x04003C72 RID: 15474
	[HideInInspector]
	public UILabel labelName;

	// Token: 0x04003C73 RID: 15475
	[HideInInspector]
	public UISprite spriteBG;

	// Token: 0x04003C74 RID: 15476
	[HideInInspector]
	public UISprite spriteIcon;

	// Token: 0x04003C75 RID: 15477
	private Vector3 startingPos;
}
