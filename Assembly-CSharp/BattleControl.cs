using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using JsonFx.Json;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class BattleControl : MonoBehaviour
{
	// Token: 0x06000524 RID: 1316 RVA: 0x0003C2B8 File Offset: 0x0003A4B8
	private void Awake()
	{
		BattleControl.instance = this;
		this.m_battleArea = new BattleArea();
		this.m_battleAbility = new BattleAbility();
		this.m_battleSchedule = new BattleSchedule();
		this.m_teamExploitsList = new List<BattleExploits>();
		this.m_enemyExploitsList = new List<BattleExploits>();
		this.m_aniMoveOffsetManager = new AnimationMoveOffestManager();
		this.m_aniMoveOffsetManager.Load();
		this.m_BattleTriggerClipList = new List<BattleTriggerClip>();
		this.m_ContinueClipList = new List<BattleTriggerClip>();
		if (BattleControl.g_BattleMapBundle == null)
		{
			BattleControl.g_BattleMapBundle = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/battlemap.pk");
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00005278 File Offset: 0x00003478
	public void LoadText()
	{
		this.m_battleArea.LoadText("BattleAreaData");
		this.m_battleAbility.LoadText("BattleCondition");
		this.m_battleSchedule.LoadText("BattleSchedule");
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x000052AA File Offset: 0x000034AA
	public void LoadDLCText()
	{
		this.m_battleArea.LoadText("DLC_BattleAreaData");
		this.m_battleAbility.LoadText("DLC_BattleCondition");
		this.m_battleSchedule.LoadText("DLC_BattleSchedule");
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x000052DC File Offset: 0x000034DC
	public void SetDLCLevelID(int LevelID)
	{
		this.iDLCLevelID = LevelID;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x000052E5 File Offset: 0x000034E5
	public int GetDLCLevelID()
	{
		return this.iDLCLevelID;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0003C358 File Offset: 0x0003A558
	public void EndBattle(int iVictoryFactionID)
	{
		this.iBattleEndVictoryFactionID = iVictoryFactionID;
		GameControlTB.onBattleStartE -= this.OnBattleStart;
		GameControlTB.onNewRoundE -= this.OnRoundTrigger;
		GameControlTB.onBattleEndE -= this.OnGameOver;
		GameControlTB.onReset -= this.OnResetGame;
		UnitControl.onFactionDefeatedE -= this.OnFactionDefeat;
		UnitTB.onUnitDestroyedE -= this.OnActorDestroy;
		UnitTB.onAttackS -= this.OnAttackStart;
		UnitTB.onAttackE -= this.OnAttackEnd;
		UnitTB.onDamage -= this.OnDamage;
		Tile.onSetUnit -= this.OnSetUnit;
		BattleAreaNode battleAreaNode = this.m_battleArea.GetBattleAreaNode(this.iLastBattleID);
		Object.Destroy(this.goRoot);
		Battle.BattleEnd();
		Game.g_InputManager.Pop();
		if (this.goPlayer != null)
		{
			this.goPlayer.SetActive(true);
			if (Camera.main.GetComponent<DepthOfFieldScatter>() != null)
			{
				Camera.main.GetComponent<DepthOfFieldScatter>().SetDoFSTarget(this.goPlayer.transform);
			}
		}
		if (this.bNeedReturnMainCamera && this.goMainCamera != null)
		{
			this.goMainCamera.transform.parent = null;
			this.goMainCamera.transform.localEulerAngles = this.vBackupCameraEulerAngle;
			if (this.goMainCamera.GetComponent<OrbitCam>() != null)
			{
				this.goMainCamera.GetComponent<OrbitCam>().enabled = this.bOrbitCamEnable;
			}
			Camera.main.cullingMask = this.lmBackupCameraMask;
			base.StartCoroutine(this.FadeIn());
		}
		if (GameGlobal.m_bWaitToBattle)
		{
			GameGlobal.m_bWaitToBattle = false;
		}
		if (GameGlobal.m_bDLCMode && TeamStatus.m_Instance.CheckDLCAllTeamMateHurt())
		{
			if (this.bOldInside != GameGlobal.m_bHouseInside)
			{
				GameGlobal.m_bHouseInside = this.bOldInside;
				MapData.m_instance.SetInOutSide();
			}
			Game.PlayBGMusicClip(this.aClip);
			this.aClip = null;
			GameGlobal.m_bBattle = false;
			if (Game.UI.Get<UIEnd>() == null)
			{
				Game.UI.CreateUI("cFormEnd");
			}
			if (Game.UI.Get<UIEnd>() != null)
			{
				Game.UI.Get<UIEnd>().PlayEnd(23, 0);
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			return;
		}
		bool flag;
		int iRewardID;
		if (iVictoryFactionID == 0)
		{
			TeamStatus.m_Instance.DLC_FinishLevel(this.iDLCLevelID);
			flag = Game.RewardData.CheckRewardHaveChangeScene(battleAreaNode.m_iRewardID);
			iRewardID = battleAreaNode.m_iRewardID;
			NpcRandomEvent.SetNpcListDoSomething();
		}
		else
		{
			MissionStatus.m_instance.LoadQuestList(BattleControl._ResetSaveDataNode.m_QuestList);
			Game.Variable.CopyDataFrom(BattleControl._ResetSaveDataNode.Variable);
			flag = Game.RewardData.CheckRewardHaveChangeScene(battleAreaNode.m_iFailResultID);
			iRewardID = battleAreaNode.m_iFailResultID;
			if (GameGlobal.m_bDLCMode)
			{
				NpcRandomEvent.SetNpcListDoSomething();
			}
		}
		if (this.bLoadNewScene && !flag)
		{
			Game.RewardData.DoRewardID(iRewardID, null);
			if (Game.IsLoading())
			{
				Debug.LogError("Battle Want to GoBack Old Scene " + this.strOldSceneName + " but Already Loading ");
			}
			else
			{
				if (this.bOldInside != GameGlobal.m_bHouseInside)
				{
					GameGlobal.m_bHouseInside = this.bOldInside;
				}
				Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Combine(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.BattleEndOnFinish));
				this.ChangeScenes(this.strOldSceneName);
			}
		}
		else
		{
			if (this.bOldInside != GameGlobal.m_bHouseInside)
			{
				GameGlobal.m_bHouseInside = this.bOldInside;
				MapData.m_instance.SetInOutSide();
			}
			Game.PlayBGMusicClip(this.aClip);
			this.aClip = null;
			GameGlobal.m_bBattle = false;
			Game.RewardData.DoRewardID(iRewardID, null);
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0003C750 File Offset: 0x0003A950
	private IEnumerator FadeIn()
	{
		ScreenOverlay SO = null;
		float fPos = 0f;
		float fTime = 0.5f;
		if (this.goMainCamera == null)
		{
			this.goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
		while (fPos < fTime)
		{
			if (this.goMainCamera == null)
			{
				break;
			}
			if (this.goMainCamera.GetComponent<ScreenOverlay>() == null)
			{
				break;
			}
			SO = this.goMainCamera.GetComponent<ScreenOverlay>();
			if (SO != null)
			{
				SO.intensity = fPos / fTime;
				SO.enabled = true;
			}
			fPos += Time.deltaTime;
			yield return null;
		}
		if (this.goMainCamera == null)
		{
			this.goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
		if (this.goMainCamera != null)
		{
			if (SO == null)
			{
				SO = this.goMainCamera.GetComponent<ScreenOverlay>();
			}
			SO.intensity = 1f;
			SO.enabled = false;
		}
		yield break;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0003C76C File Offset: 0x0003A96C
	private void ChangeScenes(string strScenesName)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Equals("cFormLoad"))
			{
				array[i].GetComponent<UILoad>().LoadStage(strScenesName);
				break;
			}
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x000052ED File Offset: 0x000034ED
	private void OnResetGame()
	{
		this.m_ContinueClipList.Clear();
		base.StartCoroutine(this._ResetGame());
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0003C7C4 File Offset: 0x0003A9C4
	private IEnumerator _ResetGame()
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return null;
		int iBattleID = this.iLastBattleID;
		GameControlTB.onBattleStartE -= this.OnBattleStart;
		GameControlTB.onNewRoundE -= this.OnRoundTrigger;
		GameControlTB.onBattleEndE -= this.OnGameOver;
		GameControlTB.onReset -= this.OnResetGame;
		UnitControl.onFactionDefeatedE -= this.OnFactionDefeat;
		UnitTB.onUnitDestroyedE -= this.OnActorDestroy;
		UnitTB.onAttackS -= this.OnAttackStart;
		UnitTB.onAttackE -= this.OnAttackEnd;
		UnitTB.onDamage -= this.OnDamage;
		Tile.onSetUnit -= this.OnSetUnit;
		GridManager.OnHoverExit();
		Object.Destroy(this.goRoot);
		Battle.BattleEnd();
		Game.g_InputManager.Pop();
		if (this.goPlayer != null)
		{
			this.goPlayer.SetActive(true);
			if (Camera.main.GetComponent<DepthOfFieldScatter>() != null)
			{
				Camera.main.GetComponent<DepthOfFieldScatter>().SetDoFSTarget(this.goPlayer.transform);
			}
		}
		if (this.bNeedReturnMainCamera && this.goMainCamera != null)
		{
			this.goMainCamera.transform.parent = null;
			this.goMainCamera.transform.localEulerAngles = this.vBackupCameraEulerAngle;
			if (this.goMainCamera.GetComponent<OrbitCam>() != null)
			{
				this.goMainCamera.GetComponent<OrbitCam>().enabled = this.bOrbitCamEnable;
			}
			Camera.main.cullingMask = this.lmBackupCameraMask;
			base.StartCoroutine(this.FadeIn());
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		yield return null;
		while (this.goRoot != null)
		{
			yield return null;
		}
		Save.m_Instance.LoadData(3, 0);
		yield return null;
		this.m_teamExploitsList.Clear();
		this.m_enemyExploitsList.Clear();
		this.m_iTeamTotalExploits = 0;
		this.m_iEnemyTotalExploits = 0;
		Debug.Log("重開 戰場 ID = " + iBattleID);
		BattleAreaNode ban = this.m_battleArea.GetBattleAreaNode(iBattleID);
		if (ban == null)
		{
			Debug.LogError("找不到戰場 編號 " + iBattleID.ToString() + " 請檢查 BattleArea.txt");
			yield break;
		}
		string strBattleMapText = this.ExtractTextFile(ban.m_sMapName);
		if (strBattleMapText == null)
		{
			Debug.Log("error load " + ban.m_sMapName);
			yield break;
		}
		JsonReader jr = new JsonReader(strBattleMapText);
		BattleField bf = jr.Deserialize<BattleField>();
		this.BuildBattleMap(bf, ban);
		Battle.BattleStart();
		Game.g_InputManager.Push(UINGUI.instance);
		if (Game.g_AudioBundle.Contains("audio/Map/" + ban.m_sSoundName))
		{
			Game.PlayBGMusicMapPath(ban.m_sSoundName);
		}
		else
		{
			Game.PlayBGMusicMapPath("Y000072");
		}
		if (this.goPlayer != null)
		{
			this.goPlayer.SetActive(false);
		}
		GameControlTB.onBattleEndE += this.OnGameOver;
		GameControlTB.onReset += this.OnResetGame;
		yield break;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0003C7E0 File Offset: 0x0003A9E0
	public void StartBattle(int iBattleID)
	{
		this.m_teamExploitsList.Clear();
		this.m_enemyExploitsList.Clear();
		this.m_iTeamTotalExploits = 0;
		this.m_iEnemyTotalExploits = 0;
		this.bSaveOldFre = false;
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		Debug.Log("戰場 ID = " + iBattleID);
		this.bLoadNewScene = false;
		GameGlobal.m_bBattle = true;
		this.iTacticPointNow = 0;
		this.iTacticItemCount = 0;
		Save.m_Instance.SaveData(3, 0, -1);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		if (array.Length != 1)
		{
			Debug.LogError("BattleControl StartBattle But " + array.Length.ToString() + " Player");
			foreach (GameObject gameObject in array)
			{
				Debug.LogError(gameObject.name);
			}
		}
		this.goPlayer = GameObject.FindGameObjectWithTag("Player");
		this.iLastBattleID = iBattleID;
		BattleAreaNode battleAreaNode = this.m_battleArea.GetBattleAreaNode(iBattleID);
		if (battleAreaNode == null)
		{
			Debug.LogError("找不到戰場 編號 " + iBattleID.ToString() + " 請檢查 BattleArea.txt");
			return;
		}
		this.bOldInside = GameGlobal.m_bHouseInside;
		if (battleAreaNode.m_iInside == 0)
		{
			GameGlobal.m_bHouseInside = false;
		}
		else
		{
			GameGlobal.m_bHouseInside = true;
		}
		int num = battleAreaNode.m_sMapName.LastIndexOf("_");
		string text = battleAreaNode.m_sMapName.Substring(0, num);
		if (text != Application.loadedLevelName)
		{
			if (Game.IsLoading())
			{
				Debug.LogError("Battle want go to " + text + " but Already loading");
				return;
			}
			if (this.bLoadFromAutoSave)
			{
				this.bLoadNewScene = false;
			}
			else
			{
				this.bLoadNewScene = true;
				this.strOldSceneName = Application.loadedLevelName;
				if (GameGlobal.m_bUIDevelop)
				{
					this.strOldSceneName = "Develop";
				}
				if (GameGlobal.m_bUIStart)
				{
					this.strOldSceneName = "GameStart";
				}
				if (GameGlobal.m_bUIReadyCombat)
				{
					this.strOldSceneName = "ReadyCombat";
				}
			}
			this.iLoadLevelID = iBattleID;
			this.DoLoadLevel(text);
			return;
		}
		else
		{
			if (this.bOldInside != GameGlobal.m_bHouseInside)
			{
				MapData.m_instance.SetInOutSide();
			}
			string text2 = this.ExtractTextFile(battleAreaNode.m_sMapName);
			if (text2 == null)
			{
				Debug.Log("error load " + battleAreaNode.m_sMapName);
				return;
			}
			JsonReader jsonReader = new JsonReader(text2);
			BattleField bf = jsonReader.Deserialize<BattleField>();
			if (Camera.main != null)
			{
				this.lmBackupCameraMask = Camera.main.cullingMask;
			}
			this.BuildBattleMap(bf, battleAreaNode);
			if (this.goMainCamera != null && this.goMainCamera.audio != null)
			{
				this.aClip = this.goMainCamera.audio.clip;
			}
			Battle.BattleStart();
			Game.g_InputManager.Push(UINGUI.instance);
			if (Game.g_AudioBundle.Contains("audio/Map/" + battleAreaNode.m_sSoundName))
			{
				Game.PlayBGMusicMapPath(battleAreaNode.m_sSoundName);
			}
			else
			{
				Game.PlayBGMusicMapPath("Y000072");
			}
			if (this.goPlayer != null)
			{
				this.goPlayer.SetActive(false);
			}
			GameControlTB.onBattleEndE += this.OnGameOver;
			GameControlTB.onReset += this.OnResetGame;
			return;
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0003CB4C File Offset: 0x0003AD4C
	private string ExtractTextFile(string fileName)
	{
		string text = string.Concat(new string[]
		{
			Game.g_strDataPathToApplicationPath,
			"Mods/",
			GameGlobal.m_strVersion,
			"/Config/battlemap/",
			fileName,
			".map"
		});
		if (File.Exists(text))
		{
			try
			{
				Stream stream = File.OpenRead(text);
				StreamReader streamReader;
				try
				{
					streamReader = new StreamReader(stream, Encoding.Unicode);
				}
				catch (Exception ex)
				{
					Debug.LogError(fileName + "Exception : " + ex.Message);
					return null;
				}
				string result = string.Empty;
				result = streamReader.ReadToEnd();
				if (GameGlobal.m_iModFixFileCount <= 0)
				{
					Debug.Log("Mod active");
				}
				GameGlobal.m_iModFixFileCount++;
				return result;
			}
			catch (Exception ex2)
			{
				Debug.LogError("散檔讀取失敗 !! ( " + text + " )");
				return null;
			}
		}
		if (BattleControl.g_BattleMapBundle == null)
		{
			Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 未包檔");
			return null;
		}
		if (!BattleControl.g_BattleMapBundle.Contains(fileName))
		{
			Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 不存在");
			return null;
		}
		TextAsset textAsset = BattleControl.g_BattleMapBundle.Load(fileName) as TextAsset;
		if (textAsset != null)
		{
			return textAsset.text;
		}
		Debug.LogError("包檔讀取失敗 !! ( " + fileName + " )");
		return null;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00005307 File Offset: 0x00003507
	private void DoLoadLevel(string str)
	{
		this.strLoadLevelName = str;
		Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Combine(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.LoadLevelOnFinish));
		this.ChangeScenes(str);
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0003CCE4 File Offset: 0x0003AEE4
	private IEnumerator LoadNewLevelFinish()
	{
		yield return new WaitForSeconds(0.5f);
		if (this.strLoadLevelName == Application.loadedLevelName)
		{
			this.LoadEnd();
		}
		else
		{
			base.StartCoroutine(this.LoadNewLevelFinish());
		}
		yield break;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00005337 File Offset: 0x00003537
	public void BattleEndOnFinish()
	{
		Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Remove(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.BattleEndOnFinish));
		this.BattleEndLoadEnd();
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0000535F File Offset: 0x0000355F
	private void BattleEndLoadEnd()
	{
		GameGlobal.m_bBattle = false;
		if (GameGlobal.m_bDLCMode && this.strOldSceneName == "ReadyCombat")
		{
			Save.m_Instance.AutoSave();
		}
		this.strOldSceneName = string.Empty;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0000539B File Offset: 0x0000359B
	public void LoadLevelOnFinish()
	{
		Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Remove(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.LoadLevelOnFinish));
		this.LoadEnd();
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0003CD00 File Offset: 0x0003AF00
	private void LoadEnd()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		if (array.Length != 1)
		{
			Debug.LogError("BattleControl LoadEnd But " + array.Length.ToString() + " Player");
			foreach (GameObject gameObject in array)
			{
				Debug.LogError(gameObject.name);
			}
		}
		this.goPlayer = GameObject.FindGameObjectWithTag("Player");
		this.iLastBattleID = this.iLoadLevelID;
		BattleAreaNode battleAreaNode = this.m_battleArea.GetBattleAreaNode(this.iLoadLevelID);
		if (battleAreaNode == null)
		{
			Debug.LogError("沒有戰場 ID " + this.iLoadLevelID.ToString() + " 請查 BattleArea.txt ");
			return;
		}
		if (this.bLoadFromAutoSave)
		{
			Game.instance.HideLoadingPercentage();
		}
		string text = this.ExtractTextFile(battleAreaNode.m_sMapName);
		if (text == null)
		{
			Debug.Log("error load " + battleAreaNode.m_sMapName);
			return;
		}
		JsonReader jsonReader = new JsonReader(text);
		BattleField bf = jsonReader.Deserialize<BattleField>();
		if (Camera.main != null)
		{
			this.lmBackupCameraMask = Camera.main.cullingMask;
		}
		this.BuildBattleMap(bf, battleAreaNode);
		Battle.BattleStart();
		Game.g_InputManager.Push(UINGUI.instance);
		if (Game.g_AudioBundle.Contains("audio/Map/" + battleAreaNode.m_sSoundName))
		{
			Game.PlayBGMusicMapPath(battleAreaNode.m_sSoundName);
		}
		else
		{
			Game.PlayBGMusicMapPath("Y000072");
		}
		if (this.goPlayer != null)
		{
			this.goPlayer.SetActive(false);
		}
		GameControlTB.onBattleEndE += this.OnGameOver;
		GameControlTB.onReset += this.OnResetGame;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0003CED0 File Offset: 0x0003B0D0
	private void OnGameOver(int vicFactionID)
	{
		Debug.Log("OnGameOver " + vicFactionID.ToString());
		if (vicFactionID == 0)
		{
			Game.PlayBGMusicMapPath("Y000073", false);
			foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
			{
				if (battleTriggerClip.m_bStartClip)
				{
					if (battleTriggerClip.m_TriggerType == _BattleTriggerType.BattleWin)
					{
						if (this.CheckRequest(battleTriggerClip, null, null))
						{
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
		else
		{
			Game.PlayBGMusicMapPath("Y000074", false);
			foreach (BattleTriggerClip battleTriggerClip2 in this.m_BattleTriggerClipList)
			{
				if (battleTriggerClip2.m_bStartClip)
				{
					if (battleTriggerClip2.m_TriggerType == _BattleTriggerType.BattleLose)
					{
						if (this.CheckRequest(battleTriggerClip2, null, null))
						{
							this.DoSchedule(battleTriggerClip2, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0003D00C File Offset: 0x0003B20C
	public UnitTB GenerateBattleUnit(int iUnitID, CharacterData dlcCharacterData = null)
	{
		CharacterData characterData;
		if (dlcCharacterData != null)
		{
			characterData = dlcCharacterData;
		}
		else
		{
			characterData = NPC.m_instance.GetCharacterData(iUnitID);
		}
		if (characterData == null)
		{
			Debug.LogError("沒有角色 ID " + iUnitID.ToString() + " 請查 CharacterData.txt ");
			return null;
		}
		GameObject gameObject = Game.g_ModelBundle.Load(characterData._NpcDataNode.m_str3DModel + "_ModelPrefab") as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("ModelBundle 沒有  " + characterData._NpcDataNode.m_str3DModel + " Model 請查有無打包入 Model 檔");
			return null;
		}
		GameObject gameObject2 = (GameObject)Object.Instantiate(gameObject);
		MeleeWeaponTrail[] array = gameObject2.GetComponents<MeleeWeaponTrail>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Emit = false;
		}
		array = gameObject2.GetComponentsInChildren<MeleeWeaponTrail>();
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Emit = false;
		}
		GameObject gameObject3 = new GameObject(characterData._NpcDataNode.m_str3DModel);
		gameObject2.name = characterData._NpcDataNode.m_str3DModel;
		gameObject2.tag = "SpawnHero";
		gameObject2.transform.parent = gameObject3.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		gameObject3.transform.parent = this.tUnits;
		gameObject3.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		if (gameObject2.GetComponent<NpcCollider>())
		{
			gameObject2.GetComponent<NpcCollider>().m_strModelName = characterData._NpcDataNode.m_str3DModel;
			gameObject2.GetComponent<NpcCollider>().enabled = false;
		}
		if (gameObject2.GetComponent<PlayerController>())
		{
			gameObject2.GetComponent<PlayerController>().m_strModelName = characterData._NpcDataNode.m_str3DModel;
			gameObject2.GetComponent<PlayerController>().enabled = false;
		}
		if (gameObject2.GetComponent<NavMeshAgent>())
		{
			gameObject2.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (gameObject2.GetComponent<AudioListener>())
		{
			gameObject2.GetComponent<AudioListener>().enabled = false;
		}
		if (gameObject2.GetComponent<BoxCollider>())
		{
			gameObject2.GetComponent<BoxCollider>().enabled = false;
		}
		UnitTBAudio unitTBAudio = gameObject3.AddComponent<UnitTBAudio>();
		gameObject3.AddComponent<UnitTBAnimation>().SetAnimation(gameObject2);
		UnitTB unitTB = gameObject3.AddComponent<UnitTB>();
		unitTB.characterData = characterData;
		unitTB.fullHP = characterData._TotalProperty.Get(CharacterData.PropertyType.MaxHP);
		unitTB.fullSP = characterData._TotalProperty.Get(CharacterData.PropertyType.MaxSP);
		unitTB.fCriticalRate = (float)characterData._TotalProperty.Get(CharacterData.PropertyType.Critical) * 0.01f;
		unitTB.fDefCriticalRate = (float)characterData._TotalProperty.Get(CharacterData.PropertyType.DefendCritical) * 0.01f;
		unitTB.fCounterRate = (float)characterData._TotalProperty.Get(CharacterData.PropertyType.Counter) * 0.01f;
		unitTB.fDefCounterRate = (float)characterData._TotalProperty.Get(CharacterData.PropertyType.DefendCounter) * 0.01f;
		unitTB.fHitRate = 1f + (float)characterData._TotalProperty.Get(CharacterData.PropertyType.DefendDodge) * 0.01f;
		unitTB.fDodgeRate = (float)characterData._TotalProperty.Get(CharacterData.PropertyType.Dodge) * 0.01f;
		unitTB.movementRange = characterData._TotalProperty.Get(CharacterData.PropertyType.MoveStep);
		NpcNeigong nowUseNeigong = characterData.GetNowUseNeigong();
		if (nowUseNeigong == null)
		{
			unitTB.iNeigongLv = 0;
			unitTB.unitNeigong = null;
		}
		else
		{
			unitTB.iNeigongLv = nowUseNeigong.iLevel;
			unitTB.unitNeigong = nowUseNeigong.m_Neigong;
		}
		unitTB.unitName = characterData._NpcDataNode.m_strNpcName;
		unitTB.iconName = characterData._NpcDataNode.m_strSmallImage;
		unitTB.iUnitID = characterData.iNpcID;
		string name = "2dtexture/gameui/hexhead/" + unitTB.iconName + "_2";
		if (Game.g_HexHeadBundle.Contains(name))
		{
			unitTB.icon = (Game.g_HexHeadBundle.Load(name) as Texture2D);
		}
		else
		{
			name = "2dtexture/gameui/hexhead/" + unitTB.iconName;
			if (Game.g_HexHeadBundle.Contains(name))
			{
				unitTB.icon = (Game.g_HexHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				name = "2dtexture/gameui/bighead/B000001";
				if (Game.g_BigHeadBundle.Contains(name))
				{
					unitTB.icon = (Game.g_BigHeadBundle.Load(name) as Texture2D);
				}
				else
				{
					unitTB.icon = null;
				}
			}
		}
		name = "2dtexture/gameui/bighead/" + characterData._NpcDataNode.m_strBigHeadImage + "_2";
		if (Game.g_BigHeadBundle.Contains(name))
		{
			unitTB.iconTalk = (Game.g_BigHeadBundle.Load(name) as Texture2D);
		}
		else
		{
			name = "2dtexture/gameui/bighead/" + characterData._NpcDataNode.m_strBigHeadImage;
			if (Game.g_BigHeadBundle.Contains(name))
			{
				unitTB.iconTalk = (Game.g_BigHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				name = "2dtexture/gameui/bighead/B000001";
				if (Game.g_BigHeadBundle.Contains(name))
				{
					unitTB.iconTalk = (Game.g_BigHeadBundle.Load(name) as Texture2D);
				}
				else
				{
					unitTB.iconTalk = null;
				}
			}
		}
		unitTB.triggered = true;
		for (int k = 0; k < characterData.RoutineList.Count; k++)
		{
			if (characterData.RoutineList[k].bUse)
			{
				unitTB.abilityIDList.Add(characterData.RoutineList[k].m_Routine.m_iRoutineID);
			}
		}
		for (int l = 0; l < characterData.sVoicList.Count; l++)
		{
			if (Game.g_AudioBundle.Contains("audio/character/" + characterData.sVoicList[l]))
			{
				unitTBAudio.AddSelectSound(Game.g_AudioBundle.Load("audio/character/" + characterData.sVoicList[l]) as AudioClip);
			}
		}
		return unitTB;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0003D5EC File Offset: 0x0003B7EC
	private void BuildBattleMap(BattleField bf, BattleAreaNode ban)
	{
		if (GameGlobal.m_bDLCMode && ban.m_iTime > 0)
		{
			MapData.m_instance.SetTime((float)ban.m_iTime);
		}
		if (this.goRoot != null)
		{
			Object.Destroy(this.goRoot);
		}
		if (this.allTiles != null)
		{
			this.allTiles.Clear();
		}
		else
		{
			this.allTiles = new List<Tile>();
		}
		this.m_ContinueClipList.Clear();
		this.m_BattleTriggerClipList.Clear();
		this.m_BattleTriggerClipList.AddRange(this.m_battleSchedule.GetAreaScheduleList(ban.m_iID));
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			battleTriggerClip.m_bUsed = false;
		}
		this.goRoot = new GameObject("BattleField");
		this.goRoot.transform.localPosition = bf.vPos;
		this.goRoot.transform.localEulerAngles = bf.vEulerAngle;
		this.tUnits = new GameObject
		{
			transform = 
			{
				parent = this.goRoot.transform,
				localPosition = bf.vGMPos,
				localEulerAngles = bf.vGMEulerAngle,
				name = "Units"
			}
		}.transform;
		this.tGridManager = new GameObject
		{
			transform = 
			{
				parent = this.goRoot.transform,
				localPosition = bf.vGMPos,
				localEulerAngles = bf.vGMEulerAngle,
				name = "GridManager"
			}
		}.transform;
		this.tGameControl = new GameObject
		{
			transform = 
			{
				parent = this.goRoot.transform,
				localPosition = new Vector3(0f, 0f, 0f),
				name = "GameControl"
			}
		}.transform;
		GameObject gameObject = Resources.Load("ScenePrefab/HexTileProjectorBattle", typeof(GameObject)) as GameObject;
		this.goRoot.transform.position = bf.vPos;
		int num = bf.TileArray.Length;
		float num2 = -1000000f;
		float num3 = 1000000f;
		float num4 = -1000000f;
		float num5 = 1000000f;
		List<Tile> list = new List<Tile>();
		List<Tile> list2 = new List<Tile>();
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject2 = (GameObject)Object.Instantiate(gameObject);
			gameObject2.name = bf.TileArray[i].strName;
			gameObject2.transform.localScale *= 2.3256f;
			gameObject2.transform.parent = this.tGridManager;
			gameObject2.transform.localPosition = bf.TileArray[i].vPos;
			if (bf.TileArray[i].bWalkable)
			{
				Vector3 vector = gameObject2.transform.position - bf.vPos;
				if (vector.x > num2)
				{
					num2 = vector.x;
				}
				if (vector.x < num3)
				{
					num3 = vector.x;
				}
				if (vector.z > num4)
				{
					num4 = vector.z;
				}
				if (vector.z < num5)
				{
					num5 = vector.z;
				}
			}
			gameObject2.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
			Tile component = gameObject2.GetComponent<Tile>();
			component.walkable = bf.TileArray[i].bWalkable;
			component.invisible = bf.TileArray[i].bInvisible;
			component.placementID = bf.TileArray[i].iPlacementID;
			if (component.placementID == 0)
			{
				list.Add(component);
				component.openForPlacement = true;
				Debug.Log(gameObject2.name + " placementID = " + component.placementID.ToString());
			}
			this.allTiles.Add(component);
		}
		GridManager gridManager = this.tGridManager.gameObject.AddComponent<GridManager>();
		gridManager.gridSize = 2f;
		gridManager.playerPlacementAreas.Add(default(Rect));
		List<UnitTB> list3 = new List<UnitTB>();
		List<CharacterData> teamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
		for (int j = 0; j < teamMemberList.Count; j++)
		{
			if (teamMemberList[j].iHurtTurn <= 0)
			{
				UnitTB unitTB = this.GenerateBattleUnit(teamMemberList[j].iNpcID, null);
				list3.Add(unitTB);
			}
		}
		List<DLCUnitInfo> dlcunitList = TeamStatus.m_Instance.GetDLCUnitList();
		for (int k = 0; k < dlcunitList.Count; k++)
		{
			DLCUnitInfo dlcunitInfo = dlcunitList[k];
			UnitTB unitTB2 = this.GenerateBattleUnit(dlcunitInfo.ID, dlcunitInfo.Data);
			unitTB2.dlcCharGuid = dlcunitInfo.GID;
			list3.Add(unitTB2);
		}
		if (ban.m_iID == 88000001)
		{
			ban.m_TeamList = this.mod_NewBattleAddTeamUnit();
		}
		else if (ban.m_iID == 88000002)
		{
			ban.m_TeamList = this.mod_BossBattleAddTeamUnit();
		}
		else if (ban.m_iID == 88000003)
		{
			ban.m_TeamList = this.mod_RandomBattleAddTeamUnit();
		}
		else if (ban.m_iID == 88000004)
		{
			ban.m_TeamList = this.mod_ShilianBattleAddTeamUnit();
		}
		foreach (BattleJoinCharacterNode battleJoinCharacterNode in ban.m_TeamList)
		{
			if (battleJoinCharacterNode.m_iTurn == 0)
			{
				bool flag = false;
				UnitTB unitTB3 = null;
				if (battleJoinCharacterNode.m_iCharID == 0)
				{
					Tile tile;
					if (battleJoinCharacterNode.m_iTile == 0)
					{
						tile = gridManager.GetFactionTile(0);
						list.Remove(tile);
					}
					else
					{
						string strTileName = "Tile" + battleJoinCharacterNode.m_iTile.ToString();
						tile = gridManager.GetNoUnitOnTileByName(strTileName);
					}
					if (tile.unit == null)
					{
						tile.bUnitOrder = true;
						tile.openForPlacement = false;
						tile.placementID = 0;
						list2.Add(tile);
					}
				}
				else
				{
					for (int l = 0; l < list3.Count; l++)
					{
						if (!list3[l].characterData.bCaptive && list3[l].iUnitID == battleJoinCharacterNode.m_iCharID)
						{
							flag = true;
							unitTB3 = list3[l];
							break;
						}
					}
					if (!flag)
					{
						unitTB3 = this.GenerateBattleUnit(battleJoinCharacterNode.m_iCharID, null);
						if (unitTB3 == null)
						{
							Debug.LogError("戰鬥角色產生失敗 ID = " + battleJoinCharacterNode.m_iCharID.ToString());
							continue;
						}
					}
					else
					{
						list3.Remove(unitTB3);
					}
					unitTB3.factionID = 0;
					Tile tile2;
					if (battleJoinCharacterNode.m_iTile == 0)
					{
						tile2 = gridManager.GetFactionTile(0);
					}
					else
					{
						string strTileName2 = "Tile" + battleJoinCharacterNode.m_iTile.ToString();
						tile2 = gridManager.GetNoUnitOnTileByName(strTileName2);
					}
					if (tile2 == null)
					{
						Debug.LogError("找不到空的Tile 放不下 ");
					}
					else if (tile2.unit != null)
					{
						Debug.LogError(tile2.name + " Already have unit");
					}
					else
					{
						if (list.Contains(tile2))
						{
							tile2.openForPlacement = false;
							list.Remove(tile2);
						}
						if (list2.Contains(tile2))
						{
							tile2.bUnitOrder = false;
							list2.Remove(tile2);
						}
						unitTB3.gameObject.transform.position = tile2.transform.position;
						tile2.SetUnit(unitTB3);
						unitTB3.occupiedTile = tile2;
						Debug.Log("Team " + unitTB3.unitName + " place on " + tile2.name);
					}
				}
			}
		}
		if (ban.m_iID == 88000001)
		{
			ban.m_EnemyList = this.mod_NewBattleAddEnemyUnit();
		}
		else if (ban.m_iID == 88000002)
		{
			ban.m_EnemyList = this.mod_BossBattleAddEnemyUnit();
		}
		else if (ban.m_iID == 88000003)
		{
			ban.m_EnemyList = this.mod_RandomBattleAddEnemyUnit();
		}
		else if (ban.m_iID == 88000004)
		{
			ban.m_EnemyList = this.mod_ShilianBattleAddEnemyUnit();
		}
		foreach (BattleJoinCharacterNode battleJoinCharacterNode2 in ban.m_EnemyList)
		{
			if (battleJoinCharacterNode2.m_iTurn == 0)
			{
				bool flag2 = false;
				UnitTB unitTB4 = null;
				for (int m = 0; m < list3.Count; m++)
				{
					if (!list3[m].characterData.bCaptive && list3[m].iUnitID == battleJoinCharacterNode2.m_iCharID)
					{
						flag2 = true;
						unitTB4 = list3[m];
						break;
					}
				}
				if (!flag2)
				{
					unitTB4 = this.GenerateBattleUnit(battleJoinCharacterNode2.m_iCharID, null);
					if (unitTB4 == null)
					{
						Debug.LogError("戰鬥角色產生失敗 ID = " + battleJoinCharacterNode2.m_iCharID.ToString());
						continue;
					}
				}
				else
				{
					list3.Remove(unitTB4);
				}
				unitTB4.factionID = battleJoinCharacterNode2.m_iFaction;
				if (ban.m_iID == 88000004)
				{
					int factor = (GameGlobal.mod_Difficulty + 1) * (GameGlobal.mod_ShilianEnemyCount + 1) * (GameGlobal.mod_ShilianLayer + 1);
					this.mod_ShilianBattleParam(factor, unitTB4);
				}
				Tile tile3;
				if (battleJoinCharacterNode2.m_iTile == 0)
				{
					tile3 = gridManager.GetFactionTile(unitTB4.factionID);
				}
				else
				{
					string strTileName3 = "Tile" + battleJoinCharacterNode2.m_iTile.ToString();
					tile3 = gridManager.GetNoUnitOnTileByName(strTileName3);
				}
				if (tile3 == null)
				{
					Debug.LogError("找不到空的Tile 放不下 ");
				}
				else if (tile3.unit != null)
				{
					Debug.LogError(tile3.name + " Already have unit");
				}
				else
				{
					if (list.Contains(tile3))
					{
						tile3.openForPlacement = false;
						list.Remove(tile3);
					}
					if (list2.Contains(tile3))
					{
						tile3.bUnitOrder = false;
						list2.Remove(tile3);
					}
					unitTB4.gameObject.transform.position = tile3.transform.position;
					tile3.SetUnit(unitTB4);
					unitTB4.occupiedTile = tile3;
					Debug.Log(string.Concat(new string[]
					{
						"Enemy ",
						unitTB4.unitName,
						" place on ",
						tile3.name,
						" faction ",
						unitTB4.factionID.ToString()
					}));
				}
			}
		}
		UnitTB.onUnitDestroyedE += this.OnActorDestroy;
		UnitTB.onAttackS += this.OnAttackStart;
		UnitTB.onAttackE += this.OnAttackEnd;
		UnitTB.onDamage += this.OnDamage;
		Tile.onSetUnit += this.OnSetUnit;
		GameControlTB gameControlTB = this.tGameControl.gameObject.AddComponent<GameControlTB>();
		gameControlTB.actionCamFrequency = 0f;
		gameControlTB.enableCounterAttack = true;
		GameControlTB.onBattleStartE += this.OnBattleStart;
		GameControlTB.onNewRoundE += this.OnRoundTrigger;
		UnitControl.onFactionDefeatedE += this.OnFactionDefeat;
		if (AIManager.instance != null)
		{
			AIManager.instance.aIStance = _AIStance.Trigger;
		}
		foreach (BattleJoinCharacterNode battleJoinCharacterNode3 in ban.m_TeamList)
		{
			if (battleJoinCharacterNode3.m_iTurn != 0 && !(UnitControl.instance == null))
			{
				if (battleJoinCharacterNode3.m_iTurn > 0)
				{
					UnitControl.instance.AddJoinTeamNode(battleJoinCharacterNode3);
				}
				bool flag3 = false;
				UnitTB unitTB5 = null;
				for (int n = 0; n < list3.Count; n++)
				{
					if (!list3[n].characterData.bCaptive && list3[n].iUnitID == battleJoinCharacterNode3.m_iCharID)
					{
						flag3 = true;
						unitTB5 = list3[n];
						break;
					}
				}
				if (flag3)
				{
					list3.Remove(unitTB5);
				}
				if (battleJoinCharacterNode3.m_iTurn < 0)
				{
					Object.DestroyImmediate(unitTB5);
				}
			}
		}
		foreach (BattleJoinCharacterNode battleJoinCharacterNode4 in ban.m_EnemyList)
		{
			if (battleJoinCharacterNode4.m_iTurn != 0 && !(UnitControl.instance == null))
			{
				UnitControl.instance.AddJoinNode(battleJoinCharacterNode4);
				bool flag4 = false;
				UnitTB unitTB6 = null;
				for (int num6 = 0; num6 < list3.Count; num6++)
				{
					if (!list3[num6].characterData.bCaptive && list3[num6].iUnitID == battleJoinCharacterNode4.m_iCharID)
					{
						flag4 = true;
						unitTB6 = list3[num6];
						break;
					}
				}
				if (flag4)
				{
					list3.Remove(unitTB6);
				}
			}
		}
		if (list2.Count > 0)
		{
			if (UnitControl.instance.playerUnitsList == null || UnitControl.instance.playerUnitsList.Count == 0)
			{
				UnitControl.instance.playerUnitsList = new List<PlayerUnits>();
				UnitControl.instance.playerUnitsList.Add(new PlayerUnits(GameControlTB.instance.playerFactionID[0]));
			}
			UnitControl.instance.playerUnitsList[0].starting.Clear();
			UnitControl.instance.playerUnitsList[0].starting.AddRange(list3);
			foreach (Tile tile4 in list)
			{
				tile4.openForPlacement = false;
			}
			foreach (Tile tile5 in list2)
			{
				tile5.bUnitOrder = false;
				tile5.openForPlacement = true;
			}
		}
		gameControlTB.SetWinRound(0);
		gameControlTB.SetLoseRound(0);
		if (!GameGlobal.m_bDLCMode)
		{
			UnitControl.instance.bAllEnemyUnitDead = true;
			UnitControl.instance.bAllFriendUnitDead = true;
		}
		for (int num7 = 0; num7 < ban.m_VicReqList.Count; num7++)
		{
			if (ban.m_VicReqList[num7].m_iType == 0)
			{
				if (GameGlobal.m_bDLCMode)
				{
					UnitControl.instance.bAllEnemyUnitDead = true;
				}
			}
			else if (ban.m_VicReqList[num7].m_iType == 1)
			{
				gameControlTB.SetWinRound(ban.m_VicReqList[num7].m_iValue1);
			}
			else if (ban.m_VicReqList[num7].m_iType == 2 && UnitControl.instance != null)
			{
				UnitTB unitTB7 = UnitControl.instance.FindUnit(ban.m_VicReqList[num7].m_iValue1);
				UnitControl.instance.DeadWillWinList.Add(unitTB7);
			}
		}
		for (int num8 = 0; num8 < ban.m_FailReqList.Count; num8++)
		{
			if (ban.m_FailReqList[num8].m_iType == 0)
			{
				if (GameGlobal.m_bDLCMode)
				{
					UnitControl.instance.bAllFriendUnitDead = true;
				}
			}
			else if (ban.m_FailReqList[num8].m_iType == 1)
			{
				gameControlTB.SetLoseRound(ban.m_FailReqList[num8].m_iValue1);
			}
			else if (ban.m_FailReqList[num8].m_iType == 2 && UnitControl.instance != null)
			{
				UnitTB unitTB8 = UnitControl.instance.FindUnit(ban.m_FailReqList[num8].m_iValue1);
				UnitControl.instance.DeadWillLostList.Add(unitTB8);
			}
		}
		this.goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		if (this.goMainCamera == null)
		{
			this.bNeedReturnMainCamera = false;
			this.goMainCamera = new GameObject();
			this.goMainCamera.name = "MainCamera";
			this.goMainCamera.tag = "MainCamera";
			this.goMainCamera.AddComponent<Camera>().depth = 0f;
		}
		else
		{
			this.bNeedReturnMainCamera = true;
			if (this.goMainCamera.GetComponent<OrbitCam>() != null)
			{
				this.bOrbitCamEnable = this.goMainCamera.GetComponent<OrbitCam>().enabled;
				this.goMainCamera.GetComponent<OrbitCam>().enabled = false;
			}
		}
		this.vBackupCameraEulerAngle = this.goMainCamera.transform.localEulerAngles;
		GameObject gameObject3 = new GameObject();
		Vector3 vector2 = bf.vCameraEulerAngle;
		if (vector2 != Vector3.zero)
		{
			gameObject3.transform.localEulerAngles = vector2;
			Debug.LogWarning("Camera use vBattleFiledCameraEulerAngle = " + vector2.ToString());
		}
		else
		{
			gameObject3.transform.localEulerAngles = this.vBackupCameraEulerAngle;
			Debug.LogWarning("Camera use vBackupCameraEulerAngle = " + this.vBackupCameraEulerAngle.ToString());
		}
		gameObject3.transform.parent = this.goRoot.transform;
		gameObject3.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject3.name = "CameraControl";
		CameraControl cameraControl = gameObject3.AddComponent<CameraControl>();
		cameraControl.enableKeyPanning = false;
		cameraControl.trackUnit = true;
		cameraControl.minPosX = gameObject3.transform.position.x + num3;
		cameraControl.maxPosX = gameObject3.transform.position.x + num2;
		cameraControl.minPosZ = gameObject3.transform.position.z + num5;
		cameraControl.maxPosZ = gameObject3.transform.position.z + num4;
		this.goMainCamera.transform.parent = gameObject3.transform;
		this.goMainCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.goMainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		cameraControl.mainCam = this.goMainCamera.GetComponent<Camera>();
		cameraControl.mainCam.transform.localPosition = new Vector3(0f, 0f, -12f);
		GameObject gameObject4 = new GameObject();
		gameObject4.name = "ActionCam";
		gameObject4.transform.parent = this.goRoot.transform;
		gameObject4.transform.localPosition = new Vector3(0f, 0f, 0f);
		Camera camera = gameObject4.AddComponent<Camera>();
		gameObject4.AddComponent<AudioListener>();
		camera.depth = this.goMainCamera.GetComponent<Camera>().depth - 1f;
		cameraControl.actionCam = camera;
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0003E994 File Offset: 0x0003CB94
	private void OnFactionDefeat(int iFaction)
	{
		Debug.Log("OnFactionDefeat " + iFaction.ToString());
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.FactionDefeat)
				{
					if (battleTriggerClip.m_iTriggerData == iFaction)
					{
						if (this.CheckRequest(battleTriggerClip, null, null))
						{
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0003EA48 File Offset: 0x0003CC48
	private void OnBattleStart()
	{
		GameGlobal.AddBattleDifficulty();
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.StartBattle)
				{
					if (this.CheckRequest(battleTriggerClip, null, null))
					{
						this.DoSchedule(battleTriggerClip, true);
					}
				}
			}
		}
		UnitControl.AllUnitFaceToHostile();
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0003EADC File Offset: 0x0003CCDC
	private void OnActorDestroy(UnitTB unit)
	{
		if (unit.bLeaveBattle)
		{
			return;
		}
		if (unit.CheckFriendFaction(0))
		{
			GameGlobal.LessBattleDifficulty();
		}
		else
		{
			GameGlobal.AddBattleDifficulty();
		}
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorDestory)
				{
					if (unit.iUnitID == battleTriggerClip.m_iTriggerData)
					{
						if (this.CheckRequest(battleTriggerClip, null, null))
						{
							float num = unit.destroyEffectDuration;
							if (num > 0.25f)
							{
								num -= 0.2f;
							}
							base.StartCoroutine(this.DelayDoSchedule(battleTriggerClip, num));
						}
					}
				}
			}
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0003EBC4 File Offset: 0x0003CDC4
	private void OnAttackStart(UnitTB srcUnit, UnitTB destUnit)
	{
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorAttackS)
				{
					if (srcUnit.iUnitID != battleTriggerClip.m_iTriggerData)
					{
						continue;
					}
					if (!this.CheckRequest(battleTriggerClip, srcUnit, destUnit))
					{
						continue;
					}
					this.DoSchedule(battleTriggerClip, true);
				}
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorHurtS)
				{
					if (destUnit.iUnitID == battleTriggerClip.m_iTriggerData)
					{
						if (this.CheckRequest(battleTriggerClip, srcUnit, destUnit))
						{
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0003ECA4 File Offset: 0x0003CEA4
	private void OnAttackEnd(UnitTB srcUnit, UnitTB destUnit)
	{
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorAttackE)
				{
					if (srcUnit.iUnitID != battleTriggerClip.m_iTriggerData)
					{
						continue;
					}
					if (!this.CheckRequest(battleTriggerClip, srcUnit, destUnit))
					{
						continue;
					}
					this.DoSchedule(battleTriggerClip, true);
				}
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorHurtE)
				{
					if (destUnit.iUnitID == battleTriggerClip.m_iTriggerData)
					{
						if (this.CheckRequest(battleTriggerClip, srcUnit, destUnit))
						{
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0003ED84 File Offset: 0x0003CF84
	private void OnSetUnit(Tile tile, int iGroupID)
	{
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.ActorInTile)
				{
					if (battleTriggerClip.m_iTriggerData == iGroupID)
					{
						if (this.CheckRequest(battleTriggerClip, tile.unit, tile.unit))
						{
							tile.unit.bTileEventCantReMove = true;
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0003EE3C File Offset: 0x0003D03C
	public void AddTeamExploitsList(UnitTB unit)
	{
		for (int i = 0; i < this.m_teamExploitsList.Count; i++)
		{
			if (this.m_teamExploitsList[i].iUnitID == unit.iUnitID)
			{
				return;
			}
		}
		BattleExploits battleExploits = new BattleExploits();
		battleExploits.iUnitID = unit.iUnitID;
		battleExploits.fValue = 0f;
		this.m_teamExploitsList.Add(battleExploits);
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0003EEAC File Offset: 0x0003D0AC
	private void OnDamage(UnitTB srcUnit, float fDamageDone)
	{
		List<BattleExploits> list;
		if (srcUnit.factionID == 0)
		{
			list = this.m_teamExploitsList;
		}
		else
		{
			list = this.m_enemyExploitsList;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].iUnitID == srcUnit.iUnitID)
			{
				list[i].fValue += fDamageDone;
				return;
			}
		}
		list.Add(new BattleExploits
		{
			iUnitID = srcUnit.iUnitID,
			fValue = fDamageDone
		});
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0003EF2C File Offset: 0x0003D12C
	private void OnRoundTrigger(int iRound)
	{
		if (iRound % 3 == 0)
		{
			GameGlobal.LessBattleDifficulty();
		}
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_bStartClip)
			{
				if (battleTriggerClip.m_TriggerType == _BattleTriggerType.RoundTrigger)
				{
					if (iRound == battleTriggerClip.m_iTriggerData || battleTriggerClip.m_iTriggerData == 0)
					{
						if (this.CheckRequest(battleTriggerClip, null, null))
						{
							this.DoSchedule(battleTriggerClip, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
	private bool CheckRequest(BattleTriggerClip btc, UnitTB srcUnit, UnitTB targetUnit)
	{
		if (btc.m_bUsed && btc.m_iRepeat == 0)
		{
			return false;
		}
		bool flag = false;
		btc.unit = UnitControl.instance.FindUnit(btc.m_iTriggerData);
		UnitTB unitTB = UnitControl.instance.FindUnit(btc.m_iRequireValue1);
		switch (btc.m_RequireType)
		{
		case _RequireType.None:
			flag = true;
			goto IL_3A5;
		case _RequireType.HitPoint:
			if (btc.unit != null)
			{
				float num = (float)btc.unit.HP;
				num = num * 100f / (float)btc.unit.fullHP;
				if (unitTB != null)
				{
					float iVal = (float)(unitTB.HP * 100 / unitTB.fullHP);
					flag = this.CheckRequestValuef(btc, num, iVal);
				}
				else
				{
					flag = this.CheckRequestValuef(btc, num);
					Debug.Log("判斷 " + btc.unit.unitName + " 血量 " + flag.ToString());
				}
			}
			else
			{
				float num = 0f;
				if (unitTB != null)
				{
					float iVal = (float)(unitTB.HP * 100 / unitTB.fullHP);
					flag = this.CheckRequestValuef(btc, num, iVal);
				}
				else
				{
					flag = this.CheckRequestValuef(btc, num);
					Debug.Log("判斷 被打死消失的人 血量 " + flag.ToString());
				}
			}
			goto IL_3A5;
		case _RequireType.Intimacy:
		{
			int val = Game.NpcData.GetNpcFriendly(btc.m_iTriggerData);
			if (btc.m_iRequireValue1 != 0)
			{
				int iVal2 = Game.NpcData.GetNpcFriendly(btc.m_iRequireValue1);
				flag = this.CheckRequestValue(btc, val, iVal2);
			}
			else
			{
				flag = this.CheckRequestValue(btc, val);
			}
			goto IL_3A5;
		}
		case _RequireType.Aggro:
			goto IL_3A5;
		case _RequireType.FactionCount:
		{
			Faction faction = UnitControl.GetFaction(btc.m_iRequireValue1);
			if (faction == null)
			{
				flag = this.CheckRequestValue(btc, 0, btc.m_iRequireValue2);
			}
			else
			{
				flag = this.CheckRequestValue(btc, faction.allUnitList.Count, btc.m_iRequireValue2);
			}
			goto IL_3A5;
		}
		case _RequireType.AttackTo:
			if (targetUnit != null && targetUnit.iUnitID == btc.m_iRequireValue1)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.HurtForm:
			if (srcUnit != null && srcUnit.iUnitID == btc.m_iRequireValue1)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.Quest:
			if (MissionStatus.m_instance.CheckQuest(btc.m_strRequireValue1) && btc.m_RequireEqual == _EqualType.Equal)
			{
				flag = true;
			}
			if (!MissionStatus.m_instance.CheckQuest(btc.m_strRequireValue1) && btc.m_RequireEqual == _EqualType.NotEqual)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.Collection:
			if (MissionStatus.m_instance.CheckCollectionQuest(btc.m_strRequireValue1) && btc.m_RequireEqual == _EqualType.Equal)
			{
				flag = true;
			}
			if (!MissionStatus.m_instance.CheckCollectionQuest(btc.m_strRequireValue1) && btc.m_RequireEqual == _EqualType.NotEqual)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.CharacterOnBattle:
			if (UnitControl.instance.FindUnit(btc.m_iRequireValue1) != null && btc.m_RequireEqual == _EqualType.Equal)
			{
				flag = true;
			}
			if (UnitControl.instance.FindUnit(btc.m_iRequireValue1) == null && btc.m_RequireEqual == _EqualType.NotEqual)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.Faction:
			if (targetUnit.factionID == btc.m_iRequireValue1)
			{
				flag = true;
			}
			goto IL_3A5;
		case _RequireType.Flag:
		{
			int val = Game.Variable[btc.m_strRequireValue1];
			int iVal2 = btc.m_iRequireValue2;
			flag = this.CheckRequestValue(btc, val, iVal2);
			goto IL_3A5;
		}
		}
		flag = false;
		IL_3A5:
		if (!flag)
		{
			BattleTriggerClip battleLinkClip = this.GetBattleLinkClip(btc);
			if (battleLinkClip != null)
			{
				Debug.Log("檢查沒通過 連接 " + battleLinkClip.m_iClipID.ToString());
				if (this.CheckRequest(battleLinkClip, null, null))
				{
					this.DoSchedule(battleLinkClip, true);
				}
			}
		}
		return flag;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0003F3E0 File Offset: 0x0003D5E0
	private bool CheckRequestValuef(BattleTriggerClip btc, float val)
	{
		float iVal = (float)btc.m_iRequireValue2;
		return this.CheckRequestValuef(btc, val, iVal);
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0003F400 File Offset: 0x0003D600
	private bool CheckRequestValue(BattleTriggerClip btc, int val)
	{
		int iRequireValue = btc.m_iRequireValue2;
		return this.CheckRequestValue(btc, val, iRequireValue);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0003F420 File Offset: 0x0003D620
	private bool CheckRequestValuef(BattleTriggerClip btc, float val, float iVal)
	{
		if (btc.m_RequireEqual == _EqualType.Equal)
		{
			return val == iVal;
		}
		if (btc.m_RequireEqual == _EqualType.Greater)
		{
			return val > iVal;
		}
		if (btc.m_RequireEqual == _EqualType.GreaterEqual)
		{
			return val >= iVal;
		}
		if (btc.m_RequireEqual == _EqualType.Less)
		{
			return val < iVal;
		}
		if (btc.m_RequireEqual == _EqualType.LessEqual)
		{
			return val <= iVal;
		}
		return btc.m_RequireEqual == _EqualType.NotEqual && val != iVal;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0003F4B8 File Offset: 0x0003D6B8
	private bool CheckRequestValue(BattleTriggerClip btc, int val, int iVal)
	{
		if (btc.m_RequireEqual == _EqualType.Equal)
		{
			return val == iVal;
		}
		if (btc.m_RequireEqual == _EqualType.Greater)
		{
			return val > iVal;
		}
		if (btc.m_RequireEqual == _EqualType.GreaterEqual)
		{
			return val >= iVal;
		}
		if (btc.m_RequireEqual == _EqualType.Less)
		{
			return val < iVal;
		}
		if (btc.m_RequireEqual == _EqualType.LessEqual)
		{
			return val <= iVal;
		}
		return btc.m_RequireEqual == _EqualType.NotEqual && val != iVal;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0003F550 File Offset: 0x0003D750
	private IEnumerator DelayDoSchedule(BattleTriggerClip btc, float fDelay)
	{
		if (!GameGlobal.m_bBattleTalk)
		{
			GameGlobal.m_bBattleTalk = true;
			yield return new WaitForSeconds(fDelay);
			GameGlobal.m_bBattleTalk = false;
			this.DoSchedule(btc, true);
			yield break;
		}
		if (this.m_ContinueClipList.IndexOf(btc) >= 0 && btc.m_iRepeat == 0)
		{
			yield break;
		}
		Debug.Log("正在對話或動作中 加入排序 Node ID = " + btc.m_iClipID.ToString());
		this.m_ContinueClipList.Add(btc);
		yield break;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0003F588 File Offset: 0x0003D788
	private void DoNextSchedule(BattleTriggerClip btc)
	{
		BattleTriggerClip battleNextClip = this.GetBattleNextClip(btc);
		if (battleNextClip != null)
		{
			if (this.CheckRequest(battleNextClip, null, null))
			{
				this.DoSchedule(battleNextClip, false);
			}
			else if (this.m_ContinueClipList.Count > 0 && !GameGlobal.m_bBattleTalk)
			{
				BattleTriggerClip battleTriggerClip = this.m_ContinueClipList[0];
				this.m_ContinueClipList.RemoveAt(0);
				while (!this.CheckRequest(battleTriggerClip, null, null))
				{
					if (this.m_ContinueClipList.Count <= 0)
					{
						battleTriggerClip = null;
						break;
					}
					battleTriggerClip = this.m_ContinueClipList[0];
					this.m_ContinueClipList.RemoveAt(0);
				}
				if (battleTriggerClip != null)
				{
					this.DoSchedule(battleTriggerClip, false);
				}
			}
		}
		else if (this.m_ContinueClipList.Count > 0 && !GameGlobal.m_bBattleTalk)
		{
			BattleTriggerClip battleTriggerClip2 = this.m_ContinueClipList[0];
			this.m_ContinueClipList.RemoveAt(0);
			while (!this.CheckRequest(battleTriggerClip2, null, null))
			{
				if (this.m_ContinueClipList.Count <= 0)
				{
					battleTriggerClip2 = null;
					break;
				}
				battleTriggerClip2 = this.m_ContinueClipList[0];
				this.m_ContinueClipList.RemoveAt(0);
			}
			if (battleTriggerClip2 != null)
			{
				this.DoSchedule(battleTriggerClip2, false);
			}
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0003F6DC File Offset: 0x0003D8DC
	private void DoSchedule(BattleTriggerClip btc, bool bStart)
	{
		if (GameGlobal.m_bBattleTalk)
		{
			if (bStart)
			{
				if (this.m_ContinueClipList.IndexOf(btc) >= 0 && btc.m_iRepeat == 0)
				{
					return;
				}
				Debug.Log("正在對話或動作中 加入排序 Node ID = " + btc.m_iClipID.ToString());
				this.m_ContinueClipList.Add(btc);
				return;
			}
			else
			{
				Debug.LogWarning("正在對話或動作中 但非起始 Node ID = " + btc.m_iClipID.ToString());
			}
		}
		btc.m_bUsed = true;
		switch (btc.m_TriggerEvent)
		{
		case _TriggerEvent.ShowMessage:
		{
			if (UINGUI.instance)
			{
				UINGUI uingui = UINGUI.instance;
				uingui.onFaceMessageClick = (UINGUI.FaceMessageClick)Delegate.Combine(uingui.onFaceMessageClick, new UINGUI.FaceMessageClick(this.FaceMessageOnClick));
			}
			btc.npcID = Game.MapTalkData.GetTalkNpcId(btc.m_iTriggerValue1, btc.m_iTriggerValue2 - 1);
			btc.unit = UnitControl.instance.FindUnit(btc.npcID);
			string talkString = Game.MapTalkData.GetTalkString(btc.m_iTriggerValue1, btc.m_iTriggerValue2 - 1);
			List<string> list = TextParser.IDtoString(talkString);
			if (list != null)
			{
				btc.msg = TextParser.StringToFormat(talkString, list);
			}
			else
			{
				btc.msg = talkString;
			}
			if (btc.unit != null)
			{
				CameraControl.instance.talkUnit = btc.unit;
				CameraControl.instance.trackTalkUnit = true;
			}
			GameGlobal.m_bBattleTalk = true;
			UINGUI.FaceMessage(btc);
			break;
		}
		case _TriggerEvent.AddTeamMate:
			this.AddBackupUnit(btc);
			break;
		case _TriggerEvent.ChangeFaction:
			this.ChangeFaction(btc);
			break;
		case _TriggerEvent.Condition:
			this.UnitAddCondition(btc);
			break;
		case _TriggerEvent.Quest:
			this.SetQuest(btc);
			break;
		case _TriggerEvent.Collection:
			this.SetCollection(btc);
			break;
		case _TriggerEvent.LeaveBattle:
			this.UnitLeaveBattle(btc);
			break;
		case _TriggerEvent.PlayBackGroundMusic:
			this.PlayerBackGround(btc);
			break;
		case _TriggerEvent.EndGame:
			this.ScheduleEndGame(btc);
			break;
		case _TriggerEvent.ChangeAI:
			this.ChangeAI(btc);
			break;
		case _TriggerEvent.Reward:
			this.DoReward(btc);
			break;
		case _TriggerEvent.SetTileEvent:
			this.SetTileEvent(btc);
			break;
		case _TriggerEvent.ClearTileEvent:
			this.ClearTileEvent(btc);
			break;
		case _TriggerEvent.Flag:
			this.FlagVariable(btc);
			break;
		case _TriggerEvent.Reset:
			this.ScheduleResetGame(btc);
			return;
		}
		if (btc.m_iGroup != 0)
		{
			foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
			{
				if (battleTriggerClip.m_iGroup == btc.m_iGroup)
				{
					battleTriggerClip.m_bUsed = true;
				}
			}
		}
		BattleTriggerClip battleLinkClip = this.GetBattleLinkClip(btc);
		if (battleLinkClip != null)
		{
			Debug.Log("連接 " + battleLinkClip.m_iClipID.ToString());
			if (this.CheckRequest(battleLinkClip, null, null))
			{
				this.DoSchedule(battleLinkClip, true);
			}
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0003F9F4 File Offset: 0x0003DBF4
	private void FaceMessageOnClick(BattleTriggerClip btc)
	{
		if (UINGUI.instance)
		{
			UINGUI uingui = UINGUI.instance;
			uingui.onFaceMessageClick = (UINGUI.FaceMessageClick)Delegate.Remove(uingui.onFaceMessageClick, new UINGUI.FaceMessageClick(this.FaceMessageOnClick));
		}
		CameraControl.instance.trackTalkUnit = false;
		GameGlobal.m_bBattleTalk = false;
		this.DoNextSchedule(btc);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x000053C3 File Offset: 0x000035C3
	private void PlayerBackGround(BattleTriggerClip btc)
	{
		Game.PlayBGMusicMapPath(btc.m_strTriggerValue1);
		this.DoNextSchedule(btc);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000053D8 File Offset: 0x000035D8
	private void ScheduleEndGame(BattleTriggerClip btc)
	{
		UnitControl.instance.bBattleOver = true;
		UnitControl.instance.iWinFaction = btc.m_iTriggerValue1;
		GameControlTB.BattleEnded(btc.m_iTriggerValue1);
		this.DoNextSchedule(btc);
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00005407 File Offset: 0x00003607
	private void ScheduleResetGame(BattleTriggerClip btc)
	{
		GameControlTB.BattleReset();
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0003FA50 File Offset: 0x0003DC50
	private void SetTileEvent(BattleTriggerClip btc)
	{
		string strTileName = "Tile" + btc.m_strTriggerValue1;
		Tile tileByName = GridManager.instance.GetTileByName(strTileName);
		if (tileByName != null)
		{
			tileByName.SetTileEvent(btc.m_iTriggerValue2, btc.m_iTriggerValue3);
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0003FAA0 File Offset: 0x0003DCA0
	private void ClearTileEvent(BattleTriggerClip btc)
	{
		int iTriggerValue = btc.m_iTriggerValue1;
		List<Tile> list = GridManager.GetAllTiles();
		foreach (Tile tile in list)
		{
			if (tile.iBattleScheduleGroup == iTriggerValue)
			{
				tile.ClearTileEvent();
			}
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0003FB14 File Offset: 0x0003DD14
	private void FlagVariable(BattleTriggerClip btc)
	{
		if (btc.m_iTriggerValue2 == 0)
		{
			Game.Variable[btc.m_strTriggerValue1] = btc.m_iTriggerValue3;
		}
		else if (btc.m_iTriggerValue2 == 1)
		{
			GlobalVariableManager variable;
			GlobalVariableManager globalVariableManager = variable = Game.Variable;
			string strTriggerValue;
			string key = strTriggerValue = btc.m_strTriggerValue1;
			int num = variable[strTriggerValue];
			globalVariableManager[key] = num + btc.m_iTriggerValue3;
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0000540E File Offset: 0x0000360E
	private void DoReward(BattleTriggerClip btc)
	{
		Game.RewardData.DoRewardID(btc.m_iTriggerValue1, null);
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0003FB80 File Offset: 0x0003DD80
	private void ChangeAI(BattleTriggerClip btc)
	{
		if (btc.m_iTriggerValue1 <= 10)
		{
			List<UnitTB> allUnitsOfFaction = UnitControl.GetAllUnitsOfFaction(btc.m_iTriggerValue1);
			for (int i = 0; i < allUnitsOfFaction.Count; i++)
			{
				UnitTB unitTB = allUnitsOfFaction[i];
				if (unitTB != null)
				{
					unitTB.aiMode = (_AIMode)btc.m_iTriggerValue2;
					switch (unitTB.aiMode)
					{
					case _AIMode.Threat:
					case _AIMode.Stand:
					case _AIMode.MaxEnemy:
						unitTB.aiTarget = null;
						unitTB.aiTile = null;
						break;
					case _AIMode.Chase:
					case _AIMode.Follow:
					case _AIMode.Protect:
						unitTB.aiTarget = UnitControl.instance.FindUnit(btc.m_iTriggerValue3);
						unitTB.aiTile = null;
						break;
					case _AIMode.Guard:
					{
						unitTB.aiTarget = null;
						string strTileName = "Tile" + btc.m_iTriggerValue3.ToString();
						unitTB.aiTile = GridManager.instance.GetNoUnitOnTileByName(strTileName);
						break;
					}
					}
					Debug.Log(string.Concat(new string[]
					{
						unitTB.unitName,
						" ChangeAI ",
						btc.m_iTriggerValue2.ToString(),
						" ",
						btc.m_iTriggerValue3.ToString()
					}));
				}
			}
		}
		else
		{
			UnitTB unitTB2 = UnitControl.instance.FindUnit(btc.m_iTriggerValue1);
			if (unitTB2 != null)
			{
				unitTB2.aiMode = (_AIMode)btc.m_iTriggerValue2;
				switch (unitTB2.aiMode)
				{
				case _AIMode.Threat:
				case _AIMode.Stand:
				case _AIMode.MaxEnemy:
					unitTB2.aiTarget = null;
					unitTB2.aiTile = null;
					break;
				case _AIMode.Chase:
				case _AIMode.Follow:
				case _AIMode.Protect:
					unitTB2.aiTarget = UnitControl.instance.FindUnit(btc.m_iTriggerValue3);
					unitTB2.aiTile = null;
					break;
				case _AIMode.Guard:
				{
					unitTB2.aiTarget = null;
					string strTileName2 = "Tile" + btc.m_iTriggerValue3.ToString();
					unitTB2.aiTile = GridManager.instance.GetNoUnitOnTileByName(strTileName2);
					break;
				}
				}
				Debug.Log(string.Concat(new string[]
				{
					unitTB2.unitName,
					" ChangeAI ",
					btc.m_iTriggerValue2.ToString(),
					" ",
					btc.m_iTriggerValue3.ToString()
				}));
			}
			else
			{
				Debug.LogWarning("人物 ChangeAI 人物找不到 ID = " + btc.m_iTriggerValue1.ToString());
			}
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0003FDF8 File Offset: 0x0003DFF8
	private void UnitLeaveBattle(BattleTriggerClip btc)
	{
		UnitTB unitTB = UnitControl.instance.FindUnit(btc.m_iTriggerValue1);
		if (unitTB != null)
		{
			unitTB.LeaveBattle(false);
		}
		else
		{
			Debug.LogWarning("人物離開戰場 人物找不到 ID = " + btc.m_iTriggerValue1.ToString());
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0003FE50 File Offset: 0x0003E050
	private void UnitAddCondition(BattleTriggerClip btc)
	{
		ConditionNode conditionNode = this.m_battleAbility.GetConditionNode(btc.m_iTriggerValue2);
		if (conditionNode == null)
		{
			Debug.LogWarning("人物加一個狀態 狀態找不到 ID = " + btc.m_iTriggerValue2.ToString());
			return;
		}
		ConditionNode conditionNode2 = conditionNode.Clone();
		if (btc.m_iTriggerValue1 == 0)
		{
			List<UnitTB> allUnitsOfFaction = UnitControl.GetAllUnitsOfFaction(btc.m_iTriggerValue3);
			for (int i = 0; i < allUnitsOfFaction.Count; i++)
			{
				UnitTB unitTB = allUnitsOfFaction[i];
				if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.StackBuff)
				{
					unitTB.animationTB.PlayUseSchedule(unitTB, 100097002);
				}
				else
				{
					unitTB.animationTB.PlayUseSchedule(unitTB, 100014140);
				}
				if (conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt) || conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee))
				{
					UnitTB nearestHostile = UnitControl.GetNearestHostile(unitTB);
					if (nearestHostile != null)
					{
						conditionNode2.m_iTargetUnitID = nearestHostile.iUnitID;
					}
				}
				unitTB.ApplyCondition(conditionNode2, true);
			}
		}
		else
		{
			UnitTB unitTB2 = UnitControl.instance.FindUnit(btc.m_iTriggerValue1);
			if (unitTB2 != null)
			{
				if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.StackBuff)
				{
					unitTB2.animationTB.PlayUseSchedule(unitTB2, 100097002);
				}
				else
				{
					unitTB2.animationTB.PlayUseSchedule(unitTB2, 100014140);
				}
				if (conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt) || conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee))
				{
					UnitTB nearestHostile2 = UnitControl.GetNearestHostile(unitTB2);
					if (nearestHostile2 != null)
					{
						conditionNode2.m_iTargetUnitID = nearestHostile2.iUnitID;
					}
				}
				unitTB2.ApplyCondition(conditionNode2, true);
			}
			else
			{
				Debug.LogWarning("人物加一個狀態 人物找不到 ID = " + btc.m_iTriggerValue1.ToString());
			}
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00040040 File Offset: 0x0003E240
	private void ChangeFaction(BattleTriggerClip btc)
	{
		UnitTB unitTB = UnitControl.instance.FindUnit(btc.m_iTriggerValue1);
		if (unitTB != null)
		{
			UnitControl.ChangeUnitFaction(unitTB, btc.m_iTriggerValue2);
			unitTB.fDamageReduc = 1f;
			unitTB.animationTB.PlayUseTactic(unitTB);
		}
		else
		{
			Debug.LogWarning("人物加一個狀態 人物找不到 ID = " + btc.m_iTriggerValue1.ToString());
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x000400B4 File Offset: 0x0003E2B4
	private IEnumerator _AddBackupUnit(BattleTriggerClip btc)
	{
		Tile tile;
		if (btc.m_iTriggerValue3 == 0)
		{
			tile = GridManager.instance.GetFactionTile(btc.m_iTriggerValue2);
		}
		else
		{
			string tileName = "Tile" + btc.m_iTriggerValue3;
			tile = GridManager.instance.GetNoUnitOnTileByName(tileName);
			if (tile == null)
			{
				tile = GridManager.instance.GetFactionTile(btc.m_iTriggerValue2);
			}
		}
		if (tile == null)
		{
			Debug.LogError("空間滿了 無法加援軍");
			GameGlobal.m_bBattleTalk = false;
			yield break;
		}
		yield return null;
		UnitTB unit = null;
		for (int i = 0; i < UnitControl.instance.playerUnitsList[0].starting.Count; i++)
		{
			UnitTB tempUnit = UnitControl.instance.playerUnitsList[0].starting[i];
			if (!tempUnit.characterData.bCaptive)
			{
				if (tempUnit.iUnitID == btc.m_iTriggerValue1)
				{
					unit = tempUnit;
					UnitControl.instance.playerUnitsList[0].starting.Remove(unit);
					break;
				}
			}
		}
		if (unit == null)
		{
			unit = this.GenerateBattleUnit(btc.m_iTriggerValue1, null);
		}
		yield return null;
		UnitControl.InsertUnit(unit, tile, btc.m_iTriggerValue2, 0);
		yield return null;
		UnitTB unitNear = UnitControl.GetNearestHostile(unit);
		unit.RotateToUnit(unitNear);
		yield return null;
		string str = string.Format(Game.StringTable.GetString(260015), unit.unitName);
		UINGUI.BattleMessage(str);
		GameGlobal.m_bBattleTalk = false;
		this.DoNextSchedule(btc);
		yield break;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00005428 File Offset: 0x00003628
	private void AddBackupUnit(BattleTriggerClip btc)
	{
		GameGlobal.m_bBattleTalk = true;
		base.StartCoroutine(this._AddBackupUnit(btc));
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000400E0 File Offset: 0x0003E2E0
	private void SetQuest(BattleTriggerClip btc)
	{
		if (!MissionStatus.m_instance.CheckQuest(btc.m_strTriggerValue1))
		{
			Debug.Log("Battle Control 進行中任務 " + Game.QuestData.GetQuestInfo(btc.m_strTriggerValue1));
			MissionStatus.m_instance.AddQuestList(btc.m_strTriggerValue1);
		}
		this.DoNextSchedule(btc);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0004013C File Offset: 0x0003E33C
	private void SetCollection(BattleTriggerClip btc)
	{
		if (MissionStatus.m_instance.CheckQuest(btc.m_strTriggerValue1))
		{
			Debug.Log("Battle Control 移除 進行中任務 " + Game.QuestData.GetQuestInfo(btc.m_strTriggerValue1));
			MissionStatus.m_instance.RemoveQuest(btc.m_strTriggerValue1);
		}
		Debug.Log("Battle Control 完成任務 " + Game.QuestData.GetQuestInfo(btc.m_strTriggerValue1));
		MissionStatus.m_instance.AddCollectionQuest(btc.m_strTriggerValue1);
		this.DoNextSchedule(btc);
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x000401C4 File Offset: 0x0003E3C4
	private BattleTriggerClip GetBattleNextClip(BattleTriggerClip btc)
	{
		if (btc.m_iNextID == 0)
		{
			return null;
		}
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_iClipID == btc.m_iNextID)
			{
				return battleTriggerClip;
			}
		}
		return null;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00040240 File Offset: 0x0003E440
	private BattleTriggerClip GetBattleLinkClip(BattleTriggerClip btc)
	{
		if (btc.m_iLinkID == 0)
		{
			return null;
		}
		foreach (BattleTriggerClip battleTriggerClip in this.m_BattleTriggerClipList)
		{
			if (battleTriggerClip.m_iClipID == btc.m_iLinkID)
			{
				return battleTriggerClip;
			}
		}
		return null;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000402BC File Offset: 0x0003E4BC
	public UnitTB TalentTeamMateRemainBuddyJoin(Tile buddyTile)
	{
		Tile tile = null;
		for (int i = 0; i < buddyTile.neighbours.Count; i++)
		{
			if (buddyTile.neighbours[i].invisible)
			{
				if (buddyTile.neighbours[i].walkable)
				{
					if (!buddyTile.neighbours[i].bUnitOrder)
					{
						if (!(buddyTile.neighbours[i].unit != null))
						{
							tile = buddyTile.neighbours[i];
						}
					}
				}
			}
		}
		if (tile == null)
		{
			tile = GridManager.instance.GetFactionTile(0);
		}
		int num = Random.Range(0, UnitControl.instance.playerUnitsList[0].starting.Count);
		UnitTB unitTB = UnitControl.instance.playerUnitsList[0].starting[num];
		UnitControl.InsertUnit(unitTB, tile, 0, 0);
		UnitControl.instance.playerUnitsList[0].starting.Remove(unitTB);
		return unitTB;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000403E4 File Offset: 0x0003E5E4
	public void UnitAddCondition(int UnitID, int ConditionID, string tacticName)
	{
		if (ConditionID == 911)
		{
			List<UnitTB> unplacedUnit = UnitControl.GetUnplacedUnit();
			Tile factionTile = GridManager.instance.GetFactionTile(0);
			foreach (UnitTB unitTB in unplacedUnit)
			{
				if (unitTB.iUnitID == UnitID)
				{
					UnitControl.InsertUnit(unitTB, factionTile, 0, 0);
					unplacedUnit.Remove(unitTB);
					break;
				}
			}
		}
		else
		{
			UnitTB unitTB2 = UnitControl.instance.FindUnit(UnitID);
			if (unitTB2 != null)
			{
				unitTB2.animationTB.PlayUseTactic(unitTB2);
				unitTB2.ApplyConditionID(ConditionID, tacticName, true);
			}
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000404A8 File Offset: 0x0003E6A8
	public void DoOneTactic(BattleTacticNode node)
	{
		if (node.iTargetType == 3)
		{
			this.iTacticItemCount += node.iTargetWeapon;
			if (UINGUI.instance != null)
			{
				UINGUI.instance.uiHUD.UpdateItemButton();
			}
			this.iTacticPointNow -= node.iTacticPoint;
			if (this.iTacticPointNow < 0)
			{
				this.iTacticPointNow = 0;
			}
			return;
		}
		List<UnitTB> list = new List<UnitTB>();
		switch (node.iTargetFaction)
		{
		case 0:
			list.AddRange(UnitControl.GetAllUnit());
			break;
		case 1:
			list.AddRange(UnitControl.GetAllUnitsOfFaction(GameControlTB.GetPlayerFactionID()));
			break;
		case 2:
			list.AddRange(UnitControl.GetAllHostile(GameControlTB.GetPlayerFactionID()));
			break;
		}
		if (node.iTargetType == 2)
		{
			int i = 0;
			while (i < list.Count)
			{
			}
		}
		if (node.lConditionIDList.Count < 1)
		{
			return;
		}
		this.iTacticPointNow -= node.iTacticPoint;
		if (this.iTacticPointNow < 0)
		{
			this.iTacticPointNow = 0;
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num = Random.Range(0, node.lConditionIDList.Count);
			UnitTB unitTB = list[j];
			unitTB.animationTB.PlayUseTactic(unitTB);
			unitTB.ApplyConditionID(node.lConditionIDList[num], node.sName, true);
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0000543E File Offset: 0x0000363E
	public void AddTacticPoint(int iPoint)
	{
		this.iTacticPointNow += iPoint;
		if (this.iTacticPointNow > this.iTacticPointMax)
		{
			this.iTacticPointNow = this.iTacticPointMax;
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0004062C File Offset: 0x0003E82C
	public List<BattleJoinCharacterNode> mod_NewBattleAddEnemyUnit()
	{
		List<int> list = new List<int>();
		List<CharacterData> npcList = Game.CharacterData.GetNpcList();
		for (int i = 0; i < npcList.Count; i++)
		{
			if (npcList[i].iNpcID < 9900000 && npcList[i].iMaxHp <= 2000)
			{
				list.Add(npcList[i].iNpcID);
			}
		}
		List<BattleJoinCharacterNode> list2 = new List<BattleJoinCharacterNode>();
		for (int j = 0; j < GameGlobal.mod_NewBattleEnemyCount; j++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode = new BattleJoinCharacterNode();
			int num = Random.Range(0, list.Count);
			battleJoinCharacterNode.m_iCharID = list[num];
			battleJoinCharacterNode.m_iFaction = 1;
			battleJoinCharacterNode.m_iTile = 0;
			battleJoinCharacterNode.m_iTurn = 0;
			list2.Add(battleJoinCharacterNode);
		}
		return list2;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x000406F4 File Offset: 0x0003E8F4
	public List<BattleJoinCharacterNode> mod_NewBattleAddTeamUnit()
	{
		List<int> teamMemberIDList = TeamStatus.m_Instance.GetTeamMemberIDList();
		List<BattleJoinCharacterNode> list = new List<BattleJoinCharacterNode>();
		int num = GameGlobal.mod_NewBattleEnemyCount;
		if (teamMemberIDList.Count < num)
		{
			num = teamMemberIDList.Count;
		}
		for (int i = 0; i < num; i++)
		{
			list.Add(new BattleJoinCharacterNode
			{
				m_iCharID = teamMemberIDList[i],
				m_iFaction = 0,
				m_iTile = 0,
				m_iTurn = 0
			});
		}
		return list;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00040764 File Offset: 0x0003E964
	public List<BattleJoinCharacterNode> mod_BossBattleAddEnemyUnit()
	{
		List<int> list = new List<int>();
		List<CharacterData> npcList = Game.CharacterData.GetNpcList();
		for (int i = 0; i < npcList.Count; i++)
		{
			if (npcList[i].iNpcID < 9900000 && npcList[i].iMaxHp >= 8000)
			{
				list.Add(npcList[i].iNpcID);
			}
		}
		List<BattleJoinCharacterNode> list2 = new List<BattleJoinCharacterNode>();
		for (int j = 0; j < GameGlobal.mod_BossBattleEnemyCount; j++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode = new BattleJoinCharacterNode();
			int num = Random.Range(0, list.Count);
			battleJoinCharacterNode.m_iCharID = list[num];
			battleJoinCharacterNode.m_iFaction = 1;
			battleJoinCharacterNode.m_iTile = 0;
			battleJoinCharacterNode.m_iTurn = 0;
			list2.Add(battleJoinCharacterNode);
		}
		return list2;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0004082C File Offset: 0x0003EA2C
	public List<BattleJoinCharacterNode> mod_BossBattleAddTeamUnit()
	{
		List<int> teamMemberIDList = TeamStatus.m_Instance.GetTeamMemberIDList();
		List<BattleJoinCharacterNode> list = new List<BattleJoinCharacterNode>();
		int num = GameGlobal.mod_BossBattleEnemyCount;
		if (teamMemberIDList.Count < num)
		{
			num = teamMemberIDList.Count;
		}
		for (int i = 0; i < num; i++)
		{
			list.Add(new BattleJoinCharacterNode
			{
				m_iCharID = teamMemberIDList[i],
				m_iFaction = 0,
				m_iTile = 0,
				m_iTurn = 0
			});
		}
		return list;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0004089C File Offset: 0x0003EA9C
	public List<BattleJoinCharacterNode> mod_RandomBattleAddTeamUnit()
	{
		List<int> teamMemberIDList = TeamStatus.m_Instance.GetTeamMemberIDList();
		List<BattleJoinCharacterNode> list = new List<BattleJoinCharacterNode>();
		int num = GameGlobal.mod_RandomBattleEnemyCount;
		if (teamMemberIDList.Count < num)
		{
			num = teamMemberIDList.Count;
		}
		for (int i = 0; i < num; i++)
		{
			list.Add(new BattleJoinCharacterNode
			{
				m_iCharID = teamMemberIDList[i],
				m_iFaction = 0,
				m_iTile = 0,
				m_iTurn = 0
			});
		}
		return list;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0004090C File Offset: 0x0003EB0C
	public List<BattleJoinCharacterNode> mod_RandomBattleAddEnemyUnit()
	{
		List<int> list = new List<int>();
		List<CharacterData> npcList = Game.CharacterData.GetNpcList();
		for (int i = 0; i < npcList.Count; i++)
		{
			if (npcList[i].iNpcID < 9900000)
			{
				list.Add(npcList[i].iNpcID);
			}
		}
		List<BattleJoinCharacterNode> list2 = new List<BattleJoinCharacterNode>();
		for (int j = 0; j < GameGlobal.mod_RandomBattleEnemyCount * 2; j++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode = new BattleJoinCharacterNode();
			int num = Random.Range(0, list.Count);
			battleJoinCharacterNode.m_iCharID = list[num];
			battleJoinCharacterNode.m_iFaction = 1;
			battleJoinCharacterNode.m_iTile = 0;
			battleJoinCharacterNode.m_iTurn = 0;
			list2.Add(battleJoinCharacterNode);
		}
		return list2;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x000409C4 File Offset: 0x0003EBC4
	public List<BattleJoinCharacterNode> mod_ShilianBattleAddTeamUnit()
	{
		List<int> teamMemberIDList = TeamStatus.m_Instance.GetTeamMemberIDList();
		List<BattleJoinCharacterNode> list = new List<BattleJoinCharacterNode>();
		int num = GameGlobal.mod_ShilianEnemyCount;
		if (teamMemberIDList.Count < num)
		{
			num = teamMemberIDList.Count;
		}
		for (int i = 0; i < num; i++)
		{
			list.Add(new BattleJoinCharacterNode
			{
				m_iCharID = teamMemberIDList[i],
				m_iFaction = 0,
				m_iTile = 0,
				m_iTurn = 0
			});
		}
		return list;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00040A34 File Offset: 0x0003EC34
	public List<BattleJoinCharacterNode> mod_ShilianBattleAddEnemyUnit()
	{
		List<int> list = new List<int>();
		List<CharacterData> npcList = Game.CharacterData.GetNpcList();
		for (int i = 0; i < npcList.Count; i++)
		{
			if (npcList[i].iNpcID < 9900000)
			{
				list.Add(npcList[i].iNpcID);
			}
		}
		List<BattleJoinCharacterNode> list2 = new List<BattleJoinCharacterNode>();
		for (int j = 0; j < GameGlobal.mod_ShilianEnemyCount; j++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode = new BattleJoinCharacterNode();
			int num = Random.Range(0, list.Count);
			battleJoinCharacterNode.m_iCharID = list[num];
			battleJoinCharacterNode.m_iFaction = 1;
			battleJoinCharacterNode.m_iTile = 0;
			battleJoinCharacterNode.m_iTurn = 0;
			list2.Add(battleJoinCharacterNode);
		}
		return list2;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00040AE8 File Offset: 0x0003ECE8
	public void mod_ShilianBattleParam(int factor, UnitTB unitTB)
	{
		unitTB.fullHP = 1000 + Random.Range(factor * 70, factor * 80);
		unitTB.HP = unitTB.fullHP;
		unitTB.fullSP = 500 + Random.Range(factor * 10, factor * 15);
		unitTB.SP = unitTB.fullSP;
		unitTB.characterData._TotalProperty.SetPlus(CharacterData.PropertyType.Strength, Random.Range(factor / 8, factor / 4));
		unitTB.characterData._TotalProperty.SetPlus(CharacterData.PropertyType.Constitution, Random.Range(factor / 8, factor / 4));
		unitTB.characterData._TotalProperty.SetPlus(CharacterData.PropertyType.Intelligence, Random.Range(factor / 8, factor / 4));
		unitTB.characterData._TotalProperty.SetPlus(CharacterData.PropertyType.Dexterity, Random.Range(factor / 8, factor / 4));
		for (int i = 21; i <= 28; i++)
		{
			unitTB.characterData._TotalProperty._MartialArts.SetPlus((CharacterData.PropertyType)i, Random.Range(factor / 8, factor / 4));
		}
		for (int j = 41; j <= 48; j++)
		{
			unitTB.characterData._TotalProperty._MartialDef.SetPlus((CharacterData.PropertyType)j, Random.Range(factor / 8, factor / 4));
		}
		for (int k = 31; k <= 38; k++)
		{
			unitTB.characterData._TotalProperty._MartialArts.SetPlus((CharacterData.PropertyType)k, Random.Range(factor / 8, factor / 4));
		}
	}

	// Token: 0x0400055E RID: 1374
	public static BattleControl instance;

	// Token: 0x0400055F RID: 1375
	public static AssetBundle g_BattleMapBundle;

	// Token: 0x04000560 RID: 1376
	public BattleArea m_battleArea;

	// Token: 0x04000561 RID: 1377
	public BattleAbility m_battleAbility;

	// Token: 0x04000562 RID: 1378
	public BattleSchedule m_battleSchedule;

	// Token: 0x04000563 RID: 1379
	public BattleTactic m_battleTactic;

	// Token: 0x04000564 RID: 1380
	public AnimationMoveOffestManager m_aniMoveOffsetManager;

	// Token: 0x04000565 RID: 1381
	public List<BattleTriggerClip> m_BattleTriggerClipList;

	// Token: 0x04000566 RID: 1382
	private List<Tile> allTiles;

	// Token: 0x04000567 RID: 1383
	private GameObject goRoot;

	// Token: 0x04000568 RID: 1384
	private GameObject goPlayer;

	// Token: 0x04000569 RID: 1385
	private GameObject goMainCamera;

	// Token: 0x0400056A RID: 1386
	private Transform tUnits;

	// Token: 0x0400056B RID: 1387
	private Transform tGridManager;

	// Token: 0x0400056C RID: 1388
	private Transform tGameControl;

	// Token: 0x0400056D RID: 1389
	private Transform tCameraControl;

	// Token: 0x0400056E RID: 1390
	private AudioClip aClip;

	// Token: 0x0400056F RID: 1391
	public int iLastBattleID;

	// Token: 0x04000570 RID: 1392
	private int iLoadLevelID;

	// Token: 0x04000571 RID: 1393
	private string strLoadLevelName;

	// Token: 0x04000572 RID: 1394
	private string strOldSceneName;

	// Token: 0x04000573 RID: 1395
	private bool bLoadNewScene;

	// Token: 0x04000574 RID: 1396
	private bool bTempRoutine;

	// Token: 0x04000575 RID: 1397
	private bool bNeedReturnMainCamera;

	// Token: 0x04000576 RID: 1398
	private bool bOrbitCamEnable;

	// Token: 0x04000577 RID: 1399
	private bool bOldInside;

	// Token: 0x04000578 RID: 1400
	private int iBattleEndVictoryFactionID;

	// Token: 0x04000579 RID: 1401
	private bool bSaveOldFre;

	// Token: 0x0400057A RID: 1402
	private bool bLoadFromAutoSave;

	// Token: 0x0400057B RID: 1403
	private int iOldFre;

	// Token: 0x0400057C RID: 1404
	private LayerMask lmBackupCameraMask = 0;

	// Token: 0x0400057D RID: 1405
	private Vector3 vBackupCameraEulerAngle;

	// Token: 0x0400057E RID: 1406
	public int iTacticPointNow;

	// Token: 0x0400057F RID: 1407
	public int iTacticPointMax = 10;

	// Token: 0x04000580 RID: 1408
	public int iTacticItemCount;

	// Token: 0x04000581 RID: 1409
	private List<BattleTriggerClip> m_ContinueClipList;

	// Token: 0x04000582 RID: 1410
	public List<BattleExploits> m_teamExploitsList;

	// Token: 0x04000583 RID: 1411
	public List<BattleExploits> m_enemyExploitsList;

	// Token: 0x04000584 RID: 1412
	private int m_iTeamTotalExploits;

	// Token: 0x04000585 RID: 1413
	private int m_iEnemyTotalExploits;

	// Token: 0x04000586 RID: 1414
	private int iDLCLevelID;

	// Token: 0x04000587 RID: 1415
	public static SaveGameDataNode _ResetSaveDataNode;
}
