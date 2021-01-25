using System;

// Token: 0x020002C5 RID: 709
public class Rumor
{
	// Token: 0x06000E0B RID: 3595 RVA: 0x00009965 File Offset: 0x00007B65
	public Rumor(string ImageId, string Name, string Tip, string Line)
	{
		this.m_strImageId = ImageId;
		this.m_strName = Name;
		this.m_strTip = Tip;
		this.m_strLine = Line;
	}

	// Token: 0x04001057 RID: 4183
	public string m_strImageId;

	// Token: 0x04001058 RID: 4184
	public string m_strName;

	// Token: 0x04001059 RID: 4185
	public string m_strTip;

	// Token: 0x0400105A RID: 4186
	public string m_strLine;
}
