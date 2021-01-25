using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
[Serializable]
public class ImposterTexture
{
	// Token: 0x06001999 RID: 6553 RVA: 0x00010A31 File Offset: 0x0000EC31
	public float getMemoryAmount()
	{
		return 12f * (float)(this.size * this.size) / 1048576f;
	}

	// Token: 0x04001E3C RID: 7740
	public int size;

	// Token: 0x04001E3D RID: 7741
	public RenderTexture texture;

	// Token: 0x04001E3E RID: 7742
	public ImposterProxy owner;

	// Token: 0x04001E3F RID: 7743
	public int x;

	// Token: 0x04001E40 RID: 7744
	public int y;

	// Token: 0x04001E41 RID: 7745
	public int h;

	// Token: 0x04001E42 RID: 7746
	public int w;

	// Token: 0x04001E43 RID: 7747
	public int createdTime;

	// Token: 0x04001E44 RID: 7748
	public int lastUsedTime;
}
