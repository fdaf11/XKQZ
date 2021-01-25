using System;
using UnityEngine;

// Token: 0x020005FE RID: 1534
[RequireComponent(typeof(Camera))]
[AddComponentMenu("V-Lights/VLight Image Effects")]
[ExecuteInEditMode]
public class VLightInterleavedSampling : MonoBehaviour
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060025EE RID: 9710 RVA: 0x0001946A File Offset: 0x0001766A
	private Material PostMaterial
	{
		get
		{
			if (this._postMaterial == null)
			{
				this._postMaterial = new Material(this.postEffectShader);
				this._postMaterial.hideFlags = 13;
			}
			return this._postMaterial;
		}
	}

	// Token: 0x060025EF RID: 9711 RVA: 0x000194A1 File Offset: 0x000176A1
	private void OnEnable()
	{
		this._vLights = null;
		this.Init();
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x001252A0 File Offset: 0x001234A0
	private void OnDisable()
	{
		if (this._vLights != null)
		{
			foreach (VLight vlight in this._vLights)
			{
				if (!(vlight == null))
				{
					vlight.lockTransforms = false;
				}
			}
		}
		this._vLights = null;
		this.CleanUp();
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x001252FC File Offset: 0x001234FC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this._vLights == null)
		{
			this._vLights = (Object.FindObjectsOfType(typeof(VLight)) as VLight[]);
		}
		int num = Mathf.Clamp(this.downSample, 1, 20);
		this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 20);
		int num2 = (int)base.camera.pixelWidth;
		int num3 = (int)base.camera.pixelHeight;
		int num4 = (int)base.camera.pixelWidth / num;
		int num5 = (int)base.camera.pixelHeight / num;
		RenderTexture temporary = RenderTexture.GetTemporary(num4, num5, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(num4, num5, 0);
		RenderTexture temporary3 = RenderTexture.GetTemporary(num4, num5, 0);
		RenderTexture temporary4 = RenderTexture.GetTemporary(num4, num5, 0);
		if (this.interleavedBuffer != null && (this.interleavedBuffer.width != num2 || this.interleavedBuffer.height != num3))
		{
			if (Application.isPlaying)
			{
				Object.Destroy(this.interleavedBuffer);
			}
			else
			{
				Object.DestroyImmediate(this.interleavedBuffer);
			}
			this.interleavedBuffer = null;
		}
		if (this.interleavedBuffer == null)
		{
			this.interleavedBuffer = new RenderTexture(num2, num3, 1);
		}
		Camera ppcamera = this.GetPPCamera();
		ppcamera.CopyFrom(base.camera);
		ppcamera.enabled = false;
		ppcamera.depthTextureMode = 0;
		ppcamera.clearFlags = 2;
		ppcamera.cullingMask = this._volumeLightLayer;
		ppcamera.backgroundColor = Color.clear;
		ppcamera.renderingPath = 0;
		foreach (VLight vlight in this._vLights)
		{
			if (!(vlight == null))
			{
				vlight.lockTransforms = true;
			}
		}
		if (this.useInterleavedSampling)
		{
			ppcamera.projectionMatrix = base.camera.projectionMatrix;
			ppcamera.pixelRect = new Rect(0f, 0f, base.camera.pixelWidth / base.camera.rect.width + (float)Screen.width / base.camera.rect.width, base.camera.pixelHeight / base.camera.rect.height + (float)Screen.height / base.camera.rect.height);
			float num6 = 0f;
			this.RenderSample(num6, ppcamera, temporary);
			num6 += this.ditherOffset;
			this.RenderSample(num6, ppcamera, temporary2);
			num6 += this.ditherOffset;
			this.RenderSample(num6, ppcamera, temporary3);
			num6 += this.ditherOffset;
			this.RenderSample(num6, ppcamera, temporary4);
			this.PostMaterial.SetTexture("_MainTexA", temporary);
			this.PostMaterial.SetTexture("_MainTexB", temporary2);
			this.PostMaterial.SetTexture("_MainTexC", temporary3);
			this.PostMaterial.SetTexture("_MainTexD", temporary4);
			Graphics.Blit(null, this.interleavedBuffer, this.PostMaterial, 0);
		}
		else
		{
			ppcamera.projectionMatrix = base.camera.projectionMatrix;
			ppcamera.pixelRect = new Rect(0f, 0f, base.camera.pixelWidth / base.camera.rect.width + (float)Screen.width / base.camera.rect.width, base.camera.pixelHeight / base.camera.rect.height + (float)Screen.height / base.camera.rect.height);
			this.RenderSample(0f, ppcamera, temporary);
			Graphics.Blit(temporary, this.interleavedBuffer);
		}
		foreach (VLight vlight2 in this._vLights)
		{
			if (!(vlight2 == null))
			{
				vlight2.lockTransforms = false;
			}
		}
		RenderTexture temporary5 = RenderTexture.GetTemporary(num2, num3, 0);
		this.PostMaterial.SetFloat("_BlurSize", this.blurRadius);
		for (int k = 0; k < this.blurIterations; k++)
		{
			Graphics.Blit(this.interleavedBuffer, temporary5, this.PostMaterial, 1);
			Graphics.Blit(temporary5, this.interleavedBuffer, this.PostMaterial, 2);
		}
		RenderTexture.ReleaseTemporary(temporary5);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary3);
		RenderTexture.ReleaseTemporary(temporary4);
		this.PostMaterial.SetTexture("_MainTexBlurred", this.interleavedBuffer);
		Graphics.Blit(source, destination, this.PostMaterial, 3);
	}

	// Token: 0x060025F2 RID: 9714 RVA: 0x000194B0 File Offset: 0x000176B0
	private void RenderSample(float offset, Camera ppCamera, RenderTexture buffer)
	{
		Shader.SetGlobalFloat("_InterleavedOffset", offset);
		ppCamera.targetTexture = buffer;
		ppCamera.RenderWithShader(this.volumeLightShader, "RenderType");
	}

	// Token: 0x060025F3 RID: 9715 RVA: 0x001257D4 File Offset: 0x001239D4
	private void Init()
	{
		if (LayerMask.NameToLayer("vlight") == -1)
		{
			Debug.LogWarning("vlight layer does not exist! Cannot use interleaved sampling please add this layer.");
			return;
		}
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning("Cannot use interleaved sampling. Image effects not supported");
			return;
		}
		this._volumeLightLayer = 1 << LayerMask.NameToLayer("vlight");
		base.camera.cullingMask &= ~this._volumeLightLayer;
		base.camera.depthTextureMode |= 2;
		if (this.postEffectShader == null)
		{
			this.postEffectShader = Shader.Find("Hidden/V-Light/Post");
		}
		if (this.volumeLightShader == null)
		{
			this.volumeLightShader = Shader.Find("V-Light/Volumetric Light Depth");
		}
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x001258A0 File Offset: 0x00123AA0
	private void CleanUp()
	{
		base.camera.cullingMask |= this._volumeLightLayer;
		if (Application.isEditor)
		{
			Object.DestroyImmediate(this._postMaterial);
			if (this.interleavedBuffer != null)
			{
				Object.DestroyImmediate(this.interleavedBuffer);
			}
		}
		else
		{
			Object.Destroy(this._postMaterial);
			if (this.interleavedBuffer != null)
			{
				Object.Destroy(this.interleavedBuffer);
			}
		}
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x00125928 File Offset: 0x00123B28
	private Camera GetPPCamera()
	{
		if (this._ppCameraGO == null)
		{
			GameObject gameObject = GameObject.Find("Post Processing Camera");
			if (gameObject != null && gameObject.camera != null)
			{
				this._ppCameraGO = gameObject;
			}
			else
			{
				this._ppCameraGO = new GameObject("Post Processing Camera", new Type[]
				{
					typeof(Camera)
				});
				this._ppCameraGO.camera.enabled = false;
				this._ppCameraGO.hideFlags = 13;
			}
		}
		return this._ppCameraGO.camera;
	}

	// Token: 0x04002E82 RID: 11906
	[SerializeField]
	private bool useInterleavedSampling = true;

	// Token: 0x04002E83 RID: 11907
	[SerializeField]
	private float ditherOffset = 0.02f;

	// Token: 0x04002E84 RID: 11908
	[SerializeField]
	private float blurRadius = 1.5f;

	// Token: 0x04002E85 RID: 11909
	[SerializeField]
	private int blurIterations = 1;

	// Token: 0x04002E86 RID: 11910
	[SerializeField]
	private int downSample = 4;

	// Token: 0x04002E87 RID: 11911
	[SerializeField]
	private Shader postEffectShader;

	// Token: 0x04002E88 RID: 11912
	[SerializeField]
	private Shader volumeLightShader;

	// Token: 0x04002E89 RID: 11913
	private GameObject _ppCameraGO;

	// Token: 0x04002E8A RID: 11914
	private LayerMask _volumeLightLayer;

	// Token: 0x04002E8B RID: 11915
	private VLight[] _vLights;

	// Token: 0x04002E8C RID: 11916
	private Material _postMaterial;

	// Token: 0x04002E8D RID: 11917
	private RenderTexture interleavedBuffer;
}
