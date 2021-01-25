using System;
using UnityEngine;

// Token: 0x0200082B RID: 2091
[Serializable]
public class TOD_CloudParameters
{
	// Token: 0x060032F5 RID: 13045 RVA: 0x00188F3C File Offset: 0x0018713C
	public void CheckRange()
	{
		this.Scale1 = new Vector2(Mathf.Max(1f, this.Scale1.x), Mathf.Max(1f, this.Scale1.y));
		this.Scale2 = new Vector2(Mathf.Max(1f, this.Scale2.x), Mathf.Max(1f, this.Scale2.y));
		this.Density = Mathf.Max(0f, this.Density);
		this.Sharpness = Mathf.Max(0f, this.Sharpness);
		this.Brightness = Mathf.Max(0f, this.Brightness);
		this.Glow = Mathf.Max(0f, this.Glow);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
	}

	// Token: 0x04003ED0 RID: 16080
	public float Density = 1f;

	// Token: 0x04003ED1 RID: 16081
	public float Sharpness = 1f;

	// Token: 0x04003ED2 RID: 16082
	public float Brightness = 1f;

	// Token: 0x04003ED3 RID: 16083
	public float Glow = 1f;

	// Token: 0x04003ED4 RID: 16084
	public float ShadowStrength;

	// Token: 0x04003ED5 RID: 16085
	public Vector2 Scale1 = new Vector2(3f, 3f);

	// Token: 0x04003ED6 RID: 16086
	public Vector2 Scale2 = new Vector2(7f, 7f);
}
