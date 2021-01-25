﻿using System;
using UnityEngine;

// Token: 0x02000699 RID: 1689
[Serializable]
public class DeluxeEyeAdaptationLogic
{
	// Token: 0x060028F6 RID: 10486 RVA: 0x00145424 File Offset: 0x00143624
	private void RenderHistogram(Material histogramMaterial)
	{
		this.ComputeHistogramCoefs();
		RenderTexture active = this.m_HistogramList[this.m_CurrentHistogram];
		RenderTexture.active = active;
		GL.Clear(true, true, Color.black);
		histogramMaterial.SetFloat("_ValueRange", this.m_Range);
		histogramMaterial.SetFloat("_StepSize", 0.03125f);
		histogramMaterial.SetFloat("_BinCount", 64f);
		histogramMaterial.SetVector("_HistogramCoefs", this.m_HistCoefs);
		if (histogramMaterial.SetPass(0))
		{
			for (int i = 0; i < this.m_Meshes.Length; i++)
			{
				Graphics.DrawMeshNow(this.m_Meshes[i], Matrix4x4.identity);
			}
		}
		RenderTexture renderTexture = this.m_HistogramList[0];
		float num = (float)(this.m_CurrentWidth * this.m_CurrentHeight);
		histogramMaterial.SetFloat("_90PixelCount", num * this.m_AverageThresholdMin);
		histogramMaterial.SetFloat("_98PixelCount", num * this.m_AverageThresholdMax);
		histogramMaterial.SetVector("_Coefs", this.m_HistCoefs);
		histogramMaterial.SetTexture("_HistogramTex", renderTexture);
		histogramMaterial.SetTexture("_PreviousBrightness", this.m_PreviousBrightnessRT);
		histogramMaterial.SetFloat("_ExposureOffset", -1f * this.m_ExposureOffset);
		float deltaTime = Time.deltaTime;
		histogramMaterial.SetVector("_MinMaxSpeedDt", new Vector4(1f / this.m_MaximumExposure, 1f / this.m_MinimumExposure, this.m_AdaptationSpeedDown * Time.deltaTime, this.m_AdaptationSpeedUp * Time.deltaTime));
		renderTexture.filterMode = 0;
		RenderTexture.active = this.m_BrightnessRT;
		GL.Clear(true, true, Color.black);
		Graphics.Blit(renderTexture, this.m_BrightnessRT, histogramMaterial, 1);
		RenderTexture brightnessRT = this.m_BrightnessRT;
		this.m_BrightnessRT = this.m_PreviousBrightnessRT;
		this.m_PreviousBrightnessRT = brightnessRT;
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x001455E8 File Offset: 0x001437E8
	private void ComputeHistogramCoefs()
	{
		this.m_HistLogMax = Mathf.Log(this.m_Range, 2f);
		float num = this.m_HistLogMax - this.m_HistLogMin;
		float num2 = 1f / num;
		float num3 = -(num2 * this.m_HistLogMin);
		this.m_HistCoefs = new Vector4(num2, num3, 0.005f, this.m_Range);
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x00145644 File Offset: 0x00143844
	public void ComputeExposure(int screenWidth, int screenHeight, Material histogramMaterial)
	{
		for (int i = 0; i < 1; i++)
		{
			if (this.m_HistogramList[i] == null)
			{
				this.m_HistogramList[i] = new RenderTexture(64, 1, 0, 11);
				this.m_HistogramList[i].hideFlags = 13;
			}
		}
		if (this.m_BrightnessRT == null)
		{
			this.m_BrightnessRT = new RenderTexture(1, 1, 0, 11);
			this.m_BrightnessRT.hideFlags = 13;
			RenderTexture.active = this.m_BrightnessRT;
			GL.Clear(false, true, Color.white);
		}
		if (this.m_PreviousBrightnessRT == null)
		{
			this.m_PreviousBrightnessRT = new RenderTexture(1, 1, 0, 11);
			this.m_PreviousBrightnessRT.hideFlags = 13;
			RenderTexture.active = this.m_PreviousBrightnessRT;
			GL.Clear(false, true, Color.white);
		}
		this.RebuildMeshIfNeeded(screenWidth, screenHeight);
		this.RenderHistogram(histogramMaterial);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x00145734 File Offset: 0x00143934
	public void RebuildMeshIfNeeded(int width, int height)
	{
		if (this.m_CurrentWidth == width && this.m_CurrentHeight == height && this.m_Meshes != null)
		{
			return;
		}
		if (this.m_Meshes != null)
		{
			foreach (Mesh mesh in this.m_Meshes)
			{
				Object.DestroyImmediate(mesh, true);
			}
		}
		this.m_Meshes = null;
		this.BuildMeshes(width, height);
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x001457A8 File Offset: 0x001439A8
	public void BuildMeshes(int width, int height)
	{
		int num = 21666;
		int num2 = width * height;
		int num3 = Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num));
		this.m_Meshes = new Mesh[num3];
		int num4 = num2;
		this.m_CurrentWidth = width;
		this.m_CurrentHeight = height;
		int num5 = 0;
		float num6 = 0.03125f;
		float num7 = 2f;
		Vector2 vector;
		vector..ctor(1f / (float)this.m_CurrentWidth * 0.5f, 1f / (float)this.m_CurrentHeight * 0.5f);
		for (int i = 0; i < num3; i++)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = 13;
			int num8 = num4;
			if (num4 > num)
			{
				num8 = num;
			}
			num4 -= num8;
			Vector3[] vertices = new Vector3[num8 * 3];
			int[] triangles = new int[num8 * 3];
			Vector2[] array = new Vector2[num8 * 3];
			Vector2[] uv = new Vector2[num8 * 3];
			Vector3[] normals = new Vector3[num8 * 3];
			Color[] colors = new Color[num8 * 3];
			for (int j = 0; j < num8; j++)
			{
				int num9 = num5 % width;
				int num10 = (num5 - num9) / width;
				this.SetupSprite(j, num9, num10, vertices, triangles, array, uv, normals, colors, new Vector2((float)num9 / (float)width + vector.x, 1f - ((float)num10 / (float)height + vector.y)), num6 * 0.5f, num7 * 0.5f);
				num5++;
			}
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.colors = null;
			mesh.uv = array;
			mesh.uv2 = null;
			mesh.normals = normals;
			mesh.RecalculateBounds();
			mesh.UploadMeshData(true);
			this.m_Meshes[i] = mesh;
		}
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x00145974 File Offset: 0x00143B74
	public void SetupSprite(int idx, int x, int y, Vector3[] vertices, int[] triangles, Vector2[] uv0, Vector2[] uv1, Vector3[] normals, Color[] colors, Vector2 targetPixelUV, float halfWidth, float halfHeight)
	{
		int num = idx * 3;
		int num2 = idx * 3;
		triangles[num2] = num;
		triangles[num2 + 1] = num + 2;
		triangles[num2 + 2] = num + 1;
		float num3 = 0f;
		vertices[num] = new Vector3(-1f + num3, -1f, 0f);
		vertices[num + 2] = new Vector3(-1f + num3, 1f, 0f);
		vertices[num + 1] = new Vector3(-0.953125f + num3, 1f, 0f);
		normals[num] = -Vector3.forward;
		normals[num + 1] = -Vector3.forward;
		normals[num + 2] = -Vector3.forward;
		uv0[num] = targetPixelUV;
		uv0[num + 1] = targetPixelUV;
		uv0[num + 2] = targetPixelUV;
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x00145A90 File Offset: 0x00143C90
	public void ClearMeshes()
	{
		if (this.m_Meshes != null)
		{
			foreach (Mesh mesh in this.m_Meshes)
			{
				Object.DestroyImmediate(mesh, true);
			}
		}
		this.m_Meshes = null;
		for (int j = 0; j < 1; j++)
		{
			if (this.m_HistogramList[j] == null)
			{
				Object.DestroyImmediate(this.m_HistogramList[j]);
				this.m_HistogramList[j] = null;
			}
		}
		if (this.m_BrightnessRT != null)
		{
			Object.DestroyImmediate(this.m_BrightnessRT);
			this.m_BrightnessRT = null;
		}
		if (this.m_PreviousBrightnessRT != null)
		{
			Object.DestroyImmediate(this.m_PreviousBrightnessRT);
			this.m_PreviousBrightnessRT = null;
		}
		if (this.m_LocalHistogram != null)
		{
			Object.DestroyImmediate(this.m_LocalHistogram);
			this.m_LocalHistogram = null;
		}
		if (this.m_LocalBrightness != null)
		{
			Object.DestroyImmediate(this.m_LocalBrightness);
			this.m_LocalBrightness = null;
		}
	}

	// Token: 0x040033CE RID: 13262
	private const float m_HistMin = 0.005f;

	// Token: 0x040033CF RID: 13263
	private const int m_BinCount = 64;

	// Token: 0x040033D0 RID: 13264
	private const float m_BinTexStep = 0.03125f;

	// Token: 0x040033D1 RID: 13265
	private const float m_BinTexStart = -0.984375f;

	// Token: 0x040033D2 RID: 13266
	private const int HIST_COUNT = 1;

	// Token: 0x040033D3 RID: 13267
	private const int m_Height = 1;

	// Token: 0x040033D4 RID: 13268
	[SerializeField]
	public float m_MinimumExposure = 0.4f;

	// Token: 0x040033D5 RID: 13269
	[SerializeField]
	public float m_MaximumExposure = 8f;

	// Token: 0x040033D6 RID: 13270
	[SerializeField]
	public float m_Range = 4f;

	// Token: 0x040033D7 RID: 13271
	[SerializeField]
	public float m_AdaptationSpeedUp = 1f;

	// Token: 0x040033D8 RID: 13272
	[SerializeField]
	public float m_AdaptationSpeedDown = 1f;

	// Token: 0x040033D9 RID: 13273
	[SerializeField]
	public float m_BrightnessMultiplier = 1f;

	// Token: 0x040033DA RID: 13274
	[SerializeField]
	public float m_ExposureOffset;

	// Token: 0x040033DB RID: 13275
	[SerializeField]
	public float m_AverageThresholdMin = 0.8f;

	// Token: 0x040033DC RID: 13276
	[SerializeField]
	public float m_AverageThresholdMax = 0.98f;

	// Token: 0x040033DD RID: 13277
	[SerializeField]
	private float m_HistLogMin = Mathf.Log(0.005f, 2f);

	// Token: 0x040033DE RID: 13278
	[SerializeField]
	private float m_HistLogMax = Mathf.Log(5f, 2f);

	// Token: 0x040033DF RID: 13279
	[SerializeField]
	public Vector4 m_HistCoefs = default(Vector4);

	// Token: 0x040033E0 RID: 13280
	private Mesh[] m_Meshes;

	// Token: 0x040033E1 RID: 13281
	public int m_CurrentWidth;

	// Token: 0x040033E2 RID: 13282
	public int m_CurrentHeight;

	// Token: 0x040033E3 RID: 13283
	private int m_CurrentHistogram;

	// Token: 0x040033E4 RID: 13284
	public RenderTexture[] m_HistogramList = new RenderTexture[1];

	// Token: 0x040033E5 RID: 13285
	public RenderTexture m_BrightnessRT;

	// Token: 0x040033E6 RID: 13286
	private RenderTexture m_PreviousBrightnessRT;

	// Token: 0x040033E7 RID: 13287
	private Texture2D m_LocalHistogram;

	// Token: 0x040033E8 RID: 13288
	private Texture2D m_LocalBrightness;

	// Token: 0x040033E9 RID: 13289
	public bool m_SetTargetExposure;
}
