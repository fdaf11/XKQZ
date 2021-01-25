using System;
using UnityEngine;

namespace Exploder.Examples
{
	// Token: 0x020000AA RID: 170
	public class ExplodeAll : MonoBehaviour
	{
		// Token: 0x0600039E RID: 926 RVA: 0x000047CF File Offset: 0x000029CF
		private void Start()
		{
			this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
			this.counter = this.DestroyableObjects.Length;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0002DB60 File Offset: 0x0002BD60
		private void Update()
		{
			this.time += Time.deltaTime;
			if (this.counter != 0)
			{
				foreach (GameObject gameObject in this.DestroyableObjects)
				{
					this.ExplodeObject(gameObject);
					this.counter--;
				}
			}
			if (GameObject.Find("FragmentRoot"))
			{
				Object.Destroy(GameObject.Find("FragmentRoot"), base.transform.GetComponent<ExploderObject>().DeactivateTimeout);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000047EF File Offset: 0x000029EF
		private void ExplodeObject(GameObject gameObject)
		{
			ExploderUtils.SetActive(base.transform.gameObject, true);
			base.transform.GetComponent<ExploderObject>().Explode();
		}

		// Token: 0x040002D4 RID: 724
		public ExploderObject ExploderObjectInstance;

		// Token: 0x040002D5 RID: 725
		private GameObject[] DestroyableObjects;

		// Token: 0x040002D6 RID: 726
		private int counter;

		// Token: 0x040002D7 RID: 727
		private int counterFinished;

		// Token: 0x040002D8 RID: 728
		private float time;
	}
}
