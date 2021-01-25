using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000821 RID: 2081
public class TOD_GameObjectControl : MonoBehaviour
{
	// Token: 0x060032DC RID: 13020 RVA: 0x0018832C File Offset: 0x0018652C
	private void Update()
	{
		float hour = base.gameObject.GetComponent<TOD_Sky>().Cycle.Hour;
		foreach (TOD_GameObjectNode tod_GameObjectNode in this.goNodeList)
		{
			if (tod_GameObjectNode.fStartActiveTime < tod_GameObjectNode.fEndActiveTime)
			{
				if (tod_GameObjectNode.fStartActiveTime < hour && hour < tod_GameObjectNode.fEndActiveTime)
				{
					if (!tod_GameObjectNode.bActive || this.bFirstSet)
					{
						tod_GameObjectNode.bActive = true;
						foreach (GameObject gameObject in tod_GameObjectNode.goList)
						{
							if (!(gameObject == null))
							{
								gameObject.SetActive(true);
							}
						}
					}
				}
				else if (tod_GameObjectNode.bActive || this.bFirstSet)
				{
					tod_GameObjectNode.bActive = false;
					foreach (GameObject gameObject2 in tod_GameObjectNode.goList)
					{
						if (!(gameObject2 == null))
						{
							gameObject2.SetActive(false);
						}
					}
				}
			}
			else if (tod_GameObjectNode.fStartActiveTime > hour && tod_GameObjectNode.fEndActiveTime < hour)
			{
				if (tod_GameObjectNode.bActive || this.bFirstSet)
				{
					tod_GameObjectNode.bActive = false;
					foreach (GameObject gameObject3 in tod_GameObjectNode.goList)
					{
						if (!(gameObject3 == null))
						{
							gameObject3.SetActive(false);
						}
					}
				}
			}
			else if (!tod_GameObjectNode.bActive || this.bFirstSet)
			{
				tod_GameObjectNode.bActive = true;
				foreach (GameObject gameObject4 in tod_GameObjectNode.goList)
				{
					if (!(gameObject4 == null))
					{
						gameObject4.SetActive(true);
					}
				}
			}
		}
		this.bFirstSet = false;
	}

	// Token: 0x04003E8A RID: 16010
	public List<TOD_GameObjectNode> goNodeList = new List<TOD_GameObjectNode>();

	// Token: 0x04003E8B RID: 16011
	private bool bFirstSet = true;
}
