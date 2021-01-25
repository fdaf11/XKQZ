using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[RequireComponent(typeof(Animation))]
public class ParticalDelay : MonoBehaviour
{
	// Token: 0x06000301 RID: 769 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0002AD44 File Offset: 0x00028F44
	public void InstantiatePartical(int i)
	{
		if (i < 0)
		{
			return;
		}
		if (i >= this.particals.Length)
		{
			return;
		}
		if (this.particals[i])
		{
			Vector3 localPosition = this.particals[i].localPosition;
			Quaternion rotation = this.particals[i].rotation;
			Transform transform = (Transform)Object.Instantiate(this.particals[i], base.gameObject.transform.position + localPosition, base.gameObject.transform.rotation * rotation);
			transform.name = "p" + this.Count;
			transform.parent = base.gameObject.transform;
			this.started = true;
			this.Count++;
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00004302 File Offset: 0x00002502
	private void Update()
	{
		if (this.started && this.autoDestruct && base.transform.childCount == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0002AE18 File Offset: 0x00029018
	public void StopEmit(string name)
	{
		Transform transform = base.gameObject.transform.FindChild(name);
		if (transform)
		{
			ParticleEmitter component = transform.GetComponent<ParticleEmitter>();
			if (component)
			{
				component.emit = false;
			}
		}
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0002AE5C File Offset: 0x0002905C
	public void DestroyPartical(string name)
	{
		Transform transform = base.gameObject.transform.FindChild(name);
		if (transform)
		{
			Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0002AE94 File Offset: 0x00029094
	public void StopAllEmit()
	{
		foreach (object obj in base.gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform)
			{
				ParticleEmitter component = transform.GetComponent<ParticleEmitter>();
				if (component)
				{
					component.emit = false;
				}
			}
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0002AF1C File Offset: 0x0002911C
	public void DestroyAll()
	{
		foreach (object obj in base.gameObject.transform)
		{
			Transform transform = (Transform)obj;
			Object.Destroy(transform.gameObject);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000233 RID: 563
	private bool started;

	// Token: 0x04000234 RID: 564
	public bool autoDestruct = true;

	// Token: 0x04000235 RID: 565
	public Transform[] particals;

	// Token: 0x04000236 RID: 566
	private int Count;
}
