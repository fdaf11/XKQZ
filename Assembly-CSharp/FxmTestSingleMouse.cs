using System;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class FxmTestSingleMouse : MonoBehaviour
{
	// Token: 0x06001928 RID: 6440 RVA: 0x000105B6 File Offset: 0x0000E7B6
	public void ChangeAngle(float angle)
	{
		this.m_fYRot = angle;
		this.m_fXRot = 0f;
		this.m_MovePostion = Vector3.zero;
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x000105D5 File Offset: 0x0000E7D5
	public void SetHandControl(bool bEnable)
	{
		this.m_bHandEnable = bEnable;
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000105DE File Offset: 0x0000E7DE
	public void SetDistance(float fDistance)
	{
		this.m_fDistance = fDistance;
		PlayerPrefs.SetFloat("FxmTestSingleMouse.m_fDistance", this.m_fDistance);
		this.UpdateCamera(true);
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000105FE File Offset: 0x0000E7FE
	private void OnEnable()
	{
		this.m_fDistance = PlayerPrefs.GetFloat("FxmTestSingleMouse.m_fDistance", this.m_fDistance);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x0001042B File Offset: 0x0000E62B
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

	// Token: 0x0600192D RID: 6445 RVA: 0x000CC004 File Offset: 0x000CA204
	private bool IsGUIMousePosition()
	{
		Vector2 guimousePosition = NgLayout.GetGUIMousePosition();
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

	// Token: 0x0600192E RID: 6446 RVA: 0x00010616 File Offset: 0x0000E816
	private void Update()
	{
		if (this.IsGUIMousePosition() && !this.m_bLeftClick && !this.m_bRightClick)
		{
			return;
		}
		this.UpdateCamera(false);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000CC078 File Offset: 0x000CA278
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
			if (!this.IsGUIMousePosition())
			{
				this.m_fDistance = Mathf.Clamp(this.m_fDistance - Input.GetAxis("Mouse ScrollWheel") * this.m_fWheelSpeed * num, this.m_fDistanceMin, this.m_fDistanceMax);
			}
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
			this.m_fYRot = FxmTestSingleMouse.ClampAngle(this.m_fYRot, this.m_fYMinLimit, this.m_fYMaxLimit);
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
				PlayerPrefs.SetFloat("FxmTestSingleMouse.m_fDistance", this.m_fDistance);
			}
		}
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x00010485 File Offset: 0x0000E685
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

	// Token: 0x06001931 RID: 6449 RVA: 0x000CC348 File Offset: 0x000CA548
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
				float worldPerScreenPixel = this.GetWorldPerScreenPixel(this.m_TargetTrans.transform.position);
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

	// Token: 0x06001932 RID: 6450 RVA: 0x000CC408 File Offset: 0x000CA608
	public float GetWorldPerScreenPixel(Vector3 worldPoint)
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return 0f;
		}
		Plane plane;
		plane..ctor(main.transform.forward, main.transform.position);
		float distanceToPoint = plane.GetDistanceToPoint(worldPoint);
		float num = 100f;
		return Vector3.Distance(main.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2) - num / 2f, distanceToPoint)), main.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2) + num / 2f, distanceToPoint))) / num;
	}

	// Token: 0x04001D8E RID: 7566
	protected const float m_fDefaultDistance = 8f;

	// Token: 0x04001D8F RID: 7567
	protected const float m_fDefaultWheelSpeed = 5f;

	// Token: 0x04001D90 RID: 7568
	public Transform m_TargetTrans;

	// Token: 0x04001D91 RID: 7569
	public Camera m_GrayscaleCamara;

	// Token: 0x04001D92 RID: 7570
	public Shader m_GrayscaleShader;

	// Token: 0x04001D93 RID: 7571
	protected bool m_bRaycastHit;

	// Token: 0x04001D94 RID: 7572
	public float m_fDistance = 8f;

	// Token: 0x04001D95 RID: 7573
	public float m_fXSpeed = 350f;

	// Token: 0x04001D96 RID: 7574
	public float m_fYSpeed = 300f;

	// Token: 0x04001D97 RID: 7575
	public float m_fWheelSpeed = 5f;

	// Token: 0x04001D98 RID: 7576
	public float m_fYMinLimit = -90f;

	// Token: 0x04001D99 RID: 7577
	public float m_fYMaxLimit = 90f;

	// Token: 0x04001D9A RID: 7578
	public float m_fDistanceMin = 1f;

	// Token: 0x04001D9B RID: 7579
	public float m_fDistanceMax = 50f;

	// Token: 0x04001D9C RID: 7580
	public int m_nMoveInputIndex = 1;

	// Token: 0x04001D9D RID: 7581
	public int m_nRotInputIndex;

	// Token: 0x04001D9E RID: 7582
	public float m_fXRot;

	// Token: 0x04001D9F RID: 7583
	public float m_fYRot;

	// Token: 0x04001DA0 RID: 7584
	protected bool m_bHandEnable = true;

	// Token: 0x04001DA1 RID: 7585
	protected Vector3 m_MovePostion;

	// Token: 0x04001DA2 RID: 7586
	protected Vector3 m_OldMousePos;

	// Token: 0x04001DA3 RID: 7587
	protected bool m_bLeftClick;

	// Token: 0x04001DA4 RID: 7588
	protected bool m_bRightClick;
}
