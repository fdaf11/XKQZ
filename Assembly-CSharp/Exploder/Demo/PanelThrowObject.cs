using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x0200009B RID: 155
	public class PanelThrowObject : UseObject
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0002C774 File Offset: 0x0002A974
		public override void Use()
		{
			base.Use();
			GameObject gameObject = this.ThrowObjects[Random.Range(0, this.ThrowObjects.Length)];
			GameObject gameObject2 = Object.Instantiate(gameObject, this.ThrowBox.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject;
			gameObject2.AddComponent<ThrowObject>();
			gameObject2.tag = "Exploder";
			if (gameObject2.rigidbody == null)
			{
				gameObject2.AddComponent<Rigidbody>();
			}
			if (gameObject2.GetComponentsInChildren<BoxCollider>().Length == 0)
			{
				gameObject2.AddComponent<BoxCollider>();
			}
			Vector3 up = Vector3.up;
			up.x = Random.Range(-0.2f, 0.2f);
			up.y = Random.Range(1f, 0.8f);
			up.z = Random.Range(-0.2f, 0.2f);
			up.Normalize();
			gameObject2.rigidbody.velocity = up * 20f;
			gameObject2.rigidbody.angularVelocity = Random.insideUnitSphere * 3f;
			gameObject2.rigidbody.mass = 20f;
		}

		// Token: 0x0400028A RID: 650
		public GameObject ThrowBox;

		// Token: 0x0400028B RID: 651
		public GameObject[] ThrowObjects;
	}
}
