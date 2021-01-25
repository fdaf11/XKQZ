using System;
using UnityEngine;

// Token: 0x02000529 RID: 1321
[ExecuteInEditMode]
[AddComponentMenu("Relief Terrain/Helpers/Use baked gloss texture")]
public class GlossBakedTextureReplacement : MonoBehaviour
{
	// Token: 0x060021CB RID: 8651 RVA: 0x000FFF58 File Offset: 0x000FE158
	public GlossBakedTextureReplacement()
	{
		this.bakedTexture = (this.originalTexture = null);
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x00016A61 File Offset: 0x00014C61
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x00016A69 File Offset: 0x00014C69
	private void Update()
	{
		if (!Application.isPlaying)
		{
			this.Refresh();
			if (this.resetGlossMultAndShaping)
			{
				this.resetGlossMultAndShaping = false;
				this.resetGlossMultAndShapingFun();
			}
		}
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x000FFF84 File Offset: 0x000FE184
	public void resetGlossMultAndShapingFun()
	{
		if (this.glossBakedData == null)
		{
			return;
		}
		Material material;
		if (this.CustomMaterial != null)
		{
			material = this.CustomMaterial;
		}
		else
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			material = base.GetComponent<Renderer>().sharedMaterial;
		}
		if (!material)
		{
			return;
		}
		if (this.RTPStandAloneShader)
		{
			Vector4 vector;
			vector..ctor(1f, 1f, 1f, 1f);
			Vector4 vector2;
			vector2..ctor(0.5f, 0.5f, 0.5f, 0.5f);
			if (material.HasProperty("RTP_gloss_mult0123"))
			{
				vector = material.GetVector("RTP_gloss_mult0123");
				if (this.layerNumber >= 1 && this.layerNumber <= 4)
				{
					vector[this.layerNumber - 1] = 1f;
				}
				material.SetVector("RTP_gloss_mult0123", vector);
			}
			if (material.HasProperty("RTP_gloss_shaping0123"))
			{
				vector2 = material.GetVector("RTP_gloss_shaping0123");
				if (this.layerNumber >= 1 && this.layerNumber <= 4)
				{
					vector2[this.layerNumber - 1] = 0.5f;
				}
				material.SetVector("RTP_gloss_shaping0123", vector2);
			}
		}
		else
		{
			string text = "RTP_gloss_mult0";
			string text2 = "RTP_gloss_shaping0";
			if (this.layerNumber == 2)
			{
				text = "RTP_gloss_mult1";
				text2 = "RTP_gloss_shaping1";
			}
			if (material.HasProperty(text))
			{
				material.SetFloat(text, 1f);
			}
			if (material.HasProperty(text2))
			{
				material.SetFloat(text2, 0.5f);
			}
		}
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x00100130 File Offset: 0x000FE330
	public void Refresh()
	{
		if (this.glossBakedData == null)
		{
			return;
		}
		string text = "_MainTex";
		if (this.RTPStandAloneShader)
		{
			text = "_SplatA0";
			if (this.layerNumber == 2)
			{
				text = "_SplatA1";
			}
			else if (this.layerNumber == 3)
			{
				text = "_SplatA2";
			}
			else if (this.layerNumber == 4)
			{
				text = "_SplatA3";
			}
		}
		else if (this.layerNumber == 2)
		{
			text = "_MainTex2";
		}
		Material material;
		if (this.CustomMaterial != null)
		{
			material = this.CustomMaterial;
		}
		else
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			material = base.GetComponent<Renderer>().sharedMaterial;
		}
		if (!material)
		{
			return;
		}
		if (material.HasProperty(text))
		{
			if (this.bakedTexture)
			{
				material.SetTexture(text, this.bakedTexture);
			}
			else
			{
				if (this.originalTexture == null)
				{
					this.originalTexture = (Texture2D)material.GetTexture(text);
				}
				if (this.originalTexture != null && this.glossBakedData != null && !this.glossBakedData.used_in_atlas && this.glossBakedData.CheckSize(this.originalTexture))
				{
					this.bakedTexture = this.glossBakedData.MakeTexture(this.originalTexture);
					if (this.bakedTexture)
					{
						material.SetTexture(text, this.bakedTexture);
					}
				}
			}
		}
	}

	// Token: 0x04002530 RID: 9520
	public RTPGlossBaked glossBakedData;

	// Token: 0x04002531 RID: 9521
	public bool RTPStandAloneShader;

	// Token: 0x04002532 RID: 9522
	public int layerNumber = 1;

	// Token: 0x04002533 RID: 9523
	public Material CustomMaterial;

	// Token: 0x04002534 RID: 9524
	public Texture2D originalTexture;

	// Token: 0x04002535 RID: 9525
	public bool resetGlossMultAndShaping;

	// Token: 0x04002536 RID: 9526
	[NonSerialized]
	public Texture2D bakedTexture;
}
