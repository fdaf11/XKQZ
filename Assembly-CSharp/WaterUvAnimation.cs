using System;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
public class WaterUvAnimation : MonoBehaviour
{
	// Token: 0x060025C7 RID: 9671 RVA: 0x00019271 File Offset: 0x00017471
	private void Awake()
	{
		this.mat = base.renderer.materials[this.MaterialNomber];
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x00124ABC File Offset: 0x00122CBC
	private void Update()
	{
		if (this.IsReverse)
		{
			this.offset -= Time.deltaTime * this.Speed;
			if (this.offset < 0f)
			{
				this.offset = 1f;
			}
		}
		else
		{
			this.offset += Time.deltaTime * this.Speed;
			if (this.offset > 1f)
			{
				this.offset = 0f;
			}
		}
		Vector2 vector;
		vector..ctor(0f, this.offset);
		this.mat.SetTextureOffset("_BumpMap", vector);
		this.mat.SetFloat("_OffsetYHeightMap", this.offset);
	}

	// Token: 0x04002E50 RID: 11856
	public bool IsReverse;

	// Token: 0x04002E51 RID: 11857
	public float Speed = 1f;

	// Token: 0x04002E52 RID: 11858
	public int MaterialNomber;

	// Token: 0x04002E53 RID: 11859
	private Material mat;

	// Token: 0x04002E54 RID: 11860
	private float deltaFps;

	// Token: 0x04002E55 RID: 11861
	private bool isVisible;

	// Token: 0x04002E56 RID: 11862
	private bool isCorutineStarted;

	// Token: 0x04002E57 RID: 11863
	private float offset;

	// Token: 0x04002E58 RID: 11864
	private float delta;
}
