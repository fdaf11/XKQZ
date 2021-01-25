using System;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class Orbiter : MonoBehaviour
{
	// Token: 0x060024B0 RID: 9392 RVA: 0x0001853D File Offset: 0x0001673D
	private void Start()
	{
		this._pos = Vector3.zero;
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x0011E374 File Offset: 0x0011C574
	private void Update()
	{
		bool flag = false;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		TankController tankController = null;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, 1000f))
		{
			tankController = raycastHit.collider.transform.root.GetComponent<TankController>();
			if (tankController == null)
			{
				this._pos = raycastHit.point;
			}
			else
			{
				flag = true;
				this._pos = tankController.transform.position;
			}
		}
		if (!flag)
		{
			this.Trail.Emit = false;
		}
		else
		{
			if (this._tankBeingController != tankController)
			{
				this.Trail.Emit = true;
				base.transform.localScale = Vector3.one * this.TankCollisionOrbitRadius;
				base.transform.Rotate(Vector3.up, this.TankCollisionRotationSpeed * Time.deltaTime);
				base.transform.position = this._pos;
			}
			if (Input.GetMouseButtonDown(0))
			{
				if (this._tankBeingController != null)
				{
					this._tankBeingController.InControl = false;
				}
				tankController.InControl = true;
				this._tankBeingController = tankController;
			}
		}
	}

	// Token: 0x04002C76 RID: 11382
	public float TankCollisionOrbitRadius = 1.5f;

	// Token: 0x04002C77 RID: 11383
	public float TankCollisionRotationSpeed = 1f;

	// Token: 0x04002C78 RID: 11384
	public Trail Trail;

	// Token: 0x04002C79 RID: 11385
	private TankController _tankBeingController;

	// Token: 0x04002C7A RID: 11386
	private Vector3 _pos;
}
