using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000740 RID: 1856
[RequireComponent(typeof(AIManager))]
[RequireComponent(typeof(AbilityManagerTB))]
[RequireComponent(typeof(UnitControl))]
public class GameControlTB : MonoBehaviour
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x06002BEC RID: 11244 RVA: 0x0001C562 File Offset: 0x0001A762
	// (remove) Token: 0x06002BED RID: 11245 RVA: 0x0001C579 File Offset: 0x0001A779
	public static event GameControlTB.BattleStartHandler onBattleStartE;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06002BEE RID: 11246 RVA: 0x0001C590 File Offset: 0x0001A790
	// (remove) Token: 0x06002BEF RID: 11247 RVA: 0x0001C5A7 File Offset: 0x0001A7A7
	public static event GameControlTB.BattleStartHandler onBattleStartRealE;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06002BF0 RID: 11248 RVA: 0x0001C5BE File Offset: 0x0001A7BE
	// (remove) Token: 0x06002BF1 RID: 11249 RVA: 0x0001C5D5 File Offset: 0x0001A7D5
	public static event GameControlTB.BattleEndHandler onBattleEndE;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06002BF2 RID: 11250 RVA: 0x0001C5EC File Offset: 0x0001A7EC
	// (remove) Token: 0x06002BF3 RID: 11251 RVA: 0x0001C603 File Offset: 0x0001A803
	public static event GameControlTB.BattleResetHandler onReset;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06002BF4 RID: 11252 RVA: 0x0001C61A File Offset: 0x0001A81A
	// (remove) Token: 0x06002BF5 RID: 11253 RVA: 0x0001C631 File Offset: 0x0001A831
	public static event GameControlTB.NextTurnHandler onUnitNextTurnE;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06002BF6 RID: 11254 RVA: 0x0001C648 File Offset: 0x0001A848
	// (remove) Token: 0x06002BF7 RID: 11255 RVA: 0x0001C65F File Offset: 0x0001A85F
	public static event GameControlTB.NextTurnHandler onNextTurnE;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06002BF8 RID: 11256 RVA: 0x0001C676 File Offset: 0x0001A876
	// (remove) Token: 0x06002BF9 RID: 11257 RVA: 0x0001C68D File Offset: 0x0001A88D
	public static event GameControlTB.NewRoundHandler onNewRoundE;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06002BFA RID: 11258 RVA: 0x0001C6A4 File Offset: 0x0001A8A4
	// (remove) Token: 0x06002BFB RID: 11259 RVA: 0x0001C6BB File Offset: 0x0001A8BB
	public static event GameControlTB.GameMessageHandler onGameMessageE;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06002BFC RID: 11260 RVA: 0x0001C6D2 File Offset: 0x0001A8D2
	// (remove) Token: 0x06002BFD RID: 11261 RVA: 0x0001C6E9 File Offset: 0x0001A8E9
	public static event GameControlTB.GameMessageHandler onBattleMessageE;

	// Token: 0x06002BFE RID: 11262 RVA: 0x0001C700 File Offset: 0x0001A900
	public static bool EnablePerkMenu()
	{
		return !(GameControlTB.instance == null) && GameControlTB.instance.enablePerkMenu;
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x0001C71E File Offset: 0x0001A91E
	public static bool EnableHeightTB()
	{
		return !(GameControlTB.instance == null) && GameControlTB.instance.enableHeight;
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x0001C73C File Offset: 0x0001A93C
	public static int GetMinimumHeightDif()
	{
		if (GameControlTB.instance == null)
		{
			return 10;
		}
		return GameControlTB.instance.minHeightDifference;
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x0001C75B File Offset: 0x0001A95B
	public static float GetH2LBonus()
	{
		if (GameControlTB.instance == null)
		{
			return 0f;
		}
		return GameControlTB.instance.highToLowHitBonus;
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x0001C77D File Offset: 0x0001A97D
	public static float GetL2HPenalty()
	{
		if (GameControlTB.instance == null)
		{
			return 0f;
		}
		return GameControlTB.instance.lowToHighHitPenalty;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x0001C79F File Offset: 0x0001A99F
	public static float GetHeightUnit()
	{
		if (GameControlTB.instance == null)
		{
			return 10f;
		}
		return GameControlTB.instance.heightUnit;
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x0001C7C1 File Offset: 0x0001A9C1
	public static int GetMoveCostH()
	{
		if (GameControlTB.instance == null)
		{
			return 0;
		}
		return GameControlTB.instance.moveCostPerHeight;
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x001559A4 File Offset: 0x00153BA4
	private void Awake()
	{
		if (GameControlTB.instance != null)
		{
			Debug.LogError("2 GameControlTB");
		}
		GameControlTB.instance = this;
		GameControlTB.turnID = -1;
		GameControlTB.turnIDLoop = 1;
		AbilityManagerTB.LoadBattleAbility2();
		GameControlTB.roundCounter = 0;
		GameControlTB.battleEnded = false;
		if (this.playerFactionID.Count == 0)
		{
			this.playerFactionID.Add(0);
		}
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x0001C7DF File Offset: 0x0001A9DF
	private void OnEnable()
	{
		UnitTB.onActionCompletedE += GameControlTB.OnActionCompleted;
		UnitTB.onTurnDepletedE += this.UnitActionDepleted;
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x0001C803 File Offset: 0x0001AA03
	private void OnDisable()
	{
		UnitTB.onActionCompletedE -= GameControlTB.OnActionCompleted;
		UnitTB.onTurnDepletedE -= this.UnitActionDepleted;
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x0001C827 File Offset: 0x0001AA27
	public static bool ActionCommenced()
	{
		if (GameControlTB.actionInProgress)
		{
			return false;
		}
		GameControlTB.actionInProgress = true;
		return true;
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x0001C83C File Offset: 0x0001AA3C
	public static void OnActionCompleted(UnitTB unit)
	{
		Debug.LogWarning(unit.unitName + "OnActionCompleted try actionInProgress = false");
		GameControlTB.instance.StartCoroutine(GameControlTB.instance._OnActionCompleted());
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x00155A0C File Offset: 0x00153C0C
	private IEnumerator _OnActionCompleted()
	{
		yield return null;
		GameControlTB.actionInProgress = false;
		Debug.LogWarning("_OnActionCompleted NextFrame actionInProgress = false");
		yield break;
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x00155A20 File Offset: 0x00153C20
	public void UnitActionDepleted()
	{
		Debug.Log("UnitActionDepleted");
		if (UnitControl.bTauntFlee)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"Round ",
				GameControlTB.roundCounter.ToString(),
				" ",
				base.name,
				" 不可控的動作結束"
			}));
			if (UnitControl.AllUnitInFactionMoved(GameControlTB.turnID))
			{
				Debug.LogWarning("Round " + GameControlTB.roundCounter.ToString() + " 所有角色都 不可控的動作結束");
			}
			return;
		}
		if (GameControlTB.playerFactionTurnID.Contains(GameControlTB.turnID))
		{
			if (UnitControl.AllUnitInFactionMoved(GameControlTB.turnID))
			{
				GameControlTB.OnEndTurn();
			}
			else
			{
				UnitControl.SwitchToNextUnit();
			}
		}
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x0001C868 File Offset: 0x0001AA68
	public static void OnEndTurn()
	{
		Debug.Log("OnEndTurn");
		GameControlTB.instance.StartCoroutine(GameControlTB.instance._OnEndTurn());
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x00155AE0 File Offset: 0x00153CE0
	private IEnumerator _OnEndTurn()
	{
		yield return null;
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		GameControlTB.unitSwitchingLocked = false;
		UnitControl.instance.CheckBattleEnd();
		if (UnitControl.instance.bBattleOver)
		{
			GameControlTB.BattleEnded(UnitControl.instance.iWinFaction);
		}
		else
		{
			GameControlTB.MoveToNextTurn();
		}
		yield break;
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x00155AF4 File Offset: 0x00153CF4
	public static void MoveToNextTurn()
	{
		GridManager.Deselect();
		GameControlTB.NextTurnID();
		if (GameControlTB.turnIDLoop > GameControlTB.totalFactionInGame)
		{
			GameControlTB.turnIDLoop = 1;
			GameControlTB.OnNewRound();
			return;
		}
		while (!UnitControl.IsFactionStillActive(GameControlTB.turnID))
		{
			GameControlTB.NextTurnID();
			if (GameControlTB.turnIDLoop > GameControlTB.totalFactionInGame)
			{
				GameControlTB.turnIDLoop = 1;
				GameControlTB.OnNewRound();
				return;
			}
		}
		GameControlTB.instance.StartCoroutine(GameControlTB.instance.OnNextTurn());
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x00155B70 File Offset: 0x00153D70
	public static void OnNewRound()
	{
		if (GameControlTB.roundCounter == 0)
		{
			GameControlTB.ResetTurnID();
		}
		GameControlTB.roundCounter++;
		GameControlTB.unitSwitchingLocked = false;
		if (GameControlTB.roundWin > 0 && GameControlTB.roundCounter > GameControlTB.roundWin)
		{
			GameControlTB.BattleEnded(0);
			return;
		}
		if (GameControlTB.roundLose > 0 && GameControlTB.roundCounter > GameControlTB.roundLose)
		{
			GameControlTB.BattleEnded(1);
			return;
		}
		GameControlTB.instance.StartCoroutine(GameControlTB.instance._OnNewRound());
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x00155BF8 File Offset: 0x00153DF8
	private IEnumerator _OnNewRound()
	{
		yield return null;
		if (GameControlTB.battleEnded)
		{
			yield break;
		}
		yield return new WaitForSeconds(this.newRoundCD * 0.25f);
		GameControlTB.actionInProgress = true;
		Debug.LogWarning("OnNewRound actionInProgress = true");
		Debug.LogWarning("Round " + GameControlTB.roundCounter);
		string msg = "進入第 " + GameControlTB.roundCounter.ToString() + " 回合";
		if (GameControlTB.onBattleMessageE != null)
		{
			GameControlTB.onBattleMessageE(msg);
		}
		if (GameControlTB.onNewRoundE != null)
		{
			GameControlTB.onNewRoundE(GameControlTB.roundCounter);
		}
		yield return new WaitForSeconds(this.newRoundCD * 0.85f);
		GameControlTB.actionInProgress = false;
		Debug.LogWarning("OnNewRound actionInProgress = false");
		base.StartCoroutine(this.OnNextTurn());
		yield return null;
		yield break;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x00155C14 File Offset: 0x00153E14
	private IEnumerator OnNextTurn()
	{
		yield return null;
		if (GameControlTB.onUnitNextTurnE != null)
		{
			GameControlTB.onUnitNextTurnE();
		}
		yield return null;
		if (GameControlTB.onNextTurnE != null)
		{
			GameControlTB.onNextTurnE();
		}
		yield break;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x0001C889 File Offset: 0x0001AA89
	public static void BattleEnded(int vicFactionID)
	{
		GameControlTB.battleEnded = true;
		if (GameControlTB.onBattleEndE != null)
		{
			GameControlTB.onBattleEndE(vicFactionID);
		}
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x0001C8A6 File Offset: 0x0001AAA6
	public static void BattleReset()
	{
		GameControlTB.battleEnded = true;
		if (GameControlTB.onReset != null)
		{
			GameControlTB.onReset();
		}
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x00155C28 File Offset: 0x00153E28
	public static void ResetTurnID()
	{
		int num = UnitControl.GetPlayerFactionTurnID();
		if (num >= 0)
		{
			GameControlTB.turnID = num;
		}
		else
		{
			GameControlTB.turnID = 0;
		}
		GameControlTB.turnIDLoop = 1;
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x0001C8C2 File Offset: 0x0001AAC2
	public static void NextTurnID()
	{
		GameControlTB.turnID++;
		GameControlTB.turnIDLoop++;
		if (GameControlTB.turnID >= GameControlTB.totalFactionInGame)
		{
			GameControlTB.turnID = 0;
		}
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x0001C8F1 File Offset: 0x0001AAF1
	public static bool IsPlayerTurn()
	{
		return GameControlTB.playerFactionExisted && GameControlTB.playerFactionTurnID.Contains(GameControlTB.turnID);
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x0001C914 File Offset: 0x0001AB14
	public static bool IsHotSeatMode()
	{
		return GameControlTB.instance.hotseat;
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x0001C920 File Offset: 0x0001AB20
	public static bool IsPlayerFaction(int ID)
	{
		return !(GameControlTB.instance == null) && GameControlTB.instance.playerFactionID.Contains(ID);
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x0001C94C File Offset: 0x0001AB4C
	public static List<int> GetPlayerFactionIDS()
	{
		return GameControlTB.instance.playerFactionID;
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x00155C5C File Offset: 0x00153E5C
	public static int GetCurrentPlayerFactionID()
	{
		for (int i = 0; i < GameControlTB.playerFactionTurnID.Count; i++)
		{
			if (GameControlTB.playerFactionTurnID[i] == GameControlTB.turnID)
			{
				return GameControlTB.instance.playerFactionID[i];
			}
		}
		return -1;
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x0001C958 File Offset: 0x0001AB58
	public static int GetPlayerFactionID()
	{
		if (GameControlTB.instance == null)
		{
			return 0;
		}
		return GameControlTB.instance.playerFactionID[0];
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x00155CAC File Offset: 0x00153EAC
	private void Update()
	{
		if (GameCursor.IsShow)
		{
			if (!GameControlTB.IsCursorOnUI(Input.mousePosition))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				LayerMask layerMask = 1 << LayerManager.GetLayerTile();
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, layerMask))
				{
					Tile component = raycastHit.collider.gameObject.GetComponent<Tile>();
					if (component == null)
					{
						if (this.lastTileTouched != null)
						{
							this.lastTileTouched.OnTouchMouseExit();
							this.lastTileTouched = null;
						}
					}
					else if (component == this.lastTileTouched)
					{
						this.lastTileTouched.OnTouchMouseOver();
					}
					else
					{
						if (this.lastTileTouched != null)
						{
							this.lastTileTouched.OnTouchMouseExit();
						}
						this.lastTileTouched = component;
						component.OnTouchMouseEnter();
						component.OnTouchMouseOver();
					}
				}
				else if (this.lastTileTouched != null)
				{
					this.lastTileTouched.OnTouchMouseExit();
					this.lastTileTouched = null;
				}
			}
			else if (this.lastTileTouched != null)
			{
				this.lastTileTouched.OnTouchMouseExit();
				this.lastTileTouched = null;
			}
		}
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x00155DF0 File Offset: 0x00153FF0
	public static bool IsCursorOnUI(Vector3 point)
	{
		if (!GameCursor.IsShow)
		{
			return false;
		}
		if (GameControlTB.instance.uiCam != null)
		{
			Ray ray = GameControlTB.instance.uiCam.ScreenPointToRay(point);
			LayerMask layerMask = 1 << LayerManager.GetLayerUI();
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, layerMask))
			{
				GameControlTB.instance.bOnUI = true;
				return true;
			}
		}
		else
		{
			Debug.LogError("uiCam 是空的 會造成戰鬥介面 按紐直接穿透，點選技能 會點到格子，造成原因 場景無 UI_NGUI 或 UI_NGUI 未隱藏");
		}
		GameControlTB.instance.bOnUI = false;
		return false;
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x00155E80 File Offset: 0x00154080
	public static bool IsObjectOnUI(Vector3 pos)
	{
		Camera main = Camera.main;
		if (GameControlTB.instance.uiCam != null && main != null)
		{
			Ray ray = GameControlTB.instance.uiCam.ScreenPointToRay(main.WorldToScreenPoint(pos));
			LayerMask layerMask = 1 << LayerManager.GetLayerUI();
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, layerMask))
			{
				return true;
			}
		}
		else
		{
			Debug.LogError("uiCam 或是 mainCam 是空的 會造成戰鬥介面 按紐直接穿透，點選技能 會點到格子，造成原因 場景無 UI_NGUI 或 UI_NGUI 未隱藏");
		}
		return false;
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x0001C97C File Offset: 0x0001AB7C
	public static void SetUICam(Camera cam)
	{
		if (GameControlTB.instance != null)
		{
			GameControlTB.instance.uiCam = cam;
		}
		else
		{
			Debug.LogError("無法設定 UI 的鏡頭，可能是 UI_NGUI 在 Sence 中 沒有隱藏");
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x0001C9A8 File Offset: 0x0001ABA8
	public static Tile GetLastTileTouched()
	{
		return GameControlTB.instance.lastTileTouched;
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
	public static void UnitPlacementCompleted()
	{
		if (GameControlTB.onBattleStartE != null)
		{
			GameControlTB.onBattleStartE();
		}
		GameControlTB.instance.StartCoroutine(GameControlTB.instance._BattleStartReal());
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x00155F08 File Offset: 0x00154108
	private IEnumerator _BattleStartReal()
	{
		yield return null;
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return null;
		if (GameControlTB.onBattleStartRealE != null)
		{
			GameControlTB.onBattleStartRealE();
		}
		yield break;
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x0001C9DF File Offset: 0x0001ABDF
	public static void DisplayMessage(string msg)
	{
		if (GameControlTB.onGameMessageE != null)
		{
			GameControlTB.onGameMessageE(msg);
		}
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x0001C9F6 File Offset: 0x0001ABF6
	public static void LoadNextScene()
	{
		Time.timeScale = 1f;
		if (GameControlTB.instance.nextScene != string.Empty)
		{
			Application.LoadLevel(GameControlTB.instance.nextScene);
		}
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x0001CA2A File Offset: 0x0001AC2A
	public static void LoadMainMenu()
	{
		Time.timeScale = 1f;
		if (GameControlTB.instance.mainMenu != string.Empty)
		{
			Application.LoadLevel(GameControlTB.instance.mainMenu);
		}
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x0001CA5E File Offset: 0x0001AC5E
	public static bool EnableUnitPlacement()
	{
		return GameControlTB.instance.enableUnitPlacement;
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x0001CA6A File Offset: 0x0001AC6A
	public static int UseItemCoolDown()
	{
		return GameControlTB.instance.useItemCoolDown;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x0001CA76 File Offset: 0x0001AC76
	public static float GetExposedCritBonus()
	{
		return GameControlTB.instance.exposedCritBonus;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x0001CA82 File Offset: 0x0001AC82
	public static bool EnableFogOfWar()
	{
		return !(GameControlTB.instance == null) && GameControlTB.instance.enableFogOfWar;
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
	public static bool IsCounterAttackEnabled()
	{
		return GameControlTB.instance.enableCounterAttack;
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x0001CAAC File Offset: 0x0001ACAC
	public static bool FullAPOnStart()
	{
		return GameControlTB.instance == null || GameControlTB.instance.fullAPOnStart;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x0001CACA File Offset: 0x0001ACCA
	public static float APOnStartPercent()
	{
		return GameControlTB.instance.fAPOnStart;
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x0001CAD6 File Offset: 0x0001ACD6
	public static bool FullAPOnNewRound()
	{
		return GameControlTB.instance.fullAPOnNewRound;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x0001CAE2 File Offset: 0x0001ACE2
	public static float APRestorePercent()
	{
		return GameControlTB.instance.fAPRestore;
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x0001CAEE File Offset: 0x0001ACEE
	public static _MovementAPCostRule MovementAPCostRule()
	{
		return GameControlTB.instance.movementAPCostRule;
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x0001CAFA File Offset: 0x0001ACFA
	public static _AttackAPCostRule AttackAPCostRule()
	{
		return GameControlTB.instance.attackAPCostRule;
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x0001CB06 File Offset: 0x0001AD06
	public static bool AllowMovementAfterAttack()
	{
		return GameControlTB.instance.allowMovementAfterAttack;
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x0001CB12 File Offset: 0x0001AD12
	public static bool AllowAbilityAfterAttack()
	{
		return GameControlTB.instance.allowAbilityAfterAttack;
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x0001CB1E File Offset: 0x0001AD1E
	public static _LoadMode LoadMode()
	{
		return GameControlTB.instance.loadMode;
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x0001CB2A File Offset: 0x0001AD2A
	public static bool AllowUnitSwitching()
	{
		return !GameControlTB.unitSwitchingLocked;
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x0000264F File Offset: 0x0000084F
	public static void LockUnitSwitching()
	{
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x0001CB34 File Offset: 0x0001AD34
	public static float GetActionCamFrequency()
	{
		return GameControlTB.instance.actionCamFrequency;
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x0001CB40 File Offset: 0x0001AD40
	public static bool IsActionInProgress()
	{
		return GameControlTB.actionInProgress || CameraControl.ActionCamInAction();
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x0001CB54 File Offset: 0x0001AD54
	public static bool IsUnitPlacementState()
	{
		return GameControlTB.turnID < 0;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x0001CB64 File Offset: 0x0001AD64
	public void SetWinRound(int iround)
	{
		GameControlTB.roundWin = iround;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x0001CB6C File Offset: 0x0001AD6C
	public int GetWinRound()
	{
		return GameControlTB.roundWin;
	}

	// Token: 0x06002C3C RID: 11324 RVA: 0x0001CB73 File Offset: 0x0001AD73
	public void SetLoseRound(int iround)
	{
		GameControlTB.roundLose = iround;
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x0001CB7B File Offset: 0x0001AD7B
	public int GetLoseRound()
	{
		return GameControlTB.roundLose;
	}

	// Token: 0x040038A4 RID: 14500
	public bool bOnUI;

	// Token: 0x040038A5 RID: 14501
	public static bool battleEnded = false;

	// Token: 0x040038A6 RID: 14502
	private static bool actionInProgress = false;

	// Token: 0x040038A7 RID: 14503
	public bool hotseat;

	// Token: 0x040038A8 RID: 14504
	public List<int> playerFactionID = new List<int>();

	// Token: 0x040038A9 RID: 14505
	public static List<int> playerFactionTurnID = new List<int>();

	// Token: 0x040038AA RID: 14506
	public List<int> _playerFactionTurnID = new List<int>();

	// Token: 0x040038AB RID: 14507
	public static bool playerFactionExisted = false;

	// Token: 0x040038AC RID: 14508
	public static int turnID = -1;

	// Token: 0x040038AD RID: 14509
	public static int turnIDLoop = 0;

	// Token: 0x040038AE RID: 14510
	public static int totalFactionInGame = 2;

	// Token: 0x040038AF RID: 14511
	public static int roundWin = 0;

	// Token: 0x040038B0 RID: 14512
	public static int roundLose = 0;

	// Token: 0x040038B1 RID: 14513
	public static int roundCounter = 0;

	// Token: 0x040038B2 RID: 14514
	private float newRoundCD = 1f;

	// Token: 0x040038B3 RID: 14515
	public int useItemCoolDown = 3;

	// Token: 0x040038B4 RID: 14516
	public _LoadMode loadMode = _LoadMode.UseCurrentData;

	// Token: 0x040038B5 RID: 14517
	public int winPointReward = 20;

	// Token: 0x040038B6 RID: 14518
	[HideInInspector]
	public int pointGain;

	// Token: 0x040038B7 RID: 14519
	public string nextScene = string.Empty;

	// Token: 0x040038B8 RID: 14520
	public string mainMenu = string.Empty;

	// Token: 0x040038B9 RID: 14521
	public bool enablePerkMenu = true;

	// Token: 0x040038BA RID: 14522
	public bool enableUnitPlacement = true;

	// Token: 0x040038BB RID: 14523
	public bool enableCounterAttack;

	// Token: 0x040038BC RID: 14524
	public bool fullAPOnStart = true;

	// Token: 0x040038BD RID: 14525
	public float fAPOnStart = 0.4f;

	// Token: 0x040038BE RID: 14526
	public bool fullAPOnNewRound;

	// Token: 0x040038BF RID: 14527
	public float fAPRestore = 0.05f;

	// Token: 0x040038C0 RID: 14528
	public _MovementAPCostRule movementAPCostRule;

	// Token: 0x040038C1 RID: 14529
	public _AttackAPCostRule attackAPCostRule;

	// Token: 0x040038C2 RID: 14530
	public float exposedCritBonus = 0.3f;

	// Token: 0x040038C3 RID: 14531
	public bool enableFogOfWar;

	// Token: 0x040038C4 RID: 14532
	public bool enableHeight;

	// Token: 0x040038C5 RID: 14533
	public int minHeightDifference;

	// Token: 0x040038C6 RID: 14534
	public float highToLowHitBonus = 0.2f;

	// Token: 0x040038C7 RID: 14535
	public float lowToHighHitPenalty = 0.2f;

	// Token: 0x040038C8 RID: 14536
	public float heightUnit = 1f;

	// Token: 0x040038C9 RID: 14537
	public int moveCostPerHeight = 1;

	// Token: 0x040038CA RID: 14538
	public bool allowMovementAfterAttack = true;

	// Token: 0x040038CB RID: 14539
	public bool allowAbilityAfterAttack;

	// Token: 0x040038CC RID: 14540
	public float actionCamFrequency = 0.5f;

	// Token: 0x040038CD RID: 14541
	private static bool unitSwitchingLocked = false;

	// Token: 0x040038CE RID: 14542
	public bool allowUnitSwitching = true;

	// Token: 0x040038CF RID: 14543
	public static GameControlTB instance;

	// Token: 0x040038D0 RID: 14544
	public Camera uiCam;

	// Token: 0x040038D1 RID: 14545
	private Tile lastTileTouched;

	// Token: 0x02000741 RID: 1857
	// (Invoke) Token: 0x06002C3F RID: 11327
	public delegate void BattleStartHandler();

	// Token: 0x02000742 RID: 1858
	// (Invoke) Token: 0x06002C43 RID: 11331
	public delegate void BattleEndHandler(int vicFactionID);

	// Token: 0x02000743 RID: 1859
	// (Invoke) Token: 0x06002C47 RID: 11335
	public delegate void BattleResetHandler();

	// Token: 0x02000744 RID: 1860
	// (Invoke) Token: 0x06002C4B RID: 11339
	public delegate void NextTurnHandler();

	// Token: 0x02000745 RID: 1861
	// (Invoke) Token: 0x06002C4F RID: 11343
	public delegate void NewRoundHandler(int round);

	// Token: 0x02000746 RID: 1862
	// (Invoke) Token: 0x06002C53 RID: 11347
	public delegate void GameMessageHandler(string msg);
}
