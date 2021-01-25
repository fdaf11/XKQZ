using System;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class IceOffsetBehaviour : MonoBehaviour
{
	// Token: 0x060025AF RID: 9647 RVA: 0x001242B4 File Offset: 0x001224B4
	private void Start()
	{
		FadeInOutShaderFloat component = base.GetComponent<FadeInOutShaderFloat>();
		if (component == null)
		{
			return;
		}
		Transform parent = base.transform.parent;
		SkinnedMeshRenderer component2 = parent.GetComponent<SkinnedMeshRenderer>();
		Mesh sharedMesh;
		if (component2 != null)
		{
			sharedMesh = component2.sharedMesh;
		}
		else
		{
			MeshFilter component3 = parent.GetComponent<MeshFilter>();
			if (component3 == null)
			{
				return;
			}
			sharedMesh = component3.sharedMesh;
		}
		if (!sharedMesh.isReadable)
		{
			component.MaxFloat = 0f;
			return;
		}
		int num = sharedMesh.triangles.Length;
		if (num < 1000)
		{
			if (num > 500)
			{
				component.MaxFloat = (float)num / 1000f - 0.5f;
			}
			else
			{
				component.MaxFloat = 0f;
			}
		}
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}
}
