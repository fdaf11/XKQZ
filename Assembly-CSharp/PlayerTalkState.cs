using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;

// Token: 0x0200013F RID: 319
public class PlayerTalkState : StateBase
{
	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00005EC9 File Offset: 0x000040C9
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

	// Token: 0x060006B8 RID: 1720 RVA: 0x0000600C File Offset: 0x0000420C
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.FSM.SetState(PlayerFSM.ePlayerState.Idle);
		this.FSM.PlayAnimation("stand01", 1f);
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00006040 File Offset: 0x00004240
	public override void OnStateUpdate()
	{
		base.OnStateUpdate();
		this.FSM.UpdateTargetEffect();
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00006053 File Offset: 0x00004253
	public override void OnStateExit()
	{
		base.OnStateExit();
		this.FSM.ResetAnimationLayer();
	}
}
