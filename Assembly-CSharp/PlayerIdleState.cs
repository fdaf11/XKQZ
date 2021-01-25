using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class PlayerIdleState : StateBase
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000697 RID: 1687 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x06000698 RID: 1688 RVA: 0x00005EC9 File Offset: 0x000040C9
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

	// Token: 0x06000699 RID: 1689 RVA: 0x00046DF0 File Offset: 0x00044FF0
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.FSM.KeyHeld += new Action<KeyControl.Key>(this.OnKeyHeld);
		this.FSM.KeyUp += new Action<KeyControl.Key>(this.OnKeyUp);
		this.FSM.ResetAddSpeed();
		if (GameGlobal.m_bBigMapMode && this.FSM.m_bJump)
		{
			return;
		}
		if (GameGlobal.m_bMovie || GameGlobal.m_bPlayerTalk || GameGlobal.m_bCFormOpen)
		{
			return;
		}
		this.FSM.PlayAnimation("stand01", 1f);
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00046E8C File Offset: 0x0004508C
	public override void OnStateUpdate()
	{
		if (Input.GetMouseButtonUp(1))
		{
			Game.UI.Show<UIMainSelect>();
		}
		base.OnStateUpdate();
		this.FSM.UpdateTargetEffect();
		if (!GameGlobal.m_bBigMapMode)
		{
			return;
		}
		if (this.FSM.UpdateJump())
		{
			this.FSM.PlayAnimation("stand01", 1f);
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00046EF0 File Offset: 0x000450F0
	public void OnKeyHeld(KeyControl.Key keyCode)
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

	// Token: 0x0600069C RID: 1692 RVA: 0x00046F44 File Offset: 0x00045144
	public void OnKeyUp(KeyControl.Key keyCode)
	{
		switch (keyCode)
		{
		case KeyControl.Key.Character:
			Game.UI.Show<UICharacter>();
			break;
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
					this.FSM.PlayAnimation("stand01", 1f);
				}
				break;
			case KeyControl.Key.TalkLog:
				Game.UI.Show<UISaveTalk>();
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
						this.FSM.SearchTarget();
						this.FSM.DoTargetEvent();
					}
				}
				break;
			}
			break;
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00005F45 File Offset: 0x00004145
	public override void OnStateExit()
	{
		base.OnStateExit();
		this.FSM.KeyHeld -= new Action<KeyControl.Key>(this.OnKeyHeld);
		this.FSM.KeyUp -= new Action<KeyControl.Key>(this.OnKeyUp);
	}
}
