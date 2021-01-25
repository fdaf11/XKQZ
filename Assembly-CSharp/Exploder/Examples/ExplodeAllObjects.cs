using System;
using UnityEngine;

namespace Exploder.Examples
{
	// Token: 0x020000AB RID: 171
	public class ExplodeAllObjects : MonoBehaviour
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00004812 File Offset: 0x00002A12
		private void Start()
		{
			this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0002DBF4 File Offset: 0x0002BDF4
		private void Update()
		{
			if (Input.GetKeyDown(13))
			{
				foreach (GameObject gameObject in this.DestroyableObjects)
				{
					this.ExplodeObject(gameObject);
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0002DC34 File Offset: 0x0002BE34
		private void ExplodeObject(GameObject gameObject)
		{
			ExploderUtils.SetActive(this.ExploderObjectInstance.gameObject, true);
			this.ExploderObjectInstance.transform.position = ExploderUtils.GetCentroid(gameObject);
			this.ExploderObjectInstance.Radius = 1f;
			this.ExploderObjectInstance.Explode();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00004824 File Offset: 0x00002A24
		private void OnGUI()
		{
			GUI.Label(new Rect(200f, 10f, 300f, 30f), "Hit enter to explode everything!");
		}

		// Token: 0x040002D9 RID: 729
		public ExploderObject ExploderObjectInstance;

		// Token: 0x040002DA RID: 730
		private GameObject[] DestroyableObjects;

		// Token: 0x040002DB RID: 731
		private int counter;

		// Token: 0x040002DC RID: 732
		private int counterFinished;
	}
}
