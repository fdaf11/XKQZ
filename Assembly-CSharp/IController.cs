using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public interface IController
{
	// Token: 0x06000920 RID: 2336
	void OnMove(Vector2 diretion);

	// Token: 0x06000921 RID: 2337
	void OnKeyUp(KeyControl.Key key);

	// Token: 0x06000922 RID: 2338
	void OnKeyDown(KeyControl.Key key);

	// Token: 0x06000923 RID: 2339
	void OnKeyHeld(KeyControl.Key key);

	// Token: 0x06000924 RID: 2340
	void OnMouseControl(bool bCtrl);
}
