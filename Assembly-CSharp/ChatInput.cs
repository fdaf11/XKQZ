using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	// Token: 0x060019DE RID: 6622 RVA: 0x000CFEC4 File Offset: 0x000CE0C4
	private void Start()
	{
		this.mInput = base.GetComponent<UIInput>();
		this.mInput.label.maxLineCount = 1;
		if (this.fillWithDummyData && this.textList != null)
		{
			for (int i = 0; i < 30; i++)
			{
				this.textList.Add(string.Concat(new object[]
				{
					(i % 2 != 0) ? "[AAAAAA]" : "[FFFFFF]",
					"This is an example paragraph for the text list, testing line ",
					i,
					"[-]"
				}));
			}
		}
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x000CFF68 File Offset: 0x000CE168
	public void OnSubmit()
	{
		if (this.textList != null)
		{
			string text = NGUIText.StripSymbols(this.mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				this.textList.Add(text);
				this.mInput.value = string.Empty;
				this.mInput.isSelected = false;
			}
		}
	}

	// Token: 0x04001EA4 RID: 7844
	public UITextList textList;

	// Token: 0x04001EA5 RID: 7845
	public bool fillWithDummyData;

	// Token: 0x04001EA6 RID: 7846
	private UIInput mInput;
}
