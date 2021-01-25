using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Heluo.Wulin.UI;
using JsonFx.Json;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020002CE RID: 718
	public class Save : MonoBehaviour
	{
		// Token: 0x06000E26 RID: 3622 RVA: 0x00009A64 File Offset: 0x00007C64
		private void Awake()
		{
			if (Save.m_Instance == null)
			{
				Save.m_Instance = this;
			}
			this.m_SaveVersionFix = new List<string>();
			this.m_SaveRumor = new List<SaveRumor>();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00074260 File Offset: 0x00072460
		public void AddSaveRumor(SaveRumor _SaveRumor)
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			if (this.m_SaveRumor.Count == this.MaxSaveRumorCount)
			{
				int num = this.m_SaveRumor.Count - 1;
				this.m_SaveRumor.RemoveAt(num);
			}
			this.m_SaveRumor.Insert(0, _SaveRumor);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000742B8 File Offset: 0x000724B8
		public void LoadRumor()
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			if (this.m_SaveRumor == null)
			{
				return;
			}
			List<Rumor> list = new List<Rumor>();
			int i = 0;
			while (i < this.m_SaveRumor.Count)
			{
				int id = this.m_SaveRumor[i].m_id;
				string rumorQuest = this.m_SaveRumor[i].m_RumorQuest;
				SaveRumor.RumorType type = this.m_SaveRumor[i].m_type;
				string imageId = string.Empty;
				string text = string.Empty;
				string tip = string.Empty;
				if (type == SaveRumor.RumorType.NpcQuset)
				{
					NpcDataNode npcData = Game.NpcData.GetNpcData(id);
					imageId = npcData.m_strBigHeadImage;
					text = npcData.m_strNpcName;
					NpcQuestNode npcquest = Game.NpcQuestData.GetNPCQuest(rumorQuest);
					if (npcquest != null)
					{
						tip = string.Format(npcquest.m_strNote, text);
						goto IL_123;
					}
				}
				else
				{
					if (type != SaveRumor.RumorType.Quest)
					{
						goto IL_123;
					}
					QuestNode questNode = Game.QuestData.GetQuestNode(rumorQuest);
					if (questNode != null)
					{
						if (questNode.m_LimitQuest.m_iNpcCall == 0)
						{
							imageId = "B000001";
						}
						else
						{
							imageId = Game.NpcData.GetBigHeadName(questNode.m_LimitQuest.m_iNpcCall);
						}
						tip = questNode.m_strQuestName;
						goto IL_123;
					}
				}
				IL_140:
				i++;
				continue;
				IL_123:
				Rumor rumor = new Rumor(imageId, string.Empty, tip, string.Empty);
				list.Add(rumor);
				goto IL_140;
			}
			Game.UI.Get<UISaveRumor>().LoadRumor(list);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0007442C File Offset: 0x0007262C
		public void AutoSave()
		{
			if (Save.m_Instance == null)
			{
				return;
			}
			if (GameGlobal.m_OpenAutoSave && !Save.m_Instance.bLoad && !GameGlobal.m_bMovie && !GameGlobal.m_bBattle)
			{
				Debug.Log("看起來沒什麼事，只好幫你自動存個檔");
				Save.m_Instance.SaveData(2, 0, -1);
			}
			if (Save.m_Instance.bLoad)
			{
				Save.m_Instance.bLoad = false;
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000744A8 File Offset: 0x000726A8
		private void GreateNewTitle(List<SaveTitleDataNode> nodeList)
		{
			nodeList.Clear();
			for (int i = 0; i < this.iMaxTileCount; i++)
			{
				nodeList.Add(new SaveTitleDataNode
				{
					m_bHaveData = false,
					m_strMissionID = string.Empty,
					m_strPlaceName = string.Empty,
					m_strTrueYear = string.Empty,
					m_strPlaceImageName = string.Empty,
					m_PlayGameTime = null
				});
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0007451C File Offset: 0x0007271C
		public void LoadTitleData(string filename, List<SaveTitleDataNode> nodeList)
		{
			int num = 0;
			if (!SteamRemoteStorage.FileExists(filename))
			{
				this.GreateNewTitle(nodeList);
				return;
			}
			int fileSize = SteamRemoteStorage.GetFileSize(filename);
			Debug.Log("FileSize = " + fileSize);
			byte[] array = new byte[fileSize];
			int num2 = SteamRemoteStorage.FileRead(filename, array, fileSize);
			string @string = Encoding.UTF8.GetString(array);
			SaveTitleDataNode[] array2 = JsonConvert.DeserializeObject<SaveTitleDataNode[]>(@string);
			if (array2 != null)
			{
				if (array2[num].m_bHaveData)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].m_strPlaceImageName.Split(new char[]
						{
							'/'
						});
						string text = array3[array3.Length - 1];
						text = text.Replace(":tex.tex", ".tex");
						array2[i].m_Texture = this.LoadTexture(text);
					}
				}
				Debug.Log(" _SaveTitle" + array2 + " 讀取擋頭 ");
				nodeList.Clear();
				nodeList.AddRange(array2);
			}
			else
			{
				this.GreateNewTitle(nodeList);
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0007462C File Offset: 0x0007282C
		public void LoadDLCTitleData()
		{
			if (!GameGlobal.m_bDLCMode)
			{
				return;
			}
			string filename = "DLC_SaveTitle.SaveTitle";
			this.LoadTitleData(filename, TeamStatus.m_Instance.m_DLCSaveTitleDataList);
			filename = "DLC_SaveAutomaticTitle.SaveTitle";
			this.LoadTitleData(filename, TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList);
			Debug.Log("DLC_Title 讀取完成");
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0007467C File Offset: 0x0007287C
		public void LoadTitleData()
		{
			string filename = "SaveTitle.SaveTitle";
			this.LoadTitleData(filename, TeamStatus.m_Instance.m_SaveTitleDataList);
			filename = "SaveAutomaticTitle.SaveTitle";
			this.LoadTitleData(filename, TeamStatus.m_Instance.m_AutoSaveTitleDataList);
			Debug.Log("_Title 讀取完成");
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000746C4 File Offset: 0x000728C4
		private SaveGameDataNode convertToSaveData(eSaveType type, int iBattleAreaID)
		{
			SaveGameDataNode saveGameDataNode = new SaveGameDataNode();
			saveGameDataNode.m_strVersion = GameGlobal.m_strSaveVersion;
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject)
			{
				if (gameObject.transform.localPosition == Vector3.zero)
				{
					MapIDNode mapIDNode = Game.MapID.GetMapIDNode(Application.loadedLevelName);
					if (mapIDNode != null)
					{
						if (mapIDNode.Pos != Vector3.zero)
						{
							saveGameDataNode.m_fNowX = mapIDNode.Pos.x;
							saveGameDataNode.m_fNowY = mapIDNode.Pos.y;
							saveGameDataNode.m_fNowZ = mapIDNode.Pos.z;
							saveGameDataNode.m_fDir = mapIDNode.AngleY;
						}
						else
						{
							saveGameDataNode.m_fNowX = GameGlobal.m_TransferPos.x;
							saveGameDataNode.m_fNowY = GameGlobal.m_TransferPos.y;
							saveGameDataNode.m_fNowZ = GameGlobal.m_TransferPos.z;
							saveGameDataNode.m_fDir = GameGlobal.m_fDir;
						}
					}
				}
				else
				{
					saveGameDataNode.m_fNowX = gameObject.transform.localPosition.x;
					saveGameDataNode.m_fNowY = gameObject.transform.localPosition.y;
					saveGameDataNode.m_fNowZ = gameObject.transform.localPosition.z;
					saveGameDataNode.m_fDir = gameObject.transform.localEulerAngles.y;
				}
			}
			if (Camera.main)
			{
				saveGameDataNode._CameraSaveDateNode.Copy(MapData.m_instance._CameraSaveDateNode);
			}
			saveGameDataNode.m_AffterMode = GameGlobal.m_bAffterMode;
			saveGameDataNode.m_iMoney = BackpackStatus.m_Instance.GetMoney();
			saveGameDataNode.m_iAttributePoints = TeamStatus.m_Instance.GetAttributePoints();
			saveGameDataNode.m_strSceneName = Application.loadedLevelName;
			saveGameDataNode.m_iRound = YoungHeroTime.m_instance._YoungHeroTime.iRound;
			saveGameDataNode.m_iDay = YoungHeroTime.m_instance._YoungHeroTime.iday;
			saveGameDataNode.m_TeammateList = TeamStatus.m_Instance.GetTeamMemberIDList();
			saveGameDataNode.m_UnitInfoList = TeamStatus.m_Instance.GetDLCUnitInfoList();
			saveGameDataNode.m_iPrestigePoints = TeamStatus.m_Instance.GetPrestigePoints();
			saveGameDataNode.m_iDLCUnitLimit = TeamStatus.m_Instance.DLCUnitLimit;
			saveGameDataNode.m_iDLCInfoLimit = TeamStatus.m_Instance.DLCInfoLimit;
			saveGameDataNode.m_iDLCStoreLimit = TeamStatus.m_Instance.DLCStoreLimit;
			saveGameDataNode.m_iDLCStoreRenewTurn = TeamStatus.m_Instance.DLCStoreRenewTurn;
			saveGameDataNode.m_iDLCInfoRemain = TeamStatus.m_Instance.DLCInfoRemain;
			saveGameDataNode.m_NowLevelList = TeamStatus.m_Instance.GetNowLevelSaveDataList();
			saveGameDataNode.m_FinishLevelList = TeamStatus.m_Instance.GetFinishLevelSaveDataList();
			saveGameDataNode.m_NpcList = NPC.m_instance.GetSaveDataList();
			saveGameDataNode.m_BackpackList = BackpackStatus.m_Instance.GetBackpackSaveDataList();
			saveGameDataNode.m_EventList.AddRange(TeamStatus.m_Instance.m_EventList);
			Game.Variable.CopyDataTo(saveGameDataNode.Variable);
			Game.Variable.mod_CopyEquipTo(saveGameDataNode.mod_EquipDic);
			saveGameDataNode.m_NpcIsFoughtList = MapData.m_instance.GetSaveNpcIsFought();
			saveGameDataNode.m_QuestList = MissionStatus.m_instance.GetQuestSaveDataList();
			saveGameDataNode.m_TimeQuestList = MissionStatus.m_instance.GetTimeQuestSaveDataList();
			saveGameDataNode.m_iBattleArea = iBattleAreaID;
			saveGameDataNode.m_TreasureBoxIDList.AddRange(TeamStatus.m_Instance.m_TreasureBoxList);
			saveGameDataNode.m_iBigMapModel = GameGlobal.m_iBigMapModel;
			saveGameDataNode.m_fInSideAbLi_r = GameGlobal.m_InSideAbLi.r;
			saveGameDataNode.m_fInSideAbLi_g = GameGlobal.m_InSideAbLi.g;
			saveGameDataNode.m_fInSideAbLi_b = GameGlobal.m_InSideAbLi.b;
			saveGameDataNode.m_fInSideAbLi_a = GameGlobal.m_InSideAbLi.a;
			saveGameDataNode.m_DLCShopInfo = Game.DLCShopInfo.Clone();
			if (GameGlobal.m_iModFixFileCount > 0 && !this.m_SaveVersionFix.Contains("WhatIsMOD1024"))
			{
				this.m_SaveVersionFix.Add("WhatIsMOD1024");
			}
			if (Camera.main != null && Camera.main.GetComponent<OrbitCam>() != null)
			{
				saveGameDataNode._CameraSaveDateNode.m_fDistance = Camera.main.GetComponent<OrbitCam>().Distance;
				saveGameDataNode._CameraSaveDateNode.m_fRotation = Camera.main.GetComponent<OrbitCam>().Rotation;
				saveGameDataNode._CameraSaveDateNode.m_fTilt = Camera.main.GetComponent<OrbitCam>().Tilt;
			}
			saveGameDataNode.m_bHouseInside = GameGlobal.m_bHouseInside;
			saveGameDataNode.m_strHouseName = GameGlobal.m_strHouseName;
			saveGameDataNode.m_iBattleDifficulty = GameGlobal.m_iBattleDifficulty;
			saveGameDataNode.m_GlobalNpcQuest = NpcRandomEvent.g_NpcQuestList;
			saveGameDataNode.m_StroeChangedList = Game.StoreData.GetStoreChangesData();
			saveGameDataNode.m_wtfnpc.Clear();
			saveGameDataNode.m_wtfnpc.AddRange(NpcRandomEvent.BeDoThings);
			this.SaveSaveVersionFix(saveGameDataNode);
			saveGameDataNode.m_SaveRumor = this.m_SaveRumor;
			saveGameDataNode.m_iAutomaticIndex = this.m_iAutomaticIndex + 1;
			saveGameDataNode.m_iAutomaticIndex %= this.iMaxTileCount;
			saveGameDataNode.mod_Difficulty = GameGlobal.mod_Difficulty;
			return saveGameDataNode;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00074B60 File Offset: 0x00072D60
		private void convertToTitle(eSaveType ST, int idx)
		{
			PlayGameTime playGameTime = YoungHeroTime.m_instance.GetPlayGameTime();
			if (GameGlobal.m_bDLCMode)
			{
				if (ST == eSaveType.General)
				{
					if (TeamStatus.m_Instance.m_DLCSaveTitleDataList.Count > 0)
					{
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_bHaveData = true;
						string loadedLevelName = Application.loadedLevelName;
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_strPlaceName = loadedLevelName;
						if (GameGlobal.m_bDLCMode)
						{
							TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_strMissionID = TeamStatus.m_Instance.GetDLCLevelID();
						}
						else
						{
							TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_strMissionID = MissionStatus.m_instance.getNewMissionID();
						}
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_strTrueYear = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_strPlaceImageName = "Config/SaveData/Save" + idx.ToString() + ".Save:tex.tex";
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_PlayGameTime = playGameTime;
					}
					else
					{
						Debug.LogWarning("沒有檔頭資料號碼  " + idx + " 程式內部新增一個");
						SaveTitleDataNode saveTitleDataNode = new SaveTitleDataNode();
						saveTitleDataNode.m_bHaveData = false;
						saveTitleDataNode.m_strPlaceName = string.Empty;
						saveTitleDataNode.m_strMissionID = string.Empty;
						saveTitleDataNode.m_strTrueYear = string.Empty;
						saveTitleDataNode.m_strPlaceImageName = string.Empty;
						TeamStatus.m_Instance.m_DLCSaveTitleDataList.Add(saveTitleDataNode);
						Debug.LogWarning("檔頭資料號碼  " + TeamStatus.m_Instance.m_DLCSaveTitleDataList.Count);
					}
				}
				else if (ST == eSaveType.AutoSave)
				{
					idx = this.m_iAutomaticIndex;
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_bHaveData = true;
					string loadedLevelName2 = Application.loadedLevelName;
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_strPlaceName = loadedLevelName2;
					if (GameGlobal.m_bDLCMode)
					{
						TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_strMissionID = TeamStatus.m_Instance.GetDLCLevelID();
					}
					else
					{
						TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_strMissionID = MissionStatus.m_instance.getNewMissionID();
					}
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_strTrueYear = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_strPlaceImageName = string.Empty;
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_PlayGameTime = playGameTime;
				}
			}
			else if (ST == eSaveType.General)
			{
				if (TeamStatus.m_Instance.m_SaveTitleDataList.Count > 0)
				{
					TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_bHaveData = true;
					string loadedLevelName3 = Application.loadedLevelName;
					TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_strPlaceName = loadedLevelName3;
					if (GameGlobal.m_bDLCMode)
					{
						TeamStatus.m_Instance.m_DLCSaveTitleDataList[idx].m_strMissionID = TeamStatus.m_Instance.GetDLCLevelID();
					}
					else
					{
						TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_strMissionID = MissionStatus.m_instance.getNewMissionID();
					}
					TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_strTrueYear = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
					TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_strPlaceImageName = "Config/SaveData/Save" + idx.ToString() + ".Save:tex.tex";
					TeamStatus.m_Instance.m_SaveTitleDataList[idx].m_PlayGameTime = playGameTime;
				}
				else
				{
					Debug.LogWarning("沒有檔頭資料號碼  " + idx + " 程式內部新增一個");
					SaveTitleDataNode saveTitleDataNode2 = new SaveTitleDataNode();
					saveTitleDataNode2.m_bHaveData = false;
					saveTitleDataNode2.m_strPlaceName = string.Empty;
					saveTitleDataNode2.m_strMissionID = string.Empty;
					saveTitleDataNode2.m_strTrueYear = string.Empty;
					saveTitleDataNode2.m_strPlaceImageName = string.Empty;
					TeamStatus.m_Instance.m_SaveTitleDataList.Add(saveTitleDataNode2);
					Debug.LogWarning("檔頭資料號碼  " + TeamStatus.m_Instance.m_SaveTitleDataList.Count);
				}
			}
			else if (ST == eSaveType.AutoSave)
			{
				idx = this.m_iAutomaticIndex;
				TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_bHaveData = true;
				string loadedLevelName4 = Application.loadedLevelName;
				TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_strPlaceName = loadedLevelName4;
				if (GameGlobal.m_bDLCMode)
				{
					TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[idx].m_strMissionID = TeamStatus.m_Instance.GetDLCLevelID();
				}
				else
				{
					TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_strMissionID = MissionStatus.m_instance.getNewMissionID();
				}
				TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_strTrueYear = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
				TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_strPlaceImageName = string.Empty;
				TeamStatus.m_Instance.m_AutoSaveTitleDataList[idx].m_PlayGameTime = playGameTime;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00075088 File Offset: 0x00073288
		public void SaveData(int iType, int m_iIndex, int iBattleAreaID = -1)
		{
			if (this.bSave)
			{
				return;
			}
			this.bSave = true;
			this.convertToTitle((eSaveType)iType, m_iIndex);
			string filename = string.Empty;
			string text = Game.g_strAssetDataPath + "Config/SaveData";
			Debug.Log(text);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				Debug.LogWarning("建立資料夾 : " + text);
			}
			switch (iType)
			{
			case 1:
				filename = this.GetTitleFileName((eSaveType)iType);
				if (GameGlobal.m_bDLCMode)
				{
					this.SaveTitleFile(filename, TeamStatus.m_Instance.m_DLCSaveTitleDataList);
				}
				else
				{
					this.SaveTitleFile(filename, TeamStatus.m_Instance.m_SaveTitleDataList);
				}
				break;
			case 2:
				filename = this.GetTitleFileName((eSaveType)iType);
				if (GameGlobal.m_bDLCMode)
				{
					this.SaveTitleFile(filename, TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList);
				}
				else
				{
					this.SaveTitleFile(filename, TeamStatus.m_Instance.m_AutoSaveTitleDataList);
				}
				break;
			}
			SaveGameDataNode saveGameDataNode = this.convertToSaveData((eSaveType)iType, iBattleAreaID);
			string text2 = string.Empty;
			switch (iType)
			{
			case 1:
				text2 = this.GetFileName((eSaveType)iType, m_iIndex);
				this.SaveFile(text2, saveGameDataNode);
				this.SaveTexture(text2, m_iIndex);
				break;
			case 2:
				m_iIndex = this.m_iAutomaticIndex;
				text2 = this.GetFileName((eSaveType)iType, m_iIndex);
				this.SaveFile(text2, saveGameDataNode);
				this.m_iAutomaticIndex++;
				this.m_iAutomaticIndex %= this.iMaxTileCount;
				break;
			case 3:
				BattleControl._ResetSaveDataNode = saveGameDataNode;
				break;
			}
			this.bSave = false;
			Debug.Log("SaveData is OK ~");
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0007523C File Offset: 0x0007343C
		private string GetTitleFileName(eSaveType type)
		{
			string result = string.Empty;
			if (type != eSaveType.General)
			{
				if (type == eSaveType.AutoSave)
				{
					if (GameGlobal.m_bDLCMode)
					{
						result = "DLC_SaveAutomaticTitle.SaveTitle";
					}
					else
					{
						result = "SaveAutomaticTitle.SaveTitle";
					}
				}
			}
			else if (GameGlobal.m_bDLCMode)
			{
				result = "DLC_SaveTitle.SaveTitle";
			}
			else
			{
				result = "SaveTitle.SaveTitle";
			}
			return result;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000752A8 File Offset: 0x000734A8
		private string GetFileName(eSaveType type, int idx)
		{
			string result = string.Empty;
			if (type != eSaveType.General)
			{
				if (type == eSaveType.AutoSave)
				{
					if (GameGlobal.m_bDLCMode)
					{
						result = "DLC_AutomaticSave" + idx.ToString() + ".Save";
					}
					else
					{
						result = "AutomaticSave" + idx.ToString() + ".Save";
					}
				}
			}
			else if (GameGlobal.m_bDLCMode)
			{
				result = "DLC_Save" + idx.ToString() + ".Save";
			}
			else
			{
				result = "Save" + idx.ToString() + ".Save";
			}
			return result;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00075358 File Offset: 0x00073558
		public void SaveTitleFile(string filename, List<SaveTitleDataNode> obj)
		{
			string text = string.Empty;
			text = JsonConvert.SerializeObject(obj);
			int byteCount = Encoding.UTF8.GetByteCount(text);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Debug.Log("FileWrite " + SteamRemoteStorage.FileWrite(filename, bytes, byteCount).ToString());
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00075358 File Offset: 0x00073558
		public void SaveFile(string filename, SaveGameDataNode obj)
		{
			string text = string.Empty;
			text = JsonConvert.SerializeObject(obj);
			int byteCount = Encoding.UTF8.GetByteCount(text);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Debug.Log("FileWrite " + SteamRemoteStorage.FileWrite(filename, bytes, byteCount).ToString());
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x000753AC File Offset: 0x000735AC
		public Texture2D LoadTexture(string name)
		{
			if (name.Length == 0)
			{
				return null;
			}
			int fileSize = SteamRemoteStorage.GetFileSize(name);
			byte[] array = new byte[fileSize];
			int num = SteamRemoteStorage.FileRead(name, array, fileSize);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(array);
			return texture2D;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000753F4 File Offset: 0x000735F4
		public void SaveTexture(string name, int idx)
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			RenderTexture active = RenderTexture.active;
			Camera main = Camera.main;
			SuperSampling_SSAA component = Camera.main.gameObject.GetComponent<SuperSampling_SSAA>();
			if (component != null)
			{
				component.Stop();
			}
			RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, 0);
			renderTexture.Create();
			RenderTexture.active = renderTexture;
			main.targetTexture = renderTexture;
			main.Render();
			int num;
			int num2;
			int num3;
			int num4;
			if (Screen.width >= 506 && Screen.height >= 1010)
			{
				num = 506;
				num2 = 1010;
				num3 = (Screen.width - num) / 2;
				num4 = (Screen.height - num2) / 2;
			}
			else
			{
				num = (int)((float)Screen.width * 0.3f);
				num2 = (int)((float)Screen.height * 0.9f);
				num3 = (Screen.width - num) / 2;
				num4 = (Screen.height - num2) / 2;
			}
			Texture2D texture2D = new Texture2D(num, num2, 5, false);
			texture2D.ReadPixels(new Rect((float)num3, (float)num4, (float)num, (float)num2), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			main.targetTexture = null;
			if (component != null)
			{
				component.Start();
			}
			byte[] array = texture2D.EncodeToJPG();
			string pchFile = name + ".tex";
			int cubData = array.Length;
			bool flag = SteamRemoteStorage.FileWrite(pchFile, array, cubData);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0007556C File Offset: 0x0007376C
		private void LoadSaveVersionFix(List<string> saveVersionFix)
		{
			this.m_SaveVersionFix.Clear();
			if (saveVersionFix == null)
			{
				return;
			}
			for (int i = 0; i < saveVersionFix.Count; i++)
			{
				if (saveVersionFix[i] != null)
				{
					if (saveVersionFix[i].Length != 0)
					{
						this.m_SaveVersionFix.Add(saveVersionFix[i]);
					}
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000755DC File Offset: 0x000737DC
		private void SaveSaveVersionFix(SaveGameDataNode _SaveGameDataNode)
		{
			_SaveGameDataNode.m_SaveVersionFix.Clear();
			if (this.m_SaveVersionFix == null)
			{
				return;
			}
			for (int i = 0; i < this.m_SaveVersionFix.Count; i++)
			{
				if (this.m_SaveVersionFix[i] != null)
				{
					if (this.m_SaveVersionFix[i].Length != 0)
					{
						_SaveGameDataNode.m_SaveVersionFix.Add(this.m_SaveVersionFix[i]);
					}
				}
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00075664 File Offset: 0x00073864
		private void convertToGameData(SaveGameDataNode _SaveGameDataNode, int index, eSaveType ST)
		{
			this.LoadSaveVersionFix(_SaveGameDataNode.m_SaveVersionFix);
			GameGlobal.m_bAffterMode = _SaveGameDataNode.m_AffterMode;
			GameGlobal.m_strSaveVersion = _SaveGameDataNode.m_strVersion;
			IniFile.SetWindowTitleSaveData(GameGlobal.m_strVersion, GameGlobal.m_strSaveVersion);
			GameGlobal.m_TransferPos = new Vector3(_SaveGameDataNode.m_fNowX, _SaveGameDataNode.m_fNowY, _SaveGameDataNode.m_fNowZ);
			GameGlobal.m_fDir = _SaveGameDataNode.m_fDir;
			Game.Variable.CopyDataFrom(_SaveGameDataNode.Variable);
			Game.Variable.mod_CopyEquipFrom(_SaveGameDataNode.mod_EquipDic);
			BackpackStatus.m_Instance.StartNewGame();
			BackpackStatus.m_Instance.setMoney(_SaveGameDataNode.m_iMoney);
			YoungHeroTime.m_instance.LoadTime(_SaveGameDataNode.m_iRound, _SaveGameDataNode.m_iDay);
			MapData.m_instance.loadNpcIsFought(_SaveGameDataNode.m_NpcIsFoughtList);
			NPC.m_instance.LoadNPCList(_SaveGameDataNode.m_NpcList);
			BackpackStatus.m_Instance.LoadBackpack(_SaveGameDataNode.m_BackpackList);
			TeamStatus.m_Instance.Reset();
			TeamStatus.m_Instance.LoadMovieEvent(_SaveGameDataNode.m_EventList);
			TeamStatus.m_Instance.LoadTeamMember(_SaveGameDataNode.m_TeammateList);
			TeamStatus.m_Instance.LoadUnitInfoList(_SaveGameDataNode.m_UnitInfoList);
			TeamStatus.m_Instance.ChangeAttributePoints(_SaveGameDataNode.m_iAttributePoints);
			TeamStatus.m_Instance.SetPrestigePoints(_SaveGameDataNode.m_iPrestigePoints);
			TeamStatus.m_Instance.DLCUnitLimit = _SaveGameDataNode.m_iDLCUnitLimit;
			TeamStatus.m_Instance.DLCStoreLimit = _SaveGameDataNode.m_iDLCStoreLimit;
			TeamStatus.m_Instance.DLCStoreRenewTurn = _SaveGameDataNode.m_iDLCStoreRenewTurn;
			TeamStatus.m_Instance.DLCInfoLimit = _SaveGameDataNode.m_iDLCInfoLimit;
			TeamStatus.m_Instance.DLCInfoRemain = _SaveGameDataNode.m_iDLCInfoRemain;
			TeamStatus.m_Instance.LoadTreasureBox(_SaveGameDataNode.m_TreasureBoxIDList);
			TeamStatus.m_Instance.LoadNowList(_SaveGameDataNode.m_NowLevelList);
			TeamStatus.m_Instance.LoadFinishLevel(_SaveGameDataNode.m_FinishLevelList);
			MissionStatus.m_instance.LoadQuestList(_SaveGameDataNode.m_QuestList);
			MissionStatus.m_instance.LoadTimeQuestList(_SaveGameDataNode.m_TimeQuestList);
			GameGlobal.m_iBigMapModel = _SaveGameDataNode.m_iBigMapModel;
			GameGlobal.m_bHouseInside = _SaveGameDataNode.m_bHouseInside;
			GameGlobal.m_strHouseName = _SaveGameDataNode.m_strHouseName;
			GameGlobal.m_InSideAbLi = new Color(_SaveGameDataNode.m_fInSideAbLi_r, _SaveGameDataNode.m_fInSideAbLi_g, _SaveGameDataNode.m_fInSideAbLi_b, _SaveGameDataNode.m_fInSideAbLi_a);
			MapData.m_instance._CameraSaveDateNode.Copy(_SaveGameDataNode._CameraSaveDateNode);
			GameGlobal.m_iBattleDifficulty = _SaveGameDataNode.m_iBattleDifficulty;
			GameGlobal.mod_Difficulty = _SaveGameDataNode.mod_Difficulty;
			Game.StoreData.LoadChangesData(_SaveGameDataNode.m_StroeChangedList);
			NpcRandomEvent.BeDoThings.Clear();
			if (_SaveGameDataNode.m_wtfnpc != null)
			{
				NpcRandomEvent.BeDoThings.AddRange(_SaveGameDataNode.m_wtfnpc.ToArray());
			}
			Game.DLCShopInfo.Copy(_SaveGameDataNode.m_DLCShopInfo);
			if (_SaveGameDataNode.m_GlobalNpcQuest == null)
			{
				NpcRandomEvent.g_NpcQuestList.Clear();
			}
			else
			{
				NpcRandomEvent.g_NpcQuestList.Clear();
				if (!this.m_SaveVersionFix.Contains("SaveGlobalNpcQuestFix1017"))
				{
					int num = 20;
					if (_SaveGameDataNode.m_GlobalNpcQuest.Count > num)
					{
						for (int i = 0; i < num; i++)
						{
							if (!NpcRandomEvent.g_NpcQuestList.Contains(_SaveGameDataNode.m_GlobalNpcQuest[i]))
							{
								NpcRandomEvent.g_NpcQuestList.Add(_SaveGameDataNode.m_GlobalNpcQuest[i]);
							}
						}
					}
					this.m_SaveVersionFix.Add("SaveGlobalNpcQuestFix1017");
				}
				else
				{
					NpcRandomEvent.g_NpcQuestList.AddRange(_SaveGameDataNode.m_GlobalNpcQuest);
				}
			}
			if (this.m_SaveRumor == null)
			{
				this.m_SaveRumor = new List<SaveRumor>();
			}
			else
			{
				this.m_SaveRumor.Clear();
				this.m_SaveRumor.AddRange(_SaveGameDataNode.m_SaveRumor);
				this.LoadRumor();
			}
			if (ST == eSaveType.General)
			{
				string strMissionID = TeamStatus.m_Instance.m_SaveTitleDataList[index].m_strMissionID;
				PlayGameTime playGameTime;
				if (GameGlobal.m_bDLCMode)
				{
					playGameTime = TeamStatus.m_Instance.m_DLCSaveTitleDataList[index].m_PlayGameTime;
				}
				else
				{
					playGameTime = TeamStatus.m_Instance.m_SaveTitleDataList[index].m_PlayGameTime;
				}
				YoungHeroTime.m_instance.LoadSaveGameTime(playGameTime);
			}
			else if (ST == eSaveType.AutoSave)
			{
				string strMissionID2 = TeamStatus.m_Instance.m_AutoSaveTitleDataList[index].m_strMissionID;
				PlayGameTime playGameTime2;
				if (GameGlobal.m_bDLCMode)
				{
					playGameTime2 = TeamStatus.m_Instance.m_DLCAutoSaveTitleDataList[index].m_PlayGameTime;
				}
				else
				{
					playGameTime2 = TeamStatus.m_Instance.m_AutoSaveTitleDataList[index].m_PlayGameTime;
				}
				YoungHeroTime.m_instance.LoadSaveGameTime(playGameTime2);
			}
			this.m_iAutomaticIndex = _SaveGameDataNode.m_iAutomaticIndex;
			this.bLoad = true;
			if (_SaveGameDataNode.m_iBattleArea > 0)
			{
				Game.g_BattleControl.StartBattle(_SaveGameDataNode.m_iBattleArea);
				return;
			}
			if (ST != eSaveType.BattleReset)
			{
				if (GameGlobal.m_bDLCMode)
				{
					Game.LoadScene("ReadyCombat");
					return;
				}
				Game.LoadScene(_SaveGameDataNode.m_strSceneName);
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00075AD4 File Offset: 0x00073CD4
		private string LoadFile(string filename)
		{
			string text = string.Empty;
			if (SteamRemoteStorage.FileExists(filename))
			{
				int fileSize = SteamRemoteStorage.GetFileSize(filename);
				Debug.Log("FileSize = " + fileSize);
				byte[] array = new byte[fileSize];
				int num = SteamRemoteStorage.FileRead(filename, array, fileSize);
				text = Encoding.UTF8.GetString(array);
				JsonReader jsonReader = new JsonReader(text);
			}
			return text;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00075B34 File Offset: 0x00073D34
		public void LoadData(int iType, int m_iIndex)
		{
			SaveGameDataNode saveGameDataNode = null;
			switch (iType)
			{
			case 1:
			case 2:
			{
				string fileName = this.GetFileName((eSaveType)iType, m_iIndex);
				string text = this.LoadFile(fileName);
				saveGameDataNode = JsonConvert.DeserializeObject<SaveGameDataNode>(text);
				break;
			}
			case 3:
				saveGameDataNode = BattleControl._ResetSaveDataNode;
				break;
			}
			if (saveGameDataNode == null)
			{
				return;
			}
			this.convertToGameData(saveGameDataNode, m_iIndex, (eSaveType)iType);
		}

		// Token: 0x040010A8 RID: 4264
		public static Save m_Instance;

		// Token: 0x040010A9 RID: 4265
		public int m_iAutomaticIndex;

		// Token: 0x040010AA RID: 4266
		public bool bLoad;

		// Token: 0x040010AB RID: 4267
		public bool bSave;

		// Token: 0x040010AC RID: 4268
		public List<string> m_SaveVersionFix;

		// Token: 0x040010AD RID: 4269
		private List<SaveRumor> m_SaveRumor;

		// Token: 0x040010AE RID: 4270
		private int MaxSaveRumorCount = 20;

		// Token: 0x040010AF RID: 4271
		private int iMaxTileCount = 20;
	}
}
