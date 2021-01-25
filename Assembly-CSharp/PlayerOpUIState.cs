using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;

// Token: 0x0200013D RID: 317
public class PlayerOpUIState : StateBase
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x060006AA RID: 1706 RVA: 0x00005EC9 File Offset: 0x000040C9
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

	// Token: 0x060006AB RID: 1707 RVA: 0x00005F9E File Offset: 0x0000419E
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.FSM.SetState(PlayerFSM.ePlayerState.Idle);
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		this.FSM.PlayAnimation("stand01", 1f);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00005FD2 File Offset: 0x000041D2
	public override void OnStateUpdate()
	{
		base.OnStateUpdate();
		this.FSM.UpdateTargetEffect();
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00005FE5 File Offset: 0x000041E5
	public override void OnStateExit()
	{
		base.OnStateExit();
	}
}
