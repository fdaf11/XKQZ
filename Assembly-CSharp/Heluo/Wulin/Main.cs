using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Heluo.Wulin.UI;
using Steamworks;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000273 RID: 627
	public class Main : MonoBehaviour
	{
		// Token: 0x06000B7D RID: 2941
		[DllImport("Mod.dll", CharSet = 3)]
		private static extern void HelloWorld();

		// Token: 0x06000B7E RID: 2942 RVA: 0x0005E07C File Offset: 0x0005C27C
		private void Awake()
		{
			if (Main.instance == null)
			{
				Main.instance = this;
				if (SteamManager.Initialized)
				{
					Debug.Log("SteamManager.Initialized OK");
				}
				else
				{
					Debug.Log("SteamManager.Initialized GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
				}
				IniFile.GetWindowHandle();
				IniFile.SetWindowTitle(GameGlobal.m_strVersion);
				Debug.Log(GameGlobal.m_strVersion);
				RoundManager.OpenRoundData();
				RoundManager.LoadRound();
				RoundManager.SaveRound();
				this.SetIniFileName();
				try
				{
					Main.HelloWorld();
				}
				catch
				{
					Debug.Log("Mod.dll NoFound");
				}
				this.ReadSettingIni();
				this.m_GameSetting = GameObject.Find("GameSetting");
				if (this.m_GameSetting == null)
				{
					this.m_GameSetting = new GameObject("GameSetting");
					this.m_GameSetting.tag = "GameSetting";
					this.m_GameSetting.AddComponent<Save>();
					this.m_GameSetting.AddComponent<Game>();
					this.m_GameSetting.AddComponent<GameSetting>();
					this.m_GameSetting.AddComponent<MapData>();
					this.m_GameSetting.AddComponent<MovieEventTrigger>();
					this.m_GameSetting.AddComponent<NPC>();
					this.m_GameSetting.AddComponent<MissionStatus>();
					this.m_GameSetting.AddComponent<YoungHeroTime>();
					this.m_GameSetting.AddComponent<TeamStatus>();
					this.m_GameSetting.AddComponent<BackpackStatus>();
					Game.SetBGMusicValue();
				}
				GameSetting component = this.m_GameSetting.GetComponent<GameSetting>();
				IniFile.SetWindowTitle(GameGlobal.m_strVersion);
				string text = IniFile.IniReadValue("setting", "developFont");
				string text2 = IniFile.IniReadValue("setting", "lang");
				if (this.NameFont != null)
				{
					component.m_NameFont = this.NameFont.GetComponent<UIFont>();
				}
				if (this.TextFont != null)
				{
					component.m_TextFont = this.TextFont.GetComponent<UIFont>();
				}
				if (GameGlobal.m_iLangType == GameGlobal.GameLan.Cht)
				{
					if (text.ToLower() == "true")
					{
						component.SetFont(this.ChtNameFont, this.ChtTextFont);
					}
					else
					{
						component.SetFont(this.ChtNameFont, this.UniCodeFont);
					}
				}
				else
				{
					component.SetFont(this.UniCodeFont, this.UniCodeFont);
				}
				if (Game.g_cForm != null)
				{
					GameObject gameObject = new GameObject("BattleShowHide");
					Object.DontDestroyOnLoad(gameObject);
					Battle battle = gameObject.AddComponent<Battle>();
					if (!Game.g_cForm.Contains("UI Root"))
					{
						Debug.LogError("Error to UI Root");
						return;
					}
					GameObject gameObject2 = Game.g_cForm.Load("UI Root") as GameObject;
					GameObject gameObject3 = Object.Instantiate(gameObject2, gameObject2.transform.position, gameObject2.transform.rotation) as GameObject;
					Object.DontDestroyOnLoad(gameObject3);
					gameObject3.name = gameObject2.name;
					if (Battle.instance != null)
					{
						Battle.AddMainGameList(gameObject3);
					}
					new GameObject("AudioListen")
					{
						transform = 
						{
							position = gameObject3.transform.position
						}
					}.AddComponent<AudioListener>();
					string name = string.Empty;
					switch (GameGlobal.m_iLangType)
					{
					case GameGlobal.GameLan.Cht:
						name = "CHT/UI_NGUI";
						break;
					case GameGlobal.GameLan.Chs:
						name = "CHS/UI_NGUI";
						break;
					case GameGlobal.GameLan.Eng:
						name = "ENG/UI_NGUI";
						break;
					}
					if (!Game.g_cForm.Contains(name))
					{
						Debug.LogError("Error to UI_NGUI");
						return;
					}
					GameObject gameObject4 = Game.g_cForm.Load(name) as GameObject;
					GameObject gameObject5 = Object.Instantiate(gameObject4, gameObject4.transform.position, gameObject4.transform.rotation) as GameObject;
					Object.DontDestroyOnLoad(gameObject5);
					gameObject5.name = gameObject4.name;
					Battle.AddBattleList(gameObject5);
					this.CreateUI("cFormMovie");
					string text3 = string.Empty;
					switch (GameGlobal.m_iLangType)
					{
					case GameGlobal.GameLan.Cht:
						text3 = "CHT/cFormStart";
						break;
					case GameGlobal.GameLan.Chs:
						text3 = "CHS/cFormStart";
						break;
					case GameGlobal.GameLan.Eng:
						text3 = "ENG/cFormStart";
						break;
					}
					Debug.Log("strGameStart : " + text3 + " -----------------------------------------------------");
					this.CreateUI(text3);
					this.CreateUI("cFormTalk");
					this.CreateUI("cFormManager");
					this.CreateUI("cFormLoad");
					this.CreateUI("cFormMainSelect");
					this.CreateUI("cFormBackpack");
					this.CreateUI("cFormSaveTalk");
					this.CreateUI("cFormDate");
					this.CreateUI("cFormCharacterStatus");
					this.CreateUI("cFormMission");
					this.CreateUI("cFormTeam");
					this.CreateUI("cFormShop");
					this.CreateUI("cFormSystemSetting");
					this.CreateUI("cFormSaveAndLoad");
					this.CreateUI("UIRumor");
					this.CreateUI("cFormPopUI");
					this.CreateUI("cFormRumor");
					this.CreateUI("cFormDLCCharacterStatus");
					this.CreateUI("cFormInfoCheck");
				}
				return;
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00008FC9 File Offset: 0x000071C9
		public GameObject CreateUI(string strForm)
		{
			return Game.UI.CreateUI(strForm);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0005E5A8 File Offset: 0x0005C7A8
		private void SetIniFileName()
		{
			string text = string.Empty;
			if (Application.platform == 1)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == null)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == 7)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == 2)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			IniFile.SetIniFileName(text + "launch/TaleOfWuxia.ini");
			text += "Config/SaveData/";
			this.BackupFileToSteamCloud(text + "SaveTitle.SaveTitle", "SaveTitle.SaveTitle");
			this.BackupFileToSteamCloud(text + "Save0.Save", "Save0.Save");
			this.BackupFileToSteamCloud(text + "Save1.Save", "Save1.Save");
			this.BackupFileToSteamCloud(text + "Save2.Save", "Save2.Save");
			this.BackupFileToSteamCloud(text + "Save3.Save", "Save3.Save");
			this.BackupFileToSteamCloud(text + "Save4.Save", "Save4.Save");
			this.BackupFileToSteamCloud(text + "Save5.Save", "Save5.Save");
			this.BackupFileToSteamCloud(text + "Save6.Save", "Save6.Save");
			this.BackupFileToSteamCloud(text + "Save7.Save", "Save7.Save");
			this.BackupFileToSteamCloud(text + "Save8.Save", "Save8.Save");
			this.BackupFileToSteamCloud(text + "Save9.Save", "Save9.Save");
			this.BackupFileToSteamCloud(text + "Save10.Save", "Save10.Save");
			this.BackupFileToSteamCloud(text + "Save11.Save", "Save11.Save");
			this.BackupFileToSteamCloud(text + "Save12.Save", "Save12.Save");
			this.BackupFileToSteamCloud(text + "Save13.Save", "Save13.Save");
			this.BackupFileToSteamCloud(text + "Save14.Save", "Save14.Save");
			this.BackupFileToSteamCloud(text + "Save15.Save", "Save15.Save");
			this.BackupFileToSteamCloud(text + "Save16.Save", "Save16.Save");
			this.BackupFileToSteamCloud(text + "Save17.Save", "Save17.Save");
			this.BackupFileToSteamCloud(text + "Save18.Save", "Save18.Save");
			this.BackupFileToSteamCloud(text + "Save19.Save", "Save19.Save");
			this.BackupFileToSteamCloud(text + "SaveAutomaticTitle.SaveTitle", "SaveAutomaticTitle.SaveTitle");
			this.BackupFileToSteamCloud(text + "AutomaticSave0.Save", "AutomaticSave0.Save");
			this.BackupFileToSteamCloud(text + "AutomaticSave1.Save", "AutomaticSave1.Save");
			this.BackupFileToSteamCloud(text + "AutomaticSave2.Save", "AutomaticSave2.Save");
			this.BackupFileToSteamCloud(text + "AutomaticSave3.Save", "AutomaticSave3.Save");
			this.BackupFileToSteamCloud(text + "AutomaticSave4.Save", "AutomaticSave4.Save");
			this.BackupFileToSteamCloud(text + "SaveAutomaticDTitle.SaveTitle", "SaveAutomaticDTitle.SaveTitle");
			this.BackupFileToSteamCloud(text + "AutomaticDSave0.Save", "AutomaticDSave0.Save");
			this.BackupFileToSteamCloud(text + "AutomaticDSave1.Save", "AutomaticDSave1.Save");
			this.BackupFileToSteamCloud(text + "AutomaticDSave2.Save", "AutomaticDSave2.Save");
			this.BackupFileToSteamCloud(text + "AutomaticDSave3.Save", "AutomaticDSave3.Save");
			this.BackupFileToSteamCloud(text + "AutomaticDSave4.Save", "AutomaticDSave4.Save");
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0005E99C File Offset: 0x0005CB9C
		private void ReadSettingIni()
		{
			Debug.Log("ReadSettingIni ");
			CtrlSystemSetting ctrlSystemSetting = new CtrlSystemSetting();
			ctrlSystemSetting.GetSystemData();
			ctrlSystemSetting.SetSystem();
			if (SteamManager.Initialized)
			{
				string currentGameLanguage = SteamApps.GetCurrentGameLanguage();
				if (currentGameLanguage == "tchinese")
				{
					GameGlobal.m_iLangType = GameGlobal.GameLan.Cht;
				}
				else if (currentGameLanguage == "english")
				{
					GameGlobal.m_iLangType = GameGlobal.GameLan.Eng;
				}
				else
				{
					GameGlobal.m_iLangType = GameGlobal.GameLan.Chs;
				}
			}
			string text = IniFile.IniReadValue("setting", "NpcDelayLoading");
			if (text.ToLower() == "false")
			{
				GameGlobal.m_bNewLoading = false;
			}
			else
			{
				GameGlobal.m_bNewLoading = true;
			}
			string text2 = IniFile.IniReadValue("setting", "JumpMovie");
			if (text2.ToLower() == "true")
			{
				GameGlobal.m_bCanJumpMovie = true;
			}
			else
			{
				GameGlobal.m_bCanJumpMovie = false;
			}
			Debug.Log("QualityLevel = " + QualitySettings.GetQualityLevel().ToString());
			Debug.Log("Quality TextureLimit = " + QualitySettings.masterTextureLimit.ToString());
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
		private void BackupFileToSteamCloud(string strLocalFileName, string strSteamFileName)
		{
			if (File.Exists(strLocalFileName))
			{
				StreamReader streamReader = new StreamReader(strLocalFileName);
				string text = streamReader.ReadToEnd();
				int byteCount = Encoding.UTF8.GetByteCount(text);
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				SteamRemoteStorage.FileWrite(strSteamFileName, bytes, byteCount);
				streamReader.Close();
				File.Delete(strLocalFileName);
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0005EB0C File Offset: 0x0005CD0C
		private bool CheckGameLaunch()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			bool result = false;
			foreach (string text in commandLineArgs)
			{
				string text2 = text.ToLower();
				if (text2 == "-launch")
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0005EB5C File Offset: 0x0005CD5C
		private void CallLaunch()
		{
			string text = string.Empty;
			if (Application.platform == 1)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == null)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == 7)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			else if (Application.platform == 2)
			{
				text = Application.dataPath;
				int num = text.LastIndexOf("/");
				text = text.Substring(0, num);
				text += "/";
			}
			Debug.Log("Launch path name = " + text + "launch.exe");
			if (File.Exists(text + "launch.exe"))
			{
				Debug.Log("try to Open ");
				Application.OpenURL(text + "launch.exe");
			}
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x04000D4A RID: 3402
		public static Main instance;

		// Token: 0x04000D4B RID: 3403
		private GameObject m_GameSetting;

		// Token: 0x04000D4C RID: 3404
		public GameObject NameFont;

		// Token: 0x04000D4D RID: 3405
		public GameObject TextFont;

		// Token: 0x04000D4E RID: 3406
		public Font ChtTextFont;

		// Token: 0x04000D4F RID: 3407
		public Font ChtNameFont;

		// Token: 0x04000D50 RID: 3408
		public Font UniCodeFont;
	}
}
