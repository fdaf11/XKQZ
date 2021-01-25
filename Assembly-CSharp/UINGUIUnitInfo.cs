using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020007DA RID: 2010
public class UINGUIUnitInfo : MonoBehaviour
{
	// Token: 0x060031A8 RID: 12712 RVA: 0x001811E0 File Offset: 0x0017F3E0
	private void Awake()
	{
		this.thisObj = base.gameObject;
		this.mod_NeigongLblTextPositon = this.lbNeigongStr.transform.localPosition;
		base.transform.localPosition = Vector3.zero;
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		for (int j = 0; j < this.abilityItems.Count; j++)
		{
			this.abilityItems[j].Init();
			UIEventListener uieventListener = UIEventListener.Get(this.abilityItems[j].rootObj);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityButton));
			this.abilityItemGameObjectList.Add(this.abilityItems[j].rootObj);
		}
		for (int k = 0; k < this.m_iEffectCount; k++)
		{
			if (k == 0)
			{
				this.effectItems[k].Init();
			}
			else
			{
				this.effectItems.Add(this.effectItems[0].CloneItem("Icon" + (k + 1).ToString(), new Vector3((float)(k * 55), 0f, 0f)));
			}
			UIEventListener uieventListener2 = UIEventListener.Get(this.effectItems[k].rootObj);
			uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnHoverEffectItem));
			this.conditionGameObjectList.Add(this.effectItems[k].rootObj);
		}
		UIEventListener uieventListener3 = UIEventListener.Get(this.lbHPVal.gameObject);
		uieventListener3.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener3.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbHPVal.gameObject);
		UIEventListener uieventListener4 = UIEventListener.Get(this.lbSPVal.gameObject);
		uieventListener4.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener4.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbSPVal.gameObject);
		UIEventListener uieventListener5 = UIEventListener.Get(this.lbDamageVal.gameObject);
		uieventListener5.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbDamageVal.gameObject);
		UIEventListener uieventListener6 = UIEventListener.Get(this.lbHitVal.gameObject);
		uieventListener6.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener6.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbHitVal.gameObject);
		UIEventListener uieventListener7 = UIEventListener.Get(this.lbCritVal.gameObject);
		uieventListener7.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener7.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbCritVal.gameObject);
		UIEventListener uieventListener8 = UIEventListener.Get(this.lbDodgeVal.gameObject);
		uieventListener8.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener8.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbDodgeVal.gameObject);
		UIEventListener uieventListener9 = UIEventListener.Get(this.lbCritDefVal.gameObject);
		uieventListener9.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener9.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbCritDefVal.gameObject);
		UIEventListener uieventListener10 = UIEventListener.Get(this.lbCounterVal.gameObject);
		uieventListener10.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener10.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbCounterVal.gameObject);
		UIEventListener uieventListener11 = UIEventListener.Get(this.lbMoveRangeVal.gameObject);
		uieventListener11.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener11.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbMoveRangeVal.gameObject);
		UIEventListener uieventListener12 = UIEventListener.Get(this.lbCounterDefVal.gameObject);
		uieventListener12.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener12.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbCounterDefVal.gameObject);
		UIEventListener uieventListener13 = UIEventListener.Get(this.lbArmorVal1.gameObject);
		uieventListener13.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener13.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal1.gameObject);
		UIEventListener uieventListener14 = UIEventListener.Get(this.lbArmorVal2.gameObject);
		uieventListener14.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener14.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal2.gameObject);
		UIEventListener uieventListener15 = UIEventListener.Get(this.lbArmorVal3.gameObject);
		uieventListener15.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener15.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal3.gameObject);
		UIEventListener uieventListener16 = UIEventListener.Get(this.lbArmorVal4.gameObject);
		uieventListener16.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener16.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal4.gameObject);
		UIEventListener uieventListener17 = UIEventListener.Get(this.lbArmorVal5.gameObject);
		uieventListener17.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener17.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal5.gameObject);
		UIEventListener uieventListener18 = UIEventListener.Get(this.lbArmorVal6.gameObject);
		uieventListener18.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener18.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal6.gameObject);
		UIEventListener uieventListener19 = UIEventListener.Get(this.lbArmorVal7.gameObject);
		uieventListener19.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener19.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal7.gameObject);
		UIEventListener uieventListener20 = UIEventListener.Get(this.lbArmorVal8.gameObject);
		uieventListener20.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener20.onHover, new UIEventListener.BoolDelegate(this.OnHoverStateItem));
		this.stateItemGameObjectList.Add(this.lbArmorVal8.gameObject);
		UIEventListener uieventListener21 = UIEventListener.Get(this.lbNeigongStr.gameObject);
		uieventListener21.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener21.onHover, new UIEventListener.BoolDelegate(this.OnHoverNeigong));
		UIEventListener uieventListener22 = UIEventListener.Get(this.lbTalent_1.gameObject);
		uieventListener22.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener22.onHover, new UIEventListener.BoolDelegate(this.OnHoverTalentItem));
		this.talentItemGameObjectList.Add(this.lbTalent_1.gameObject);
		UIEventListener uieventListener23 = UIEventListener.Get(this.lbTalent_2.gameObject);
		uieventListener23.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener23.onHover, new UIEventListener.BoolDelegate(this.OnHoverTalentItem));
		this.talentItemGameObjectList.Add(this.lbTalent_2.gameObject);
		UIEventListener uieventListener24 = UIEventListener.Get(this.lbTalent_3.gameObject);
		uieventListener24.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener24.onHover, new UIEventListener.BoolDelegate(this.OnHoverTalentItem));
		this.talentItemGameObjectList.Add(this.lbTalent_3.gameObject);
		UIEventListener uieventListener25 = UIEventListener.Get(this.lbTalent_4.gameObject);
		uieventListener25.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener25.onHover, new UIEventListener.BoolDelegate(this.OnHoverTalentItem));
		this.talentItemGameObjectList.Add(this.lbTalent_4.gameObject);
		UIEventListener uieventListener26 = UIEventListener.Get(this.lbTalent_5.gameObject);
		uieventListener26.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener26.onHover, new UIEventListener.BoolDelegate(this.OnHoverTalentItem));
		this.talentItemGameObjectList.Add(this.lbTalent_5.gameObject);
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x00181A20 File Offset: 0x0017FC20
	private void OnEnable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move += new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl += new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x00181A80 File Offset: 0x0017FC80
	private void OnDisable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move -= new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl -= new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x0001F321 File Offset: 0x0001D521
	private void OnMouseControl(bool bMouse)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.UnitInfo)
		{
			return;
		}
		if (bMouse)
		{
			this.ShowFocus(false);
		}
		else
		{
			this.ShowFocus(true);
		}
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00181AE0 File Offset: 0x0017FCE0
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (UINGUI.instance.battleControlState != BattleControlState.UnitInfo)
		{
			return;
		}
		if (keyCode == KeyControl.Key.Cancel)
		{
			this.ShowFocus(false);
			this.Hide(false);
		}
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x0001F34D File Offset: 0x0001D54D
	public void OnMove(Vector2 direction)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.UnitInfo)
		{
			return;
		}
		this.MoveDirect(direction);
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x00181B2C File Offset: 0x0017FD2C
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
		GameObject gameObject = this.MoveDirectGameObject(direction);
		if (gameObject != null)
		{
			this.goFocus = gameObject;
			this.ShowFocus(true);
		}
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x00181B90 File Offset: 0x0017FD90
	private void ShowFocus(bool bShow)
	{
		this.goHighLight.SetActive(false);
		this.goConditionHighLight.SetActive(false);
		this.goAbilityHighLight.SetActive(false);
		this.OnHoverStateItem(this.goFocus, false);
		this.OnHoverEffectItem(this.goFocus, false);
		this.OnHoverAbilityButton(this.goFocus, false);
		this.OnHoverNeigong(this.goFocus, false);
		this.OnHoverTalentItem(this.goFocus, false);
		if (this.goFocus == null)
		{
			return;
		}
		if (bShow)
		{
			if (this.stateItemGameObjectList.Contains(this.goFocus))
			{
				this.OnHoverStateItem(this.goFocus, true);
				this.goHighLight.SetActive(true);
				this.goHighLight.transform.parent = this.goFocus.transform;
				this.goHighLight.transform.localPosition = new Vector3(-150f, 0f);
			}
			if (this.conditionGameObjectList.Contains(this.goFocus))
			{
				this.OnHoverEffectItem(this.goFocus, true);
				this.goConditionHighLight.SetActive(true);
				this.goConditionHighLight.transform.parent = this.goFocus.transform;
				this.goConditionHighLight.transform.localPosition = new Vector3(0f, 0f);
			}
			if (this.abilityItemGameObjectList.Contains(this.goFocus))
			{
				this.OnHoverAbilityButton(this.goFocus, true);
				this.goAbilityHighLight.SetActive(true);
				this.goAbilityHighLight.transform.parent = this.goFocus.transform;
				this.goAbilityHighLight.transform.localPosition = new Vector3(0f, 0f);
			}
			if (this.talentItemGameObjectList.Contains(this.goFocus))
			{
				this.OnHoverTalentItem(this.goFocus, true);
				this.goHighLight.SetActive(true);
				this.goHighLight.transform.parent = this.goFocus.transform;
				this.goHighLight.transform.localPosition = new Vector3(150f, 0f);
			}
			if (this.goFocus == this.lbNeigongStr.gameObject)
			{
				this.OnHoverNeigong(this.goFocus, true);
				this.goHighLight.SetActive(true);
				this.goHighLight.transform.parent = this.goFocus.transform;
				this.goHighLight.transform.localPosition = new Vector3(150f, 0f);
			}
		}
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x00181E2C File Offset: 0x0018002C
	private GameObject MoveDirectGameObject(Vector2 direction)
	{
		Vector3 vector = Vector3.zero;
		Vector2 vector2;
		vector2..ctor(0.5f, 0.5f);
		Vector2 vector3 = vector2 - this.goFocus.GetComponent<UIWidget>().pivotOffset;
		vector3..ctor((float)this.goFocus.GetComponent<UIWidget>().width * vector3.x, (float)this.goFocus.GetComponent<UIWidget>().height * vector3.y);
		Vector2 vector4 = Vector2.zero;
		if (this.stateItemGameObjectList.Contains(this.goFocus))
		{
			UIWidget component = this.goFocus.transform.parent.gameObject.GetComponent<UIWidget>();
			vector4 = vector2 - component.pivotOffset;
			vector4..ctor((float)component.width * vector4.x, (float)component.height * vector4.y);
			vector = this.goFocus.transform.localPosition + this.goFocus.transform.parent.localPosition + this.goFocus.transform.parent.parent.localPosition + new Vector3(vector4.x, vector4.y, 0f);
		}
		else if (this.conditionGameObjectList.Contains(this.goFocus))
		{
			vector = this.goFocus.transform.localPosition;
		}
		else if (this.abilityItemGameObjectList.Contains(this.goFocus))
		{
			vector = this.goFocus.transform.localPosition + this.goFocus.transform.parent.localPosition;
		}
		else if (this.talentItemGameObjectList.Contains(this.goFocus))
		{
			vector = this.goFocus.transform.localPosition + this.goFocus.transform.parent.localPosition;
		}
		else if (this.goFocus == this.lbNeigongStr.gameObject)
		{
			vector = this.goFocus.transform.localPosition + this.goFocus.transform.parent.localPosition;
		}
		else
		{
			vector = this.goFocus.transform.localPosition;
		}
		vector += new Vector3(vector3.x, vector3.y);
		GameObject result = null;
		float num = float.MaxValue;
		Vector3 vector5;
		vector5..ctor(direction.x, direction.y, 0f);
		Quaternion quaternion = Quaternion.LookRotation(vector5, Vector3.forward);
		foreach (GameObject gameObject in this.stateItemGameObjectList)
		{
			if (!(gameObject == this.goFocus))
			{
				if (gameObject.activeSelf)
				{
					vector3 = vector2 - gameObject.GetComponent<UIWidget>().pivotOffset;
					vector3..ctor((float)gameObject.GetComponent<UIWidget>().width * vector3.x, (float)gameObject.GetComponent<UIWidget>().height * vector3.y);
					UIWidget component = gameObject.transform.parent.gameObject.GetComponent<UIWidget>();
					vector4 = vector2 - component.pivotOffset;
					vector4..ctor((float)component.width * vector4.x, (float)component.height * vector4.y);
					Vector3 vector6 = gameObject.transform.localPosition + gameObject.transform.parent.localPosition + gameObject.transform.parent.parent.localPosition + new Vector3(vector3.x, vector3.y) + new Vector3(vector4.x, vector4.y, 0f);
					float num2 = Vector3.Distance(vector6, vector);
					vector6 -= vector;
					vector6.z = 0f;
					Quaternion quaternion2 = Quaternion.LookRotation(vector6, Vector3.forward);
					float num3 = Quaternion.Angle(quaternion, quaternion2);
					if (num3 < 45.5f && num2 < num)
					{
						result = gameObject;
						num = num2;
					}
				}
			}
		}
		foreach (GameObject gameObject2 in this.conditionGameObjectList)
		{
			if (!(gameObject2 == this.goFocus))
			{
				if (gameObject2.activeSelf)
				{
					vector3 = vector2 - gameObject2.GetComponent<UIWidget>().pivotOffset;
					vector3..ctor((float)gameObject2.GetComponent<UIWidget>().width * vector3.x, (float)gameObject2.GetComponent<UIWidget>().height * vector3.y);
					Vector3 vector7 = gameObject2.transform.localPosition + new Vector3(vector3.x, vector3.y);
					float num4 = Vector3.Distance(vector7, vector);
					vector7 -= vector;
					vector7.z = 0f;
					Quaternion quaternion3 = Quaternion.LookRotation(vector7, Vector3.forward);
					float num5 = Quaternion.Angle(quaternion, quaternion3);
					if (num5 < 45.5f && num4 < num)
					{
						result = gameObject2;
						num = num4;
					}
				}
			}
		}
		foreach (GameObject gameObject3 in this.abilityItemGameObjectList)
		{
			if (!(gameObject3 == this.goFocus))
			{
				if (gameObject3.activeSelf)
				{
					vector3 = vector2 - gameObject3.GetComponent<UIWidget>().pivotOffset;
					vector3..ctor((float)gameObject3.GetComponent<UIWidget>().width * vector3.x, (float)gameObject3.GetComponent<UIWidget>().height * vector3.y);
					Vector3 vector8 = gameObject3.transform.localPosition + gameObject3.transform.parent.localPosition + new Vector3(vector3.x, vector3.y);
					float num6 = Vector3.Distance(vector8, vector);
					vector8 -= vector;
					vector8.z = 0f;
					Quaternion quaternion4 = Quaternion.LookRotation(vector8, Vector3.forward);
					float num7 = Quaternion.Angle(quaternion, quaternion4);
					if (num7 < 45.5f && num6 < num)
					{
						result = gameObject3;
						num = num6;
					}
				}
			}
		}
		foreach (GameObject gameObject4 in this.talentItemGameObjectList)
		{
			if (!(gameObject4 == this.goFocus))
			{
				if (gameObject4.activeSelf)
				{
					vector3 = vector2 - gameObject4.GetComponent<UIWidget>().pivotOffset;
					vector3..ctor((float)gameObject4.GetComponent<UIWidget>().width * vector3.x, (float)gameObject4.GetComponent<UIWidget>().height * vector3.y);
					Vector3 vector9 = gameObject4.transform.localPosition + gameObject4.transform.parent.localPosition + new Vector3(vector3.x, vector3.y);
					float num8 = Vector3.Distance(vector9, vector);
					vector9 -= vector;
					vector9.z = 0f;
					Quaternion quaternion5 = Quaternion.LookRotation(vector9, Vector3.forward);
					float num9 = Quaternion.Angle(quaternion, quaternion5);
					if (num9 < 45.5f && num8 < num)
					{
						result = gameObject4;
						num = num8;
					}
				}
			}
		}
		if (this.goFocus != this.lbNeigongStr.gameObject && this.lbNeigongStr.gameObject.activeSelf)
		{
			GameObject gameObject5 = this.lbNeigongStr.gameObject;
			vector3 = vector2 - gameObject5.GetComponent<UIWidget>().pivotOffset;
			vector3..ctor((float)gameObject5.GetComponent<UIWidget>().width * vector3.x, (float)gameObject5.GetComponent<UIWidget>().height * vector3.y);
			Vector3 vector10 = this.lbNeigongStr.gameObject.transform.localPosition + this.lbNeigongStr.gameObject.transform.parent.localPosition + new Vector3(vector3.x, vector3.y);
			float num10 = Vector3.Distance(vector10, vector);
			vector10 -= vector;
			vector10.z = 0f;
			Quaternion quaternion6 = Quaternion.LookRotation(vector10, Vector3.forward);
			float num11 = Quaternion.Angle(quaternion, quaternion6);
			if (num11 < 45.5f && num10 < num)
			{
				result = this.lbNeigongStr.gameObject;
				num = num10;
			}
		}
		return result;
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x001827B8 File Offset: 0x001809B8
	public void OnHoverTalentItem(GameObject hoverObj, bool state)
	{
		if (state)
		{
			if (hoverObj == null)
			{
				return;
			}
			if (!hoverObj.activeSelf)
			{
				return;
			}
			BtnData component = hoverObj.GetComponent<BtnData>();
			if (component == null)
			{
				return;
			}
			TalentNewDataNode talentData = TalentNewManager.Singleton.GetTalentData(component.m_iBtnID);
			if (talentData == null)
			{
				return;
			}
			this.lbTooltip.text = talentData.m_strTalentName;
			this.lbTooltipDesc.text = talentData.m_strTalentTip;
			Utility.SetActive(this.tooltipObj, true);
		}
		else
		{
			Utility.SetActive(this.tooltipObj, false);
		}
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x00182850 File Offset: 0x00180A50
	public void OnHoverStateItem(GameObject hoverObj, bool state)
	{
		if (state)
		{
			Hover component = hoverObj.GetComponent<Hover>();
			List<string> list = this.currentUnit.GetEffectString(component.epType);
			if (list.Count <= 0)
			{
				Utility.SetActive(this.conditionTooltipObj, false);
				return;
			}
			string text = string.Empty;
			UIWidget uiwidget = this.conditionTooltipValue;
			int height = list.Count * 36 + 4;
			this.conditionTooltip.height = height;
			uiwidget.height = height;
			for (int i = 0; i < list.Count; i++)
			{
				if (i + 1 < list.Count)
				{
					text = text + list[i] + "\n";
				}
				else
				{
					text += list[i];
				}
			}
			this.conditionTooltip.text = text;
			text = string.Empty;
			list = this.currentUnit.GetEffectValueString(component.epType);
			for (int j = 0; j < list.Count; j++)
			{
				if (j + 1 < list.Count)
				{
					text = text + list[j] + "\n";
				}
				else
				{
					text += list[j];
				}
			}
			this.conditionTooltipValue.text = text;
			UISprite component2 = this.conditionTooltipObj.GetComponent<UISprite>();
			component2.height = (1 + list.Count) * 36;
			Utility.SetActive(this.conditionTooltipObj, true);
		}
		else
		{
			Utility.SetActive(this.conditionTooltipObj, false);
		}
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x001829CC File Offset: 0x00180BCC
	public void OnHoverEffectItem(GameObject butObj, bool state)
	{
		if (state)
		{
			int num = 0;
			for (int i = 0; i < this.effectItems.Count; i++)
			{
				if (this.effectItems[i].rootObj == butObj)
				{
					num = i;
				}
			}
			List<ConditionNode> allCondition = this.currentUnit.GetAllCondition();
			if (num < allCondition.Count)
			{
				ConditionNode conditionNode = allCondition[num];
				string text = string.Empty;
				if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff)
				{
					if (conditionNode.m_iRemoveByAttack > 0)
					{
						text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
					}
					else if (conditionNode.m_iRemoveOnHit > 0)
					{
						text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveOnHit);
					}
					else
					{
						text = string.Format("[40FF40]{0}[-]", conditionNode.m_strName);
					}
				}
				else if (conditionNode.m_iCondType == _ConditionType.StackBuff)
				{
					text = string.Format("[40FF40]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
				}
				else if (conditionNode.m_iCondType == _ConditionType.Debuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff)
				{
					if (conditionNode.m_iRemoveByAttack > 0)
					{
						text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
					}
					else if (conditionNode.m_iRemoveOnHit > 0)
					{
						text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveOnHit);
					}
					else
					{
						text = string.Format("[FF3030]{0}[-]", conditionNode.m_strName);
					}
				}
				else if (conditionNode.m_iCondType == _ConditionType.StackDebuff)
				{
					text = string.Format("[FF3030]{0}({1})[-]", conditionNode.m_strName, conditionNode.m_iRemoveByAttack);
				}
				this.lbTooltip.text = text;
				this.lbTooltipDesc.text = conditionNode.m_strDesp;
				Utility.SetActive(this.tooltipObj, true);
			}
		}
		else
		{
			Utility.SetActive(this.tooltipObj, false);
		}
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x00182BEC File Offset: 0x00180DEC
	public void OnHoverNeigong(GameObject butObj, bool state)
	{
		if (state && this.currentUnit != null && this.currentUnit.unitNeigong != null)
		{
			this.lbNeigongTooltip.text = this.currentUnit.unitNeigong.m_strNeigongName;
			string text = string.Empty;
			int count = NeigongDataManager.Singleton.GetNeigongData(this.currentUnit.unitNeigong.m_iNeigongID).m_ConditionEffectList.Count;
			for (int i = 0; i < this.currentUnit.unitNeigongConditionList.Count; i++)
			{
				if (i < count)
				{
					ConditionNode conditionNode = this.currentUnit.unitNeigongConditionList[i];
					string[] array = conditionNode.m_strDesp.Split(new char[]
					{
						"\n".get_Chars(0)
					});
					for (int j = 0; j < array.Length; j++)
					{
						if (j == 0)
						{
							text += string.Format("[FFC880]{0}[-]:{1}\n", conditionNode.m_strName, array[j]);
						}
						else
						{
							text += string.Format("[00000000]{0}[-]:{1}\n", conditionNode.m_strName, array[j]);
						}
					}
				}
			}
			text = text.TrimEnd(new char[0]);
			this.lbNeigongTooltipDesc.text = text;
			Utility.SetActive(this.neigongTooltipObj, true);
			return;
		}
		Utility.SetActive(this.neigongTooltipObj, false);
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x00182D44 File Offset: 0x00180F44
	private string GetColorString(_EffectPartType epType)
	{
		if (this.currentUnit == null)
		{
			return string.Empty;
		}
		string result;
		if (this.currentUnit.GetUnitTileAbsoluteBuff(epType, null))
		{
			result = "[FFC880]";
		}
		else if (this.currentUnit.GetUnitTileAbsoluteDebuff(epType, null))
		{
			result = "[FF4040]";
		}
		else
		{
			float num = this.currentUnit.GetEffectPartValue(epType, null, 0);
			if (this.currentUnit.occupiedTile != null)
			{
				num += this.currentUnit.occupiedTile.GetEffectPartValue(epType, this.currentUnit);
			}
			if (num < 0f)
			{
				result = "[FF0000]";
			}
			else if (num > 0f)
			{
				result = "[00FF00]";
			}
			else
			{
				num = this.currentUnit.GetEffectPartValuePercent(epType, null, 0) + this.currentUnit.occupiedTile.GetEffectPartValuePercent(epType, this.currentUnit);
				if (num < 0f)
				{
					result = "[FF0000]";
				}
				else if (num > 0f)
				{
					result = "[00FF00]";
				}
				else
				{
					result = "[FFFFFF]";
				}
			}
		}
		return result;
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x0001F367 File Offset: 0x0001D567
	public void UpdateUnit()
	{
		this.Show(this.currentUnit);
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x00182E68 File Offset: 0x00181068
	public void Show(UnitTB unit)
	{
		this.goFocus = this.stateItemGameObjectList[0];
		unit.audioTB.PlaySelect();
		this.isOn = true;
		Utility.SetActive(this.thisObj, true);
		this.currentUnit = unit;
		string name = "2dtexture/gameui/maphead/" + unit.characterData._NpcDataNode.m_strHalfImage + "_2";
		if (Game.g_MapHeadBundle.Contains(name))
		{
			this.texPlayer.mainTexture = (Game.g_MapHeadBundle.Load(name) as Texture2D);
		}
		else
		{
			name = "2dtexture/gameui/maphead/" + unit.characterData._NpcDataNode.m_strHalfImage;
			if (Game.g_MapHeadBundle.Contains(name))
			{
				this.texPlayer.mainTexture = (Game.g_MapHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				this.texPlayer.mainTexture = null;
			}
		}
		this.lbName.text = unit.unitName + unit.characterData.NeigongList.Count.ToString();
		string colorString = this.GetColorString(_EffectPartType.HitPoint);
		this.lbHPVal.text = string.Concat(new object[]
		{
			colorString,
			unit.HP,
			"[-]/",
			unit.GetFullHP()
		});
		colorString = this.GetColorString(_EffectPartType.InternalForce);
		this.lbSPVal.text = string.Concat(new object[]
		{
			colorString,
			unit.SP,
			"[-]/",
			unit.GetFullSP()
		});
		float num = this.currentUnit.GetEffectPartValue(_EffectPartType.Damage, null, 0);
		if (this.currentUnit.occupiedTile != null)
		{
			num += this.currentUnit.occupiedTile.GetEffectPartValue(_EffectPartType.Damage, this.currentUnit);
		}
		float num2 = this.currentUnit.GetEffectPartValuePercent(_EffectPartType.Damage, null, 0);
		if (this.currentUnit.occupiedTile != null)
		{
			num2 += this.currentUnit.occupiedTile.GetEffectPartValuePercent(_EffectPartType.Damage, this.currentUnit);
		}
		string text = string.Empty;
		if (num == 0f && num2 == 0f)
		{
			text = "0";
		}
		if (num != 0f)
		{
			if (num > 0f)
			{
				text = text + "[00FF00]+" + num.ToString() + "[-]";
			}
			else
			{
				text = text + "[FF0000]" + num.ToString() + "[-]";
			}
		}
		if (num2 != 0f)
		{
			if (num2 > 0f)
			{
				text = text + "[00FF00]+" + num2.ToString() + "%[-]";
			}
			else
			{
				text = text + "[FF0000]" + num2.ToString() + "%[-]";
			}
		}
		this.lbDamageVal.text = text;
		colorString = this.GetColorString(_EffectPartType.HitChance);
		this.lbHitVal.text = colorString + (unit.GetAbilityHitRate(null, 0, unit.occupiedTile) * 100f).ToString("f0") + "%[-]";
		colorString = this.GetColorString(_EffectPartType.CriticalChance);
		this.lbCritVal.text = colorString + (unit.GetAbilityCriticalRate(null, 0, unit.occupiedTile) * 100f).ToString("f0") + "%[-]";
		colorString = this.GetColorString(_EffectPartType.DamageReduction);
		this.lbArmorVal1.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(1)).ToString("f0") + "[-]";
		this.lbArmorVal2.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(2)).ToString("f0") + "[-]";
		this.lbArmorVal3.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(3)).ToString("f0") + "[-]";
		this.lbArmorVal4.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(4)).ToString("f0") + "[-]";
		this.lbArmorVal5.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(5)).ToString("f0") + "[-]";
		this.lbArmorVal6.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(6)).ToString("f0") + "[-]";
		this.lbArmorVal7.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(7)).ToString("f0") + "[-]";
		this.lbArmorVal8.text = colorString + unit.GetDamageReduc((float)unit.GetMartialTypeDef(8)).ToString("f0") + "[-]";
		colorString = this.GetColorString(_EffectPartType.DodgeChance);
		this.lbDodgeVal.text = colorString + (unit.GetDodgeRate(null, 0) * 100f).ToString("f0") + "%[-]";
		colorString = this.GetColorString(_EffectPartType.DefCriticalChance);
		this.lbCritDefVal.text = colorString + (unit.GetDefanceCriticalRate(null, 0) * 100f).ToString("f0") + "%[-]";
		colorString = this.GetColorString(_EffectPartType.CounterAttackChance);
		this.lbCounterVal.text = colorString + (unit.GetCounterRate(null, 0) * 100f).ToString("f0") + "%[-]";
		colorString = this.GetColorString(_EffectPartType.MoveRange);
		this.lbMoveRangeVal.text = colorString + unit.GetMoveRange().ToString("f0") + "[-]";
		colorString = this.GetColorString(_EffectPartType.DefCounterAttackChance);
		this.lbCounterDefVal.text = colorString + (unit.GetAbilityDefanceCounterRate(null, 0) * 100f).ToString("f0") + "%[-]";
		if (unit.unitNeigong != null)
		{
			this.lbNeigongStr.text = unit.unitNeigong.m_strNeigongName + " " + unit.iNeigongLv.ToString();
		}
		else
		{
			this.lbNeigongStr.text = string.Empty;
		}
		if (unit.unitNeigong != null)
		{
			List<NpcNeigong> list = unit.characterData.mod_GetNowUsedNeigong().FindAll((NpcNeigong x) => x.m_Neigong != unit.unitNeigong);
			this.lbNeigongStr.autoResizeBoxCollider = false;
			this.lbNeigongStr.height = 300;
			this.lbNeigongStr.maxLineCount = 0;
			this.lbNeigongStr.fontSize = 27;
			this.lbNeigongStr.transform.localPosition = this.mod_NeigongLblTextPositon - new Vector3(0f, (float)list.Count * 12f, 0f);
			for (int i = 0; i < list.Count; i++)
			{
				UILabel uilabel = this.lbNeigongStr;
				uilabel.text = string.Concat(new string[]
				{
					uilabel.text,
					"\n",
					list[i].m_Neigong.m_strNeigongName,
					" ",
					list[i].iLevel.ToString()
				});
			}
		}
		int iID = unit.characterData._NpcDataNode.m_iMartialType + 140001;
		this.spfaction.spriteName = iID.ToString();
		this.lbfaction.text = Game.StringTable.GetString(iID);
		this.lbTitle.text = unit.characterData._NpcDataNode.m_strTitle;
		this.lbIntroduction.text = unit.characterData._NpcDataNode.m_strDescription;
		List<UnitAbility> unitAbilityList = unit.unitAbilityList;
		for (int j = 0; j < this.abilityItems.Count; j++)
		{
			if (j < unitAbilityList.Count)
			{
				if (this.abilityItems[j].spIcon.atlas.GetSprite(unitAbilityList[j].iconName) == null)
				{
					this.abilityItems[j].spIcon.spriteName = "s00008";
				}
				else
				{
					this.abilityItems[j].spIcon.spriteName = unitAbilityList[j].iconName;
				}
				Utility.SetActive(this.abilityItems[j].rootObj, true);
			}
			else
			{
				Utility.SetActive(this.abilityItems[j].rootObj, false);
			}
		}
		List<ConditionNode> allCondition = unit.GetAllCondition();
		if (allCondition.Count == 0)
		{
			for (int k = 0; k < this.effectItems.Count; k++)
			{
				Utility.SetActive(this.effectItems[k].rootObj, false);
			}
		}
		else
		{
			for (int l = 0; l < this.effectItems.Count; l++)
			{
				if (l < allCondition.Count)
				{
					this.effectItems[l].spIcon.spriteName = allCondition[l].m_strIconName;
					Utility.SetActive(this.effectItems[l].rootObj, true);
				}
				else
				{
					Utility.SetActive(this.effectItems[l].rootObj, false);
				}
			}
		}
		for (int m = 0; m < this.effectBoxBG.Count; m++)
		{
			this.effectBoxBG[m].width = Mathf.Max(130, 10 + allCondition.Count * 45);
		}
		for (int n = 0; n < this.talentItemGameObjectList.Count; n++)
		{
			Utility.SetActive(this.talentItemGameObjectList[n], false);
		}
		int num3 = 0;
		for (int num4 = 0; num4 < unit.characterData.TalentList.Count; num4++)
		{
			if (unit.characterData.TalentList[num4] > 300 && num3 < this.talentItemGameObjectList.Count)
			{
				Utility.SetActive(this.talentItemGameObjectList[num3], true);
				if (this.talentItemGameObjectList[num3].GetComponent<BtnData>() != null && this.talentItemGameObjectList[num3].GetComponent<UILabel>() != null)
				{
					this.talentItemGameObjectList[num3].GetComponent<UILabel>().text = TalentNewManager.Singleton.GetTalentName(unit.characterData.TalentList[num4]);
					this.talentItemGameObjectList[num3].GetComponent<BtnData>().m_iBtnID = unit.characterData.TalentList[num4];
				}
				num3++;
			}
		}
		Utility.SetActive(this.tooltipObj, false);
		Utility.SetActive(this.neigongTooltipObj, false);
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x0001F375 File Offset: 0x0001D575
	public void Hide(bool bDontCallBack = false)
	{
		this.isOn = false;
		this.currentUnit = null;
		Utility.SetActive(this.thisObj, false);
		if (!bDontCallBack)
		{
			UINGUI.instance.CloseUnitInfoCallBack();
		}
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x00183A58 File Offset: 0x00181C58
	public void OnHoverAbilityButton(GameObject butObj, bool state)
	{
		string text = string.Empty;
		if (state)
		{
			if (this.currentUnit == null)
			{
				Utility.SetActive(this.InfoBoxObj, false);
				return;
			}
			int num = 0;
			for (int i = 0; i < this.abilityItems.Count; i++)
			{
				if (this.abilityItems[i].rootObj == butObj)
				{
					num = i;
				}
			}
			if (num >= this.currentUnit.abilityIDList.Count)
			{
				Utility.SetActive(this.InfoBoxObj, false);
				return;
			}
			num = this.currentUnit.abilityIDList[num];
			UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(num);
			this.lbAbilityName.text = unitAbility.name + " " + this.currentUnit.GetRoutineLv(num).ToString();
			this.lbAbilityDamage.text = this.currentUnit.GetAbilityDamage(unitAbility).ToString();
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
				for (int j = 0; j < unitAbility.chainedAbilityIDList.Count; j++)
				{
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(unitAbility.chainedAbilityIDList[j]);
					if (conditionNode == null)
					{
						Debug.LogError(unitAbility.chainedAbilityIDList[j].ToString() + " 狀態找不到");
					}
					else
					{
						if (num2 > 0)
						{
							text += " ";
						}
						if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.StackBuff)
						{
							text = text + "[40FF40]" + conditionNode.m_strName + " [-]";
						}
						else
						{
							text = text + "[FF3030]" + conditionNode.m_strName + " [-]";
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
						text = Game.StringTable.GetString(263035).Replace("<br>", "\n");
						text3 = string.Format(text, unitAbility.range, text2);
					}
					else
					{
						text = Game.StringTable.GetString(263036).Replace("<br>", "\n");
						text3 = string.Format(text, unitAbility.range, unitAbility.aoeRange, text2);
					}
				}
				else
				{
					text = Game.StringTable.GetString(263037).Replace("<br>", "\n");
					text3 = string.Format(text, unitAbility.aoeRange, text2);
				}
			}
			else if (unitAbility.targetArea == _TargetArea.Line)
			{
				text = Game.StringTable.GetString(263038).Replace("<br>", "\n");
				text3 = string.Format(text, unitAbility.range, text2);
			}
			else if (unitAbility.targetArea == _TargetArea.Cone)
			{
				text = Game.StringTable.GetString(263039).Replace("<br>", "\n");
				text3 = string.Format(text, unitAbility.range, text2);
			}
			else if (unitAbility.targetArea == _TargetArea.Fan1)
			{
				text = Game.StringTable.GetString(263040).Replace("<br>", "\n");
				text3 = string.Format(text, unitAbility.range, text2);
			}
			this.lbAbilityDesp.text = text3;
			this.lbAbilityCost.text = this.currentUnit.GetAbilityCost(unitAbility, false) + Game.StringTable.GetString(263042);
			Utility.SetActive(this.InfoBoxObj, true);
		}
		else
		{
			Utility.SetActive(this.InfoBoxObj, false);
		}
	}

	// Token: 0x04003D47 RID: 15687
	[HideInInspector]
	public bool isOn;

	// Token: 0x04003D48 RID: 15688
	private int m_iEffectCount = 16;

	// Token: 0x04003D49 RID: 15689
	public List<EffectItem> effectItems = new List<EffectItem>();

	// Token: 0x04003D4A RID: 15690
	private List<UISprite> effectBoxBG = new List<UISprite>();

	// Token: 0x04003D4B RID: 15691
	public GameObject goHighLight;

	// Token: 0x04003D4C RID: 15692
	public GameObject goConditionHighLight;

	// Token: 0x04003D4D RID: 15693
	public GameObject goAbilityHighLight;

	// Token: 0x04003D4E RID: 15694
	public GameObject tooltipObj;

	// Token: 0x04003D4F RID: 15695
	public UILabel lbTooltip;

	// Token: 0x04003D50 RID: 15696
	public UILabel lbTooltipDesc;

	// Token: 0x04003D51 RID: 15697
	public GameObject neigongTooltipObj;

	// Token: 0x04003D52 RID: 15698
	public UILabel lbNeigongTooltip;

	// Token: 0x04003D53 RID: 15699
	public UILabel lbNeigongTooltipDesc;

	// Token: 0x04003D54 RID: 15700
	public GameObject conditionTooltipObj;

	// Token: 0x04003D55 RID: 15701
	public UILabel conditionTooltip;

	// Token: 0x04003D56 RID: 15702
	public UILabel conditionTooltipValue;

	// Token: 0x04003D57 RID: 15703
	private List<GameObject> conditionGameObjectList = new List<GameObject>();

	// Token: 0x04003D58 RID: 15704
	private List<GameObject> stateItemGameObjectList = new List<GameObject>();

	// Token: 0x04003D59 RID: 15705
	private List<GameObject> abilityItemGameObjectList = new List<GameObject>();

	// Token: 0x04003D5A RID: 15706
	private List<GameObject> talentItemGameObjectList = new List<GameObject>();

	// Token: 0x04003D5B RID: 15707
	private GameObject goFocus;

	// Token: 0x04003D5C RID: 15708
	private float ftime;

	// Token: 0x04003D5D RID: 15709
	private float ftick = 0.2f;

	// Token: 0x04003D5E RID: 15710
	private GameObject thisObj;

	// Token: 0x04003D5F RID: 15711
	private UnitTB currentUnit;

	// Token: 0x04003D60 RID: 15712
	public UITexture texPlayer;

	// Token: 0x04003D61 RID: 15713
	public UILabel lbName;

	// Token: 0x04003D62 RID: 15714
	public UILabel lbHPVal;

	// Token: 0x04003D63 RID: 15715
	public UILabel lbSPVal;

	// Token: 0x04003D64 RID: 15716
	public UILabel lbDamageVal;

	// Token: 0x04003D65 RID: 15717
	public UILabel lbHitVal;

	// Token: 0x04003D66 RID: 15718
	public UILabel lbCritVal;

	// Token: 0x04003D67 RID: 15719
	public UILabel lbDodgeVal;

	// Token: 0x04003D68 RID: 15720
	public UILabel lbCritDefVal;

	// Token: 0x04003D69 RID: 15721
	public UILabel lbCounterVal;

	// Token: 0x04003D6A RID: 15722
	public UILabel lbMoveRangeVal;

	// Token: 0x04003D6B RID: 15723
	public UILabel lbCounterDefVal;

	// Token: 0x04003D6C RID: 15724
	public UILabel lbArmorVal1;

	// Token: 0x04003D6D RID: 15725
	public UILabel lbArmorVal2;

	// Token: 0x04003D6E RID: 15726
	public UILabel lbArmorVal3;

	// Token: 0x04003D6F RID: 15727
	public UILabel lbArmorVal4;

	// Token: 0x04003D70 RID: 15728
	public UILabel lbArmorVal5;

	// Token: 0x04003D71 RID: 15729
	public UILabel lbArmorVal6;

	// Token: 0x04003D72 RID: 15730
	public UILabel lbArmorVal7;

	// Token: 0x04003D73 RID: 15731
	public UILabel lbArmorVal8;

	// Token: 0x04003D74 RID: 15732
	public UILabel lbAbility;

	// Token: 0x04003D75 RID: 15733
	public List<EffectItem> abilityItems = new List<EffectItem>();

	// Token: 0x04003D76 RID: 15734
	public UILabel lbNeigongStr;

	// Token: 0x04003D77 RID: 15735
	public UISprite spfaction;

	// Token: 0x04003D78 RID: 15736
	public UILabel lbfaction;

	// Token: 0x04003D79 RID: 15737
	public UILabel lbTitle;

	// Token: 0x04003D7A RID: 15738
	public UILabel lbIntroduction;

	// Token: 0x04003D7B RID: 15739
	public UILabel lbTalent_1;

	// Token: 0x04003D7C RID: 15740
	public UILabel lbTalent_2;

	// Token: 0x04003D7D RID: 15741
	public UILabel lbTalent_3;

	// Token: 0x04003D7E RID: 15742
	public UILabel lbTalent_4;

	// Token: 0x04003D7F RID: 15743
	public UILabel lbTalent_5;

	// Token: 0x04003D80 RID: 15744
	public GameObject InfoBoxObj;

	// Token: 0x04003D81 RID: 15745
	public UILabel lbAbilityName;

	// Token: 0x04003D82 RID: 15746
	public UILabel lbAbilityDesp;

	// Token: 0x04003D83 RID: 15747
	public UILabel lbAbilityCost;

	// Token: 0x04003D84 RID: 15748
	public UILabel lbAbilityDamage;

	// Token: 0x04003D85 RID: 15749
	public UILabel lbAbilityStatus;

	// Token: 0x04003D86 RID: 15750
	public Vector3 mod_NeigongLblTextPositon;
}
