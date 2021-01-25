using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D2 RID: 2002
public class UINGUIOverlay : MonoBehaviour
{
	// Token: 0x0600316E RID: 12654 RVA: 0x0001F1BE File Offset: 0x0001D3BE
	private void Awake()
	{
		this.thisObj = base.gameObject;
		base.transform.localPosition = Vector3.zero;
		Utility.SetActive(this.targetStatusObj, false);
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x0001F1E8 File Offset: 0x0001D3E8
	private void Start()
	{
		Utility.SetActive(this.healthDownOverlayObject, false);
		this.uintoverlay.rootObj.transform.localPosition = new Vector3(0f, 5000f, 0f);
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x0017E8D8 File Offset: 0x0017CAD8
	private void OnEnable()
	{
		GameControlTB.onBattleStartE += this.OnBattleStart;
		GameControlTB.onBattleEndE += this.OnBattleEnd;
		GameControlTB.onReset += this.OnReset;
		UnitTB.onUnitDestroyedE += this.OnUnitDestroyed;
		GridManager.onHoverTileEnterE += this.OnHoverTileEnter;
		GridManager.onHoverTileExitE += this.OnHoverTileExit;
		UnitControl.onNewUnitInRuntimeE += this.OnNewUnit;
		UnitControl.onUnitFactionChangedE += this.OnUnitFactionChanged;
		EffectOverlay.onEffectOverlayE += this.OnEffectOverlay;
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x0017E980 File Offset: 0x0017CB80
	private void OnDisable()
	{
		GameControlTB.onBattleStartE -= this.OnBattleStart;
		GameControlTB.onBattleEndE -= this.OnBattleEnd;
		GameControlTB.onReset -= this.OnReset;
		UnitTB.onUnitDestroyedE -= this.OnUnitDestroyed;
		GridManager.onHoverTileEnterE -= this.OnHoverTileEnter;
		GridManager.onHoverTileExitE -= this.OnHoverTileExit;
		UnitControl.onNewUnitInRuntimeE -= this.OnNewUnit;
		UnitControl.onUnitFactionChangedE -= this.OnUnitFactionChanged;
		EffectOverlay.onEffectOverlayE -= this.OnEffectOverlay;
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x0017EA28 File Offset: 0x0017CC28
	private void OnHoverTileEnter(Tile tile, UnitAbility uab)
	{
		UnitTB unit = tile.unit;
		UnitTB selectedUnit = UnitControl.selectedUnit;
		if (unit != null)
		{
			if (tile.attackableToSelected && selectedUnit != null)
			{
				Utility.SetActive(this.targetStatusObj, true);
				this.unitTarget.UpdateUnit(unit);
				this.unitTarget.UpdateDefense(selectedUnit, uab, unit);
				this.unitAttacker.UpdateAttack(selectedUnit, uab, unit);
				return;
			}
		}
		else if (tile.walkableToSelected)
		{
			selectedUnit != null;
		}
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x0001F21F File Offset: 0x0001D41F
	private void OnHoverTileExit()
	{
		Utility.SetActive(this.targetStatusObj, false);
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x0001F22D File Offset: 0x0001D42D
	private void OnReset()
	{
		this.OnBattleEnd(-1);
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x0017EAA4 File Offset: 0x0017CCA4
	private void OnBattleEnd(int val)
	{
		for (int i = 0; i < this.unitOverlays.Count; i++)
		{
			UnitOverlay unitOverlay = this.unitOverlays[i];
			if (unitOverlay != null && unitOverlay.rootObj != null)
			{
				Object.Destroy(unitOverlay.rootObj);
			}
		}
		this.unitOverlays.Clear();
		for (int j = 0; j < this.hpmpOverlayList.Count; j++)
		{
			GameObject gameObject = this.hpmpOverlayList[j];
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
		this.hpmpOverlayList.Clear();
		Utility.SetActive(this.targetStatusObj, false);
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x0001F236 File Offset: 0x0001D436
	private void OnBattleStart()
	{
		base.StartCoroutine(this.StartBattle());
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x0001F245 File Offset: 0x0001D445
	private IEnumerator StartBattle()
	{
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		this.healthDownOverlayObject.transform.parent.GetComponent<UIAnchor>().enabled = true;
		yield return null;
		this.InitUnitOverlay();
		yield break;
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x0017EB44 File Offset: 0x0017CD44
	private void InitUnitOverlay()
	{
		List<UnitTB> allUnit = UnitControl.GetAllUnit();
		this.uintoverlay.Init(null);
		for (int i = 0; i < this.unitOverlays.Count; i++)
		{
			UnitOverlay unitOverlay = this.unitOverlays[i];
			if (unitOverlay != null && unitOverlay.rootObj != null)
			{
				Object.Destroy(unitOverlay.rootObj);
			}
		}
		this.unitOverlays.Clear();
		for (int j = 0; j < allUnit.Count; j++)
		{
			this.unitOverlays.Add(this.uintoverlay.CloneItem("UnitOverlay" + (j + 1), allUnit[j]));
			if (allUnit[j].factionID == 0)
			{
				if (GameGlobal.m_bDLCMode)
				{
					BattleControl.instance.AddTeamExploitsList(allUnit[j]);
				}
				Utility.SetActive(this.unitOverlays[j].EnemyBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].FriendBar.gameObject, true);
				Utility.SetActive(this.unitOverlays[j].OtherBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].SpecialBar.gameObject, false);
			}
			else if (allUnit[j].factionID == 2)
			{
				Utility.SetActive(this.unitOverlays[j].EnemyBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].FriendBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].OtherBar.gameObject, true);
				Utility.SetActive(this.unitOverlays[j].SpecialBar.gameObject, false);
			}
			else if (allUnit[j].factionID == 10)
			{
				Utility.SetActive(this.unitOverlays[j].EnemyBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].FriendBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].OtherBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].SpecialBar.gameObject, true);
			}
			else
			{
				Utility.SetActive(this.unitOverlays[j].EnemyBar.gameObject, true);
				Utility.SetActive(this.unitOverlays[j].FriendBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].OtherBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[j].SpecialBar.gameObject, false);
			}
		}
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x0017EE08 File Offset: 0x0017D008
	private void OnUnitDestroyed(UnitTB unit)
	{
		for (int i = 0; i < this.unitOverlays.Count; i++)
		{
			if (unit == this.unitOverlays[i].unit)
			{
				Object.Destroy(this.unitOverlays[i].rootObj);
				this.unitOverlays.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x0017EE68 File Offset: 0x0017D068
	private void OnNewUnit(UnitTB unit)
	{
		this.unitOverlays.Add(this.uintoverlay.CloneItem("UnitOverlayN", unit));
		UnitOverlay unitOverlay = this.unitOverlays[this.unitOverlays.Count - 1];
		if (unit.factionID == 0)
		{
			if (GameGlobal.m_bDLCMode)
			{
				BattleControl.instance.AddTeamExploitsList(unit);
			}
			Utility.SetActive(unitOverlay.EnemyBar.gameObject, false);
			Utility.SetActive(unitOverlay.FriendBar.gameObject, true);
			Utility.SetActive(unitOverlay.OtherBar.gameObject, false);
			Utility.SetActive(unitOverlay.SpecialBar.gameObject, false);
			return;
		}
		if (unit.factionID == 2)
		{
			Utility.SetActive(unitOverlay.EnemyBar.gameObject, false);
			Utility.SetActive(unitOverlay.FriendBar.gameObject, false);
			Utility.SetActive(unitOverlay.OtherBar.gameObject, true);
			Utility.SetActive(unitOverlay.SpecialBar.gameObject, false);
			return;
		}
		if (unit.factionID == 10)
		{
			Utility.SetActive(unitOverlay.EnemyBar.gameObject, false);
			Utility.SetActive(unitOverlay.FriendBar.gameObject, false);
			Utility.SetActive(unitOverlay.OtherBar.gameObject, false);
			Utility.SetActive(unitOverlay.SpecialBar.gameObject, true);
			return;
		}
		Utility.SetActive(unitOverlay.EnemyBar.gameObject, true);
		Utility.SetActive(unitOverlay.FriendBar.gameObject, false);
		Utility.SetActive(unitOverlay.OtherBar.gameObject, false);
		Utility.SetActive(unitOverlay.SpecialBar.gameObject, false);
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x0017EFEC File Offset: 0x0017D1EC
	private void OnUnitFactionChanged(UnitTB unit)
	{
		int i = 0;
		while (i < this.unitOverlays.Count)
		{
			if (unit == this.unitOverlays[i].unit)
			{
				if (unit.factionID == 0)
				{
					Utility.SetActive(this.unitOverlays[i].EnemyBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].FriendBar.gameObject, true);
					Utility.SetActive(this.unitOverlays[i].OtherBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].SpecialBar.gameObject, false);
					return;
				}
				if (unit.factionID == 2)
				{
					Utility.SetActive(this.unitOverlays[i].EnemyBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].FriendBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].OtherBar.gameObject, true);
					Utility.SetActive(this.unitOverlays[i].SpecialBar.gameObject, false);
					return;
				}
				if (unit.factionID == 10)
				{
					Utility.SetActive(this.unitOverlays[i].EnemyBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].FriendBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].OtherBar.gameObject, false);
					Utility.SetActive(this.unitOverlays[i].SpecialBar.gameObject, true);
					return;
				}
				Utility.SetActive(this.unitOverlays[i].EnemyBar.gameObject, true);
				Utility.SetActive(this.unitOverlays[i].FriendBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[i].OtherBar.gameObject, false);
				Utility.SetActive(this.unitOverlays[i].SpecialBar.gameObject, false);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x0001F254 File Offset: 0x0001D454
	public void OnEffectOverlay(EffectOverlay eff)
	{
		base.StartCoroutine(this.DamageOverlayRoutine(eff));
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x0001F264 File Offset: 0x0001D464
	private IEnumerator DamageOverlayRoutine(EffectOverlay eff)
	{
		if (eff.fTime > 0f)
		{
			yield return new WaitForSeconds(eff.fTime);
		}
		Vector3 pos = eff.pos;
		Vector3 ScreenPosPlus = Vector3.zero;
		GameObject obj;
		switch (eff.overType)
		{
		case _OverlayType.HealthDown:
		{
			obj = (GameObject)Object.Instantiate(this.healthDownOverlayObject);
			UILabel component = obj.GetComponent<UILabel>();
			component.text = eff.msg;
			component.depth = 75;
			break;
		}
		case _OverlayType.HealthUp:
		{
			obj = (GameObject)Object.Instantiate(this.healthUpOverlayObject);
			UILabel component2 = obj.GetComponent<UILabel>();
			component2.text = eff.msg;
			component2.depth = 75;
			break;
		}
		case _OverlayType.InternalForceDown:
		{
			obj = (GameObject)Object.Instantiate(this.internalForceDownOverlayObject);
			UILabel component3 = obj.GetComponent<UILabel>();
			component3.text = eff.msg;
			component3.depth = 75;
			break;
		}
		case _OverlayType.InternalForceUp:
		{
			obj = (GameObject)Object.Instantiate(this.internalForceUpOverlayObject);
			UILabel component4 = obj.GetComponent<UILabel>();
			component4.text = eff.msg;
			component4.depth = 75;
			break;
		}
		case _OverlayType.Critical:
			obj = (GameObject)Object.Instantiate(this.criticalObject);
			break;
		case _OverlayType.Miss:
			obj = (GameObject)Object.Instantiate(this.missObject);
			break;
		case _OverlayType.Counter:
			obj = (GameObject)Object.Instantiate(this.conterAttackObject);
			break;
		case _OverlayType.SpLow:
			obj = (GameObject)Object.Instantiate(this.spLowObject);
			break;
		case _OverlayType.DeadPlus:
			obj = (GameObject)Object.Instantiate(this.deadPlusObject);
			break;
		case _OverlayType.AttackPlus:
			obj = (GameObject)Object.Instantiate(this.attackPlusObject);
			break;
		case _OverlayType.CantHurt:
			obj = (GameObject)Object.Instantiate(this.cantHurtObject);
			break;
		case _OverlayType.Stun:
			obj = (GameObject)Object.Instantiate(this.stunObject);
			break;
		case _OverlayType.ClearCD:
			obj = (GameObject)Object.Instantiate(this.clearCDObject);
			break;
		case _OverlayType.DamageAbsorbToSP:
			obj = (GameObject)Object.Instantiate(this.noNeedManaObject);
			break;
		case _OverlayType.PoBati:
			obj = (GameObject)Object.Instantiate(this.poBatiObject);
			break;
		case _OverlayType.SpeedRun:
			obj = (GameObject)Object.Instantiate(this.poSpeedRunObject);
			break;
		case _OverlayType.Shock:
			obj = (GameObject)Object.Instantiate(this.poShockObject);
			break;
		case _OverlayType.RemoveFlower:
			obj = (GameObject)Object.Instantiate(this.poRemoveFlowerObject);
			break;
		case _OverlayType.Talent:
		{
			obj = (GameObject)Object.Instantiate(this.talentOverlayObject);
			UILabel component5 = obj.GetComponent<UILabel>();
			component5.text = eff.msg;
			component5.depth = 75;
			break;
		}
		default:
			obj = (GameObject)Object.Instantiate(this.conterAttackObject);
			break;
		}
		this.hpmpOverlayList.Add(obj);
		obj.transform.parent = this.healthDownOverlayObject.transform.parent;
		obj.transform.localScale = Vector3.zero;
		Utility.SetActive(obj, true);
		Color color = new Color(1f, 1f, 1f, 0f);
		TweenScale.Begin(obj, 0.2f, new Vector3(1.2f, 1.2f, 1.2f));
		float duration = 0f;
		while (duration < 8f)
		{
			Vector3 vector = CameraControl.GetActiveCamera().WorldToScreenPoint(pos);
			vector *= base.gameObject.transform.root.GetComponent<UIRoot>().pixelSizeAdjustment;
			vector += ScreenPosPlus;
			if (obj == null)
			{
				break;
			}
			obj.transform.localPosition = vector;
			pos += new Vector3(0f, 3f * Time.deltaTime, 0f);
			if (duration > 0.2f)
			{
				TweenScale.Begin(obj, 1f, new Vector3(1f, 1f, 1f));
			}
			if (duration > 0.8f)
			{
				TweenColor.Begin(obj, 0.5f, color);
			}
			duration += Time.deltaTime;
			yield return null;
		}
		this.hpmpOverlayList.Remove(obj);
		Object.Destroy(obj);
		yield break;
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x0017F210 File Offset: 0x0017D410
	private void Update()
	{
		if (this.showUnitOverlay)
		{
			for (int i = 0; i < this.unitOverlays.Count; i++)
			{
				UnitOverlay unitOverlay = this.unitOverlays[i];
				UnitTB unit = unitOverlay.unit;
				if (unit != null)
				{
					Camera activeCamera = CameraControl.GetActiveCamera();
					if (activeCamera != null)
					{
						Vector3 vector = activeCamera.WorldToScreenPoint(unitOverlay.unit.thisT.position);
						vector *= base.gameObject.transform.root.GetComponent<UIRoot>().pixelSizeAdjustment;
						unitOverlay.rootT.localPosition = vector + this.uoOffset;
					}
					float value = (float)unit.HP / (float)unit.GetFullHP();
					unitOverlay.FriendBar.value = value;
					unitOverlay.EnemyBar.value = value;
					unitOverlay.OtherBar.value = value;
					unitOverlay.SpecialBar.value = value;
				}
			}
		}
	}

	// Token: 0x04003CF2 RID: 15602
	private bool showUnitOverlay = true;

	// Token: 0x04003CF3 RID: 15603
	public UnitOverlay uintoverlay;

	// Token: 0x04003CF4 RID: 15604
	public List<UnitOverlay> unitOverlays = new List<UnitOverlay>();

	// Token: 0x04003CF5 RID: 15605
	public List<GameObject> hpmpOverlayList = new List<GameObject>();

	// Token: 0x04003CF6 RID: 15606
	private Vector3 uoOffset = new Vector3(0f, -15f, 0f);

	// Token: 0x04003CF7 RID: 15607
	private Vector3 uoFaceOffset = new Vector3(0f, 30f, 0f);

	// Token: 0x04003CF8 RID: 15608
	private GameObject thisObj;

	// Token: 0x04003CF9 RID: 15609
	public GameObject targetStatusObj;

	// Token: 0x04003CFA RID: 15610
	public UINGUIHUDUnit unitAttacker;

	// Token: 0x04003CFB RID: 15611
	public UINGUIHUDUnit unitTarget;

	// Token: 0x04003CFC RID: 15612
	public GameObject healthUpOverlayObject;

	// Token: 0x04003CFD RID: 15613
	public GameObject healthDownOverlayObject;

	// Token: 0x04003CFE RID: 15614
	public GameObject internalForceUpOverlayObject;

	// Token: 0x04003CFF RID: 15615
	public GameObject internalForceDownOverlayObject;

	// Token: 0x04003D00 RID: 15616
	public GameObject talentOverlayObject;

	// Token: 0x04003D01 RID: 15617
	public GameObject conterAttackObject;

	// Token: 0x04003D02 RID: 15618
	public GameObject missObject;

	// Token: 0x04003D03 RID: 15619
	public GameObject criticalObject;

	// Token: 0x04003D04 RID: 15620
	public GameObject spLowObject;

	// Token: 0x04003D05 RID: 15621
	public GameObject attackPlusObject;

	// Token: 0x04003D06 RID: 15622
	public GameObject deadPlusObject;

	// Token: 0x04003D07 RID: 15623
	public GameObject cantHurtObject;

	// Token: 0x04003D08 RID: 15624
	public GameObject stunObject;

	// Token: 0x04003D09 RID: 15625
	public GameObject clearCDObject;

	// Token: 0x04003D0A RID: 15626
	public GameObject noNeedManaObject;

	// Token: 0x04003D0B RID: 15627
	public GameObject poBatiObject;

	// Token: 0x04003D0C RID: 15628
	public GameObject poSpeedRunObject;

	// Token: 0x04003D0D RID: 15629
	public GameObject poShockObject;

	// Token: 0x04003D0E RID: 15630
	public GameObject poRemoveFlowerObject;

	// Token: 0x04003D0F RID: 15631
	public Vector3 mod_TalentPosiionAdd = Vector3.zero;
}
