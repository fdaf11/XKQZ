using System;
using UnityEngine;

// Token: 0x02000540 RID: 1344
[RequireComponent(typeof(Collider))]
[AddComponentMenu("Relief Terrain/Helpers/Resolve Hole Collision")]
public class ResolveHoleCollision : MonoBehaviour
{
	// Token: 0x06002213 RID: 8723 RVA: 0x00103A98 File Offset: 0x00101C98
	private void Awake()
	{
		for (int i = 0; i < this.entranceTriggers.Length; i++)
		{
			if (this.entranceTriggers[i] != null)
			{
				this.entranceTriggers[i].isTrigger = true;
			}
		}
		if (base.GetComponent<Rigidbody>() != null && this.StartBelowGroundSurface)
		{
			for (int j = 0; j < this.terrainColliders.Length; j++)
			{
				if (this.terrainColliders[j] != null && base.GetComponent<Collider>() != null)
				{
					Physics.IgnoreCollision(base.GetComponent<Collider>(), this.terrainColliders[j], true);
				}
			}
		}
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x00103B4C File Offset: 0x00101D4C
	private void OnTriggerEnter(Collider other)
	{
		if (base.GetComponent<Collider>() == null)
		{
			return;
		}
		for (int i = 0; i < this.entranceTriggers.Length; i++)
		{
			if (this.entranceTriggers[i] == other)
			{
				for (int j = 0; j < this.terrainColliders.Length; j++)
				{
					Physics.IgnoreCollision(base.GetComponent<Collider>(), this.terrainColliders[j], true);
				}
			}
		}
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x00103BC4 File Offset: 0x00101DC4
	private void FixedUpdate()
	{
		if (this.terrainColliderForUpdate)
		{
			RaycastHit raycastHit = default(RaycastHit);
			if (this.terrainColliderForUpdate.Raycast(new Ray(base.transform.position + Vector3.up * this.checkOffset, Vector3.down), ref raycastHit, float.PositiveInfinity))
			{
				for (int i = 0; i < this.terrainColliders.Length; i++)
				{
					Physics.IgnoreCollision(base.GetComponent<Collider>(), this.terrainColliders[i], false);
				}
			}
			this.terrainColliderForUpdate = null;
		}
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x00103C60 File Offset: 0x00101E60
	private void OnTriggerExit(Collider other)
	{
		if (base.GetComponent<Collider>() == null)
		{
			return;
		}
		for (int i = 0; i < this.entranceTriggers.Length; i++)
		{
			if (this.entranceTriggers[i] == other)
			{
				if (base.GetComponent<Rigidbody>() == null)
				{
					for (int j = 0; j < this.terrainColliders.Length; j++)
					{
						Physics.IgnoreCollision(base.GetComponent<Collider>(), this.terrainColliders[j], false);
					}
				}
				else
				{
					TerrainCollider terrainCollider = null;
					for (int k = 0; k < this.terrainColliders.Length; k++)
					{
						if (this.terrainColliders[k].bounds.min.x <= base.transform.position.x && this.terrainColliders[k].bounds.min.z <= base.transform.position.z && this.terrainColliders[k].bounds.max.x >= base.transform.position.x && this.terrainColliders[k].bounds.max.z >= base.transform.position.z)
						{
							terrainCollider = this.terrainColliders[k];
							break;
						}
					}
					this.terrainColliderForUpdate = terrainCollider;
				}
			}
		}
	}

	// Token: 0x0400266E RID: 9838
	public Collider[] entranceTriggers;

	// Token: 0x0400266F RID: 9839
	public TerrainCollider[] terrainColliders;

	// Token: 0x04002670 RID: 9840
	public float checkOffset = 1f;

	// Token: 0x04002671 RID: 9841
	public bool StartBelowGroundSurface;

	// Token: 0x04002672 RID: 9842
	private TerrainCollider terrainColliderForUpdate;
}
