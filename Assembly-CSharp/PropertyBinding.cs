using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
	// Token: 0x06001D63 RID: 7523 RVA: 0x000136FC File Offset: 0x000118FC
	private void Start()
	{
		this.UpdateTarget();
		if (this.update == PropertyBinding.UpdateCondition.OnStart)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00013716 File Offset: 0x00011916
	private void Update()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0001372A File Offset: 0x0001192A
	private void LateUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnLateUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0001373E File Offset: 0x0001193E
	private void FixedUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnFixedUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x00013752 File Offset: 0x00011952
	private void OnValidate()
	{
		if (this.source != null)
		{
			this.source.Reset();
		}
		if (this.target != null)
		{
			this.target.Reset();
		}
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000E5428 File Offset: 0x000E3628
	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.source != null && this.target != null && this.source.isValid && this.target.isValid)
		{
			if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
			{
				this.target.Set(this.source.Get());
			}
			else if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
			{
				this.source.Set(this.target.Get());
			}
			else if (this.source.GetPropertyType() == this.target.GetPropertyType())
			{
				object obj = this.source.Get();
				if (this.mLastValue == null || !this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.target.Set(obj);
				}
				else
				{
					obj = this.target.Get();
					if (!this.mLastValue.Equals(obj))
					{
						this.mLastValue = obj;
						this.source.Set(obj);
					}
				}
			}
		}
	}

	// Token: 0x04002196 RID: 8598
	public PropertyReference source;

	// Token: 0x04002197 RID: 8599
	public PropertyReference target;

	// Token: 0x04002198 RID: 8600
	public PropertyBinding.Direction direction;

	// Token: 0x04002199 RID: 8601
	public PropertyBinding.UpdateCondition update = PropertyBinding.UpdateCondition.OnUpdate;

	// Token: 0x0400219A RID: 8602
	public bool editMode = true;

	// Token: 0x0400219B RID: 8603
	private object mLastValue;

	// Token: 0x020004A6 RID: 1190
	public enum UpdateCondition
	{
		// Token: 0x0400219D RID: 8605
		OnStart,
		// Token: 0x0400219E RID: 8606
		OnUpdate,
		// Token: 0x0400219F RID: 8607
		OnLateUpdate,
		// Token: 0x040021A0 RID: 8608
		OnFixedUpdate
	}

	// Token: 0x020004A7 RID: 1191
	public enum Direction
	{
		// Token: 0x040021A2 RID: 8610
		SourceUpdatesTarget,
		// Token: 0x040021A3 RID: 8611
		TargetUpdatesSource,
		// Token: 0x040021A4 RID: 8612
		BiDirectional
	}
}
