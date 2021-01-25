using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000011 RID: 17
	public class Shooter : MonoBehaviour
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0002224C File Offset: 0x0002044C
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f));
				GameObject gameObject = GameObject.CreatePrimitive(0);
				Transform transform = gameObject.transform;
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				gameObject.renderer.useLightProbes = true;
				gameObject.AddComponent<ProjectileCollider>();
				transform.position = ray.origin + ray.direction * 1f;
				transform.localScale *= 0.25f;
				rigidbody.AddForce(ray.direction * 1500f);
				gameObject.tag = "Respawn";
			}
		}
	}
}
