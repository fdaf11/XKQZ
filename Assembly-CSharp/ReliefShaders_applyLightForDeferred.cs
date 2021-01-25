using System;
using UnityEngine;

// Token: 0x02000534 RID: 1332
[ExecuteInEditMode]
public class ReliefShaders_applyLightForDeferred : MonoBehaviour
{
	// Token: 0x060021F2 RID: 8690 RVA: 0x00016B91 File Offset: 0x00014D91
	private void Reset()
	{
		if (base.GetComponent<Light>())
		{
			this.lightForSelfShadowing = base.GetComponent<Light>();
		}
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x00102304 File Offset: 0x00100504
	private void Update()
	{
		if (this.lightForSelfShadowing)
		{
			if (base.GetComponent<Renderer>())
			{
				if (this.lightForSelfShadowing.type == 1)
				{
					for (int i = 0; i < base.GetComponent<Renderer>().sharedMaterials.Length; i++)
					{
						base.GetComponent<Renderer>().sharedMaterials[i].SetVector("_WorldSpaceLightPosCustom", -this.lightForSelfShadowing.transform.forward);
					}
				}
				else
				{
					for (int j = 0; j < base.GetComponent<Renderer>().materials.Length; j++)
					{
						base.GetComponent<Renderer>().sharedMaterials[j].SetVector("_WorldSpaceLightPosCustom", new Vector4(this.lightForSelfShadowing.transform.position.x, this.lightForSelfShadowing.transform.position.y, this.lightForSelfShadowing.transform.position.z, 1f));
					}
				}
			}
			else if (this.lightForSelfShadowing.type == 1)
			{
				Shader.SetGlobalVector("_WorldSpaceLightPosCustom", -this.lightForSelfShadowing.transform.forward);
			}
			else
			{
				Shader.SetGlobalVector("_WorldSpaceLightPosCustom", new Vector4(this.lightForSelfShadowing.transform.position.x, this.lightForSelfShadowing.transform.position.y, this.lightForSelfShadowing.transform.position.z, 1f));
			}
		}
	}

	// Token: 0x040025F5 RID: 9717
	public Light lightForSelfShadowing;
}
