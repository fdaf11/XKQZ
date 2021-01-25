using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000604 RID: 1540
[ExecuteInEditMode]
public class VLightManager : MonoBehaviour
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x0600261F RID: 9759 RVA: 0x00127E98 File Offset: 0x00126098
	public static VLightManager Instance
	{
		get
		{
			if (VLightManager._instance == null)
			{
				VLightManager._instance = (Object.FindObjectOfType(typeof(VLightManager)) as VLightManager);
				if (VLightManager._instance == null)
				{
					GameObject gameObject = new GameObject("Volume Light Manager");
					VLightManager._instance = gameObject.AddComponent<VLightManager>();
				}
			}
			return VLightManager._instance;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06002620 RID: 9760 RVA: 0x00019622 File Offset: 0x00017822
	public Matrix4x4 ViewProjection
	{
		get
		{
			return this._projection;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06002621 RID: 9761 RVA: 0x0001962A File Offset: 0x0001782A
	public Matrix4x4 ViewCameraToWorldMatrix
	{
		get
		{
			return this._cameraToWorld;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06002622 RID: 9762 RVA: 0x00019632 File Offset: 0x00017832
	public Matrix4x4 ViewWorldToCameraMatrix
	{
		get
		{
			return this._worldToCamera;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06002623 RID: 9763 RVA: 0x0001963A File Offset: 0x0001783A
	// (set) Token: 0x06002624 RID: 9764 RVA: 0x00019642 File Offset: 0x00017842
	public List<VLight> VLights
	{
		get
		{
			return this._vLights;
		}
		set
		{
			this._vLights = value;
		}
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x0001964B File Offset: 0x0001784B
	public void UpdateViewCamera(Camera viewCam)
	{
		if (viewCam == null)
		{
			return;
		}
		this._cameraToWorld = viewCam.cameraToWorldMatrix;
		this._worldToCamera = viewCam.worldToCameraMatrix;
		this._projection = viewCam.projectionMatrix;
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x00127EFC File Offset: 0x001260FC
	private void Update()
	{
		if (Application.isPlaying)
		{
			Camera camera;
			if (Camera.current != null)
			{
				camera = Camera.current;
			}
			else if (this.targetCamera != null)
			{
				camera = this.targetCamera;
			}
			else
			{
				camera = Camera.main;
			}
			if (camera == null)
			{
				return;
			}
		}
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x00127F60 File Offset: 0x00126160
	private void Start()
	{
		this._vLights.Clear();
		VLight[] array = Object.FindObjectsOfType(typeof(VLight)) as VLight[];
		this._vLights.AddRange(array);
	}

	// Token: 0x06002628 RID: 9768 RVA: 0x00127F60 File Offset: 0x00126160
	private void Enabled()
	{
		this._vLights.Clear();
		VLight[] array = Object.FindObjectsOfType(typeof(VLight)) as VLight[];
		this._vLights.AddRange(array);
	}

	// Token: 0x04002EE5 RID: 12005
	public const string VOLUMETRIC_LIGHT_LAYER_NAME = "vlight";

	// Token: 0x04002EE6 RID: 12006
	public Camera targetCamera;

	// Token: 0x04002EE7 RID: 12007
	public float maxDistance = 50f;

	// Token: 0x04002EE8 RID: 12008
	private static VLightManager _instance;

	// Token: 0x04002EE9 RID: 12009
	private Matrix4x4 _projection;

	// Token: 0x04002EEA RID: 12010
	private Matrix4x4 _cameraToWorld;

	// Token: 0x04002EEB RID: 12011
	private Matrix4x4 _worldToCamera;

	// Token: 0x04002EEC RID: 12012
	private List<VLight> _vLights = new List<VLight>();
}
