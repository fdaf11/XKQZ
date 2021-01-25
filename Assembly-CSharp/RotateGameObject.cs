using System;
using UnityEngine;

// Token: 0x02000535 RID: 1333
public class RotateGameObject : MonoBehaviour
{
	// Token: 0x060021F5 RID: 8693 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x001024BC File Offset: 0x001006BC
	private void FixedUpdate()
	{
		if (this.local)
		{
			base.transform.Rotate(Time.fixedDeltaTime * new Vector3(this.rot_speed_x, this.rot_speed_y, this.rot_speed_z), 1);
		}
		else
		{
			base.transform.Rotate(Time.fixedDeltaTime * new Vector3(this.rot_speed_x, this.rot_speed_y, this.rot_speed_z), 0);
		}
	}

	// Token: 0x040025F6 RID: 9718
	public float rot_speed_x;

	// Token: 0x040025F7 RID: 9719
	public float rot_speed_y;

	// Token: 0x040025F8 RID: 9720
	public float rot_speed_z;

	// Token: 0x040025F9 RID: 9721
	public bool local;
}
