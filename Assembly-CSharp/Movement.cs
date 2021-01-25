using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class Movement : MonoBehaviour
{
	// Token: 0x06000F16 RID: 3862 RVA: 0x0000A2E7 File Offset: 0x000084E7
	private void Start()
	{
		this.Man = base.gameObject.GetComponent<NavMeshAgent>();
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0007E220 File Offset: 0x0007C420
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit))
			{
				Vector3 point = raycastHit.point;
				this.Man.SetDestination(point);
			}
		}
	}

	// Token: 0x040011D2 RID: 4562
	private NavMeshAgent Man;
}
