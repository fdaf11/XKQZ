using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class JoystickControl : KeyControl
{
	// Token: 0x06000948 RID: 2376 RVA: 0x00007A43 File Offset: 0x00005C43
	public JoystickControl(Dictionary<KeyControl.Key, KeyCode> mapping) : base(mapping)
	{
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00007A57 File Offset: 0x00005C57
	public JoystickControl()
	{
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0005018C File Offset: 0x0004E38C
	public override void Update()
	{
		base.Update();
		this.Direction.x = Input.GetAxis("X Axis");
		this.Direction.y = Input.GetAxis("Y Axis");
		this.dpadDirection.x = Input.GetAxis("D-Pad X Axis");
		this.dpadDirection.y = Input.GetAxis("D-Pad Y Axis");
		if (this.dpadDirection.sqrMagnitude > this.Direction.sqrMagnitude)
		{
			this.Direction = Vector2.Lerp(this.Direction, this.dpadDirection, this.smoothSpeed * Time.deltaTime);
		}
		this.OnAnalogTrigger(this.Direction.x, this.prevDirection.x, KeyControl.Key.Right, KeyControl.Key.Left);
		this.OnAnalogTrigger(this.Direction.y, this.prevDirection.y, KeyControl.Key.Up, KeyControl.Key.Down);
		this.prevDirection = this.Direction;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0005027C File Offset: 0x0004E47C
	private void OnAnalogTrigger(float value, float prevValue, KeyControl.Key positive, KeyControl.Key nagetive)
	{
		if (value >= 0.5f && 0.5f > prevValue)
		{
			this.KeyDown.Invoke(positive);
		}
		else if (value < 0.5f && 0.5f <= prevValue)
		{
			this.KeyUp.Invoke(positive);
		}
		else if (value >= 0.5f)
		{
			this.KeyHeld.Invoke(positive);
		}
		if (value <= -0.5f && -0.5f < prevValue)
		{
			this.KeyDown.Invoke(nagetive);
		}
		else if (value > -0.5f && -0.5f >= prevValue)
		{
			this.KeyUp.Invoke(nagetive);
		}
		else if (value <= -0.5f)
		{
			this.KeyHeld.Invoke(nagetive);
		}
	}

	// Token: 0x04000901 RID: 2305
	private const float Threshold = 0.5f;

	// Token: 0x04000902 RID: 2306
	private const float SquareThreshold = 0.25f;

	// Token: 0x04000903 RID: 2307
	private Vector2 dpadDirection;

	// Token: 0x04000904 RID: 2308
	public float smoothSpeed = 10f;
}
