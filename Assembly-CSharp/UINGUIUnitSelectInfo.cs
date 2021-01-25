using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007B8 RID: 1976
public class UINGUIUnitSelectInfo : MonoBehaviour
{
	// Token: 0x0600305B RID: 12379 RVA: 0x0001E84F File Offset: 0x0001CA4F
	public void Hide()
	{
		this.isOn = false;
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x0600305C RID: 12380 RVA: 0x0001E864 File Offset: 0x0001CA64
	public void Show()
	{
		this.isOn = true;
		Utility.SetActive(this.thisObj, true);
		this.detail = UINGUIUnitSelectInfo._Detail.Less;
		Utility.SetActive(this.detailMoreObject, false);
		Utility.SetActive(this.detailLessObject, true);
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x001773D4 File Offset: 0x001755D4
	public void Show(UnitTB unit)
	{
		if (!this.isOn)
		{
			this.Show();
		}
		this.currentUnit = unit;
		this.lbDesp.text = unit.desp;
		this.spIcon.spriteName = unit.iconName;
		this.lbName.text = unit.unitName;
		this.lbHPAP.text = unit.GetFullHP() + "\n" + unit.GetFullSP();
		this.lbDamage.text = "傷害:";
		this.lbHit.text = "反擊機率:";
		this.lbCrit.text = "暴擊機率:";
		this.lbDamageVal.text = "0-0";
		this.lbHitVal.text = (unit.GetCounterRate(null, 0) * 100f).ToString("f0") + "%";
		this.lbCritVal.text = (unit.GetAbilityCriticalRate(null, 0, unit.occupiedTile) * 100f).ToString("f0") + "%";
		this.lbArmor.text = unit.GetDamageReduc(100f).ToString("f0");
		this.lbDodge.text = (unit.GetDodgeRate(null, 0) * 100f).ToString("f0") + "%";
		this.lbCritImmune.text = (unit.GetDefanceCriticalRate(null, 0) * 100f).ToString("f0") + "%";
		this.lbMobility.text = unit.GetMoveRange().ToString("f0");
		this.lbSpeed.text = unit.GetTurnPriority().ToString("f0");
		List<int> abilityIDList = unit.abilityIDList;
		for (int i = 0; i < this.abilityItems.Count; i++)
		{
			if (i < abilityIDList.Count)
			{
				foreach (UnitAbility unitAbility in this.unitAbilityList)
				{
					if (unitAbility.ID == abilityIDList[i])
					{
						this.abilityItems[i].spIcon.spriteName = unitAbility.iconName;
					}
				}
				if (this.detail == UINGUIUnitSelectInfo._Detail.More)
				{
					Utility.SetActive(this.abilityItems[i].rootObj, true);
				}
			}
			else
			{
				Utility.SetActive(this.abilityItems[i].rootObj, false);
			}
		}
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x001776A8 File Offset: 0x001758A8
	private void Awake()
	{
		this.thisObj = base.gameObject;
		this.buttonSwitch.Init();
		for (int i = 0; i < this.abilityItems.Count; i++)
		{
			this.abilityItems[i].Init();
			UIEventListener uieventListener = UIEventListener.Get(this.abilityItems[i].rootObj);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityItem));
		}
		this.LoadUnitAbility();
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x00177738 File Offset: 0x00175938
	private void Start()
	{
		for (int i = 0; i < this.abilityItems.Count; i++)
		{
			this.abilityItems[i].Init();
		}
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x00177774 File Offset: 0x00175974
	public void LoadUnitAbility()
	{
		GameObject gameObject = Resources.Load("PrefabList/UnitAbilityListPrefab", typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("load unit ability fail, make sure the resource file exists");
			return;
		}
		UnitAbilityListPrefab component = gameObject.GetComponent<UnitAbilityListPrefab>();
		this.unitAbilityList = component.unitAbilityList;
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x001777C8 File Offset: 0x001759C8
	public void OnSwitchButton()
	{
		if (this.detail == UINGUIUnitSelectInfo._Detail.Less)
		{
			this.detail = UINGUIUnitSelectInfo._Detail.More;
			Utility.SetActive(this.detailMoreObject, true);
			Utility.SetActive(this.detailLessObject, false);
			Utility.SetActive(this.abilityTooltipObject, false);
			Utility.SetActive(this.nonAbilityStatObject, true);
			List<int> abilityIDList = this.currentUnit.abilityIDList;
			for (int i = 0; i < this.abilityItems.Count; i++)
			{
				if (i >= abilityIDList.Count)
				{
					Utility.SetActive(this.abilityItems[i].rootObj, false);
				}
			}
		}
		else
		{
			this.detail = UINGUIUnitSelectInfo._Detail.Less;
			Utility.SetActive(this.detailMoreObject, false);
			Utility.SetActive(this.detailLessObject, true);
		}
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x0017788C File Offset: 0x00175A8C
	private void OnHoverAbilityItem(GameObject butObj, bool state)
	{
		if (state)
		{
			for (int i = 0; i < this.abilityItems.Count; i++)
			{
				if (this.abilityItems[i].rootObj == butObj)
				{
					foreach (UnitAbility unitAbility in this.unitAbilityList)
					{
						if (unitAbility.ID == this.currentUnit.abilityIDList[i])
						{
							this.lbAbilityTooltip.text = unitAbility.desp;
							break;
						}
					}
					break;
				}
			}
			Utility.SetActive(this.nonAbilityStatObject, false);
			Utility.SetActive(this.abilityTooltipObject, true);
		}
		else
		{
			Utility.SetActive(this.abilityTooltipObject, false);
			Utility.SetActive(this.nonAbilityStatObject, true);
		}
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04003BFE RID: 15358
	public NGUIButton buttonSwitch;

	// Token: 0x04003BFF RID: 15359
	private UINGUIUnitSelectInfo._Detail detail;

	// Token: 0x04003C00 RID: 15360
	private UnitTB currentUnit;

	// Token: 0x04003C01 RID: 15361
	public GameObject detailLessObject;

	// Token: 0x04003C02 RID: 15362
	public UILabel lbDesp;

	// Token: 0x04003C03 RID: 15363
	public GameObject detailMoreObject;

	// Token: 0x04003C04 RID: 15364
	public UISprite spIcon;

	// Token: 0x04003C05 RID: 15365
	public UILabel lbName;

	// Token: 0x04003C06 RID: 15366
	public UILabel lbHPAP;

	// Token: 0x04003C07 RID: 15367
	public UILabel lbDamage;

	// Token: 0x04003C08 RID: 15368
	public UILabel lbDamageVal;

	// Token: 0x04003C09 RID: 15369
	public UILabel lbHit;

	// Token: 0x04003C0A RID: 15370
	public UILabel lbHitVal;

	// Token: 0x04003C0B RID: 15371
	public UILabel lbCrit;

	// Token: 0x04003C0C RID: 15372
	public UILabel lbCritVal;

	// Token: 0x04003C0D RID: 15373
	public UILabel lbRange;

	// Token: 0x04003C0E RID: 15374
	public UILabel lbArmor;

	// Token: 0x04003C0F RID: 15375
	public UILabel lbDodge;

	// Token: 0x04003C10 RID: 15376
	public UILabel lbCritImmune;

	// Token: 0x04003C11 RID: 15377
	public UILabel lbMobility;

	// Token: 0x04003C12 RID: 15378
	public UILabel lbSpeed;

	// Token: 0x04003C13 RID: 15379
	public List<EffectItem> abilityItems = new List<EffectItem>();

	// Token: 0x04003C14 RID: 15380
	private bool isOn;

	// Token: 0x04003C15 RID: 15381
	private GameObject thisObj;

	// Token: 0x04003C16 RID: 15382
	[HideInInspector]
	public List<UnitAbility> unitAbilityList;

	// Token: 0x04003C17 RID: 15383
	public GameObject abilityTooltipObject;

	// Token: 0x04003C18 RID: 15384
	public GameObject nonAbilityStatObject;

	// Token: 0x04003C19 RID: 15385
	public UILabel lbAbilityTooltip;

	// Token: 0x020007B9 RID: 1977
	private enum _Detail
	{
		// Token: 0x04003C1B RID: 15387
		Less,
		// Token: 0x04003C1C RID: 15388
		More
	}
}
