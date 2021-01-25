using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000549 RID: 1353
internal class AnimateTiledTexture : MonoBehaviour
{
	// Token: 0x0600224A RID: 8778 RVA: 0x0010CB78 File Offset: 0x0010AD78
	private void Start()
	{
		base.StartCoroutine(this.updateTiling());
		Vector2 vector;
		vector..ctor(1f / (float)this.columns, 1f / (float)this.rows);
		base.renderer.sharedMaterial.SetTextureScale("_MainTex", vector);
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x0010CBCC File Offset: 0x0010ADCC
	private IEnumerator updateTiling()
	{
		for (;;)
		{
			this.index++;
			if (this.index >= this.rows * this.columns)
			{
				this.index = 0;
			}
			Vector2 offset = new Vector2((float)this.index / (float)this.columns - (float)(this.index / this.columns), (float)(this.index / this.columns) / (float)this.rows);
			base.renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
			yield return new WaitForSeconds(1f / this.framesPerSecond);
		}
		yield break;
	}

	// Token: 0x04002877 RID: 10359
	public int columns = 2;

	// Token: 0x04002878 RID: 10360
	public int rows = 2;

	// Token: 0x04002879 RID: 10361
	public float framesPerSecond = 10f;

	// Token: 0x0400287A RID: 10362
	private int index;
}
