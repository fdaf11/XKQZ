using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200043D RID: 1085
[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x00011067 File Offset: 0x0000F267
	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x0001106F File Offset: 0x0000F26F
	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x000D0AE8 File Offset: 0x000CECE8
	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.Length;
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x00011097 File Offset: 0x0000F297
	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000D0B80 File Offset: 0x000CED80
	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		while (this.mCurrentOffset < this.mFullText.Length && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
			{
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset >= this.mFullText.Length)
			{
				break;
			}
			float num2 = 1f / (float)this.charsPerSecond;
			char c = (num >= this.mFullText.Length) ? '\n' : this.mFullText.get_Chars(num);
			if (c == '\n')
			{
				num2 += this.delayOnNewLine;
			}
			else if (num + 1 == this.mFullText.Length || this.mFullText.get_Chars(num + 1) <= ' ')
			{
				if (c == '.')
				{
					if (num + 2 < this.mFullText.Length && this.mFullText.get_Chars(num + 1) == '.' && this.mFullText.get_Chars(num + 2) == '.')
					{
						num2 += this.delayOnPeriod * 3f;
						num += 2;
					}
					else
					{
						num2 += this.delayOnPeriod;
					}
				}
				else if (c == '!' || c == '?')
				{
					num2 += this.delayOnPeriod;
				}
			}
			if (this.mNextChar == 0f)
			{
				this.mNextChar = RealTime.time + num2;
			}
			else
			{
				this.mNextChar += num2;
			}
			if (this.fadeInTime != 0f)
			{
				TypewriterEffect.FadeEntry item = default(TypewriterEffect.FadeEntry);
				item.index = num;
				item.alpha = 0f;
				item.text = this.mFullText.Substring(num, this.mCurrentOffset - num);
				this.mFade.Add(item);
			}
			else
			{
				this.mLabel.text = ((!this.keepFullDimensions) ? this.mFullText.Substring(0, this.mCurrentOffset) : (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset)));
				if (!this.keepFullDimensions && this.scrollView != null)
				{
					this.scrollView.UpdatePosition();
				}
			}
		}
		if (this.mFade.size != 0)
		{
			int i = 0;
			while (i < this.mFade.size)
			{
				TypewriterEffect.FadeEntry value = this.mFade[i];
				value.alpha += RealTime.deltaTime / this.fadeInTime;
				if (value.alpha < 1f)
				{
					this.mFade[i] = value;
					i++;
				}
				else
				{
					this.mFade.RemoveAt(i);
				}
			}
			if (this.mFade.size == 0)
			{
				if (this.keepFullDimensions)
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
				}
				else
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
				}
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.mFade.size; j++)
				{
					TypewriterEffect.FadeEntry fadeEntry = this.mFade[j];
					if (j == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, fadeEntry.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(fadeEntry.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
		}
		else if (this.mCurrentOffset == this.mFullText.Length)
		{
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
		}
	}

	// Token: 0x04001EDA RID: 7898
	public static TypewriterEffect current;

	// Token: 0x04001EDB RID: 7899
	public int charsPerSecond = 20;

	// Token: 0x04001EDC RID: 7900
	public float fadeInTime;

	// Token: 0x04001EDD RID: 7901
	public float delayOnPeriod;

	// Token: 0x04001EDE RID: 7902
	public float delayOnNewLine;

	// Token: 0x04001EDF RID: 7903
	public UIScrollView scrollView;

	// Token: 0x04001EE0 RID: 7904
	public bool keepFullDimensions;

	// Token: 0x04001EE1 RID: 7905
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001EE2 RID: 7906
	private UILabel mLabel;

	// Token: 0x04001EE3 RID: 7907
	private string mFullText = string.Empty;

	// Token: 0x04001EE4 RID: 7908
	private int mCurrentOffset;

	// Token: 0x04001EE5 RID: 7909
	private float mNextChar;

	// Token: 0x04001EE6 RID: 7910
	private bool mReset = true;

	// Token: 0x04001EE7 RID: 7911
	private bool mActive;

	// Token: 0x04001EE8 RID: 7912
	private BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

	// Token: 0x0200043E RID: 1086
	private struct FadeEntry
	{
		// Token: 0x04001EE9 RID: 7913
		public int index;

		// Token: 0x04001EEA RID: 7914
		public string text;

		// Token: 0x04001EEB RID: 7915
		public float alpha;
	}
}
