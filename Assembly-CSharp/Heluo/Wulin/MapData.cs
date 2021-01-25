using System;
using System.Collections;
using System.Collections.Generic;
using SSAA;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000276 RID: 630
	public class MapData : MonoBehaviour
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x00009080 File Offset: 0x00007280
		private void Awake()
		{
			MapData.m_instance = this;
			this.go_Sky = GameObject.FindGameObjectWithTag("WorldTime");
			this.go_Npc = GameObject.FindGameObjectWithTag("MapNpc");
			this.go_TreasureBox = GameObject.FindGameObjectWithTag("MapTresurbox");
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0005ED24 File Offset: 0x0005CF24
		private void Start()
		{
			TeamStatus instance = TeamStatus.m_Instance;
			instance.AddTeammate = (Action)Delegate.Combine(instance.AddTeammate, new Action(this.FadeIn));
			MissionStatus.m_instance.SetNotify(new Action(this.CheckNpcSpeicalList));
			this.Initial();
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x000090B8 File Offset: 0x000072B8
		public void StartDelayLoading()
		{
			base.StartCoroutine(this._StartDelayLoading());
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0005ED74 File Offset: 0x0005CF74
		public List<NpcIsFought> GetSaveNpcIsFought()
		{
			List<NpcIsFought> list = new List<NpcIsFought>();
			for (int i = 0; i < this.m_NpcIsFoughtList.Count; i++)
			{
				list.Add(this.m_NpcIsFoughtList[i].Clone());
			}
			return list;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0005EDBC File Offset: 0x0005CFBC
		public void loadNpcIsFought(List<NpcIsFought> LoadNpcIsFoughtList)
		{
			this.m_NpcIsFoughtList.Clear();
			if (LoadNpcIsFoughtList == null)
			{
				return;
			}
			for (int i = 0; i < LoadNpcIsFoughtList.Count; i++)
			{
				this.m_NpcIsFoughtList.Add(LoadNpcIsFoughtList[i].Clone());
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000090C7 File Offset: 0x000072C7
		private void FadeIn()
		{
			if (GameGlobal.m_bMovie)
			{
				return;
			}
			base.StartCoroutine(this._FadeIn());
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0005EE0C File Offset: 0x0005D00C
		private IEnumerator _FadeIn()
		{
			ScreenOverlay SO = null;
			float fPos = 0f;
			float fTime = 1f;
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
					SO.intensity = fPos / fTime;
					SO.enabled = true;
				}
				fPos += Time.deltaTime;
				yield return null;
			}
			if (goMainCamera == null)
			{
				goMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			if (goMainCamera != null)
			{
				if (SO == null)
				{
					SO = goMainCamera.GetComponent<ScreenOverlay>();
				}
				SO.intensity = 1f;
				SO.enabled = false;
			}
			yield break;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0005EE20 File Offset: 0x0005D020
		public void CheckNpcSpeicalList()
		{
			if (this.m_NpcConductAlwaysList.Count <= 0)
			{
				return;
			}
			if (this.m_SpecialNpcList.Count <= 0)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (NpcConductNode npcConductNode in this.m_NpcConductAlwaysList)
			{
				if (this.m_SpecialNpcList.Contains(npcConductNode.m_iNpcID))
				{
					NpcCollider component = npcConductNode.m_go_Npc.GetComponent<NpcCollider>();
					if (component != null)
					{
						if (component.m_bSpecialOpen)
						{
							bool flag = component.CheckNpcOpen();
							if (flag)
							{
								if (!npcConductNode.m_go_Npc.activeSelf)
								{
									npcConductNode.m_go_Npc.SetActive(true);
								}
							}
							else if (npcConductNode.m_go_Npc.activeSelf)
							{
								npcConductNode.m_go_Npc.SetActive(false);
							}
							if (component.AllConditionOver)
							{
								list.Add(npcConductNode.m_iNpcID);
							}
						}
					}
					else
					{
						Debug.LogError("這個Frame  " + npcConductNode.m_go_Npc.name + "  還沒有NPC還沒產生完成 。");
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.DeleteAlwaysNpc(list[i], 0);
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0005EF88 File Offset: 0x0005D188
		public NpcIsFought GetNpcIsFoughtNode(string MapName, string NpcName)
		{
			for (int i = 0; i < this.m_NpcIsFoughtList.Count; i++)
			{
				bool flag = this.m_NpcIsFoughtList[i].m_MapName.Equals(MapName);
				bool flag2 = this.m_NpcIsFoughtList[i].m_Npc.Equals(NpcName);
				if (flag && flag2)
				{
					return this.m_NpcIsFoughtList[i];
				}
			}
			return null;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0005EFFC File Offset: 0x0005D1FC
		public NpcIsFought SetNpcIsFoughtRound(string MapName, string NpcName, int AddRound)
		{
			for (int i = 0; i < this.m_NpcIsFoughtList.Count; i++)
			{
				bool flag = this.m_NpcIsFoughtList[i].m_MapName.Equals(MapName);
				bool flag2 = this.m_NpcIsFoughtList[i].m_Npc.Equals(NpcName);
				if (flag && flag2)
				{
					this.m_NpcIsFoughtList[i].ReSetRound = YoungHeroTime.m_instance.AddCheckRound(AddRound);
					return this.m_NpcIsFoughtList[i];
				}
			}
			return null;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0005F08C File Offset: 0x0005D28C
		public bool CheckNpcIsFoughtName(NpcIsFought _NIF)
		{
			for (int i = 0; i < this.m_NpcIsFoughtList.Count; i++)
			{
				bool flag = this.m_NpcIsFoughtList[i].m_MapName.Equals(_NIF.m_MapName);
				bool flag2 = this.m_NpcIsFoughtList[i].m_Npc.Equals(_NIF.m_Npc);
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0005F100 File Offset: 0x0005D300
		public GameObject LoadNpcInNewLoading(string name)
		{
			NpcConductNode npcConductNode = null;
			bool flag = false;
			for (int i = 0; i < this.m_NpcConductDelayLoadList.Count; i++)
			{
				npcConductNode = this.m_NpcConductDelayLoadList[i];
				if (npcConductNode.m_iNpcID.ToString().Equals(name))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return null;
			}
			if (npcConductNode.m_ConductNodeList.Count < 1)
			{
				return null;
			}
			ConductNode conductNode = npcConductNode.m_ConductNodeList[0];
			GameObject go = Game.g_ModelBundle.Load(conductNode.m_strModelName + "_ModelPrefab") as GameObject;
			GameObject gameObject = this.BuildModel(npcConductNode, go);
			this.m_NpcConductDelayLoadList.Remove(npcConductNode);
			GameSetting.m_Instance.GetComponent<MovieEventTrigger>().AddNpcToMovieList(gameObject);
			return gameObject;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0005F1CC File Offset: 0x0005D3CC
		private GameObject BuildModel(NpcConductNode npcConductNode, GameObject go)
		{
			if (npcConductNode == null)
			{
				Debug.Log("Npc 行為 null");
				return null;
			}
			if (npcConductNode.m_go_Npc != null && npcConductNode.m_go_Npc.animation != null)
			{
				Debug.Log(string.Format("已經有了 {0} 的 Npc Model 可能 Movie 先讀進來了 ", npcConductNode.m_iNpcID));
				return npcConductNode.m_go_Npc;
			}
			if (go == null)
			{
				Debug.Log(string.Format("找不到 {0} 的 Npc Model", npcConductNode.m_iNpcID));
				return null;
			}
			if (npcConductNode.m_ConductNodeList.Count < 1)
			{
				Debug.Log(string.Format("沒有 {0} 的 Npc 行為表 ", npcConductNode.m_iNpcID));
				return null;
			}
			ConductNode conductNode = npcConductNode.m_ConductNodeList[0];
			GameObject gameObject;
			if (npcConductNode.m_go_Npc != null)
			{
				gameObject = (Object.Instantiate(go, npcConductNode.m_go_Npc.transform.position, Quaternion.identity) as GameObject);
			}
			else
			{
				gameObject = (Object.Instantiate(go) as GameObject);
			}
			MeleeWeaponTrail[] array = gameObject.GetComponents<MeleeWeaponTrail>();
			foreach (MeleeWeaponTrail meleeWeaponTrail in array)
			{
				meleeWeaponTrail.Emit = false;
			}
			array = gameObject.GetComponentsInChildren<MeleeWeaponTrail>();
			foreach (MeleeWeaponTrail meleeWeaponTrail2 in array)
			{
				meleeWeaponTrail2.Emit = false;
			}
			gameObject.name = npcConductNode.m_iNpcID.ToString();
			gameObject.tag = "Npc";
			gameObject.layer = 13;
			if (gameObject.GetComponent<NpcCollider>() != null)
			{
				gameObject.GetComponent<NpcCollider>().m_strModelName = conductNode.m_strModelName;
				gameObject.GetComponent<NpcCollider>().SetNpcData(conductNode);
			}
			else
			{
				Debug.LogError(gameObject.name + " model name = " + conductNode.m_strModelName + " 沒有 NpcCollider 請檢查後再包檔");
			}
			if (gameObject.GetComponent<AudioListener>() != null)
			{
				gameObject.GetComponent<AudioListener>().enabled = false;
			}
			if (gameObject.GetComponent<PlayerController>() != null)
			{
				gameObject.GetComponent<PlayerController>().enabled = false;
			}
			gameObject.transform.parent = this.go_Npc.transform;
			if (npcConductNode.m_go_Npc != null)
			{
				gameObject.transform.parent = npcConductNode.m_go_Npc.transform.parent;
				gameObject.transform.localPosition = npcConductNode.m_go_Npc.transform.localPosition;
				gameObject.transform.localEulerAngles = npcConductNode.m_go_Npc.transform.localEulerAngles;
				gameObject.tag = npcConductNode.m_go_Npc.tag;
				npcConductNode.m_go_Npc.name = "delete";
				npcConductNode.m_go_Npc.transform.parent = null;
				Object.Destroy(npcConductNode.m_go_Npc);
			}
			npcConductNode.m_go_Npc = gameObject;
			return gameObject;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0005F4AC File Offset: 0x0005D6AC
		private IEnumerator _StartDelayLoading()
		{
			yield return null;
			while (this.m_NpcConductDelayLoadList.Count > 0)
			{
				this.SortPos();
				yield return null;
				if (this.m_NpcConductDelayLoadList.Count <= 0)
				{
					yield break;
				}
				NpcConductNode npcConductNode = this.m_NpcConductDelayLoadList[0];
				this.m_NpcConductDelayLoadList.RemoveAt(0);
				if (npcConductNode != null)
				{
					ConductNode conductNode = null;
					if (npcConductNode.m_iType == 0)
					{
						conductNode = npcConductNode.m_ConductNodeList[0];
					}
					else
					{
						conductNode = npcConductNode.GetConductNode(npcConductNode.m_fClickTime);
					}
					if (conductNode != null)
					{
						if (Game.g_ModelBundle != null)
						{
							AssetBundle ab = Game.g_ModelBundle.GetAssetBundle(conductNode.m_strModelName + "_ModelPrefab");
							if (ab == null)
							{
								Debug.LogError(string.Format("Not have ID = {0} Model = {1} ", npcConductNode.m_iNpcID, conductNode.m_strModelName));
							}
							else
							{
								AssetBundleRequest abr = ab.LoadAsync(conductNode.m_strModelName + "_ModelPrefab", typeof(GameObject));
								yield return abr;
								if (ab == null)
								{
									yield break;
								}
								if (abr == null)
								{
									yield break;
								}
								if (abr.asset == null)
								{
									yield break;
								}
								GameObject go = abr.asset as GameObject;
								if (npcConductNode != null)
								{
									this.BuildModel(npcConductNode, go);
									yield return null;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0005F4C8 File Offset: 0x0005D6C8
		public void Initial()
		{
			for (int i = 0; i < this.m_NpcConductAlwaysList.Count; i++)
			{
				Object.Destroy(this.m_NpcConductAlwaysList[i].m_go_Npc);
			}
			for (int i = 0; i < this.m_NpcConductDelayLoadList.Count; i++)
			{
				Object.Destroy(this.m_NpcConductDelayLoadList[i].m_go_Npc);
			}
			this.m_NpcConductAlwaysList.Clear();
			this.m_NpcConductDelayLoadList.Clear();
			for (int i = 0; i < this.TreasureBoxList.Count; i++)
			{
				Object.Destroy(this.TreasureBoxList[i].m_go);
			}
			this.TreasureBoxList.Clear();
			this.m_SpecialNpcList.Clear();
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0000264F File Offset: 0x0000084F
		public void SortNpcConductList()
		{
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0005F598 File Offset: 0x0005D798
		public int LoadMapIconNpcConduct(string strNewSceneName)
		{
			Game.MapIcon.LoadMapIconFile(strNewSceneName);
			Game.NpcConduct.LoadNpcConductFile(strNewSceneName);
			return Game.NpcConduct.m_NpcConductNodeList.Count;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0005F5CC File Offset: 0x0005D7CC
		public int LoadTreasureBox(string strNewSceneName)
		{
			List<TreasureBoxNode> treasureBoxList = Game.TreasureBox.GetTreasureBoxList(strNewSceneName);
			if (treasureBoxList == null)
			{
				return 0;
			}
			return treasureBoxList.Count;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000090E1 File Offset: 0x000072E1
		public void LoadNpcConductPos(int index)
		{
			this.CheckMapNpc();
			if (GameGlobal.m_bNewLoading)
			{
				this.LoadNpcConductPosGameObject(index);
			}
			else
			{
				this.LoadNpcConductPos(index, 0f);
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0000910B File Offset: 0x0000730B
		public void LoadTreasureBoxPos(string SceneName, int index)
		{
			this.LoadTreaureBoxPosGameObject(SceneName, index);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0005F5F8 File Offset: 0x0005D7F8
		public void LoadNpcConductPosGameObject(int index)
		{
			NpcConductNode npcConductNode = Game.NpcConduct.m_NpcConductNodeList[index];
			int num = 0;
			if (num < npcConductNode.m_ConductNodeList.Count)
			{
				ConductNode conductNode = npcConductNode.m_ConductNodeList[num];
				GameObject gameObject = new GameObject(npcConductNode.m_iNpcID.ToString());
				gameObject.tag = "Npc";
				gameObject.layer = 13;
				gameObject.transform.parent = this.go_Npc.transform;
				MapNpcDataNode mapNpcDataNode = Game.MapIcon.GetMapNpcDataNode(conductNode.m_ConductID);
				if (mapNpcDataNode != null)
				{
					if (mapNpcDataNode.m_PointNodeList.Count > 0)
					{
						gameObject.transform.localPosition = new Vector3(mapNpcDataNode.m_PointNodeList[0].m_fX, mapNpcDataNode.m_PointNodeList[0].m_fY, mapNpcDataNode.m_PointNodeList[0].m_fZ);
						gameObject.transform.localEulerAngles = new Vector3(0f, (float)mapNpcDataNode.m_PointNodeList[0].m_iDirAngle, 0f);
					}
					else
					{
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localEulerAngles = Vector3.zero;
					}
					if (mapNpcDataNode.m_iCheakPlayer == 1)
					{
						gameObject.tag = "BattleNpc";
					}
					if (mapNpcDataNode.m_iNpcType == 2)
					{
						gameObject.tag = "BattleNpc";
					}
				}
				else
				{
					gameObject.transform.localPosition = Vector3.zero;
				}
				npcConductNode.m_go_Npc = gameObject;
				this.m_NpcConductDelayLoadList.Add(npcConductNode);
			}
			this.m_NpcConductAlwaysList.Add(npcConductNode);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
		private void LoadTreaureBoxPosGameObject(string SceneName, int index)
		{
			if (this.go_TreasureBox == null)
			{
				this.go_TreasureBox = GameObject.FindGameObjectWithTag("MapTresurbox");
				if (this.go_TreasureBox == null)
				{
					this.go_TreasureBox = new GameObject("MapTreasureBox");
					this.go_TreasureBox.transform.position = new Vector3(0f, 0f, 0f);
					this.go_TreasureBox.tag = "MapTresurbox";
					Battle.AddMainGameList(this.go_TreasureBox);
				}
				else
				{
					Battle.AddMainGameList(this.go_TreasureBox);
				}
			}
			TreasureBoxNode treasureBox = Game.TreasureBox.GetTreasureBox(SceneName, index);
			string text = "Artist/Objects/Interactive Object/Chest/";
			if (TeamStatus.m_Instance.m_TreasureBoxList.Contains(treasureBox.m_strBoxID))
			{
				return;
			}
			GameObject gameObject = Resources.Load(text + treasureBox.m_ModelName) as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				gameObject2.name = treasureBox.m_strBoxID.ToString();
				gameObject2.tag = "TreasureBox";
				gameObject2.layer = LayerMask.NameToLayer("Item");
				gameObject2.transform.parent = this.go_TreasureBox.transform;
				gameObject2.transform.position = new Vector3(treasureBox.m_fPosX, treasureBox.m_fPosY, treasureBox.m_fPosZ);
				gameObject2.transform.localEulerAngles = new Vector3(treasureBox.m_fRotX, treasureBox.m_fRotY, treasureBox.m_fRotZ);
				if (gameObject2.GetComponent<BoxCollider>() == null)
				{
					BoxCollider boxCollider = gameObject2.AddComponent<BoxCollider>();
					if (treasureBox.m_ModelName == "AniChest04")
					{
						boxCollider.size = new Vector3(6.5f, 3f, 4f);
						boxCollider.center = new Vector3(0f, 1.23f, 0f);
					}
				}
				if (gameObject2.GetComponent<TreasureBox>() != null)
				{
					gameObject2.GetComponent<TreasureBox>().m_strBoxID = treasureBox.m_strBoxID;
					gameObject2.GetComponent<TreasureBox>().SetTreasureBoxNode(treasureBox);
				}
				else
				{
					TreasureBox treasureBox2 = gameObject2.AddComponent<TreasureBox>();
					treasureBox2.m_strBoxID = treasureBox.m_strBoxID;
					treasureBox2.SetTreasureBoxNode(treasureBox);
				}
				treasureBox.m_go = gameObject2;
				this.TreasureBoxList.Add(treasureBox);
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0005F9EC File Offset: 0x0005DBEC
		public void CheckMapNpc()
		{
			if (this.go_Npc == null)
			{
				this.go_Npc = GameObject.FindGameObjectWithTag("MapNpc");
				if (this.go_Npc == null)
				{
					this.go_Npc = new GameObject("MapNpc");
					this.go_Npc.transform.position = new Vector3(0f, 0f, 0f);
					this.go_Npc.tag = "MapNpc";
					Battle.AddMainGameList(this.go_Npc);
				}
			}
			else
			{
				Battle.AddMainGameList(this.go_Npc);
			}
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0005FA8C File Offset: 0x0005DC8C
		public void LoadNpcConductPos(int index, float fTime)
		{
			NpcConductNode npcConductNode = Game.NpcConduct.m_NpcConductNodeList[index];
			int num = 0;
			if (num < npcConductNode.m_ConductNodeList.Count)
			{
				ConductNode conductNode = npcConductNode.m_ConductNodeList[num];
				GameObject go = Game.g_ModelBundle.Load(conductNode.m_strModelName + "_ModelPrefab") as GameObject;
				this.BuildModel(npcConductNode, go);
			}
			this.m_NpcConductAlwaysList.Add(npcConductNode);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0005FB0C File Offset: 0x0005DD0C
		public void SetInOutSide()
		{
			if (this.go_Sky == null)
			{
				return;
			}
			Debug.Log(GameGlobal.m_bHouseInside);
			if (GameGlobal.m_bHouseInside)
			{
				if (this.go_Sky.GetComponent<TOD_Time>() != null)
				{
					this.go_Sky.GetComponent<TOD_Time>().m_bInside = true;
				}
				if (this.go_Sky.GetComponent<TOD_Sky>() != null)
				{
					this.go_Sky.GetComponent<TOD_Sky>().m_bDontUpdateAmbient = true;
				}
				if (this.go_Sky.GetComponentInChildren<Light>() != null)
				{
					this.go_Sky.GetComponentInChildren<Light>().enabled = false;
				}
				if (this.go_Sky.GetComponentInChildren<Projector>() != null)
				{
					this.go_Sky.GetComponentInChildren<Projector>().enabled = false;
				}
				RenderSettings.ambientLight = GameGlobal.m_InSideAbLi;
				GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
				if (gameObject != null && gameObject.camera != null)
				{
					if (gameObject.GetComponent<SSAOEffectDepthCutoff>() != null)
					{
						this.m_fOrigOccInt = gameObject.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity;
						gameObject.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity = 1.6f;
					}
					Debug.Log("SetInOutSide MainCamera Set CameraClearFlags.SolidColor");
					gameObject.camera.clearFlags = 2;
					if (Application.loadedLevelName == "M0040_01")
					{
						gameObject.camera.farClipPlane = 100f;
					}
					else
					{
						gameObject.camera.farClipPlane = 30f;
					}
				}
				else
				{
					Debug.LogWarning("SetInOutSide MainCamera no Found");
					if (Camera.main != null)
					{
						if (Camera.main.GetComponent<SSAOEffectDepthCutoff>() != null)
						{
							this.m_fOrigOccInt = Camera.main.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity;
							Camera.main.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity = 1.6f;
						}
						Debug.Log("SetInOutSide Camera.main Set CameraClearFlags.SolidColor");
						Camera.main.clearFlags = 2;
						Camera.main.farClipPlane = 30f;
					}
					else
					{
						Debug.LogWarning("Camera.main == null");
					}
				}
			}
			else
			{
				if (this.go_Sky.GetComponent<TOD_Time>() != null)
				{
					this.go_Sky.GetComponent<TOD_Time>().m_bInside = false;
				}
				if (this.go_Sky.GetComponent<TOD_Sky>() != null)
				{
					this.go_Sky.GetComponent<TOD_Sky>().m_bDontUpdateAmbient = false;
				}
				if (this.go_Sky.GetComponentInChildren<Light>() != null)
				{
					this.go_Sky.GetComponentInChildren<Light>().enabled = true;
				}
				if (this.go_Sky.GetComponentInChildren<Projector>() != null)
				{
					this.go_Sky.GetComponentInChildren<Projector>().enabled = true;
				}
				RenderSettings.ambientLight = this.go_Sky.GetComponent<TOD_Sky>().AmbientColor;
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
				if (gameObject2 != null && gameObject2.camera != null)
				{
					if (gameObject2.GetComponent<SSAOEffectDepthCutoff>() != null)
					{
						gameObject2.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity = this.m_fOrigOccInt;
					}
					Debug.Log("SetInOutSide MainCamera Set CameraClearFlags.Skybox");
					gameObject2.camera.clearFlags = 1;
				}
				else
				{
					Debug.LogWarning("SetInOutSide MainCamera no Found");
					if (Camera.main != null)
					{
						if (Camera.main.GetComponent<SSAOEffectDepthCutoff>() != null)
						{
							Camera.main.GetComponent<SSAOEffectDepthCutoff>().m_OcclusionIntensity = this.m_fOrigOccInt;
						}
						Debug.Log("SetInOutSide Camera.main Set CameraClearFlags.Skybox");
						Camera.main.clearFlags = 1;
					}
					else
					{
						Debug.LogWarning("Camera.main == null");
					}
				}
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0005FEA8 File Offset: 0x0005E0A8
		public void SetTime(float fTime)
		{
			if (this.go_Sky == null)
			{
				return;
			}
			if (this.go_Sky.GetComponent<TOD_Sky>() == null)
			{
				return;
			}
			Debug.Log("MapData:SetTime " + fTime.ToString());
			this.go_Sky.GetComponent<TOD_Sky>().Cycle.Hour = fTime;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0005FF0C File Offset: 0x0005E10C
		public void SetPos()
		{
			Debug.Log("MapData:SetPos");
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			if (array.Length > 1)
			{
				Debug.Log(array.Length.ToString() + " Player damn");
				foreach (GameObject gameObject in array)
				{
					if (!(gameObject == null))
					{
						gameObject.transform.localPosition = GameGlobal.m_TransferPos;
						gameObject.transform.localEulerAngles = new Vector3(GameGlobal.m_fDir, 0f);
						if (gameObject.GetComponent<PlayerController>() != null)
						{
							Debug.Log("PlayerController:ReSetMoveDate");
							gameObject.GetComponent<PlayerController>().ReSetMoveDate();
						}
					}
				}
			}
			else if (array.Length == 1)
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
				if (gameObject2 == null)
				{
					return;
				}
				gameObject2.transform.localPosition = GameGlobal.m_TransferPos;
				gameObject2.transform.localEulerAngles = new Vector3(0f, GameGlobal.m_fDir, 0f);
				if (gameObject2.GetComponent<PlayerController>() != null)
				{
					Debug.Log("PlayerController:ReSetMoveDate : " + GameGlobal.m_TransferPos);
					gameObject2.GetComponent<PlayerController>().ReSetMoveDate();
				}
			}
			else
			{
				Debug.Log("MapData:SetPos no player");
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0006006C File Offset: 0x0005E26C
		public void SortPos()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject == null)
			{
				return;
			}
			if (this.m_NpcConductDelayLoadList.Count > 2)
			{
				MapData.s_vPos = gameObject.transform.localPosition;
				this.m_NpcConductDelayLoadList.Sort(new Comparison<NpcConductNode>(MapData.CompareFunc));
			}
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000600CC File Offset: 0x0005E2CC
		public void SetCameraFocus()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
			if (gameObject2 != null && gameObject2.camera != null)
			{
				if (gameObject2.GetComponent<DepthOfFieldScatter>() != null)
				{
					gameObject2.GetComponent<DepthOfFieldScatter>().SetDoFSTarget(gameObject.transform);
				}
				if (gameObject2.GetComponent<OrbitCam>() != null)
				{
					gameObject2.GetComponent<OrbitCam>().Target = gameObject.transform;
					if (Save.m_Instance != null && (Save.m_Instance.bLoad || GameGlobal.m_bSetCaramaDRT))
					{
						GameGlobal.m_bSetCaramaDRT = false;
						gameObject2.GetComponent<OrbitCam>().SnapTo(this._CameraSaveDateNode.m_fDistance, this._CameraSaveDateNode.m_fRotation, this._CameraSaveDateNode.m_fTilt);
					}
				}
				if (gameObject2.GetComponent<AntialiasingAsPostEffect>() != null)
				{
					if (QualitySettings.antiAliasing == 0)
					{
						gameObject2.GetComponent<AntialiasingAsPostEffect>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<AntialiasingAsPostEffect>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<Bloom>() != null)
				{
					if (GameGlobal.m_bBloom)
					{
						gameObject2.GetComponent<Bloom>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<Bloom>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<ColorCorrectionCurves>() != null)
				{
					if (!GameGlobal.m_bLensEffects)
					{
						gameObject2.GetComponent<ColorCorrectionCurves>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<ColorCorrectionCurves>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<DeluxeTonemapper>() != null)
				{
					if (!GameGlobal.m_bLensEffects)
					{
						gameObject2.GetComponent<DeluxeTonemapper>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<DeluxeTonemapper>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<SSAOEffectDepthCutoff>() != null)
				{
					if (!GameGlobal.m_bSSAO)
					{
						gameObject2.GetComponent<SSAOEffectDepthCutoff>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<SSAOEffectDepthCutoff>().enabled = true;
					}
				}
				if (gameObject2.GetComponent<SuperSampling_SSAA>() != null)
				{
					if (!GameGlobal.m_bSSAA)
					{
						gameObject2.GetComponent<SuperSampling_SSAA>().enabled = false;
					}
					else
					{
						gameObject2.GetComponent<SuperSampling_SSAA>().enabled = true;
						internal_SSAA.ChangeScale(GameGlobal.m_fSSAA);
					}
				}
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00060314 File Offset: 0x0005E514
		private static int CompareFunc(NpcConductNode node_a, NpcConductNode node_b)
		{
			Vector3 zero = Vector3.zero;
			if (node_a != node_b && node_a != null && node_b != null)
			{
				if (node_a.m_go_Npc == null)
				{
					return 0;
				}
				float num;
				if (node_a.m_go_Npc.transform.localPosition.Equals(Vector3.zero))
				{
					if (node_a.m_ConductNodeList.Count <= 0)
					{
						return 0;
					}
					ConductNode conductNode;
					if (node_a.m_iType == 0)
					{
						conductNode = node_a.m_ConductNodeList[0];
					}
					else
					{
						conductNode = node_a.GetConductNode(node_a.m_fClickTime);
					}
					if (conductNode == null)
					{
						return 0;
					}
					MapNpcDataNode mapNpcDataNode = Game.MapIcon.GetMapNpcDataNode(conductNode.m_ConductID);
					if (mapNpcDataNode == null)
					{
						return 0;
					}
					if (mapNpcDataNode.m_PointNodeList.Count <= 0)
					{
						return 0;
					}
					zero..ctor(mapNpcDataNode.m_PointNodeList[0].m_fX, mapNpcDataNode.m_PointNodeList[0].m_fY, mapNpcDataNode.m_PointNodeList[0].m_fZ);
					num = Vector3.Distance(MapData.s_vPos, zero);
				}
				else
				{
					num = Vector3.Distance(MapData.s_vPos, node_a.m_go_Npc.transform.localPosition);
				}
				if (node_b.m_go_Npc == null)
				{
					return 0;
				}
				float num2;
				if (node_b.m_go_Npc.transform.localPosition.Equals(Vector3.zero))
				{
					if (node_b.m_ConductNodeList.Count <= 0)
					{
						return 0;
					}
					ConductNode conductNode;
					if (node_b.m_iType == 0)
					{
						conductNode = node_b.m_ConductNodeList[0];
					}
					else
					{
						conductNode = node_b.GetConductNode(node_b.m_fClickTime);
					}
					if (conductNode == null)
					{
						return 0;
					}
					MapNpcDataNode mapNpcDataNode = Game.MapIcon.GetMapNpcDataNode(conductNode.m_ConductID);
					if (mapNpcDataNode == null)
					{
						return 0;
					}
					if (mapNpcDataNode.m_PointNodeList.Count <= 0)
					{
						return 0;
					}
					zero..ctor(mapNpcDataNode.m_PointNodeList[0].m_fX, mapNpcDataNode.m_PointNodeList[0].m_fY, mapNpcDataNode.m_PointNodeList[0].m_fZ);
					num2 = Vector3.Distance(MapData.s_vPos, zero);
				}
				else
				{
					num2 = Vector3.Distance(MapData.s_vPos, node_b.m_go_Npc.transform.localPosition);
				}
				if (num < num2)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
			}
			return 0;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00060584 File Offset: 0x0005E784
		public void FindSkyDome()
		{
			if (this.go_Sky == null)
			{
				this.go_Sky = GameObject.FindGameObjectWithTag("WorldTime");
				if (this.go_Sky == null)
				{
					Debug.LogWarning(Application.loadedLevelName + " MapData:FindMapNpcSkyDome Tag WorldTime 找不到 如果是小遊戲 養成 找不到是正常的");
				}
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000605D8 File Offset: 0x0005E7D8
		public void SetNpcData()
		{
			Debug.Log("MapData:SetNpcData");
			this.CheckMapNpc();
			string loadedLevelName = Application.loadedLevelName;
			Game.MapIcon.LoadMapIconFile(loadedLevelName);
			Game.NpcConduct.LoadNpcConductFile(loadedLevelName);
			for (int i = 0; i < Game.NpcConduct.m_NpcConductNodeList.Count; i++)
			{
				NpcConductNode npcConductNode = Game.NpcConduct.m_NpcConductNodeList[i];
				int num = 0;
				if (num < npcConductNode.m_ConductNodeList.Count)
				{
					ConductNode conductNode = npcConductNode.m_ConductNodeList[num];
					GameObject go = Game.g_ModelBundle.Load(conductNode.m_strModelName + "_ModelPrefab") as GameObject;
					this.BuildModel(npcConductNode, go);
				}
				this.m_NpcConductAlwaysList.Add(npcConductNode);
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000606A8 File Offset: 0x0005E8A8
		public int CheckNpcAlwaysList(string strID)
		{
			int num;
			if (!int.TryParse(strID, ref num))
			{
				return -1;
			}
			for (int i = 0; i < this.m_NpcConductAlwaysList.Count; i++)
			{
				if (this.m_NpcConductAlwaysList[i].m_iNpcID == num)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x000606FC File Offset: 0x0005E8FC
		public void DeleteAlwaysNpc(int iID, int iIndex = 0)
		{
			if (iIndex == 0)
			{
				iIndex = this.CheckNpcAlwaysList(iID.ToString());
			}
			if (iIndex > 0 && this.m_NpcConductAlwaysList[iIndex].m_go_Npc != null)
			{
				this.m_SpecialNpcList.Remove(this.m_NpcConductAlwaysList[iIndex].m_iNpcID);
				this.m_NpcConductAlwaysList[iIndex].m_go_Npc.transform.localPosition = Vector3.zero;
				Object.Destroy(this.m_NpcConductAlwaysList[iIndex].m_go_Npc);
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00060794 File Offset: 0x0005E994
		private void BringNpc(NpcConductNode _NpcConductNode, float fNowTime)
		{
			if (_NpcConductNode == null)
			{
				return;
			}
			ConductNode conductNode = _NpcConductNode.GetConductNode(fNowTime);
			_NpcConductNode.m_fClickTime = fNowTime;
			if (conductNode != null)
			{
				GameObject go = Game.g_ModelBundle.Load(conductNode.m_strModelName + "_ModelPrefab") as GameObject;
				this.BuildModel(_NpcConductNode, go);
				_NpcConductNode.m_iTimeType = 0;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000607F0 File Offset: 0x0005E9F0
		private void NpcCheckPlayer()
		{
			if (this.goPlayer == null)
			{
				this.goPlayer = GameObject.FindWithTag("Player");
			}
			if (this.goPlayer == null)
			{
				return;
			}
			if (GameGlobal.m_bMovie || GameGlobal.m_bPlayerTalk || GameGlobal.m_bCFormOpen || GameGlobal.m_bWaitToBattle || GameGlobal.m_bDoTalkReward)
			{
				return;
			}
			if (PlayerTarget.BattleNPClist.Count > 0)
			{
				for (int i = 0; i < PlayerTarget.BattleNPClist.Count; i++)
				{
					PlayerTarget playerTarget = PlayerTarget.BattleNPClist[i];
					GameObject gameObject = playerTarget.gameObject;
					if (gameObject != null && gameObject.activeSelf)
					{
						NpcCollider component = gameObject.GetComponent<NpcCollider>();
						if (component == null)
						{
							Debug.Log(" Npc Nmae " + gameObject.name + " no NpcColliderPlease Check");
						}
						else if (Time.time > component.fCheckDelayTime && component.bBattle && component.bCheakPlayer)
						{
							int num = this.CheckPlayer(gameObject, this.goPlayer.transform.position);
							if (num == 2)
							{
								component.setMode(num);
								break;
							}
							component.setMode(num);
						}
					}
				}
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0006094C File Offset: 0x0005EB4C
		private int CheckPlayer(GameObject pos, Vector3 tpos)
		{
			Vector3 forward = pos.transform.forward;
			if (TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.AvoidAlert))
			{
				this.m_fForwardowerSight = 1f;
			}
			Vector3 vector = tpos - pos.transform.position;
			float num = vector.magnitude;
			if (num > 50f)
			{
				num = 50f;
			}
			vector.Normalize();
			float num2 = Vector3.Dot(vector, forward);
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			else if (num2 < -1f)
			{
				num2 = -1f;
			}
			pos.GetComponent<NpcCollider>().fCheckDelayTime = Time.time + num * 0.02f;
			float num3 = Mathf.Acos(num2) * 57.29578f;
			if (0f <= num3 && num3 <= 45f)
			{
				if (num < this.m_fForwardowerSight)
				{
					return 2;
				}
				if (this.m_fForwardowerSight * 2f > num && num > this.m_fForwardowerSight)
				{
					return 1;
				}
				if (num > this.m_fForwardowerSight * 2f)
				{
					return 0;
				}
			}
			else if (45f < num3 && num3 <= 180f)
			{
				if (num < this.f_fBackSight)
				{
					return 2;
				}
				if (0f <= num && num <= this.f_fBackSight * 2f)
				{
					return 1;
				}
				if (num > this.f_fBackSight * 2f)
				{
					return 0;
				}
			}
			return 0;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00009115 File Offset: 0x00007315
		private void Update()
		{
			this.NpcCheckPlayer();
		}

		// Token: 0x04000D5A RID: 3418
		public static MapData m_instance;

		// Token: 0x04000D5B RID: 3419
		public CameraSaveDateNode _CameraSaveDateNode = new CameraSaveDateNode();

		// Token: 0x04000D5C RID: 3420
		public List<NpcConductNode> m_NpcConductAlwaysList = new List<NpcConductNode>();

		// Token: 0x04000D5D RID: 3421
		public List<NpcConductNode> m_NpcConductDelayLoadList = new List<NpcConductNode>();

		// Token: 0x04000D5E RID: 3422
		public List<NpcIsFought> m_NpcIsFoughtList = new List<NpcIsFought>();

		// Token: 0x04000D5F RID: 3423
		public GameObject go_Npc;

		// Token: 0x04000D60 RID: 3424
		private GameObject go_Sky;

		// Token: 0x04000D61 RID: 3425
		private float m_fOrigOccInt = 0.65f;

		// Token: 0x04000D62 RID: 3426
		private static Vector3 s_vPos = Vector3.zero;

		// Token: 0x04000D63 RID: 3427
		private List<TreasureBoxNode> TreasureBoxList = new List<TreasureBoxNode>();

		// Token: 0x04000D64 RID: 3428
		public GameObject go_TreasureBox;

		// Token: 0x04000D65 RID: 3429
		private GameObject goPlayer;

		// Token: 0x04000D66 RID: 3430
		public float m_fForwardowerSight = 3f;

		// Token: 0x04000D67 RID: 3431
		public float f_fBackSight = 1f;

		// Token: 0x04000D68 RID: 3432
		public List<int> m_SpecialNpcList = new List<int>();
	}
}
