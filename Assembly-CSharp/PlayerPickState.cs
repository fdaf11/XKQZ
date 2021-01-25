using System;
using Heluo.Wulin;
using Heluo.Wulin.FSM;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class PlayerPickState : StateBase
{
	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x00005EBC File Offset: 0x000040BC
	// (set) Token: 0x060006B0 RID: 1712 RVA: 0x00005EC9 File Offset: 0x000040C9
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

	// Token: 0x060006B1 RID: 1713 RVA: 0x00047548 File Offset: 0x00045748
	public override void OnStateEnter()
	{
		base.OnStateEnter();
		this.delayTime = 0f;
		float playerHeight = this.FSM.GetPlayerHeight();
		float targetHeight = this.FSM.GetTargetHeight();
		float num = targetHeight - playerHeight;
		if (num > 0.5f)
		{
			this.FSM.PlayAnimation("act_05_life01", 1f);
		}
		else
		{
			this.FSM.PlayAnimation("act_06_life02", 1f);
		}
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000475C8 File Offset: 0x000457C8
	public override void OnStateUpdate()
	{
		this.delayTime += Time.deltaTime;
		if (this.delayTime >= 1f)
		{
			this.FSM.SetState(PlayerFSM.ePlayerState.Idle);
			this.FSM.DoMouseEvent();
		}
		base.OnStateUpdate();
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnKeyUp(KeyControl.Key keyCode)
	{
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00005FED File Offset: 0x000041ED
	public override void OnStateExit()
	{
		base.OnStateExit();
		this.FSM.KeyUp -= new Action<KeyControl.Key>(this.OnKeyUp);
	}

	// Token: 0x0400073C RID: 1852
	private float delayTime;
}
