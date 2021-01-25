using System;
using System.Collections.Generic;
using System.IO;
using SSAA;
using UnityEngine;

// Token: 0x02000712 RID: 1810
public class SuperSampling_SSAA : MonoBehaviour
{
	// Token: 0x06002AFE RID: 11006 RVA: 0x0014E3E0 File Offset: 0x0014C5E0
	private void OnEnable()
	{
		Camera component = base.GetComponent<Camera>();
		if (component == null)
		{
			Debug.LogWarning("No Camera attached!");
			return;
		}
		if (component.targetTexture == null)
		{
			internal_SSAA internal_SSAA = base.gameObject.AddComponent<internal_SSAA>();
			internal_SSAA.hideFlags = 15;
			internal_SSAA.Filter = this.Filter;
			internal_SSAA.ChangeScale(this.Scale);
			internal_SSAA.Format = this.renderTextureFormat;
		}
		else
		{
			SSAARenderTarget ssaarenderTarget = base.gameObject.AddComponent<SSAARenderTarget>();
			ssaarenderTarget.hideFlags = 15;
			ssaarenderTarget.Scale = this.Scale;
			ssaarenderTarget.TargetTexture = component.targetTexture;
			ssaarenderTarget.Filter = this.Filter;
			ssaarenderTarget.Format = this.renderTextureFormat;
		}
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x0014E49C File Offset: 0x0014C69C
	public void Start()
	{
		Camera component = base.GetComponent<Camera>();
		if (component.targetTexture == null)
		{
			internal_SSAA component2 = base.gameObject.GetComponent<internal_SSAA>();
			if (component2 != null)
			{
				component2.StartSSAA();
			}
		}
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x0014E4E0 File Offset: 0x0014C6E0
	public void Stop()
	{
		Camera component = base.GetComponent<Camera>();
		if (component.targetTexture == null)
		{
			internal_SSAA component2 = base.gameObject.GetComponent<internal_SSAA>();
			if (component2 != null)
			{
				component2.StopSSAA();
			}
		}
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x0014E524 File Offset: 0x0014C724
	private void OnDisable()
	{
		Camera component = base.GetComponent<Camera>();
		if (component == null)
		{
			return;
		}
		if (component.targetTexture == null)
		{
			internal_SSAA component2 = base.gameObject.GetComponent<internal_SSAA>();
			if (component2 != null)
			{
				Object.Destroy(component2);
			}
		}
		else
		{
			SSAARenderTarget component3 = base.gameObject.GetComponent<SSAARenderTarget>();
			if (component3 != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(component3);
				}
				else
				{
					Object.DestroyImmediate(component3);
				}
			}
		}
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x0014E5AC File Offset: 0x0014C7AC
	public void TakeHighScaledShot(int width, int height, float scale, SSAAFilter filter, string path)
	{
		Texture2D highScaledScreenshot = this.GetHighScaledScreenshot(width, height, scale, filter);
		byte[] array = highScaledScreenshot.EncodeToPNG();
		Object.DestroyImmediate(highScaledScreenshot);
		string text = Application.dataPath + path + ".png";
		if (File.Exists(text))
		{
			int num = 1;
			while (File.Exists(string.Concat(new string[]
			{
				Application.dataPath,
				path,
				" (",
				num.ToString(),
				").png"
			})))
			{
				num++;
				if (num == 2147483647)
				{
					break;
				}
			}
			text = string.Concat(new string[]
			{
				Application.dataPath,
				path,
				" (",
				num.ToString(),
				").png"
			});
		}
		File.WriteAllBytes(text, array);
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x0014E680 File Offset: 0x0014C880
	public Texture2D GetHighScaledScreenshot(int width = 1920, int height = 1080, float scale = 2f, SSAAFilter filter = 1)
	{
		if (!Application.isPlaying)
		{
			Debug.LogWarning("Screenshots only supported in PlayMode");
			return null;
		}
		RenderTexture renderTexture = new RenderTexture(width, height, 24);
		renderTexture.name = "HighScaleShot";
		List<Camera> list = new List<Camera>((Camera[])Object.FindObjectsOfType(typeof(Camera)));
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].enabled || list[i].gameObject.name == "SSAARenderTargetCamera")
			{
				list.RemoveAt(i);
				i--;
			}
		}
		SSAARenderTarget.SampleSSAAForTexture(renderTexture, scale, filter, list);
		Texture2D texture2D = new Texture2D(width, height, 3, false);
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		foreach (Camera camera in list)
		{
			if (camera.targetTexture != null && camera.targetTexture.name == "HighScaleShot")
			{
				camera.targetTexture = null;
			}
		}
		renderTexture.Release();
		return texture2D;
	}

	// Token: 0x04003737 RID: 14135
	public float Scale;

	// Token: 0x04003738 RID: 14136
	public bool unlocked;

	// Token: 0x04003739 RID: 14137
	public SSAAFilter Filter;

	// Token: 0x0400373A RID: 14138
	public RenderTextureFormat renderTextureFormat = 2;

	// Token: 0x0400373B RID: 14139
	public bool showScreenshot;

	// Token: 0x0400373C RID: 14140
	public int screenshotWidth = 1920;

	// Token: 0x0400373D RID: 14141
	public int screenshotHeight = 1080;

	// Token: 0x0400373E RID: 14142
	public float screenshotScale = 2f;

	// Token: 0x0400373F RID: 14143
	public int scalingSelector;

	// Token: 0x04003740 RID: 14144
	public SSAAFilter screenshotFilter;

	// Token: 0x04003741 RID: 14145
	public string relativeScreenshotPath = "/Assets/MyImage";
}
