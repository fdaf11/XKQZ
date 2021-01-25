using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class AnimatorEventFly : MonoBehaviour
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x00005097 File Offset: 0x00003297
	private void Start()
	{
		this.tf_obj = base.transform.parent;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0003A3BC File Offset: 0x000385BC
	private void Update()
	{
		if (this.DelayTime <= 0f)
		{
			if (this.iType == 0)
			{
				this.UpdateType1();
			}
			else
			{
				this.UpdateType2();
			}
		}
		else
		{
			this.DelayTime -= Time.deltaTime;
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0003A40C File Offset: 0x0003860C
	private void UpdateType1()
	{
		Vector3 vector = this.unitTarget.transform.position - this.tf_obj.position;
		this.tf_obj.Translate(vector.normalized * this.Speed * Time.deltaTime, 0);
		this.tf_obj.LookAt(this.unitTarget.transform);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x000050AA File Offset: 0x000032AA
	private void UpdateType2()
	{
		this.tf_obj.Translate(this.LineDir.normalized * this.Speed * Time.deltaTime, 0);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0003A478 File Offset: 0x00038678
	public float SetTarget(UnitTB unit)
	{
		if (unit == null)
		{
			return 0f;
		}
		this.unitTarget = unit;
		this.tf_obj = base.transform.parent;
		float num = Vector3.Distance(this.tf_obj.position, this.unitTarget.transform.position);
		if (this.Speed > 0f)
		{
			num /= this.Speed;
			if (base.gameObject.transform.parent != null)
			{
				Object.Destroy(base.gameObject.transform.parent.gameObject, num);
			}
			else
			{
				Object.Destroy(base.gameObject, num);
			}
			return num;
		}
		return 0f;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0003A53C File Offset: 0x0003873C
	public bool SetLineDir(UnitTB unit)
	{
		if (unit == null)
		{
			return false;
		}
		this.iType = 1;
		this.tf_obj = base.transform.parent;
		this.LineDir = unit.transform.position - this.tf_obj.position;
		this.tf_obj.LookAt(unit.transform);
		if (base.gameObject.transform.parent != null)
		{
			Object.Destroy(base.gameObject.transform.parent.gameObject, this.DelayTime + 3f);
		}
		else
		{
			Object.Destroy(base.gameObject, this.DelayTime + 3f);
		}
		return true;
	}

	// Token: 0x040004A8 RID: 1192
	public UnitTB unitTarget;

	// Token: 0x040004A9 RID: 1193
	public float Speed = 20f;

	// Token: 0x040004AA RID: 1194
	public float DelayTime;

	// Token: 0x040004AB RID: 1195
	private Transform tf_obj;

	// Token: 0x040004AC RID: 1196
	private int iType;

	// Token: 0x040004AD RID: 1197
	private Vector3 LineDir;
}
