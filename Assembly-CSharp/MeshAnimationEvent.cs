using System;
using UnityEngine;

// Token: 0x02000677 RID: 1655
[Serializable]
public class MeshAnimationEvent
{
	// Token: 0x0600286D RID: 10349 RVA: 0x001401DC File Offset: 0x0013E3DC
	public void FireEvent(GameObject eventReciever)
	{
		if (eventReciever)
		{
			if (this.eventType == MeshAnimationEvent.Mode.Data)
			{
				eventReciever.SendMessage(this.methodName, this.data, 1);
			}
			else if (this.eventType == MeshAnimationEvent.Mode.Float)
			{
				eventReciever.SendMessage(this.methodName, this.floatValue, 1);
			}
			else if (this.eventType == MeshAnimationEvent.Mode.String)
			{
				eventReciever.SendMessage(this.methodName, this.stringValue, 1);
			}
		}
	}

	// Token: 0x040032C2 RID: 12994
	public string methodName;

	// Token: 0x040032C3 RID: 12995
	public int frame;

	// Token: 0x040032C4 RID: 12996
	public MeshAnimationEvent.Mode eventType;

	// Token: 0x040032C5 RID: 12997
	public string stringValue;

	// Token: 0x040032C6 RID: 12998
	public float floatValue;

	// Token: 0x040032C7 RID: 12999
	public Object data;

	// Token: 0x02000678 RID: 1656
	public enum Mode : byte
	{
		// Token: 0x040032C9 RID: 13001
		Data,
		// Token: 0x040032CA RID: 13002
		String,
		// Token: 0x040032CB RID: 13003
		Float
	}
}
