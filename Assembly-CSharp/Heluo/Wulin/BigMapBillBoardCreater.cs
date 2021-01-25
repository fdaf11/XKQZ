using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200011F RID: 287
	public class BigMapBillBoardCreater : MonoBehaviour
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0000563D File Offset: 0x0000383D
		private void Awake()
		{
			base.StartCoroutine(this.CreateBigMapBillBoard(0.5f));
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000428C4 File Offset: 0x00040AC4
		private IEnumerator CreateBigMapBillBoard(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			foreach (KeyValuePair<string, BigMapNode> item in BigMapNode.BigMapNodeList)
			{
				BigMapNode node = item.Value;
				if (node.m_bOpen)
				{
					GameObject Obj = Object.Instantiate(this.m_BillBoard.gameObject) as GameObject;
					Obj.transform.parent = this.m_Parent;
					Obj.SetActive(true);
					BigMapBillBoard bmbb = Obj.GetComponent<BigMapBillBoard>();
					if (node.MapIDNode != null)
					{
						bmbb.SetBillBoard(node.MapIDNode.m_strMapName, node.MapIDNode.MapIcon);
					}
					bmbb.SetPosition(node.transform.position + Vector3.up * 20f);
					yield return null;
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x0400064E RID: 1614
		public Transform m_Parent;

		// Token: 0x0400064F RID: 1615
		public BigMapBillBoard m_BillBoard;
	}
}
