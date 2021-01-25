using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D2 RID: 1490
public class WaterWaveUvAnimation : MonoBehaviour
{
	// Token: 0x06002501 RID: 9473 RVA: 0x00018846 File Offset: 0x00016A46
	private void Start()
	{
		this.mat = base.renderer.material;
		this.delta = 1f / (float)this.fps * this.speed;
		base.StartCoroutine(this.updateTiling());
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x00120E68 File Offset: 0x0011F068
	private IEnumerator updateTiling()
	{
		for (;;)
		{
			this.offset += this.delta;
			this.offsetHeight += this.delta;
			if (this.offset >= 1f)
			{
				Object.Destroy(base.gameObject);
			}
			Vector2 vec = new Vector2(0f, this.offset);
			this.mat.SetTextureOffset("_BumpMap", vec);
			this.mat.SetFloat("_OffsetYHeightMap", this.offset);
			if (this.offset < 0.3f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, this.offset / 0.3f));
			}
			if (this.offset > 0.7f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, (1f - this.offset) / 0.3f));
			}
			yield return new WaitForSeconds(1f / (float)this.fps);
		}
		yield break;
	}

	// Token: 0x04002D2B RID: 11563
	public float speed = 1f;

	// Token: 0x04002D2C RID: 11564
	public int fps = 30;

	// Token: 0x04002D2D RID: 11565
	public Color color;

	// Token: 0x04002D2E RID: 11566
	private Material mat;

	// Token: 0x04002D2F RID: 11567
	private float offset;

	// Token: 0x04002D30 RID: 11568
	private float offsetHeight;

	// Token: 0x04002D31 RID: 11569
	private float delta;
}
