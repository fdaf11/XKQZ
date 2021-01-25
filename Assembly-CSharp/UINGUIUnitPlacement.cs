using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007F5 RID: 2037
public class UINGUIUnitPlacement : MonoBehaviour
{
	// Token: 0x060031F3 RID: 12787 RVA: 0x00183FF0 File Offset: 0x001821F0
	private void Awake()
	{
		this.thisObj = base.gameObject;
		base.transform.localPosition = Vector3.zero;
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		for (int j = 0; j < this.abilityList.Count; j++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.abilityList[j].gameObject);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(this.OnHoverAbilityButton));
		}
		UIEventListener uieventListener2 = UIEventListener.Get(this.lbNeigongStr.gameObject);
		uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnHoverNeigong));
		UIEventListener uieventListener3 = UIEventListener.Get(this.sprNext.gameObject);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnNextUnitButton));
		UIEventListener uieventListener4 = UIEventListener.Get(this.sprPrev.gameObject);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnPrevUnitButton));
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x0001F662 File Offset: 0x0001D862
	private void OnEnable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move += new Action<Vector2>(this.OnMove);
		}
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x0001F6A0 File Offset: 0x0001D8A0
	private void OnDisable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move -= new Action<Vector2>(this.OnMove);
		}
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x00184134 File Offset: 0x00182334
	public void OnStartButton()
	{
		if (UnitControl.GetPlayerUnitsRemainng() == 1 && UnitControl.IsFactionAllUnitPlaced())
		{
			GameControlTB.UnitPlacementCompleted();
		}
		else if (UnitControl.IsFactionAllUnitPlaced())
		{
			UnitControl.NextFactionPlacementID();
		}
		else
		{
			UINGUI.DisplayMessage(Game.StringTable.GetString(260041));
		}
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x00184188 File Offset: 0x00182388
	private void UpdateUnitInfo()
	{
		if (UnitControl.IsFactionAllUnitPlaced())
		{
			Utility.SetActive(this.controlObj, false);
			Utility.SetActive(this.buttonStart.gameObject, true);
		}
		else
		{
			List<UnitTB> unplacedUnit = UnitControl.GetUnplacedUnit();
			UnitTB unitTB = unplacedUnit[UnitControl.GetUnitPlacementID()];
			this.texUnitIcon.mainTexture = unitTB.iconTalk;
			this.lbUnitName.text = unitTB.unitName;
			this.lbUnitRemain.text = string.Format(Game.StringTable.GetString(263064), unplacedUnit.Count);
			this.lbHP.text = Game.StringTable.GetString(261001);
			this.lbHPVal.text = unitTB.fullHP.ToString();
			this.lbSP.text = Game.StringTable.GetString(261002);
			this.lbSPVal.text = unitTB.fullSP.ToString();
			this.lbNeigong.text = Game.StringTable.GetString(261015);
			if (unitTB.unitNeigong != null)
			{
				this.lbNeigongStr.text = unitTB.unitNeigong.m_strNeigongName;
			}
			else
			{
				this.lbNeigongStr.text = Game.StringTable.GetString(263041);
			}
			List<UnitAbility> unitAbilityList = unitTB.unitAbilityList;
			for (int i = 0; i < this.abilityList.Count; i++)
			{
				if (i < unitAbilityList.Count)
				{
					this.abilityList[i].text = unitAbilityList[i].name;
					Utility.SetActive(this.abilityList[i].gameObject, true);
				}
				else
				{
					Utility.SetActive(this.abilityList[i].gameObject, false);
				}
			}
			Utility.SetActive(this.controlObj, true);
			Utility.SetActive(this.buttonStart.gameObject, false);
		}
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x0001F6DE File Offset: 0x0001D8DE
	public void OnNextUnitButton(GameObject go)
	{
		UnitControl.NextUnitPlacementID();
		this.UpdateUnitInfo();
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x0001F6EB File Offset: 0x0001D8EB
	public void OnPrevUnitButton(GameObject go)
	{
		UnitControl.PrevUnitPlacementID();
		this.UpdateUnitInfo();
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x0001F6F8 File Offset: 0x0001D8F8
	public void OnAutoPlaceButton(GameObject go)
	{
		if (GridManager.GetAllPlaceableTiles().Count > 0)
		{
			UnitControl.AutoPlaceUnit();
		}
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x00184378 File Offset: 0x00182578
	private void MoveToPlaceableTile()
	{
		if (!GridManager.AllPlaceableTileOccupied())
		{
			int count = GridManager.GetAllPlaceableTiles().Count;
			if (this.iIndex >= count)
			{
				this.iIndex = 0;
			}
			if (count > 0)
			{
				CameraControl.instance.trackNowTile = GridManager.GetAllPlaceableTiles()[this.iIndex];
				this.nowCursorTile = GridManager.GetAllPlaceableTiles()[this.iIndex];
				GridManager.OnHoverEnter(this.nowCursorTile);
			}
			else
			{
				CameraControl.instance.trackNowTile = null;
			}
			CameraControl.instance.trackTile = true;
		}
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x0001F70F File Offset: 0x0001D90F
	public void SavePlaceTile()
	{
		this.placeList.Clear();
		this.placeList.AddRange(GridManager.GetAllPlaceableTiles());
	}

	// Token: 0x060031FF RID: 12799 RVA: 0x0001F72C File Offset: 0x0001D92C
	public void Show()
	{
		UINGUI.instance.DelayBattleControlState(BattleControlState.Placement);
		Utility.SetActive(this.thisObj, true);
		this.UpdateUnitInfo();
		this.iIndex = 0;
		this.MoveToPlaceableTile();
	}

	// Token: 0x06003200 RID: 12800 RVA: 0x0001F758 File Offset: 0x0001D958
	public void Hide()
	{
		this.nowCursorTile = null;
		Utility.SetActive(this.thisObj, false);
		CameraControl.instance.trackNowTile = null;
		CameraControl.instance.trackTile = false;
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnHoverAbilityButton(GameObject butObj, bool state)
	{
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnHoverNeigong(GameObject butObj, bool state)
	{
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x0001F783 File Offset: 0x0001D983
	private void KeyOk()
	{
		if (UnitControl.IsFactionAllUnitPlaced())
		{
			this.OnStartButton();
		}
		else if (this.nowCursorTile != null)
		{
			this.nowCursorTile.OnTouchMouseDown();
		}
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x0001F7B6 File Offset: 0x0001D9B6
	private void KeyCancel()
	{
		if (this.nowCursorTile != null && this.nowCursorTile.unit != null)
		{
			this.nowCursorTile.OnTouchMouseDown();
		}
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x0018440C File Offset: 0x0018260C
	private void MoveDirect(Vector2 direction)
	{
		if (direction.sqrMagnitude <= 0.1f)
		{
			return;
		}
		if (this.ftime + this.ftick > Time.time)
		{
			return;
		}
		this.ftime = Time.time;
		Tile directNearTileInGroup = GridManager.instance.GetDirectNearTileInGroup(this.placeList, this.nowCursorTile, direction);
		if (directNearTileInGroup != null)
		{
			this.nowCursorTile = directNearTileInGroup;
			CameraControl.instance.trackNowTile = this.nowCursorTile;
			GridManager.OnHoverEnter(this.nowCursorTile);
		}
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x00184494 File Offset: 0x00182694
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.Placement)
		{
			return;
		}
		switch (keyCode)
		{
		case KeyControl.Key.BattlePrevUnit:
			this.OnPrevUnitButton(null);
			break;
		case KeyControl.Key.BattleNextUnit:
			this.OnNextUnitButton(null);
			break;
		default:
			if (keyCode != KeyControl.Key.OK)
			{
				if (keyCode == KeyControl.Key.Cancel)
				{
					this.KeyCancel();
				}
			}
			else
			{
				this.KeyOk();
			}
			break;
		case KeyControl.Key.FindTile:
			this.iIndex++;
			this.MoveToPlaceableTile();
			break;
		case KeyControl.Key.PlaceAllUnit:
			this.OnAutoPlaceButton(null);
			break;
		}
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x0001F7EA File Offset: 0x0001D9EA
	public void OnMove(Vector2 direction)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.Placement)
		{
			return;
		}
		this.MoveDirect(direction);
	}

	// Token: 0x04003DA1 RID: 15777
	public UISprite buttonStart;

	// Token: 0x04003DA2 RID: 15778
	public GameObject controlObj;

	// Token: 0x04003DA3 RID: 15779
	public UITexture texUnitIcon;

	// Token: 0x04003DA4 RID: 15780
	public UILabel lbUnitName;

	// Token: 0x04003DA5 RID: 15781
	public UILabel lbHP;

	// Token: 0x04003DA6 RID: 15782
	public UILabel lbHPVal;

	// Token: 0x04003DA7 RID: 15783
	public UILabel lbSP;

	// Token: 0x04003DA8 RID: 15784
	public UILabel lbSPVal;

	// Token: 0x04003DA9 RID: 15785
	public UILabel lbAbility;

	// Token: 0x04003DAA RID: 15786
	public List<UILabel> abilityList = new List<UILabel>();

	// Token: 0x04003DAB RID: 15787
	public UILabel lbNeigong;

	// Token: 0x04003DAC RID: 15788
	public UILabel lbNeigongStr;

	// Token: 0x04003DAD RID: 15789
	public UILabel lbUnitRemain;

	// Token: 0x04003DAE RID: 15790
	public UISprite sprNext;

	// Token: 0x04003DAF RID: 15791
	public UISprite sprPrev;

	// Token: 0x04003DB0 RID: 15792
	private GameObject thisObj;

	// Token: 0x04003DB1 RID: 15793
	private int iIndex;

	// Token: 0x04003DB2 RID: 15794
	private Tile nowCursorTile;

	// Token: 0x04003DB3 RID: 15795
	private List<Tile> placeList = new List<Tile>();

	// Token: 0x04003DB4 RID: 15796
	private float ftime;

	// Token: 0x04003DB5 RID: 15797
	private float ftick = 0.2f;
}
