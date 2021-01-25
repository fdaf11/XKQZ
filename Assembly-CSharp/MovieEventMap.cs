using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using JsonFx.Json;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class MovieEventMap : MonoBehaviour
{
	// Token: 0x06000C11 RID: 3089 RVA: 0x00009321 File Offset: 0x00007521
	public bool IsMoviePlaying()
	{
		return this.bMoviePlaying;
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0006366C File Offset: 0x0006186C
	public void AddNpcToList(GameObject go)
	{
		if (go.GetComponent<NpcCollider>())
		{
			go.GetComponent<NpcCollider>().enabled = false;
		}
		if (go.GetComponent<NavMeshAgent>())
		{
			go.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (!this.NpcList.Contains(go))
		{
			this.NpcList.Add(go);
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000636D0 File Offset: 0x000618D0
	public void SaveSceneAllGroup(string strSceneName)
	{
		foreach (MovieEventGroup movieEventGroup in this.movieEventGroupList)
		{
			movieEventGroup.Save(strSceneName);
		}
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x00009329 File Offset: 0x00007529
	public void LoadSceneAllGroup(string strSceneName)
	{
		this.movieEventGroupList.Clear();
		this.LoadMovieFormAssetBundle(strSceneName);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x0006372C File Offset: 0x0006192C
	private void LoadMovieFormAssetBundle(string strSceneName)
	{
		string text = string.Concat(new string[]
		{
			Game.g_strDataPathToApplicationPath,
			"Mods/",
			GameGlobal.m_strVersion,
			"/Config/Movie/",
			strSceneName,
			".txt"
		});
		string[] array = null;
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
					Debug.LogError(strSceneName + "Exception : " + ex.Message);
					return;
				}
				string text2 = string.Empty;
				text2 = streamReader.ReadToEnd();
				if (GameGlobal.m_iModFixFileCount <= 0)
				{
					Debug.Log("Mod active");
				}
				GameGlobal.m_iModFixFileCount++;
				array = text2.Split(new char[]
				{
					"\n".get_Chars(0)
				});
			}
			catch
			{
				Debug.LogError("MovieEventCube 散檔讀取失敗 !! ( " + text + " )");
				return;
			}
		}
		else
		{
			TextAsset textAsset = Game.g_Movie.Load(strSceneName) as TextAsset;
			if (textAsset == null)
			{
				Debug.LogError(strSceneName + " no have movie");
				return;
			}
			array = textAsset.text.Split(new char[]
			{
				"\n".get_Chars(0)
			});
		}
		if (array != null)
		{
			foreach (string text3 in array)
			{
				text = string.Concat(new string[]
				{
					Game.g_strDataPathToApplicationPath,
					"Mods/",
					GameGlobal.m_strVersion,
					"/Config/Movie/",
					strSceneName,
					"/",
					text3.Trim(),
					".txt"
				});
				if (File.Exists(text))
				{
					try
					{
						Stream stream2 = File.OpenRead(text);
						StreamReader streamReader2;
						try
						{
							streamReader2 = new StreamReader(stream2, Encoding.Unicode);
						}
						catch (Exception ex2)
						{
							Debug.LogError(strSceneName + "Exception : " + ex2.Message);
							break;
						}
						string text4 = string.Empty;
						text4 = streamReader2.ReadToEnd();
						if (GameGlobal.m_iModFixFileCount <= 0)
						{
							Debug.Log("Mod active");
						}
						GameGlobal.m_iModFixFileCount++;
						JsonReader jsonReader = new JsonReader(text4);
						MovieEventGroupJson movieEventGroupJson = jsonReader.Deserialize<MovieEventGroupJson>();
						MovieEventGroup movieEventGroup = movieEventGroupJson.ToPlayMode();
						this.movieEventGroupList.Add(movieEventGroup);
					}
					catch
					{
						Debug.LogError("MovieEventCube 散檔讀取失敗 !! ( " + text + " )");
						break;
					}
				}
				else
				{
					string text5 = strSceneName + "/" + text3.Trim();
					TextAsset textAsset2 = Game.g_Movie.Load(text5) as TextAsset;
					if (!(textAsset2 == null))
					{
						JsonReader jsonReader2 = new JsonReader(textAsset2.text);
						MovieEventGroupJson movieEventGroupJson2 = jsonReader2.Deserialize<MovieEventGroupJson>();
						MovieEventGroup movieEventGroup2 = movieEventGroupJson2.ToPlayMode();
						this.movieEventGroupList.Add(movieEventGroup2);
					}
				}
			}
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00063A5C File Offset: 0x00061C5C
	public void LoadOneMovieEventGroup(string fullFileName)
	{
		StreamReader streamReader = new StreamReader(fullFileName);
		JsonReader jsonReader = new JsonReader(streamReader);
		streamReader.Close();
		MovieEventGroupJson movieEventGroupJson = jsonReader.Deserialize<MovieEventGroupJson>();
		MovieEventGroup movieEventGroup = movieEventGroupJson.ToPlayMode();
		this.movieEventGroupList.Add(movieEventGroup);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00063A98 File Offset: 0x00061C98
	private IEnumerator JumpMovieFadeOut()
	{
		if (this.SO == null)
		{
			this.SO = Camera.main.GetComponent<ScreenOverlay>();
			if (this.SO == null)
			{
				this.SO = Camera.main.gameObject.AddComponent<ScreenOverlay>();
			}
		}
		float fPos = 0.25f;
		float fTime = Time.realtimeSinceStartup;
		if (this.SO != null)
		{
			while (fPos > 0f)
			{
				this.SO.intensity = fPos * 4f;
				this.SO.enabled = true;
				fPos -= Time.realtimeSinceStartup - fTime;
				fTime = Time.realtimeSinceStartup;
				yield return null;
			}
			this.SO.intensity = 0f;
			this.SO.enabled = true;
		}
		this.bStartJump = true;
		if (this.talkOptionMen != null)
		{
			base.StartCoroutine(this.JumpMovieFadeIn());
		}
		else if (MovieEventMap.iTalkNextID > 0)
		{
			this.TextOutOnClick(null);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00063AB4 File Offset: 0x00061CB4
	private IEnumerator JumpMovieFadeIn()
	{
		while (this.DelayClipList.Count > 0)
		{
			yield return null;
		}
		while (this.NextClipList.Count > 0 && this.playingNodeList.Count > 0)
		{
			yield return null;
		}
		this.bStartJump = false;
		if (this.SO == null)
		{
			this.SO = Camera.main.GetComponent<ScreenOverlay>();
			if (this.SO == null)
			{
				this.SO = Camera.main.gameObject.AddComponent<ScreenOverlay>();
			}
		}
		float fPos = 0f;
		float fTime = Time.realtimeSinceStartup;
		if (this.SO != null)
		{
			while (fPos < 0.25f)
			{
				this.SO.intensity = fPos * 4f;
				this.SO.enabled = true;
				fPos += Time.realtimeSinceStartup - fTime;
				fTime = Time.realtimeSinceStartup;
				yield return null;
			}
			this.SO.intensity = 1f;
			this.SO.enabled = false;
		}
		this.bJump = false;
		yield break;
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00063AD0 File Offset: 0x00061CD0
	private void Update()
	{
		if (!GameGlobal.m_bMovie || GameGlobal.m_bBattle || !GameGlobal.m_bCanJumpMovie || !Input.GetKeyDown(113) || !this.bMoviePlaying)
		{
			this.UpdatePlayingNodeList();
			return;
		}
		if (this.iQuestionNextID > 0)
		{
			return;
		}
		if (this.talkOptionMen != null)
		{
			return;
		}
		if (this.bJump)
		{
			return;
		}
		this.bJump = true;
		base.StartCoroutine(this.JumpMovieFadeOut());
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00063B54 File Offset: 0x00061D54
	private void SetMovieStatus()
	{
		GameGlobal.m_bMovie = true;
		this.bMoviePlaying = true;
		if (this.bStartJump || this.bJump)
		{
			this.bStartJump = false;
			this.bJump = false;
		}
		if (Game.UI.Get<UITalk>() == null)
		{
			Debug.LogError(Application.loadedLevelName + " not have UITalk");
		}
		else
		{
			Game.UI.Get<UITalk>().StartMovieEvent(new UIEventListener.VoidDelegate(this.TextOutOnClick), new UIEventListener.VoidDelegate(this.TextOutOptionOnClick));
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		if (array.Length != 1)
		{
			Debug.LogError("Movie Event SetMovieStatus But " + array.Length.ToString() + " Player");
			foreach (GameObject gameObject in array)
			{
				Debug.LogError(gameObject.name);
			}
		}
		this.goPlayer = GameObject.FindGameObjectWithTag("Player");
		if (this.goPlayer != null)
		{
			if (this.goPlayer.GetComponent<NavMeshAgent>() != null)
			{
				this.goPlayer.GetComponent<NavMeshAgent>().enabled = false;
			}
			if (this.goPlayer.GetComponent<PlayerController>() != null)
			{
				this.goPlayer.GetComponent<PlayerController>().ReSetMoveDate();
			}
			if (this.goPlayer.GetComponent<HighlightableObject>() != null)
			{
				this.goPlayer.GetComponent<HighlightableObject>().enabled = false;
			}
			if (this.goPlayer.animation != null)
			{
				if (this.goPlayer.animation["stand01"] == null)
				{
					Game.g_ModelBundle.LoadAnimation(this.goPlayer.animation, "stand01");
				}
				if (this.goPlayer.animation["stand01"] == null)
				{
					this.goPlayer.animation.CrossFade("stand01");
				}
			}
		}
		if (Camera.main.GetComponent<DepthOfFieldScatter>() != null)
		{
			Camera.main.GetComponent<DepthOfFieldScatter>().SetDoFSTarget(null);
		}
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0000933D File Offset: 0x0000753D
	private void SetRewardIndex(MovieEventNode men)
	{
		Game.RewardData.DoRewardID(men.iTextOrder, null);
		if (men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00063D80 File Offset: 0x00061F80
	private void SetReward(int GroupID)
	{
		MapTalkTypeNode mapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(GroupID.ToString());
		if (mapTalkTypeNode != null)
		{
			for (int i = 0; i < mapTalkTypeNode.m_MapTalkNodeList.Count; i++)
			{
				int iGiftID = mapTalkTypeNode.m_MapTalkNodeList[i].m_iGiftID;
				if (iGiftID != 0)
				{
					Game.RewardData.DoRewardID(iGiftID, null);
				}
			}
		}
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00063DE8 File Offset: 0x00061FE8
	public void StopMovieGroup(int iMovieGroupID)
	{
		int i = 0;
		while (i < this.playingNodeList.Count)
		{
			if (this.playingNodeList[i].GroupID == iMovieGroupID)
			{
				Debug.LogWarning("動畫停止 " + this.playingNodeList[i].GroupID.ToString() + " node " + this.playingNodeList[i].NodeID.ToString());
				if (this.playingNodeList[i].NextNodeID >= 0)
				{
					this.NextClipList.Remove(this.playingNodeList[i].NextNodeID);
				}
				this.playingNodeList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		if (!this.StopMovieList.Contains(iMovieGroupID))
		{
			this.StopMovieList.Add(iMovieGroupID);
		}
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00063ECC File Offset: 0x000620CC
	public void ChangeSceneResetMovieStyle()
	{
		if (!GameGlobal.m_bMovie)
		{
			return;
		}
		this.NpcList.Clear();
		GameGlobal.m_bMovie = false;
		this.bMoviePlaying = false;
		Time.timeScale = 1f;
		GameGlobal.m_bPlayerTalk = false;
		if (Game.UI.Get<UITalk>() != null)
		{
			Game.UI.Get<UITalk>().ResetMovieTalk();
			Game.UI.Get<UITalk>().EndMovieEvnet();
		}
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00063F40 File Offset: 0x00062140
	private void SetPlayingStatus()
	{
		if (this.bStartJump)
		{
			base.StartCoroutine(this.JumpMovieFadeIn());
		}
		foreach (GameObject gameObject in this.NpcList)
		{
			if (!(gameObject == null))
			{
				if (!(gameObject.GetComponent<NpcCollider>() == null))
				{
					gameObject.GetComponent<NpcCollider>().enabled = true;
					gameObject.GetComponent<NpcCollider>().ResetNavMeshAgent();
				}
			}
		}
		this.NpcList.Clear();
		GameGlobal.m_bMovie = false;
		this.bMoviePlaying = false;
		if (Game.UI.Get<UITalk>() != null)
		{
			Game.UI.Get<UITalk>().ResetMovieTalk();
			Game.UI.Get<UITalk>().EndMovieEvnet();
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		if (array.Length != 1)
		{
			Debug.LogError("Movie Event SetPlayingStatus But " + array.Length.ToString() + " Player");
			foreach (GameObject gameObject2 in array)
			{
				Debug.LogError(gameObject2.name);
			}
		}
		this.goPlayer = GameObject.FindGameObjectWithTag("Player");
		if (this.goPlayer != null)
		{
			if (this.goPlayer.GetComponent<UnitTB>() != null)
			{
				this.goPlayer.layer = this.goPlayer.GetComponent<UnitTB>().backupLayer;
				Object.DestroyImmediate(this.goPlayer.GetComponent<UnitTB>());
				Object.DestroyImmediate(this.goPlayer.GetComponent<UnitTBAnimation>());
				Object.DestroyImmediate(this.goPlayer.GetComponent<UnitTBAudio>());
				Object.DestroyImmediate(this.goPlayer.GetComponent<AnimationEventTrigger>());
				if (this.goPlayer.animation["stand01"] != null)
				{
					this.goPlayer.animation["stand01"].layer = 0;
					this.goPlayer.animation["stand01"].wrapMode = 2;
				}
				if (this.goPlayer.animation["run"] != null)
				{
					this.goPlayer.animation["run"].layer = 0;
					this.goPlayer.animation["run"].wrapMode = 2;
				}
			}
			if (this.goPlayer.GetComponent<NavMeshAgent>() == null)
			{
				NavMeshAgent navMeshAgent = this.goPlayer.AddComponent<NavMeshAgent>();
				navMeshAgent.radius = 1E-05f;
				navMeshAgent.speed = 5f;
				navMeshAgent.acceleration = 256f;
				navMeshAgent.angularSpeed = 720f;
				navMeshAgent.autoTraverseOffMeshLink = false;
				navMeshAgent.Warp(this.goPlayer.transform.position);
				navMeshAgent.SetDestination(this.goPlayer.transform.position);
			}
			else if (!this.goPlayer.GetComponent<NavMeshAgent>().enabled)
			{
				this.goPlayer.GetComponent<NavMeshAgent>().enabled = true;
			}
			if (this.goPlayer.GetComponent<PlayerController>() != null)
			{
				this.goPlayer.GetComponent<PlayerController>().ResetNav();
				this.goPlayer.GetComponent<PlayerController>().ReSetMoveDate();
			}
			if (this.goPlayer.GetComponent<HighlightableObject>() != null)
			{
				this.goPlayer.GetComponent<HighlightableObject>().enabled = true;
			}
			if (Camera.main.GetComponent<DepthOfFieldScatter>() != null && !GameGlobal.m_bBattle)
			{
				Camera.main.GetComponent<DepthOfFieldScatter>().SetDoFSTarget(this.goPlayer.transform);
			}
		}
		else
		{
			Debug.LogError("Movie End Player no Found");
		}
		for (int j = 0; j < this.PlaySkillList.Count; j++)
		{
			GameObject gameObject3 = this.PlaySkillList[j];
			if (!(gameObject3 == null))
			{
				if (!(gameObject3.GetComponent<UnitTB>() == null))
				{
					gameObject3.layer = gameObject3.GetComponent<UnitTB>().backupLayer;
					Object.DestroyImmediate(gameObject3.GetComponent<UnitTB>());
					Object.DestroyImmediate(gameObject3.GetComponent<UnitTBAnimation>());
					Object.DestroyImmediate(gameObject3.GetComponent<UnitTBAudio>());
					Object.DestroyImmediate(gameObject3.GetComponent<AnimationEventTrigger>());
					if (gameObject3.animation["stand01"] != null)
					{
						gameObject3.animation["stand01"].layer = 0;
						gameObject3.animation["stand01"].wrapMode = 2;
					}
					if (gameObject3.animation["run"] != null)
					{
						gameObject3.animation["run"].layer = 0;
						gameObject3.animation["run"].wrapMode = 2;
					}
				}
			}
		}
		this.PlaySkillList.Clear();
		this.goEnableList.Clear();
		Debug.Log(" Movie finish");
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00064480 File Offset: 0x00062680
	public void PlayMovie(int iMovieID)
	{
		if (this.StopMovieList.Contains(iMovieID))
		{
			this.StopMovieList.Remove(iMovieID);
		}
		this.SetMovieStatus();
		Debug.Log("PlayMovie ID = " + iMovieID.ToString());
		if (this.goMapNpc == null)
		{
			this.goMapNpc = GameObject.FindGameObjectWithTag("MapNpc");
			if (this.goMapNpc == null)
			{
				this.goMapNpc = new GameObject("MapNpc");
				this.goMapNpc.transform.position = new Vector3(0f, 0f, 0f);
				this.goMapNpc.tag = "MapNpc";
				Battle.AddMainGameList(this.goMapNpc);
			}
		}
		bool flag = false;
		foreach (MovieEventGroup movieEventGroup in this.movieEventGroupList)
		{
			if (movieEventGroup.ID == iMovieID)
			{
				this.playingMeg = movieEventGroup;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogError("Movie " + iMovieID.ToString() + " no found");
		}
		if (this.playingMeg != null && flag)
		{
			this.PlayMovieGroup(this.playingMeg);
		}
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x000645E8 File Offset: 0x000627E8
	public void ContinueMovie(int iMovieID, int iNextNodeID)
	{
		if (this.StopMovieList.Contains(iMovieID))
		{
			this.StopMovieList.Remove(iMovieID);
		}
		this.SetMovieStatus();
		Debug.Log("ContinueMovie ID = " + iMovieID.ToString() + " Node ID = " + iNextNodeID.ToString());
		if (this.goMapNpc == null)
		{
			this.goMapNpc = GameObject.FindGameObjectWithTag("MapNpc");
			if (this.goMapNpc == null)
			{
				this.goMapNpc = new GameObject("MapNpc");
				this.goMapNpc.transform.position = new Vector3(0f, 0f, 0f);
				this.goMapNpc.tag = "MapNpc";
				Battle.AddMainGameList(this.goMapNpc);
			}
		}
		foreach (MovieEventGroup movieEventGroup in this.movieEventGroupList)
		{
			if (movieEventGroup.ID == iMovieID)
			{
				this.playingMeg = movieEventGroup;
				break;
			}
		}
		if (this.playingMeg != null)
		{
			int count = this.playingMeg.movieEventNodeList.Count;
			for (int i = 0; i < count; i++)
			{
				MovieEventNode movieEventNode = this.playingMeg.movieEventNodeList[i];
				movieEventNode.GroupID = this.playingMeg.ID;
				movieEventNode.goActor = null;
			}
			this.PlayMovieNode(this.FindMovieEventNodeByID(iNextNodeID, iMovieID));
		}
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00064784 File Offset: 0x00062984
	private void PlayMovieGroup(MovieEventGroup myMeg)
	{
		if (myMeg == null)
		{
			return;
		}
		int count = myMeg.movieEventNodeList.Count;
		for (int i = 0; i < count; i++)
		{
			MovieEventNode movieEventNode = myMeg.movieEventNodeList[i];
			movieEventNode.GroupID = myMeg.ID;
			movieEventNode.goActor = null;
		}
		for (int j = 0; j < count; j++)
		{
			MovieEventNode movieEventNode2 = myMeg.movieEventNodeList[j];
			if (movieEventNode2.mEventnType == _MovieEventNodeType.Enable)
			{
				movieEventNode2.goActor = this.FindActor(movieEventNode2.strActorName, movieEventNode2.strActorTag);
			}
			if (movieEventNode2.bMovieStart)
			{
				this.NextClipList.Add(movieEventNode2.NodeID);
				this.PlayMovieNode(movieEventNode2);
			}
		}
		Game.Achievement.SetMovie(myMeg.ID);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x0006485C File Offset: 0x00062A5C
	private void PlayMovieNode(MovieEventNode men)
	{
		if (men == null)
		{
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"Group ID = ",
			men.GroupID,
			" PlayMovieNode id = ",
			men.NodeID,
			"  ",
			men.strEventName
		}));
		if (men.TriggerNodeID >= 0 && !this.CheckMovieStop(men.GroupID))
		{
			this.NextClipList.Add(men.TriggerNodeID);
		}
		if (men.NextNodeID >= 0 && !this.CheckMovieStop(men.GroupID))
		{
			this.NextClipList.Add(men.NextNodeID);
		}
		this.NextClipList.Remove(men.NodeID);
		switch (men.mEventnType)
		{
		case _MovieEventNodeType.SetPos:
			base.StartCoroutine(this.SetPos(men));
			break;
		case _MovieEventNodeType.SetRotate:
			base.StartCoroutine(this.SetRotate(men));
			break;
		case _MovieEventNodeType.PlayAni:
			base.StartCoroutine(this.PlayAni(men));
			break;
		case _MovieEventNodeType.Move:
		case _MovieEventNodeType.TrackingMove:
		case _MovieEventNodeType.MoveCurve:
			base.StartCoroutine(this.PlayMove(men));
			break;
		case _MovieEventNodeType.TextOut:
			base.StartCoroutine(this.TextOut(men));
			break;
		case _MovieEventNodeType.TextOutOption:
			base.StartCoroutine(this.TextOutOption(men));
			break;
		case _MovieEventNodeType.PlayBackgroundMusic:
			base.StartCoroutine(this.PlayBGMusic(men));
			break;
		case _MovieEventNodeType.MoveLookAt:
			base.StartCoroutine(this.PlayMoveLockAt(men));
			break;
		case _MovieEventNodeType.MoveRotate:
			base.StartCoroutine(this.PlayMoveRotate(men));
			break;
		case _MovieEventNodeType.CameraLookat:
			base.StartCoroutine(this.PlayCameraLookat(men));
			break;
		case _MovieEventNodeType.Finish:
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Why finish have Next Node ID");
				this.NextClipList.Remove(men.NextNodeID);
			}
			this.SetPlayingStatus();
			break;
		case _MovieEventNodeType.TimeScale:
			base.StartCoroutine(this.PlayTimeScale(men));
			break;
		case _MovieEventNodeType.DepthOfField:
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Why DepthOfField have Next Node ID");
				this.NextClipList.Remove(men.NextNodeID);
			}
			break;
		case _MovieEventNodeType.CamaraLockPlayer:
			base.StartCoroutine(this.SetCameraLock(men));
			break;
		case _MovieEventNodeType.Transfer:
			base.StartCoroutine(this.Transfer(men));
			break;
		case _MovieEventNodeType.PlayNextMovie:
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Why PlayNextMovie have Next Node ID");
				this.NextClipList.Remove(men.NextNodeID);
			}
			base.StartCoroutine(this.PlayNextMovie(men));
			break;
		case _MovieEventNodeType.SetReward:
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Why SetReward have Next Node ID");
				this.NextClipList.Remove(men.NextNodeID);
			}
			this.SetReward(men.GroupID);
			break;
		case _MovieEventNodeType.ChangeScenes:
			this.SaveTimeChangeScenes();
			GameSetting.m_Instance.GetComponent<MovieEventTrigger>().ChangeScenes(men);
			break;
		case _MovieEventNodeType.StartBattle:
			base.StartCoroutine(this.PlayStartBattle(men));
			break;
		case _MovieEventNodeType.CameraZoom:
			base.StartCoroutine(this.PlayCameraZoom(men));
			break;
		case _MovieEventNodeType.OribitCameraMove:
			base.StartCoroutine(this.PlayOribitCameraMove(men));
			break;
		case _MovieEventNodeType.CreateModel:
			this.CreateMovieModel(men);
			break;
		case _MovieEventNodeType.DestroyModel:
			this.DestroyMovieModel(men);
			break;
		case _MovieEventNodeType.SetRewardIndex:
			this.SetRewardIndex(men);
			break;
		case _MovieEventNodeType.FadeInOut:
			base.StartCoroutine(this.FadeInOut(men));
			break;
		case _MovieEventNodeType.FarPlane:
			base.StartCoroutine(this.FarPlane(men));
			break;
		case _MovieEventNodeType.SetQuest:
			base.StartCoroutine(this.SetQuest(men));
			break;
		case _MovieEventNodeType.SetCollection:
			base.StartCoroutine(this.SetCollection(men));
			break;
		case _MovieEventNodeType.StopMovie:
			base.StartCoroutine(this.StopMovie(men));
			break;
		case _MovieEventNodeType.CreateEffect:
			base.StartCoroutine(this.CreateEffect(men));
			break;
		case _MovieEventNodeType.SetTime:
			this.SetTime(men);
			break;
		case _MovieEventNodeType.PlaySkill:
			base.StartCoroutine(this.PlaySkill(men));
			break;
		case _MovieEventNodeType.PlayHurt:
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Why PlayHurt have Next Node ID");
				this.NextClipList.Remove(men.NextNodeID);
			}
			break;
		case _MovieEventNodeType.Enable:
			base.StartCoroutine(this.SetEnable(men));
			break;
		case _MovieEventNodeType.CheckPropert:
			base.StartCoroutine(this.CheckPropert(men));
			break;
		case _MovieEventNodeType.CheckFriendly:
			base.StartCoroutine(this.CheckFriendly(men));
			break;
		case _MovieEventNodeType.CheckQuest:
			base.StartCoroutine(this.CheckQuest(men));
			break;
		case _MovieEventNodeType.SetQuestion:
			base.StartCoroutine(this.SetQuestion(men));
			break;
		case _MovieEventNodeType.SetInside:
			base.StartCoroutine(this.SetInside(men));
			break;
		case _MovieEventNodeType.CheckMoney:
			base.StartCoroutine(this.CheckMoney(men));
			break;
		case _MovieEventNodeType.CheckItemAmount:
			base.StartCoroutine(this.CheckItemAmount(men));
			break;
		case _MovieEventNodeType.SetMovieEvent:
			base.StartCoroutine(this.SetMovieEvent(men));
			break;
		case _MovieEventNodeType.CheckParty:
			base.StartCoroutine(this.CheckParty(men));
			break;
		case _MovieEventNodeType.CheckAchievement:
			base.StartCoroutine(this.CheckAchievement(men));
			break;
		case _MovieEventNodeType.CheckTalent:
			base.StartCoroutine(this.CheckTalent(men));
			break;
		case _MovieEventNodeType.CheckFlag:
			base.StartCoroutine(this.CheckFlag(men));
			break;
		case _MovieEventNodeType.SetFlag:
			base.StartCoroutine(this.SetFlag(men));
			break;
		case _MovieEventNodeType.CheckRoutine:
			base.StartCoroutine(this.CheckRoutine(men));
			break;
		case _MovieEventNodeType.CheckNeigong:
			base.StartCoroutine(this.CheckNeigong(men));
			break;
		}
		if (men.TriggerNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.TriggerNodeID, men.GroupID));
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0000264F File Offset: 0x0000084F
	private void SaveTimeChangeScenes()
	{
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00064EC8 File Offset: 0x000630C8
	private void CreateMovieModel(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			men.goActor = this.FindActor(men.strActorName, men.strActorTag);
		}
		if (men.goActor == null)
		{
			GameObject gameObject = Game.g_ModelBundle.Load(men.strLookatName + "_ModelPrefab") as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				MeleeWeaponTrail[] array = gameObject2.GetComponents<MeleeWeaponTrail>();
				foreach (MeleeWeaponTrail meleeWeaponTrail in array)
				{
					meleeWeaponTrail.Emit = false;
				}
				array = gameObject2.GetComponentsInChildren<MeleeWeaponTrail>();
				foreach (MeleeWeaponTrail meleeWeaponTrail2 in array)
				{
					meleeWeaponTrail2.Emit = false;
				}
				gameObject2.transform.position = new Vector3(0f, 0f, 0f);
				gameObject2.name = men.strActorName;
				gameObject2.tag = men.strActorTag;
				if (this.goMapNpc != null)
				{
					gameObject2.transform.parent = this.goMapNpc.transform;
				}
				if (gameObject2.GetComponent<NpcCollider>())
				{
					gameObject2.GetComponent<NpcCollider>().m_strModelName = men.strLookatName;
					gameObject2.GetComponent<NpcCollider>().enabled = false;
				}
				if (gameObject2.GetComponent<PlayerController>())
				{
					gameObject2.GetComponent<PlayerController>().m_strModelName = men.strLookatName;
					gameObject2.GetComponent<PlayerController>().enabled = false;
				}
			}
		}
		if (men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00065088 File Offset: 0x00063288
	private void DestroyMovieModel(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			men.goActor = this.FindActor(men.strActorName, men.strActorTag);
		}
		if (men.goActor != null)
		{
			Object.Destroy(men.goActor);
		}
		if (men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00065100 File Offset: 0x00063300
	private void SetTime(MovieEventNode men)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("WorldTime");
		if (gameObject != null && gameObject.GetComponent<TOD_Sky>() != null)
		{
			gameObject.GetComponent<TOD_Sky>().Cycle.Hour = men.fTotal;
			gameObject.GetComponent<TOD_Sky>().Ambient.bSkipCheckIntervalOnce = men.bCrossFade;
		}
		if (men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00065188 File Offset: 0x00063388
	private GameObject FindActor(string name, string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject gameObject in array)
		{
			if (gameObject.name == name)
			{
				if (tag == "Npc" || tag == "BattleNpc")
				{
					if (GameGlobal.m_bNewLoading && gameObject.animation == null && MapData.m_instance != null)
					{
						return MapData.m_instance.LoadNpcInNewLoading(name);
					}
					if (gameObject.GetComponent<NpcCollider>())
					{
						gameObject.GetComponent<NpcCollider>().enabled = false;
					}
					if (gameObject.GetComponent<NavMeshAgent>())
					{
						gameObject.GetComponent<NavMeshAgent>().enabled = false;
					}
					if (!this.NpcList.Contains(gameObject))
					{
						this.NpcList.Add(gameObject);
					}
				}
				return gameObject;
			}
		}
		foreach (GameObject gameObject2 in this.goEnableList)
		{
			if (!(gameObject2 == null))
			{
				if (gameObject2.name == name && gameObject2.tag == tag)
				{
					return gameObject2;
				}
			}
		}
		return null;
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00065304 File Offset: 0x00063504
	private void UpdatePlayingNodeList()
	{
		int i = 0;
		while (i < this.playingNodeList.Count)
		{
			bool flag = this.UpdatePlayingNode(this.playingNodeList[i]);
			if (flag)
			{
				MovieEventNode movieEventNode = this.playingNodeList[i];
				this.playingNodeList.RemoveAt(i);
				if (movieEventNode.NextNodeID >= 0)
				{
					this.PlayMovieNode(this.FindMovieEventNodeByID(movieEventNode.NextNodeID, movieEventNode.GroupID));
				}
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00065388 File Offset: 0x00063588
	private bool UpdatePlayingNode(MovieEventNode men)
	{
		_MovieEventNodeType mEventnType = men.mEventnType;
		switch (mEventnType)
		{
		case _MovieEventNodeType.Move:
			return this.UpdateMove(men);
		default:
			if (mEventnType == _MovieEventNodeType.TrackingMove)
			{
				return this.UpdateTrackingMove(men);
			}
			if (mEventnType == _MovieEventNodeType.MoveCurve)
			{
				return this.UpdateMoveCurve(men);
			}
			if (mEventnType != _MovieEventNodeType.OribitCameraMove)
			{
				return mEventnType != _MovieEventNodeType.FadeInOut || this.UpdateFadeInOut(men);
			}
			return this.UpdateOribitCamera(men);
		case _MovieEventNodeType.MoveLookAt:
			return this.UpdateMoveLookAt(men);
		case _MovieEventNodeType.MoveRotate:
			return this.UpdateMoveRotate(men);
		case _MovieEventNodeType.CameraLookat:
			return this.UpdateCameraLookAt(men);
		}
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0006542C File Offset: 0x0006362C
	private bool UpdateFadeInOut(MovieEventNode men)
	{
		if (men.fPos >= men.fTotal + men.fMoveSpeed)
		{
			return true;
		}
		if (this.SO == null)
		{
			return true;
		}
		if (men.fTotal < 0f)
		{
			return true;
		}
		if (men.fMoveSpeed < 0f)
		{
			return true;
		}
		if (this.bStartJump)
		{
			return true;
		}
		if (this.bJump)
		{
			return true;
		}
		men.fPos += Time.deltaTime;
		if (men.fPos < men.fTotal)
		{
			float num = men.fPos / men.fTotal;
			this.SO.intensity = 1f - num;
		}
		else if (men.fPos < men.fTotal + men.fMoveSpeed)
		{
			float num2 = men.fPos - men.fTotal;
			num2 /= men.fMoveSpeed;
			this.SO.intensity = num2;
		}
		else if (men.fMoveSpeed > 0f)
		{
			this.SO.intensity = 1f;
			this.SO.enabled = false;
		}
		return false;
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0006555C File Offset: 0x0006375C
	private bool UpdateOribitCamera(MovieEventNode men)
	{
		if (this.bStartJump)
		{
			men.fPos = men.fTotal;
		}
		if (men.fPos >= men.fTotal)
		{
			if (Camera.main.GetComponent<OrbitCam>() != null)
			{
				Vector3 vector = men.vPos * men.fTotal;
				vector = men.vOrigPos + vector;
				Debug.Log("OrbitCam vPos End = " + vector.ToString());
				Camera.main.GetComponent<OrbitCam>().SnapTo(vector.x, vector.z, vector.y);
				Camera.main.GetComponent<OrbitCam>().Target = men.goLookat.transform;
				Camera.main.GetComponent<OrbitCam>().enabled = men.bCrossFade;
				Camera.main.GetComponent<OrbitCam>().CollisionDetection = true;
			}
			return true;
		}
		men.fPos += Time.deltaTime;
		if (men.fPos < men.fTotal)
		{
			if (Camera.main.GetComponent<OrbitCam>() != null)
			{
				if (!Camera.main.GetComponent<OrbitCam>().enabled)
				{
					Camera.main.GetComponent<OrbitCam>().enabled = true;
				}
				Vector3 vector2 = men.vPos * men.fPos;
				vector2 = men.vOrigPos + vector2;
				Camera.main.GetComponent<OrbitCam>().SnapTo(vector2.x, vector2.z, vector2.y);
			}
		}
		else if (Camera.main.GetComponent<OrbitCam>() != null)
		{
			if (!Camera.main.GetComponent<OrbitCam>().enabled)
			{
				Camera.main.GetComponent<OrbitCam>().enabled = true;
			}
			Vector3 vector3 = men.vPos * men.fTotal;
			vector3 = men.vOrigPos + vector3;
			Debug.Log("OrbitCam vPos End = " + vector3.ToString());
			Camera.main.GetComponent<OrbitCam>().SnapTo(vector3.x, vector3.z, vector3.y);
		}
		return false;
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00065778 File Offset: 0x00063978
	private bool UpdateMove(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法移動 強制跳出");
			return true;
		}
		if (this.bStartJump)
		{
			men.goActor.transform.position = men.vPos;
			return true;
		}
		if (men.goActor.transform.position == men.vPos)
		{
			return true;
		}
		Vector3 position = men.goActor.transform.position;
		Vector3 vPos = men.vPos;
		float num;
		if (men.bCrossFade)
		{
			if (men.vLinkTerrain.x != 0f)
			{
				position.x = vPos.x;
			}
			if (men.vLinkTerrain.y != 0f)
			{
				position.y = vPos.y;
			}
			if (men.vLinkTerrain.z != 0f)
			{
				position.z = vPos.z;
			}
			num = Vector3.Distance(position, vPos);
		}
		else
		{
			num = Vector3.Distance(men.goActor.transform.position, men.vPos);
		}
		float num2;
		if (men.bCrossFade)
		{
			num2 = 0.1f;
		}
		else
		{
			num2 = 0.01f;
		}
		if (num > Time.deltaTime * men.fMoveSpeed && num > num2)
		{
			Vector3 vector = men.vPos - men.goActor.transform.position;
			vector.Normalize();
			vector = vector * Time.deltaTime * men.fMoveSpeed;
			Vector3 vector2 = men.goActor.transform.position + vector;
			if (men.bCrossFade)
			{
				Vector3 vector3 = vector2 - men.vLinkTerrain * 4f;
				int num3 = 2048;
				RaycastHit raycastHit;
				if (Physics.Raycast(vector3, men.vLinkTerrain, ref raycastHit, 8f, num3))
				{
					men.goActor.transform.position = raycastHit.point;
				}
				else
				{
					men.goActor.transform.position = vector2;
				}
			}
			else
			{
				men.goActor.transform.position = vector2;
			}
			if (men.bLookAtMoving)
			{
				if (men.bCrossFade)
				{
					Vector3 vPos2 = men.vPos;
					vPos2.y = men.goActor.transform.position.y;
					men.goActor.transform.LookAt(vPos2);
				}
				else
				{
					men.goActor.transform.LookAt(men.vPos);
				}
			}
			return false;
		}
		if (men.bCrossFade)
		{
			Vector3 vector4 = vPos - men.vLinkTerrain * 4f;
			int num4 = 2048;
			RaycastHit raycastHit2;
			if (Physics.Raycast(vector4, men.vLinkTerrain, ref raycastHit2, 8f, num4))
			{
				men.goActor.transform.position = raycastHit2.point;
			}
			else
			{
				men.goActor.transform.position = vPos;
			}
		}
		else
		{
			men.goActor.transform.position = vPos;
		}
		return true;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00065AC0 File Offset: 0x00063CC0
	private bool UpdateMoveCurve(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法曲線移動 強制跳出");
			return true;
		}
		if (this.bStartJump)
		{
			men.goActor.transform.position = men.vPos;
			return true;
		}
		if (men.fPos >= men.fTotal)
		{
			return true;
		}
		men.fPos += Time.deltaTime;
		if (men.fPos < men.fTotal)
		{
			float num = men.fPos / men.fTotal;
			Vector3 vector = Vector3.Lerp(men.vOrigPos, men.vPosDest, num);
			Vector3 vector2 = Vector3.Lerp(men.vPosDest, men.vPos, num);
			men.goActor.transform.position = Vector3.Lerp(vector, vector2, num);
			if (men.bLookAtMoving)
			{
				men.goActor.transform.LookAt(men.vPos);
			}
			return false;
		}
		men.goActor.transform.position = men.vPos;
		return true;
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x00065BDC File Offset: 0x00063DDC
	private bool UpdateMoveLookAt(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法移動 強制跳出");
			return true;
		}
		if (this.bStartJump)
		{
			men.goActor.transform.position = men.vPos;
			if (men.goLookat != null)
			{
				men.goActor.transform.LookAt(men.goLookat.transform);
			}
			return true;
		}
		if (men.goActor.transform.position == men.vPos)
		{
			return true;
		}
		float num = Vector3.Distance(men.goActor.transform.position, men.vPos);
		if (num > Time.deltaTime * men.fMoveSpeed && num > 0.01f)
		{
			Vector3 vector = men.vPos - men.goActor.transform.position;
			vector.Normalize();
			vector = vector * Time.deltaTime * men.fMoveSpeed;
			men.goActor.transform.position += vector;
			if (men.goLookat != null)
			{
				men.goActor.transform.LookAt(men.goLookat.transform);
			}
			return false;
		}
		men.goActor.transform.position = men.vPos;
		if (men.goLookat != null)
		{
			men.goActor.transform.LookAt(men.goLookat.transform);
		}
		return true;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00065D88 File Offset: 0x00063F88
	private bool UpdateMoveRotate(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法轉向 強制跳出");
			return true;
		}
		if (this.bStartJump)
		{
			GameObject gameObject = new GameObject();
			men.goActor.transform.localEulerAngles = men.vPosDest;
			Object.Destroy(gameObject);
			return true;
		}
		men.fPos += Time.deltaTime;
		if (men.fPos > men.fTotal)
		{
			GameObject gameObject2 = new GameObject();
			men.goActor.transform.localEulerAngles = men.vPosDest;
			Object.Destroy(gameObject2);
			return true;
		}
		GameObject gameObject3 = new GameObject();
		GameObject gameObject4 = new GameObject();
		gameObject3.transform.localEulerAngles = men.vPos;
		gameObject4.transform.localEulerAngles = men.vPosDest;
		float num = men.fPos / men.fTotal;
		men.goActor.transform.rotation = Quaternion.Lerp(gameObject3.transform.rotation, gameObject4.transform.rotation, num);
		Object.Destroy(gameObject3);
		Object.Destroy(gameObject4);
		return false;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00065EAC File Offset: 0x000640AC
	private bool UpdateCameraLookAt(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法當 Camera 的 基點 強制跳出");
			return true;
		}
		if (men.goLookat == null)
		{
			Debug.LogError(men.strLookatName + " 被刪掉了 無法當 Camera 的 Look At點 強制跳出");
			return true;
		}
		if (Camera.main == null)
		{
			Debug.LogError("Camera.main == null 強制跳出");
			return true;
		}
		if (men.fTotal <= 0f)
		{
			Debug.LogWarning("UpdateCameraLookAt Finish");
			return true;
		}
		if (this.bStartJump)
		{
			Camera.main.transform.position = men.goActor.transform.position;
			Camera.main.transform.LookAt(men.goLookat.transform);
			return true;
		}
		Camera.main.transform.position = men.goActor.transform.position;
		Camera.main.transform.LookAt(men.goLookat.transform);
		men.fTotal -= Time.deltaTime;
		return false;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00065FD4 File Offset: 0x000641D4
	private bool UpdateTrackingMove(MovieEventNode men)
	{
		if (men.goActor == null)
		{
			Debug.LogError(men.strActorName + " 被刪掉了 無法移動 強制跳出");
			return true;
		}
		if (this.bStartJump)
		{
			men.goActor.transform.position = men.vPos;
			return true;
		}
		if (men.goActor.transform.position == men.vPos)
		{
			return true;
		}
		float num = Vector3.Distance(men.goActor.transform.position, men.vPos);
		if (num > Time.deltaTime * men.fMoveSpeed && num > 0.01f)
		{
			num *= 0.025f;
			num = Mathf.Max(1f, num);
			men.goActor.transform.position = Vector3.Lerp(men.goActor.transform.position, men.vPos, Time.deltaTime * men.fMoveSpeed * num);
			if (men.bLookAtMoving)
			{
				men.goActor.transform.LookAt(men.vPos);
			}
			return false;
		}
		men.goActor.transform.position = men.vPos;
		return true;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00066114 File Offset: 0x00064314
	private MovieEventNode FindMovieEventNodeByID(int Id, int GroupID)
	{
		if (Id < 0)
		{
			return null;
		}
		if (this.playingMeg != null && this.playingMeg.ID == GroupID)
		{
			foreach (MovieEventNode movieEventNode in this.playingMeg.movieEventNodeList)
			{
				if (movieEventNode.NodeID == Id)
				{
					return movieEventNode;
				}
			}
			return null;
		}
		foreach (MovieEventGroup movieEventGroup in this.movieEventGroupList)
		{
			if (movieEventGroup.ID == GroupID)
			{
				foreach (MovieEventNode movieEventNode2 in movieEventGroup.movieEventNodeList)
				{
					if (movieEventNode2.NodeID == Id)
					{
						return movieEventNode2;
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0006625C File Offset: 0x0006445C
	private IEnumerator PlayMove(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				foreach (MovieEventNode mentemp in this.playingNodeList)
				{
					if (mentemp.goActor == men.goActor && mentemp.mEventnType == men.mEventnType)
					{
						this.playingNodeList.Remove(mentemp);
						if (mentemp.NextNodeID >= 0)
						{
							this.PlayMovieNode(this.FindMovieEventNodeByID(mentemp.NextNodeID, mentemp.GroupID));
						}
						break;
					}
				}
				men.vOrigPos = men.goActor.transform.position;
				float flength = Vector3.Distance(men.vOrigPos, men.vPos);
				if (flength < 0.01f)
				{
					flength = 0f;
				}
				else if (men.fMoveSpeed != 0f)
				{
					men.fPos = 0f;
					men.fTotal = flength / men.fMoveSpeed;
				}
				else
				{
					men.fTotal = 0f;
				}
				this.playingNodeList.Add(men);
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00066288 File Offset: 0x00064488
	private IEnumerator PlayOribitCameraMove(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (men.goActor == null)
		{
			men.goActor = this.FindActor(men.strActorName, men.strActorTag);
		}
		if (men.goActor != null)
		{
			foreach (MovieEventNode mentemp in this.playingNodeList)
			{
				if (mentemp.goActor == men.goActor && mentemp.mEventnType == men.mEventnType)
				{
					this.playingNodeList.Remove(mentemp);
					if (mentemp.NextNodeID >= 0)
					{
						this.PlayMovieNode(this.FindMovieEventNodeByID(mentemp.NextNodeID, mentemp.GroupID));
					}
					break;
				}
			}
			Debug.Log("Main Camera = " + Camera.main);
			if (Camera.main.GetComponent<OrbitCam>() != null)
			{
				men.bCrossFade = Camera.main.GetComponent<OrbitCam>().enabled;
				men.goLookat = Camera.main.GetComponent<OrbitCam>().Target.gameObject;
				Camera.main.GetComponent<OrbitCam>().Target = men.goActor.transform;
				Camera.main.GetComponent<OrbitCam>().CollisionDetection = false;
				men.vOrigPos.x = Camera.main.GetComponent<OrbitCam>().Distance;
				men.vOrigPos.y = Camera.main.GetComponent<OrbitCam>().Tilt;
				men.vOrigPos.z = Camera.main.GetComponent<OrbitCam>().Rotation;
				men.fPos = 0f;
				Debug.Log("OrbitCam vPos Orig = " + men.vOrigPos.ToString());
				this.playingNodeList.Add(men);
			}
		}
		else if (men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
		yield break;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x000662B4 File Offset: 0x000644B4
	private IEnumerator SetPos(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				men.goActor.transform.position = men.vPos;
			}
			else if (men.bCrossFade)
			{
				GameObject go = Game.g_ModelBundle.Load(men.strLookatName + "_ModelPrefab") as GameObject;
				if (go != null)
				{
					GameObject go2 = Object.Instantiate(go) as GameObject;
					MeleeWeaponTrail[] mwtArray = go2.GetComponents<MeleeWeaponTrail>();
					foreach (MeleeWeaponTrail mwt in mwtArray)
					{
						mwt.Emit = false;
					}
					mwtArray = go2.GetComponentsInChildren<MeleeWeaponTrail>();
					foreach (MeleeWeaponTrail mwt2 in mwtArray)
					{
						mwt2.Emit = false;
					}
					go2.transform.position = men.vPos;
					go2.name = men.strActorName;
					go2.tag = men.strActorTag;
					if (this.goMapNpc != null)
					{
						go2.transform.parent = this.goMapNpc.transform;
					}
					if (go2.GetComponent<PlayerController>())
					{
						go2.GetComponent<PlayerController>().m_strModelName = men.strLookatName;
						go2.GetComponent<PlayerController>().enabled = false;
					}
					if (go2.GetComponent<NpcCollider>())
					{
						go2.GetComponent<NpcCollider>().m_strModelName = men.strLookatName;
						go2.GetComponent<NpcCollider>().enabled = false;
					}
					else
					{
						NpcCollider npc = go2.AddComponent<NpcCollider>();
						npc.m_strModelName = men.strLookatName;
						npc.enabled = false;
					}
					if (go2.GetComponent<NavMeshAgent>())
					{
						go2.GetComponent<NavMeshAgent>().enabled = false;
					}
					men.goActor = go2;
					men.goActor.transform.position = men.vPos;
				}
				else
				{
					Debug.LogError(men.strLookatName + " Model cant create ");
				}
			}
			if (men.iAniLayer == 1)
			{
				Debug.Log(men.strActorName + " Model Delete");
				int iIndex = MapData.m_instance.CheckNpcAlwaysList(men.strActorName);
				if (iIndex > 0)
				{
					MapData.m_instance.DeleteAlwaysNpc(int.Parse(men.strActorName), iIndex);
				}
				else
				{
					Object.Destroy(men.goActor);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x000662E0 File Offset: 0x000644E0
	private IEnumerator SetRotate(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				men.goActor.transform.localEulerAngles = men.vPos;
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0006630C File Offset: 0x0006450C
	private IEnumerator PlayAni(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			float fAniLength = 0f;
			if (men.goActor != null && men.goActor.animation != null && men.strAnimationClipName != string.Empty)
			{
				if (men.goActor.animation[men.strAnimationClipName] == null)
				{
					Game.g_ModelBundle.LoadAnimation(men.goActor.animation, men.strAnimationClipName);
				}
				if (men.goActor.animation[men.strAnimationClipName] == null)
				{
					Debug.LogError(men.goActor.name + " not have " + men.strAnimationClipName + " AnimationClip");
				}
				else
				{
					if (!men.bCrossFade)
					{
						men.goActor.animation.Stop();
					}
					if (men.iAniLayer != 0)
					{
						Debug.LogWarning(string.Format("NodeID = {0} , {1} 設定 {2} 動作 Layer != 0 請小心使用", men.NodeID, men.goActor.name, men.strAnimationClipName));
					}
					men.goActor.animation[men.strAnimationClipName].layer = men.iAniLayer;
					men.goActor.animation[men.strAnimationClipName].wrapMode = men.wrapMode;
					men.goActor.animation[men.strAnimationClipName].speed = men.fAniSpeed;
					if (men.strAnimationClipName.IndexOf("die") >= 0)
					{
						men.goActor.animation.playAutomatically = false;
					}
					if (men.fAniSpeed < 0f)
					{
						if (men.bLookAtMoving)
						{
							men.goActor.animation[men.strAnimationClipName].time = men.goActor.animation[men.strAnimationClipName].length;
							fAniLength = men.goActor.animation[men.strAnimationClipName].length / -men.fAniSpeed;
						}
						else
						{
							float fFrameRate = men.goActor.animation[men.strAnimationClipName].clip.frameRate;
							float fPos = (float)men.iTextOrder;
							float fTime = fPos / fFrameRate;
							men.goActor.animation[men.strAnimationClipName].time = fTime;
							fAniLength = fTime / -men.fAniSpeed;
						}
						if ((men.wrapMode == 1 || men.wrapMode == 1) && this.bStartJump)
						{
							men.goActor.animation[men.strAnimationClipName].time = 0f;
						}
					}
					else
					{
						if (men.bLookAtMoving)
						{
							men.goActor.animation[men.strAnimationClipName].time = 0f;
							fAniLength = men.goActor.animation[men.strAnimationClipName].length / men.fAniSpeed;
						}
						else
						{
							float fFrameRate2 = men.goActor.animation[men.strAnimationClipName].clip.frameRate;
							float fPos2 = (float)men.iTextOrder;
							float fTime2 = fPos2 / fFrameRate2;
							men.goActor.animation[men.strAnimationClipName].time = fTime2;
							fTime2 = men.goActor.animation[men.strAnimationClipName].length - fTime2;
							fAniLength = fTime2 / men.fAniSpeed;
						}
						if ((men.wrapMode == 1 || men.wrapMode == 1) && this.bStartJump)
						{
							men.goActor.animation[men.strAnimationClipName].time = men.goActor.animation[men.strAnimationClipName].length;
						}
					}
					if (men.bCrossFade)
					{
						men.goActor.animation.cullingType = 0;
						men.goActor.animation.CrossFade(men.strAnimationClipName);
					}
					else
					{
						men.goActor.animation.cullingType = 0;
						men.goActor.animation.Play(men.strAnimationClipName);
					}
				}
				if (men.NextNodeID >= 0)
				{
					if (men.wrapMode == 1 || men.wrapMode == 1)
					{
						if (!this.bStartJump)
						{
							yield return new WaitForSeconds(fAniLength);
						}
						Debug.Log(men.strAnimationClipName + " Length = " + fAniLength.ToString());
						this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
					}
					else
					{
						Debug.LogError("NodeID = " + men.NodeID.ToString() + " PlayAni 是 Loop 動作 但是有 NextNodeID 看來等不到結束的那一天 ");
						this.NextClipList.Remove(men.NextNodeID);
					}
				}
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00066338 File Offset: 0x00064538
	private IEnumerator TextOut(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (this.bStartJump || GameGlobal.m_bBattle)
			{
				if ((men.mTextOutType == _MovieTextOutType.Popup || men.mTextOutType == _MovieTextOutType.Face) && Game.UI.Get<UITalk>() != null)
				{
					Game.UI.Get<UITalk>().CheckJumpMovieTalkQuest(men.GroupID, men.iTextOrder);
				}
				if (men.NextNodeID >= 0)
				{
					this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
				}
				yield break;
			}
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			switch (men.mTextOutType)
			{
			case _MovieTextOutType.OneLine:
			{
				MapTalkNode _MapTalkNode = null;
				if (Game.UI.Get<UITalk>() != null)
				{
					_MapTalkNode = Game.UI.Get<UITalk>().GetMovieTalkNode(men.GroupID, men.iTextOrder);
				}
				if (_MapTalkNode == null)
				{
					Debug.LogError("OneLine not Have Talk Node = " + men.iTextOrder.ToString());
				}
				else
				{
					string str = _MapTalkNode.m_strManager;
					if (men.goActor == null)
					{
						Debug.LogError(string.Concat(new string[]
						{
							men.strActorName,
							" Tag ( ",
							men.strActorTag,
							" ) 找不到 無法設 OneLine 對話 : ",
							str
						}));
					}
					else
					{
						Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AddOneLineString(men.goActor, str, _MapTalkNode.m_fDestroyTime);
						Debug.Log("OneLine " + men.goActor.name + " " + str);
					}
				}
				break;
			}
			case _MovieTextOutType.Popup:
			case _MovieTextOutType.Face:
			{
				if (men.bCrossFade)
				{
					Time.timeScale = 1E-06f;
				}
				MovieEventMap.iTalkNextID = men.NextNodeID;
				MovieEventMap.iGroupNextID = men.GroupID;
				if (!this.bMoviePlaying)
				{
					Time.timeScale = 1f;
					Debug.LogError(string.Concat(new object[]
					{
						"對話錯誤 已經不在動話中",
						men.GroupID,
						" ",
						men.iTextOrder
					}));
					if (men.NextNodeID >= 0)
					{
						this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
					}
					yield break;
				}
				bool ret = false;
				if (Game.UI.Get<UITalk>() != null)
				{
					ret = Game.UI.Get<UITalk>().SetMovieTalk(men.goActor, men.GroupID, men.iTextOrder);
				}
				if (!ret)
				{
					Time.timeScale = 1f;
					Debug.LogError(string.Concat(new object[]
					{
						"對話錯誤 ",
						men.GroupID,
						" ",
						men.iTextOrder
					}));
					if (men.NextNodeID >= 0)
					{
						this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
					}
				}
				break;
			}
			}
			if (men.mTextOutType == _MovieTextOutType.OneLine)
			{
				Debug.Log("OneLine");
				if (men.NextNodeID >= 0)
				{
					this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
				}
			}
		}
		yield break;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00066364 File Offset: 0x00064564
	private IEnumerator TextOutOption(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Damn TextOutOption Call Next Clip ");
				this.NextClipList.Remove(men.NextNodeID);
			}
			if (this.bStartJump)
			{
				base.StartCoroutine(this.JumpMovieFadeIn());
			}
			if (men.bCrossFade)
			{
				Time.timeScale = 1E-06f;
			}
			this.talkOptionMen = men;
			bool ret = false;
			if (Game.UI.Get<UITalk>() != null)
			{
				ret = Game.UI.Get<UITalk>().SetMovieTalk(men.goActor, men.GroupID, men.iTextOrder);
			}
			if (!ret)
			{
				Time.timeScale = 1f;
				Debug.LogError(string.Concat(new object[]
				{
					"對話錯誤 ",
					men.GroupID,
					" ",
					men.iTextOrder
				}));
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption1, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
		}
		yield break;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00066390 File Offset: 0x00064590
	private IEnumerator PlayBGMusic(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.acSound == null)
			{
				men.acSound = (Game.g_AudioBundle.Load("audio/Map/" + men.strSoundName) as AudioClip);
			}
			if (men.acSound != null)
			{
				Game.PlayBGMusicClip(men.acSound);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x000663BC File Offset: 0x000645BC
	private IEnumerator PlayMoveLockAt(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goLookat == null)
			{
				men.goLookat = this.FindActor(men.strLookatName, men.strLookatTag);
			}
			if (men.goActor != null)
			{
				foreach (MovieEventNode mentemp in this.playingNodeList)
				{
					if (mentemp.goActor == men.goActor && mentemp.mEventnType == men.mEventnType)
					{
						this.playingNodeList.Remove(mentemp);
						if (mentemp.NextNodeID >= 0)
						{
							this.PlayMovieNode(this.FindMovieEventNodeByID(mentemp.NextNodeID, mentemp.GroupID));
						}
						break;
					}
				}
				this.playingNodeList.Add(men);
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x000663E8 File Offset: 0x000645E8
	private IEnumerator PlayMoveRotate(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				men.fPos = 0f;
				men.vPos = men.goActor.transform.localEulerAngles;
				this.playingNodeList.Add(men);
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00066414 File Offset: 0x00064614
	private IEnumerator PlayCameraLookat(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goLookat == null)
			{
				men.goLookat = this.FindActor(men.strLookatName, men.strLookatTag);
			}
			if (men.goActor != null && men.goLookat != null)
			{
				this.playingNodeList.Add(men);
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00066440 File Offset: 0x00064640
	private IEnumerator PlayTimeScale(MovieEventNode men)
	{
		Debug.Log("PlayTimeScale delaytime = " + men.fDelayTime.ToString() + " real time " + RealTime.time.ToString());
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			Time.timeScale = men.fAniSpeed;
			Debug.Log(string.Concat(new string[]
			{
				"time scale = ",
				Time.timeScale.ToString(),
				" ",
				men.fTotal.ToString(),
				" sec real time ",
				RealTime.time.ToString()
			}));
			yield return new WaitForSeconds(men.fTotal);
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0006646C File Offset: 0x0006466C
	private IEnumerator Transfer(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				Debug.Log("Transfer " + men.strLookatName);
				men.goActor.SendMessage(men.strLookatName, 1);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00066498 File Offset: 0x00064698
	private IEnumerator SetCameraLock(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			Debug.Log("Main Camera = " + Camera.main);
			if (Camera.main.GetComponent<OrbitCam>() != null)
			{
				Camera.main.GetComponent<OrbitCam>().enabled = men.bLookAtMoving;
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x000664C4 File Offset: 0x000646C4
	private IEnumerator PlayNextMovie(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (this.bStartJump)
		{
			base.StartCoroutine(this.JumpMovieFadeIn());
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			foreach (MovieEventGroup meg in this.movieEventGroupList)
			{
				if (meg.ID == men.iTextOrder)
				{
					this.playingMeg = meg;
					break;
				}
			}
			if (this.playingMeg != null)
			{
				if (!TeamStatus.m_Instance.m_EventList.Contains(this.playingMeg.ID))
				{
					TeamStatus.m_Instance.m_EventList.Add(this.playingMeg.ID);
				}
				Debug.Log("PlayNextMovie ID = " + this.playingMeg.ID.ToString());
				this.PlayMovieGroup(this.playingMeg);
			}
		}
		yield break;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x000664F0 File Offset: 0x000646F0
	private IEnumerator PlayStartBattle(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (this.bStartJump)
		{
			while (this.DelayClipList.Count > 0)
			{
				yield return null;
			}
			if (men.NextNodeID >= 0)
			{
				Debug.LogWarning("NodeID = " + men.NodeID.ToString() + " Damn StartBattle Have Next Clip ");
				this.NextClipList.Remove(men.NextNodeID);
			}
			while (this.NextClipList.Count > 0)
			{
				yield return null;
			}
			base.StartCoroutine(this.JumpMovieFadeIn());
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			yield return null;
			if (this.goPlayer != null)
			{
				GameGlobal.m_TransferPos = this.goPlayer.transform.localPosition;
			}
			Game.g_BattleControl.StartBattle(men.iTextOrder);
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0006651C File Offset: 0x0006471C
	private IEnumerator PlayCameraZoom(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (Camera.main.GetComponent<OrbitCam>() != null)
			{
				if (men.bLookAtMoving)
				{
					Camera.main.GetComponent<OrbitCam>().SnapTo(men.fMoveSpeed, men.fTotal, men.fPos);
				}
				else
				{
					Camera.main.GetComponent<OrbitCam>().Distance = men.fMoveSpeed;
					Camera.main.GetComponent<OrbitCam>().Tilt = men.fPos;
					Camera.main.GetComponent<OrbitCam>().Rotation = men.fTotal;
					Debug.Log(string.Concat(new string[]
					{
						"OrbitCam Distance Rotation Tilt ",
						men.fMoveSpeed.ToString(),
						" ",
						men.fTotal.ToString(),
						" ",
						men.fPos.ToString()
					}));
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00066548 File Offset: 0x00064748
	private IEnumerator FadeInOut(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (this.bStartJump || this.bJump)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
				yield break;
			}
			if (this.SO == null)
			{
				this.SO = Camera.main.GetComponent<ScreenOverlay>();
				if (this.SO == null)
				{
					this.SO = Camera.main.gameObject.AddComponent<ScreenOverlay>();
				}
			}
			if (this.SO != null)
			{
				if (men.fTotal > 0f)
				{
					men.fPos = 0f;
					this.SO.enabled = true;
					this.SO.intensity = 1f;
				}
				else
				{
					men.fPos = 0f;
					this.SO.enabled = true;
					this.SO.intensity = 0f;
				}
				this.playingNodeList.Add(men);
			}
			else if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00066574 File Offset: 0x00064774
	private IEnumerator FarPlane(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = men.fTotal;
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x000665A0 File Offset: 0x000647A0
	private IEnumerator SetMovieEvent(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (TeamStatus.m_Instance != null)
			{
				if (men.bCrossFade)
				{
					Debug.Log("Movie Event 刪除 Movie " + TeamStatus.m_Instance.m_EventList.Contains(men.iAniLayer));
					TeamStatus.m_Instance.m_EventList.Remove(men.iAniLayer);
				}
				else
				{
					Debug.Log("Movie Event 進行中任務 Movie " + men.iAniLayer.ToString());
					TeamStatus.m_Instance.m_EventList.Add(men.iAniLayer);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000665CC File Offset: 0x000647CC
	private IEnumerator SetQuest(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (MissionStatus.m_instance != null)
			{
				if (men.bCrossFade)
				{
					Debug.Log("Movie Event 刪除進行中任務 " + Game.QuestData.GetQuestInfo(men.strScenesName));
					MissionStatus.m_instance.RemoveQuest(men.strScenesName);
				}
				else
				{
					Debug.Log("Movie Event 進行中任務 " + Game.QuestData.GetQuestInfo(men.strScenesName));
					MissionStatus.m_instance.AddQuestList(men.strScenesName);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000665F8 File Offset: 0x000647F8
	private IEnumerator SetCollection(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (MissionStatus.m_instance != null)
			{
				if (men.bCrossFade)
				{
					Debug.Log("Movie Event 刪除已完成任務 " + Game.QuestData.GetQuestInfo(men.strScenesName));
					MissionStatus.m_instance.RemoveCollectionQuest(men.strScenesName);
				}
				else if (!MissionStatus.m_instance.CheckCollectionQuest(men.strScenesName))
				{
					Debug.Log("Movie Event 完成任務 " + Game.QuestData.GetQuestInfo(men.strScenesName));
					MissionStatus.m_instance.AddCollectionQuest(men.strScenesName);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00066624 File Offset: 0x00064824
	private IEnumerator StopMovie(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			this.StopMovieGroup(men.iTextOrder);
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00066650 File Offset: 0x00064850
	private IEnumerator CreateEffect(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				men.goActor.transform.position = men.vPos;
			}
			else if (men.bCrossFade)
			{
				GameObject go = null;
				string strEffectName = "Effects/" + men.strLookatName;
				if (Game.g_EffectsBundle != null)
				{
					if (Game.g_EffectsBundle.Contains(strEffectName))
					{
						go = (Game.g_EffectsBundle.Load(strEffectName) as GameObject);
					}
					else
					{
						Debug.LogWarning("Movie File No Found effect = " + strEffectName);
					}
				}
				else
				{
					Debug.LogWarning("Movie g_EffectsBundle = null");
				}
				if (go != null)
				{
					GameObject go2 = Object.Instantiate(go, men.vPos, go.transform.rotation) as GameObject;
					go2.transform.position = men.vPos;
					go2.name = men.strActorName;
					go2.tag = men.strActorTag;
					if (this.goMapNpc != null)
					{
						go2.transform.parent = this.goMapNpc.transform;
					}
					if (go2.GetComponent<NpcCollider>())
					{
						go2.GetComponent<NpcCollider>().enabled = false;
					}
					if (go2.GetComponent<NavMeshAgent>())
					{
						go2.GetComponent<NavMeshAgent>().enabled = false;
					}
					men.goActor = go2;
					men.goActor.transform.position = men.vPos;
				}
				else
				{
					Debug.LogError(men.strLookatName + " Effect cant create Check Path name = " + strEffectName);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0006667C File Offset: 0x0006487C
	private IEnumerator PlaySkill(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		int skillID = men.iOption1;
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				UnitTB srcUnit = men.goActor.GetComponent<UnitTB>();
				if (srcUnit == null)
				{
					UnitTBAudio unitAudio = men.goActor.AddComponent<UnitTBAudio>();
					UnitTBAnimation unitAnimation = men.goActor.AddComponent<UnitTBAnimation>();
					unitAnimation.SetMovieAnimationSkill(men);
					srcUnit = men.goActor.AddComponent<UnitTB>();
					if (!this.PlaySkillList.Contains(men.goActor))
					{
						this.PlaySkillList.Add(men.goActor);
					}
					yield return null;
				}
				else
				{
					UnitTBAnimation unitAnimation2 = men.goActor.GetComponent<UnitTBAnimation>();
					unitAnimation2.SetMovieAnimationSkill(men);
					yield return null;
				}
				List<AttackInstance> atkList = new List<AttackInstance>();
				MovieEventNode node = men;
				for (;;)
				{
					node = this.GetNextHurtNode(node);
					if (node == null)
					{
						break;
					}
					if (node.goActor == null)
					{
						node.goActor = this.FindActor(node.strActorName, node.strActorTag);
					}
					if (!(node.goActor == null))
					{
						UnitTB targetUnit = node.goActor.GetComponent<UnitTB>();
						if (targetUnit == null)
						{
							UnitTBAudio unitAudio2 = node.goActor.AddComponent<UnitTBAudio>();
							UnitTBAnimation unitAnimation3 = node.goActor.AddComponent<UnitTBAnimation>();
							unitAnimation3.SetMovieAnimationSkill(node);
							targetUnit = node.goActor.AddComponent<UnitTB>();
							if (!this.PlaySkillList.Contains(node.goActor))
							{
								this.PlaySkillList.Add(node.goActor);
							}
							yield return null;
						}
						else
						{
							UnitTBAnimation unitAnimation4 = node.goActor.GetComponent<UnitTBAnimation>();
							unitAnimation4.SetMovieAnimationSkill(node);
							yield return null;
						}
						AttackInstance atkInst = new AttackInstance();
						atkInst.srcUnit = srcUnit;
						atkInst.targetUnit = targetUnit;
						atkInst.missed = node.bCrossFade;
						atkInst.critical = node.bLookAtMoving;
						if (node.iTextOrder == 0)
						{
							atkInst.destroyed = false;
						}
						else
						{
							atkInst.destroyed = true;
						}
						atkList.Add(atkInst);
						yield return null;
					}
				}
				yield return null;
				srcUnit.animationTB.PlayMovieSkill(skillID, atkList);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x000666A8 File Offset: 0x000648A8
	private IEnumerator SetEnable(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.goActor == null)
			{
				men.goActor = this.FindActor(men.strActorName, men.strActorTag);
			}
			if (men.goActor != null)
			{
				men.goActor.SetActive(men.bCrossFade);
				if (!this.goEnableList.Contains(men.goActor))
				{
					this.goEnableList.Add(men.goActor);
				}
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x000666D4 File Offset: 0x000648D4
	private IEnumerator CheckPropert(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID) && men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
		yield break;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00066700 File Offset: 0x00064900
	private IEnumerator CheckFriendly(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.iOption2 <= Game.NpcData.GetNpcFriendly(men.iOption1))
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption3, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption4, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0006672C File Offset: 0x0006492C
	private IEnumerator CheckQuest(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			bool bhave = false;
			if (men.iTextOrder == 0)
			{
				bhave = MissionStatus.m_instance.CheckQuest(men.strAnimationClipName);
			}
			if (men.iTextOrder == 1)
			{
				bhave = MissionStatus.m_instance.CheckCollectionQuest(men.strAnimationClipName);
			}
			if (men.iTextOrder == 3)
			{
				bhave = TeamStatus.m_Instance.m_EventList.Contains(men.iAniLayer);
			}
			if (bhave)
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption1, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption2, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00066758 File Offset: 0x00064958
	private IEnumerator SetQuestion(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (this.bStartJump)
			{
				base.StartCoroutine(this.JumpMovieFadeIn());
			}
			bool bFound = false;
			GameObject[] gos = GameObject.FindGameObjectsWithTag("cForm");
			for (int i = 0; i < gos.Length; i++)
			{
				if (gos[i].name.Equals("cFormQuestion"))
				{
					UIQuestion questionUI = gos[i].GetComponent<UIQuestion>();
					if (questionUI != null)
					{
						questionUI.StartMovieQuestionEvent(new UIEventListener.VoidDelegate(this.QuestionOnClick));
						questionUI.SetQuestion(men.iTextOrder, 0);
						bFound = true;
						break;
					}
				}
			}
			if (!bFound)
			{
				Debug.LogError("找不到文考 cFormQuestion no found");
			}
			if (men.NextNodeID >= 0)
			{
				this.iQuestionNextID = men.NextNodeID;
				this.iQuestionGroupID = men.GroupID;
				this.NextClipList.Remove(men.NextNodeID);
			}
		}
		yield break;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00066784 File Offset: 0x00064984
	private IEnumerator SetInside(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			Debug.Log("MovieEvent SetInside  Orig House Inside = " + GameGlobal.m_bHouseInside.ToString());
			GameGlobal.m_bHouseInside = men.bCrossFade;
			Debug.Log("MovieEvent SetInside  Now House Inside = " + GameGlobal.m_bHouseInside.ToString());
			MapData.m_instance.SetInOutSide();
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x000667B0 File Offset: 0x000649B0
	private IEnumerator CheckMoney(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.iOption1 <= BackpackStatus.m_Instance.GetMoney())
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption3, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption4, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000667DC File Offset: 0x000649DC
	private IEnumerator CheckItemAmount(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (men.iOption2 <= BackpackStatus.m_Instance.CheclItemAmount(men.iOption1))
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption3, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption4, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00066808 File Offset: 0x00064A08
	private IEnumerator CheckParty(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (TeamStatus.m_Instance.CheckTeamMember(men.iOption1))
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption3, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption4, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00066834 File Offset: 0x00064A34
	private IEnumerator CheckAchievement(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			if (Game.Achievement.GetAchievementOpen(men.iOption1) > 0)
			{
				MovieEventNode nextMen = this.FindMovieEventNodeByID(men.iOption3, men.GroupID);
				this.PlayMovieNode(nextMen);
			}
			else
			{
				MovieEventNode nextMen2 = this.FindMovieEventNodeByID(men.iOption4, men.GroupID);
				this.PlayMovieNode(nextMen2);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00066860 File Offset: 0x00064A60
	private IEnumerator CheckTalent(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID) && men.NextNodeID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
		}
		yield break;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0006688C File Offset: 0x00064A8C
	private IEnumerator CheckFlag(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			yield return null;
			int flag = Game.Variable[men.strAnimationClipName];
			Func<int, int, bool> f = null;
			if (men.iOption1 == 0)
			{
				f = ((int a, int b) => a == b);
			}
			else if (men.iOption1 == 1)
			{
				f = ((int a, int b) => a > b);
			}
			else if (men.iOption1 == 2)
			{
				f = ((int a, int b) => a < b);
			}
			int nextNodeId = (!f.Invoke(flag, men.iOption2)) ? men.iOption4 : men.iOption3;
			if (nextNodeId >= 0)
			{
				MovieEventNode node = this.FindMovieEventNodeByID(nextNodeId, men.GroupID);
				this.PlayMovieNode(node);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x000668B8 File Offset: 0x00064AB8
	private IEnumerator SetFlag(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			string flagName = men.strAnimationClipName;
			if (men.iOption1 == 0)
			{
				GlobalVariableManager variable;
				GlobalVariableManager globalVariableManager = variable = Game.Variable;
				string key2;
				string key = key2 = flagName;
				int num = variable[key2];
				globalVariableManager[key] = num + men.iOption2;
			}
			else if (men.iOption1 == 1)
			{
				GlobalVariableManager variable2;
				GlobalVariableManager globalVariableManager2 = variable2 = Game.Variable;
				string key2;
				string key3 = key2 = flagName;
				int num = variable2[key2];
				globalVariableManager2[key3] = num - men.iOption2;
			}
			else if (men.iOption1 == 2)
			{
				Game.Variable.Data.Remove(flagName);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000668E4 File Offset: 0x00064AE4
	private IEnumerator CheckRoutine(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			int flag = NPC.m_instance.GetRoutineLv(men.iTextOrder, men.iAniLayer);
			Func<int, int, bool> f = null;
			if (men.iOption1 == 0)
			{
				f = ((int a, int b) => a == b);
			}
			else if (men.iOption1 == 1)
			{
				f = ((int a, int b) => a > b);
			}
			else if (men.iOption1 == 2)
			{
				f = ((int a, int b) => a < b);
			}
			int nextNodeId = (!f.Invoke(flag, men.iOption2)) ? men.iOption4 : men.iOption3;
			if (nextNodeId >= 0)
			{
				MovieEventNode node = this.FindMovieEventNodeByID(nextNodeId, men.GroupID);
				this.PlayMovieNode(node);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00066910 File Offset: 0x00064B10
	private IEnumerator CheckNeigong(MovieEventNode men)
	{
		if (!this.bStartJump)
		{
			this.DelayClipList.Add(men.NodeID);
			yield return new WaitForSeconds(men.fDelayTime);
			this.DelayClipList.Remove(men.NodeID);
		}
		if (!this.CheckMovieStop(men.GroupID))
		{
			int flag = NPC.m_instance.GetNeigongLv(men.iTextOrder, men.iAniLayer);
			Func<int, int, bool> f = null;
			if (men.iOption1 == 0)
			{
				f = ((int a, int b) => a == b);
			}
			else if (men.iOption1 == 1)
			{
				f = ((int a, int b) => a > b);
			}
			else if (men.iOption1 == 2)
			{
				f = ((int a, int b) => a < b);
			}
			int nextNodeId = (!f.Invoke(flag, men.iOption2)) ? men.iOption4 : men.iOption3;
			if (nextNodeId >= 0)
			{
				MovieEventNode node = this.FindMovieEventNodeByID(nextNodeId, men.GroupID);
				this.PlayMovieNode(node);
			}
			if (men.NextNodeID >= 0)
			{
				this.PlayMovieNode(this.FindMovieEventNodeByID(men.NextNodeID, men.GroupID));
			}
		}
		yield break;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0006693C File Offset: 0x00064B3C
	private MovieEventNode GetNextHurtNode(MovieEventNode men)
	{
		MovieEventNode movieEventNode = this.FindMovieEventNodeByID(men.TriggerNodeID, men.GroupID);
		if (movieEventNode == null)
		{
			return null;
		}
		if (movieEventNode.mEventnType == _MovieEventNodeType.PlayHurt)
		{
			return movieEventNode;
		}
		return null;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00066974 File Offset: 0x00064B74
	public void QuestionOnClick(GameObject go)
	{
		if (this.iQuestionNextID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(this.iQuestionNextID, this.iQuestionGroupID));
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Equals("cFormQuestion"))
				{
					UIQuestion component = array[i].GetComponent<UIQuestion>();
					if (component != null)
					{
						component.EndMovieQuestionEvnet();
						break;
					}
				}
			}
			this.iQuestionNextID = -1;
			this.iQuestionGroupID = -1;
		}
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00066A0C File Offset: 0x00064C0C
	public void TextOutOnClick(GameObject go)
	{
		if (Game.UI.Get<UITalk>() == null)
		{
			Debug.LogError("UITalk no Found");
			return;
		}
		if (Game.UI.Get<UITalk>().m_bSelect)
		{
			return;
		}
		if (!Game.UI.Get<UITalk>().m_bMovie)
		{
			Debug.LogWarning("Not Movie Talk");
			Time.timeScale = 1f;
			return;
		}
		Time.timeScale = 1f;
		Game.UI.Get<UITalk>().ResetMovieTalk();
		if (MovieEventMap.iTalkNextID >= 0)
		{
			this.PlayMovieNode(this.FindMovieEventNodeByID(MovieEventMap.iTalkNextID, MovieEventMap.iGroupNextID));
			MovieEventMap.iTalkNextID = -1;
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00066AB8 File Offset: 0x00064CB8
	public void TextOutOptionOnClick(GameObject go)
	{
		if (!Game.UI.Get<UITalk>().m_bMovie)
		{
			Debug.LogWarning("Not Movie Talk");
			Time.timeScale = 1f;
			return;
		}
		Time.timeScale = 1f;
		Game.UI.Get<UITalk>().ResetMovieTalk();
		switch (go.GetComponent<SelectBtn>().m_iIndex)
		{
		case 0:
			if (this.talkOptionMen.iOption1 >= 0)
			{
				MovieEventNode men = this.FindMovieEventNodeByID(this.talkOptionMen.iOption1, this.talkOptionMen.GroupID);
				this.PlayMovieNode(men);
			}
			break;
		case 1:
			if (this.talkOptionMen.iOption2 >= 0)
			{
				MovieEventNode men = this.FindMovieEventNodeByID(this.talkOptionMen.iOption2, this.talkOptionMen.GroupID);
				this.PlayMovieNode(men);
			}
			break;
		case 2:
			if (this.talkOptionMen.iOption3 >= 0)
			{
				MovieEventNode men = this.FindMovieEventNodeByID(this.talkOptionMen.iOption3, this.talkOptionMen.GroupID);
				this.PlayMovieNode(men);
			}
			break;
		case 3:
			if (this.talkOptionMen.iOption4 >= 0)
			{
				MovieEventNode men = this.FindMovieEventNodeByID(this.talkOptionMen.iOption4, this.talkOptionMen.GroupID);
				this.PlayMovieNode(men);
			}
			break;
		}
		Game.UI.Get<UITalk>().m_bSelect = false;
		this.talkOptionMen = null;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00009374 File Offset: 0x00007574
	private bool CheckMovieStop(int movieID)
	{
		if (this.StopMovieList.Contains(movieID))
		{
			Debug.LogWarning(movieID.ToString() + " 動畫片段已經停止");
			return true;
		}
		return false;
	}

	// Token: 0x04000DE7 RID: 3559
	public List<MovieEventGroup> movieEventGroupList = new List<MovieEventGroup>();

	// Token: 0x04000DE8 RID: 3560
	private List<MovieEventNode> playingNodeList = new List<MovieEventNode>();

	// Token: 0x04000DE9 RID: 3561
	private List<GameObject> NpcList = new List<GameObject>();

	// Token: 0x04000DEA RID: 3562
	private List<GameObject> PlaySkillList = new List<GameObject>();

	// Token: 0x04000DEB RID: 3563
	private List<GameObject> goEnableList = new List<GameObject>();

	// Token: 0x04000DEC RID: 3564
	private List<int> StopMovieList = new List<int>();

	// Token: 0x04000DED RID: 3565
	private List<int> NextClipList = new List<int>();

	// Token: 0x04000DEE RID: 3566
	private List<int> DelayClipList = new List<int>();

	// Token: 0x04000DEF RID: 3567
	private ScreenOverlay SO;

	// Token: 0x04000DF0 RID: 3568
	private MovieEventGroup playingMeg;

	// Token: 0x04000DF1 RID: 3569
	private MovieEventNode talkOptionMen;

	// Token: 0x04000DF2 RID: 3570
	private MovieEventNode changeSceneNode;

	// Token: 0x04000DF3 RID: 3571
	private static int iTalkNextID;

	// Token: 0x04000DF4 RID: 3572
	private static int iGroupNextID;

	// Token: 0x04000DF5 RID: 3573
	private int iQuestionNextID;

	// Token: 0x04000DF6 RID: 3574
	private int iQuestionGroupID;

	// Token: 0x04000DF7 RID: 3575
	private GameObject goPlayer;

	// Token: 0x04000DF8 RID: 3576
	private GameObject goMapNpc;

	// Token: 0x04000DF9 RID: 3577
	private bool bStartJump;

	// Token: 0x04000DFA RID: 3578
	private bool bJump;

	// Token: 0x04000DFB RID: 3579
	private bool bMoviePlaying;
}
