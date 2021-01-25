using System;
using UnityEngine;

// Token: 0x02000645 RID: 1605
[AddComponentMenu("AFS/Touch Bending/Collision")]
public class touchBendingCollision : MonoBehaviour
{
	// Token: 0x06002794 RID: 10132 RVA: 0x0001A195 File Offset: 0x00018395
	private void Awake()
	{
		this.myTransform = base.transform;
		this.myRenderer = base.renderer;
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x0001A1AF File Offset: 0x000183AF
	private void Start()
	{
		this.myRenderer.sharedMaterial = this.simpleBendingMaterial;
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x00139D90 File Offset: 0x00137F90
	private void OnTriggerEnter(Collider other)
	{
		touchBendingPlayerListener component = other.GetComponent<touchBendingPlayerListener>();
		if (component != null && component.enabled)
		{
			if (!this.touched)
			{
				this.Player_ID = other.GetInstanceID();
				Object.Destroy(this.myRenderer.material);
				this.PlayerVars = component;
				this.Player_Direction = this.PlayerVars.Player_Direction;
				this.Player_Speed = this.PlayerVars.Player_Speed;
				this.intialTouchForce = this.Player_Speed;
				this.myRenderer.material = this.touchBendingMaterial;
				this.myRenderer.material.SetVector("_TouchBendingPosition", new Vector4(0f, 0f, 0f, 0f));
				this.axis = this.myTransform.InverseTransformDirection(this.Player_Direction);
				this.axis = Quaternion.Euler(0f, 90f, 0f) * this.axis;
				this.timer = 0f;
				this.touched = true;
				this.left = false;
				this.targetTouchBending = 1f;
				this.touchBending = this.targetTouchBending;
				this.finished = false;
			}
			else
			{
				if (this.doubletouched)
				{
					this.SwapTouchBending();
				}
				this.Player1_ID = other.GetInstanceID();
				this.PlayerVars1 = component;
				this.Player_Direction1 = this.PlayerVars1.Player_Direction;
				this.Player_Speed1 = this.PlayerVars1.Player_Speed;
				this.intialTouchForce1 = this.Player_Speed1;
				this.axis1 = this.myTransform.InverseTransformDirection(this.Player_Direction1);
				this.axis1 = Quaternion.Euler(0f, 90f, 0f) * this.axis1;
				this.timer1 = 0f;
				this.left1 = false;
				this.targetTouchBending1 = 1f;
				this.touchBending1 = this.targetTouchBending1;
				this.finished1 = false;
				this.lerptime = this.duration - this.timer;
				this.doubletouched = true;
			}
		}
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x00139FA8 File Offset: 0x001381A8
	private void OnTriggerExit(Collider other)
	{
		if (this.Player_ID != this.Player1_ID)
		{
			if (other.GetInstanceID() == this.Player_ID)
			{
				this.left = true;
				this.targetTouchBending = 0f;
			}
			else
			{
				this.left1 = true;
				this.targetTouchBending1 = 0f;
			}
		}
		else
		{
			this.left = true;
			this.targetTouchBending = 0f;
			this.left1 = true;
			this.targetTouchBending1 = 0f;
		}
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x0013A02C File Offset: 0x0013822C
	private void Update()
	{
		if (this.touched)
		{
			this.Player_Speed = this.PlayerVars.Player_Speed;
			this.touchBending = Mathf.Lerp(this.touchBending, this.targetTouchBending, this.timer / this.duration);
			this.easingControl = this.Bounce(this.timer);
			if (!this.doubletouched)
			{
				if (this.finished && this.targetTouchBending == 0f)
				{
					this.ResetTouchBending();
				}
				else
				{
					Quaternion quaternion = Quaternion.Euler(this.axis * (this.intialTouchForce * this.stiffness) * this.easingControl);
					this.myMatrix.SetTRS(Vector3.zero, quaternion, new Vector3(1f, 1f, 1f));
					this.myRenderer.material.SetMatrix("_RotMatrix", this.myMatrix);
					this.myRenderer.material.SetVector("_TouchBendingForce", new Vector4(this.Player_Direction.x, this.Player_Direction.y, this.Player_Direction.z, this.Player_Speed * this.easingControl * this.disturbance));
					if (this.left)
					{
						this.timer += Time.deltaTime;
					}
					else
					{
						this.timer += Time.deltaTime * this.Player_Speed;
					}
				}
			}
			else if (this.finished && this.targetTouchBending == 0f)
			{
				this.SwapTouchBending();
				this.doubletouched = false;
				this.Player_Speed = this.PlayerVars.Player_Speed;
				this.touchBending = Mathf.Lerp(this.touchBending, this.targetTouchBending, this.timer / this.duration);
				this.easingControl = this.Bounce(this.timer);
				if (this.finished && this.targetTouchBending == 0f)
				{
					this.ResetTouchBending();
				}
				else
				{
					Quaternion quaternion2 = Quaternion.Euler(this.axis * (this.intialTouchForce * this.stiffness) * this.easingControl);
					this.myMatrix.SetTRS(Vector3.zero, quaternion2, new Vector3(1f, 1f, 1f));
					this.myRenderer.material.SetMatrix("_RotMatrix", this.myMatrix);
					this.myRenderer.material.SetVector("_TouchBendingForce", new Vector4(this.Player_Direction.x, this.Player_Direction.y, this.Player_Direction.z, this.Player_Speed * this.easingControl * this.disturbance));
					if (this.left)
					{
						this.timer += Time.deltaTime;
					}
					else
					{
						this.timer += Time.deltaTime * this.Player_Speed;
					}
				}
			}
			else
			{
				this.Player_Speed1 = this.PlayerVars1.Player_Speed;
				this.touchBending1 = Mathf.Lerp(this.touchBending1, this.targetTouchBending1, this.timer1 / this.duration);
				this.easingControl1 = this.Bounce1(this.timer1);
				if (this.finished1 && this.targetTouchBending1 == 0f)
				{
					this.doubletouched = false;
				}
				else
				{
					Quaternion quaternion3 = Quaternion.Euler(this.axis * (this.intialTouchForce * this.stiffness) * this.easingControl);
					Quaternion quaternion4 = Quaternion.Euler(this.axis1 * (this.intialTouchForce1 * this.stiffness) * this.easingControl1);
					quaternion3 *= quaternion4;
					this.myMatrix.SetTRS(Vector3.zero, quaternion3, new Vector3(1f, 1f, 1f));
					this.myRenderer.material.SetMatrix("_RotMatrix", this.myMatrix);
					this.myRenderer.material.SetVector("_TouchBendingForce", Vector4.Lerp(new Vector4(this.Player_Direction.x, this.Player_Direction.y, this.Player_Direction.z, this.Player_Speed * this.easingControl * this.disturbance), new Vector4(this.Player_Direction1.x, this.Player_Direction1.y, this.Player_Direction1.z, this.Player_Speed1 * this.easingControl1 * this.disturbance), this.timer1 / (this.lerptime + 0.0001f) * 8f));
					if (this.left)
					{
						this.timer += Time.deltaTime;
					}
					else
					{
						this.timer += Time.deltaTime * this.Player_Speed;
					}
					if (this.left1)
					{
						this.timer1 += Time.deltaTime;
					}
					else
					{
						this.timer1 += Time.deltaTime * this.Player_Speed1;
					}
				}
			}
		}
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x0013A568 File Offset: 0x00138768
	public float Bounce(float x)
	{
		if (x / this.duration >= 1f)
		{
			if (this.easingControl == 0f && this.left)
			{
				this.finished = true;
			}
			return this.targetTouchBending;
		}
		return Mathf.Lerp(Mathf.Sin(x * 10f / this.duration) / (x + 1.25f) * 8f, this.touchBending, Mathf.Sqrt(x / this.duration));
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x0013A5EC File Offset: 0x001387EC
	public float Bounce1(float x)
	{
		if (x / this.duration >= 1f)
		{
			if (this.easingControl1 == 0f && this.left1)
			{
				this.finished1 = true;
			}
			return this.targetTouchBending1;
		}
		return Mathf.Lerp(Mathf.Sin(x * 10f / this.duration) / (x + 1.25f) * 8f, this.touchBending1, Mathf.Sqrt(x / this.duration));
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x0013A670 File Offset: 0x00138870
	public void SwapTouchBending()
	{
		this.Player_ID = this.Player1_ID;
		this.PlayerVars = this.PlayerVars1;
		this.Player_Direction = this.Player_Direction1;
		this.Player_Speed = this.Player_Speed1;
		this.intialTouchForce = this.intialTouchForce1;
		this.touchBending = this.touchBending1;
		this.targetTouchBending = this.targetTouchBending1;
		this.easingControl = this.easingControl1;
		this.left = this.left1;
		this.finished = this.finished1;
		this.axis = this.axis1;
		this.timer = this.timer1;
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x0001A1C2 File Offset: 0x000183C2
	public void ResetTouchBending()
	{
		Object.DestroyImmediate(this.myRenderer.material);
		this.myRenderer.sharedMaterial = this.simpleBendingMaterial;
		this.touched = false;
		this.doubletouched = false;
	}

	// Token: 0x04003161 RID: 12641
	public Material simpleBendingMaterial;

	// Token: 0x04003162 RID: 12642
	public Material touchBendingMaterial;

	// Token: 0x04003163 RID: 12643
	public float stiffness = 10f;

	// Token: 0x04003164 RID: 12644
	public float disturbance = 0.3f;

	// Token: 0x04003165 RID: 12645
	public float duration = 5f;

	// Token: 0x04003166 RID: 12646
	private Transform myTransform;

	// Token: 0x04003167 RID: 12647
	private Renderer myRenderer;

	// Token: 0x04003168 RID: 12648
	private Matrix4x4 myMatrix;

	// Token: 0x04003169 RID: 12649
	private Vector3 axis;

	// Token: 0x0400316A RID: 12650
	private Vector3 axis1;

	// Token: 0x0400316B RID: 12651
	private bool touched;

	// Token: 0x0400316C RID: 12652
	private bool doubletouched;

	// Token: 0x0400316D RID: 12653
	private bool left;

	// Token: 0x0400316E RID: 12654
	private bool finished = true;

	// Token: 0x0400316F RID: 12655
	private bool left1;

	// Token: 0x04003170 RID: 12656
	private bool finished1 = true;

	// Token: 0x04003171 RID: 12657
	private float intialTouchForce;

	// Token: 0x04003172 RID: 12658
	private float touchBending;

	// Token: 0x04003173 RID: 12659
	private float targetTouchBending;

	// Token: 0x04003174 RID: 12660
	private float easingControl;

	// Token: 0x04003175 RID: 12661
	private float intialTouchForce1;

	// Token: 0x04003176 RID: 12662
	private float touchBending1;

	// Token: 0x04003177 RID: 12663
	private float targetTouchBending1;

	// Token: 0x04003178 RID: 12664
	private float easingControl1;

	// Token: 0x04003179 RID: 12665
	private int Player_ID;

	// Token: 0x0400317A RID: 12666
	private touchBendingPlayerListener PlayerVars;

	// Token: 0x0400317B RID: 12667
	private Vector3 Player_Direction;

	// Token: 0x0400317C RID: 12668
	private float Player_Speed;

	// Token: 0x0400317D RID: 12669
	private int Player1_ID;

	// Token: 0x0400317E RID: 12670
	private touchBendingPlayerListener PlayerVars1;

	// Token: 0x0400317F RID: 12671
	private Vector3 Player_Direction1;

	// Token: 0x04003180 RID: 12672
	private float Player_Speed1;

	// Token: 0x04003181 RID: 12673
	private float timer;

	// Token: 0x04003182 RID: 12674
	private float timer1;

	// Token: 0x04003183 RID: 12675
	private float lerptime;
}
