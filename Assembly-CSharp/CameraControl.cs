using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000734 RID: 1844
public class CameraControl : MonoBehaviour
{
	// Token: 0x06002BA9 RID: 11177 RVA: 0x0001C290 File Offset: 0x0001A490
	private void Awake()
	{
		CameraControl.instance = this;
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x0001C298 File Offset: 0x0001A498
	private void OnDestroy()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyHeld -= new Action<KeyControl.Key>(this.OnKeyHeld);
		}
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x00154350 File Offset: 0x00152550
	private void Start()
	{
		this.thisT = base.transform;
		this.camT = this.mainCam.transform;
		this.actionCamT = this.actionCam.transform;
		if (this.actionCamDummyT == null)
		{
			this.actionCamDummyT = new GameObject().transform;
			this.actionCamDummyT.parent = this.thisT;
			this.actionCamDummyT.gameObject.name = "ActionCamDummy";
		}
		this.actionCam.enabled = false;
		int layerUI = LayerManager.GetLayerUI();
		int layerUnitAIInvisible = LayerManager.GetLayerUnitAIInvisible();
		LayerMask layerMask = 1 << layerUI | 1 << layerUnitAIInvisible;
		layerMask = ~layerMask;
		this.mainCam.cullingMask = (this.mainCam.cullingMask & layerMask);
		this.actionCam.cullingMask = (this.actionCam.cullingMask & layerMask);
		this.actionCam.depth = 99f;
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyHeld += new Action<KeyControl.Key>(this.OnKeyHeld);
		}
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
	public static void ActionCam(Tile attacker, Tile target)
	{
		if (CameraControl.instance != null)
		{
			CameraControl.instance.StartCoroutine(CameraControl.instance.ActionCamRoutine(attacker, target));
		}
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x0015447C File Offset: 0x0015267C
	private IEnumerator ActionCamRoutine(Tile attacker, Tile target)
	{
		this.actionCamActivated = true;
		UnitTB.onActionCompletedE += this.OnActionCompleted;
		this.actionInProgress = true;
		this.actionCamDummyT.position = attacker.pos;
		this.actionCamDummyT.LookAt(target.thisT);
		float x = Random.Range(-2f, 2f);
		if (x > 0f)
		{
			x = 1.5f;
		}
		else
		{
			x = -1.5f;
		}
		Vector3 vector;
		vector..ctor(x, 2f, -5f);
		Vector3 dir = vector.normalized;
		Vector3 wantedPos = this.actionCamDummyT.TransformPoint(dir * this.actionCamDistance);
		this.actionCamT.position = wantedPos;
		this.actionCamT.LookAt(target.thisT);
		this.actionCam.enabled = true;
		this.mainCam.enabled = false;
		while (this.actionInProgress && !this.stopActionCam)
		{
			yield return null;
		}
		if (!this.stopActionCam)
		{
			yield return new WaitForSeconds(this.actionCamDelay);
		}
		this.actionCam.enabled = false;
		this.mainCam.enabled = true;
		this.actionCamActivated = false;
		this.stopActionCam = false;
		yield break;
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x0001C2E9 File Offset: 0x0001A4E9
	public static void StopActionCam()
	{
		if (CameraControl.instance.actionCamActivated)
		{
			CameraControl.instance.stopActionCam = true;
		}
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x0001C305 File Offset: 0x0001A505
	public static bool ActionCamInAction()
	{
		return CameraControl.instance.actionCamActivated || CameraControl.instance.killCamActivated;
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x0001C328 File Offset: 0x0001A528
	private void OnActionCompleted(UnitTB unit)
	{
		UnitTB.onActionCompletedE -= this.OnActionCompleted;
		base.StartCoroutine(this._OnActionCompleted());
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x001544B4 File Offset: 0x001526B4
	private IEnumerator _OnActionCompleted()
	{
		yield return new WaitForSeconds(0.5f);
		this.actionInProgress = false;
		yield break;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x001544D0 File Offset: 0x001526D0
	private void Update()
	{
		if (this.trackUnit)
		{
			if (this.trackTalkUnit)
			{
				if (this.talkUnit != null && this.lastSelectedUnit != this.talkUnit)
				{
					this.lastSelectedUnit = this.talkUnit;
					this.tracking = true;
				}
			}
			else if (this.trackTile)
			{
				if (this.trackNowTile != null && this.lastTile != this.trackNowTile)
				{
					this.lastTile = this.trackNowTile;
					this.tracking = true;
				}
			}
			else
			{
				this.lastTile = null;
				if (UnitControl.selectedUnit != null && this.lastSelectedUnit != UnitControl.selectedUnit)
				{
					this.DoFS = this.mainCam.GetComponent<DepthOfFieldScatter>();
					if (this.DoFS != null)
					{
						this.DoFS.focalTransform = UnitControl.selectedUnit.transform;
					}
					this.lastSelectedUnit = UnitControl.selectedUnit;
					this.tracking = true;
				}
			}
			if (this.tracking)
			{
				if (this.lastTile != null)
				{
					float num = Vector3.Distance(this.thisT.position, this.lastTile.thisT.position) * 0.025f;
					num = Mathf.Max(1f, num);
					this.thisT.position = Vector3.Lerp(this.thisT.position, this.lastTile.thisT.position, Time.deltaTime * 5f * num);
				}
				else if (this.lastSelectedUnit != null && this.lastSelectedUnit.HP > 0)
				{
					float num2 = Vector3.Distance(this.thisT.position, this.lastSelectedUnit.thisT.position) * 0.025f;
					num2 = Mathf.Max(1f, num2);
					this.thisT.position = Vector3.Lerp(this.thisT.position, this.lastSelectedUnit.thisT.position, Time.deltaTime * 5f * num2);
				}
				else
				{
					this.tracking = false;
				}
			}
		}
		if (Input.GetKeyDown(99))
		{
			CameraControl.StopActionCam();
		}
		if (Time.timeScale == 1f)
		{
			this.deltaT = Time.deltaTime;
		}
		else if (Time.timeScale > 1f)
		{
			this.deltaT = Time.deltaTime / Time.timeScale;
		}
		else
		{
			this.deltaT = 0.015f;
		}
		Quaternion quaternion = Quaternion.Euler(0f, this.thisT.eulerAngles.y, 0f);
		if (this.enableKeyPanning)
		{
			if (Input.GetButton("Horizontal"))
			{
				this.tracking = false;
				Vector3 vector = base.transform.InverseTransformDirection(quaternion * Vector3.right);
				this.thisT.Translate(vector * this.panSpeed * this.deltaT * Input.GetAxisRaw("Horizontal"));
			}
			if (Input.GetButton("Vertical"))
			{
				this.tracking = false;
				Vector3 vector2 = base.transform.InverseTransformDirection(quaternion * Vector3.forward);
				this.thisT.Translate(vector2 * this.panSpeed * this.deltaT * Input.GetAxisRaw("Vertical"));
			}
		}
		if (this.enableMousePanning)
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 vector3 = base.transform.InverseTransformDirection(quaternion * Vector3.right);
			if (mousePosition.x <= 0f)
			{
				this.tracking = false;
				this.thisT.Translate(vector3 * this.panSpeed * this.deltaT * -3f);
			}
			else if (mousePosition.x <= (float)this.mousePanningZoneWidth)
			{
				this.tracking = false;
				this.thisT.Translate(vector3 * this.panSpeed * this.deltaT * -1f);
			}
			else if (mousePosition.x >= (float)Screen.width)
			{
				this.tracking = false;
				this.thisT.Translate(vector3 * this.panSpeed * this.deltaT * 3f);
			}
			else if (mousePosition.x > (float)(Screen.width - this.mousePanningZoneWidth))
			{
				this.tracking = false;
				this.thisT.Translate(vector3 * this.panSpeed * this.deltaT * 1f);
			}
			Vector3 vector4 = base.transform.InverseTransformDirection(quaternion * Vector3.forward);
			if (mousePosition.y <= 0f)
			{
				this.tracking = false;
				this.thisT.Translate(vector4 * this.panSpeed * this.deltaT * -3f);
			}
			else if (mousePosition.y <= (float)this.mousePanningZoneWidth)
			{
				this.tracking = false;
				this.thisT.Translate(vector4 * this.panSpeed * this.deltaT * -1f);
			}
			else if (mousePosition.y >= (float)Screen.height)
			{
				this.tracking = false;
				this.thisT.Translate(vector4 * this.panSpeed * this.deltaT * 3f);
			}
			else if (mousePosition.y > (float)(Screen.height - this.mousePanningZoneWidth))
			{
				this.tracking = false;
				this.thisT.Translate(vector4 * this.panSpeed * this.deltaT * 1f);
			}
		}
		if (this.enableMouseWheelZoom)
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.camT.Translate(Vector3.forward * this.zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
			}
			else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.camT.Translate(Vector3.forward * this.zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
			}
			float num3 = Mathf.Clamp(this.camT.localPosition.z, -this.maxRadius, -this.minRadius);
			float num4 = Mathf.Abs(num3);
			num4 = 25f + (num4 - this.minRadius) * 2.5f;
			this.thisT.localEulerAngles = new Vector3(num4, this.thisT.localEulerAngles.y, this.thisT.localEulerAngles.z);
			this.camT.localPosition = new Vector3(this.camT.localPosition.x, this.camT.localPosition.y, num3);
		}
		float num5 = Mathf.Clamp(this.thisT.position.x, this.minPosX, this.maxPosX);
		float num6 = Mathf.Clamp(this.thisT.position.z, this.minPosZ, this.maxPosZ);
		this.thisT.position = new Vector3(num5, this.thisT.position.y, num6);
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x0001C348 File Offset: 0x0001A548
	public static Camera GetActiveCamera()
	{
		if (CameraControl.instance.mainCam.enabled)
		{
			return CameraControl.instance.mainCam;
		}
		return CameraControl.instance.actionCam;
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x0001C373 File Offset: 0x0001A573
	public static void Enable()
	{
		if (CameraControl.instance != null)
		{
			CameraControl.instance.enabled = true;
		}
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x0001C390 File Offset: 0x0001A590
	public static void Disable()
	{
		if (CameraControl.instance != null)
		{
			CameraControl.instance.enabled = false;
		}
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x00154CC0 File Offset: 0x00152EC0
	private void OnDrawGizmos()
	{
		if (this.showGizmo)
		{
			Vector3 vector;
			vector..ctor(this.minPosX, base.transform.position.y, this.maxPosZ);
			Vector3 vector2;
			vector2..ctor(this.maxPosX, base.transform.position.y, this.maxPosZ);
			Vector3 vector3;
			vector3..ctor(this.maxPosX, base.transform.position.y, this.minPosZ);
			Vector3 vector4;
			vector4..ctor(this.minPosX, base.transform.position.y, this.minPosZ);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x00154D9C File Offset: 0x00152F9C
	private void OnKeyHeld(KeyControl.Key keyCode)
	{
		if (keyCode != KeyControl.Key.RotateLeft)
		{
			if (keyCode == KeyControl.Key.RotateRight)
			{
				if (this.enableKeyRotate)
				{
					this.thisT.Rotate(-Vector3.up * Time.deltaTime * 10f * this.rotateSpeed, 0);
				}
			}
		}
		else if (this.enableKeyRotate)
		{
			this.thisT.Rotate(Vector3.up * Time.deltaTime * 10f * this.rotateSpeed, 0);
		}
	}

	// Token: 0x0400384D RID: 14413
	private DepthOfFieldScatter DoFS;

	// Token: 0x0400384E RID: 14414
	public float panSpeed = 8f;

	// Token: 0x0400384F RID: 14415
	public float zoomSpeed = 5f;

	// Token: 0x04003850 RID: 14416
	public float rotateSpeed = 5f;

	// Token: 0x04003851 RID: 14417
	public bool enableKeyRotate = true;

	// Token: 0x04003852 RID: 14418
	public bool enableKeyPanning = true;

	// Token: 0x04003853 RID: 14419
	public bool enableMouseWheelZoom = true;

	// Token: 0x04003854 RID: 14420
	public bool enableMousePanning;

	// Token: 0x04003855 RID: 14421
	public int mousePanningZoneWidth = 10;

	// Token: 0x04003856 RID: 14422
	public float minPosX = -4f;

	// Token: 0x04003857 RID: 14423
	public float maxPosX = 4f;

	// Token: 0x04003858 RID: 14424
	public float minPosZ = -4f;

	// Token: 0x04003859 RID: 14425
	public float maxPosZ = 4f;

	// Token: 0x0400385A RID: 14426
	public float minRadius = 6f;

	// Token: 0x0400385B RID: 14427
	public float maxRadius = 15f;

	// Token: 0x0400385C RID: 14428
	private float deltaT = 1f;

	// Token: 0x0400385D RID: 14429
	private Transform camT;

	// Token: 0x0400385E RID: 14430
	private Transform thisT;

	// Token: 0x0400385F RID: 14431
	private Transform actionCamT;

	// Token: 0x04003860 RID: 14432
	[HideInInspector]
	public Transform actionCamDummyT;

	// Token: 0x04003861 RID: 14433
	public Camera actionCam;

	// Token: 0x04003862 RID: 14434
	public Camera mainCam;

	// Token: 0x04003863 RID: 14435
	public float actionCamDistance = 5.5f;

	// Token: 0x04003864 RID: 14436
	public float actionCamDelay = 0.5f;

	// Token: 0x04003865 RID: 14437
	[HideInInspector]
	public bool actionCamActivated;

	// Token: 0x04003866 RID: 14438
	[HideInInspector]
	public bool killCamActivated;

	// Token: 0x04003867 RID: 14439
	public static CameraControl instance;

	// Token: 0x04003868 RID: 14440
	private bool stopActionCam;

	// Token: 0x04003869 RID: 14441
	private bool actionInProgress;

	// Token: 0x0400386A RID: 14442
	public bool trackUnit;

	// Token: 0x0400386B RID: 14443
	private Tile lastTile;

	// Token: 0x0400386C RID: 14444
	private UnitTB lastSelectedUnit;

	// Token: 0x0400386D RID: 14445
	private bool tracking = true;

	// Token: 0x0400386E RID: 14446
	[HideInInspector]
	public bool trackTile;

	// Token: 0x0400386F RID: 14447
	[HideInInspector]
	public Tile trackNowTile;

	// Token: 0x04003870 RID: 14448
	[HideInInspector]
	public UnitTB talkUnit;

	// Token: 0x04003871 RID: 14449
	[HideInInspector]
	public bool trackTalkUnit;

	// Token: 0x04003872 RID: 14450
	public bool showGizmo = true;
}
