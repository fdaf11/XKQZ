using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildren : MonoBehaviour
{
	// Token: 0x0600303A RID: 12346 RVA: 0x00176460 File Offset: 0x00174660
	private void Start()
	{
		Component[] componentsInChildren = base.GetComponentsInChildren(typeof(MeshFilter));
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshFilter meshFilter = (MeshFilter)componentsInChildren[i];
			Renderer renderer = componentsInChildren[i].renderer;
			MeshCombineUtility.MeshInstance meshInstance = default(MeshCombineUtility.MeshInstance);
			meshInstance.mesh = meshFilter.sharedMesh;
			if (renderer != null && renderer.enabled && meshInstance.mesh != null)
			{
				meshInstance.transform = worldToLocalMatrix * meshFilter.transform.localToWorldMatrix;
				Material[] sharedMaterials = renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					meshInstance.subMeshIndex = Math.Min(j, meshInstance.mesh.subMeshCount - 1);
					ArrayList arrayList = (ArrayList)hashtable[sharedMaterials[j]];
					if (arrayList != null)
					{
						arrayList.Add(meshInstance);
					}
					else
					{
						arrayList = new ArrayList();
						arrayList.Add(meshInstance);
						hashtable.Add(sharedMaterials[j], arrayList);
					}
				}
				renderer.enabled = false;
			}
		}
		foreach (object obj in hashtable)
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
			ArrayList arrayList2 = (ArrayList)dictionaryEntry.Value;
			MeshCombineUtility.MeshInstance[] combines = (MeshCombineUtility.MeshInstance[])arrayList2.ToArray(typeof(MeshCombineUtility.MeshInstance));
			if (hashtable.Count == 1)
			{
				if (base.GetComponent(typeof(MeshFilter)) == null)
				{
					base.gameObject.AddComponent(typeof(MeshFilter));
				}
				if (!base.GetComponent("MeshRenderer"))
				{
					base.gameObject.AddComponent("MeshRenderer");
				}
				MeshFilter meshFilter2 = (MeshFilter)base.GetComponent(typeof(MeshFilter));
				meshFilter2.mesh = MeshCombineUtility.Combine(combines, this.generateTriangleStrips);
				base.renderer.material = (Material)dictionaryEntry.Key;
				base.renderer.enabled = true;
			}
			else
			{
				GameObject gameObject = new GameObject("Combined mesh");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.AddComponent(typeof(MeshFilter));
				gameObject.AddComponent("MeshRenderer");
				gameObject.renderer.material = (Material)dictionaryEntry.Key;
				MeshFilter meshFilter3 = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
				meshFilter3.mesh = MeshCombineUtility.Combine(combines, this.generateTriangleStrips);
			}
		}
	}

	// Token: 0x04003BE7 RID: 15335
	public bool generateTriangleStrips = true;
}
