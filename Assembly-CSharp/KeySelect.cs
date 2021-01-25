using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DE RID: 734
public class KeySelect : MonoBehaviour
{
	// Token: 0x06000F0A RID: 3850 RVA: 0x0000A2A0 File Offset: 0x000084A0
	public void SetSize(int width, int height)
	{
		this.m_Arrow.width = width;
		this.m_Arrow.height = height;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0000A2BA File Offset: 0x000084BA
	public void Hide()
	{
		this.m_Arrow.enabled = false;
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0000A2C8 File Offset: 0x000084C8
	public void Show()
	{
		this.m_Arrow.enabled = true;
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0007DE28 File Offset: 0x0007C028
	private IEnumerator ShowDalay(float waitTime)
	{
		this.m_Arrow.enabled = false;
		yield return new WaitForSeconds(waitTime);
		this.m_Arrow.enabled = true;
		yield break;
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0007DE54 File Offset: 0x0007C054
	public void SetArrowDir(KeySelect.eSelectDir dir)
	{
		TweenPosition tweenPosition = this.m_Arrow.gameObject.GetComponent<TweenPosition>();
		if (tweenPosition == null)
		{
			tweenPosition = this.m_Arrow.gameObject.AddComponent<TweenPosition>();
		}
		tweenPosition.from = Vector3.zero;
		switch (dir)
		{
		case KeySelect.eSelectDir.Top:
			tweenPosition.to = Vector3.up * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Vertically;
			break;
		case KeySelect.eSelectDir.LeftTop:
			tweenPosition.to = Vector3.up * 5f + Vector3.left * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Both;
			break;
		case KeySelect.eSelectDir.Left:
			tweenPosition.to = Vector3.left * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Both;
			break;
		case KeySelect.eSelectDir.LeftBottom:
			tweenPosition.to = Vector3.down * 5f + Vector3.left * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Horizontally;
			break;
		case KeySelect.eSelectDir.Bottom:
			tweenPosition.to = Vector3.down * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Horizontally;
			break;
		case KeySelect.eSelectDir.RightBottom:
			tweenPosition.to = Vector3.down * 5f + Vector3.right * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Nothing;
			break;
		case KeySelect.eSelectDir.Right:
			tweenPosition.to = Vector3.right * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Nothing;
			break;
		case KeySelect.eSelectDir.RightTop:
			tweenPosition.to = Vector3.up * 5f + Vector3.right * 5f;
			this.m_Arrow.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.m_Arrow.flip = UIBasicSprite.Flip.Vertically;
			break;
		}
		tweenPosition.ResetToBeginning();
		tweenPosition.ignoreTimeScale = true;
		tweenPosition.enabled = true;
	}

	// Token: 0x040011C3 RID: 4547
	public UITexture m_Arrow;

	// Token: 0x020002DF RID: 735
	public enum eSelectDir
	{
		// Token: 0x040011C5 RID: 4549
		Top,
		// Token: 0x040011C6 RID: 4550
		LeftTop,
		// Token: 0x040011C7 RID: 4551
		Left,
		// Token: 0x040011C8 RID: 4552
		LeftBottom,
		// Token: 0x040011C9 RID: 4553
		Bottom,
		// Token: 0x040011CA RID: 4554
		RightBottom,
		// Token: 0x040011CB RID: 4555
		Right,
		// Token: 0x040011CC RID: 4556
		RightTop
	}
}
