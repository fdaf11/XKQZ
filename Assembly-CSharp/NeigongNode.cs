using System;
using System.Collections.Generic;

// Token: 0x020000F2 RID: 242
public class NeigongNode
{
	// Token: 0x06000508 RID: 1288 RVA: 0x0003AECC File Offset: 0x000390CC
	public NeigongNode Clone()
	{
		NeigongNode neigongNode = new NeigongNode();
		neigongNode.m_iNeigongID = this.m_iNeigongID;
		neigongNode.m_strName = this.m_strName;
		neigongNode.m_strDesp = this.m_strDesp;
		neigongNode.m_iExp = this.m_iExp;
		neigongNode.m_iNeigongType = this.m_iNeigongType;
		neigongNode.m_strSelectImage = this.m_strSelectImage;
		neigongNode.m_strStatusImage = this.m_strStatusImage;
		foreach (EffectPart effectPart in this.m_effectPartList)
		{
			neigongNode.m_effectPartList.Add(effectPart.Clone());
		}
		return neigongNode;
	}

	// Token: 0x04000509 RID: 1289
	public int m_iNeigongID;

	// Token: 0x0400050A RID: 1290
	public string m_strName;

	// Token: 0x0400050B RID: 1291
	public string m_strDesp;

	// Token: 0x0400050C RID: 1292
	public int m_iExp;

	// Token: 0x0400050D RID: 1293
	public int m_iNeigongType;

	// Token: 0x0400050E RID: 1294
	public string m_strSelectImage;

	// Token: 0x0400050F RID: 1295
	public string m_strStatusImage;

	// Token: 0x04000510 RID: 1296
	public List<EffectPart> m_effectPartList = new List<EffectPart>();
}
