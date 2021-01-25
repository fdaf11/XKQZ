using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public class QueueUvAnimation : MonoBehaviour
{
	// Token: 0x06002578 RID: 9592 RVA: 0x00018F25 File Offset: 0x00017125
	private void Start()
	{
		this.deltaTime = 1f / this.Fps;
		this.InitDefaultTex(this.RowsFadeIn, this.ColumnsFadeIn);
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x0012334C File Offset: 0x0012154C
	private void InitDefaultTex(int rows, int colums)
	{
		this.count = rows * colums;
		this.index += colums - 1;
		Vector2 vector;
		vector..ctor(1f / (float)colums, 1f / (float)rows);
		base.renderer.material.SetTextureScale("_MainTex", vector);
		if (this.IsBump)
		{
			base.renderer.material.SetTextureScale("_BumpMap", vector);
		}
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x00018F4B File Offset: 0x0001714B
	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateTiling());
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x00018F61 File Offset: 0x00017161
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x001233C0 File Offset: 0x001215C0
	private IEnumerator UpdateTiling()
	{
		while (this.isVisible && this.allCount != this.count)
		{
			this.allCount++;
			this.index++;
			if (this.index >= this.count)
			{
				this.index = 0;
			}
			Vector2 offset = this.isFadeHandle ? new Vector2((float)this.index / (float)this.ColumnsLoop - (float)(this.index / this.ColumnsLoop), 1f - (float)(this.index / this.ColumnsLoop) / (float)this.RowsLoop) : new Vector2((float)this.index / (float)this.ColumnsFadeIn - (float)(this.index / this.ColumnsFadeIn), 1f - (float)(this.index / this.ColumnsFadeIn) / (float)this.RowsFadeIn);
			if (!this.isFadeHandle)
			{
				base.renderer.material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.renderer.material.SetTextureOffset("_BumpMap", offset);
				}
			}
			else
			{
				base.renderer.material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.renderer.material.SetTextureOffset("_BumpMap", offset);
				}
			}
			if (this.allCount == this.count)
			{
				this.isFadeHandle = true;
				base.renderer.material = this.NextMaterial;
				this.InitDefaultTex(this.RowsLoop, this.ColumnsLoop);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	// Token: 0x04002DEE RID: 11758
	public int RowsFadeIn = 4;

	// Token: 0x04002DEF RID: 11759
	public int ColumnsFadeIn = 4;

	// Token: 0x04002DF0 RID: 11760
	public int RowsLoop = 4;

	// Token: 0x04002DF1 RID: 11761
	public int ColumnsLoop = 4;

	// Token: 0x04002DF2 RID: 11762
	public float Fps = 20f;

	// Token: 0x04002DF3 RID: 11763
	public bool IsBump;

	// Token: 0x04002DF4 RID: 11764
	public Material NextMaterial;

	// Token: 0x04002DF5 RID: 11765
	private int index;

	// Token: 0x04002DF6 RID: 11766
	private int count;

	// Token: 0x04002DF7 RID: 11767
	private int allCount;

	// Token: 0x04002DF8 RID: 11768
	private float deltaTime;

	// Token: 0x04002DF9 RID: 11769
	private bool isVisible;

	// Token: 0x04002DFA RID: 11770
	private bool isFadeHandle;
}
