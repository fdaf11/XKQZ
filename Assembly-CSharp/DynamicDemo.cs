using System;
using ch.sycoforge.Decal;
using UnityEngine;

// Token: 0x0200066D RID: 1645
public class DynamicDemo : MonoBehaviour
{
	// Token: 0x06002846 RID: 10310 RVA: 0x0001A81D File Offset: 0x00018A1D
	public void Start()
	{
		if (this.DecalPrefab == null)
		{
			Debug.LogError("The DynamicDemo script has no decal prefab attached.");
		}
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x0013EF74 File Offset: 0x0013D174
	public void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit, 200f))
			{
				Debug.DrawLine(ray.origin, raycastHit.point, Color.red);
				EasyDecal.ProjectAt(this.DecalPrefab.gameObject, raycastHit.collider.gameObject, raycastHit.point, raycastHit.normal);
			}
		}
	}

	// Token: 0x04003289 RID: 12937
	public EasyDecal DecalPrefab;
}
