using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000090 RID: 144
	public class ReSetUI : MonoBehaviour
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0002BE24 File Offset: 0x0002A024
		private void Start()
		{
			GameObject gameObject = GameObject.FindWithTag("UIRoot");
			if (gameObject != null)
			{
				Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
				foreach (Transform transform in componentsInChildren)
				{
					if (transform.name.Equals("Camera") && Camera.main != null)
					{
						transform.camera.depth = Camera.main.depth + 1f;
					}
				}
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}
	}
}
