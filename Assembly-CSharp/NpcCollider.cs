using System;
using System.Collections;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class NpcCollider : PlayerTarget
{
	// Token: 0x06000DDC RID: 3548 RVA: 0x00071754 File Offset: 0x0006F954
	public void ResetNavMeshAgent()
	{
		if (this.m_MapNpcDataNode == null)
		{
			return;
		}
		if (this.m_MapNpcDataNode.m_iMoveType == 2)
		{
			base.gameObject.GetComponent<NavMeshAgent>().enabled = false;
		}
		else
		{
			base.gameObject.GetComponent<NavMeshAgent>().enabled = true;
		}
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0000982B File Offset: 0x00007A2B
	private void Awake()
	{
		this.m_TargetType = PlayerTarget.eTargetType.NpcCollider;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000717A8 File Offset: 0x0006F9A8
	private void Start()
	{
		this.m_Animation = base.GetComponent<Animation>();
		this.m_strNpcID = base.name;
		this.m_Contrller = base.GetComponent<BoxCollider>();
		this.m_iStep = 0;
		this.m_iCount = 0;
		this.m_vec3LookDir = Vector3.zero;
		this.AllConditionOver = false;
		int num;
		if (int.TryParse(this.m_strNpcID, ref num))
		{
			this.m_MapNpcDataNode = Game.MapIcon.GetMapNpcDataNode(this.m_ConductNode.m_ConductID);
			if (this.m_MapNpcDataNode == null)
			{
				Debug.LogError("Map_icon 表內沒有 NpcID = " + this.m_strNpcID + " 的資料 請檢查一下");
				base.gameObject.SetActive(false);
				return;
			}
			if (this.m_MapNpcDataNode.m_iCheakPlayer == 1)
			{
				base.AddBattleNpcList(this);
				base.gameObject.tag = "BattleNpc";
				this.bBattle = true;
				this.bCheakPlayer = true;
			}
			this._PointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
			if (this.m_MapNpcDataNode.m_iMoveType == 2)
			{
				base.gameObject.GetComponent<NavMeshAgent>().enabled = false;
			}
			else
			{
				this.m_fMoveSpeed = base.gameObject.GetComponent<NavMeshAgent>().speed;
			}
			Vector3 localPosition;
			localPosition..ctor(this._PointNode.m_fX, this._PointNode.m_fY, this._PointNode.m_fZ);
			base.transform.localPosition = localPosition;
			base.transform.localEulerAngles = new Vector3(0f, (float)this._PointNode.m_iDirAngle, 0f);
			if (this.m_MapNpcDataNode != null)
			{
				if (this.m_MapNpcDataNode.m_iNpcType == 1)
				{
					this.m_StoreID = this.m_MapNpcDataNode.m_iStoreID;
				}
				else if (this.m_MapNpcDataNode.m_iNpcType == 2)
				{
					base.gameObject.tag = "BattleNpc";
				}
				if (this.m_MapNpcDataNode.m_iMoveType == 2)
				{
					this.m_Mob = base.GetComponent<NavMeshAgent>();
					this.m_bToPoint = true;
					if (this._PointNode.m_strPointAni == "die01" || this._PointNode.m_strPointAni.Contains("act01_body"))
					{
						if (this.m_Animation[this._PointNode.m_strPointAni] == null)
						{
							Game.g_ModelBundle.LoadAnimation(this.m_Animation, this._PointNode.m_strPointAni);
						}
						if (this.m_Animation[this._PointNode.m_strPointAni] == null)
						{
							Debug.Log(base.name + " not have " + this._PointNode.m_strPointAni + " animation clip plz kuro check");
						}
						else
						{
							this.m_Animation[this._PointNode.m_strPointAni].normalizedTime = 1f;
							this.m_Animation.wrapMode = 1;
							base.animation.Play(this._PointNode.m_strPointAni);
						}
					}
					else
					{
						this.SetOnTerrain();
						this.PlayAnimation(this._PointNode.m_strPointAni, true);
					}
					this.m_iCount = 1;
					this.m_Mob = base.GetComponent<NavMeshAgent>();
					this.m_Mob.enabled = false;
				}
				else
				{
					this.m_Mob = base.GetComponent<NavMeshAgent>();
					this.m_Mob.Warp(base.transform.position);
					this.m_Animation.wrapMode = 2;
				}
			}
			this.fTimePos = Random.Range(NpcCollider.m_fNpc543RepeatTime * 0.5f, NpcCollider.m_fNpc543RepeatTime);
			if (MapData.m_instance.m_NpcIsFoughtList.Count > 0)
			{
				this.m_NpcIsFought = MapData.m_instance.GetNpcIsFoughtNode(Application.loadedLevelName, base.gameObject.name);
				if (this.m_NpcIsFought != null)
				{
					bool flag = MapData.m_instance.CheckNpcIsFoughtName(this.m_NpcIsFought);
					if (flag)
					{
						if (this.m_NpcIsFought.iStay.Equals(0))
						{
							if (YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, this.m_NpcIsFought.ReSetRound))
							{
								base.gameObject.SetActive(true);
							}
							else
							{
								base.gameObject.SetActive(false);
							}
						}
						else if (YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, this.m_NpcIsFought.ReSetRound))
						{
							this.bCheakPlayer = true;
							base.gameObject.tag = "Npc";
						}
						else
						{
							this.bCheakPlayer = false;
						}
					}
				}
			}
			if (this.m_MapNpcDataNode.m_ShowSCondition.Count > 0 || this.m_MapNpcDataNode.m_CloseSCondition.Count > 0)
			{
				this.m_bSpecialOpen = true;
				base.gameObject.SetActive(this.CheckNpcOpen());
				if (this.m_bSpecialOpen)
				{
					MapData.m_instance.m_SpecialNpcList.Add(int.Parse(base.gameObject.name));
				}
			}
		}
		if (base.gameObject.tag == "Npc")
		{
			base.AddPlayerTargetList(this);
		}
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x00071CB8 File Offset: 0x0006FEB8
	public bool CheckNpcOpen()
	{
		if (this.m_MapNpcDataNode.m_CloseSCondition.Count != 0)
		{
			bool flag = ConditionManager.CheckCondition(this.m_MapNpcDataNode.m_CloseSCondition, true, 0, string.Empty);
			if (flag)
			{
				if (this.bBattle)
				{
					MapData.m_instance.m_NpcIsFoughtList.Remove(this.m_NpcIsFought);
					this.bBattle = false;
				}
				foreach (Condition condition in this.m_MapNpcDataNode.m_CloseSCondition)
				{
					if (condition._iType != ConditionKind.PartyMember)
					{
						this.AllConditionOver = true;
					}
				}
				return false;
			}
			if (this.m_MapNpcDataNode.m_ShowSCondition.Count != 0)
			{
				bool result = ConditionManager.CheckCondition(this.m_MapNpcDataNode.m_ShowSCondition, true, 0, string.Empty);
				this.m_bSpecialOpen = true;
				return result;
			}
		}
		else if (this.m_MapNpcDataNode.m_ShowSCondition.Count != 0)
		{
			bool result2 = ConditionManager.CheckCondition(this.m_MapNpcDataNode.m_ShowSCondition, true, 0, string.Empty);
			this.m_bSpecialOpen = true;
			return result2;
		}
		return true;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00071DF4 File Offset: 0x0006FFF4
	private void OnEnable()
	{
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindWithTag("Player");
		}
		if (this.goPlayer == null)
		{
			return;
		}
		if (this.goPlayer == null || this.m_MapNpcDataNode == null)
		{
			return;
		}
		if (this.m_MapNpcDataNode.m_iMoveType == 2)
		{
			this.m_bToPoint = true;
			if (this._PointNode.m_strPointAni == "die01" || this._PointNode.m_strPointAni.Contains("act01_body"))
			{
				if (this.m_Animation[this._PointNode.m_strPointAni] == null)
				{
					Game.g_ModelBundle.LoadAnimation(this.m_Animation, this._PointNode.m_strPointAni);
				}
				if (this.m_Animation[this._PointNode.m_strPointAni] == null)
				{
					Debug.Log(base.name + " not have " + this._PointNode.m_strPointAni + " animation clip plz kuro check");
				}
				else
				{
					this.m_Animation[this._PointNode.m_strPointAni].normalizedTime = 1f;
					this.m_Animation.wrapMode = 1;
					base.animation.Play(this._PointNode.m_strPointAni);
				}
			}
			else
			{
				this.SetOnTerrain();
				this.PlayAnimation(this._PointNode.m_strPointAni, true);
			}
			this.m_Animation.wrapMode = 1;
		}
		else if (this.m_Mob != null && !this.m_Mob.enabled)
		{
			this.m_Mob.enabled = true;
		}
		if (!GameGlobal.m_bMovie)
		{
			base.transform.localEulerAngles = new Vector3(0f, (float)this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep].m_iDirAngle, 0f);
		}
		if (MapData.m_instance.m_NpcIsFoughtList.Count > 0 && this.m_NpcIsFought != null)
		{
			bool flag = MapData.m_instance.CheckNpcIsFoughtName(this.m_NpcIsFought);
			if (flag)
			{
				if (this.m_NpcIsFought.iStay.Equals(0))
				{
					if (YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, this.m_NpcIsFought.ReSetRound))
					{
						this.bCheakPlayer = true;
						GameGlobal.m_bPlayerTalk = false;
						this.iSetMode = 0;
						base.gameObject.SetActive(true);
						base.StartCoroutine(this.ReSetBattleNpcLook());
					}
					else
					{
						base.gameObject.SetActive(false);
					}
				}
				else if (YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, this.m_NpcIsFought.ReSetRound))
				{
					this.bCheakPlayer = true;
					this.bBattle = true;
					base.gameObject.tag = "BattleNpc";
				}
				else
				{
					this.bBattle = false;
					this.bCheakPlayer = false;
					this.iSetMode = 0;
					base.gameObject.tag = "Npc";
				}
			}
		}
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x00009834 File Offset: 0x00007A34
	public void SetNpcData(ConductNode _ConductNode)
	{
		if (_ConductNode == null)
		{
			return;
		}
		this.m_ConductNode = _ConductNode;
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x00072110 File Offset: 0x00070310
	public void ClickTalk(NpcCollider.eTalkType _TalkType = NpcCollider.eTalkType.Talk)
	{
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindWithTag("Player");
		}
		if (this.goPlayer == null)
		{
			return;
		}
		this.StopNav();
		this.SetOnTerrain();
		UITalk uitalk = Game.UI.Get<UITalk>();
		string talkData = string.Empty;
		if (_TalkType == NpcCollider.eTalkType.Talk)
		{
			if (PlayerController.m_Instance.Target == null)
			{
				return;
			}
			if (PlayerController.m_Instance.Target.name.Equals(this.m_strNpcID))
			{
				if (this.m_MapNpcDataNode.m_iTalkSteer == 1)
				{
					base.transform.LookAt(new Vector3(this.goPlayer.transform.position.x, PlayerController.m_Instance.Target.transform.position.y, this.goPlayer.transform.position.z));
				}
				this.goPlayer.transform.LookAt(new Vector3(PlayerController.m_Instance.Target.transform.position.x, this.goPlayer.transform.position.y, PlayerController.m_Instance.Target.transform.position.z));
				if (this.m_bToPoint)
				{
					talkData = this.m_MapNpcDataNode.GetTalkID(this.m_Animation, base.transform.position, MapNpcDataNode.ToPointType.Stand);
				}
				else
				{
					talkData = this.m_MapNpcDataNode.GetTalkID(this.m_Animation, base.transform.position, MapNpcDataNode.ToPointType.Walk);
				}
				uitalk.SetTalkData(talkData);
			}
		}
		else if (_TalkType == NpcCollider.eTalkType.EnemySeek)
		{
			this.PlayAnimation("idle01", false);
			PlayerController.m_Instance.Target = base.gameObject;
			talkData = this.m_MapNpcDataNode.GetTalkID(this.m_Animation, base.transform.position, MapNpcDataNode.ToPointType.Battle);
			uitalk.SetTalkData(talkData);
			if (this.m_MapNpcDataNode.m_bIsBattle)
			{
				this.toBattle = true;
				Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AlertShowImage(base.gameObject, this.iSetMode);
				if (this.m_NpcIsFought == null)
				{
					NpcIsFought npcIsFought = new NpcIsFought();
					npcIsFought.m_MapName = Application.loadedLevelName;
					npcIsFought.m_Npc = base.gameObject.name;
					npcIsFought.iStay = this.m_MapNpcDataNode.m_iStayAffterBattle;
					npcIsFought.ReSetRound = YoungHeroTime.m_instance.AddCheckRound(this.AddRound);
					this.m_NpcIsFought = npcIsFought;
					MapData.m_instance.m_NpcIsFoughtList.Add(npcIsFought);
				}
				else
				{
					this.m_NpcIsFought = MapData.m_instance.SetNpcIsFoughtRound(Application.loadedLevelName, base.gameObject.name, this.AddRound);
				}
			}
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x00009844 File Offset: 0x00007A44
	private void Update()
	{
		if (this.m_MapNpcDataNode != null && this.m_MapNpcDataNode.m_iMoveType != 2)
		{
			this.SetOnTerrain();
		}
		this.ClickToMove();
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x000723EC File Offset: 0x000705EC
	private void UpdateNpc543()
	{
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindWithTag("Player");
		}
		if (this.goPlayer == null || this.m_MapNpcDataNode == null)
		{
			return;
		}
		if (Vector3.Distance(this.goPlayer.transform.position, base.transform.position) > NpcCollider.m_fNpc543Dist)
		{
			return;
		}
		this.fTimePos += Time.deltaTime;
		if (NpcCollider.m_fNpc543RepeatTime + this.m_fDestroyTime > this.fTimePos)
		{
			return;
		}
		this.fTimePos = 0f;
		PointNode pointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
		if (pointNode.m_strMoodID == "0")
		{
			return;
		}
		if (this._MoodTalkGroup != null && pointNode.m_strMoodID != this._MoodTalkGroup.m_strMoodID)
		{
			this._MoodTalkGroup = Game.MoodTalk.GetMoodTalkGroup(pointNode.m_strMoodID);
		}
		if (this._MoodTalkGroup == null)
		{
			Debug.Log(base.name + " MoodTalk表 " + pointNode.m_strMoodID + " Not Found");
			return;
		}
		if (this.m_iNpc543Index >= this._MoodTalkGroup.m_MoodTalkNodeList.Count)
		{
			this.m_iNpc543Index = 0;
		}
		this.m_fDestroyTime = this._MoodTalkGroup.m_MoodTalkNodeList[this.m_iNpc543Index].m_fDestroyTime;
		if (this._MoodTalkGroup.m_MoodTalkNodeList.Count > 1)
		{
			this.m_iNpc543Index = Random.Range(0, this._MoodTalkGroup.m_MoodTalkNodeList.Count);
		}
		string strString = this._MoodTalkGroup.m_MoodTalkNodeList[this.m_iNpc543Index].m_strString;
		this.fTimePos = Random.Range(NpcCollider.m_fNpc543RepeatTime * 0.5f, NpcCollider.m_fNpc543RepeatTime);
		Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AddOneLineString(base.gameObject, strString, this.m_fDestroyTime);
		this.m_iNpc543Index++;
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00072608 File Offset: 0x00070808
	public void ReSetNpcLook()
	{
		if (this.m_bToPoint)
		{
			PointNode pointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
			if (!this.toBattle)
			{
				this.m_vec3LookDir = new Vector3(base.transform.localEulerAngles.x, (float)pointNode.m_iDirAngle, base.transform.localEulerAngles.z);
				base.StartCoroutine(this.NpcLookAT(this.m_vec3LookDir));
			}
			if (!this.m_MapNpcDataNode.m_bIsBattle && this.m_MapNpcDataNode.m_iCheakPlayer == 1)
			{
				this.iSetMode = 0;
				this.iChangeMode = 0;
			}
		}
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x000726BC File Offset: 0x000708BC
	private AnimationState GetPlayingAnimationState()
	{
		if (this.m_Animation == null)
		{
			return null;
		}
		if (!this.m_Animation.isPlaying)
		{
			return null;
		}
		foreach (object obj in this.m_Animation)
		{
			AnimationState animationState = (AnimationState)obj;
			if (this.m_Animation.IsPlaying(animationState.name))
			{
				return animationState;
			}
		}
		return null;
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0000986E File Offset: 0x00007A6E
	private void StopNav()
	{
		if (this.m_Mob != null && this.m_Mob.enabled)
		{
			this.m_Mob.Stop(true);
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0000989D File Offset: 0x00007A9D
	private void ResumeNav()
	{
		if (this.m_Mob != null && this.m_Mob.enabled)
		{
			this.m_Mob.Resume();
		}
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00072760 File Offset: 0x00070960
	public void PlayAnimation(string AniName, bool bOnce = false)
	{
		if (this.m_Animation == null)
		{
			return;
		}
		if (this.m_Animation[AniName] == null)
		{
			Game.g_ModelBundle.LoadAnimation(this.m_Animation, AniName);
		}
		if (this.m_Animation[AniName] == null)
		{
			Debug.Log(base.name + " not have " + AniName + " animation clip plz check");
			return;
		}
		if (bOnce)
		{
			this.m_Animation[AniName].wrapMode = 1;
		}
		else
		{
			this.m_Animation[AniName].wrapMode = 2;
		}
		this.m_Animation.CrossFade(AniName);
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x00072818 File Offset: 0x00070A18
	private void PlayOldAnimation()
	{
		this.m_bChnageToStand01 = false;
		if (this.m_Animation == null)
		{
			return;
		}
		if (this.m_asSaveOldAnimationState == null)
		{
			return;
		}
		this.m_Animation.CrossFade(this.m_asSaveOldAnimationState.name, 0.3f, 4);
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x000098CB File Offset: 0x00007ACB
	private void onBusy()
	{
		if (this.m_Mob != null)
		{
			this.m_Mob.enabled = false;
		}
		if (this.m_Animation.IsPlaying("walk"))
		{
			this.PlayAnimation("stand01", false);
		}
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0007286C File Offset: 0x00070A6C
	private void ClickToMove()
	{
		if (this.m_MapNpcDataNode != null)
		{
			if (GameGlobal.m_bMovie || GameGlobal.m_bCFormOpen || GameGlobal.m_bPlayerTalk || GameGlobal.m_bPlayingSmallGame || GameGlobal.m_bDoTalkReward)
			{
				this.onBusy();
				return;
			}
			if (this.iSetMode != 2 || this.iChangeMode != 2)
			{
				if (this.goPlayer == null)
				{
					this.goPlayer = GameObject.FindWithTag("Player");
				}
				if (this.goPlayer == null)
				{
					return;
				}
				if (this.iChangeMode != this.iSetMode && !GameGlobal.m_bWaitToBattle)
				{
					this.iSetMode = this.iChangeMode;
					if (this.iSetMode == 0)
					{
						Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AlertShowImage(base.gameObject, this.iSetMode);
						if (this.bLookOK)
						{
							this.bLookOK = false;
						}
					}
					else if (this.iSetMode == 1)
					{
						this.falertTime = Time.time;
						Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AlertShowImage(base.gameObject, this.iSetMode);
					}
					else if (this.iSetMode == 2)
					{
						Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AlertShowImage(base.gameObject, this.iSetMode);
						PlayerController.m_Instance.LookNpc(base.gameObject);
						if (this.m_MapNpcDataNode.m_strCheakPlayerTalkID.Length > 1)
						{
							if (this.iChangeMode == 2 && !this.toBattle)
							{
								this.ClickTalk(NpcCollider.eTalkType.EnemySeek);
							}
						}
						else if (this.m_MapNpcDataNode.m_bIsBattle)
						{
							this.toBattle = true;
							if (this.m_NpcIsFought == null)
							{
								NpcIsFought npcIsFought = new NpcIsFought();
								npcIsFought.m_MapName = Application.loadedLevelName;
								npcIsFought.m_Npc = base.gameObject.name;
								npcIsFought.iStay = this.m_MapNpcDataNode.m_iStayAffterBattle;
								npcIsFought.ReSetRound = YoungHeroTime.m_instance.AddCheckRound(this.AddRound);
								this.m_NpcIsFought = npcIsFought;
								MapData.m_instance.m_NpcIsFoughtList.Add(npcIsFought);
							}
							else
							{
								this.m_NpcIsFought = MapData.m_instance.SetNpcIsFoughtRound(Application.loadedLevelName, base.gameObject.name, this.AddRound);
							}
						}
					}
				}
			}
			if (this.iSetMode == 0)
			{
				PointNode pointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
				if (this.m_MapNpcDataNode.m_iMoveType != 2)
				{
					if (this.m_Mob != null && !this.m_Mob.enabled)
					{
						this.m_Mob.enabled = true;
					}
					Vector3 vector;
					vector..ctor(pointNode.m_fX, pointNode.m_fY, pointNode.m_fZ);
					float num = Vector3.Distance(vector, this.m_Contrller.transform.position);
					if (num <= 0.2f)
					{
						this.PlayAnimation(pointNode.m_strPointAni, false);
						if (!this.m_bToPoint)
						{
							this.m_bToPoint = true;
							this.m_fPointTime = Time.time;
							this.m_vec3LookDir = new Vector3(base.transform.localEulerAngles.x, (float)pointNode.m_iDirAngle, base.transform.localEulerAngles.z);
							base.StartCoroutine(this.NpcLookAT(this.m_vec3LookDir));
							if (this.m_fDestroyTime != 0f)
							{
								this.m_fDestroyTime = 0f;
							}
							this.fTimePos = NpcCollider.m_fNpc543RepeatTime;
							this.UpdateNpc543();
						}
						if (Time.time - this.m_fPointTime >= pointNode.m_fPointTime)
						{
							this.m_vec3LookDir = Vector3.zero;
							if (this.m_MapNpcDataNode.m_iMoveType == 1)
							{
								if (this.m_MapNpcDataNode.m_PointNodeList.Count > 1)
								{
									if (!this.m_bMaxOrMin)
									{
										if (this.m_iStep + 1 > this.m_MapNpcDataNode.m_PointNodeList.Count - 1)
										{
											this.m_iStep--;
											this.m_bMaxOrMin = true;
										}
										else
										{
											this.m_iStep++;
										}
									}
									else if (this.m_iStep - 1 < 0)
									{
										this.m_iStep++;
										this.m_bMaxOrMin = false;
									}
									else
									{
										this.m_iStep--;
									}
								}
							}
							else if (this.m_MapNpcDataNode.m_iMoveType == 4)
							{
								this.m_iStep = ((this.m_iStep + 1 <= this.m_MapNpcDataNode.m_PointNodeList.Count - 1) ? (this.m_iStep + 1) : 0);
							}
							else if (this.m_MapNpcDataNode.m_iMoveType == 3)
							{
								if (this.m_iStep + 1 > this.m_MapNpcDataNode.m_PointNodeList.Count - 1)
								{
									base.gameObject.SetActive(false);
									MapData.m_instance.DeleteAlwaysNpc(int.Parse(this.m_strNpcID), 0);
								}
								else
								{
									this.m_iStep++;
								}
							}
							else if (this.m_MapNpcDataNode.m_iMoveType == 5)
							{
								if (this.m_iStep + 1 > this.m_MapNpcDataNode.m_PointNodeList.Count - 1)
								{
									this.m_iStep = this.m_MapNpcDataNode.m_PointNodeList.Count - 1;
									this.m_Mob.enabled = false;
									this.m_MapNpcDataNode.m_iMoveType = 2;
								}
								else
								{
									this.m_iStep++;
								}
							}
						}
					}
					else
					{
						this.m_bToPoint = false;
						this.m_Animation.wrapMode = 2;
						this.PlayAnimation("walk", false);
						if (this.m_Mob != null && this.m_Mob.enabled)
						{
							if (this.m_Mob.speed != 1f + base.animation["walk"].length)
							{
								this.m_Mob.speed = 1f + base.animation["walk"].length;
							}
							this.m_Mob.SetDestination(vector);
						}
						else
						{
							Debug.LogWarning(base.name);
						}
					}
				}
				else if (this.m_MapNpcDataNode.m_iMoveType == 2)
				{
					if (GameGlobal.m_bPlayerTalk)
					{
						return;
					}
					this.UpdateNpc543();
					if (!this.m_Animation.isPlaying)
					{
						this.m_iCount++;
						if ((float)this.m_iCount > pointNode.m_fPointTime)
						{
							this.m_iCount = 1;
							this.m_iStep = ((this.m_iStep + 1 <= this.m_MapNpcDataNode.m_PointNodeList.Count - 1) ? (this.m_iStep + 1) : 0);
							pointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
						}
						if (this.m_strModelName == "HZ_oldbitch")
						{
						}
						if (pointNode.m_strPointAni != "die01")
						{
							this.PlayAnimation(pointNode.m_strPointAni, true);
						}
					}
				}
			}
			else if (this.iSetMode == 1)
			{
				if (this.m_Mob != null && this.m_Mob.enabled)
				{
					this.m_Mob.Stop(true);
				}
				this.PlayAnimation("idle01", false);
				this.m_bToPoint = true;
			}
			else if (this.iSetMode == 2 && this.toBattle && !GameGlobal.m_bPlayerTalk)
			{
				this.toBattle = false;
				Game.UI.Root.GetComponentInChildren<UIStringOverlay>().AlertShowImage(base.gameObject, this.iSetMode);
				base.StartCoroutine(this.GoToBattle(this.m_MapNpcDataNode.m_iBattleArea, 0.05f));
			}
			if ((this.iSetMode == 1 || this.iSetMode == 2) && Time.time > this.falertTime + 2f)
			{
				this.LookAtObj(this.goPlayer);
			}
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00073098 File Offset: 0x00071298
	private IEnumerator ReSetBattleNpcLook()
	{
		float fdelayTime = 1.5f;
		yield return new WaitForSeconds(fdelayTime);
		this.iSetMode = 0;
		PointNode _PointNode = this.m_MapNpcDataNode.m_PointNodeList[this.m_iStep];
		this.m_vec3LookDir = new Vector3(base.transform.localEulerAngles.x, (float)_PointNode.m_iDirAngle, base.transform.localEulerAngles.z);
		base.StartCoroutine(this.NpcLookAT(this.m_vec3LookDir));
		yield break;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000730B4 File Offset: 0x000712B4
	private void LookAtObj(GameObject go)
	{
		Vector3 vector = go.transform.position - base.transform.position;
		vector.y = 0f;
		Quaternion quaternion = Quaternion.LookRotation(vector);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * 5f);
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00073118 File Offset: 0x00071318
	private IEnumerator GoToBattle(int BattleID, float fdelayTime)
	{
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindWithTag("Player");
		}
		GameGlobal.m_TransferPos = this.goPlayer.transform.position;
		GameGlobal.m_bWaitToBattle = true;
		ScreenOverlay SO = null;
		if (SO == null)
		{
			SO = Camera.main.GetComponent<ScreenOverlay>();
			if (SO == null)
			{
				SO = Camera.main.gameObject.AddComponent<ScreenOverlay>();
			}
		}
		float fPos = 0f;
		float fTime = Time.realtimeSinceStartup;
		if (SO != null)
		{
			while (fPos < 0.05f)
			{
				SO.intensity = fPos * 1.02f;
				SO.enabled = true;
				fPos += Time.realtimeSinceStartup - fTime;
				fTime = Time.realtimeSinceStartup;
				yield return null;
			}
			yield return new WaitForSeconds(fdelayTime);
			SO.enabled = false;
		}
		Game.g_BattleControl.StartBattle(BattleID);
		yield break;
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00073150 File Offset: 0x00071350
	private IEnumerator NpcLookAT(Vector3 targetPos)
	{
		float rotateSpeed = 200f;
		float fTimeD = 0f;
		Vector3 vPos = base.transform.localEulerAngles;
		Vector3 targetDir = targetPos - vPos;
		Vector3 forward = base.transform.forward;
		float fAngle = Vector3.Angle(targetDir, forward);
		float fTotalTime = fAngle / rotateSpeed;
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindWithTag("Player");
		}
		while (targetPos.y > 180f)
		{
			targetPos.y -= 360f;
		}
		while (targetPos.y < -180f)
		{
			targetPos.y += 360f;
		}
		if (Mathf.Abs(vPos.y - targetPos.y) > 180f)
		{
			if (vPos.y < 0f)
			{
				targetPos.y -= 360f;
			}
			else
			{
				targetPos.y += 360f;
			}
		}
		for (;;)
		{
			fTimeD += Time.deltaTime;
			if (fTimeD >= fTotalTime)
			{
				break;
			}
			float fPos = fTimeD / fTotalTime;
			base.transform.localEulerAngles = Vector3.Lerp(vPos, targetPos, fPos);
			yield return null;
		}
		base.transform.localEulerAngles = targetPos;
		yield break;
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0007317C File Offset: 0x0007137C
	public void SetOnTerrain()
	{
		Vector3 vector;
		vector..ctor(this.m_Contrller.transform.position.x, this.m_Contrller.transform.position.y + 1f, this.m_Contrller.transform.position.z);
		int num = 2048;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -Vector3.up, ref raycastHit, 2f, num))
		{
			this.m_Contrller.transform.position = raycastHit.point;
		}
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0000990B File Offset: 0x00007B0B
	public void setMode(int imode)
	{
		this.iChangeMode = imode;
	}

	// Token: 0x0400100F RID: 4111
	private static float m_fNpc543Dist = 15f;

	// Token: 0x04001010 RID: 4112
	private static float m_fNpc543RepeatTime = 10f;

	// Token: 0x04001011 RID: 4113
	private float m_fDestroyTime;

	// Token: 0x04001012 RID: 4114
	private ConductNode m_ConductNode;

	// Token: 0x04001013 RID: 4115
	private MapNpcDataNode m_MapNpcDataNode;

	// Token: 0x04001014 RID: 4116
	private NavMeshAgent m_Mob;

	// Token: 0x04001015 RID: 4117
	private BoxCollider m_Contrller;

	// Token: 0x04001016 RID: 4118
	public Animation m_Animation;

	// Token: 0x04001017 RID: 4119
	public string m_strModelName;

	// Token: 0x04001018 RID: 4120
	public int m_StoreID;

	// Token: 0x04001019 RID: 4121
	private string m_strNpcID;

	// Token: 0x0400101A RID: 4122
	private int m_iStep;

	// Token: 0x0400101B RID: 4123
	private int m_iCount;

	// Token: 0x0400101C RID: 4124
	private float m_fPointTime;

	// Token: 0x0400101D RID: 4125
	private bool m_bToPoint;

	// Token: 0x0400101E RID: 4126
	private bool m_bMaxOrMin;

	// Token: 0x0400101F RID: 4127
	private Vector3 m_vec3LookDir;

	// Token: 0x04001020 RID: 4128
	private GameObject goPlayer;

	// Token: 0x04001021 RID: 4129
	public bool AllConditionOver;

	// Token: 0x04001022 RID: 4130
	public bool m_bSpecialOpen;

	// Token: 0x04001023 RID: 4131
	public bool bBattle;

	// Token: 0x04001024 RID: 4132
	private float fTimePos;

	// Token: 0x04001025 RID: 4133
	private int m_iNpc543Index;

	// Token: 0x04001026 RID: 4134
	private int AddRound = 30;

	// Token: 0x04001027 RID: 4135
	private NpcIsFought m_NpcIsFought;

	// Token: 0x04001028 RID: 4136
	private bool m_bChnageToStand01;

	// Token: 0x04001029 RID: 4137
	private string m_strSaveAnimationName = string.Empty;

	// Token: 0x0400102A RID: 4138
	private AnimationState m_asSaveOldAnimationState;

	// Token: 0x0400102B RID: 4139
	public float m_fMoveSpeed;

	// Token: 0x0400102C RID: 4140
	public bool bCheakPlayer;

	// Token: 0x0400102D RID: 4141
	private float falertTime;

	// Token: 0x0400102E RID: 4142
	public int iChangeMode;

	// Token: 0x0400102F RID: 4143
	public int iSetMode;

	// Token: 0x04001030 RID: 4144
	private PointNode _PointNode;

	// Token: 0x04001031 RID: 4145
	private MoodTalkGroup _MoodTalkGroup;

	// Token: 0x04001032 RID: 4146
	private bool SpecialNpc;

	// Token: 0x04001033 RID: 4147
	private bool toBattle;

	// Token: 0x04001034 RID: 4148
	private bool bLookOK;

	// Token: 0x04001035 RID: 4149
	[HideInInspector]
	public float fCheckDelayTime;

	// Token: 0x04001036 RID: 4150
	[HideInInspector]
	public float fCheckTime;

	// Token: 0x020002BF RID: 703
	public enum eTalkType
	{
		// Token: 0x04001038 RID: 4152
		Talk,
		// Token: 0x04001039 RID: 4153
		EnemySeek
	}
}
