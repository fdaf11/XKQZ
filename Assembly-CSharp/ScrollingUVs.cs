using System;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class ScrollingUVs : MonoBehaviour
{
	// Token: 0x0600225E RID: 8798 RVA: 0x0010D0E0 File Offset: 0x0010B2E0
	private void LateUpdate()
	{
		this.uvOffset += this.uvAnimationRate * Time.deltaTime;
		if (base.renderer.enabled)
		{
			base.renderer.materials[this.materialIndex].SetTextureOffset(this.textureName, this.uvOffset);
			if (this.ScrollBump)
			{
				base.renderer.materials[this.materialIndex].SetTextureOffset(this.bumpName, this.uvOffset);
			}
		}
	}

	// Token: 0x04002895 RID: 10389
	public int materialIndex;

	// Token: 0x04002896 RID: 10390
	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	// Token: 0x04002897 RID: 10391
	public string textureName = "_MainTex";

	// Token: 0x04002898 RID: 10392
	public bool ScrollBump = true;

	// Token: 0x04002899 RID: 10393
	public string bumpName = "_BumpMap";

	// Token: 0x0400289A RID: 10394
	private Vector2 uvOffset = Vector2.zero;
}
