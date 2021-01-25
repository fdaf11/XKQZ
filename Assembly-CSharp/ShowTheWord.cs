using System;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class ShowTheWord : MonoBehaviour
{
	// Token: 0x0600153C RID: 5436 RVA: 0x0000D7C9 File Offset: 0x0000B9C9
	private void Start()
	{
		this.m_strWord = Game.StringTable.GetString(this.m_iWordID);
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000B61F8 File Offset: 0x000B43F8
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Equals("cFormWordImage"))
			{
				array[i].GetComponent<UIWordImage>().ShowTheWord(this.m_strWord);
				break;
			}
		}
	}

	// Token: 0x040019F3 RID: 6643
	public int m_iWordID;

	// Token: 0x040019F4 RID: 6644
	private string m_strWord;
}
