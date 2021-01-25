using System;
using System.Text;
using UnityEngine;

// Token: 0x0200050C RID: 1292
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06002136 RID: 8502 RVA: 0x0001647F File Offset: 0x0001467F
	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06002137 RID: 8503 RVA: 0x000164A6 File Offset: 0x000146A6
	// (set) Token: 0x06002138 RID: 8504 RVA: 0x000FA8AC File Offset: 0x000F8AAC
	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
				}
				else
				{
					this.mScroll = value;
					this.UpdateVisibleText();
				}
			}
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06002139 RID: 8505 RVA: 0x000164AE File Offset: 0x000146AE
	protected float lineHeight
	{
		get
		{
			return (!(this.textLabel != null)) ? 20f : ((float)(this.textLabel.fontSize + this.textLabel.spacingY));
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x0600213A RID: 8506 RVA: 0x000FA908 File Offset: 0x000F8B08
	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000164E3 File Offset: 0x000146E3
	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000FA94C File Offset: 0x000F8B4C
	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
		}
		else
		{
			this.textLabel.pivot = UIWidget.Pivot.TopLeft;
			this.scrollValue = 0f;
		}
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000FA9F0 File Offset: 0x000F8BF0
	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.Rebuild();
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000FAA5C File Offset: 0x000F8C5C
	public void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000FAA94 File Offset: 0x000F8C94
	public void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000164F6 File Offset: 0x000146F6
	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x0001650E File Offset: 0x0001470E
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000FAAD0 File Offset: 0x000F8CD0
	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.mParagraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000FAB34 File Offset: 0x000F8D34
	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.mParagraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				NGUIText.WrapText(paragraph.text, out text);
				paragraph.lines = text.Split(new char[]
				{
					'\n'
				});
				this.mTotalLines += paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uiscrollBar = this.scrollBar as UIScrollBar;
				if (uiscrollBar != null)
				{
					uiscrollBar.barSize = ((this.mTotalLines != 0) ? (1f - (float)this.scrollHeight / (float)this.mTotalLines) : 1f);
				}
			}
			this.UpdateVisibleText();
		}
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000FAC78 File Offset: 0x000F8E78
	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = string.Empty;
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.mParagraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string text = paragraph.lines[num5];
					if (num3 > 0)
					{
						num3--;
					}
					else
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(text);
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	// Token: 0x0400246C RID: 9324
	public UILabel textLabel;

	// Token: 0x0400246D RID: 9325
	public UIProgressBar scrollBar;

	// Token: 0x0400246E RID: 9326
	public UITextList.Style style;

	// Token: 0x0400246F RID: 9327
	public int paragraphHistory = 50;

	// Token: 0x04002470 RID: 9328
	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	// Token: 0x04002471 RID: 9329
	protected BetterList<UITextList.Paragraph> mParagraphs = new BetterList<UITextList.Paragraph>();

	// Token: 0x04002472 RID: 9330
	protected float mScroll;

	// Token: 0x04002473 RID: 9331
	protected int mTotalLines;

	// Token: 0x04002474 RID: 9332
	protected int mLastWidth;

	// Token: 0x04002475 RID: 9333
	protected int mLastHeight;

	// Token: 0x0200050D RID: 1293
	public enum Style
	{
		// Token: 0x04002477 RID: 9335
		Text,
		// Token: 0x04002478 RID: 9336
		Chat
	}

	// Token: 0x0200050E RID: 1294
	protected class Paragraph
	{
		// Token: 0x04002479 RID: 9337
		public string text;

		// Token: 0x0400247A RID: 9338
		public string[] lines;
	}
}
