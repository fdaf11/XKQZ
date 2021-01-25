using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class UINGUIUnitSelection : MonoBehaviour
{
	// Token: 0x06003065 RID: 12389 RVA: 0x0001E8D3 File Offset: 0x0001CAD3
	private void Awake()
	{
		this.thisObj = base.gameObject;
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x0017798C File Offset: 0x00175B8C
	public void Init()
	{
		this.InitAvailableButtons();
		this.InitSelectedButtons();
		this.buttonSwitch.Init();
		Utility.SetActive(this.buttonSwitch.rootObj, false);
		this.selectHighlight.localPosition = new Vector3(0f, 50000f, 0f);
		this.OnAvailableUnitButton(this.availableButtonList[this.selectedID].rootObj);
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x001779FC File Offset: 0x00175BFC
	private void InitAvailableButtons()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.availableUnitList.Count; i++)
		{
			if (i == 0)
			{
				this.availableButtonList[i].Init();
			}
			else
			{
				Vector3 posOffset;
				posOffset..ctor((float)(num * 70), (float)(-(float)num2 * 70), 0f);
				this.availableButtonList.Add(this.availableButtonList[0].CloneItem("ButtonUnit" + (i + 1), posOffset));
			}
			if (i < this.availableUnitList.Count)
			{
				this.availableButtonList[i].spriteIcon.spriteName = this.availableUnitList[i].iconName;
				this.availableButtonList[i].label.text = this.availableUnitList[i].pointCost.ToString();
			}
			else
			{
				Utility.SetActive(this.availableButtonList[i].rootObj, false);
			}
			UIEventListener uieventListener = UIEventListener.Get(this.availableButtonList[i].rootObj);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnAvailableUnitButton));
			num++;
			if (num == 6)
			{
				num = 0;
				num2++;
			}
		}
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x00177B54 File Offset: 0x00175D54
	private void InitSelectedButtons()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.maxSelectedUnitCount; i++)
		{
			if (i == 0)
			{
				this.selectedButtonList[i].Init();
			}
			else
			{
				Vector3 posOffset;
				posOffset..ctor((float)(num * 70), (float)(-(float)num2 * 70), 0f);
				this.selectedButtonList.Add(this.selectedButtonList[0].CloneItem("ButtonUnit" + (i + 1), posOffset));
			}
			if (i < this.selectedUnitList.Count)
			{
				this.selectedButtonList[i].spriteIcon.spriteName = this.selectedUnitList[i].iconName;
			}
			else
			{
				Utility.SetActive(this.selectedButtonList[i].rootObj, false);
			}
			UIEventListener uieventListener = UIEventListener.Get(this.selectedButtonList[i].rootObj);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnSelectedUnitButton));
			num++;
			if (num == 5)
			{
				num = 0;
				num2++;
			}
		}
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x00177C7C File Offset: 0x00175E7C
	public void OnAvailableUnitButton(GameObject butObj)
	{
		this.buttonSwitch.label.text = "Select";
		Utility.SetActive(this.buttonSwitch.rootObj, true);
		this.currentTab = UINGUIUnitSelection._CurrentTab.Available;
		for (int i = 0; i < this.availableButtonList.Count; i++)
		{
			if (this.availableButtonList[i].rootObj == butObj)
			{
				this.selectedID = i;
			}
		}
		this.uiUnitInfo.Show(this.availableUnitList[this.selectedID]);
		this.selectHighlight.position = this.availableButtonList[this.selectedID].rootT.position;
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x00177D38 File Offset: 0x00175F38
	public void OnSelectedUnitButton(GameObject butObj)
	{
		this.buttonSwitch.label.text = "Remove";
		Utility.SetActive(this.buttonSwitch.rootObj, true);
		this.currentTab = UINGUIUnitSelection._CurrentTab.Selected;
		for (int i = 0; i < this.selectedButtonList.Count; i++)
		{
			if (this.selectedButtonList[i].rootObj == butObj)
			{
				this.selectedID = i;
			}
		}
		this.selectHighlight.position = this.selectedButtonList[this.selectedID].rootT.position;
		this.uiUnitInfo.Show(this.selectedUnitList[this.selectedID]);
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x0001E8E1 File Offset: 0x0001CAE1
	public void OnSwitchButton()
	{
		if (this.currentTab == UINGUIUnitSelection._CurrentTab.Available)
		{
			this.OnAddToSelectedUnit();
		}
		else if (this.currentTab == UINGUIUnitSelection._CurrentTab.Selected)
		{
			this.OnRemoveFromSelectedUnit();
		}
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x00177DF4 File Offset: 0x00175FF4
	private void OnAddToSelectedUnit()
	{
		if (this.selectedUnitList.Count == this.maxSelectedUnitCount)
		{
			UINGUISetup.DisplayMessage("Max unit quota reached!");
			return;
		}
		UnitTB unitTB = this.availableUnitList[this.selectedID];
		int pointCost = unitTB.pointCost;
		if (UINGUISetup.playerPoint < pointCost)
		{
			UINGUISetup.DisplayMessage("Insufficient Points!");
			return;
		}
		UINGUISetup.UpdatePoints(-pointCost);
		NGUIButton nguibutton = this.selectedButtonList[this.selectedUnitList.Count];
		nguibutton.label.text = unitTB.pointCost.ToString();
		nguibutton.spriteIcon.spriteName = unitTB.iconName;
		Utility.SetActive(nguibutton.rootObj, true);
		this.selectedUnitList.Add(unitTB);
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x00177EB0 File Offset: 0x001760B0
	private void OnRemoveFromSelectedUnit()
	{
		UnitTB unitTB = this.selectedUnitList[this.selectedID];
		UINGUISetup.UpdatePoints(unitTB.pointCost);
		this.selectedUnitList.RemoveAt(this.selectedID);
		for (int i = this.selectedID; i < this.selectedButtonList.Count - 1; i++)
		{
			if (i < this.selectedUnitList.Count)
			{
				this.selectedButtonList[i].spriteIcon.spriteName = this.selectedUnitList[i].iconName;
				this.selectedButtonList[i].label.text = this.selectedUnitList[i].pointCost.ToString();
			}
			else
			{
				Utility.SetActive(this.selectedButtonList[i].rootObj, false);
			}
		}
		Utility.SetActive(this.selectedButtonList[this.selectedButtonList.Count - 1].rootObj, false);
		if (this.selectedUnitList.Count == 0)
		{
			this.selectedID = 0;
			this.OnAvailableUnitButton(this.availableButtonList[this.selectedID].rootObj);
		}
		else
		{
			if (this.selectedID == this.selectedUnitList.Count)
			{
				this.selectedID--;
			}
			this.OnSelectedUnitButton(this.selectedButtonList[this.selectedID].rootObj);
		}
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x0017802C File Offset: 0x0017622C
	public void Show()
	{
		Utility.SetActive(this.thisObj, true);
		this.uiUnitInfo.Show();
		for (int i = 0; i < this.selectedButtonList.Count; i++)
		{
			if (i >= this.selectedUnitList.Count)
			{
				Utility.SetActive(this.selectedButtonList[i].rootObj, false);
			}
		}
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x0001E90C File Offset: 0x0001CB0C
	public void Hide()
	{
		Utility.SetActive(this.thisObj, false);
		this.uiUnitInfo.Hide();
	}

	// Token: 0x04003C1D RID: 15389
	public List<UnitTB> availableUnitList = new List<UnitTB>();

	// Token: 0x04003C1E RID: 15390
	public List<NGUIButton> availableButtonList = new List<NGUIButton>();

	// Token: 0x04003C1F RID: 15391
	public int maxSelectedUnitCount = 8;

	// Token: 0x04003C20 RID: 15392
	[HideInInspector]
	public List<UnitTB> selectedUnitList = new List<UnitTB>();

	// Token: 0x04003C21 RID: 15393
	public List<NGUIButton> selectedButtonList = new List<NGUIButton>();

	// Token: 0x04003C22 RID: 15394
	public NGUIButton buttonSwitch;

	// Token: 0x04003C23 RID: 15395
	public Transform selectHighlight;

	// Token: 0x04003C24 RID: 15396
	public UINGUIUnitSelectInfo uiUnitInfo;

	// Token: 0x04003C25 RID: 15397
	[HideInInspector]
	public int selectedID;

	// Token: 0x04003C26 RID: 15398
	[HideInInspector]
	public UINGUIUnitSelection._CurrentTab currentTab;

	// Token: 0x04003C27 RID: 15399
	private GameObject thisObj;

	// Token: 0x020007BB RID: 1979
	public enum _CurrentTab
	{
		// Token: 0x04003C29 RID: 15401
		None,
		// Token: 0x04003C2A RID: 15402
		Available,
		// Token: 0x04003C2B RID: 15403
		Selected
	}
}
