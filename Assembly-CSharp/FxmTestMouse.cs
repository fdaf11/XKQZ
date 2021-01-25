using System;
using UnityEngine;

// Token: 0x02000406 RID: 1030
public class FxmTestMouse : MonoBehaviour
{
	// Token: 0x060018FC RID: 6396 RVA: 0x000103CB File Offset: 0x0000E5CB
	public void ChangeAngle(float angle)
	{
		this.m_fYRot = angle;
		this.m_fXRot = 0f;
		this.m_MovePostion = Vector3.zero;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x000103EA File Offset: 0x0000E5EA
	public void SetHandControl(bool bEnable)
	{
		this.m_bHandEnable = bEnable;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x000103F3 File Offset: 0x0000E5F3
	public void SetDistance(float fDistance)
	{
		this.m_fDistance = fDistance;
		PlayerPrefs.SetFloat("FxmTestMouse.m_fDistance", this.m_fDistance);
		this.UpdateCamera(true);
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x00010413 File Offset: 0x0000E613
	private void OnEnable()
	{
		this.m_fDistance = PlayerPrefs.GetFloat("FxmTestMouse.m_fDistance", this.m_fDistance);
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0001042B File Offset: 0x0000E62B
	private void Start()
	{
		if (Camera.main == null)
		{
			return;
		}
		if (base.rigidbody)
		{
			base.rigidbody.freezeRotation = true;
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x000CAEE8 File Offset: 0x000C90E8
	private bool IsGUIMousePosition()
	{
		Vector2 guimousePosition = NgLayout.GetGUIMousePosition();
		if (FxmTestMain.inst.GetFXMakerControls().GetActionToolbarRect().Contains(guimousePosition))
		{
			return true;
		}
		Rect rect;
		rect..ctor(0f, 0f, (float)Screen.width, (float)(Screen.height / 10 + 30));
		if (rect.Contains(guimousePosition))
		{
			return true;
		}
		Rect rect2;
		rect2..ctor(0f, 0f, 40f, (float)Screen.height);
		return rect2.Contains(guimousePosition);
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0001045A File Offset: 0x0000E65A
	private void Update()
	{
		if (this.IsGUIMousePosition() && !this.m_bLeftClick && !this.m_bRightClick)
		{
			return;
		}
		this.UpdateCamera(false);
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x000CAF78 File Offset: 0x000C9178
	public void UpdateCamera(bool bOnlyZoom)
	{
		if (Camera.main == null)
		{
			return;
		}
		if (this.m_fWheelSpeed < 0f)
		{
			this.m_fWheelSpeed = 5f;
		}
		float num = this.m_fDistance / 8f;
		float fDistance = this.m_fDistance;
		if (this.m_TargetTrans)
		{
			this.m_fDistance = Mathf.Clamp(this.m_fDistance - Input.GetAxis("Mouse ScrollWheel") * this.m_fWheelSpeed * num, this.m_fDistanceMin, this.m_fDistanceMax);
			if (Camera.main.orthographic)
			{
				Camera.main.orthographicSize = this.m_fDistance * 0.6f;
				if (this.m_GrayscaleCamara != null)
				{
					this.m_GrayscaleCamara.orthographicSize = this.m_fDistance * 0.6f;
				}
			}
			if (!bOnlyZoom && this.m_bRightClick && Input.GetMouseButton(this.m_nRotInputIndex))
			{
				this.m_fXRot += Input.GetAxis("Mouse X") * this.m_fXSpeed * 0.02f;
				this.m_fYRot -= Input.GetAxis("Mouse Y") * this.m_fYSpeed * 0.02f;
			}
			if (!bOnlyZoom && Input.GetMouseButtonDown(this.m_nRotInputIndex))
			{
				this.m_bRightClick = true;
			}
			if (!bOnlyZoom && Input.GetMouseButtonUp(this.m_nRotInputIndex))
			{
				this.m_bRightClick = false;
			}
			this.m_fYRot = FxmTestMouse.ClampAngle(this.m_fYRot, this.m_fYMinLimit, this.m_fYMaxLimit);
			Quaternion quaternion = Quaternion.Euler(this.m_fYRot, this.m_fXRot, 0f);
			RaycastHit raycastHit;
			if (this.m_bRaycastHit && Physics.Linecast(this.m_TargetTrans.position, Camera.main.transform.position, ref raycastHit))
			{
				this.m_fDistance -= raycastHit.distance;
			}
			Vector3 vector;
			vector..ctor(0f, 0f, -this.m_fDistance);
			Vector3 position = quaternion * vector + this.m_TargetTrans.position;
			Camera.main.transform.rotation = quaternion;
			Camera.main.transform.position = position;
			this.UpdatePosition(Camera.main.transform);
			if (this.m_GrayscaleCamara != null)
			{
				this.m_GrayscaleCamara.transform.rotation = Camera.main.transform.rotation;
				this.m_GrayscaleCamara.transform.position = Camera.main.transform.position;
			}
			if (fDistance != this.m_fDistance)
			{
				PlayerPrefs.SetFloat("FxmTestMouse.m_fDistance", this.m_fDistance);
			}
		}
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x00010485 File Offset: 0x0000E685
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x000CB240 File Offset: 0x000C9440
	private void UpdatePosition(Transform camera)
	{
		if (this.m_bHandEnable)
		{
			if (Input.GetMouseButtonDown(this.m_nMoveInputIndex))
			{
				this.m_OldMousePos = Input.mousePosition;
				this.m_bLeftClick = true;
			}
			if (this.m_bLeftClick && Input.GetMouseButton(this.m_nMoveInputIndex))
			{
				Vector3 mousePosition = Input.mousePosition;
				float worldPerScreenPixel = NgLayout.GetWorldPerScreenPixel(this.m_TargetTrans.transform.position);
				this.m_MovePostion += (this.m_OldMousePos - mousePosition) * worldPerScreenPixel;
				this.m_OldMousePos = mousePosition;
			}
			if (Input.GetMouseButtonUp(this.m_nMoveInputIndex))
			{
				this.m_bLeftClick = false;
			}
		}
		camera.Translate(this.m_MovePostion, 1);
	}

	// Token: 0x04001D4F RID: 7503
	protected const float m_fDefaultDistance = 8f;

	// Token: 0x04001D50 RID: 7504
	protected const float m_fDefaultWheelSpeed = 5f;

	// Token: 0x04001D51 RID: 7505
	public Transform m_TargetTrans;

	// Token: 0x04001D52 RID: 7506
	public Camera m_GrayscaleCamara;

	// Token: 0x04001D53 RID: 7507
	public Shader m_GrayscaleShader;

	// Token: 0x04001D54 RID: 7508
	protected bool m_bRaycastHit;

	// Token: 0x04001D55 RID: 7509
	public float m_fDistance = 8f;

	// Token: 0x04001D56 RID: 7510
	public float m_fXSpeed = 350f;

	// Token: 0x04001D57 RID: 7511
	public float m_fYSpeed = 300f;

	// Token: 0x04001D58 RID: 7512
	public float m_fWheelSpeed = 5f;

	// Token: 0x04001D59 RID: 7513
	public float m_fYMinLimit = -90f;

	// Token: 0x04001D5A RID: 7514
	public float m_fYMaxLimit = 90f;

	// Token: 0x04001D5B RID: 7515
	public float m_fDistanceMin = 1f;

	// Token: 0x04001D5C RID: 7516
	public float m_fDistanceMax = 50f;

	// Token: 0x04001D5D RID: 7517
	public int m_nMoveInputIndex = 1;

	// Token: 0x04001D5E RID: 7518
	public int m_nRotInputIndex;

	// Token: 0x04001D5F RID: 7519
	public float m_fXRot;

	// Token: 0x04001D60 RID: 7520
	public float m_fYRot;

	// Token: 0x04001D61 RID: 7521
	protected bool m_bHandEnable = true;

	// Token: 0x04001D62 RID: 7522
	protected Vector3 m_MovePostion;

	// Token: 0x04001D63 RID: 7523
	protected Vector3 m_OldMousePos;

	// Token: 0x04001D64 RID: 7524
	protected bool m_bLeftClick;

	// Token: 0x04001D65 RID: 7525
	protected bool m_bRightClick;
}
