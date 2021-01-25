using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A9 RID: 169
	public class DemoSimple : MonoBehaviour
	{
		// Token: 0x06000399 RID: 921 RVA: 0x0002D9E8 File Offset: 0x0002BBE8
		private void Start()
		{
			if (this.Exploder.DontUseTag)
			{
				Object[] array = Object.FindObjectsOfType(typeof(Explodable));
				List<GameObject> list = new List<GameObject>(array.Length);
				list.AddRange(Enumerable.Select<Explodable, GameObject>(Enumerable.Where<Explodable>(Enumerable.Cast<Explodable>(array), (Explodable ex) => ex), (Explodable ex) => ex.gameObject));
				this.DestroyableObjects = list.ToArray();
			}
			else
			{
				this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0002DA90 File Offset: 0x0002BC90
		private void OnGUI()
		{
			if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Explode!") && this.Exploder)
			{
				this.Exploder.Explode();
			}
			if (GUI.Button(new Rect(130f, 10f, 100f, 30f), "Reset"))
			{
				ExploderUtils.SetActive(this.Exploder.gameObject, true);
				if (!this.Exploder.DestroyOriginalObject)
				{
					foreach (GameObject obj in this.DestroyableObjects)
					{
						ExploderUtils.SetActiveRecursively(obj, true);
					}
					ExploderUtils.SetActive(this.Exploder.gameObject, true);
				}
			}
		}

		// Token: 0x040002D0 RID: 720
		public ExploderObject Exploder;

		// Token: 0x040002D1 RID: 721
		private GameObject[] DestroyableObjects;
	}
}
