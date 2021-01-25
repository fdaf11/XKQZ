using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A7 RID: 167
	public class DemoClickExplode2D : MonoBehaviour
	{
		// Token: 0x06000388 RID: 904 RVA: 0x0002D724 File Offset: 0x0002B924
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

		// Token: 0x06000389 RID: 905 RVA: 0x00004774 File Offset: 0x00002974
		private bool IsExplodable(GameObject obj)
		{
			if (this.Exploder.DontUseTag)
			{
				return obj.GetComponent<Explodable>() != null;
			}
			return obj.CompareTag(ExploderObject.Tag);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(this.Camera.ScreenPointToRay(Input.mousePosition));
				if (rayIntersection)
				{
					GameObject gameObject = rayIntersection.collider.gameObject;
					if (this.IsExplodable(gameObject))
					{
						if (Input.GetMouseButtonDown(0))
						{
							this.ExplodeObject(gameObject);
						}
						else
						{
							this.ExplodeAfterCrack();
						}
					}
				}
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0002D848 File Offset: 0x0002BA48
		private void ExplodeObject(GameObject obj)
		{
			ExploderUtils.SetActive(this.Exploder.gameObject, true);
			this.Exploder.transform.position = ExploderUtils.GetCentroid(obj);
			this.Exploder.Radius = 0.1f;
			this.Exploder.Explode(new ExploderObject.OnExplosion(this.OnExplosion));
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000475B File Offset: 0x0000295B
		private void OnExplosion(float time, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionFinished)
			{
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnCracked()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000264F File Offset: 0x0000084F
		private void ExplodeAfterCrack()
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0002D8A4 File Offset: 0x0002BAA4
		private void OnGUI()
		{
			if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Reset") && !this.Exploder.DestroyOriginalObject)
			{
				foreach (GameObject obj in this.DestroyableObjects)
				{
					ExploderUtils.SetActiveRecursively(obj, true);
				}
				ExploderUtils.SetActive(this.Exploder.gameObject, true);
			}
		}

		// Token: 0x040002CA RID: 714
		public ExploderObject Exploder;

		// Token: 0x040002CB RID: 715
		private GameObject[] DestroyableObjects;

		// Token: 0x040002CC RID: 716
		public Camera Camera;
	}
}
