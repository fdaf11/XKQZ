using System;
using Heluo.Wulin.FSM;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class PlayerFSMBase : FiniteStateMachine, IController
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000640 RID: 1600 RVA: 0x00005A29 File Offset: 0x00003C29
	// (remove) Token: 0x06000641 RID: 1601 RVA: 0x00005A42 File Offset: 0x00003C42
	public event Action<KeyControl.Key> KeyDown;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000642 RID: 1602 RVA: 0x00005A5B File Offset: 0x00003C5B
	// (remove) Token: 0x06000643 RID: 1603 RVA: 0x00005A74 File Offset: 0x00003C74
	public event Action<KeyControl.Key> KeyUp;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000644 RID: 1604 RVA: 0x00005A8D File Offset: 0x00003C8D
	// (remove) Token: 0x06000645 RID: 1605 RVA: 0x00005AA6 File Offset: 0x00003CA6
	public event Action<KeyControl.Key> KeyHeld;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000646 RID: 1606 RVA: 0x00005ABF File Offset: 0x00003CBF
	// (remove) Token: 0x06000647 RID: 1607 RVA: 0x00005AD8 File Offset: 0x00003CD8
	public event Action<Vector2> Move;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000648 RID: 1608 RVA: 0x00005AF1 File Offset: 0x00003CF1
	// (remove) Token: 0x06000649 RID: 1609 RVA: 0x00005B0A File Offset: 0x00003D0A
	public event Action<bool> MouseControl;

	// Token: 0x0600064A RID: 1610 RVA: 0x00005B23 File Offset: 0x00003D23
	public void OnMove(Vector2 diretion)
	{
		if (this.Move != null)
		{
			this.Move.Invoke(diretion);
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00005B3C File Offset: 0x00003D3C
	public void OnKeyUp(KeyControl.Key key)
	{
		if (this.KeyUp != null)
		{
			this.KeyUp.Invoke(key);
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00005B55 File Offset: 0x00003D55
	public void OnKeyDown(KeyControl.Key key)
	{
		if (this.KeyDown != null)
		{
			this.KeyDown.Invoke(key);
		}
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00005B6E File Offset: 0x00003D6E
	public void OnKeyHeld(KeyControl.Key key)
	{
		if (this.KeyHeld != null)
		{
			this.KeyHeld.Invoke(key);
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00005B87 File Offset: 0x00003D87
	public void OnMouseControl(bool bCtrl)
	{
		if (this.MouseControl != null)
		{
			this.MouseControl.Invoke(bCtrl);
		}
	}
}
