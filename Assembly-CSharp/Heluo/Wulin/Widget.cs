using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200037E RID: 894
	public class Widget : MonoBehaviour
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		public virtual void InitWidget(UILayer layer)
		{
			this.ParentLayer = layer;
			this.Obj = base.gameObject;
			this.Traversal(base.transform);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0000D5ED File Offset: 0x0000B7ED
		private void Traversal(Transform t)
		{
			this.AssignControl(t);
			t.ForEach(delegate(Transform x)
			{
				this.Traversal(x);
			});
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void AssignControl(Transform sender)
		{
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0000AF4B File Offset: 0x0000914B
		protected virtual void CheckMouseWheel(int nowAmount, int maxAmount, Control scrollSlider, Control scrollView, bool bReset = false)
		{
			if (nowAmount > maxAmount)
			{
				scrollSlider.GameObject.SetActive(true);
			}
			else
			{
				scrollSlider.GameObject.SetActive(false);
			}
			if (bReset)
			{
				scrollView.GetComponent<UIScrollView>().ResetPosition();
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000913D8 File Offset: 0x0008F5D8
		protected virtual void SetMouseWheel(Control ScrollSlider)
		{
			if (Input.GetAxis("Mouse ScrollWheel") == 0f)
			{
				return;
			}
			float sliderValue = ScrollSlider.sliderValue;
			ScrollSlider.sliderValue = sliderValue - Input.GetAxis("Mouse ScrollWheel") / 4f;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000B42AC File Offset: 0x000B24AC
		protected virtual void SetScrollBar(int iSelectIdx, int iShowAmount, int iMaxAmount, bool bReverse, Control ScrollBar)
		{
			float num = ScrollBar.sliderValue * (float)(iMaxAmount - iShowAmount) + 1f;
			int num2 = Mathf.Clamp((int)Mathf.Round(num), 1, iMaxAmount);
			int num3 = num2 + iShowAmount - 1;
			iSelectIdx++;
			Debug.Log("iSelectIdx : " + iSelectIdx);
			Debug.Log(string.Concat(new object[]
			{
				"sliderValue : ",
				ScrollBar.sliderValue,
				" f : ",
				Mathf.Round(num),
				" start : ",
				num2,
				" end : ",
				num3
			}));
			if (iSelectIdx < num2 || iSelectIdx > num3)
			{
				ScrollBar.GameObject.SetActive(true);
				float sliderValue;
				if (bReverse)
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount - 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				else
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount + 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				ScrollBar.sliderValue = sliderValue;
				Debug.Log("sliderValue : " + ScrollBar.sliderValue);
			}
		}

		// Token: 0x0400196B RID: 6507
		public GameObject Obj;

		// Token: 0x0400196C RID: 6508
		protected UILayer ParentLayer;
	}
}
