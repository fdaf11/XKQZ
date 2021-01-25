using System;
using System.Collections;
using System.IO;
using Heluo.Wulin.UI;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000121 RID: 289
	public class BigMapController : MonoBehaviour
	{
		// Token: 0x060005DC RID: 1500 RVA: 0x00042BA0 File Offset: 0x00040DA0
		private void Start()
		{
			if (BigMapController.m_Instance == null)
			{
				BigMapController.m_Instance = this;
			}
			this.createBigMapNode();
			this.createBigMapBillBoard();
			this.createBigMapPlayer();
			if (YoungHeroTime.m_instance != null)
			{
				YoungHeroTime.m_instance.OnBigMapEventTrigger = null;
				YoungHeroTime instance = YoungHeroTime.m_instance;
				instance.OnBigMapEventTrigger = (Action)Delegate.Combine(instance.OnBigMapEventTrigger, new Action(this.OnBigMapEventTrigger));
			}
			GameGlobal.m_bBigMapMode = true;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00042C1C File Offset: 0x00040E1C
		private void createBigMapNode()
		{
			string text = Game.g_strDataPathToApplicationPath + "Mods/" + GameGlobal.m_strVersion + "/Config/bigmapnodedata/bigmapnodedata.txt";
			if (File.Exists(text))
			{
				try
				{
					StreamReader streamReader = new StreamReader(text);
					JsonReader jsonReader = new JsonReader(streamReader);
					streamReader.Close();
					foreach (BigMapNodeData bigMapNodeData in jsonReader.Deserialize<BigMapNodeData[]>())
					{
						bigMapNodeData.ToGame(true);
					}
				}
				catch
				{
					Debug.LogError("散檔讀取失敗 !! ( " + text + " )");
				}
			}
			else
			{
				TextAsset textAsset = Game.g_BigMapNodeData.Load("bigmapnodedata") as TextAsset;
				JsonReader jsonReader2 = new JsonReader(textAsset.text);
				foreach (BigMapNodeData bigMapNodeData2 in jsonReader2.Deserialize<BigMapNodeData[]>())
				{
					bigMapNodeData2.ToGame(true);
				}
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00042D24 File Offset: 0x00040F24
		private void createBigMapBillBoard()
		{
			GameObject gameObject = Game.g_cForm.Load("BigMapBillBoardCreater") as GameObject;
			Object.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00042D64 File Offset: 0x00040F64
		private void createBigMapPlayer()
		{
			GameObject gameObject = Resources.Load("BigMap/BigMapPlayerController") as GameObject;
			this.m_Player = (Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject);
			this.m_Player.name = "Xyg_NEW_Xuan";
			this.m_iCrtModelNum = GameGlobal.m_iBigMapModel;
			this.createPlayerModel(this.m_iCrtModelNum);
			this.setToPos();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00042DCC File Offset: 0x00040FCC
		private void setToPos()
		{
			if (this.EditMode)
			{
				this.setBigMapPlayer(base.gameObject.transform.position, Vector3.zero);
				return;
			}
			if (!Save.m_Instance.bLoad && BigMapNode.BigMapNodeList.ContainsKey(Game.PrvSceneName))
			{
				BigMapNode bigMapNode = BigMapNode.BigMapNodeList[Game.PrvSceneName];
				if (bigMapNode != null)
				{
					GameGlobal.m_TransferPos = bigMapNode.OutPosition;
					return;
				}
				GameDebugTool.Log("無此地圖節點  ID : " + Game.PrvSceneName);
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00042E60 File Offset: 0x00041060
		public void createPlayerModel(int idx = 0)
		{
			if (idx < 0 || idx >= this.m_ModelName.Length)
			{
				return;
			}
			this.m_iCrtModelNum = idx;
			GameObject gameObject = Game.g_ModelBundle.Load(this.m_ModelName[idx] + "_ModelPrefab") as GameObject;
			if (gameObject == null)
			{
				return;
			}
			this.DestoryOldModel();
			GameObject gameObject2 = Object.Instantiate(gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
			gameObject2.transform.parent = this.m_Player.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localEulerAngles = Vector3.zero;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00005659 File Offset: 0x00003859
		public void DestoryOldModel()
		{
			PlayerController.m_Instance.DestoryModel();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00005665 File Offset: 0x00003865
		private void setBigMapPlayer(Vector3 pos, Vector3 angel)
		{
			PlayerController.m_Instance.Init();
			this.m_Player.transform.position = pos;
			this.m_Player.transform.localEulerAngles = angel;
			PlayerController.m_Instance.ReSetMoveDate();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00042F48 File Offset: 0x00041148
		private void Update()
		{
			if (this.m_Player == null)
			{
				return;
			}
			if (GameGlobal.m_bCFormOpen || GameGlobal.m_bLoading || GameGlobal.m_bPlayerTalk)
			{
				return;
			}
			if (PlayerController.m_Instance.GetIsMoveState())
			{
				this.updateDate();
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00042F9C File Offset: 0x0004119C
		private void updateDate()
		{
			this.movSec += Time.deltaTime;
			if (this.movSec >= this.addDaySec)
			{
				this.movSec -= this.addDaySec;
				BigMapSaveData.Singleton.UseDate++;
				GameDebugTool.Log("走 " + BigMapSaveData.Singleton.UseDate + "天", Color.yellow);
				if (YoungHeroTime.m_instance != null)
				{
					YoungHeroTime.m_instance.AddDay(1);
				}
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00043034 File Offset: 0x00041234
		public void OnBigMapEventTrigger()
		{
			if (this.m_BigMapEventLock)
			{
				return;
			}
			this.m_BigMapEventLock = true;
			QuestNode questNode = MissionStatus.m_instance.RandomBigMapEvent();
			if (questNode == null)
			{
				GameDebugTool.Log("無事件觸發");
			}
			else
			{
				Game.UI.Get<UITalk>().SetTalkData(questNode.m_strGetManager);
				GameGlobal.m_TransferPos = PlayerController.m_Instance.transform.position;
				Debug.Log("紀錄大地圖位置 : " + GameGlobal.m_TransferPos);
				GameDebugTool.Log("大地圖事件觸發 : " + questNode.m_strQuestID, Color.red);
			}
			this.m_BigMapEventLock = false;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0000569D File Offset: 0x0000389D
		public void BigMapGoToBattle(int BattleID)
		{
			base.StartCoroutine(this.GoToBattle(BattleID));
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000430D8 File Offset: 0x000412D8
		private IEnumerator GoToBattle(int BattleID)
		{
			GameGlobal.m_TransferPos = PlayerController.m_Instance.transform.position;
			ScreenOverlay SO = null;
			float fPos = 0f;
			float fTime = 0.5f;
			GameObject goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			if (goMainCamera == null)
			{
				goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			while (fPos < fTime)
			{
				if (goMainCamera == null)
				{
					break;
				}
				if (goMainCamera.GetComponent<ScreenOverlay>() == null)
				{
					break;
				}
				SO = goMainCamera.GetComponent<ScreenOverlay>();
				if (SO != null)
				{
					SO.intensity = (fTime - fPos) / fTime;
					SO.enabled = true;
				}
				fPos += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			if (goMainCamera != null)
			{
				SO.enabled = false;
			}
			Game.g_BattleControl.StartBattle(BattleID);
			yield break;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000056AD File Offset: 0x000038AD
		public void OnDestroy()
		{
			YoungHeroTime instance = YoungHeroTime.m_instance;
			instance.OnBigMapEventTrigger = (Action)Delegate.Remove(instance.OnBigMapEventTrigger, new Action(this.OnBigMapEventTrigger));
			GameGlobal.m_bBigMapMode = false;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000430FC File Offset: 0x000412FC
		public void ChangeModel()
		{
			bool flag = false;
			while (!flag)
			{
				this.m_iCrtModelNum++;
				if (this.m_iCrtModelNum >= this.m_ModelName.Length)
				{
					this.m_iCrtModelNum = 0;
				}
				if (TeamStatus.m_Instance.CheckTeamMember(this.m_ModelId[this.m_iCrtModelNum]))
				{
					flag = true;
					GameGlobal.m_iBigMapModel = this.m_iCrtModelNum;
					this.createPlayerModel(this.m_iCrtModelNum);
				}
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00043174 File Offset: 0x00041374
		[ContextMenu("取得地域ID")]
		public static string GetRegional()
		{
			if (BigMapNode.BigMapNodeList.Count == 0)
			{
				return "0";
			}
			string text = "0";
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			float num = float.MaxValue;
			if (PlayerController.m_Instance != null)
			{
				zero..ctor(PlayerController.m_Instance.transform.position.x, PlayerController.m_Instance.transform.position.z);
			}
			for (int i = 0; i < Game.MapID.m_MapIDNodeList.Count; i++)
			{
				MapIDNode mapIDNode = Game.MapID.m_MapIDNodeList[i];
				if (BigMapNode.BigMapNodeList.ContainsKey(mapIDNode.m_strMapID))
				{
					BigMapNode bigMapNode = BigMapNode.BigMapNodeList[mapIDNode.m_strMapID];
					zero2..ctor(bigMapNode.transform.position.x, bigMapNode.transform.position.z);
					float num2 = Vector2.Distance(zero, zero2);
					if (num2 <= mapIDNode.Range && num2 <= num)
					{
						num = num2;
						text = mapIDNode.m_strMapID;
					}
				}
			}
			Debug.Log("所在區域為 : " + text);
			return text;
		}

		// Token: 0x0400065A RID: 1626
		public static BigMapController m_Instance;

		// Token: 0x0400065B RID: 1627
		public static GameObject BigMapCircle;

		// Token: 0x0400065C RID: 1628
		private string[] m_ModelName = new string[]
		{
			"Horse_Xyg_NEW_Xuan",
			"Horse_Xyg_NEW_Gingi",
			"Horse_xiaoyao_purple"
		};

		// Token: 0x0400065D RID: 1629
		private int[] m_ModelId = new int[]
		{
			210001,
			210002,
			200000
		};

		// Token: 0x0400065E RID: 1630
		private int m_iCrtModelNum;

		// Token: 0x0400065F RID: 1631
		private float addDaySec = 1f;

		// Token: 0x04000660 RID: 1632
		private float movSec;

		// Token: 0x04000661 RID: 1633
		private GameObject m_Player;

		// Token: 0x04000662 RID: 1634
		public bool EditMode;

		// Token: 0x04000663 RID: 1635
		private bool m_BigMapEventLock;
	}
}
