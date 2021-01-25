using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A6 RID: 166
	public class DemoClickExplode : MonoBehaviour
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0002D500 File Offset: 0x0002B700
		private void Start()
		{
			Application.targetFrameRate = 60;
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

		// Token: 0x0600037E RID: 894 RVA: 0x00004731 File Offset: 0x00002931
		private bool IsExplodable(GameObject obj)
		{
			if (this.Exploder.DontUseTag)
			{
				return obj.GetComponent<Explodable>() != null;
			}
			return obj.CompareTag(ExploderObject.Tag);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0002D5B0 File Offset: 0x0002B7B0
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				Ray ray;
				if (this.Camera)
				{
					ray = this.Camera.ScreenPointToRay(Input.mousePosition);
				}
				else
				{
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				}
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit))
				{
					GameObject gameObject = raycastHit.collider.gameObject;
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

		// Token: 0x06000380 RID: 896 RVA: 0x0002D64C File Offset: 0x0002B84C
		private void ExplodeObject(GameObject obj)
		{
			ExploderUtils.SetActive(this.Exploder.gameObject, true);
			this.Exploder.transform.position = ExploderUtils.GetCentroid(obj);
			this.Exploder.Radius = 1f;
			this.Exploder.Explode(new ExploderObject.OnExplosion(this.OnExplosion));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000475B File Offset: 0x0000295B
		private void OnExplosion(float time, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionFinished)
			{
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnCracked()
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000264F File Offset: 0x0000084F
		private void ExplodeAfterCrack()
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
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

		// Token: 0x040002C5 RID: 709
		public ExploderObject Exploder;

		// Token: 0x040002C6 RID: 710
		private GameObject[] DestroyableObjects;

		// Token: 0x040002C7 RID: 711
		public Camera Camera;
	}
}
