using System;
using System.Collections.Generic;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class TankController : MonoBehaviour
{
	// Token: 0x060024B5 RID: 9397 RVA: 0x0011E4A4 File Offset: 0x0011C6A4
	private void Update()
	{
		this.Animator.SetBool("InControl", this.InControl);
		if (this.InControl)
		{
			this.WeaponController.enabled = true;
			if (Input.GetKey(119))
			{
				this.Animator.SetBool("Forward", true);
				this.Animator.SetBool("Backward", false);
				this._moveSpeed += this.MoveAcceleration * Time.deltaTime * 2f;
				if (this._moveSpeed > this.MoveSpeed)
				{
					this._moveSpeed = this.MoveSpeed;
				}
			}
			else if (Input.GetKey(115))
			{
				this.Animator.SetBool("Backward", true);
				this.Animator.SetBool("Forward", false);
				this._moveSpeed -= this.MoveAcceleration * Time.deltaTime * 2f;
				if (this._moveSpeed < -this.MoveSpeed)
				{
					this._moveSpeed = -this.MoveSpeed;
				}
			}
			else
			{
				this.Animator.SetBool("Backward", false);
				this.Animator.SetBool("Forward", false);
			}
			if (Input.GetKey(100))
			{
				this._rotateSpeed += this.RotateAcceleration * Time.deltaTime * 2f;
				if (this._rotateSpeed > this.RotateSpeed)
				{
					this._rotateSpeed = this.RotateSpeed;
				}
			}
			else if (Input.GetKey(97))
			{
				this._rotateSpeed -= this.RotateAcceleration * Time.deltaTime * 2f;
				if (this._rotateSpeed < -this.RotateSpeed)
				{
					this._rotateSpeed = -this.RotateSpeed;
				}
			}
		}
		else
		{
			this.WeaponController.enabled = false;
		}
		if (Mathf.Abs(this._moveSpeed) > 0f)
		{
			this.TankTrackTrails.ForEach(delegate(Trail trail)
			{
				trail.Emit = true;
			});
		}
		else
		{
			this.TankTrackTrails.ForEach(delegate(Trail trail)
			{
				trail.Emit = false;
			});
		}
		base.transform.position += base.transform.forward * this._moveSpeed * Time.deltaTime;
		base.transform.RotateAround(base.transform.position, base.transform.up, this._rotateSpeed);
		this.TrailMaterial.mainTextureOffset = new Vector2(this.TrailMaterial.mainTextureOffset.x + Mathf.Sign(this._moveSpeed) * Mathf.Lerp(0f, this.TrailMaterialOffsetSpeed, Mathf.Abs(this._moveSpeed / this.MoveSpeed) + Mathf.Abs(this._rotateSpeed / this.RotateSpeed)), this.TrailMaterial.mainTextureOffset.y);
		this._moveSpeed = Mathf.MoveTowards(this._moveSpeed, 0f, this.MoveFriction * Time.deltaTime);
		this._rotateSpeed = Mathf.MoveTowards(this._rotateSpeed, 0f, this.RotateFriction * Time.deltaTime);
	}

	// Token: 0x04002C7D RID: 11389
	public float TrailMaterialOffsetSpeed;

	// Token: 0x04002C7E RID: 11390
	public float MoveSpeed;

	// Token: 0x04002C7F RID: 11391
	public float MoveFriction;

	// Token: 0x04002C80 RID: 11392
	public float MoveAcceleration;

	// Token: 0x04002C81 RID: 11393
	public float RotateSpeed;

	// Token: 0x04002C82 RID: 11394
	public float RotateFriction;

	// Token: 0x04002C83 RID: 11395
	public float RotateAcceleration;

	// Token: 0x04002C84 RID: 11396
	public Material TrailMaterial;

	// Token: 0x04002C85 RID: 11397
	public Animator Animator;

	// Token: 0x04002C86 RID: 11398
	public List<Trail> TankTrackTrails;

	// Token: 0x04002C87 RID: 11399
	public TankWeaponController WeaponController;

	// Token: 0x04002C88 RID: 11400
	private float _moveSpeed;

	// Token: 0x04002C89 RID: 11401
	private float _rotateSpeed;

	// Token: 0x04002C8A RID: 11402
	public bool InControl;
}
