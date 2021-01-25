using System;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class TOD_Animation : MonoBehaviour
{
	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060032CE RID: 13006 RVA: 0x0001FD7C File Offset: 0x0001DF7C
	// (set) Token: 0x060032CF RID: 13007 RVA: 0x0001FD84 File Offset: 0x0001DF84
	internal Vector4 CloudUV { get; set; }

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00187CC4 File Offset: 0x00185EC4
	internal Vector4 OffsetUV
	{
		get
		{
			if (!this.WorldSpaceCloudUV)
			{
				return Vector4.zero;
			}
			Vector3 position = base.transform.position;
			Vector3 lossyScale = base.transform.lossyScale;
			Vector3 vector;
			vector..ctor(position.x / lossyScale.x, 0f, position.z / lossyScale.z);
			vector = Quaternion.Euler(0f, -base.transform.rotation.eulerAngles.y, 0f) * vector;
			return new Vector4(vector.x, vector.z, vector.x, vector.z);
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x0001FD8D File Offset: 0x0001DF8D
	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x00187D78 File Offset: 0x00185F78
	protected void Update()
	{
		Vector2 vector;
		vector..ctor(Mathf.Cos(0.017453292f * (this.WindDegrees + 15f)), Mathf.Sin(0.017453292f * (this.WindDegrees + 15f)));
		Vector2 vector2;
		vector2..ctor(Mathf.Cos(0.017453292f * (this.WindDegrees - 15f)), Mathf.Sin(0.017453292f * (this.WindDegrees - 15f)));
		Vector4 vector3 = this.WindSpeed / 100f * new Vector4(vector.x, vector.y, vector2.x, vector2.y);
		this.CloudUV += Time.deltaTime * vector3;
		this.CloudUV = new Vector4(this.CloudUV.x % this.sky.Clouds.Scale1.x, this.CloudUV.y % this.sky.Clouds.Scale1.y, this.CloudUV.z % this.sky.Clouds.Scale2.x, this.CloudUV.w % this.sky.Clouds.Scale2.y);
	}

	// Token: 0x04003E40 RID: 15936
	public float WindDegrees;

	// Token: 0x04003E41 RID: 15937
	public float WindSpeed = 3f;

	// Token: 0x04003E42 RID: 15938
	public bool WorldSpaceCloudUV = true;

	// Token: 0x04003E43 RID: 15939
	private TOD_Sky sky;
}
