using System;
using UnityEngine;

// Token: 0x02000541 RID: 1345
[ExecuteInEditMode]
[AddComponentMenu("Relief Terrain/Helpers/Sync Caustics Water Level")]
public class SyncCausticsWaterLevel : MonoBehaviour
{
	// Token: 0x06002218 RID: 8728 RVA: 0x00103E00 File Offset: 0x00102000
	private void Update()
	{
		if (this.refGameObject && this.refGameObject.GetComponent<Renderer>())
		{
			this.refGameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("TERRAIN_CausticsWaterLevel", base.transform.position.y + this.yOffset);
		}
		else
		{
			Shader.SetGlobalFloat("TERRAIN_CausticsWaterLevel", base.transform.position.y + this.yOffset);
		}
	}

	// Token: 0x04002673 RID: 9843
	public GameObject refGameObject;

	// Token: 0x04002674 RID: 9844
	public float yOffset;
}
