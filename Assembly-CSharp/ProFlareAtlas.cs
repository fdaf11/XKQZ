using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class ProFlareAtlas : MonoBehaviour
{
	// Token: 0x060028CA RID: 10442 RVA: 0x00142344 File Offset: 0x00140544
	public void UpdateElementNameList()
	{
		this.elementNameList = new string[this.elementsList.Count];
		for (int i = 0; i < this.elementNameList.Length; i++)
		{
			this.elementNameList[i] = this.elementsList[i].name;
		}
	}

	// Token: 0x04003376 RID: 13174
	public Texture2D texture;

	// Token: 0x04003377 RID: 13175
	public int elementNumber;

	// Token: 0x04003378 RID: 13176
	public bool editElements;

	// Token: 0x04003379 RID: 13177
	[SerializeField]
	public List<ProFlareAtlas.Element> elementsList = new List<ProFlareAtlas.Element>();

	// Token: 0x0400337A RID: 13178
	public string[] elementNameList;

	// Token: 0x0200068D RID: 1677
	[Serializable]
	public class Element
	{
		// Token: 0x0400337B RID: 13179
		public string name = "Flare Element";

		// Token: 0x0400337C RID: 13180
		public Rect UV = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400337D RID: 13181
		public bool Imported;
	}
}
