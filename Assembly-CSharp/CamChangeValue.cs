using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000666 RID: 1638
public class CamChangeValue : MonoBehaviour
{
	// Token: 0x06002823 RID: 10275 RVA: 0x0001A734 File Offset: 0x00018934
	private void Start()
	{
		if (!this.MainCamera)
		{
			this.MainCamera = GameObject.FindWithTag("MainCamera");
		}
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x0001A756 File Offset: 0x00018956
	private void Update()
	{
		if (CamChangeValue.go)
		{
			this.goChange();
		}
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x0013D3F8 File Offset: 0x0013B5F8
	private void OnTriggerEnter(Collider playerOne)
	{
		if (this.CloseCamOrbit)
		{
			this.MainCamera.GetComponentInChildren<OrbitCam>().enabled = false;
			this.MainCamera.transform.position = this.CameraDummy.position;
			this.MainCamera.transform.rotation = this.CameraDummy.rotation;
		}
		CamChangeValue.ssmoothRotate = this.smoothRotate;
		CamChangeValue.sDistance = this.Distance;
		CamChangeValue.sRotation = this.Rotation;
		CamChangeValue.sTilt = this.Tilt;
		CamChangeValue.sDistanceDampening = this.DistanceDampening;
		CamChangeValue.sRotationDampening = this.RotationDampening;
		CamChangeValue.sTiltDampening = this.TiltDampening;
		CamChangeValue.go = true;
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x0013D4AC File Offset: 0x0013B6AC
	private void goChange()
	{
		this.MainCamera.GetComponentInChildren<OrbitCam>().Smoothing = CamChangeValue.ssmoothRotate;
		if (this.MainCamera.GetComponentInChildren<OrbitCam>().Distance == CamChangeValue.sDistance && this.MainCamera.GetComponentInChildren<OrbitCam>().Rotation == CamChangeValue.sRotation && this.MainCamera.GetComponentInChildren<OrbitCam>().Tilt == CamChangeValue.sTilt)
		{
			CamChangeValue.go = false;
			return;
		}
		this.MainCamera.GetComponentInChildren<OrbitCam>().ZoomDampening = CamChangeValue.sDistanceDampening;
		this.MainCamera.GetComponentInChildren<OrbitCam>().RotationDampening = CamChangeValue.sRotationDampening;
		this.MainCamera.GetComponentInChildren<OrbitCam>().TiltDampening = CamChangeValue.sTiltDampening;
		this.MainCamera.GetComponentInChildren<OrbitCam>().Distance = CamChangeValue.sDistance;
		this.MainCamera.GetComponentInChildren<OrbitCam>().Rotation = CamChangeValue.sRotation;
		this.MainCamera.GetComponentInChildren<OrbitCam>().Tilt = CamChangeValue.sTilt;
		MapData.m_instance._CameraSaveDateNode.m_fDistance = CamChangeValue.sDistance;
		MapData.m_instance._CameraSaveDateNode.m_fRotation = CamChangeValue.sRotation;
		MapData.m_instance._CameraSaveDateNode.m_fTilt = CamChangeValue.sTilt;
	}

	// Token: 0x0400323A RID: 12858
	public GameObject MainCamera;

	// Token: 0x0400323B RID: 12859
	public float Distance = 12f;

	// Token: 0x0400323C RID: 12860
	public float Rotation = -45f;

	// Token: 0x0400323D RID: 12861
	public float Tilt = 45f;

	// Token: 0x0400323E RID: 12862
	public float DistanceDampening = 5f;

	// Token: 0x0400323F RID: 12863
	public float RotationDampening = 1.65f;

	// Token: 0x04003240 RID: 12864
	public float TiltDampening = 5f;

	// Token: 0x04003241 RID: 12865
	public bool smoothRotate = true;

	// Token: 0x04003242 RID: 12866
	public bool CloseCamOrbit;

	// Token: 0x04003243 RID: 12867
	public Transform CameraDummy;

	// Token: 0x04003244 RID: 12868
	private static float sDistanceDampening;

	// Token: 0x04003245 RID: 12869
	private static float sRotationDampening;

	// Token: 0x04003246 RID: 12870
	private static float sTiltDampening;

	// Token: 0x04003247 RID: 12871
	private static bool ssmoothRotate;

	// Token: 0x04003248 RID: 12872
	private static float sDistance;

	// Token: 0x04003249 RID: 12873
	private static float sRotation;

	// Token: 0x0400324A RID: 12874
	private static float sTilt;

	// Token: 0x0400324B RID: 12875
	private static bool go;
}
