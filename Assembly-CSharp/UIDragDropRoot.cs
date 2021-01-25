using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	// Token: 0x06001AA3 RID: 6819 RVA: 0x000118DC File Offset: 0x0000FADC
	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000118E9 File Offset: 0x0000FAE9
	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	// Token: 0x04001F63 RID: 8035
	public static Transform root;
}
