using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class PlayerMoveState : StateBase
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600069F RID: 1695 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00005EC9 File Offset: 0x000040C9
	public new PlayerFSM FSM
	{
		get
		{
			return base.FSM as PlayerFSM;
		}
		set
		{
			base.FSM = value;
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x000470E8 File Offset: 0x000452E8
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.FSM.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
		this.FSM.KeyUp += new Action<KeyControl.Key>(this.OnKeyUp);
		this.FSM.Move += new Action<Vector2>(this.OnMove);
		if (GameGlobal.m_bBigMapMode && this.FSM.m_bJump)
		{
			return;
		}
		if (GameGlobal.m_bMovie || GameGlobal.m_bPlayerTalk || GameGlobal.m_bCFormOpen)
		{
			return;
		}
		this.FSM.PlayAnimation("run", 1f);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00005F7B File Offset: 0x0000417B
	public override void OnStateUpdate()
	{
		if (Input.GetMouseButtonUp(1))
		{
			Game.UI.Show<UIMainSelect>();
		}
		this.UpdateMove();
		base.OnStateUpdate();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00047190 File Offset: 0x00045390
	public void UpdateMove()
	{
		if (this.FSM.nowState == PlayerFSM.ePlayerState.DirMove)
		{
			if (Camera.main == null)
			{
				return;
			}
			Vector3 vector = Camera.main.transform.TransformDirection(new Vector3(this.dir.x, 0f, this.dir.y));
			vector.y = 0f;
			this.FSM.SetDirMove(vector, true);
			this.FSM.UpdateMoveSpeed();
		}
		else if (this.FSM.nowState == PlayerFSM.ePlayerState.ClickMove)
		{
			this.FSM.UpdateMouseMove();
			this.FSM.UpdateMoveSpeed();
		}
		this.FSM.UpdateTargetEffect();
		if (!GameGlobal.m_bBigMapMode)
		{
			if (!this.FSM.IsPlayingAnimation("run"))
			{
				this.FSM.PlayAnimation("run", 1f);
			}
			return;
		}
		if (this.FSM.UpdateJump())
		{
			this.FSM.PlayAnimation("run", 1f);
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000472A4 File Offset: 0x000454A4
	public void OnMove(Vector2 direction)
	{
		if (this.FSM.nowState != PlayerFSM.ePlayerState.DirMove)
		{
			return;
		}
		if (direction.sqrMagnitude <= 0.5f)
		{
			this.FSM.StopKeyMove();
			this.FSM.SetState(PlayerFSM.ePlayerState.Idle);
			return;
		}
		this.dir = direction;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000472F4 File Offset: 0x000454F4
	public void OnKeyDown(KeyControl.Key keyCode)
	{
		switch (keyCode)
		{
		case KeyControl.Key.Up:
		case KeyControl.Key.Down:
		case KeyControl.Key.Left:
		case KeyControl.Key.Right:
			this.FSM.SetState(PlayerFSM.ePlayerState.DirMove);
			break;
		default:
			if (keyCode == KeyControl.Key.AddSpeed)
			{
				this.FSM.AddSpeed();
			}
			break;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00047348 File Offset: 0x00045548
	public void OnKeyUp(KeyControl.Key keyCode)
	{
		switch (keyCode)
		{
		case KeyControl.Key.Team:
			Game.UI.Show<UITeam>();
			break;
		case KeyControl.Key.Backpack:
			Game.UI.Show<UIBackpack>();
			break;
		case KeyControl.Key.Mission:
			Game.UI.Show<UIMission>();
			break;
		case KeyControl.Key.Rumor:
			Game.UI.Show<UISaveRumor>();
			break;
		case KeyControl.Key.Save:
			Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(1);
			break;
		case KeyControl.Key.Load:
			Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(3);
			break;
		default:
			switch (keyCode)
			{
			case KeyControl.Key.Menu:
				Game.UI.Show<UIMainSelect>();
				break;
			case KeyControl.Key.Jump:
				if (GameGlobal.m_bBigMapMode)
				{
					this.FSM.SetJump();
				}
				break;
			case KeyControl.Key.ChangeModel:
				if (GameGlobal.m_bBigMapMode)
				{
					BigMapController.m_Instance.ChangeModel();
					this.FSM.ReSetAnimation();
					if (GameGlobal.m_bMovie || GameGlobal.m_bPlayerTalk || GameGlobal.m_bCFormOpen)
					{
						return;
					}
					this.FSM.PlayAnimation("run", 1f);
				}
				break;
			case KeyControl.Key.TalkLog:
				Game.UI.Show<UISaveTalk>();
				break;
			case KeyControl.Key.AddSpeed:
				this.FSM.ResetAddSpeed();
				break;
			default:
				if (keyCode == KeyControl.Key.OK)
				{
					if (GameGlobal.m_bBigMapMode)
					{
						UIBigMapEnter uibigMapEnter = Game.UI.Get<UIBigMapEnter>();
						if (uibigMapEnter)
						{
							uibigMapEnter.Enter();
						}
					}
					else
					{
						this.FSM.DoTargetEvent();
					}
				}
				break;
			}
			break;
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000474E4 File Offset: 0x000456E4
	public override void OnStateExit()
	{
		base.OnStateExit();
		this.FSM.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
		this.FSM.KeyUp -= new Action<KeyControl.Key>(this.OnKeyUp);
		this.FSM.Move -= new Action<Vector2>(this.OnMove);
		this.FSM.StopMove();
	}

	// Token: 0x0400073B RID: 1851
	private Vector2 dir;
}
