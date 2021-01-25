using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x020006A5 RID: 1701
public class RotationHelper : MonoBehaviour
{
	// Token: 0x06002940 RID: 10560 RVA: 0x00146F58 File Offset: 0x00145158
	private void Start()
	{
		HOTween.To(base.transform, this.duration, new TweenParms().Prop("rotation", new Vector3((float)this.rotation, 0f, 0f)).Ease(0).Loops(-1, 3));
	}

	// Token: 0x04003439 RID: 13369
	public float duration;

	// Token: 0x0400343A RID: 13370
	public int rotation;
}
