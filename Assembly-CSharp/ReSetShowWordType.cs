using System;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class ReSetShowWordType : MonoBehaviour
{
	// Token: 0x0600152E RID: 5422 RVA: 0x000B5E74 File Offset: 0x000B4074
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
				array[i].GetComponent<UIWordImage>().SetType(1);
				break;
			}
		}
	}
}
