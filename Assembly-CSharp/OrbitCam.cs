using System;
using UnityEngine;

// Token: 0x0200051F RID: 1311
[AddComponentMenu("Camera-Control/OrbitCam")]
public class OrbitCam : MonoBehaviour
{
	// Token: 0x0600219A RID: 8602 RVA: 0x000FCD5C File Offset: 0x000FAF5C
	public void Reset()
	{
		this.Smoothing = true;
		this.CollisionDetection = true;
		this.CameraRadius = 0.5f;
		this.PushCameraCollisionIgnoreLayer = 0;
		this.HighlightingIgnoreLayer = 0;
		this.DisapperIgnoreLayer = 0;
		this.AutoSetIgnoreLayers = false;
		this.Distance = 12f;
		this.MinDistance = 6f;
		this.MaxDistance = 12f;
		this.Rotation = 0f;
		this.MinRotation = -180f;
		this.MaxRotation = 180f;
		this.Tilt = 45f;
		this.MinTilt = -15f;
		this.MaxTilt = 85f;
		this.TiltDampening = 5f;
		this.ZoomDampening = 5f;
		this.RotationDampening = 5f;
		this.FollowBehind = false;
		this.FollowRotationOffset = 0f;
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x000FCE44 File Offset: 0x000FB044
	protected void Start()
	{
		this.ho = false;
		this.CanZoom = true;
		this.Zoom = true;
		this.DoHightLight = false;
		this.GoAlpha = false;
		this.GoHide = false;
		this.tempPosition = base.transform.position;
		this.DiffuseShader = Shader.Find("Diffuse");
		this.transparentwithshadow = Shader.Find("Shaq/transparentwithshadow");
		if (this.transparentwithshadow == null)
		{
			Debug.LogError("Shaq Shader Lost  Why ?");
		}
		if (this.Target == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			this.Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (this.Target)
		{
			if (this.Target.transform.GetComponent<HighlightableObject>() != null)
			{
				this.highligh = this.Target.transform.GetComponent<HighlightableObject>();
			}
			else
			{
				this.highligh = this.Target.gameObject.AddComponent<HighlightableObject>();
			}
		}
		if (base.rigidbody)
		{
			base.rigidbody.freezeRotation = true;
		}
		this._currDistance = this.Distance;
		this._currRotation = this.Rotation;
		this._currTilt = this.Tilt;
	}

	// Token: 0x0600219C RID: 8604 RVA: 0x000FCFA0 File Offset: 0x000FB1A0
	private void CheckTarget()
	{
		if (this.Target != null)
		{
			return;
		}
		if (this.Target == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			this.Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (this.Target)
		{
			if (this.Target.transform.GetComponent<HighlightableObject>() != null)
			{
				this.highligh = this.Target.transform.GetComponent<HighlightableObject>();
			}
			else
			{
				this.highligh = this.Target.gameObject.AddComponent<HighlightableObject>();
			}
		}
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x000FD058 File Offset: 0x000FB258
	protected void Update()
	{
		this.CheckTarget();
		if (this.bFadeIn)
		{
			this.UpdateFadeIn();
		}
		if (GameGlobal.m_bMovie)
		{
			return;
		}
		this.Zoom = (this.Distance == this.MaxDistance);
		if (Input.GetKeyDown(9) && this.CanZoom && !this.LockTabKey)
		{
			if (this.Zoom)
			{
				this.CanZoom = false;
				this.Distance = this.MinDistance;
				this.Tilt = 40f;
				this.CanZoom = true;
			}
			else
			{
				this.CanZoom = false;
				this.Distance = this.MaxDistance;
				this.Tilt = 45f;
				this.CanZoom = true;
			}
		}
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000FD124 File Offset: 0x000FB324
	protected void LateUpdate()
	{
		if (this.Target == null)
		{
			return;
		}
		if (this.highligh == null)
		{
			if (this.Target.transform.GetComponent<HighlightableObject>() != null)
			{
				this.highligh = this.Target.transform.GetComponent<HighlightableObject>();
			}
			else
			{
				this.highligh = this.Target.gameObject.AddComponent<HighlightableObject>();
			}
		}
		if (this.ho)
		{
			if (this.Target.transform.animation != null)
			{
				Color color = new Color32(251, 228, 179, byte.MaxValue);
				Color color2 = new Color32(251, 228, 179, 0);
				if (this.Target.transform.animation.IsPlaying("run"))
				{
					this.highligh.Off();
					this.highligh.ConstantOnImmediate(color);
				}
				else
				{
					this.highligh.FlashingOn(color, color2, 0.75f);
				}
			}
		}
		else
		{
			this.highligh.Off();
		}
		this.Rotation = this.WrapAngle(this.Rotation);
		this.Tilt = this.WrapAngle(this.Tilt);
		this.Tilt = Mathf.Clamp(this.Tilt, this.MinTilt, this.MaxTilt);
		this.Distance = Mathf.Clamp(this.Distance, this.MinDistance, this.MaxDistance);
		this.Rotation = Mathf.Clamp(this.Rotation, this.MinRotation, this.MaxRotation);
		if (this.Smoothing)
		{
			this._currRotation = Mathf.LerpAngle(this._currRotation, this.Rotation, Time.deltaTime * this.RotationDampening);
			this._currDistance = Mathf.Lerp(this._currDistance, this.Distance, Time.deltaTime * this.ZoomDampening);
			this._currTilt = Mathf.LerpAngle(this._currTilt, this.Tilt, Time.deltaTime * this.TiltDampening);
		}
		else
		{
			this._currRotation = this.Rotation;
			this._currDistance = Mathf.Lerp(this._currDistance, this.Distance, Time.deltaTime * this.ZoomDampening);
			this._currTilt = Mathf.LerpAngle(this._currTilt, this.Tilt, Time.deltaTime * this.TiltDampening);
		}
		if (this.FollowBehind)
		{
			this.ForceFollowBehind();
		}
		if (this.CollisionDetection)
		{
			if (this.AutoSetIgnoreLayers && this.PushCameraCollisionIgnoreLayer == 0)
			{
				this.PushCameraCollisionIgnoreLayer = 1 << this.Target.gameObject.layer;
			}
			if (this.AutoSetIgnoreLayers && this.HighlightingIgnoreLayer == 0)
			{
				this.HighlightingIgnoreLayer = 1 << this.Target.gameObject.layer;
			}
			if (this.AutoSetIgnoreLayers && this.DisapperIgnoreLayer == 0)
			{
				this.HighlightingIgnoreLayer = 1 << this.Target.gameObject.layer;
			}
			this.CheckCollisions();
		}
		this.UpdateCamera();
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000FD47C File Offset: 0x000FB67C
	public void SnapTo(float distance, float rotation, float tilt)
	{
		this._currDistance = distance;
		this.Distance = distance;
		this._currRotation = rotation;
		this.Rotation = rotation;
		this._currTilt = tilt;
		this.Tilt = tilt;
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000FD4BC File Offset: 0x000FB6BC
	private void UpdateCamera()
	{
		if (this.Target == null)
		{
			return;
		}
		Quaternion quaternion = Quaternion.Euler(this._currTilt, this._currRotation, 0f);
		Vector3 vector;
		vector..ctor(0f, 0f, -this._currDistance);
		Vector3 position = quaternion * vector + this.Target.transform.position + new Vector3(0f, 0f, 0f);
		if (base.camera.orthographic)
		{
			base.camera.orthographicSize = this._currDistance;
		}
		base.transform.rotation = quaternion;
		base.transform.position = position;
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000FD57C File Offset: 0x000FB77C
	private void CheckCollisions()
	{
		Vector3 vector = base.transform.position - (this.Target.transform.position + new Vector3(0f, 1.5f, 0.5f));
		vector.Normalize();
		float distance = this.Distance;
		RaycastHit raycastHit;
		if (Physics.SphereCast(this.Target.transform.position + new Vector3(0f, 1.5f, 0.5f), this.CameraRadius, vector, ref raycastHit, distance, ~this.PushCameraCollisionIgnoreLayer))
		{
			if (raycastHit.transform != this.Target)
			{
				this.DisapperOb = raycastHit.collider.transform.parent;
				this.DisapperObChild = this.DisapperOb.GetComponentsInChildren<Renderer>();
				this.OriColor = raycastHit.collider.transform.renderer.material.color;
				foreach (Component component in this.DisapperObChild)
				{
					this.GoHide = true;
					component.renderer.enabled = false;
				}
			}
		}
		else if (this.GoHide)
		{
			foreach (Component component2 in this.DisapperObChild)
			{
				component2.renderer.enabled = true;
				this.GoHide = false;
			}
		}
		if (Physics.SphereCast(this.Target.transform.position + new Vector3(0f, 1.5f, 0.5f), this.CameraRadius, vector, ref raycastHit, distance, ~this.HighlightingIgnoreLayer))
		{
			if (raycastHit.transform != this.Target)
			{
				this.DoHightLight = true;
				this.ho = true;
				this.CanZoom = false;
				if (!this.LockTabKey)
				{
					this.Distance = this.MaxDistance;
					this.Tilt = 45f;
				}
			}
		}
		else if (this.DoHightLight)
		{
			this.ho = false;
			this.CanZoom = true;
			this.DoHightLight = false;
		}
		if (Physics.SphereCast(this.Target.transform.position + new Vector3(0f, 1.5f, 0.5f), this.CameraRadius, vector, ref raycastHit, distance, ~this.DisapperIgnoreLayer))
		{
			if (raycastHit.transform != this.Target)
			{
				this.ho = true;
				this.DoHightLight = true;
				this.DisapperOb = raycastHit.collider.transform.parent;
				this.DisapperObChild = this.DisapperOb.GetComponentsInChildren<Renderer>();
				this.OriColor = raycastHit.collider.transform.renderer.material.color;
				foreach (Component component3 in this.DisapperObChild)
				{
					component3.renderer.material.shader = this.transparentwithshadow;
					this.GoAlpha = true;
					component3.renderer.material.color = Color.Lerp(component3.renderer.material.color, new Color(component3.renderer.material.color.r, component3.renderer.material.color.g, component3.renderer.material.color.b, 0.2f), Time.deltaTime);
				}
			}
		}
		else if (this.GoAlpha)
		{
			foreach (Component component4 in this.DisapperObChild)
			{
				component4.renderer.material.color = Color.Lerp(component4.renderer.material.color, new Color(component4.renderer.material.color.r, component4.renderer.material.color.g, component4.renderer.material.color.b, 1f), Time.deltaTime);
				if (component4.renderer.material.color.a > 0.99f)
				{
					component4.renderer.material.color = new Color(component4.renderer.material.color.r, component4.renderer.material.color.g, component4.renderer.material.color.b, 1f);
					component4.renderer.material.shader = this.DiffuseShader;
					this.GoAlpha = false;
				}
			}
		}
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000FDAA4 File Offset: 0x000FBCA4
	private void ForceFollowBehind()
	{
		Vector3 vector = this.Target.transform.forward * -1f;
		float num = Vector3.Angle(Vector3.forward, vector);
		float num2 = (Vector3.Dot(vector, Vector3.right) <= 0f) ? -1f : 1f;
		this._currRotation = (this.Rotation = 180f + num2 * num + this.FollowRotationOffset);
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000168A8 File Offset: 0x00014AA8
	private float WrapAngle(float angle)
	{
		while (angle < -180f)
		{
			angle += 360f;
		}
		while (angle > 180f)
		{
			angle -= 360f;
		}
		return angle;
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000FDB1C File Offset: 0x000FBD1C
	public void FadeIn(float fadeinTime)
	{
		if (this.SO == null)
		{
			this.SO = base.gameObject.GetComponent<ScreenOverlay>();
			if (this.SO == null)
			{
				this.SO = base.gameObject.AddComponent<ScreenOverlay>();
			}
		}
		if (this.SO == null)
		{
			return;
		}
		this.fFadeInTime = fadeinTime;
		this.fFadeInPos = 0f;
		this.bFadeIn = true;
		this.SO.enabled = true;
		this.SO.intensity = 0f;
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000FDBB4 File Offset: 0x000FBDB4
	private void UpdateFadeIn()
	{
		if (this.SO == null)
		{
			this.bFadeIn = false;
			return;
		}
		if (this.fFadeInTime < 0f)
		{
			this.bFadeIn = false;
			return;
		}
		this.fFadeInPos += Time.deltaTime;
		if (this.fFadeInPos > this.fFadeInTime)
		{
			this.SO.intensity = 1f;
			this.SO.enabled = false;
			this.bFadeIn = false;
		}
		else
		{
			this.SO.intensity = this.fFadeInPos / this.fFadeInTime;
		}
	}

	// Token: 0x040024BF RID: 9407
	public bool LockTabKey;

	// Token: 0x040024C0 RID: 9408
	public Transform Target;

	// Token: 0x040024C1 RID: 9409
	public float Distance;

	// Token: 0x040024C2 RID: 9410
	public float Rotation;

	// Token: 0x040024C3 RID: 9411
	public float Tilt;

	// Token: 0x040024C4 RID: 9412
	public bool Smoothing;

	// Token: 0x040024C5 RID: 9413
	public float ZoomDampening;

	// Token: 0x040024C6 RID: 9414
	public float RotationDampening;

	// Token: 0x040024C7 RID: 9415
	public float TiltDampening;

	// Token: 0x040024C8 RID: 9416
	public float MinDistance;

	// Token: 0x040024C9 RID: 9417
	public float MaxDistance;

	// Token: 0x040024CA RID: 9418
	public float MinRotation;

	// Token: 0x040024CB RID: 9419
	public float MaxRotation;

	// Token: 0x040024CC RID: 9420
	public float MinTilt;

	// Token: 0x040024CD RID: 9421
	public float MaxTilt;

	// Token: 0x040024CE RID: 9422
	public bool CollisionDetection;

	// Token: 0x040024CF RID: 9423
	public float CameraRadius = 0.5f;

	// Token: 0x040024D0 RID: 9424
	public bool AutoSetIgnoreLayers;

	// Token: 0x040024D1 RID: 9425
	public LayerMask PushCameraCollisionIgnoreLayer;

	// Token: 0x040024D2 RID: 9426
	public LayerMask HighlightingIgnoreLayer;

	// Token: 0x040024D3 RID: 9427
	public LayerMask DisapperIgnoreLayer;

	// Token: 0x040024D4 RID: 9428
	public bool FollowBehind;

	// Token: 0x040024D5 RID: 9429
	public float FollowRotationOffset;

	// Token: 0x040024D6 RID: 9430
	private bool ho;

	// Token: 0x040024D7 RID: 9431
	private bool Zoom;

	// Token: 0x040024D8 RID: 9432
	private bool CanZoom;

	// Token: 0x040024D9 RID: 9433
	private Transform DisapperOb;

	// Token: 0x040024DA RID: 9434
	public Component[] DisapperObChild;

	// Token: 0x040024DB RID: 9435
	public Component DisapperSpec;

	// Token: 0x040024DC RID: 9436
	private Shader DiffuseShader;

	// Token: 0x040024DD RID: 9437
	private Shader transparentwithshadow;

	// Token: 0x040024DE RID: 9438
	private Vector3 tempPosition;

	// Token: 0x040024DF RID: 9439
	private HighlightableObject highligh;

	// Token: 0x040024E0 RID: 9440
	private float _currDistance;

	// Token: 0x040024E1 RID: 9441
	private float _currRotation;

	// Token: 0x040024E2 RID: 9442
	private float _currTilt;

	// Token: 0x040024E3 RID: 9443
	private bool DoHightLight;

	// Token: 0x040024E4 RID: 9444
	private Color OriColor;

	// Token: 0x040024E5 RID: 9445
	private bool GoAlpha;

	// Token: 0x040024E6 RID: 9446
	private bool GoHide;

	// Token: 0x040024E7 RID: 9447
	private ScreenOverlay SO;

	// Token: 0x040024E8 RID: 9448
	private float fFadeInTime;

	// Token: 0x040024E9 RID: 9449
	private bool bFadeIn;

	// Token: 0x040024EA RID: 9450
	private float fFadeInPos;

	// Token: 0x040024EB RID: 9451
	private RaycastHit[] _prevHits;
}
