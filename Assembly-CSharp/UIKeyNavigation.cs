using System;
using UnityEngine;

// Token: 0x0200046A RID: 1130
[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
	// Token: 0x06001B1C RID: 6940 RVA: 0x000D55B8 File Offset: 0x000D37B8
	protected virtual void OnEnable()
	{
		UIKeyNavigation.list.Add(this);
		if (this.startsSelected && (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.gameObject;
		}
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x00011FFE File Offset: 0x000101FE
	protected virtual void OnDisable()
	{
		UIKeyNavigation.list.Remove(this);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x0001200C File Offset: 0x0001020C
	protected GameObject GetLeft()
	{
		if (NGUITools.GetActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.left, true);
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x0001204B File Offset: 0x0001024B
	private GameObject GetRight()
	{
		if (NGUITools.GetActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.right, true);
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0001208A File Offset: 0x0001028A
	protected GameObject GetUp()
	{
		if (NGUITools.GetActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.up, false);
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000120C9 File Offset: 0x000102C9
	protected GameObject GetDown()
	{
		if (NGUITools.GetActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.down, false);
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000D560C File Offset: 0x000D380C
	protected GameObject Get(Vector3 myDir, bool horizontal)
	{
		Transform transform = base.transform;
		myDir = transform.TransformDirection(myDir);
		Vector3 center = UIKeyNavigation.GetCenter(base.gameObject);
		float num = float.MaxValue;
		GameObject result = null;
		for (int i = 0; i < UIKeyNavigation.list.size; i++)
		{
			UIKeyNavigation uikeyNavigation = UIKeyNavigation.list[i];
			if (!(uikeyNavigation == this))
			{
				UIButton component = uikeyNavigation.GetComponent<UIButton>();
				if (!(component != null) || component.isEnabled)
				{
					Vector3 vector = UIKeyNavigation.GetCenter(uikeyNavigation.gameObject) - center;
					float num2 = Vector3.Dot(myDir, vector.normalized);
					if (num2 >= 0.707f)
					{
						vector = transform.InverseTransformDirection(vector);
						if (horizontal)
						{
							vector.y *= 2f;
						}
						else
						{
							vector.x *= 2f;
						}
						float sqrMagnitude = vector.sqrMagnitude;
						if (sqrMagnitude <= num)
						{
							result = uikeyNavigation.gameObject;
							num = sqrMagnitude;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x000D5734 File Offset: 0x000D3934
	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			Vector3[] worldCorners = component.worldCorners;
			return (worldCorners[0] + worldCorners[2]) * 0.5f;
		}
		return go.transform.position;
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x000D5790 File Offset: 0x000D3990
	protected virtual void OnKey(KeyCode key)
	{
		if (!NGUITools.GetActive(this))
		{
			return;
		}
		GameObject gameObject = null;
		switch (key)
		{
		case 273:
			gameObject = this.GetUp();
			break;
		case 274:
			gameObject = this.GetDown();
			break;
		case 275:
			gameObject = this.GetRight();
			break;
		case 276:
			gameObject = this.GetLeft();
			break;
		default:
			if (key == 9)
			{
				if (Input.GetKey(304) || Input.GetKey(303))
				{
					gameObject = this.GetLeft();
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetRight();
					}
				}
				else
				{
					gameObject = this.GetRight();
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetLeft();
					}
				}
			}
			break;
		}
		if (gameObject != null)
		{
			UICamera.selectedObject = gameObject;
		}
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x00012108 File Offset: 0x00010308
	protected virtual void OnClick()
	{
		if (NGUITools.GetActive(this) && NGUITools.GetActive(this.onClick))
		{
			UICamera.selectedObject = this.onClick;
		}
	}

	// Token: 0x04001FF8 RID: 8184
	public static BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();

	// Token: 0x04001FF9 RID: 8185
	public UIKeyNavigation.Constraint constraint;

	// Token: 0x04001FFA RID: 8186
	public GameObject onUp;

	// Token: 0x04001FFB RID: 8187
	public GameObject onDown;

	// Token: 0x04001FFC RID: 8188
	public GameObject onLeft;

	// Token: 0x04001FFD RID: 8189
	public GameObject onRight;

	// Token: 0x04001FFE RID: 8190
	public GameObject onClick;

	// Token: 0x04001FFF RID: 8191
	public bool startsSelected;

	// Token: 0x0200046B RID: 1131
	public enum Constraint
	{
		// Token: 0x04002001 RID: 8193
		None,
		// Token: 0x04002002 RID: 8194
		Vertical,
		// Token: 0x04002003 RID: 8195
		Horizontal,
		// Token: 0x04002004 RID: 8196
		Explicit
	}
}
