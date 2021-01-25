using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class UIGeometry
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001E01 RID: 7681 RVA: 0x00013D6B File Offset: 0x00011F6B
	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06001E02 RID: 7682 RVA: 0x00013D7B File Offset: 0x00011F7B
	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x00013DB4 File Offset: 0x00011FB4
	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000E97FC File Offset: 0x000E79FC
	public void ApplyTransform(Matrix4x4 widgetToPanel)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
			Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
			this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000E98C8 File Offset: 0x000E7AC8
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
		}
	}

	// Token: 0x0400220E RID: 8718
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x0400220F RID: 8719
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x04002210 RID: 8720
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x04002211 RID: 8721
	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	// Token: 0x04002212 RID: 8722
	private Vector3 mRtpNormal;

	// Token: 0x04002213 RID: 8723
	private Vector4 mRtpTan;
}
