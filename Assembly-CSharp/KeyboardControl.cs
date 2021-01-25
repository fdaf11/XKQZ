using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class KeyboardControl : KeyControl
{
	// Token: 0x0600094C RID: 2380 RVA: 0x00007A6A File Offset: 0x00005C6A
	public KeyboardControl(Dictionary<KeyControl.Key, KeyCode> mapping) : base(mapping)
	{
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00007A7E File Offset: 0x00005C7E
	public KeyboardControl()
	{
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00050358 File Offset: 0x0004E558
	public override void Update()
	{
		base.Update();
		Vector2 zero = Vector2.zero;
		if (Input.GetKey(this.key[0]))
		{
			zero.y = 1f;
		}
		else if (Input.GetKey(this.key[1]))
		{
			zero.y = -1f;
		}
		else
		{
			zero.y = 0f;
		}
		if (Input.GetKey(this.key[3]))
		{
			zero.x = 1f;
		}
		else if (Input.GetKey(this.key[2]))
		{
			zero.x = -1f;
		}
		else
		{
			zero.x = 0f;
		}
		if (Vector2.Distance(this.Direction, zero) < 0.3f)
		{
			this.Direction = zero;
		}
		else
		{
			this.Direction = Vector2.Lerp(this.Direction, zero, this.smoothSpeed * Time.deltaTime);
		}
	}

	// Token: 0x04000905 RID: 2309
	public float smoothSpeed = 10f;
}
