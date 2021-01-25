using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;

// Token: 0x0200013A RID: 314
public class PlayerBusyState : StateBase
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000690 RID: 1680 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x06000691 RID: 1681 RVA: 0x00005EC9 File Offset: 0x000040C9
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

	// Token: 0x06000692 RID: 1682 RVA: 0x00005ED2 File Offset: 0x000040D2
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.FSM.KeyUp += new Action<KeyControl.Key>(this.OnKeyUp);
		this.FSM.SetState(PlayerFSM.ePlayerState.Idle);
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00005F08 File Offset: 0x00004108
	public override void OnStateUpdate()
	{
		base.OnStateUpdate();
		this.FSM.UpdateTargetEffect();
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnKeyUp(KeyControl.Key keyCode)
	{
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00005F1B File Offset: 0x0000411B
	public override void OnStateExit()
	{
		base.OnStateExit();
		this.FSM.KeyUp -= new Action<KeyControl.Key>(this.OnKeyUp);
		this.FSM.ResetAnimationLayer();
	}
}
