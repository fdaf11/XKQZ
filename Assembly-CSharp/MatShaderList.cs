using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public class MatShaderList
{
	// Token: 0x0600302B RID: 12331 RVA: 0x0001E701 File Offset: 0x0001C901
	public void Add(Material mat, Shader sha)
	{
		this.count++;
		this.mats.Add(mat);
		this.shaders.Add(sha);
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x00175EC0 File Offset: 0x001740C0
	public void RemoveAt(int num)
	{
		this.count--;
		if (num < this.mats.Count && num < this.shaders.Count)
		{
			this.mats.RemoveAt(num);
			this.shaders.RemoveAt(num);
		}
	}

	// Token: 0x04003BDE RID: 15326
	public int count;

	// Token: 0x04003BDF RID: 15327
	public List<Material> mats = new List<Material>();

	// Token: 0x04003BE0 RID: 15328
	public List<Shader> shaders = new List<Shader>();
}
