using System;
using UnityEngine;

// Token: 0x02000830 RID: 2096
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public abstract class TOD_PostEffectsBase : MonoBehaviour
{
	// Token: 0x060032FF RID: 13055
	protected abstract bool CheckResources();

	// Token: 0x06003300 RID: 13056 RVA: 0x001890C4 File Offset: 0x001872C4
	protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
	{
		if (!shader)
		{
			Debug.Log("Missing shader in " + this.ToString());
			base.enabled = false;
			return null;
		}
		if (shader.isSupported && material && material.shader == shader)
		{
			return material;
		}
		if (!shader.isSupported)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"The shader ",
				shader.ToString(),
				" on effect ",
				this.ToString(),
				" is not supported on this platform!"
			}));
			base.enabled = false;
			return null;
		}
		material = new Material(shader);
		material.hideFlags = 4;
		return (!material) ? null : material;
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x00189194 File Offset: 0x00187394
	protected Material CreateMaterial(Shader shader, Material material)
	{
		if (!shader)
		{
			Debug.Log("Missing shader in " + this.ToString());
			return null;
		}
		if (material && material.shader == shader && shader.isSupported)
		{
			return material;
		}
		if (!shader.isSupported)
		{
			return null;
		}
		material = new Material(shader);
		material.hideFlags = 4;
		return (!material) ? null : material;
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x0018921C File Offset: 0x0018741C
	protected void OnEnable()
	{
		if (!this.cam)
		{
			this.cam = base.GetComponent<Camera>();
		}
		if (!this.sky)
		{
			this.sky = (Object.FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky);
		}
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
	protected void Start()
	{
		this.CheckResources();
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x00189270 File Offset: 0x00187470
	protected bool CheckSupport(bool needDepth = false, bool needHdr = false)
	{
		if (!this.sky)
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it requires a valid sky dome reference.");
			base.enabled = false;
			return false;
		}
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
			base.enabled = false;
			return false;
		}
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(1))
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it requires a depth texture.");
			base.enabled = false;
			return false;
		}
		if (needDepth)
		{
			this.cam.depthTextureMode |= 1;
		}
		if (needHdr && !SystemInfo.SupportsRenderTextureFormat(2))
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it requires HDR.");
			base.enabled = false;
			return false;
		}
		return true;
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x0018936C File Offset: 0x0018756C
	protected void DrawBorder(RenderTexture dest, Material material)
	{
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float num;
			float num2;
			if (flag)
			{
				num = 1f;
				num2 = 0f;
			}
			else
			{
				num = 0f;
				num2 = 1f;
			}
			float num3 = 0f;
			float num4 = 0f + 1f / ((float)dest.width * 1f);
			float num5 = 0f;
			float num6 = 1f;
			GL.Begin(7);
			GL.TexCoord2(0f, num);
			GL.Vertex3(num3, num5, 0.1f);
			GL.TexCoord2(1f, num);
			GL.Vertex3(num4, num5, 0.1f);
			GL.TexCoord2(1f, num2);
			GL.Vertex3(num4, num6, 0.1f);
			GL.TexCoord2(0f, num2);
			GL.Vertex3(num3, num6, 0.1f);
			num3 = 1f - 1f / ((float)dest.width * 1f);
			num4 = 1f;
			num5 = 0f;
			num6 = 1f;
			GL.TexCoord2(0f, num);
			GL.Vertex3(num3, num5, 0.1f);
			GL.TexCoord2(1f, num);
			GL.Vertex3(num4, num5, 0.1f);
			GL.TexCoord2(1f, num2);
			GL.Vertex3(num4, num6, 0.1f);
			GL.TexCoord2(0f, num2);
			GL.Vertex3(num3, num6, 0.1f);
			num3 = 0f;
			num4 = 1f;
			num5 = 0f;
			num6 = 0f + 1f / ((float)dest.height * 1f);
			GL.TexCoord2(0f, num);
			GL.Vertex3(num3, num5, 0.1f);
			GL.TexCoord2(1f, num);
			GL.Vertex3(num4, num5, 0.1f);
			GL.TexCoord2(1f, num2);
			GL.Vertex3(num4, num6, 0.1f);
			GL.TexCoord2(0f, num2);
			GL.Vertex3(num3, num6, 0.1f);
			num3 = 0f;
			num4 = 1f;
			num5 = 1f - 1f / ((float)dest.height * 1f);
			num6 = 1f;
			GL.TexCoord2(0f, num);
			GL.Vertex3(num3, num5, 0.1f);
			GL.TexCoord2(1f, num);
			GL.Vertex3(num4, num5, 0.1f);
			GL.TexCoord2(1f, num2);
			GL.Vertex3(num4, num6, 0.1f);
			GL.TexCoord2(0f, num2);
			GL.Vertex3(num3, num6, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	// Token: 0x04003EE8 RID: 16104
	public TOD_Sky sky;

	// Token: 0x04003EE9 RID: 16105
	protected Camera cam;
}
