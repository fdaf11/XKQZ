using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class CameraDeAlpha : MonoBehaviour
{
	// Token: 0x060002FB RID: 763 RVA: 0x0002AC64 File Offset: 0x00028E64
	private void OnPostRender()
	{
		if (!this.mat)
		{
			this.mat = new Material("Shader \"Hidden/Alpha\" {SubShader {    Pass {        ZTest Always Cull Off ZWrite Off        ColorMask A        Color (1,1,1,1)    }}}");
			this.mat.shader.hideFlags = 13;
			this.mat.hideFlags = 13;
		}
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < this.mat.passCount; i++)
		{
			this.mat.SetPass(i);
			GL.Begin(7);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	// Token: 0x04000232 RID: 562
	private Material mat;
}
