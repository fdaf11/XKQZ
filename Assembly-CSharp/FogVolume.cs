using System;
using UnityEngine;

// Token: 0x0200061B RID: 1563
[ExecuteInEditMode]
internal class FogVolume : MonoBehaviour
{
	// Token: 0x060026A0 RID: 9888 RVA: 0x0012C1C8 File Offset: 0x0012A3C8
	private void OnEnable()
	{
		if (!this.FogMaterial)
		{
			this.FogMaterial = new Material(Shader.Find("Hidden/FogVolume"));
		}
		this.VolumeObj = base.gameObject;
		this.VolumeObj.renderer.sharedMaterial = this.FogMaterial;
		Camera.main.depthTextureMode |= 1;
	}

	// Token: 0x060026A1 RID: 9889 RVA: 0x0012C230 File Offset: 0x0012A430
	private void Update()
	{
		FogVolume.ToggleKeyword(this.EnableInscattering, "FOG_VOLUME_INSCATTERING_ON", "FOG_VOLUME_INSCATTERING_OFF");
		this.FogMaterial.SetColor("_Color", this.FogColor);
		this.FogMaterial.SetColor("_InscatteringColor", this.InscatteringColor);
		if (this.Sun)
		{
			this.InscatteringIntensity = Mathf.Max(0f, this.InscatteringIntensity);
			this.FogMaterial.SetFloat("_InscatteringIntensity", this.InscatteringIntensity);
			this.FogMaterial.SetVector("L", -this.Sun.transform.forward);
		}
		this.InscateringExponent = Mathf.Max(1f, this.InscateringExponent);
		this.FogMaterial.SetFloat("_InscateringExponent", this.InscateringExponent);
		this.InscatteringTransitionWideness = Mathf.Max(1f, this.InscatteringTransitionWideness);
		this.FogMaterial.SetFloat("InscatteringTransitionWideness", this.InscatteringTransitionWideness);
		this.InscatteringStartDistance = Mathf.Max(0f, this.InscatteringStartDistance);
		this.FogMaterial.SetFloat("InscatteringStartDistance", this.InscatteringStartDistance);
		this.VolumeSize = this.VolumeObj.transform.lossyScale;
		base.transform.localScale = new Vector3((float)decimal.Round((decimal)this.VolumeSize.x, 2), this.VolumeSize.y, this.VolumeSize.z);
		this.FogMaterial.SetVector("_BoxMin", this.VolumeSize * -0.5051f);
		this.FogMaterial.SetVector("_BoxMax", this.VolumeSize * 0.5051f);
		this.Visibility = Mathf.Max(0f, this.Visibility);
		this.FogMaterial.SetFloat("_Visibility", this.Visibility);
		base.renderer.sortingOrder = this.DrawOrder;
	}

	// Token: 0x060026A2 RID: 9890 RVA: 0x00019B40 File Offset: 0x00017D40
	private static void ToggleKeyword(bool toggle, string keywordON, string keywordOFF)
	{
		Shader.DisableKeyword((!toggle) ? keywordON : keywordOFF);
		Shader.EnableKeyword((!toggle) ? keywordOFF : keywordON);
	}

	// Token: 0x04002FAC RID: 12204
	private GameObject VolumeObj;

	// Token: 0x04002FAD RID: 12205
	private Vector3 VolumeSize;

	// Token: 0x04002FAE RID: 12206
	private Material FogMaterial;

	// Token: 0x04002FAF RID: 12207
	[SerializeField]
	private Color InscatteringColor = Color.white;

	// Token: 0x04002FB0 RID: 12208
	[SerializeField]
	private Color FogColor = new Color(0.5f, 0.6f, 0.7f, 1f);

	// Token: 0x04002FB1 RID: 12209
	[SerializeField]
	private float Visibility = 5f;

	// Token: 0x04002FB2 RID: 12210
	[SerializeField]
	private float InscateringExponent = 2f;

	// Token: 0x04002FB3 RID: 12211
	[SerializeField]
	private float InscatteringIntensity = 2f;

	// Token: 0x04002FB4 RID: 12212
	[SerializeField]
	private float InscatteringTransitionWideness = 1f;

	// Token: 0x04002FB5 RID: 12213
	[SerializeField]
	private float InscatteringStartDistance;

	// Token: 0x04002FB6 RID: 12214
	[SerializeField]
	private Light Sun;

	// Token: 0x04002FB7 RID: 12215
	[SerializeField]
	private int DrawOrder;

	// Token: 0x04002FB8 RID: 12216
	[SerializeField]
	private bool HideWireframe = true;

	// Token: 0x04002FB9 RID: 12217
	[SerializeField]
	private bool EnableInscattering;
}
