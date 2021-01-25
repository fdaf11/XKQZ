using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Heluo.Wulin.UI;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000192 RID: 402
	public class Game : MonoBehaviour
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00006E2C File Offset: 0x0000502C
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x00006E3D File Offset: 0x0000503D
		public static string NextSceneName
		{
			get
			{
				string result = Game.nextSceneName;
				Game.nextSceneName = string.Empty;
				return result;
			}
			set
			{
				Game.nextSceneName = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00006E45 File Offset: 0x00005045
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x00006E4C File Offset: 0x0000504C
		public static string SceneName
		{
			get
			{
				return Game.sceneName;
			}
			set
			{
				Game.sceneName = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00006E54 File Offset: 0x00005054
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x00006E5B File Offset: 0x0000505B
		public static string PrvSceneName
		{
			get
			{
				return Game.prvSceneName;
			}
			set
			{
				Game.prvSceneName = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00006E63 File Offset: 0x00005063
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x00006E6A File Offset: 0x0000506A
		public static string CurSceneName
		{
			get
			{
				return Game.curSceneName;
			}
			set
			{
				Game.curSceneName = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00006E72 File Offset: 0x00005072
		public static MissionPositionManager MissionPositionData
		{
			get
			{
				return MissionPositionManager.Singleton;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00006E79 File Offset: 0x00005079
		public static MissionLevelManager MissionLeveData
		{
			get
			{
				return MissionLevelManager.Singleton;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x00006E80 File Offset: 0x00005080
		public static LevelUpPassiveManager LevelUpPassiveData
		{
			get
			{
				return LevelUpPassiveManager.Singleton;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00006E87 File Offset: 0x00005087
		public static LevelUpExpManager LevelUpExpData
		{
			get
			{
				return LevelUpExpManager.Singleton;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00006E8E File Offset: 0x0000508E
		public static UpgradeManager UpgradeData
		{
			get
			{
				return UpgradeManager.Singleton;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x00006E95 File Offset: 0x00005095
		public static FameManager FameData
		{
			get
			{
				return FameManager.Singleton;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00006E9C File Offset: 0x0000509C
		public static TalentNewManager TalentNewData
		{
			get
			{
				return TalentNewManager.Singleton;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00006EA3 File Offset: 0x000050A3
		public static CharacterDataManager CharacterData
		{
			get
			{
				return CharacterDataManager.Singleton;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00006EAA File Offset: 0x000050AA
		public static NeigongDataManager NeigongData
		{
			get
			{
				return NeigongDataManager.Singleton;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00006EB1 File Offset: 0x000050B1
		public static RoutineNewManager RoutineNewData
		{
			get
			{
				return RoutineNewManager.Singleton;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00006EB8 File Offset: 0x000050B8
		public static UIManager UI
		{
			get
			{
				return UIManager.Singleton;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00006EBF File Offset: 0x000050BF
		public static SettingDataManager SettingData
		{
			get
			{
				return SettingDataManager.Singleton;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00006EC6 File Offset: 0x000050C6
		public static NpcQuestManager NpcQuestData
		{
			get
			{
				return NpcQuestManager.Singleton;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00006ECD File Offset: 0x000050CD
		public static NpcRandomEventManager NpcRandomEventData
		{
			get
			{
				return NpcRandomEventManager.Singleton;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00006ED4 File Offset: 0x000050D4
		public static NpcDataManager NpcData
		{
			get
			{
				return NpcDataManager.Singleton;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00006EDB File Offset: 0x000050DB
		public static StringTableManager StringTable
		{
			get
			{
				return StringTableManager.Singleton;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00006EE2 File Offset: 0x000050E2
		public static QuestDataManager QuestData
		{
			get
			{
				return QuestDataManager.Singleton;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00006EE9 File Offset: 0x000050E9
		public static ItemDataManager ItemData
		{
			get
			{
				return ItemDataManager.Singleton;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x00006EF0 File Offset: 0x000050F0
		public static GroundItemDataManager GroundItemData
		{
			get
			{
				return GroundItemDataManager.Singleton;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00006EF7 File Offset: 0x000050F7
		public static MapIconManager MapIcon
		{
			get
			{
				return MapIconManager.Singleton;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00006EFE File Offset: 0x000050FE
		public static TitleDataManager TitleData
		{
			get
			{
				return TitleDataManager.Singleton;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00006F05 File Offset: 0x00005105
		public static MapTalkManager MapTalkData
		{
			get
			{
				return MapTalkManager.Singleton;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00006F0C File Offset: 0x0000510C
		public static SceneAudioManager SceneAudio
		{
			get
			{
				return SceneAudioManager.Singleton;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00006F13 File Offset: 0x00005113
		public static RewardDataManager RewardData
		{
			get
			{
				return RewardDataManager.Singleton;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00006F1A File Offset: 0x0000511A
		public static MoodTalkManager MoodTalk
		{
			get
			{
				return MoodTalkManager.Singleton;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00006F21 File Offset: 0x00005121
		public static StoreDataManager StoreData
		{
			get
			{
				return StoreDataManager.Singleton;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00006F28 File Offset: 0x00005128
		public static AbilityBookDataManager AbilityBookData
		{
			get
			{
				return AbilityBookDataManager.Singleton;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00006F2F File Offset: 0x0000512F
		public static NpcConductManager NpcConduct
		{
			get
			{
				return NpcConductManager.Singleton;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00006F36 File Offset: 0x00005136
		public static RoutineExpManager RoutineExp
		{
			get
			{
				return RoutineExpManager.Singleton;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00006F3D File Offset: 0x0000513D
		public static BasicExpManager BasicExp
		{
			get
			{
				return BasicExpManager.Singleton;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00006F44 File Offset: 0x00005144
		public static NeigongUpValueManager NeigongUpValue
		{
			get
			{
				return NeigongUpValueManager.Singleton;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00006F4B File Offset: 0x0000514B
		public static QuestionMenuManager QuestionMenu
		{
			get
			{
				return QuestionMenuManager.Singleton;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00006F52 File Offset: 0x00005152
		public static QuestionDataManager QuestionData
		{
			get
			{
				return QuestionDataManager.Singleton;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00006F59 File Offset: 0x00005159
		public static HerbsDataManager HerbsData
		{
			get
			{
				return HerbsDataManager.Singleton;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00006F60 File Offset: 0x00005160
		public static MiningDataManager MiningData
		{
			get
			{
				return MiningDataManager.Singleton;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00006F67 File Offset: 0x00005167
		public static AlchemyManager AlchemyData
		{
			get
			{
				return AlchemyManager.Singleton;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00006F6E File Offset: 0x0000516E
		public static AchievementManager Achievement
		{
			get
			{
				return AchievementManager.Singleton;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00006F75 File Offset: 0x00005175
		public static MapIDManager MapID
		{
			get
			{
				return MapIDManager.Singleton;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00006F7C File Offset: 0x0000517C
		public static BowDataManager BowData
		{
			get
			{
				return BowDataManager.Singleton;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00006F83 File Offset: 0x00005183
		public static HuntDataManager HuntData
		{
			get
			{
				return HuntDataManager.Singleton;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00006F8A File Offset: 0x0000518A
		public static TreasureBoxManager TreasureBox
		{
			get
			{
				return TreasureBoxManager.Singleton;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x00006F91 File Offset: 0x00005191
		public static MainEndDataManager MainEndData
		{
			get
			{
				return MainEndDataManager.Singleton;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00006F98 File Offset: 0x00005198
		public static ChildEndDataManager ChildEndData
		{
			get
			{
				return ChildEndDataManager.Singleton;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00006F9F File Offset: 0x0000519F
		public static GlobalVariableManager Variable
		{
			get
			{
				return GlobalVariableManager.Singleton;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00006FA6 File Offset: 0x000051A6
		public static BiographiesManager Biographies
		{
			get
			{
				return BiographiesManager.Singleton;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00006FAD File Offset: 0x000051AD
		public static NpcBuyItemManager NpcBuyItem
		{
			get
			{
				return NpcBuyItemManager.Singleton;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00006FB4 File Offset: 0x000051B4
		public static DLCShopInfo DLCShopInfo
		{
			get
			{
				return DLCShopInfo.Singleton;
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0004A1B4 File Offset: 0x000483B4
		private void Awake()
		{
			if (Game.instance == null)
			{
				Game.instance = this;
				Application.runInBackground = true;
				Object.DontDestroyOnLoad(base.gameObject);
				Game.sceneList.Add("GameStart");
				Debug.Log(SystemInfo.deviceModel);
				Debug.Log(SystemInfo.deviceName);
				Debug.Log(SystemInfo.deviceType);
				Debug.Log(SystemInfo.deviceUniqueIdentifier);
				Debug.Log(SystemInfo.graphicsDeviceID);
				Debug.Log(SystemInfo.graphicsDeviceName);
				Debug.Log(SystemInfo.graphicsDeviceVendor);
				Debug.Log(SystemInfo.graphicsDeviceVendorID);
				Debug.Log(SystemInfo.graphicsDeviceVersion);
				Debug.Log("graphicsMemorySize = " + SystemInfo.graphicsMemorySize);
				Debug.Log("graphicsPixelFillrate = " + SystemInfo.graphicsPixelFillrate);
				Debug.Log("graphicsShaderLevel = " + SystemInfo.graphicsShaderLevel);
				Debug.Log("maxTextureSize = " + SystemInfo.maxTextureSize);
				Debug.Log("npotSupport = " + SystemInfo.npotSupport);
				Debug.Log("operatingSystem = " + SystemInfo.operatingSystem);
				Debug.Log("processorCount = " + SystemInfo.processorCount);
				Debug.Log("processorType = " + SystemInfo.processorType);
				Debug.Log("systemMemorySize = " + SystemInfo.systemMemorySize);
				if (Application.platform == 1)
				{
					Game.g_strDataPathToApplicationPath = Application.dataPath;
					int num = Game.g_strDataPathToApplicationPath.LastIndexOf("/");
					Game.g_strDataPathToApplicationPath = Game.g_strDataPathToApplicationPath.Substring(0, num);
					num = Game.g_strDataPathToApplicationPath.LastIndexOf("/");
					Game.g_strDataPathToApplicationPath = Game.g_strDataPathToApplicationPath.Substring(0, num);
					Game.g_strDataPathToApplicationPath += "/";
					Game.g_strAssetDataPath = Game.g_strDataPathToApplicationPath;
				}
				else if (Application.platform == null)
				{
					Game.g_strDataPathToApplicationPath = Application.dataPath;
					Game.g_strAssetDataPath = Game.g_strDataPathToApplicationPath + "/";
					int num2 = Game.g_strDataPathToApplicationPath.LastIndexOf("/");
					Game.g_strDataPathToApplicationPath = Game.g_strDataPathToApplicationPath.Substring(0, num2);
					Game.g_strDataPathToApplicationPath += "/";
				}
				else if (Application.platform == 7)
				{
					Game.g_strDataPathToApplicationPath = Application.dataPath;
					Game.g_strAssetDataPath = Game.g_strDataPathToApplicationPath + "/";
					int num3 = Game.g_strDataPathToApplicationPath.LastIndexOf("/");
					Game.g_strDataPathToApplicationPath = Game.g_strDataPathToApplicationPath.Substring(0, num3);
					Game.g_strDataPathToApplicationPath += "/";
				}
				else if (Application.platform == 2)
				{
					Game.g_strDataPathToApplicationPath = Application.dataPath;
					int num4 = Game.g_strDataPathToApplicationPath.LastIndexOf("/");
					Game.g_strDataPathToApplicationPath = Game.g_strDataPathToApplicationPath.Substring(0, num4);
					Game.g_strDataPathToApplicationPath += "/";
					Game.g_strAssetDataPath = Game.g_strDataPathToApplicationPath;
				}
				if (Game.g_AudioPatch == null)
				{
					Game.g_AudioPatch = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Audio/Patch.pk");
				}
				if (Game.g_EffectsPatch == null)
				{
					Game.g_EffectsPatch = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Effects/Patch.pk");
				}
				if (Game.g_ImageFirstPatch == null)
				{
					Game.g_ImageFirstPatch = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/Patch01.pk");
				}
				if (Game.g_ImagePatch == null)
				{
					Game.g_ImagePatch = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/Patch.pk");
				}
				if (Game.g_cFormPatch == null)
				{
					Game.g_cFormPatch = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/cFormPatch.pk");
				}
				if (Game.g_AudioPatch != null)
				{
					Game.g_AudioPatchList.Add(Game.g_AudioPatch);
				}
				if (Game.g_EffectsPatch != null)
				{
					Game.g_EffectsPatchList.Add(Game.g_EffectsPatch);
				}
				if (Game.g_ImageFirstPatch != null)
				{
					Game.g_ImagePatchList.Add(Game.g_ImageFirstPatch);
				}
				if (Game.g_ImagePatch != null)
				{
					Game.g_ImagePatchList.Add(Game.g_ImagePatch);
				}
				if (Game.g_cFormPatch != null)
				{
					Game.g_cFormPatchList.Add(Game.g_cFormPatch);
				}
				if (Game.g_cForm == null)
				{
					switch (GameGlobal.m_iLangType)
					{
					case GameGlobal.GameLan.Cht:
						Game.g_cForm = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/cForm_cht.pk", Game.g_cFormPatchList);
						break;
					case GameGlobal.GameLan.Chs:
						Game.g_cForm = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/cForm_chs.pk", Game.g_cFormPatchList);
						break;
					case GameGlobal.GameLan.Eng:
						Game.g_cForm = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/cForm_eng.pk", Game.g_cFormPatchList);
						break;
					}
				}
				if (Game.g_AudioBundle == null)
				{
					Game.g_AudioBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Audio/Audios.pk", Game.g_AudioPatchList);
				}
				if (Game.g_EffectsBundle == null)
				{
					Game.g_EffectsBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Effects/Effects.pk", Game.g_EffectsPatchList);
				}
				if (Game.g_MapHeadBundle == null)
				{
					Game.g_MapHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/MapHead.pk", Game.g_ImagePatchList);
				}
				if (Game.g_BigHeadBundle == null)
				{
					Game.g_BigHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/BigHead.pk", Game.g_ImagePatchList);
				}
				if (Game.g_TeamHeadBundle == null)
				{
					Game.g_TeamHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/TeamHead.pk", Game.g_ImagePatchList);
				}
				if (Game.g_HexHeadBundle == null)
				{
					Game.g_HexHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/HexHead.pk", Game.g_ImagePatchList);
				}
				if (Game.g_TalentImageBundle == null)
				{
					Game.g_TalentImageBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/TalentImage.pk", Game.g_ImagePatchList);
				}
				if (Game.g_DevelopBackground == null)
				{
					Game.g_DevelopBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/DevelopBackground.pk", Game.g_ImagePatchList);
				}
				if (Game.g_DevelopTalkEvent == null)
				{
					Game.g_DevelopTalkEvent = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/DevelopTalkEvent.pk", Game.g_ImagePatchList);
				}
				if (Game.g_DevelopQHead == null)
				{
					Game.g_DevelopQHead = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/DevelopQHead.pk", Game.g_ImagePatchList);
				}
				if (Game.g_Item == null)
				{
					Game.g_Item = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/Item.pk", Game.g_ImagePatchList);
				}
				if (Game.g_LoadBackground == null)
				{
					switch (GameGlobal.m_iLangType)
					{
					case GameGlobal.GameLan.Cht:
						Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_cht.pk", Game.g_ImagePatchList);
						break;
					case GameGlobal.GameLan.Chs:
						Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_chs.pk", Game.g_ImagePatchList);
						break;
					case GameGlobal.GameLan.Eng:
						Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_eng.pk", Game.g_ImagePatchList);
						break;
					}
				}
			}
			if (Game.g_EffectText == null)
			{
				Game.g_EffectText = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/EffectText.pk");
				if (Game.g_Movie == null)
				{
					Game.g_Movie = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/Movie.pk");
				}
				if (Game.g_EventCubeData == null)
				{
					Game.g_EventCubeData = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/EventCube.pk");
				}
				if (Game.g_MouseCubeData == null)
				{
					Game.g_MouseCubeData = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/MouseEventCube.pk");
				}
				if (Game.g_BigMapNodeData == null)
				{
					Game.g_BigMapNodeData = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/BigMapNodeData.pk");
				}
				if (Game.g_TextFiles == null)
				{
					switch (GameGlobal.m_iLangType)
					{
					case GameGlobal.GameLan.Cht:
						Game.g_TextFiles = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/TextFiles_cht.pk");
						break;
					case GameGlobal.GameLan.Chs:
						Game.g_TextFiles = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/TextFiles_chs.pk");
						break;
					case GameGlobal.GameLan.Eng:
						Game.g_TextFiles = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/TextFiles_eng.pk");
						break;
					}
				}
				if (Game.g_SmallGame == null)
				{
					Game.g_SmallGame = AssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Config/SmallGame.pk");
				}
				if (Game.g_ModelBundle == null)
				{
					Game.g_ModelBundle = new ModelAssetBundle();
					Game.g_ModelBundle.CreateForPath(Game.g_strDataPathToApplicationPath + "Model/");
				}
				if (Game.g_BattleControl == null)
				{
					Game.g_BattleControl = Game.instance.gameObject.AddComponent<BattleControl>();
				}
				foreach (PropertyInfo propertyInfo in typeof(Game).GetProperties(24))
				{
					if (propertyInfo.PropertyType.Name.EndsWith("Manager"))
					{
						try
						{
							propertyInfo.GetValue(this, null);
						}
						catch
						{
							Debug.LogWarning("Can not instantiate " + propertyInfo.PropertyType + " !!");
						}
					}
				}
				return;
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0004AAC8 File Offset: 0x00048CC8
		private void Update()
		{
			if (Game.g_InputManager != null)
			{
				Game.g_InputManager.Update();
			}
			if (Input.GetKeyDown(293))
			{
				string text = Game.g_strDataPathToApplicationPath + "ScreenShot/";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = text + "ScreenShot_" + Game.printCount.ToString("0000") + ".png";
				while (File.Exists(text2))
				{
					Game.printCount++;
					text2 = text + "ScreenShot_" + Game.printCount.ToString("0000") + ".png";
				}
				Application.CaptureScreenshot(text2);
				Game.printCount++;
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0004AB7C File Offset: 0x00048D7C
		public static string[] ExtractTextFile(string fileName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/TextFiles/",
				fileName,
				".txt"
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
					string empty = string.Empty;
					string[] result = streamReader.ReadToEnd().Split(new char[]
					{
						"\n".get_Chars(0)
					});
					if (GameGlobal.m_iModFixFileCount <= 0)
					{
						Debug.Log("Mod active");
					}
					GameGlobal.m_iModFixFileCount++;
					return result;
				}
				catch
				{
					Debug.LogError("散檔讀取失敗 !! ( " + text + " )");
					return null;
				}
			}
			if (Game.g_TextFiles == null)
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 未包檔");
				return null;
			}
			if (!Game.g_TextFiles.Contains("textfiles/" + fileName))
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 不存在");
				return null;
			}
			TextAsset textAsset = Game.g_TextFiles.Load("textfiles/" + fileName) as TextAsset;
			if (textAsset)
			{
				return textAsset.text.Split(new char[]
				{
					"\n".get_Chars(0)
				});
			}
			Debug.LogError("包檔讀取失敗 !! ( " + fileName + " )");
			return null;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00006FBB File Offset: 0x000051BB
		public static AudioClip GetBGClip()
		{
			if (Game.GetMainCameraClip() != null)
			{
				return Game.GetMainCameraClip();
			}
			return Game.GetUIRootClip();
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00006FD5 File Offset: 0x000051D5
		public static AudioClip GetMainCameraClip()
		{
			if (Camera.main == null)
			{
				return null;
			}
			if (Camera.main.audio == null)
			{
				return null;
			}
			return Camera.main.audio.clip;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0004AD34 File Offset: 0x00048F34
		public static AudioClip GetUIRootClip()
		{
			if (Game.UI.Root == null)
			{
				return null;
			}
			Camera componentInChildren = Game.UI.Root.GetComponentInChildren<Camera>();
			if (componentInChildren == null)
			{
				return null;
			}
			if (componentInChildren.audio == null)
			{
				return null;
			}
			return componentInChildren.audio.clip;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00007009 File Offset: 0x00005209
		public static bool PlayBGMusicArea(string strAreaName, int iSubArea)
		{
			return Game.PlayBGMusicMapPath(Game.SceneAudio.GetSencesAudioName(strAreaName, iSubArea));
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000701C File Offset: 0x0000521C
		public static bool PlayBGMusicMapPath(string strClipName)
		{
			return Game.PlayBGMusicMapPath(strClipName, true);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0004AD8C File Offset: 0x00048F8C
		public static bool PlayBGMusicMapPath(string strClipName, bool bloop)
		{
			Debug.Log("PlayBGMusicMapPath " + strClipName);
			AudioClip audioClip = Game.g_AudioBundle.Load("audio/Map/" + strClipName) as AudioClip;
			if (audioClip == null)
			{
				Debug.Log("AudioBundle no have Map Clip");
				return false;
			}
			return Game.PlayBGMusicClip(audioClip, bloop);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00007025 File Offset: 0x00005225
		public static bool PlayBGMusicUIPath(string strClipName, bool bloop)
		{
			return Game.PlayBGMusicClip(Game.g_AudioBundle.Load("audio/UI/" + strClipName) as AudioClip, bloop);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00007047 File Offset: 0x00005247
		public static bool PlayBGMusicClip(AudioClip clip)
		{
			return Game.PlayBGMusicClip(clip, true);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0004ADE0 File Offset: 0x00048FE0
		public static void StopMainCameraBGMusic()
		{
			if (Camera.main == null)
			{
				return;
			}
			if (Camera.main.audio == null)
			{
				return;
			}
			if (Camera.main.audio.clip == null)
			{
				return;
			}
			Camera.main.audio.Stop();
			Camera.main.audio.clip = null;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0004AE48 File Offset: 0x00049048
		public static void StopUIRootBGMusic()
		{
			if (Game.UI.Root == null)
			{
				return;
			}
			Camera componentInChildren = Game.UI.Root.GetComponentInChildren<Camera>();
			if (componentInChildren == null)
			{
				return;
			}
			if (componentInChildren.audio == null)
			{
				return;
			}
			componentInChildren.audio.Stop();
			componentInChildren.audio.clip = null;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00007050 File Offset: 0x00005250
		public static bool PlayBGMusicClip(AudioClip clip, bool bloop)
		{
			return Game.PlayMainCameraBGMusicClip(clip, bloop) || Game.PlayUIRootBGMusicClip(clip, bloop);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0004AEA8 File Offset: 0x000490A8
		public static bool PlayMainCameraBGMusicClip(AudioClip clip, bool bloop)
		{
			if (clip == null)
			{
				return false;
			}
			if (Camera.main == null)
			{
				return false;
			}
			if (Camera.main.audio == null)
			{
				return false;
			}
			if (!Camera.main.audio.enabled)
			{
				Camera.main.audio.enabled = true;
			}
			Camera.main.audio.volume = GameGlobal.m_fMusicValue;
			if (Camera.main.audio.clip != null && Camera.main.audio.clip.name == clip.name)
			{
				return true;
			}
			Camera.main.audio.clip = clip;
			Camera.main.audio.loop = bloop;
			Camera.main.audio.Play();
			return true;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0004AF84 File Offset: 0x00049184
		public static bool PlayUIRootBGMusicClip(AudioClip clip, bool bloop)
		{
			if (clip == null)
			{
				return false;
			}
			if (Game.UI.Root == null)
			{
				return false;
			}
			Camera componentInChildren = Game.UI.Root.GetComponentInChildren<Camera>();
			if (componentInChildren == null)
			{
				return false;
			}
			if (componentInChildren.audio == null)
			{
				return false;
			}
			if (!componentInChildren.audio.enabled)
			{
				componentInChildren.audio.enabled = true;
			}
			componentInChildren.audio.volume = GameGlobal.m_fMusicValue;
			if (componentInChildren.audio.clip != null && componentInChildren.audio.clip.name == clip.name)
			{
				return true;
			}
			componentInChildren.audio.clip = clip;
			componentInChildren.audio.loop = bloop;
			componentInChildren.audio.Play();
			return true;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0004B05C File Offset: 0x0004925C
		public static void CreateBigMap(string sceneName)
		{
			if (sceneName != "M0000_01")
			{
				return;
			}
			GameObject gameObject = new GameObject("BigMapNodePathData");
			if (gameObject != null)
			{
				gameObject.AddComponent<BigMapController>();
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0004B094 File Offset: 0x00049294
		public static void CreateMouseEventCube(string sceneName)
		{
			if (Game.g_MouseCubeData == null)
			{
				return;
			}
			TextAsset textAsset = Game.g_MouseCubeData.Load(sceneName) as TextAsset;
			MouseEventCubeData[] array = null;
			if (textAsset == null)
			{
				Debug.LogError(sceneName + " no have event cube data");
			}
			else
			{
				array = new JsonReader(textAsset.text).Deserialize<MouseEventCubeData[]>();
			}
			MouseEventCube.m_MouseEventCubeList.Clear();
			if (array == null)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].CreateToGameObject();
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0004B114 File Offset: 0x00049314
		public static string GetMovieEventCubeText(string sceneName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/EventCube/",
				sceneName,
				".txt"
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
						Debug.LogError(sceneName + "Exception : " + ex.Message);
						return null;
					}
					string empty = string.Empty;
					string result = streamReader.ReadToEnd();
					if (GameGlobal.m_iModFixFileCount <= 0)
					{
						Debug.Log("Mod active");
					}
					GameGlobal.m_iModFixFileCount++;
					return result;
				}
				catch
				{
					Debug.LogError("MovieEventCube 散檔讀取失敗 !! ( " + text + " )");
					return null;
				}
			}
			if (Game.g_EventCubeData == null)
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + sceneName + " ) 未包檔");
				return null;
			}
			if (!Game.g_EventCubeData.Contains(sceneName))
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + sceneName + " ) 不存在");
				return null;
			}
			TextAsset textAsset = Game.g_EventCubeData.Load(sceneName) as TextAsset;
			if (textAsset)
			{
				return textAsset.text;
			}
			Debug.LogError("包檔讀取失敗 !! ( " + sceneName + " )");
			return null;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0004B284 File Offset: 0x00049484
		public static void CreateMovieEventCube(string sceneName)
		{
			if (Game.g_EventCubeData == null)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("EventCube");
			if (!(gameObject == null))
			{
				Battle.AddMainGameList(gameObject);
				return;
			}
			string movieEventCubeText = Game.GetMovieEventCubeText(sceneName);
			if (movieEventCubeText == null)
			{
				Debug.LogError(sceneName + " no have event cube data");
				return;
			}
			MovieEventCubeDataGroup.ToPlayMode(movieEventCubeText);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00007064 File Offset: 0x00005264
		public static void SetBGMusicValue()
		{
			Game.SetUIRootBGMusicValue();
			Game.SetMainCameraBGMusicValue();
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0004B2DC File Offset: 0x000494DC
		private static void SetUIRootBGMusicValue()
		{
			if (Game.UI.Root == null)
			{
				return;
			}
			Camera componentInChildren = Game.UI.Root.GetComponentInChildren<Camera>();
			if (componentInChildren == null)
			{
				return;
			}
			if (componentInChildren.audio == null)
			{
				return;
			}
			componentInChildren.audio.volume = GameGlobal.m_fMusicValue;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00007070 File Offset: 0x00005270
		private static void SetMainCameraBGMusicValue()
		{
			if (Camera.main == null)
			{
				return;
			}
			if (Camera.main.audio == null)
			{
				return;
			}
			Camera.main.audio.volume = GameGlobal.m_fMusicValue;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0004B338 File Offset: 0x00049538
		public static void PrefabSmallGameEnd()
		{
			if (GameGlobal.m_camMain != null)
			{
				GameGlobal.m_camMain.enabled = true;
			}
			if (!GameGlobal.m_bUIDevelop)
			{
				Game.PlayBGMusicArea(Application.loadedLevelName, GameGlobal.m_iMusicIndex);
			}
			else if (GameGlobal.m_goDevelop != null)
			{
				GameGlobal.m_goDevelop.SetActive(true);
			}
			GameGlobal.m_bCFormOpen = false;
			GameGlobal.m_bPlayingSmallGame = false;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0004B39C File Offset: 0x0004959C
		public static void ChangeChildAllLayer(GameObject go, int layer)
		{
			go.layer = layer;
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Game.ChangeChildAllLayer(go.transform.GetChild(i).gameObject, layer);
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0004B3E0 File Offset: 0x000495E0
		public static bool PrefabSmallGame(string name)
		{
			if (GameGlobal.m_camMain == null)
			{
				GameGlobal.m_camMain = Camera.main;
			}
			if (GameGlobal.m_camSmallGame == null)
			{
				GameGlobal.m_camSmallGame = Game.UI.Root.GetComponentInChildren<Camera>();
			}
			bool flag = false;
			GameObject gameObject = null;
			GameGlobal.m_goDevelop = null;
			foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("cForm"))
			{
				if (gameObject2.name.Equals("cFormDevelop"))
				{
					GameGlobal.m_goDevelop = gameObject2;
				}
				if (gameObject2.name.IndexOf(name) >= 0)
				{
					flag = true;
					gameObject = gameObject2;
				}
			}
			if (!flag)
			{
				string text = "cForm" + name;
				GameObject gameObject3 = Game.g_SmallGame.Load(text) as GameObject;
				if (gameObject3 == null)
				{
					return false;
				}
				gameObject = (Object.Instantiate(gameObject3) as GameObject);
				gameObject.name = gameObject3.name;
				gameObject.transform.parent = Game.UI.Root.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localEulerAngles = Vector3.zero;
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.SetActive(true);
			}
			if (gameObject == null)
			{
				return false;
			}
			if (GameGlobal.m_bUIDevelop && GameGlobal.m_goDevelop != null)
			{
				GameGlobal.m_goDevelop.SetActive(false);
			}
			if (gameObject.GetComponent<UITreasureBox>() != null)
			{
				Game.PlayBGMusicUIPath("TreasureBox", true);
				if (GameGlobal.m_camMain != GameGlobal.m_camSmallGame && GameGlobal.m_camMain != null)
				{
					GameGlobal.m_camMain.enabled = false;
				}
			}
			GameGlobal.m_bPlayingSmallGame = true;
			return true;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000070A7 File Offset: 0x000052A7
		public static bool IsLoading()
		{
			return Game.SceneName != null && Game.SceneName != string.Empty;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0004B59C File Offset: 0x0004979C
		public static void LoadScene(string name)
		{
			if (Game.SceneName != null && Game.SceneName != string.Empty)
			{
				Debug.LogError("Already Load Scene");
				return;
			}
			GameGlobal.m_bCFormOpen = true;
			if ((name == "Hunt" || name == "Herbs" || name == "Fish" || name == "Mining" || name == "TreasureBox" || name == "Alchemy" || name == "Blacksmith" || name == "Cooking" || name == "Flower" || name == "Liquor" || name == "PuzzleGame") && Game.PrefabSmallGame(name))
			{
				return;
			}
			GameGlobal.m_iLoadPos = 0;
			GameGlobal.m_iLoadStringID = 170001;
			Game.instance.ShowLoadingPercentage();
			Game.PrvSceneName = Game.CurSceneName;
			Game.CurSceneName = name;
			Game.NextSceneName = name;
			Game.SceneName = name;
			if (Game.instance.GetComponent<MovieEventTrigger>() != null)
			{
				Game.instance.GetComponent<MovieEventTrigger>().BeforeChangeSceneResetMovieStyle();
			}
			Game.instance.StartCoroutine(Game.ChangeToLoadingScene(Game.NextSceneName));
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000070C1 File Offset: 0x000052C1
		private static IEnumerator ChangeToLoadingScene(string sName)
		{
			if (Game.instance.gameObject.GetComponent<MapData>() != null)
			{
				Game.instance.gameObject.GetComponent<MapData>().Initial();
			}
			yield return null;
			while (Game.layerHideList.Count > 0)
			{
				Game.layerHideList[0].Hide();
				Game.layerHideList.RemoveAt(0);
			}
			while (Game.layerDeleteList.Count > 0)
			{
				Object.Destroy(Game.layerDeleteList[0].gameObject);
				Game.layerDeleteList.RemoveAt(0);
			}
			yield return null;
			if (!Game.sceneList.Contains("Loading"))
			{
				string text = Game.g_strDataPathToApplicationPath + "Scenes/Loading.s";
				Debug.Log(text);
				AssetBundle assetBundle = AssetBundle.CreateFromFile(text);
				yield return assetBundle;
				Game.sceneList.Add("Loading");
			}
			Application.LoadLevel("Loading");
			yield return null;
			Debug.Log("QualitySettings.GetQualityLevel = " + QualitySettings.GetQualityLevel().ToString());
			CtrlSystemSetting ctrlSystemSetting = new CtrlSystemSetting();
			ctrlSystemSetting.GetEffect();
			QualitySettings.shadowDistance = ctrlSystemSetting.GetShadowDistance();
			Game.instance.StartCoroutine(Game.ToLoadScene(sName));
			yield break;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x000070D0 File Offset: 0x000052D0
		private static IEnumerator ToLoadScene(string sName)
		{
			Game.StopUIRootBGMusic();
			yield return null;
			if (Game.g_LoadBackground != null)
			{
				Game.g_LoadBackground.Unload(false);
				switch (GameGlobal.m_iLangType)
				{
				case GameGlobal.GameLan.Cht:
					Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_cht.pk", Game.g_ImagePatchList);
					break;
				case GameGlobal.GameLan.Chs:
					Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_chs.pk", Game.g_ImagePatchList);
					break;
				case GameGlobal.GameLan.Eng:
					Game.g_LoadBackground = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/LoadBackground_eng.pk", Game.g_ImagePatchList);
					break;
				}
				yield return null;
			}
			if (Game.g_AudioBundle != null)
			{
				Game.g_AudioBundle.Unload(true);
				Game.g_AudioBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Audio/Audios.pk", Game.g_AudioPatchList);
				yield return null;
			}
			if (Game.g_BigHeadBundle != null)
			{
				Game.g_BigHeadBundle.Unload(true);
				Game.g_BigHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/BigHead.pk", Game.g_ImagePatchList);
				yield return null;
			}
			if (Game.g_EffectsBundle != null)
			{
				Game.g_EffectsBundle.Unload(true);
				Game.g_EffectsBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Effects/Effects.pk", Game.g_EffectsPatchList);
				yield return null;
			}
			if (Game.g_Item != null)
			{
				Game.g_Item.Unload(true);
				Game.g_Item = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/Item.pk", Game.g_ImagePatchList);
				yield return null;
			}
			if (Game.g_MapHeadBundle != null)
			{
				Game.g_MapHeadBundle.Unload(true);
				Game.g_MapHeadBundle = PatchAssetBundle.CreateFromFile(Game.g_strDataPathToApplicationPath + "Image/MapHead.pk", Game.g_ImagePatchList);
				yield return null;
			}
			int num;
			for (int i = 0; i < Game.g_ModelBundle.GetAssetBundleCount(); i = num + 1)
			{
				Game.g_ModelBundle.FreeAllAssetBundleAndReCreate(i);
				yield return null;
				num = i;
			}
			Resources.UnloadUnusedAssets();
			yield return null;
			GC.Collect();
			yield return null;
			if (sName == "GameStart")
			{
				string strForm = string.Empty;
				switch (GameGlobal.m_iLangType)
				{
				case GameGlobal.GameLan.Cht:
					strForm = "CHT/cFormStart";
					break;
				case GameGlobal.GameLan.Chs:
					strForm = "CHS/cFormStart";
					break;
				case GameGlobal.GameLan.Eng:
					strForm = "ENG/cFormStart";
					break;
				}
				Game.UI.CreateUI(strForm);
				GameGlobal.m_bCFormOpen = false;
				yield return null;
				if (Game.LoadLevelOnFinish != null)
				{
					GameGlobal.m_iLoadStringID = 170005;
					yield return null;
					Game.LoadLevelOnFinish();
				}
				Game.SceneName = string.Empty;
				Game.instance.HideLoadingPercentage();
				Game.UI.ClearAllUIData();
				yield break;
			}
			if (sName == "ReadyCombat")
			{
				Game.UI.CreateUI("cFormReadyCombat");
				GameGlobal.m_bCFormOpen = false;
				yield return null;
				if (Game.LoadLevelOnFinish != null)
				{
					GameGlobal.m_iLoadStringID = 170005;
					yield return null;
					Game.LoadLevelOnFinish();
				}
				Game.SceneName = string.Empty;
				Game.instance.HideLoadingPercentage();
				yield break;
			}
			if (!Game.sceneList.Contains(sName))
			{
				string text = Game.g_strDataPathToApplicationPath + "Scenes/" + sName + ".s";
				Debug.Log(text);
				AssetBundle assetBundle = AssetBundle.CreateFromFile(text);
				yield return assetBundle;
				Game.sceneList.Add(sName);
			}
			AsyncOperation op = Application.LoadLevelAsync(sName);
			op.allowSceneActivation = false;
			int iNpcCount = 0;
			if (Game.instance.gameObject.GetComponent<MapData>() == null)
			{
				Debug.LogError("No MapData");
			}
			else
			{
				string strNewSceneName = (!GameGlobal.m_bAffterMode) ? Game.SceneName : (Game.SceneName + "_after");
				iNpcCount = Game.instance.gameObject.GetComponent<MapData>().LoadMapIconNpcConduct(strNewSceneName);
			}
			int totalCount = iNpcCount + 100;
			int nowPos = 0;
			int iPos = 0;
			float num2;
			while (op.progress < 0.9f)
			{
				num2 = op.progress * 10000f / (float)totalCount;
				iPos = (int)num2;
				while (nowPos < iPos)
				{
					num = nowPos;
					nowPos = num + 1;
					if (nowPos >= 100)
					{
						break;
					}
					GameGlobal.m_iLoadPos = nowPos;
					yield return null;
				}
				yield return null;
			}
			yield return null;
			op.allowSceneActivation = true;
			while (Application.loadedLevelName != Game.SceneName)
			{
				yield return null;
			}
			Debug.Log("開始加載MouseEvent場景");
			string addName = sName + "_MouseEvent";
			string text2 = Game.g_strDataPathToApplicationPath + "Scenes/" + addName + ".s";
			if (File.Exists(text2))
			{
				if (!Game.sceneList.Contains(addName))
				{
					Debug.Log(text2);
					AssetBundle ab2 = AssetBundle.CreateFromFile(text2);
					yield return ab2;
					if (ab2 != null)
					{
						Game.sceneList.Add(addName);
					}
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
				}
				Application.LoadLevelAdditive(addName);
			}
			Debug.Log("結束加載MouseEvent場景");
			yield return new WaitForEndOfFrame();
			Debug.Log("開始加載TerrainForHQ or LQ場景");
			Debug.Log("Quality level = " + QualitySettings.GetQualityLevel());
			int qualityLevel = QualitySettings.GetQualityLevel();
			int num3 = 3;
			string name = sName + ((qualityLevel < num3) ? "_TerrainForLQ" : "_TerrainForHQ");
			string text3 = Game.g_strDataPathToApplicationPath + "Scenes/" + name + ".s";
			if (File.Exists(text3))
			{
				if (!Game.sceneList.Contains(name))
				{
					AssetBundle ab2 = AssetBundle.CreateFromFile(text3);
					yield return ab2;
					if (ab2 != null)
					{
						Game.sceneList.Add(name);
					}
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
					ab2 = null;
				}
				Application.LoadLevelAdditive(name);
			}
			yield return new WaitForEndOfFrame();
			Debug.Log("結束加載TerrainForHQ or LQ場景");
			Debug.LogWarning("轉換場景 " + Application.loadedLevelName);
			CtrlSystemSetting ctrlSystemSetting = new CtrlSystemSetting();
			ctrlSystemSetting.GetEffect();
			if (Terrain.activeTerrain != null)
			{
				float[] activeTerrain = ctrlSystemSetting.GetActiveTerrain();
				Terrain.activeTerrain.heightmapPixelError = activeTerrain[0];
				Terrain.activeTerrain.detailObjectDistance = activeTerrain[1];
				Terrain.activeTerrain.detailObjectDensity = activeTerrain[2];
			}
			Game.ProducePlayer();
			Game.CreateMovieEventCube(Game.SceneName);
			Game.CreateBigMap(Game.SceneName);
			Game.SetBGMusicValue();
			Resources.UnloadUnusedAssets();
			yield return null;
			GC.Collect();
			yield return null;
			num2 = (float)(10000 / totalCount);
			iPos = (int)num2;
			while (nowPos < iPos)
			{
				num = nowPos;
				nowPos = num + 1;
				if (nowPos >= 100)
				{
					break;
				}
				GameGlobal.m_iLoadPos = nowPos;
				yield return null;
			}
			GameGlobal.m_iLoadStringID = 170002;
			if (Game.instance.gameObject.GetComponent<MapData>() != null)
			{
				Game.instance.gameObject.GetComponent<MapData>().FindSkyDome();
				Game.instance.gameObject.GetComponent<MapData>().SetCameraFocus();
			}
			if (Game.instance.GetComponent<MovieEventTrigger>() != null)
			{
				Game.instance.GetComponent<MovieEventTrigger>().PreLoadMovie();
				Debug.Log("PreLoadMovie");
			}
			yield return null;
			if (Game.instance.gameObject.GetComponent<MapData>() != null)
			{
				string strNewSceneName2 = (!GameGlobal.m_bAffterMode) ? Game.SceneName : (Game.SceneName + "_after");
				int num4 = Game.instance.gameObject.GetComponent<MapData>().LoadTreasureBox(strNewSceneName2);
				for (int j = 0; j < num4; j++)
				{
					Game.instance.gameObject.GetComponent<MapData>().LoadTreasureBoxPos(strNewSceneName2, j);
				}
			}
			yield return null;
			if (iNpcCount > 0 && Game.instance.gameObject.GetComponent<MapData>() != null)
			{
				Game.instance.gameObject.GetComponent<MapData>().CheckMapNpc();
			}
			for (int i = 0; i < iNpcCount; i = num + 1)
			{
				if (Game.instance.gameObject.GetComponent<MapData>() != null)
				{
					Game.instance.gameObject.GetComponent<MapData>().LoadNpcConductPos(i);
				}
				iPos = (i + 100) * 100 / totalCount;
				while (nowPos < iPos)
				{
					num = nowPos;
					nowPos = num + 1;
					if (nowPos > 100)
					{
						break;
					}
					GameGlobal.m_iLoadPos = nowPos;
					yield return null;
				}
				num = i;
			}
			Game.UI.Show<UIDate>();
			Game.UI.Hide<UIBigMapEnter>();
			if (Game.instance.gameObject.GetComponent<MapData>() != null)
			{
				GameGlobal.m_iLoadStringID = 170003;
				Game.instance.gameObject.GetComponent<MapData>().SetInOutSide();
				yield return null;
				Debug.Log("SetInOutSide");
				GameGlobal.m_iLoadStringID = 170004;
				Game.instance.gameObject.GetComponent<MapData>().SetPos();
				yield return null;
				Debug.Log("SetPos");
			}
			yield return null;
			yield return null;
			Game.PlayBGMusicArea(Game.SceneName, 0);
			Debug.Log("PlayBGMusicLoop");
			if (Game.LoadLevelOnFinish != null)
			{
				GameGlobal.m_iLoadStringID = 170005;
				yield return null;
				Game.LoadLevelOnFinish();
			}
			yield return new WaitForEndOfFrame();
			if (Save.m_Instance != null)
			{
				Save.m_Instance.AutoSave();
			}
			yield return null;
			if (Game.instance.gameObject.GetComponent<MapData>() != null && GameGlobal.m_bNewLoading)
			{
				Game.instance.gameObject.GetComponent<MapData>().StartDelayLoading();
			}
			GameGlobal.m_bCFormOpen = false;
			yield return null;
			Game.SceneName = string.Empty;
			Game.instance.HideLoadingPercentage();
			yield break;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000070DF File Offset: 0x000052DF
		private void SetLoadingText(string strText)
		{
			if (Game.UI.Get<UILoad>() != null)
			{
				Game.UI.Get<UILoad>().SetLoadingBarTitle(strText);
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00007103 File Offset: 0x00005303
		private void SetLoadingPercentage(int iValue)
		{
			GameGlobal.m_iLoadPos = iValue;
			if (Game.UI.Get<UILoad>() != null)
			{
				Game.UI.Get<UILoad>().SetLoadingPercentage(iValue);
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0000712D File Offset: 0x0000532D
		private void ShowLoadingPercentage()
		{
			GameGlobal.m_bLoading = true;
			Game.UI.Show<UILoad>();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000713F File Offset: 0x0000533F
		public void HideLoadingPercentage()
		{
			GameGlobal.m_bLoading = false;
			if (Game.UI.Get<UILoad>() != null)
			{
				Game.UI.Get<UILoad>().HideLoading();
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0004B6DC File Offset: 0x000498DC
		private static void ProducePlayer()
		{
			if (Game.sceneName == "M0000_01")
			{
				return;
			}
			if (GameObject.FindGameObjectWithTag("Player") != null)
			{
				return;
			}
			Debug.LogWarning("ProducePlayer " + GameGlobal.m_strMainPlayer);
			GameGlobal.m_strMainPlayer = ((!GameGlobal.m_bAffterMode) ? "Xyg_NEW_Xuan" : "Player");
			if (GameGlobal.m_strMainPlayer == null)
			{
				return;
			}
			if (GameGlobal.m_strMainPlayer.IndexOf("_ModelPrefab") >= 0)
			{
				GameGlobal.m_strMainPlayer = GameGlobal.m_strMainPlayer.Substring(0, GameGlobal.m_strMainPlayer.Length - 12);
			}
			GameObject gameObject = Game.g_ModelBundle.Load(GameGlobal.m_strMainPlayer + "_ModelPrefab") as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate(gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
				gameObject2.name = gameObject.name;
				gameObject2.tag = "Player";
				gameObject2.layer = 8;
				gameObject2.transform.localEulerAngles = new Vector3(0f, GameGlobal.m_fDir, 0f);
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
				if (gameObject2.GetComponent<PlayerController>() != null)
				{
					gameObject2.GetComponent<PlayerController>().m_strModelName = GameGlobal.m_strMainPlayer;
					if (gameObject2.GetComponent<NpcCollider>())
					{
						gameObject2.GetComponent<NpcCollider>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<NpcCollider>() != null)
				{
					gameObject2.GetComponent<NpcCollider>().m_strModelName = GameGlobal.m_strMainPlayer;
					gameObject2.GetComponent<NpcCollider>().enabled = false;
				}
			}
			else
			{
				Debug.LogWarning("ProducePlayer = null");
			}
			Debug.Log("ProducePlayer End");
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0004B8B8 File Offset: 0x00049AB8
		public static Texture2D mod_Load(string fileName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/Image/",
				fileName,
				".png"
			});
			Texture2D texture2D = new Texture2D(2, 2, 5, false, false);
			texture2D.LoadImage(File.ReadAllBytes(text));
			return texture2D;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0004B914 File Offset: 0x00049B14
		public static Sprite mod_LoadSprite(string fileName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/Image/",
				fileName,
				".png"
			});
			Texture2D texture2D = new Texture2D(2, 2, 5, false, false);
			texture2D.LoadImage(File.ReadAllBytes(text));
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2((float)texture2D.width / 2f, (float)texture2D.height / 2f));
		}

		// Token: 0x040007F9 RID: 2041
		public static Game instance;

		// Token: 0x040007FA RID: 2042
		public static Game.LoadSenceOnFinish LoadLevelOnFinish = null;

		// Token: 0x040007FB RID: 2043
		private static string nextSceneName;

		// Token: 0x040007FC RID: 2044
		private static string sceneName;

		// Token: 0x040007FD RID: 2045
		private static string prvSceneName;

		// Token: 0x040007FE RID: 2046
		private static string curSceneName;

		// Token: 0x040007FF RID: 2047
		public static AssetBundle g_AudioPatch;

		// Token: 0x04000800 RID: 2048
		public static AssetBundle g_EffectsPatch;

		// Token: 0x04000801 RID: 2049
		public static AssetBundle g_ImagePatch;

		// Token: 0x04000802 RID: 2050
		public static AssetBundle g_ImageFirstPatch;

		// Token: 0x04000803 RID: 2051
		public static AssetBundle g_cFormPatch;

		// Token: 0x04000804 RID: 2052
		public static List<AssetBundle> g_AudioPatchList = new List<AssetBundle>();

		// Token: 0x04000805 RID: 2053
		public static List<AssetBundle> g_EffectsPatchList = new List<AssetBundle>();

		// Token: 0x04000806 RID: 2054
		public static List<AssetBundle> g_ImagePatchList = new List<AssetBundle>();

		// Token: 0x04000807 RID: 2055
		public static List<AssetBundle> g_cFormPatchList = new List<AssetBundle>();

		// Token: 0x04000808 RID: 2056
		public static PatchAssetBundle g_cForm;

		// Token: 0x04000809 RID: 2057
		public static PatchAssetBundle g_AudioBundle;

		// Token: 0x0400080A RID: 2058
		public static PatchAssetBundle g_EffectsBundle;

		// Token: 0x0400080B RID: 2059
		public static ModelAssetBundle g_ModelBundle;

		// Token: 0x0400080C RID: 2060
		public static PatchAssetBundle g_MapHeadBundle;

		// Token: 0x0400080D RID: 2061
		public static PatchAssetBundle g_BigHeadBundle;

		// Token: 0x0400080E RID: 2062
		public static PatchAssetBundle g_TeamHeadBundle;

		// Token: 0x0400080F RID: 2063
		public static PatchAssetBundle g_HexHeadBundle;

		// Token: 0x04000810 RID: 2064
		public static PatchAssetBundle g_TalentImageBundle;

		// Token: 0x04000811 RID: 2065
		public static PatchAssetBundle g_DevelopBackground;

		// Token: 0x04000812 RID: 2066
		public static PatchAssetBundle g_DevelopTalkEvent;

		// Token: 0x04000813 RID: 2067
		public static PatchAssetBundle g_DevelopQHead;

		// Token: 0x04000814 RID: 2068
		public static PatchAssetBundle g_AbilityBook;

		// Token: 0x04000815 RID: 2069
		public static PatchAssetBundle g_Item;

		// Token: 0x04000816 RID: 2070
		public static PatchAssetBundle g_NeigongBundle;

		// Token: 0x04000817 RID: 2071
		public static PatchAssetBundle g_LoadBackground;

		// Token: 0x04000818 RID: 2072
		public static PatchAssetBundle g_Achievement;

		// Token: 0x04000819 RID: 2073
		public static AssetBundle g_TextFiles;

		// Token: 0x0400081A RID: 2074
		public static AssetBundle g_SmallGame;

		// Token: 0x0400081B RID: 2075
		public static AssetBundle g_EffectText;

		// Token: 0x0400081C RID: 2076
		public static AssetBundle g_Movie;

		// Token: 0x0400081D RID: 2077
		public static AssetBundle g_EventCubeData;

		// Token: 0x0400081E RID: 2078
		public static AssetBundle g_MouseCubeData;

		// Token: 0x0400081F RID: 2079
		public static AssetBundle g_BigMapNodeData;

		// Token: 0x04000820 RID: 2080
		public static BattleControl g_BattleControl;

		// Token: 0x04000821 RID: 2081
		public static string g_strDataPathToApplicationPath;

		// Token: 0x04000822 RID: 2082
		public static string g_strAssetDataPath;

		// Token: 0x04000823 RID: 2083
		public static int printCount;

		// Token: 0x04000824 RID: 2084
		private static List<string> sceneList = new List<string>();

		// Token: 0x04000825 RID: 2085
		public static List<UILayer> layerHideList = new List<UILayer>();

		// Token: 0x04000826 RID: 2086
		public static List<UILayer> layerDeleteList = new List<UILayer>();

		// Token: 0x04000827 RID: 2087
		public static InputManager g_InputManager = new InputManager();

		// Token: 0x04000828 RID: 2088
		public static CharacterData mod_CurrentCharacterData = new CharacterData();

		// Token: 0x02000193 RID: 403
		// (Invoke) Token: 0x06000861 RID: 2145
		public delegate void LoadSenceOnFinish();
	}
}
