using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class AddMaterialOnHit : MonoBehaviour
{
	// Token: 0x060024D7 RID: 9431 RVA: 0x0011FA40 File Offset: 0x0011DC40
	private void Update()
	{
		if (this.EffectSettings == null)
		{
			return;
		}
		if (this.EffectSettings.IsVisible)
		{
			this.timeToDelete = 0f;
		}
		else
		{
			this.timeToDelete += Time.deltaTime;
			if (this.timeToDelete > this.RemoveAfterTime)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x0011FAB0 File Offset: 0x0011DCB0
	public void UpdateMaterial(RaycastHit hit)
	{
		Transform transform = hit.transform;
		if (transform != null)
		{
			if (!this.RemoveWhenDisable)
			{
				Object.Destroy(base.gameObject, this.RemoveAfterTime);
			}
			this.fadeInOutShaderColor = base.GetComponents<FadeInOutShaderColor>();
			this.fadeInOutShaderFloat = base.GetComponents<FadeInOutShaderFloat>();
			this.uvTextureAnimator = base.GetComponent<UVTextureAnimator>();
			this.renderParent = base.transform.parent.GetComponent<Renderer>();
			Material[] sharedMaterials = this.renderParent.sharedMaterials;
			int num = sharedMaterials.Length + 1;
			Material[] array = new Material[num];
			sharedMaterials.CopyTo(array, 0);
			this.renderParent.material = this.Material;
			this.instanceMat = this.renderParent.material;
			array[num - 1] = this.instanceMat;
			this.renderParent.sharedMaterials = array;
			if (this.UsePointMatrixTransform)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(hit.transform.InverseTransformPoint(hit.point), Quaternion.Euler(180f, 180f, 0f), this.TransformScale);
				this.instanceMat.SetMatrix("_DecalMatr", matrix4x);
			}
			if (this.materialQueue != -1)
			{
				this.instanceMat.renderQueue = this.materialQueue;
			}
			if (this.fadeInOutShaderColor != null)
			{
				foreach (FadeInOutShaderColor fadeInOutShaderColor in this.fadeInOutShaderColor)
				{
					fadeInOutShaderColor.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.fadeInOutShaderFloat != null)
			{
				foreach (FadeInOutShaderFloat fadeInOutShaderFloat in this.fadeInOutShaderFloat)
				{
					fadeInOutShaderFloat.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.uvTextureAnimator != null)
			{
				this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, hit.textureCoord);
			}
		}
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x0011FC98 File Offset: 0x0011DE98
	public void UpdateMaterial(Transform transformTarget)
	{
		if (transformTarget != null)
		{
			if (!this.RemoveWhenDisable)
			{
				Object.Destroy(base.gameObject, this.RemoveAfterTime);
			}
			this.fadeInOutShaderColor = base.GetComponents<FadeInOutShaderColor>();
			this.fadeInOutShaderFloat = base.GetComponents<FadeInOutShaderFloat>();
			this.uvTextureAnimator = base.GetComponent<UVTextureAnimator>();
			this.renderParent = base.transform.parent.GetComponent<Renderer>();
			Material[] sharedMaterials = this.renderParent.sharedMaterials;
			int num = sharedMaterials.Length + 1;
			Material[] array = new Material[num];
			sharedMaterials.CopyTo(array, 0);
			this.renderParent.material = this.Material;
			this.instanceMat = this.renderParent.material;
			array[num - 1] = this.instanceMat;
			this.renderParent.sharedMaterials = array;
			if (this.materialQueue != -1)
			{
				this.instanceMat.renderQueue = this.materialQueue;
			}
			if (this.fadeInOutShaderColor != null)
			{
				foreach (FadeInOutShaderColor fadeInOutShaderColor in this.fadeInOutShaderColor)
				{
					fadeInOutShaderColor.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.fadeInOutShaderFloat != null)
			{
				foreach (FadeInOutShaderFloat fadeInOutShaderFloat in this.fadeInOutShaderFloat)
				{
					fadeInOutShaderFloat.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.uvTextureAnimator != null)
			{
				this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, Vector2.zero);
			}
		}
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000186C6 File Offset: 0x000168C6
	public void SetMaterialQueue(int matlQueue)
	{
		this.materialQueue = matlQueue;
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000186CF File Offset: 0x000168CF
	public int GetDefaultMaterialQueue()
	{
		return this.instanceMat.renderQueue;
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x0011FE24 File Offset: 0x0011E024
	private void OnDestroy()
	{
		if (this.renderParent == null)
		{
			return;
		}
		List<Material> list = Enumerable.ToList<Material>(this.renderParent.sharedMaterials);
		list.Remove(this.instanceMat);
		this.renderParent.sharedMaterials = list.ToArray();
	}

	// Token: 0x04002CDB RID: 11483
	public float RemoveAfterTime = 5f;

	// Token: 0x04002CDC RID: 11484
	public bool RemoveWhenDisable;

	// Token: 0x04002CDD RID: 11485
	public EffectSettings EffectSettings;

	// Token: 0x04002CDE RID: 11486
	public Material Material;

	// Token: 0x04002CDF RID: 11487
	public bool UsePointMatrixTransform;

	// Token: 0x04002CE0 RID: 11488
	public Vector3 TransformScale = Vector3.one;

	// Token: 0x04002CE1 RID: 11489
	private FadeInOutShaderColor[] fadeInOutShaderColor;

	// Token: 0x04002CE2 RID: 11490
	private FadeInOutShaderFloat[] fadeInOutShaderFloat;

	// Token: 0x04002CE3 RID: 11491
	private UVTextureAnimator uvTextureAnimator;

	// Token: 0x04002CE4 RID: 11492
	private Renderer renderParent;

	// Token: 0x04002CE5 RID: 11493
	private Material instanceMat;

	// Token: 0x04002CE6 RID: 11494
	private int materialQueue = -1;

	// Token: 0x04002CE7 RID: 11495
	private bool waitRemove;

	// Token: 0x04002CE8 RID: 11496
	private float timeToDelete;
}
