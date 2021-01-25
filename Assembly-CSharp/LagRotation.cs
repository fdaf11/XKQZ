using System;
using UnityEngine;

// Token: 0x02000432 RID: 1074
[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x00010E6C File Offset: 0x0000F06C
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x000D036C File Offset: 0x000CE56C
	private void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
			this.mTrans.rotation = this.mAbsolute;
		}
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00010E79 File Offset: 0x0000F079
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x00010EA9 File Offset: 0x0000F0A9
	private void Update()
	{
		this.Interpolate((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
	}

	// Token: 0x04001EBC RID: 7868
	public float speed = 10f;

	// Token: 0x04001EBD RID: 7869
	public bool ignoreTimeScale;

	// Token: 0x04001EBE RID: 7870
	private Transform mTrans;

	// Token: 0x04001EBF RID: 7871
	private Quaternion mRelative;

	// Token: 0x04001EC0 RID: 7872
	private Quaternion mAbsolute;
}
