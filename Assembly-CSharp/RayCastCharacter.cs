using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class RayCastCharacter : MonoBehaviour
{
	// Token: 0x06000614 RID: 1556 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00043B98 File Offset: 0x00041D98
	private void Start()
	{
		this.readyNow = true;
		this.maincamera = GameObject.FindGameObjectWithTag("MainCamera");
		this.MainCharacter = GameObject.FindGameObjectWithTag("Player");
		base.transform.position = Vector3.zero;
		this.Click = false;
		this.ray = default(Ray);
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00043BF4 File Offset: 0x00041DF4
	private void Update()
	{
		if (GameGlobal.m_bMovie)
		{
			return;
		}
		if (this.Target != null)
		{
			base.transform.position = this.Target.transform.position;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int num = 10240;
			if (Physics.Raycast(this.ray, ref this.hit, 30f, num))
			{
				if (this.hit.transform.tag == "Npc")
				{
					this.Target = this.hit.transform.gameObject;
					this.TurnOffOldHitTransform();
					this.hitTransform = this.hit.transform;
					if (this.readyNow)
					{
						this.MoveProjector();
					}
				}
				else if (this.hit.transform.tag == "Enemy")
				{
					this.TurnOffOldHitTransform();
					this.hitTransform = this.hit.transform;
					this.MoveProjector();
				}
				else if (this.hit.transform.tag == "Ground")
				{
					if (GameGlobal.m_bPlayerTalk)
					{
						return;
					}
					this.TurnOffOldHitTransform();
					this.Target = null;
					base.transform.position = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00043D64 File Offset: 0x00041F64
	private void MoveProjector()
	{
		this.readyNow = false;
		if (this.hit.transform.tag == "Enemy")
		{
			if (this.hit.transform.GetComponent<HighlightableObject>() != null)
			{
				this.hit.transform.GetComponent<HighlightableObject>().ConstantOnImmediate(Color.red);
			}
		}
		base.transform.position = this.hit.transform.position;
		this.readyNow = true;
		this.Click = true;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x000058DC File Offset: 0x00003ADC
	private void TurnOffOldHitTransform()
	{
		if (this.hitTransform && this.hitTransform.GetComponent<HighlightableObject>() != null)
		{
			this.hitTransform.GetComponent<HighlightableObject>().Off();
		}
	}

	// Token: 0x0400068E RID: 1678
	private GameObject MainCharacter;

	// Token: 0x0400068F RID: 1679
	private GameObject Target;

	// Token: 0x04000690 RID: 1680
	private bool Click;

	// Token: 0x04000691 RID: 1681
	private RaycastHit hit;

	// Token: 0x04000692 RID: 1682
	private Ray ray;

	// Token: 0x04000693 RID: 1683
	private Transform hitTransform;

	// Token: 0x04000694 RID: 1684
	private int hitTransformRot;

	// Token: 0x04000695 RID: 1685
	private GameObject maincamera;

	// Token: 0x04000696 RID: 1686
	private bool readyNow;
}
