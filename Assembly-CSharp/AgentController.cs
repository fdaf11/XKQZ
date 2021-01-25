using System;
using System.Collections;
using NavMeshExtension;
using UnityEngine;

// Token: 0x02000513 RID: 1299
[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
	// Token: 0x0600216A RID: 8554 RVA: 0x000FBBE0 File Offset: 0x000F9DE0
	private void Start()
	{
		if (!AgentController.pointerObj)
		{
			AgentController.pointerObj = (GameObject)Object.Instantiate(this.pointer, base.transform.position, Quaternion.identity);
		}
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000FBC30 File Offset: 0x000F9E30
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit))
			{
				AgentController.pointerObj.transform.position = raycastHit.point;
				this.path = PortalManager.GetPath(base.transform.position, AgentController.pointerObj.transform.position);
				base.StopAllCoroutines();
				base.StartCoroutine(this.GoToDestination());
			}
		}
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000FBCB4 File Offset: 0x000F9EB4
	private IEnumerator GoToDestination()
	{
		for (int i = 0; i < this.path.Length; i++)
		{
			this.agent.Warp(this.path[i]);
			i++;
			this.agent.SetDestination(this.path[i]);
			while (this.agent.pathPending)
			{
				yield return null;
			}
			float remain = this.agent.remainingDistance;
			while (remain == float.PositiveInfinity || remain - this.agent.stoppingDistance > 1E-45f || this.agent.pathStatus != null)
			{
				remain = this.agent.remainingDistance;
				yield return null;
			}
		}
		this.agent.Stop(true);
		yield break;
	}

	// Token: 0x04002496 RID: 9366
	public GameObject pointer;

	// Token: 0x04002497 RID: 9367
	private static GameObject pointerObj;

	// Token: 0x04002498 RID: 9368
	private NavMeshAgent agent;

	// Token: 0x04002499 RID: 9369
	private Vector3[] path;
}
