using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class CameraPathEvent : CameraPathPoint
{
	// Token: 0x040001DE RID: 478
	public CameraPathEvent.Types type;

	// Token: 0x040001DF RID: 479
	public string eventName = "Camera Path Event";

	// Token: 0x040001E0 RID: 480
	public GameObject target;

	// Token: 0x040001E1 RID: 481
	public string methodName;

	// Token: 0x040001E2 RID: 482
	public string methodArgument;

	// Token: 0x040001E3 RID: 483
	public CameraPathEvent.ArgumentTypes argumentType;

	// Token: 0x0200006E RID: 110
	public enum Types
	{
		// Token: 0x040001E5 RID: 485
		Broadcast,
		// Token: 0x040001E6 RID: 486
		Call
	}

	// Token: 0x0200006F RID: 111
	public enum ArgumentTypes
	{
		// Token: 0x040001E8 RID: 488
		None,
		// Token: 0x040001E9 RID: 489
		Float,
		// Token: 0x040001EA RID: 490
		Int,
		// Token: 0x040001EB RID: 491
		String
	}
}
