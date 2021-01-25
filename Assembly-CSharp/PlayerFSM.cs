using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class PlayerFSM : PlayerFSMBase
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x00005BA9 File Offset: 0x00003DA9
	// (set) Token: 0x06000650 RID: 1616 RVA: 0x00005BA0 File Offset: 0x00003DA0
	public PlayerFSM.ePlayerState nowState { get; private set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000653 RID: 1619 RVA: 0x00005BBA File Offset: 0x00003DBA
	// (set) Token: 0x06000652 RID: 1618 RVA: 0x00005BB1 File Offset: 0x00003DB1
	public PlayerFSM.ePlayerState prvState { get; private set; }

	// Token: 0x06000654 RID: 1620 RVA: 0x00045644 File Offset: 0x00043844
	public void init(PlayerController ctrl, NavMeshAgent nma, Animation anim, CharacterController cctr, Transform tf)
	{
		this.m_PlayerController = ctrl;
		this.m_NavMeshAgent = nma;
		this.m_CharacterController = cctr;
		this.m_Transform = tf;
		this.m_fAddSpeed = 0.5f;
		this.m_fBaseSpeed = 5f;
		this.m_fNowSpeed = this.m_fBaseSpeed;
		this.m_fAddSpeedTime = 0.5f;
		this.m_fDecSpeedTime = 0.5f;
		this.m_NavMeshAgent.speed = this.m_fBaseSpeed;
		this.setIdleState();
		this.setMoveState();
		this.setBusyState();
		this.setOpUIState();
		this.setTalkState();
		this.setAnimation();
		this.setTakeState();
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		base.Add(new IState[]
		{
			this.m_PlayerIdleState,
			this.m_PlayerMoveState,
			this.m_PlayerBusyState,
			this.m_PlayerOpUIState,
			this.m_PlayerTalkState,
			this.m_PlayerTakeState
		});
		this.Start(this.m_PlayerIdleState);
		Game.g_InputManager.Push(this);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00005BC2 File Offset: 0x00003DC2
	private void setAnimation()
	{
		this.m_PlayerAnimation = new PlayerAnimation(this.m_Transform.gameObject);
		this.m_PlayerAnimation.SetAnimation();
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00045748 File Offset: 0x00043948
	private void setIdleState()
	{
		this.m_IdleToMove.EvaluateFunction = (() => this.nowState == PlayerFSM.ePlayerState.ClickMove || this.nowState == PlayerFSM.ePlayerState.DirMove);
		this.m_IdleToBusy.EvaluateFunction = (() => GameGlobal.m_bMovie);
		this.m_IdleToOpUI.EvaluateFunction = (() => GameGlobal.m_bCFormOpen);
		this.m_IdleToTalk.EvaluateFunction = (() => GameGlobal.m_bPlayerTalk || GameGlobal.m_bDoTalkReward);
		this.m_IdleToTake.EvaluateFunction = (() => this.nowState == PlayerFSM.ePlayerState.TakeItem);
		this.m_IdleToMove.NextState = this.m_PlayerMoveState;
		this.m_IdleToBusy.NextState = this.m_PlayerBusyState;
		this.m_IdleToOpUI.NextState = this.m_PlayerOpUIState;
		this.m_IdleToTalk.NextState = this.m_PlayerTalkState;
		this.m_IdleToTake.NextState = this.m_PlayerTakeState;
		this.m_PlayerIdleState.Transitions.Add(this.m_IdleToMove);
		this.m_PlayerIdleState.Transitions.Add(this.m_IdleToBusy);
		this.m_PlayerIdleState.Transitions.Add(this.m_IdleToOpUI);
		this.m_PlayerIdleState.Transitions.Add(this.m_IdleToTalk);
		this.m_PlayerIdleState.Transitions.Add(this.m_IdleToTake);
		this.m_PlayerIdleState.StateUpdate += new Action(this.CheckMouse);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x000458D8 File Offset: 0x00043AD8
	private void setMoveState()
	{
		this.m_MoveToIdle.EvaluateFunction = (() => this.nowState == PlayerFSM.ePlayerState.Idle);
		this.m_MoveToBusy.EvaluateFunction = (() => GameGlobal.m_bMovie);
		this.m_MoveToOpUI.EvaluateFunction = (() => GameGlobal.m_bCFormOpen);
		this.m_MoveToTalk.EvaluateFunction = (() => GameGlobal.m_bPlayerTalk || GameGlobal.m_bDoTalkReward);
		this.m_MoveToTake.EvaluateFunction = (() => this.nowState == PlayerFSM.ePlayerState.TakeItem);
		this.m_MoveToIdle.NextState = this.m_PlayerIdleState;
		this.m_MoveToBusy.NextState = this.m_PlayerBusyState;
		this.m_MoveToOpUI.NextState = this.m_PlayerOpUIState;
		this.m_MoveToTalk.NextState = this.m_PlayerTalkState;
		this.m_MoveToTake.NextState = this.m_PlayerTakeState;
		this.m_PlayerMoveState.Transitions.Add(this.m_MoveToIdle);
		this.m_PlayerMoveState.Transitions.Add(this.m_MoveToBusy);
		this.m_PlayerMoveState.Transitions.Add(this.m_MoveToOpUI);
		this.m_PlayerMoveState.Transitions.Add(this.m_MoveToTalk);
		this.m_PlayerMoveState.Transitions.Add(this.m_MoveToTake);
		this.m_PlayerMoveState.StateUpdate += new Action(this.CheckMouse);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00045A68 File Offset: 0x00043C68
	private void setBusyState()
	{
		this.m_BusyToIdle.EvaluateFunction = (() => !GameGlobal.m_bMovie);
		this.m_BusyToIdle.NextState = this.m_PlayerIdleState;
		this.m_PlayerBusyState.Transitions.Add(this.m_BusyToIdle);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00045AC4 File Offset: 0x00043CC4
	private void setOpUIState()
	{
		this.m_OpUIToIdle.EvaluateFunction = (() => !GameGlobal.m_bCFormOpen);
		this.m_OpUIToIdle.NextState = this.m_PlayerIdleState;
		this.m_PlayerOpUIState.Transitions.Add(this.m_OpUIToIdle);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00045B20 File Offset: 0x00043D20
	private void setTalkState()
	{
		this.m_TalkToIdle.EvaluateFunction = (() => !GameGlobal.m_bPlayerTalk && !GameGlobal.m_bDoTalkReward);
		this.m_TalkToIdle.NextState = this.m_PlayerIdleState;
		this.m_PlayerTalkState.Transitions.Add(this.m_TalkToIdle);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00005BE5 File Offset: 0x00003DE5
	private void setTakeState()
	{
		this.m_TakeToIdle.EvaluateFunction = (() => this.nowState == PlayerFSM.ePlayerState.Idle);
		this.m_TakeToIdle.NextState = this.m_PlayerIdleState;
		this.m_PlayerTakeState.Transitions.Add(this.m_TakeToIdle);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00045B7C File Offset: 0x00043D7C
	private void targetLock()
	{
		if (GameCursor.Instance.CheckCursorOnUI())
		{
			GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
			return;
		}
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		if (Camera.main)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int num = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Npc")) + (1 << LayerMask.NameToLayer("Item"));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit, this.m_RayCastDis, num) && raycastHit.collider != null)
			{
				if (raycastHit.collider.tag == "Enemy")
				{
					GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Attack);
				}
				else if (raycastHit.collider.tag == "Ground")
				{
					GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
				}
				else if (raycastHit.collider.tag == "Npc" || raycastHit.collider.tag == "SceneObject")
				{
					GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Npc);
				}
				else if (raycastHit.collider.tag == "Chest")
				{
					GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Pick);
				}
				else if (raycastHit.collider.tag == "TreasureBox")
				{
					TreasureBox component = raycastHit.transform.GetComponent<TreasureBox>();
					if (component != null && !component.m_bOpen)
					{
						GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Pick);
					}
				}
			}
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00045D38 File Offset: 0x00043F38
	private void setPlayerOnTerrain()
	{
		float num = 1f;
		if (GameGlobal.m_bBigMapMode)
		{
			num = 50f;
		}
		Vector3 vector;
		vector..ctor(this.m_CharacterController.transform.position.x, this.m_CharacterController.transform.position.y + num, this.m_CharacterController.transform.position.z);
		int num2 = 2048;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, Vector3.down, ref raycastHit, num + 1f, num2))
		{
			this.m_CharacterController.transform.position = raycastHit.point;
		}
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00045DE8 File Offset: 0x00043FE8
	public float GetTargetHeight()
	{
		if (this.m_Target != null)
		{
			return this.m_Target.transform.position.y;
		}
		return 0f;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00045E24 File Offset: 0x00044024
	public float GetPlayerHeight()
	{
		return this.m_CharacterController.transform.position.y;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00045E4C File Offset: 0x0004404C
	private void checkTarget(RaycastHit hit)
	{
		if (hit.collider.tag != "Player")
		{
			Vector3 clickMove = hit.point;
			this.m_fCheckRange = GameGlobal.m_fWalkRange;
			if (hit.collider.tag == "Ground")
			{
				clickMove = hit.point;
				this.m_fCheckRange = GameGlobal.m_fWalkRange;
				this.m_Target = null;
				GameCursor.Instance.SetCursorEffect(GameCursor.eCursorEffect.Normal, new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z), hit.collider.transform.rotation, null);
			}
			else if (hit.collider.tag == "Enemy")
			{
				this.m_fCheckRange = GameGlobal.m_fTalkRange;
				this.m_Target = hit.collider.gameObject;
				GameObject gameObject = GameCursor.Instance.SetCursorEffect(GameCursor.eCursorEffect.Attack, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 0.02f, hit.collider.transform.position.z), Quaternion.identity, null);
				gameObject.transform.parent = this.m_Target.transform;
				clickMove = this.m_Target.transform.position;
				this.showTargetEffect = true;
			}
			else if (!(hit.collider.tag == "BattleNpc"))
			{
				if (hit.collider.tag == "Npc")
				{
					this.m_fCheckRange = GameGlobal.m_fTalkRange;
					this.m_Target = hit.collider.gameObject;
					clickMove = this.m_Target.transform.position;
					this.showTargetEffect = true;
				}
				else if (hit.collider.tag == "Chest" || hit.collider.tag == "TreasureBox" || hit.collider.tag == "SceneObject")
				{
					this.m_fCheckRange = GameGlobal.m_fPickRange;
					this.m_Target = hit.collider.gameObject;
					clickMove = this.m_Target.transform.position;
					this.showTargetEffect = false;
				}
			}
			this.m_fDestDistance = Vector2.Distance(new Vector2(clickMove.x, clickMove.z), new Vector2(this.m_Transform.position.x, this.m_Transform.position.z));
			if (this.m_fDestDistance < this.m_fCheckRange)
			{
				this.DoTargetEvent();
			}
			else
			{
				this.SetClickMove(clickMove);
			}
		}
		else if (this.Hitdist.collider.tag == "Player")
		{
			this.m_NavMeshAgent.Stop(true);
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00046188 File Offset: 0x00044388
	public void SearchTarget()
	{
		if (MapData.m_instance == null)
		{
			return;
		}
		float num = float.MaxValue;
		PlayerTarget playerTarget = null;
		for (int i = 0; i < PlayerTarget.PlayerTargetList.Count; i++)
		{
			PlayerTarget playerTarget2 = PlayerTarget.PlayerTargetList[i];
			if (playerTarget2.gameObject.activeSelf)
			{
				if (!(playerTarget2.collider != null) || playerTarget2.collider.enabled)
				{
					bool flag = SearchRange.IsGameObjectPointInRange(this.m_Transform.gameObject, this.fSquaredR, this.fCosTheta, playerTarget2.gameObject);
					if (flag)
					{
						playerTarget = playerTarget2;
						break;
					}
					Vector2 vector;
					vector..ctor(playerTarget2.transform.position.x, playerTarget2.transform.position.z);
					Vector2 vector2;
					vector2..ctor(this.m_Transform.position.x, this.m_Transform.position.z);
					float num2 = Vector2.Distance(vector2, vector);
					if (num2 < GameGlobal.m_fTalkRange && num2 <= num)
					{
						num = num2;
						playerTarget = playerTarget2;
					}
				}
			}
		}
		if (playerTarget == null)
		{
			this.m_Target = null;
			return;
		}
		if (this.m_Target == playerTarget.gameObject)
		{
			return;
		}
		if (playerTarget.m_TargetType == PlayerTarget.eTargetType.MouseEventCube || playerTarget.m_TargetType == PlayerTarget.eTargetType.SmallGame || playerTarget.m_TargetType == PlayerTarget.eTargetType.TreasureBox)
		{
			this.showTargetEffect = false;
		}
		else
		{
			this.showTargetEffect = true;
		}
		this.m_Target = playerTarget.gameObject;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00046334 File Offset: 0x00044534
	private void setTargetEffect()
	{
		if (this.m_Target != null && this.showTargetEffect)
		{
			GameCursor.Instance.SetCursorEffect(GameCursor.eCursorEffect.Arrow, this.m_Target.transform.position, Quaternion.identity, this.m_Target.transform);
		}
		else
		{
			GameCursor.Instance.SetCursorEffect(GameCursor.eCursorEffect.Arrow, new Vector3(0f, -5000f, 0f), Quaternion.identity, null);
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00005C25 File Offset: 0x00003E25
	public override void Update()
	{
		this.setPlayerOnTerrain();
		base.Update();
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00005C33 File Offset: 0x00003E33
	public void SetState(PlayerFSM.ePlayerState state)
	{
		if (state == this.nowState)
		{
			return;
		}
		this.prvState = this.nowState;
		this.nowState = state;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00005C55 File Offset: 0x00003E55
	public bool IsPlayingAnimation(string name)
	{
		return this.m_PlayerAnimation.IsPlaying(name);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00005C63 File Offset: 0x00003E63
	public void PlayAnimation(string name, float speed = 1f)
	{
		this.m_PlayerAnimation.Play(name, 0.2f, speed);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00005C77 File Offset: 0x00003E77
	public int GetAnimationLayer(string name)
	{
		return this.m_PlayerAnimation.GetAnimationLayer(name);
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00005C85 File Offset: 0x00003E85
	public void ResetAnimationLayer()
	{
		this.m_PlayerAnimation.ResetAnimationLayer();
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00005C92 File Offset: 0x00003E92
	public void StopKeyMove()
	{
		this.m_DestPosition = this.m_Transform.position;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000463B4 File Offset: 0x000445B4
	public void SetDirMove(Vector3 dir, bool searchTarget = true)
	{
		if (this.m_IsOnNaveMesh)
		{
			float num = 1f;
			if (GameGlobal.m_bBigMapMode)
			{
				num = 5f;
			}
			NavMeshHit navMeshHit;
			Vector3 vector;
			if (NavMesh.SamplePosition(this.m_Transform.position + dir * num, ref navMeshHit, 50f, -1))
			{
				vector = navMeshHit.position;
			}
			else
			{
				vector = this.m_Transform.position + dir * num;
			}
			this.m_NavMeshAgent.enabled = true;
			this.m_Transform.forward = dir;
			this.m_DestPosition = vector;
			this.m_NavMeshAgent.SetDestination(vector);
			if (searchTarget)
			{
				this.SearchTarget();
			}
		}
		else
		{
			this.m_NavMeshAgent.enabled = false;
			this.m_Transform.forward = dir;
			this.m_CharacterController.Move(dir * Time.deltaTime * this.m_fNowSpeed);
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x000464A8 File Offset: 0x000446A8
	public void SetClickMove(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(pos, ref navMeshHit, 50f, -1))
		{
			this.m_DestPosition = navMeshHit.position;
		}
		else
		{
			this.m_DestPosition = pos;
		}
		this.m_fDestDistance = Vector2.Distance(new Vector2(this.m_DestPosition.x, this.m_DestPosition.z), new Vector2(this.m_Transform.position.x, this.m_Transform.position.z));
		if (this.m_IsOnNaveMesh)
		{
			this.m_NavMeshAgent.enabled = true;
			this.m_NavMeshAgent.SetDestination(this.m_DestPosition);
			this.SetState(PlayerFSM.ePlayerState.ClickMove);
		}
		else
		{
			this.m_NavMeshAgent.enabled = false;
			this.m_CharacterController.SimpleMove(this.m_DestPosition.normalized * Time.deltaTime * this.m_fNowSpeed);
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x000465A0 File Offset: 0x000447A0
	public void CheckMouse()
	{
		this.targetLock();
		if (Input.GetMouseButtonDown(0))
		{
			if (GameCursor.Instance.CheckCursorOnUI())
			{
				return;
			}
			this.AddSpeed();
			if (Camera.main == null)
			{
				return;
			}
			this.m_fOldDestDistance = 0f;
			this.m_AutoStopCheckTime = 0f;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			this.m_DestPosition = this.m_Transform.position;
			int num = 11264;
			RaycastHit hit;
			if (Physics.Raycast(ray, ref hit, this.m_RayCastDis, num) && hit.collider.tag != "Player")
			{
				this.checkTarget(hit);
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (GameCursor.Instance.CheckCursorOnUI())
			{
				return;
			}
			this.m_BlockMouseTime += Time.deltaTime;
			if (this.m_BlockMouseTime < 0.25f)
			{
				return;
			}
			if (Camera.main == null)
			{
				return;
			}
			this.m_Target = null;
			this.AddSpeed();
			this.m_bMouseDown = true;
			this.m_fCheckRange = GameGlobal.m_fWalkRange;
			Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
			int num2 = 11264;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray2, ref raycastHit, this.m_RayCastDis, num2) && raycastHit.collider.tag != "Player")
			{
				this.SetState(PlayerFSM.ePlayerState.ClickMove);
				Vector3 vector = raycastHit.point - this.m_Transform.position;
				vector.y = 0f;
				if (GameGlobal.m_bBigMapMode)
				{
					this.SetClickMove(raycastHit.point);
				}
				else
				{
					this.SetDirMove(vector.normalized, false);
				}
			}
		}
		else
		{
			this.m_bMouseDown = false;
			this.m_BlockMouseTime = 0f;
			if (this.nowState == PlayerFSM.ePlayerState.DirMove)
			{
				return;
			}
			this.ResetAddSpeed();
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00005CA5 File Offset: 0x00003EA5
	public bool UpdateJump()
	{
		if (!this.m_bJump)
		{
			return false;
		}
		this.m_JumpCount += Time.deltaTime;
		if (this.m_JumpCount >= this.m_JumpTime)
		{
			this.m_bJump = false;
			return true;
		}
		return false;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00005CE1 File Offset: 0x00003EE1
	public void SetJump()
	{
		this.m_bJump = true;
		this.m_JumpCount = 0f;
		this.m_JumpTime = this.m_PlayerAnimation.Play("jump", 0f, 1f, 0, 1);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00005D17 File Offset: 0x00003F17
	public void ResetAddSpeed()
	{
		if (this.m_bAddSpeed)
		{
			this.m_bAddSpeed = false;
			this.m_fMouseDownTime = Time.time;
			this.m_fDecSpeed = this.m_fNowSpeed - this.m_fBaseSpeed;
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00005D49 File Offset: 0x00003F49
	public void AddSpeed()
	{
		if (!this.m_bAddSpeed)
		{
			this.m_bAddSpeed = true;
			this.m_fMouseDownTime = Time.time;
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00046794 File Offset: 0x00044994
	public void UpdateMouseMove()
	{
		this.m_fDestDistance = Vector2.Distance(new Vector2(this.m_DestPosition.x, this.m_DestPosition.z), new Vector2(this.m_Transform.position.x, this.m_Transform.position.z));
		if (this.m_fDestDistance < this.m_fCheckRange)
		{
			this.SetState(PlayerFSM.ePlayerState.Idle);
			this.m_DestPosition = this.m_Transform.position;
			this.m_NavMeshAgent.Stop(true);
			this.DoTargetEvent();
		}
		else if (this.m_fDestDistance > this.m_fCheckRange)
		{
			if (this.m_Target != null)
			{
				return;
			}
			this.m_AutoStopCheckTime += Time.deltaTime;
			if (this.m_AutoStopCheckTime < 0.2f)
			{
				return;
			}
			if (this.m_fOldDestDistance != this.m_fDestDistance)
			{
				float num = this.m_fOldDestDistance - this.m_fDestDistance;
				num = ((num <= 0f) ? (-num) : num);
				if (num <= 5E-05f)
				{
					this.m_DestPosition = this.m_Transform.position;
					this.m_fOldDestDistance = 0f;
					this.m_NavMeshAgent.Stop(true);
				}
				else
				{
					this.m_fOldDestDistance = this.m_fDestDistance;
					this.SetState(PlayerFSM.ePlayerState.ClickMove);
				}
			}
			else
			{
				this.SetState(PlayerFSM.ePlayerState.Idle);
			}
		}
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00046904 File Offset: 0x00044B04
	public void UpdateMoveSpeed()
	{
		if (this.m_fAddSpeedTime <= 0f)
		{
			return;
		}
		bool flag = false;
		if (this.m_bAddSpeed)
		{
			float num = Time.time;
			if (num - this.m_fMouseDownTime < this.m_fAddSpeedTime)
			{
				num -= this.m_fMouseDownTime;
				num /= this.m_fAddSpeedTime;
				num *= this.m_fAddSpeed;
				this.m_fNowSpeed = this.m_fBaseSpeed * (1f + num);
				flag = true;
			}
			else
			{
				num = this.m_fBaseSpeed * (1f + this.m_fAddSpeed);
				if (num > this.m_fNowSpeed)
				{
					this.m_fNowSpeed = num;
					flag = true;
				}
			}
		}
		else
		{
			float num2 = Time.time;
			if (this.m_fDecSpeed > 0f)
			{
				if (this.m_fDecSpeed < 0.05f)
				{
					this.m_fDecSpeed = 0f;
					this.m_fNowSpeed = this.m_fBaseSpeed;
					flag = true;
				}
				else if (num2 - this.m_fMouseDownTime < this.m_fDecSpeedTime)
				{
					num2 -= this.m_fMouseDownTime;
					num2 /= this.m_fDecSpeedTime;
					num2 = 1f - num2;
					num2 *= this.m_fDecSpeed;
					this.m_fNowSpeed = this.m_fBaseSpeed + num2;
					flag = true;
				}
				else if (this.m_fNowSpeed > this.m_fBaseSpeed)
				{
					this.m_fNowSpeed = this.m_fBaseSpeed;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.m_NavMeshAgent.speed = this.m_fNowSpeed;
			this.m_PlayerAnimation.SetAnimationSpeed("run", this.m_fNowSpeed / this.m_fBaseSpeed);
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00005D68 File Offset: 0x00003F68
	public void UpdateTargetEffect()
	{
		this.setTargetEffect();
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00046A90 File Offset: 0x00044C90
	public void DoMouseEvent()
	{
		if (this.m_Target == null)
		{
			return;
		}
		MouseEventCube component = this.m_Target.GetComponent<MouseEventCube>();
		if (component != null)
		{
			GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
			component.SetEvent();
			this.m_Target = null;
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00046AE0 File Offset: 0x00044CE0
	public void DoTargetEvent()
	{
		if (this.m_Target == null)
		{
			return;
		}
		if (this.m_Target.tag == "Npc")
		{
			this.m_Target.GetComponent<NpcCollider>().ClickTalk(NpcCollider.eTalkType.Talk);
		}
		else if (this.m_Target.tag == "Chest")
		{
			this.LookNpc(this.m_Target);
			this.SetState(PlayerFSM.ePlayerState.TakeItem);
		}
		else if (this.m_Target.tag == "TreasureBox")
		{
			TreasureBox component = this.m_Target.GetComponent<TreasureBox>();
			if (!component.m_bOpen)
			{
				GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
				component.OpenBox();
				this.m_Target = null;
			}
		}
		else if (this.m_Target.tag == "SceneObject")
		{
			this.m_Target.GetComponent<SceneObejctEvent>().DoEnent();
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00005D70 File Offset: 0x00003F70
	public void StopMove()
	{
		Debug.Log("離開移動狀態，就停止");
		if (this.m_NavMeshAgent != null)
		{
			this.m_DestPosition = this.m_Transform.position;
			this.m_NavMeshAgent.Stop(true);
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00046BDC File Offset: 0x00044DDC
	public void SetTarget(GameObject val)
	{
		this.m_Target = val;
		if (this.m_Target.tag == "Chest" || this.m_Target.tag == "TreasureBox")
		{
			this.showTargetEffect = false;
		}
		else
		{
			this.showTargetEffect = true;
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00005DAA File Offset: 0x00003FAA
	public GameObject GetTarget()
	{
		return this.m_Target;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x00005DB2 File Offset: 0x00003FB2
	public void SetOnNavMesh(bool bval)
	{
		this.m_IsOnNaveMesh = bval;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00005DBB File Offset: 0x00003FBB
	public void SetSpeed(float fbase, float fadd)
	{
		this.m_fBaseSpeed = fbase;
		this.m_fNowSpeed = this.m_fBaseSpeed;
		this.m_fAddSpeed = fadd;
		if (this.m_NavMeshAgent != null)
		{
			this.m_NavMeshAgent.speed = this.m_fNowSpeed;
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00005DF9 File Offset: 0x00003FF9
	public void SetTalkAni(string strAniID, WrapMode mod)
	{
		this.m_PlayerAnimation.SetTalkAni(strAniID, 0.2f, 1f, 0, mod);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00046C38 File Offset: 0x00044E38
	public void TalkStop()
	{
		this.m_DestPosition = this.m_Transform.position;
		this.SetState(PlayerFSM.ePlayerState.Idle);
		if (this.m_CharacterController != null)
		{
			this.m_CharacterController.transform.position = this.m_Transform.position;
		}
		NavMeshHit navMeshHit;
		if (this.m_NavMeshAgent != null && NavMesh.SamplePosition(this.m_Transform.position, ref navMeshHit, 50f, -1))
		{
			this.m_NavMeshAgent.Warp(navMeshHit.position);
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00046CCC File Offset: 0x00044ECC
	public void LookNpc(GameObject GO)
	{
		if (this.m_Transform != null)
		{
			this.m_Transform.transform.LookAt(new Vector3(GO.transform.position.x, this.m_Transform.position.y, GO.transform.position.z));
		}
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00005E13 File Offset: 0x00004013
	public void ReSetAnimation()
	{
		this.setAnimation();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00005E1B File Offset: 0x0000401B
	public void ResetNav()
	{
		this.m_NavMeshAgent = this.m_Transform.GetComponent<NavMeshAgent>();
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00046D38 File Offset: 0x00044F38
	public void ReSetMoveDate()
	{
		this.m_Target = null;
		this.m_DestPosition = this.m_Transform.position;
		this.m_fDestDistance = 0f;
		this.m_fOldDestDistance = 0f;
		this.SetState(PlayerFSM.ePlayerState.Idle);
		if (this.m_CharacterController != null)
		{
			this.m_CharacterController.transform.position = this.m_Transform.position;
		}
		NavMeshHit navMeshHit;
		if (this.m_NavMeshAgent != null && NavMesh.SamplePosition(this.m_Transform.position, ref navMeshHit, 50f, -1))
		{
			this.m_NavMeshAgent.Warp(navMeshHit.position);
			this.setPlayerOnTerrain();
		}
	}

	// Token: 0x040006F8 RID: 1784
	private PlayerController m_PlayerController;

	// Token: 0x040006F9 RID: 1785
	private NavMeshAgent m_NavMeshAgent;

	// Token: 0x040006FA RID: 1786
	private PlayerAnimation m_PlayerAnimation;

	// Token: 0x040006FB RID: 1787
	private CharacterController m_CharacterController;

	// Token: 0x040006FC RID: 1788
	private Transform m_Transform;

	// Token: 0x040006FD RID: 1789
	private GameObject m_Target;

	// Token: 0x040006FE RID: 1790
	private RaycastHit Hitdist;

	// Token: 0x040006FF RID: 1791
	public bool m_IsOnNaveMesh = true;

	// Token: 0x04000700 RID: 1792
	private float m_RayCastDis = 1000f;

	// Token: 0x04000701 RID: 1793
	private bool m_bAddSpeed;

	// Token: 0x04000702 RID: 1794
	private bool m_bMouseDown;

	// Token: 0x04000703 RID: 1795
	private float m_BlockMouseTime;

	// Token: 0x04000704 RID: 1796
	private float m_fMouseDownTime;

	// Token: 0x04000705 RID: 1797
	private float m_fBaseSpeed;

	// Token: 0x04000706 RID: 1798
	private float m_fNowSpeed;

	// Token: 0x04000707 RID: 1799
	private float m_fAddSpeed;

	// Token: 0x04000708 RID: 1800
	private float m_fDecSpeed;

	// Token: 0x04000709 RID: 1801
	private float m_fAddSpeedTime;

	// Token: 0x0400070A RID: 1802
	private float m_fDecSpeedTime;

	// Token: 0x0400070B RID: 1803
	public bool m_bJump;

	// Token: 0x0400070C RID: 1804
	private float m_JumpTime;

	// Token: 0x0400070D RID: 1805
	private float m_JumpCount;

	// Token: 0x0400070E RID: 1806
	private PlayerIdleState m_PlayerIdleState = new PlayerIdleState
	{
		Name = "Idle"
	};

	// Token: 0x0400070F RID: 1807
	private PlayerMoveState m_PlayerMoveState = new PlayerMoveState
	{
		Name = "Move"
	};

	// Token: 0x04000710 RID: 1808
	private PlayerBusyState m_PlayerBusyState = new PlayerBusyState
	{
		Name = "Busy"
	};

	// Token: 0x04000711 RID: 1809
	private PlayerOpUIState m_PlayerOpUIState = new PlayerOpUIState
	{
		Name = "OpUI"
	};

	// Token: 0x04000712 RID: 1810
	private PlayerTalkState m_PlayerTalkState = new PlayerTalkState
	{
		Name = "Talk"
	};

	// Token: 0x04000713 RID: 1811
	private PlayerPickState m_PlayerTakeState = new PlayerPickState
	{
		Name = "Take"
	};

	// Token: 0x04000714 RID: 1812
	private EvaluateTriggerTransition m_IdleToMove = new EvaluateTriggerTransition
	{
		Name = "Idle->Move"
	};

	// Token: 0x04000715 RID: 1813
	private EvaluateTriggerTransition m_IdleToBusy = new EvaluateTriggerTransition
	{
		Name = "Idle->Busy"
	};

	// Token: 0x04000716 RID: 1814
	private EvaluateTriggerTransition m_IdleToOpUI = new EvaluateTriggerTransition
	{
		Name = "Idle->OpUI"
	};

	// Token: 0x04000717 RID: 1815
	private EvaluateTriggerTransition m_IdleToTalk = new EvaluateTriggerTransition
	{
		Name = "Idle->Talk"
	};

	// Token: 0x04000718 RID: 1816
	private EvaluateTriggerTransition m_IdleToTake = new EvaluateTriggerTransition
	{
		Name = "Idle->Take"
	};

	// Token: 0x04000719 RID: 1817
	private EvaluateTriggerTransition m_MoveToIdle = new EvaluateTriggerTransition
	{
		Name = "Move->Idle"
	};

	// Token: 0x0400071A RID: 1818
	private EvaluateTriggerTransition m_MoveToBusy = new EvaluateTriggerTransition
	{
		Name = "Move->Busy"
	};

	// Token: 0x0400071B RID: 1819
	private EvaluateTriggerTransition m_MoveToOpUI = new EvaluateTriggerTransition
	{
		Name = "Move->OpUI"
	};

	// Token: 0x0400071C RID: 1820
	private EvaluateTriggerTransition m_MoveToTalk = new EvaluateTriggerTransition
	{
		Name = "Move->Talk"
	};

	// Token: 0x0400071D RID: 1821
	private EvaluateTriggerTransition m_MoveToTake = new EvaluateTriggerTransition
	{
		Name = "Move->Take"
	};

	// Token: 0x0400071E RID: 1822
	private EvaluateTriggerTransition m_BusyToIdle = new EvaluateTriggerTransition
	{
		Name = "Busy->Idle"
	};

	// Token: 0x0400071F RID: 1823
	private EvaluateTriggerTransition m_OpUIToIdle = new EvaluateTriggerTransition
	{
		Name = "OpUI->Idle"
	};

	// Token: 0x04000720 RID: 1824
	private EvaluateTriggerTransition m_TalkToIdle = new EvaluateTriggerTransition
	{
		Name = "Talk->Idle"
	};

	// Token: 0x04000721 RID: 1825
	private EvaluateTriggerTransition m_TakeToIdle = new EvaluateTriggerTransition
	{
		Name = "Take->Idle"
	};

	// Token: 0x04000722 RID: 1826
	private float fCosTheta = Mathf.Cos(0.785f);

	// Token: 0x04000723 RID: 1827
	private float fSquaredR = GameGlobal.m_fTalkRange * GameGlobal.m_fTalkRange;

	// Token: 0x04000724 RID: 1828
	private bool showTargetEffect = true;

	// Token: 0x04000725 RID: 1829
	private Vector3 m_DestPosition;

	// Token: 0x04000726 RID: 1830
	private float m_fDestDistance;

	// Token: 0x04000727 RID: 1831
	private float m_fOldDestDistance;

	// Token: 0x04000728 RID: 1832
	private float m_AutoStopCheckTime;

	// Token: 0x04000729 RID: 1833
	private float m_fCheckRange;

	// Token: 0x02000139 RID: 313
	public enum ePlayerState
	{
		// Token: 0x04000736 RID: 1846
		Idle,
		// Token: 0x04000737 RID: 1847
		DirMove,
		// Token: 0x04000738 RID: 1848
		ClickMove,
		// Token: 0x04000739 RID: 1849
		TakeItem,
		// Token: 0x0400073A RID: 1850
		None
	}
}
