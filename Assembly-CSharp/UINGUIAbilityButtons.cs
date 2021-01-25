using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007C8 RID: 1992
public class UINGUIAbilityButtons : MonoBehaviour
{
	// Token: 0x060030DE RID: 12510 RVA: 0x0017938C File Offset: 0x0017758C
	private void Awake()
	{
		this.thisObj = base.gameObject;
		base.transform.localPosition = Vector3.zero;
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		for (int j = 0; j < this.buttonList.Count; j++)
		{
			this.buttonList[j].Init();
		}
		if (this.tacticItem != null)
		{
			for (int k = 0; k < this.iTacticItemOneLineCount; k++)
			{
				GameObject gameObject = Object.Instantiate(this.tacticItem) as GameObject;
				gameObject.transform.parent = this.tacticItem.transform.parent;
				gameObject.transform.localScale = this.tacticItem.transform.localScale;
				gameObject.transform.localPosition = this.tacticItem.transform.localPosition;
				BattleUITactic component = gameObject.GetComponent<BattleUITactic>();
				if (component != null)
				{
					this.tacticItemList.Add(component);
				}
			}
		}
		if (this.tacticFace != null)
		{
			for (int l = 0; l < this.iTacticFaceOneLineCount; l++)
			{
				GameObject gameObject2 = Object.Instantiate(this.tacticFace) as GameObject;
				gameObject2.transform.parent = this.tacticFace.transform.parent;
				gameObject2.transform.localScale = this.tacticFace.transform.localScale;
				gameObject2.transform.localPosition = this.tacticFace.transform.localPosition;
				BattleUITactic component2 = gameObject2.GetComponent<BattleUITactic>();
				if (component2 != null)
				{
					this.tacticFaceList.Add(component2);
				}
			}
		}
		for (int m = 0; m < this.goRoutineBar.transform.childCount; m++)
		{
			Transform child = this.goRoutineBar.transform.GetChild(m);
			if (child.gameObject.name == "Left")
			{
				this.goPrevRoutionPage = child.gameObject;
			}
			if (child.gameObject.name == "Right")
			{
				this.goNextRoutionPage = child.gameObject;
			}
		}
		for (int n = 0; n < this.buttonList.Count; n++)
		{
			this.goList.Add(this.buttonList[n].rootObj);
		}
		this.goList.Add(this.buttonItem.gameObject);
		this.goList.Add(this.buttonEndTurn.gameObject);
		Utility.SetActive(this.selectedHightlight, false);
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x0017967C File Offset: 0x0017787C
	private void OnEnable()
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.buttonList[i].rootObj);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnAbilityButton));
			UIEventListener uieventListener2 = UIEventListener.Get(this.buttonList[i].rootObj);
			uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityButton));
		}
		for (int j = 0; j < this.routineItemList.Count; j++)
		{
			UIEventListener uieventListener3 = UIEventListener.Get(this.routineItemList[j].gameObject);
			uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnRoutionButton));
		}
		for (int k = 0; k < this.tacticItemList.Count; k++)
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.tacticItemList[k].gameObject);
			uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnTacticButton));
			UIEventListener uieventListener5 = UIEventListener.Get(this.tacticItemList[k].gameObject);
			uieventListener5.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onHover, new UIEventListener.BoolDelegate(this.OnHoverTacticButton));
		}
		for (int l = 0; l < this.tacticFaceList.Count; l++)
		{
			UIEventListener uieventListener6 = UIEventListener.Get(this.tacticFaceList[l].gameObject);
			uieventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener6.onClick, new UIEventListener.VoidDelegate(this.OnTacticFaceButton));
			UIEventListener uieventListener7 = UIEventListener.Get(this.tacticFaceList[l].gameObject);
			uieventListener7.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener7.onHover, new UIEventListener.BoolDelegate(this.OnHoverTacticFaceButton));
		}
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move += new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl += new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x001798CC File Offset: 0x00177ACC
	private void OnDisable()
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			if (this.buttonList[i].rootObj != null)
			{
				UIEventListener uieventListener = UIEventListener.Get(this.buttonList[i].rootObj);
				uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnAbilityButton));
				UIEventListener uieventListener2 = UIEventListener.Get(this.buttonList[i].rootObj);
				uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityButton));
			}
		}
		for (int j = 0; j < this.routineItemList.Count; j++)
		{
			if (this.routineItemList[j] != null && this.routineItemList[j].gameObject != null)
			{
				UIEventListener uieventListener3 = UIEventListener.Get(this.routineItemList[j].gameObject);
				uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnRoutionButton));
			}
		}
		for (int k = 0; k < this.tacticItemList.Count; k++)
		{
			if (this.tacticItemList[k] != null && this.tacticItemList[k].gameObject != null)
			{
				UIEventListener uieventListener4 = UIEventListener.Get(this.tacticItemList[k].gameObject);
				uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnTacticButton));
				UIEventListener uieventListener5 = UIEventListener.Get(this.tacticItemList[k].gameObject);
				uieventListener5.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener5.onHover, new UIEventListener.BoolDelegate(this.OnHoverTacticButton));
			}
		}
		for (int l = 0; l < this.tacticFaceList.Count; l++)
		{
			if (this.tacticFaceList[l] != null && this.tacticFaceList[l].gameObject != null)
			{
				UIEventListener uieventListener6 = UIEventListener.Get(this.tacticFaceList[l].gameObject);
				uieventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener6.onClick, new UIEventListener.VoidDelegate(this.OnTacticFaceButton));
				UIEventListener uieventListener7 = UIEventListener.Get(this.tacticFaceList[l].gameObject);
				uieventListener7.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener7.onHover, new UIEventListener.BoolDelegate(this.OnHoverTacticFaceButton));
			}
		}
		UnitTB.onUnitDeselectedE -= this.OnUnitDeselected;
		UnitTB.onUnitDestroyedE -= this.OnUnitDestroyed;
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move -= new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl -= new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x0001EE03 File Offset: 0x0001D003
	private void OnMouseControl(bool bMouse)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.SelectAction)
		{
			return;
		}
		if (bMouse)
		{
			this.ActionOnHover(this.goFocus, false);
		}
		else
		{
			this.ActionOnHover(this.goFocus, true);
		}
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x00179BF4 File Offset: 0x00177DF4
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (UINGUI.instance.battleControlState != BattleControlState.SelectAction)
		{
			return;
		}
		switch (keyCode)
		{
		case KeyControl.Key.Skill1:
			this.OnAbilityButton(this.buttonList[0].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.Skill2:
			this.OnAbilityButton(this.buttonList[1].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.Skill3:
			this.OnAbilityButton(this.buttonList[2].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.Skill4:
			this.OnAbilityButton(this.buttonList[3].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.Skill5:
			this.OnAbilityButton(this.buttonList[4].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.Skill6:
			this.OnAbilityButton(this.buttonList[5].rootObj);
			GridManager.instance.ReHoverLastTile();
			break;
		case KeyControl.Key.SelectItem:
			UINGUI.instance.ShowBattleItemInBackpack();
			break;
		case KeyControl.Key.RestTurn:
			UINGUI.instance.uiHUD.OnEndTurnButton();
			break;
		default:
			if (keyCode != KeyControl.Key.OK)
			{
				if (keyCode != KeyControl.Key.Cancel)
				{
					if (keyCode == KeyControl.Key.BattleNextUnit)
					{
						if (GameControlTB.IsPlayerTurn())
						{
							UnitControl.SwitchToNextUnitInTurnList();
						}
					}
				}
				else if (UnitControl.selectedUnit != null)
				{
					if (UnitControl.selectedUnit.bIsMoving)
					{
						UnitControl.selectedUnit.BackToOrigTile();
					}
					else
					{
						GridManager.Select(UnitControl.selectedUnit.occupiedTile);
					}
				}
			}
			else
			{
				this.ActionOnClick();
			}
			break;
		}
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x00179DCC File Offset: 0x00177FCC
	private void ActionOnClick()
	{
		if (this.goFocus == null)
		{
			return;
		}
		if (this.goList.IndexOf(this.goFocus) < this.buttonList.Count)
		{
			UIEventListener.Get(this.goFocus).onClick(this.goFocus);
		}
		else
		{
			EventDelegate.Execute(this.goFocus.GetComponent<UIButton>().onClick);
		}
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0001EE3B File Offset: 0x0001D03B
	public void OnMove(Vector2 direction)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.SelectAction)
		{
			return;
		}
		this.MoveDirect(direction);
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x00179E44 File Offset: 0x00178044
	private void MoveDirect(Vector2 direction)
	{
		if (direction.sqrMagnitude <= 0.5f)
		{
			return;
		}
		if (this.ftime + this.ftick > Time.time)
		{
			return;
		}
		this.ftime = Time.time;
		this.ActionOnHover(this.goFocus, false);
		GameObject gameObject = this.MoveDirectGameObject(direction);
		if (gameObject != null)
		{
			this.goFocus = gameObject;
		}
		else
		{
			gameObject = this.MoveInverseDirectGameObject(direction);
			if (gameObject != null)
			{
				this.goFocus = gameObject;
			}
		}
		this.ActionOnHover(this.goFocus, true);
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x00179EDC File Offset: 0x001780DC
	private void ActionOnHover(GameObject go, bool bHover)
	{
		if (this.goList.IndexOf(go) < this.buttonList.Count)
		{
			this.OnHoverAbilityButton(go, bHover);
		}
		else
		{
			this.OnHoverAbilityButton(go, false);
		}
		if (bHover && go != null)
		{
			Utility.SetActive(this.selectedHightlight, true);
			this.selectedHightlight.localPosition = new Vector3(go.transform.localPosition.x - 30f, go.transform.localPosition.y);
		}
		else
		{
			Utility.SetActive(this.selectedHightlight, false);
		}
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x0001EE55 File Offset: 0x0001D055
	public void ChangeStateHover(bool bHover)
	{
		this.ActionOnHover(this.goFocus, bHover);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x00179F88 File Offset: 0x00178188
	private GameObject MoveDirectGameObject(Vector2 direction)
	{
		if (this.goFocus == null)
		{
			return null;
		}
		GameObject result = null;
		float num = float.MaxValue;
		Vector3 vector;
		vector..ctor(direction.x, direction.y, 0f);
		Quaternion quaternion = Quaternion.LookRotation(vector);
		foreach (GameObject gameObject in this.goList)
		{
			if (!(gameObject == this.goFocus))
			{
				if (gameObject.activeSelf)
				{
					if (gameObject.GetComponent<UIButton>().isEnabled)
					{
						Vector3 vector2 = Vector3.zero;
						float num2 = Vector3.Distance(gameObject.transform.localPosition, this.goFocus.transform.localPosition);
						vector2 = gameObject.transform.localPosition - this.goFocus.transform.localPosition;
						vector2.z = 0f;
						Quaternion quaternion2 = Quaternion.LookRotation(vector2);
						float num3 = Quaternion.Angle(quaternion, quaternion2);
						if (num3 < 45.5f && num2 < num)
						{
							result = gameObject;
							num = num2;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x0017A0E0 File Offset: 0x001782E0
	private GameObject MoveInverseDirectGameObject(Vector2 direction)
	{
		if (this.goFocus == null)
		{
			return null;
		}
		direction..ctor(-direction.x, -direction.y);
		GameObject result = null;
		float num = float.MinValue;
		Vector3 vector;
		vector..ctor(direction.x, direction.y, 0f);
		Quaternion quaternion = Quaternion.LookRotation(vector);
		foreach (GameObject gameObject in this.goList)
		{
			if (!(gameObject == this.goFocus))
			{
				if (gameObject.activeSelf)
				{
					if (gameObject.GetComponent<UIButton>().isEnabled)
					{
						Vector3 vector2 = Vector3.zero;
						float num2 = Vector3.Distance(gameObject.transform.localPosition, this.goFocus.transform.localPosition);
						vector2 = gameObject.transform.localPosition - this.goFocus.transform.localPosition;
						vector2.z = 0f;
						Quaternion quaternion2 = Quaternion.LookRotation(vector2);
						float num3 = Quaternion.Angle(quaternion, quaternion2);
						if (num3 < 45.5f && num2 > num)
						{
							result = gameObject;
							num = num2;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x0017A250 File Offset: 0x00178450
	public void OnUseItem(ItemDataNode pItemDataNode)
	{
		int num = this.selectedUnit.UseItem(pItemDataNode);
		if (num > 0)
		{
			if (num == 5)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263051));
			}
			if (num == 6)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263055));
			}
			if (num == 7)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263055));
			}
		}
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x0017A2C8 File Offset: 0x001784C8
	public void OnAbilityButton(GameObject butObj)
	{
		if (GameControlTB.IsActionInProgress())
		{
			return;
		}
		int buttonID = this.GetButtonID(butObj);
		int num = this.selectedUnit.ActivateAbility(buttonID);
		if (num > 0)
		{
			if (num == 1)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263052));
			}
			if (num == 2)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263053));
			}
			if (num == 3)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263054));
			}
			if (num == 4)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263055));
			}
			if (num == 5)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263056));
			}
			if (num == 6)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263057));
			}
			if (num == 7)
			{
				this.ExitUnitAbilityTargetSelect();
				UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
			}
			if (num == 8)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263058));
			}
			if (num == 9)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263059));
			}
			if (num == 10)
			{
				UINGUI.DisplayMessage(Game.StringTable.GetString(263060));
			}
		}
		else
		{
			Utility.SetActive(this.selectedHightlight, true);
			this.selectedHightlight.position = new Vector3(this.buttonList[buttonID].rootT.position.x - 30f, this.buttonList[buttonID].rootT.position.y);
		}
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x0017A46C File Offset: 0x0017866C
	public void OnHoverAbilityButton(GameObject butObj, bool state)
	{
		string text = string.Empty;
		if (state)
		{
			if (this.selectedUnit == null)
			{
				return;
			}
			int num = this.GetButtonID(butObj);
			if (num >= this.selectedUnit.abilityIDList.Count)
			{
				return;
			}
			num = this.selectedUnit.abilityIDList[num];
			UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(num);
			this.lbAbilityName.text = unitAbility.name;
			this.lbAbilityDamage.text = this.selectedUnit.GetAbilityDamage(unitAbility).ToString();
			if (unitAbility.chainedAbilityIDList.Count <= 0)
			{
				this.lbAbilityStatus.text = Game.StringTable.GetString(263041);
			}
			else
			{
				if (unitAbility.effectType == _EffectType.Buff || unitAbility.effectType == _EffectType.Debuff)
				{
					text += "[FFC880][[-]";
				}
				int num2 = 0;
				for (int i = 0; i < unitAbility.chainedAbilityIDList.Count; i++)
				{
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(unitAbility.chainedAbilityIDList[i]);
					if (conditionNode == null)
					{
						Debug.LogError(unitAbility.chainedAbilityIDList[i].ToString() + " 狀態找不到");
					}
					else
					{
						if (num2 > 0)
						{
							text += " ";
						}
						if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.StackBuff)
						{
							text = text + "[40FF40]" + conditionNode.m_strName + "[-]";
						}
						else
						{
							text = text + "[FF3030]" + conditionNode.m_strName + "[-]";
						}
						num2++;
					}
				}
				if (unitAbility.effectType == _EffectType.Buff || unitAbility.effectType == _EffectType.Debuff)
				{
					text = text + "[FFC880]] " + Game.StringTable.GetString(263093) + "[-]";
				}
				this.lbAbilityStatus.text = text;
			}
			string text2 = string.Empty;
			if (unitAbility.targetType == _AbilityTargetType.Hostile)
			{
				text2 = Game.StringTable.GetString(263030);
			}
			if (unitAbility.targetType == _AbilityTargetType.Friendly)
			{
				text2 = Game.StringTable.GetString(263031);
			}
			if (unitAbility.targetType == _AbilityTargetType.AllUnits)
			{
				text2 = Game.StringTable.GetString(263032);
			}
			if (unitAbility.targetType == _AbilityTargetType.EmptyTile)
			{
				text2 = Game.StringTable.GetString(263033);
			}
			if (unitAbility.targetType == _AbilityTargetType.AllTile)
			{
				text2 = Game.StringTable.GetString(263034);
			}
			string text3 = string.Empty;
			if (unitAbility.targetArea == _TargetArea.Default)
			{
				if (unitAbility.requireTargetSelection)
				{
					if (unitAbility.aoeRange == 0)
					{
						text3 = string.Format(Game.StringTable.GetString(263035), unitAbility.range, text2);
					}
					else
					{
						text3 = string.Format(Game.StringTable.GetString(263036), unitAbility.range, unitAbility.aoeRange, text2);
					}
				}
				else
				{
					text3 = string.Format(Game.StringTable.GetString(263037), unitAbility.aoeRange, text2);
				}
			}
			else if (unitAbility.targetArea == _TargetArea.Line)
			{
				text3 = string.Format(Game.StringTable.GetString(263038), unitAbility.range, text2);
			}
			else if (unitAbility.targetArea == _TargetArea.Cone)
			{
				text3 = string.Format(Game.StringTable.GetString(263039), unitAbility.range, text2);
			}
			else if (unitAbility.targetArea == _TargetArea.Fan1)
			{
				text3 = string.Format(Game.StringTable.GetString(263040), unitAbility.range, text2);
			}
			text3 = text3.Replace("<br>", "\n");
			this.lbAbilityDesp.text = text3;
			this.lbAbilityCost.text = this.selectedUnit.GetAbilityCost(unitAbility, false) + Game.StringTable.GetString(263042);
			Utility.SetActive(this.InfoBoxObj, true);
		}
		else
		{
			Utility.SetActive(this.InfoBoxObj, false);
		}
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x0017A8C8 File Offset: 0x00178AC8
	public void OnRoutionButton(GameObject butObj)
	{
		if (this.selectedUnit.iRoutineID != butObj.GetComponent<BattleUIRoutine>().iRoutineID)
		{
			this.selectedUnit.ChangeRoutine(butObj.GetComponent<BattleUIRoutine>().iRoutineID);
			this.UpdateButton();
			UINGUI.instance.uiHUD.ShowControlUnitInfo(UnitControl.selectedUnit);
		}
		Utility.SetActive(this.goRoutineBar, false);
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0017A92C File Offset: 0x00178B2C
	private int GetButtonID(GameObject butObj)
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			if (butObj == this.buttonList[i].rootObj)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnTacticButton(GameObject butObj)
	{
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnHoverTacticButton(GameObject butObj, bool state)
	{
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x0017A974 File Offset: 0x00178B74
	public void OnTacticFaceButton(GameObject butObj)
	{
		BattleUITactic component = butObj.GetComponent<BattleUITactic>();
		if (component == null)
		{
			return;
		}
		BattleControl.instance.iTacticPointNow -= this.tacticPoint;
		if (BattleControl.instance.iTacticPointNow < 0)
		{
			BattleControl.instance.iTacticPointNow = 0;
		}
		if (this.iTacticMode == 2)
		{
			if (this.selectedUnit != null)
			{
				this.selectedUnit.ChangeNeigong(component.iTacticID);
			}
		}
		else
		{
			Game.g_BattleControl.UnitAddCondition(component.iTacticID, this.tacticSelectCondition, this.strTacticName);
		}
		component.tacticBox.gameObject.SetActive(false);
		Utility.SetActive(this.goTacticBar, false);
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x0017AA34 File Offset: 0x00178C34
	public void OnHoverTacticFaceButton(GameObject butObj, bool state)
	{
		BattleUITactic component = butObj.GetComponent<BattleUITactic>();
		if (component == null)
		{
			return;
		}
		if (state)
		{
			component.tacticBox.gameObject.SetActive(true);
		}
		else
		{
			component.tacticBox.gameObject.SetActive(false);
		}
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x0017AA84 File Offset: 0x00178C84
	private void ShowTacticSelect(BattleTacticNode node)
	{
		switch (node.iTargetFaction)
		{
		case 0:
			this.tacticSelectList = UnitControl.GetAllUnit();
			break;
		case 1:
			this.tacticSelectList = UnitControl.GetAllUnitsOfFaction(GameControlTB.GetPlayerFactionID());
			break;
		case 2:
			this.tacticSelectList = UnitControl.GetAllHostile(GameControlTB.GetPlayerFactionID());
			break;
		case 3:
			this.tacticSelectList = UnitControl.GetUnplacedUnit();
			break;
		}
		if (node.lConditionIDList.Count > 1)
		{
			int num = Random.Range(0, node.lConditionIDList.Count);
			this.tacticSelectCondition = node.lConditionIDList[num];
		}
		else if (node.lConditionIDList.Count == 1)
		{
			this.tacticSelectCondition = node.lConditionIDList[0];
		}
		this.strTacticName = node.sName;
		this.tacticPoint = node.iTacticPoint;
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x0001EE64 File Offset: 0x0001D064
	private void ShowNeigongSelect(BattleTacticNode node)
	{
		this.strTacticName = node.sName;
		this.tacticPoint = node.iTacticPoint;
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x0001EE7E File Offset: 0x0001D07E
	public void ExitUnitAbilityTargetSelect()
	{
		UnitControl.selectedUnit.SetActiveAbilityPendingTarget(null);
		UnitControl.selectedUnit.SetActiveItemPendingTarget(null);
		GridManager.ExitTargetTileSelectMode();
		this.OnHoverAbilityButton(null, false);
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x0017AB74 File Offset: 0x00178D74
	private void UpdateButton()
	{
		List<UnitAbility> unitAbilityList = this.selectedUnit.unitAbilityList;
		if (this.selectedUnit.useItemCD > 0 || this.selectedUnit.GetEffectPartAbsoluteDebuff(_EffectPartType.Giddy, null) || this.selectedUnit.GetEffectPartAbsoluteDebuff(_EffectPartType.Stun, null) || this.selectedUnit.bNightFragrance)
		{
			this.buttonItem.isEnabled = false;
			if (this.selectedUnit.useItemCD > 0)
			{
				this.buttonItemCD.text = this.selectedUnit.useItemCD.ToString();
			}
			else
			{
				this.buttonItemCD.text = string.Empty;
			}
		}
		else
		{
			this.buttonItem.isEnabled = true;
			this.buttonItemCD.text = string.Empty;
		}
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			if (i < unitAbilityList.Count)
			{
				if (this.selectedUnit.IsAbilityAvailable(i) == 0)
				{
					if (this.buttonList[i].spriteIcon.atlas.GetSprite(unitAbilityList[i].iconName) == null)
					{
						this.buttonList[i].spriteIcon.spriteName = "cdata_028";
						this.buttonList[i].uiButton.normalSprite = "cdata_028";
						this.buttonList[i].uiButton.disabledSprite = "cdata_028";
					}
					else
					{
						this.buttonList[i].spriteIcon.spriteName = unitAbilityList[i].iconName;
						this.buttonList[i].uiButton.normalSprite = unitAbilityList[i].iconName;
						this.buttonList[i].uiButton.disabledSprite = unitAbilityList[i].iconName;
					}
					if (unitAbilityList[i].effectType == _EffectType.Heal || unitAbilityList[i].effectType == _EffectType.Buff)
					{
						this.buttonList[i].labelName.text = string.Format("[40FF40]{0}[-]", unitAbilityList[i].name);
					}
					else
					{
						this.buttonList[i].labelName.text = unitAbilityList[i].name;
					}
					this.buttonList[i].label.text = string.Empty;
					this.buttonList[i].uiButton.isEnabled = true;
				}
				else
				{
					int abilityCD = this.selectedUnit.GetAbilityCD(i);
					if (abilityCD > 0)
					{
						this.buttonList[i].label.text = abilityCD.ToString();
					}
					else
					{
						this.buttonList[i].label.text = string.Empty;
					}
					if (this.buttonList[i].spriteIcon.atlas.GetSprite(unitAbilityList[i].iconName) == null)
					{
						this.buttonList[i].spriteIcon.spriteName = "cdata_028";
						this.buttonList[i].uiButton.normalSprite = "cdata_028";
						this.buttonList[i].uiButton.disabledSprite = "cdata_028";
					}
					else
					{
						this.buttonList[i].spriteIcon.spriteName = unitAbilityList[i].iconName;
						this.buttonList[i].uiButton.normalSprite = unitAbilityList[i].iconName;
						this.buttonList[i].uiButton.disabledSprite = unitAbilityList[i].iconName;
					}
					this.buttonList[i].uiButton.isEnabled = false;
					this.buttonList[i].labelName.text = unitAbilityList[i].name;
				}
				Utility.SetActive(this.buttonList[i].rootObj, true);
			}
			else
			{
				Utility.SetActive(this.buttonList[i].rootObj, false);
			}
		}
		for (int j = 0; j < this.routineItemList.Count; j++)
		{
			this.routineItemList[j].gameObject.SetActive(false);
		}
		this.goNextRoutionPage.SetActive(false);
		this.goPrevRoutionPage.SetActive(false);
		int num;
		if (this.iRoutionStartPos + this.iRoutionOneLineCount < this.selectedUnit.routineIDList.Count)
		{
			num = this.iRoutionOneLineCount;
			this.goNextRoutionPage.SetActive(true);
		}
		else
		{
			num = this.selectedUnit.routineIDList.Count - this.iRoutionStartPos;
		}
		if (this.iRoutionStartPos != 0)
		{
			this.goPrevRoutionPage.SetActive(true);
		}
		float num2 = 0f;
		if (num > 0)
		{
			num2 = -80f * (float)(num - 1);
		}
		int num3 = 0;
		for (int k = this.iRoutionStartPos; k < this.iRoutionStartPos + num; k++)
		{
			this.routineItemList[num3].SetRoutine(this.selectedUnit.routineIDList[k]);
			this.routineItemList[num3].gameObject.transform.localPosition = new Vector3(num2, -25f, 0f);
			this.routineItemList[num3].gameObject.SetActive(true);
			num2 += 160f;
			num3++;
		}
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x0017B12C File Offset: 0x0017932C
	private void InitFocusButton()
	{
		for (int i = 0; i < this.goList.Count; i++)
		{
			UIButton component = this.goList[i].GetComponent<UIButton>();
			if (component.isEnabled && component.gameObject.activeSelf)
			{
				this.goFocus = this.goList[i];
				break;
			}
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x0001EEA3 File Offset: 0x0001D0A3
	private void OnNextPageRoutionClick(GameObject butObj)
	{
		this.iRoutionStartPos += this.iRoutionOneLineCount;
		this.UpdateButton();
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x0001EEBE File Offset: 0x0001D0BE
	private void OnPrevPageRoutionClick(GameObject butObj)
	{
		this.iRoutionStartPos -= this.iRoutionOneLineCount;
		this.UpdateButton();
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x0017B19C File Offset: 0x0017939C
	private void OnNextPageClick(GameObject butObj)
	{
		if (this.iTacticMode == 0)
		{
			this.iTacticItemIndex += this.iTacticItemOneLineCount;
			this.UpdateTactic();
		}
		else if (this.iTacticMode == 1)
		{
			this.iTacticFaceIndex += this.iTacticFaceOneLineCount;
			this.UpdateSelectTarget();
		}
		else if (this.iTacticMode == 2)
		{
			this.iTacticFaceIndex += this.iTacticFaceOneLineCount;
			this.UpdateNeigong();
		}
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x0017B224 File Offset: 0x00179424
	private void OnPrevPageClick(GameObject butObj)
	{
		if (this.iTacticMode == 0)
		{
			this.iTacticItemIndex -= this.iTacticItemOneLineCount;
			this.UpdateTactic();
		}
		else if (this.iTacticMode == 1)
		{
			this.iTacticFaceIndex -= this.iTacticFaceOneLineCount;
			this.UpdateSelectTarget();
		}
		else if (this.iTacticMode == 2)
		{
			this.iTacticFaceIndex -= this.iTacticFaceOneLineCount;
			this.UpdateNeigong();
		}
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x0017B2AC File Offset: 0x001794AC
	private void UpdateNeigong()
	{
		this.iTacticMode = 2;
		for (int i = 0; i < this.tacticItemList.Count; i++)
		{
			this.tacticItemList[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.tacticFaceList.Count; j++)
		{
			this.tacticFaceList[j].gameObject.SetActive(false);
		}
		int num = 0;
		if (this.selectedUnit != null)
		{
			num = this.selectedUnit.characterData.NeigongList.Count;
		}
		this.goNextPage.SetActive(false);
		this.goPrevPage.SetActive(false);
		int num2;
		if (num > this.iTacticFaceIndex + this.iTacticFaceOneLineCount)
		{
			num2 = this.iTacticFaceOneLineCount;
			this.goNextPage.SetActive(true);
		}
		else
		{
			num2 = num - this.iTacticFaceIndex;
		}
		if (this.iTacticFaceIndex > 0)
		{
			this.goPrevPage.SetActive(true);
		}
		float num3 = 0f;
		if (num2 > 1)
		{
			num3 = -75f * (float)(num2 - 1);
		}
		int num4 = 0;
		int num5 = this.iTacticFaceIndex;
		while (num5 < num && num5 < this.iTacticFaceIndex + this.iTacticFaceOneLineCount)
		{
			this.tacticFaceList[num4].gameObject.transform.localPosition = new Vector3(num3, this.tacticFaceList[num4].gameObject.transform.localPosition.y);
			this.tacticFaceList[num4].SetTacticNeigong(this.selectedUnit.characterData.NeigongList[num5]);
			this.tacticFaceList[num4].gameObject.SetActive(true);
			num3 += 150f;
			num4++;
			num5++;
		}
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x0000264F File Offset: 0x0000084F
	private void UpdateTactic()
	{
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x0017B49C File Offset: 0x0017969C
	private void UpdateSelectTarget()
	{
		this.iTacticMode = 1;
		for (int i = 0; i < this.tacticItemList.Count; i++)
		{
			this.tacticItemList[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.tacticFaceList.Count; j++)
		{
			this.tacticFaceList[j].gameObject.SetActive(false);
		}
		int count = this.tacticSelectList.Count;
		this.goNextPage.SetActive(false);
		this.goPrevPage.SetActive(false);
		int num;
		if (count > this.iTacticFaceIndex + this.iTacticFaceOneLineCount)
		{
			num = this.iTacticFaceOneLineCount;
			this.goNextPage.SetActive(true);
		}
		else
		{
			num = count - this.iTacticFaceIndex;
		}
		if (this.iTacticFaceIndex > 0)
		{
			this.goPrevPage.SetActive(true);
		}
		float num2 = 0f;
		if (num > 1)
		{
			num2 = -75f * (float)(num - 1);
		}
		int num3 = 0;
		int num4 = this.iTacticFaceIndex;
		while (num4 < count && num4 < this.iTacticFaceIndex + this.iTacticFaceOneLineCount)
		{
			this.tacticFaceList[num3].gameObject.transform.localPosition = new Vector3(num2, this.tacticFaceList[num3].gameObject.transform.localPosition.y);
			this.tacticFaceList[num3].SetTacticFace(this.tacticSelectList[num4]);
			this.tacticFaceList[num3].gameObject.SetActive(true);
			num2 += 150f;
			num3++;
			num4++;
		}
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x0017B664 File Offset: 0x00179864
	public void ArrangeButtons(int num)
	{
		float num2 = (float)((num - 1) * 70);
		float num3 = -num2 / 2f;
		for (int i = 0; i < num; i++)
		{
			this.buttonList[i].rootT.localPosition = new Vector3(num3 + (float)(i * 70), this.buttonList[i].rootT.localPosition.y, this.buttonList[i].rootT.localPosition.z);
		}
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x0001EED9 File Offset: 0x0001D0D9
	private void OnUnitDestroyed(UnitTB unit)
	{
		if (unit == this.selectedUnit)
		{
			this.OnUnitDeselected();
		}
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x0001EEF2 File Offset: 0x0001D0F2
	private void OnUnitDeselected()
	{
		this.Hide();
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x0017B6F4 File Offset: 0x001798F4
	public void Show(UnitTB sUnit)
	{
		this.selectedUnit = sUnit;
		UnitTB.onUnitDeselectedE += this.OnUnitDeselected;
		UnitTB.onUnitDestroyedE += this.OnUnitDestroyed;
		Utility.SetActive(this.thisObj, true);
		Utility.SetActive(this.InfoBoxObj, false);
		this.UpdateButton();
		this.InitFocusButton();
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x0017B750 File Offset: 0x00179950
	public void Hide()
	{
		this.selectedUnit = null;
		UnitTB.onUnitDeselectedE -= this.OnUnitDeselected;
		UnitTB.onUnitDestroyedE -= this.OnUnitDestroyed;
		Utility.SetActive(this.thisObj, false);
		Utility.SetActive(this.goRoutineBar, false);
		Utility.SetActive(this.goTacticBar, false);
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x0017B7AC File Offset: 0x001799AC
	public void RoutineOnClick()
	{
		bool activeSelf = this.goRoutineBar.activeSelf;
		if (activeSelf)
		{
			Utility.SetActive(this.goRoutineBar, false);
		}
		else
		{
			Utility.SetActive(this.goRoutineBar, true);
		}
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x0017B7E8 File Offset: 0x001799E8
	public void TacticOnClick()
	{
		bool activeSelf = this.goTacticBar.activeSelf;
		if (activeSelf)
		{
			Utility.SetActive(this.goTacticBar, false);
		}
		else
		{
			this.iTacticItemIndex = 0;
			this.UpdateTactic();
			Utility.SetActive(this.goTacticBar, true);
		}
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x0001EEFA File Offset: 0x0001D0FA
	public void TacticOnClose()
	{
		Utility.SetActive(this.goTacticBar, false);
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x0001EF08 File Offset: 0x0001D108
	public void RoutionOnClose()
	{
		Utility.SetActive(this.goRoutineBar, false);
	}

	// Token: 0x04003C76 RID: 15478
	public List<NGUIButton> buttonList = new List<NGUIButton>();

	// Token: 0x04003C77 RID: 15479
	public GameObject InfoBoxObj;

	// Token: 0x04003C78 RID: 15480
	public UILabel lbAbilityName;

	// Token: 0x04003C79 RID: 15481
	public UILabel lbAbilityDesp;

	// Token: 0x04003C7A RID: 15482
	public UILabel lbAbilityCost;

	// Token: 0x04003C7B RID: 15483
	public UILabel lbAbilityDamage;

	// Token: 0x04003C7C RID: 15484
	public UILabel lbAbilityStatus;

	// Token: 0x04003C7D RID: 15485
	public Transform selectedHightlight;

	// Token: 0x04003C7E RID: 15486
	private UnitTB selectedUnit;

	// Token: 0x04003C7F RID: 15487
	public GameObject thisObj;

	// Token: 0x04003C80 RID: 15488
	public UIButton buttonItem;

	// Token: 0x04003C81 RID: 15489
	public UILabel buttonItemCD;

	// Token: 0x04003C82 RID: 15490
	public UIButton buttonEndTurn;

	// Token: 0x04003C83 RID: 15491
	public GameObject goRoutineBar;

	// Token: 0x04003C84 RID: 15492
	public GameObject goRoutineItem;

	// Token: 0x04003C85 RID: 15493
	public List<BattleUIRoutine> routineItemList = new List<BattleUIRoutine>();

	// Token: 0x04003C86 RID: 15494
	public GameObject goTacticBar;

	// Token: 0x04003C87 RID: 15495
	private int iTacticItemIndex;

	// Token: 0x04003C88 RID: 15496
	private int iTacticItemOneLineCount = 13;

	// Token: 0x04003C89 RID: 15497
	public GameObject tacticItem;

	// Token: 0x04003C8A RID: 15498
	public List<BattleUITactic> tacticItemList = new List<BattleUITactic>();

	// Token: 0x04003C8B RID: 15499
	private int iTacticFaceIndex;

	// Token: 0x04003C8C RID: 15500
	private int iTacticFaceOneLineCount = 9;

	// Token: 0x04003C8D RID: 15501
	public GameObject tacticFace;

	// Token: 0x04003C8E RID: 15502
	public List<BattleUITactic> tacticFaceList = new List<BattleUITactic>();

	// Token: 0x04003C8F RID: 15503
	private List<UnitTB> tacticSelectList;

	// Token: 0x04003C90 RID: 15504
	private int tacticSelectCondition;

	// Token: 0x04003C91 RID: 15505
	private int tacticPoint;

	// Token: 0x04003C92 RID: 15506
	private string strTacticName;

	// Token: 0x04003C93 RID: 15507
	public GameObject goNextPage;

	// Token: 0x04003C94 RID: 15508
	public GameObject goPrevPage;

	// Token: 0x04003C95 RID: 15509
	public GameObject goTooltip;

	// Token: 0x04003C96 RID: 15510
	public UILabel lbTooltip;

	// Token: 0x04003C97 RID: 15511
	public UILabel lbTooltipDesc;

	// Token: 0x04003C98 RID: 15512
	public UILabel lbTooltipPoint;

	// Token: 0x04003C99 RID: 15513
	public UILabel lbTacticPoint;

	// Token: 0x04003C9A RID: 15514
	private GameObject goNextRoutionPage;

	// Token: 0x04003C9B RID: 15515
	private GameObject goPrevRoutionPage;

	// Token: 0x04003C9C RID: 15516
	private int iRoutionOneLineCount = 10;

	// Token: 0x04003C9D RID: 15517
	private int iRoutionStartPos;

	// Token: 0x04003C9E RID: 15518
	private int iTacticMode;

	// Token: 0x04003C9F RID: 15519
	private List<GameObject> goList = new List<GameObject>();

	// Token: 0x04003CA0 RID: 15520
	private GameObject goFocus;

	// Token: 0x04003CA1 RID: 15521
	private float ftime;

	// Token: 0x04003CA2 RID: 15522
	private float ftick = 0.15f;
}
