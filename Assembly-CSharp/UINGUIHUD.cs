using System;
using System.Collections;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007CD RID: 1997
public class UINGUIHUD : MonoBehaviour
{
	// Token: 0x06003133 RID: 12595 RVA: 0x0001EFDC File Offset: 0x0001D1DC
	private void Awake()
	{
		this.thisObj = base.gameObject;
		base.transform.localPosition = Vector3.zero;
		this.currentUnit = null;
		Utility.SetActive(this.playerStatusObj, false);
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0017CF38 File Offset: 0x0017B138
	private void OnEnable()
	{
		GameControlTB.onNextTurnE += this.OnNextTurn;
		GameControlTB.onBattleEndE += this.OnBattleEnd;
		GameControlTB.onReset += this.Hide;
		GameControlTB.onNewRoundE += this.OnNewRound;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x0017CF8C File Offset: 0x0017B18C
	private void OnDisable()
	{
		GameControlTB.onNextTurnE -= this.OnNextTurn;
		GameControlTB.onBattleEndE -= this.OnBattleEnd;
		GameControlTB.onReset -= this.Hide;
		GameControlTB.onNewRoundE -= this.OnNewRound;
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x0017CFE0 File Offset: 0x0017B1E0
	private void OnNewRound(int iRound)
	{
		string text = iRound.ToString();
		if (GameControlTB.instance.GetLoseRound() > 0)
		{
			text = text + "/[FF0000]" + GameControlTB.instance.GetLoseRound().ToString() + "[-]";
		}
		if (GameControlTB.instance.GetWinRound() > 0)
		{
			text = text + "/[00FF00]" + GameControlTB.instance.GetWinRound().ToString() + "[-]";
		}
		this.labelTurn.text = string.Format(Game.StringTable.GetString(263066), text);
		base.StartCoroutine(this._OnNewRound(iRound));
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x0017D08C File Offset: 0x0017B28C
	private IEnumerator _OnNewRound(int iRound)
	{
		yield return null;
		this.labelRound.text = string.Format(Game.StringTable.GetString(263065), iRound);
		Utility.SetActive(this.spriteRound.gameObject, true);
		Color color = new Color(1f, 1f, 1f, 1f);
		TweenColor.Begin(this.spriteRound.gameObject, 0.3f, color);
		yield return new WaitForSeconds(1f);
		color = new Color(1f, 1f, 1f, 0f);
		TweenColor.Begin(this.spriteRound.gameObject, 0.5f, color);
		yield return new WaitForSeconds(0.6f);
		yield break;
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x0017D0B8 File Offset: 0x0017B2B8
	private void OnNextTurn()
	{
		if (this.thisObj == null)
		{
			return;
		}
		if (!this.thisObj.activeSelf)
		{
			return;
		}
		if (!this.thisObj.activeInHierarchy)
		{
			return;
		}
		base.StartCoroutine(this._OnNextTurn());
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x0017D108 File Offset: 0x0017B308
	private IEnumerator _OnNextTurn()
	{
		yield return null;
		if (GameControlTB.IsPlayerTurn())
		{
			this.buttonTactic.isEnabled = false;
			Utility.SetActive(this.playerControlObj, true);
		}
		else
		{
			Utility.SetActive(this.playerControlObj, false);
			Utility.SetActive(this.playerStatusObj, false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0017D124 File Offset: 0x0017B324
	public void OnEndTurnButton()
	{
		if (GameControlTB.IsActionInProgress())
		{
			return;
		}
		if (!GameControlTB.IsPlayerTurn())
		{
			return;
		}
		if (this.currentUnit == null)
		{
			return;
		}
		if (UnitControl.selectedUnit != null)
		{
			UINGUI.OnEndTurn();
			this.HideControlUnitInfo();
			UnitControl.selectedUnit.RestAndRecoveHPMP();
		}
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x0017D180 File Offset: 0x0017B380
	public void Show()
	{
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		Utility.SetActive(this.thisObj, true);
		Utility.SetActive(this.unitAttacker.gameObject, false);
		this.labelTurn.text = string.Empty;
		if (!GameControlTB.IsPlayerTurn())
		{
			Utility.SetActive(this.playerControlObj, false);
			Utility.SetActive(this.playerStatusObj, false);
		}
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x0001F019 File Offset: 0x0001D219
	public void Hide()
	{
		this.unitAttacker.HideTooltip();
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x0001F032 File Offset: 0x0001D232
	private void OnBattleEnd(int vicFactionID)
	{
		this.Hide();
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x0001F03A File Offset: 0x0001D23A
	public void ShowControlUnitInfo(UnitTB unit)
	{
		this.currentUnit = unit;
		Utility.SetActive(this.playerControlObj, true);
		Utility.SetActive(this.playerStatusObj, true);
		this.unitAttacker.UpdateUnit(unit);
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x0001F067 File Offset: 0x0001D267
	public void HideControlUnitInfo()
	{
		this.currentUnit = null;
		Utility.SetActive(this.playerControlObj, false);
		Utility.SetActive(this.playerStatusObj, false);
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0017D208 File Offset: 0x0017B408
	public void OnMessageClick()
	{
		if (this.bShowMessage)
		{
			this.bShowMessage = false;
		}
		else
		{
			this.bShowMessage = true;
		}
		if (this.battleMessageObj != null)
		{
			this.battleMessageObj.SetActive(this.bShowMessage);
		}
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x0000264F File Offset: 0x0000084F
	public void UpdateItemButton()
	{
	}

	// Token: 0x04003CBC RID: 15548
	public GameObject playerControlObj;

	// Token: 0x04003CBD RID: 15549
	public GameObject playerStatusObj;

	// Token: 0x04003CBE RID: 15550
	public UIButton buttonTactic;

	// Token: 0x04003CBF RID: 15551
	public UISprite spriteRound;

	// Token: 0x04003CC0 RID: 15552
	public UILabel labelRound;

	// Token: 0x04003CC1 RID: 15553
	public UILabel labelTurn;

	// Token: 0x04003CC2 RID: 15554
	private GameObject thisObj;

	// Token: 0x04003CC3 RID: 15555
	private UnitTB currentUnit;

	// Token: 0x04003CC4 RID: 15556
	public GameObject battleMessageObj;

	// Token: 0x04003CC5 RID: 15557
	private bool bShowMessage;

	// Token: 0x04003CC6 RID: 15558
	public UINGUIHUDUnit unitAttacker;

	// Token: 0x04003CC7 RID: 15559
	public GameObject goDeadthWinLose;

	// Token: 0x04003CC8 RID: 15560
	public UILabel goWinLoseText;
}
