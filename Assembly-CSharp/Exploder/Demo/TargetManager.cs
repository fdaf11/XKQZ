using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A2 RID: 162
	public class TargetManager : MonoBehaviour
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00004671 File Offset: 0x00002871
		public static TargetManager Instance
		{
			get
			{
				return TargetManager.instance;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00004678 File Offset: 0x00002878
		private void Awake()
		{
			TargetManager.instance = this;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00004680 File Offset: 0x00002880
		private void Start()
		{
			ExploderUtils.SetActive(this.CrosshairGun.gameObject, true);
			ExploderUtils.SetActive(this.CrosshairHand.gameObject, true);
			ExploderUtils.SetActive(this.PanelText.gameObject, true);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0002D0CC File Offset: 0x0002B2CC
		private void Update()
		{
			Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
			Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0f);
			this.CrosshairGun.color = Color.white;
			this.TargetObject = null;
			this.TargetType = TargetType.None;
			this.TargetPosition = Vector3.zero;
			List<RaycastHit> list = new List<RaycastHit>(Physics.RaycastAll(ray, float.PositiveInfinity));
			GameObject gameObject = null;
			if (list.Count > 0)
			{
				list.Sort((RaycastHit a, RaycastHit b) => (this.MouseLookCamera.transform.position - a.point).sqrMagnitude.CompareTo((this.MouseLookCamera.transform.position - b.point).sqrMagnitude));
				gameObject = list[0].collider.gameObject;
				this.TargetPosition = list[0].point;
			}
			if (gameObject != null)
			{
				this.TargetObject = gameObject;
				if (this.IsDestroyableObject(this.TargetObject))
				{
					this.TargetType = TargetType.DestroyableObject;
				}
				else if (this.IsUseObject(this.TargetObject))
				{
					UseObject component = this.TargetObject.GetComponent<UseObject>();
					if (component && (this.MouseLookCamera.transform.position - component.transform.position).sqrMagnitude < component.UseRadius * component.UseRadius)
					{
						this.TargetType = TargetType.UseObject;
					}
				}
				else
				{
					this.TargetType = TargetType.Default;
				}
			}
			switch (this.TargetType)
			{
			case TargetType.DestroyableObject:
				this.CrosshairHand.enabled = false;
				this.CrosshairGun.enabled = true;
				this.CrosshairGun.color = Color.red;
				break;
			case TargetType.UseObject:
				this.CrosshairGun.enabled = false;
				this.CrosshairHand.enabled = true;
				this.PanelText.enabled = true;
				this.PanelText.text = this.TargetObject.GetComponent<UseObject>().HelperText;
				break;
			case TargetType.Default:
			case TargetType.None:
				this.CrosshairHand.enabled = false;
				this.CrosshairGun.enabled = true;
				this.CrosshairGun.color = Color.white;
				this.PanelText.enabled = false;
				break;
			}
			if (Input.GetKeyDown(101) && this.TargetType == TargetType.UseObject)
			{
				UseObject component2 = this.TargetObject.GetComponent<UseObject>();
				if (component2)
				{
					component2.Use();
				}
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0002D354 File Offset: 0x0002B554
		private bool IsDestroyableObject(GameObject obj)
		{
			return obj.CompareTag("Exploder") || (obj.transform.parent && this.IsDestroyableObject(obj.transform.parent.gameObject));
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0002D3A0 File Offset: 0x0002B5A0
		private bool IsUseObject(GameObject obj)
		{
			return obj.CompareTag("UseObject") || (obj.transform.parent && this.IsDestroyableObject(obj.transform.parent.gameObject));
		}

		// Token: 0x040002B7 RID: 695
		private static TargetManager instance;

		// Token: 0x040002B8 RID: 696
		public GameObject TargetObject;

		// Token: 0x040002B9 RID: 697
		public TargetType TargetType;

		// Token: 0x040002BA RID: 698
		public Vector3 TargetPosition;

		// Token: 0x040002BB RID: 699
		public GUITexture CrosshairGun;

		// Token: 0x040002BC RID: 700
		public GUITexture CrosshairHand;

		// Token: 0x040002BD RID: 701
		public ExploderMouseLook MouseLookCamera;

		// Token: 0x040002BE RID: 702
		public GUIText PanelText;
	}
}
