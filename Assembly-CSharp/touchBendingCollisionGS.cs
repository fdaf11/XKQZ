using System;
using UnityEngine;

// Token: 0x02000646 RID: 1606
[AddComponentMenu("AFS/Touch Bending/CollisionGS")]
public class touchBendingCollisionGS : MonoBehaviour
{
	// Token: 0x0600279E RID: 10142 RVA: 0x0001A22A File Offset: 0x0001842A
	private void Awake()
	{
		this.myTransform = base.transform;
		this.myRenderer = base.renderer;
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x0001A244 File Offset: 0x00018444
	private void Start()
	{
		this.myRenderer.sharedMaterial = this.simpleBendingMaterial;
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x0013A710 File Offset: 0x00138910
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
				this.myRenderer.material.SetVector("_TouchBendingPosition", new Vector4(this.myTransform.position.x, this.myTransform.position.y, this.myTransform.position.z, 0f));
				this.axis = this.PlayerVars.Player_Direction;
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
				this.axis1 = this.Player_Direction1;
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

	// Token: 0x060027A1 RID: 10145 RVA: 0x0013A940 File Offset: 0x00138B40
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

	// Token: 0x060027A2 RID: 10146 RVA: 0x0013A9C4 File Offset: 0x00138BC4
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

	// Token: 0x060027A3 RID: 10147 RVA: 0x0013AF00 File Offset: 0x00139100
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

	// Token: 0x060027A4 RID: 10148 RVA: 0x0013AF84 File Offset: 0x00139184
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

	// Token: 0x060027A5 RID: 10149 RVA: 0x0013B008 File Offset: 0x00139208
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

	// Token: 0x060027A6 RID: 10150 RVA: 0x0001A257 File Offset: 0x00018457
	public void ResetTouchBending()
	{
		Object.DestroyImmediate(this.myRenderer.material);
		this.myRenderer.sharedMaterial = this.simpleBendingMaterial;
		this.touched = false;
		this.doubletouched = false;
	}

	// Token: 0x04003184 RID: 12676
	public Material simpleBendingMaterial;

	// Token: 0x04003185 RID: 12677
	public Material touchBendingMaterial;

	// Token: 0x04003186 RID: 12678
	public float stiffness = 10f;

	// Token: 0x04003187 RID: 12679
	public float disturbance = 0.3f;

	// Token: 0x04003188 RID: 12680
	public float duration = 5f;

	// Token: 0x04003189 RID: 12681
	private Transform myTransform;

	// Token: 0x0400318A RID: 12682
	private Renderer myRenderer;

	// Token: 0x0400318B RID: 12683
	private Matrix4x4 myMatrix;

	// Token: 0x0400318C RID: 12684
	private Vector3 axis;

	// Token: 0x0400318D RID: 12685
	private Vector3 axis1;

	// Token: 0x0400318E RID: 12686
	private bool touched;

	// Token: 0x0400318F RID: 12687
	private bool doubletouched;

	// Token: 0x04003190 RID: 12688
	private bool left;

	// Token: 0x04003191 RID: 12689
	private bool finished = true;

	// Token: 0x04003192 RID: 12690
	private bool left1;

	// Token: 0x04003193 RID: 12691
	private bool finished1 = true;

	// Token: 0x04003194 RID: 12692
	private float intialTouchForce;

	// Token: 0x04003195 RID: 12693
	private float touchBending;

	// Token: 0x04003196 RID: 12694
	private float targetTouchBending;

	// Token: 0x04003197 RID: 12695
	private float easingControl;

	// Token: 0x04003198 RID: 12696
	private float intialTouchForce1;

	// Token: 0x04003199 RID: 12697
	private float touchBending1;

	// Token: 0x0400319A RID: 12698
	private float targetTouchBending1;

	// Token: 0x0400319B RID: 12699
	private float easingControl1;

	// Token: 0x0400319C RID: 12700
	private int Player_ID;

	// Token: 0x0400319D RID: 12701
	private touchBendingPlayerListener PlayerVars;

	// Token: 0x0400319E RID: 12702
	private Vector3 Player_Direction;

	// Token: 0x0400319F RID: 12703
	private float Player_Speed;

	// Token: 0x040031A0 RID: 12704
	private int Player1_ID;

	// Token: 0x040031A1 RID: 12705
	private touchBendingPlayerListener PlayerVars1;

	// Token: 0x040031A2 RID: 12706
	private Vector3 Player_Direction1;

	// Token: 0x040031A3 RID: 12707
	private float Player_Speed1;

	// Token: 0x040031A4 RID: 12708
	private float timer;

	// Token: 0x040031A5 RID: 12709
	private float timer1;

	// Token: 0x040031A6 RID: 12710
	private float lerptime;
}
