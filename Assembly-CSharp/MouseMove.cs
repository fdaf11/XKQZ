using System;
using UnityEngine;

// Token: 0x0200061C RID: 1564
public class MouseMove : MonoBehaviour
{
	// Token: 0x060026A4 RID: 9892 RVA: 0x00019B79 File Offset: 0x00017D79
	private void Start()
	{
		this._originalPos = base.transform.position;
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x0012C448 File Offset: 0x0012A648
	private void Update()
	{
		Vector3 vector = Input.mousePosition;
		vector.x /= (float)Screen.width;
		vector.y /= (float)Screen.height;
		vector.x -= 0.5f;
		vector.y -= 0.5f;
		vector *= 2f * this._sensitivity;
		base.transform.position = this._originalPos + vector;
	}

	// Token: 0x04002FBA RID: 12218
	[SerializeField]
	private float _sensitivity = 0.5f;

	// Token: 0x04002FBB RID: 12219
	private Vector3 _originalPos;
}
