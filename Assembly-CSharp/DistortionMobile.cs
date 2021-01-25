using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class DistortionMobile : MonoBehaviour
{
	// Token: 0x060025D5 RID: 9685 RVA: 0x0001930F File Offset: 0x0001750F
	private void Start()
	{
		base.Invoke("Initialize", 0.1f);
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x00124C5C File Offset: 0x00122E5C
	private void Initialize()
	{
		GameObject gameObject = new GameObject("RenderTextureCamera");
		Camera camera = gameObject.AddComponent<Camera>();
		Camera main = Camera.main;
		camera.CopyFrom(main);
		camera.depth += 1f;
		camera.cullingMask = this.CullingMask;
		gameObject.transform.parent = main.transform;
		this.renderTexture = new RenderTexture((int)((float)Screen.width * this.Scale), (int)((float)Screen.height * this.Scale), 32, this.RenderTextureFormat);
		this.renderTexture.DiscardContents();
		this.renderTexture.filterMode = 0;
		camera.targetTexture = this.renderTexture;
		Shader.SetGlobalTexture("_GrabTextureMobile", this.renderTexture);
	}

	// Token: 0x04002E5E RID: 11870
	public float Scale = 1f;

	// Token: 0x04002E5F RID: 11871
	public RenderTextureFormat RenderTextureFormat;

	// Token: 0x04002E60 RID: 11872
	public FilterMode FilterMode;

	// Token: 0x04002E61 RID: 11873
	public LayerMask CullingMask;

	// Token: 0x04002E62 RID: 11874
	private RenderTexture renderTexture;
}
