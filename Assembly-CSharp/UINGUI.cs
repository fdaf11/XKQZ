using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class UINGUI : MonoBehaviour, IController
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06003072 RID: 12402 RVA: 0x0001E943 File Offset: 0x0001CB43
	// (remove) Token: 0x06003073 RID: 12403 RVA: 0x0001E95C File Offset: 0x0001CB5C
	public event Action<KeyControl.Key> KeyDown;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06003074 RID: 12404 RVA: 0x0001E975 File Offset: 0x0001CB75
	// (remove) Token: 0x06003075 RID: 12405 RVA: 0x0001E98E File Offset: 0x0001CB8E
	public event Action<KeyControl.Key> KeyUp;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06003076 RID: 12406 RVA: 0x0001E9A7 File Offset: 0x0001CBA7
	// (remove) Token: 0x06003077 RID: 12407 RVA: 0x0001E9C0 File Offset: 0x0001CBC0
	public event Action<KeyControl.Key> KeyHeld;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06003078 RID: 12408 RVA: 0x0001E9D9 File Offset: 0x0001CBD9
	// (remove) Token: 0x06003079 RID: 12409 RVA: 0x0001E9F2 File Offset: 0x0001CBF2
	public event Action<Vector2> Move;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x0600307A RID: 12410 RVA: 0x0001EA0B File Offset: 0x0001CC0B
	// (remove) Token: 0x0600307B RID: 12411 RVA: 0x0001EA24 File Offset: 0x0001CC24
	public event Action<bool> MouseControl;

	// Token: 0x0600307C RID: 12412 RVA: 0x0001EA3D File Offset: 0x0001CC3D
	private void Awake()
	{
		UINGUI.instance = this;
		UINGUI.uiCam = base.gameObject.GetComponentInChildren<Camera>();
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x0001EA55 File Offset: 0x0001CC55
	private void Start()
	{
		if (UINGUI.uiCam != null)
		{
			GameControlTB.SetUICam(UINGUI.uiCam);
		}
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x00178094 File Offset: 0x00176294
	private void OnEnable()
	{
		GameControlTB.onGameMessageE += this.OnDisplayMessage;
		GameControlTB.onBattleMessageE += this.OnBattleMessage;
		GameControlTB.onBattleStartE += this.OnStartBattle;
		GameControlTB.onBattleEndE += this.OnBattleEnd;
		GameControlTB.onReset += this.OnResetGame;
		UnitControl.onPlacementUpdateE += this.OnUnitPlacement;
		UnitTB.onUnitSelectedE += this.OnUnitSelected;
		UnitTB.onUnitDeselectedE += this.OnUnitDeselect;
		Tile.onShowUnitInfoE += this.OnShowUnitInfo;
		UINGUI uingui = UINGUI.instance;
		uingui.KeyDown = (Action<KeyControl.Key>)Delegate.Combine(uingui.KeyDown, new Action<KeyControl.Key>(this.FaceMessageKeyDown));
		if (UINGUI.uiCam != null)
		{
			GameControlTB.SetUICam(UINGUI.uiCam);
		}
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x0017817C File Offset: 0x0017637C
	private void OnDisable()
	{
		GameControlTB.onGameMessageE -= this.OnDisplayMessage;
		GameControlTB.onBattleMessageE -= this.OnBattleMessage;
		GameControlTB.onBattleStartE -= this.OnStartBattle;
		GameControlTB.onBattleEndE -= this.OnBattleEnd;
		GameControlTB.onReset -= this.OnResetGame;
		UnitControl.onPlacementUpdateE -= this.OnUnitPlacement;
		UnitTB.onUnitSelectedE -= this.OnUnitSelected;
		UnitTB.onUnitDeselectedE -= this.OnUnitDeselect;
		Tile.onShowUnitInfoE -= this.OnShowUnitInfo;
		UINGUI uingui = UINGUI.instance;
		uingui.KeyDown = (Action<KeyControl.Key>)Delegate.Remove(uingui.KeyDown, new Action<KeyControl.Key>(this.FaceMessageKeyDown));
		for (int i = 0; i < this.msgList.Count; i++)
		{
			Object.DestroyImmediate(this.msgList[i]);
		}
		this.msgList.Clear();
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x00178280 File Offset: 0x00176480
	private void OnStartBattle()
	{
		this.bBattleEnd = false;
		this.DelayBattleControlState(BattleControlState.Unselect);
		this.bShowUnitPlacement = false;
		this.uiUnitPlacement.Hide();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(false);
		}
		this.uiHUD.Show();
		this.tlBattleMessage.GetComponent<UITextList>().Clear();
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x001782E4 File Offset: 0x001764E4
	private void OnUnitPlacement()
	{
		if (!this.bShowUnitPlacement)
		{
			this.uiUnitPlacement.SavePlaceTile();
		}
		this.bShowUnitPlacement = true;
		this.uiUnitPlacement.Show();
		this.uiHUD.Hide();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(true);
		}
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x0001EA71 File Offset: 0x0001CC71
	private void OnBattleEnd(int vicFactionID)
	{
		if (this.bBattleEnd)
		{
			return;
		}
		this.bBattleEnd = true;
		base.StartCoroutine(this._OnBattleEnd(vicFactionID));
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x00178340 File Offset: 0x00176540
	private IEnumerator _OnBattleEnd(int vicFactionID)
	{
		this.uiAbilityButtons.Hide();
		this.uiHUD.HideControlUnitInfo();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(false);
		}
		if (this.uiUnitInfo.isOn)
		{
			this.uiUnitInfo.Hide(true);
		}
		for (int i = 0; i < this.msgList.Count; i++)
		{
			Object.DestroyImmediate(this.msgList[i]);
		}
		this.msgList.Clear();
		this.DelayBattleControlState(BattleControlState.EndBattle);
		yield return new WaitForSeconds(2f);
		this.DelayBattleControlState(BattleControlState.EndBattle);
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		this.uiGameOverMenu.Show(vicFactionID);
		yield break;
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x0001EA94 File Offset: 0x0001CC94
	private void OnResetGame()
	{
		if (this.bBattleEnd)
		{
			return;
		}
		this.bBattleEnd = true;
		base.StartCoroutine(this._OnResetGame());
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x0001EAB6 File Offset: 0x0001CCB6
	public bool IsBattleEnd()
	{
		return this.bBattleEnd;
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x0017836C File Offset: 0x0017656C
	private IEnumerator _OnResetGame()
	{
		this.uiAbilityButtons.Hide();
		this.uiHUD.HideControlUnitInfo();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(false);
		}
		if (this.uiUnitInfo.isOn)
		{
			this.uiUnitInfo.Hide(true);
		}
		this.DelayBattleControlState(BattleControlState.EndBattle);
		for (int i = 0; i < this.msgList.Count; i++)
		{
			Object.DestroyImmediate(this.msgList[i]);
		}
		this.msgList.Clear();
		yield return null;
		yield break;
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x0001EABE File Offset: 0x0001CCBE
	private void OnUnitSelected(UnitTB sUnit)
	{
		base.StartCoroutine(this._OnUnitSelected(sUnit));
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x00178388 File Offset: 0x00176588
	private IEnumerator _OnUnitSelected(UnitTB sUnit)
	{
		yield return null;
		if (sUnit == null)
		{
			yield break;
		}
		if (sUnit.IsControllable() && !this.bBattleEnd)
		{
			this.uiHUD.ShowControlUnitInfo(sUnit);
			this.uiAbilityButtons.Show(sUnit);
			GridManager.instance.nowCursorTile = sUnit.occupiedTile;
			if (!sUnit.moved && sUnit.stun <= 0 && !sUnit.GetMovementDisabled() && sUnit.GetMoveRange() > 0)
			{
				GridManager.instance.nowCursorTile = sUnit.occupiedTile;
				if (!GameCursor.IsShow)
				{
					CameraControl.instance.trackNowTile = GridManager.instance.nowCursorTile;
					GridManager.OnHoverEnter(GridManager.instance.nowCursorTile);
				}
				else
				{
					CameraControl.instance.trackNowTile = null;
				}
				this.DelayBattleControlState(BattleControlState.Move);
			}
			else
			{
				this.DelayBattleControlState(BattleControlState.SelectAction);
			}
			if (this.uiItem.isOn)
			{
				this.uiItem.Hide(true);
			}
		}
		else
		{
			this.uiAbilityButtons.Hide();
			this.uiHUD.HideControlUnitInfo();
			if (this.uiItem.isOn)
			{
				this.uiItem.Hide(true);
			}
			CameraControl.instance.trackTile = false;
		}
		yield break;
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x0001EACE File Offset: 0x0001CCCE
	private void OnUnitDeselect()
	{
		this.DelayBattleControlState(BattleControlState.Unselect);
		this.uiHUD.HideControlUnitInfo();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(true);
		}
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x0001EAFE File Offset: 0x0001CCFE
	private void OnShowUnitInfo(Tile tile)
	{
		if (tile.unit != null)
		{
			this.OnShowUnitInfo(tile.unit);
		}
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x0001EB1D File Offset: 0x0001CD1D
	public void UpdateUnitInfo()
	{
		if (this.uiUnitInfo.isOn)
		{
			this.uiUnitInfo.UpdateUnit();
		}
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x0001EB3A File Offset: 0x0001CD3A
	private void OnShowUnitInfo(UnitTB unit)
	{
		this.callbackBattleControlState = this.battleControlState;
		this.DelayBattleControlState(BattleControlState.UnitInfo);
		this.uiUnitInfo.Show(unit);
		if (this.bShowUnitPlacement)
		{
			this.uiUnitPlacement.Hide();
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x0001EB71 File Offset: 0x0001CD71
	public void CloseUnitInfoCallBack()
	{
		if (this.bShowUnitPlacement)
		{
			this.uiUnitPlacement.Show();
		}
		else
		{
			this.DelayBattleControlState(this.callbackBattleControlState);
		}
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x0001EB9A File Offset: 0x0001CD9A
	public void CallBackSelectItem()
	{
		this.DelayBattleControlState(this.callbackBattleControlState);
	}

	// Token: 0x0600308F RID: 12431 RVA: 0x0001EBA8 File Offset: 0x0001CDA8
	public void DelayBattleControlState(BattleControlState bcs)
	{
		base.StartCoroutine(this._DelayBattleControlState(bcs));
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x001783B4 File Offset: 0x001765B4
	private IEnumerator _DelayBattleControlState(BattleControlState bcs)
	{
		yield return new WaitForEndOfFrame();
		this.ChangeBattleControlState(bcs);
		yield break;
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x001783E0 File Offset: 0x001765E0
	public void ChangeBattleControlState(BattleControlState bcs)
	{
		if (this.battleControlState != bcs)
		{
			Debug.Log("Change BattleControlState " + this.battleControlState.ToString() + " to " + bcs.ToString());
			this.battleControlState = bcs;
			if (this.battleControlState == BattleControlState.Unselect)
			{
				if (GameCursor.IsShow)
				{
					CameraControl.instance.trackTile = false;
				}
				else
				{
					CameraControl.instance.trackTile = true;
				}
			}
			if (this.battleControlState == BattleControlState.Move)
			{
				if (GameCursor.IsShow)
				{
					CameraControl.instance.trackTile = false;
				}
				else
				{
					GridManager.instance.nowCursorTile = UnitControl.selectedUnit.occupiedTile;
					CameraControl.instance.trackTile = true;
				}
			}
			if (this.battleControlState == BattleControlState.SelectTarget)
			{
				if (GameCursor.IsShow)
				{
					CameraControl.instance.trackTile = false;
				}
				else
				{
					GridManager.instance.nowCursorTile = UnitControl.selectedUnit.occupiedTile;
					CameraControl.instance.trackTile = true;
				}
			}
			if (this.battleControlState == BattleControlState.SelectAction)
			{
				if (GameCursor.IsShow)
				{
					this.uiAbilityButtons.ChangeStateHover(false);
					CameraControl.instance.trackTile = false;
				}
				else
				{
					this.uiAbilityButtons.ChangeStateHover(true);
					CameraControl.instance.trackTile = true;
				}
			}
			else
			{
				this.uiAbilityButtons.ChangeStateHover(false);
			}
		}
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x00178544 File Offset: 0x00176744
	private void Update()
	{
		if (Input.GetMouseButtonDown(1) && UnitControl.selectedUnit != null && UnitControl.selectedUnit.GetActiveAbilityPendingTarget() != null)
		{
			this.uiAbilityButtons.ExitUnitAbilityTargetSelect();
			UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
		}
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x00178594 File Offset: 0x00176794
	public static void OnEndTurn()
	{
		if (UnitControl.selectedUnit != null && UnitControl.selectedUnit.GetActiveAbilityPendingTarget() != null)
		{
			UINGUI.instance.uiAbilityButtons.ExitUnitAbilityTargetSelect();
		}
		if (UINGUI.instance.uiItem.isOn)
		{
			UINGUI.instance.uiItem.Hide(true);
		}
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
	private void OnBattleMessage(string msg)
	{
		this._BattleMessage(msg);
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x0001EBC1 File Offset: 0x0001CDC1
	public static void BattleMessage(string msg)
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance._BattleMessage(msg);
		}
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x0001EBDE File Offset: 0x0001CDDE
	private void _BattleMessage(string msg)
	{
		if (this.tlBattleMessage == null)
		{
			return;
		}
		if (this.tlBattleMessage.GetComponent<UITextList>() == null)
		{
			return;
		}
		this.tlBattleMessage.GetComponent<UITextList>().Add(msg);
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x0001EC1A File Offset: 0x0001CE1A
	private void OnDisplayMessage(string msg)
	{
		this._DisplayMessage(msg);
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x0001EC23 File Offset: 0x0001CE23
	public static void DisplayMessage(string msg)
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance._DisplayMessage(msg);
		}
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x001785F4 File Offset: 0x001767F4
	private void _DisplayMessage(string msg)
	{
		if (this.lbGlobalMessage == null)
		{
			return;
		}
		int num = this.msgList.Count;
		for (int i = 0; i < this.msgList.Count; i++)
		{
			if (!(this.msgList[i] == null))
			{
				Vector3 localPosition = this.lbGlobalMessage.transform.localPosition + new Vector3(0f, (float)(num * 20), 0f);
				this.msgList[i].transform.localPosition = localPosition;
				num--;
			}
		}
		GameObject gameObject = (GameObject)Object.Instantiate(this.lbGlobalMessage);
		gameObject.transform.parent = this.lbGlobalMessage.transform.parent;
		gameObject.transform.localPosition = this.lbGlobalMessage.transform.localPosition;
		gameObject.transform.localScale = this.lbGlobalMessage.transform.localScale;
		gameObject.GetComponent<UILabel>().text = msg;
		this.msgList.Add(gameObject);
		base.StartCoroutine(this.DestroyMessage(gameObject));
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x00178724 File Offset: 0x00176924
	private IEnumerator DestroyMessage(GameObject obj)
	{
		yield return new WaitForSeconds(1.25f);
		if (obj == null)
		{
			yield break;
		}
		TweenScale.Begin(obj, 0.5f, new Vector3(0.01f, 0.01f, 0.01f));
		yield return new WaitForSeconds(0.75f);
		if (obj != null)
		{
			this.msgList.Remove(obj);
			Object.DestroyImmediate(obj);
		}
		yield break;
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x0001EC40 File Offset: 0x0001CE40
	public static void FaceMessage(BattleTriggerClip battleTrigClip)
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance._OnFaceMessage(battleTrigClip);
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x00178750 File Offset: 0x00176950
	private void _OnFaceMessage(BattleTriggerClip battleTrigClip)
	{
		this.btcSaveClip = battleTrigClip;
		this.goTalkObj.SetActive(true);
		this.goTalkObj.GetComponent<UIButton>().tweenTarget = null;
		if (battleTrigClip.unit != null)
		{
			this.texTalk.mainTexture = battleTrigClip.unit.iconTalk;
			this.lTalkName.text = battleTrigClip.unit.unitName;
		}
		else
		{
			string text = string.Empty;
			CharacterData characterData = NPC.m_instance.GetCharacterData(battleTrigClip.npcID);
			if (characterData != null)
			{
				text = characterData._NpcDataNode.m_strBigHeadImage;
			}
			else
			{
				text = Game.NpcData.GetBigHeadName(battleTrigClip.npcID);
			}
			string name = "2dtexture/gameui/bighead/" + text + "_2";
			if (Game.g_BigHeadBundle.Contains(name))
			{
				this.texTalk.mainTexture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				name = "2dtexture/gameui/bighead/" + text;
				if (Game.g_BigHeadBundle.Contains(name))
				{
					this.texTalk.mainTexture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
				}
				else
				{
					name = "2dtexture/gameui/bighead/B000001";
					if (Game.g_BigHeadBundle.Contains(name))
					{
						this.texTalk.mainTexture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
					}
					else
					{
						this.texTalk.mainTexture = null;
					}
				}
			}
			this.lTalkName.text = Game.NpcData.GetNpcName(battleTrigClip.npcID);
		}
		this.lTalkText.text = battleTrigClip.msg;
		this.uiAbilityButtons.Hide();
		this.uiHUD.HideControlUnitInfo();
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(true);
		}
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x00178920 File Offset: 0x00176B20
	public void TalkClick()
	{
		if (UnitControl.selectedUnit != null && UnitControl.selectedUnit.IsControllable() && !this.bBattleEnd)
		{
			this.uiAbilityButtons.Show(UnitControl.selectedUnit);
			this.uiHUD.ShowControlUnitInfo(UnitControl.selectedUnit);
		}
		if (this.goTalkObj != null)
		{
			this.goTalkObj.SetActive(false);
		}
		if (this.onFaceMessageClick != null)
		{
			this.onFaceMessageClick(this.btcSaveClip);
		}
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x001789B0 File Offset: 0x00176BB0
	public static Vector3 GetScreenPosition(Vector3 pos)
	{
		Vector3 vector;
		vector..ctor(15f, -15f, 0f);
		UIRoot component = UINGUI.instance.transform.root.GetComponent<UIRoot>();
		float num = (float)Mathf.Clamp(Screen.height, component.minimumHeight, component.maximumHeight);
		float num2 = num / (float)Screen.height;
		float num3 = (float)(Screen.width / Screen.height) * num2;
		Vector3 vector2 = Camera.main.WorldToScreenPoint(pos);
		vector2.y *= num2;
		vector2.x *= num3;
		vector2 -= new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f) * num2;
		return vector2 + vector;
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x0001EC5D File Offset: 0x0001CE5D
	public static bool IsCursorOnUI(Vector3 point)
	{
		return UINGUI.uiCam != null && UIUtility.IsCursorOnUI(UINGUI.uiCam, point, UINGUI.uiCam.gameObject.layer);
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x00178A78 File Offset: 0x00176C78
	public void ShowBattleItemInBackpack()
	{
		if (GameControlTB.IsActionInProgress())
		{
			return;
		}
		if (UnitControl.selectedUnit == null)
		{
			return;
		}
		if (this.uiItem.isOn)
		{
			this.uiItem.Hide(true);
		}
		else
		{
			this.callbackBattleControlState = this.battleControlState;
			this.DelayBattleControlState(BattleControlState.SelectItem);
			this.uiItem.Show();
		}
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x0001EC8B File Offset: 0x0001CE8B
	public void UpdateItemBackpack()
	{
		if (this.uiItem.isOn)
		{
			this.uiItem.Show();
		}
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x0001ECA8 File Offset: 0x0001CEA8
	public void UseItem(ItemDataNode pItemDataNode)
	{
		this.uiAbilityButtons.OnUseItem(pItemDataNode);
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x0001ECB6 File Offset: 0x0001CEB6
	public void OnMove(Vector2 diretion)
	{
		if (this.Move != null)
		{
			this.Move.Invoke(diretion);
		}
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x0001ECCF File Offset: 0x0001CECF
	public void OnKeyUp(KeyControl.Key key)
	{
		if (this.KeyUp != null)
		{
			this.KeyUp.Invoke(key);
		}
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x0001ECE8 File Offset: 0x0001CEE8
	public void OnKeyDown(KeyControl.Key key)
	{
		if (this.KeyDown != null)
		{
			this.KeyDown.Invoke(key);
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x0001ED01 File Offset: 0x0001CF01
	public void OnKeyHeld(KeyControl.Key key)
	{
		if (this.KeyHeld != null)
		{
			this.KeyHeld.Invoke(key);
		}
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x0001ED1A File Offset: 0x0001CF1A
	public void OnMouseControl(bool bCtrl)
	{
		if (this.MouseControl != null)
		{
			this.MouseControl.Invoke(bCtrl);
		}
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x00178AE0 File Offset: 0x00176CE0
	private void FaceMessageKeyDown(KeyControl.Key keyCode)
	{
		if (!GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (keyCode == KeyControl.Key.OK || keyCode == KeyControl.Key.Cancel || keyCode == KeyControl.Key.BattleNextUnit)
		{
			this.KeyDownFaceMessage();
		}
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x0001ED33 File Offset: 0x0001CF33
	private void KeyDownFaceMessage()
	{
		if (GameGlobal.m_bBattleTalk)
		{
			base.StartCoroutine(this._KeyDownFaceMessage());
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x00178B20 File Offset: 0x00176D20
	private IEnumerator _KeyDownFaceMessage()
	{
		yield return new WaitForEndOfFrame();
		if (GameGlobal.m_bBattleTalk)
		{
			this.TalkClick();
		}
		yield break;
	}

	// Token: 0x04003C36 RID: 15414
	public Font uiFont;

	// Token: 0x04003C37 RID: 15415
	public BattleControlState battleControlState;

	// Token: 0x04003C38 RID: 15416
	public BattleControlState callbackBattleControlState;

	// Token: 0x04003C39 RID: 15417
	public UINGUIHUD uiHUD;

	// Token: 0x04003C3A RID: 15418
	public UINGUIUnitPlacement uiUnitPlacement;

	// Token: 0x04003C3B RID: 15419
	public UINGUIAbilityButtons uiAbilityButtons;

	// Token: 0x04003C3C RID: 15420
	public UINGUIUnitInfo uiUnitInfo;

	// Token: 0x04003C3D RID: 15421
	public UINGUIGameOverMenu uiGameOverMenu;

	// Token: 0x04003C3E RID: 15422
	public UINGUIItem uiItem;

	// Token: 0x04003C3F RID: 15423
	public static UINGUI instance;

	// Token: 0x04003C40 RID: 15424
	private bool bBattleEnd;

	// Token: 0x04003C41 RID: 15425
	private bool bShowUnitPlacement;

	// Token: 0x04003C42 RID: 15426
	private List<BattleControlState> pushBCSList = new List<BattleControlState>();

	// Token: 0x04003C43 RID: 15427
	public GameObject tlBattleMessage;

	// Token: 0x04003C44 RID: 15428
	public GameObject lbGlobalMessage;

	// Token: 0x04003C45 RID: 15429
	private List<GameObject> msgList = new List<GameObject>();

	// Token: 0x04003C46 RID: 15430
	public UINGUI.FaceMessageClick onFaceMessageClick;

	// Token: 0x04003C47 RID: 15431
	public GameObject goTalkObj;

	// Token: 0x04003C48 RID: 15432
	public UITexture texTalk;

	// Token: 0x04003C49 RID: 15433
	public UILabel lTalkName;

	// Token: 0x04003C4A RID: 15434
	public UILabel lTalkText;

	// Token: 0x04003C4B RID: 15435
	private BattleTriggerClip btcSaveClip;

	// Token: 0x04003C4C RID: 15436
	public static Camera uiCam;

	// Token: 0x020007BE RID: 1982
	// (Invoke) Token: 0x060030AC RID: 12460
	public delegate void FaceMessageClick(BattleTriggerClip battleTrigClip);
}
