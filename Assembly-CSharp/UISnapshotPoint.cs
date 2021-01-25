using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Snapshot Point")]
public class UISnapshotPoint : MonoBehaviour
{
	// Token: 0x06001E36 RID: 7734 RVA: 0x000140A7 File Offset: 0x000122A7
	private void Start()
	{
		if (base.tag != "EditorOnly")
		{
			base.tag = "EditorOnly";
		}
	}

	// Token: 0x04002231 RID: 8753
	public bool isOrthographic = true;

	// Token: 0x04002232 RID: 8754
	public float nearClip = -100f;

	// Token: 0x04002233 RID: 8755
	public float farClip = 100f;

	// Token: 0x04002234 RID: 8756
	[Range(10f, 80f)]
	public int fieldOfView = 35;

	// Token: 0x04002235 RID: 8757
	public float orthoSize = 30f;

	// Token: 0x04002236 RID: 8758
	public Texture2D thumbnail;
}
