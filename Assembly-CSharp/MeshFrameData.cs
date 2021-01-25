using System;
using UnityEngine;

// Token: 0x0200067B RID: 1659
[Serializable]
public class MeshFrameData
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06002889 RID: 10377 RVA: 0x0001ABCC File Offset: 0x00018DCC
	public Vector3[] verts
	{
		get
		{
			return this.decompressed;
		}
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x0001ABD4 File Offset: 0x00018DD4
	public void SetVerts(Vector3[] v)
	{
		this.decompressed = v;
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x00140FB4 File Offset: 0x0013F1B4
	public override bool Equals(object obj)
	{
		if (!(obj is MeshFrameData))
		{
			return base.Equals(obj);
		}
		MeshFrameData meshFrameData = (MeshFrameData)obj;
		if (meshFrameData.verts.Length != this.verts.Length)
		{
			return false;
		}
		for (int i = 0; i < meshFrameData.verts.Length; i++)
		{
			if (this.verts[i] != meshFrameData.verts[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x0001ABDD File Offset: 0x00018DDD
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x040032F6 RID: 13046
	[NonSerialized]
	private Vector3[] decompressed;
}
