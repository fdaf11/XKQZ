using System;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class HuntArrow : MonoBehaviour
{
	// Token: 0x06001504 RID: 5380 RVA: 0x0000D67A File Offset: 0x0000B87A
	private void Start()
	{
		this.m_CurrentPosition = base.transform.position;
		this.m_bFly = false;
		this.m_bEnd = false;
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000B4FAC File Offset: 0x000B31AC
	private void CheckOutsidet()
	{
		Vector3 localPosition = base.transform.localPosition;
		float num = this.m_goBackground.transform.localPosition.x + (float)(this.m_goBackground.GetComponent<UITexture>().mainTexture.width / 2);
		float num2 = this.m_goBackground.transform.localPosition.x - (float)(this.m_goBackground.GetComponent<UITexture>().mainTexture.width / 2);
		float num3 = this.m_goBackground.transform.localPosition.y + (float)(this.m_goBackground.GetComponent<UITexture>().mainTexture.height / 2);
		if (localPosition.x < num2 || localPosition.x > num || localPosition.y > num3)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x000B5090 File Offset: 0x000B3290
	private void Update()
	{
		if (this.m_bEnd)
		{
			return;
		}
		if (!this.m_bFly)
		{
			Vector3 vector = GameGlobal.m_camSmallGame.ScreenToWorldPoint(Input.mousePosition);
			this.m_MoveDirection = vector - this.m_CurrentPosition;
			this.m_MoveDirection.z = 0f;
			this.m_MoveDirection.Normalize();
			float num = Mathf.Atan2(this.m_MoveDirection.y, this.m_MoveDirection.x) * 57.29578f;
			num -= 90f;
			if (num < -85f)
			{
				if (num > -180f)
				{
					num = -85f;
				}
				else
				{
					num = 85f;
				}
			}
			else if (num > 85f)
			{
				num = 85f;
			}
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, 0f, num), this.m_fTurnSpeed * Time.deltaTime);
		}
		else
		{
			base.transform.Translate(Vector3.up * Time.deltaTime * this.m_fMoveSpeed);
			this.CheckOutsidet();
		}
		if (Input.GetMouseButtonUp(0) && !this.m_bFly)
		{
			this.m_bFly = true;
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Equals("cFormHunt"))
				{
					array[i].GetComponent<UIHunt>().Fire();
					break;
				}
			}
		}
	}

	// Token: 0x04001998 RID: 6552
	public int m_iIndex;

	// Token: 0x04001999 RID: 6553
	public bool m_bFly;

	// Token: 0x0400199A RID: 6554
	private Vector3 m_CurrentPosition;

	// Token: 0x0400199B RID: 6555
	private Vector3 m_MoveDirection;

	// Token: 0x0400199C RID: 6556
	public float m_fTurnSpeed;

	// Token: 0x0400199D RID: 6557
	public float m_fMoveSpeed;

	// Token: 0x0400199E RID: 6558
	public GameObject m_goBackground;

	// Token: 0x0400199F RID: 6559
	public bool m_bEnd;

	// Token: 0x040019A0 RID: 6560
	public int m_iAtk;
}
