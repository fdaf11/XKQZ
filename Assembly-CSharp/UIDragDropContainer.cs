using System;
using UnityEngine;

// Token: 0x02000452 RID: 1106
[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
	// Token: 0x06001A8D RID: 6797 RVA: 0x00011747 File Offset: 0x0000F947
	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	// Token: 0x04001F4A RID: 8010
	public Transform reparentTarget;
}
