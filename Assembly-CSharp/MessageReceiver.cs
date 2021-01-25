using System;
using System.Collections;
using SWS;
using UnityEngine;

// Token: 0x0200069F RID: 1695
public class MessageReceiver : MonoBehaviour
{
	// Token: 0x06002921 RID: 10529 RVA: 0x0000264F File Offset: 0x0000084F
	private void MyMethod()
	{
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x00006875 File Offset: 0x00004A75
	private void PrintText(string text)
	{
		Debug.Log(text);
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x00146774 File Offset: 0x00144974
	private void RotateSprite(float newRot)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.y = newRot;
		base.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x001467A4 File Offset: 0x001449A4
	private IEnumerator SetDestination(Object target)
	{
		NavMeshAgent agent = base.GetComponent<NavMeshAgent>();
		navMove myMove = base.GetComponent<navMove>();
		GameObject tar = (GameObject)target;
		myMove.Pause(false);
		myMove.ChangeSpeed(4f);
		agent.SetDestination(tar.transform.position);
		while (agent.pathPending)
		{
			yield return null;
		}
		float remain = agent.remainingDistance;
		while (remain == float.PositiveInfinity || remain - agent.stoppingDistance > 1E-45f || agent.pathStatus != null)
		{
			remain = agent.remainingDistance;
			yield return null;
		}
		yield return new WaitForSeconds(4f);
		myMove.ChangeSpeed(1.5f);
		myMove.Resume();
		yield break;
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x001467D0 File Offset: 0x001449D0
	private IEnumerator ActivateForTime(Object target)
	{
		GameObject tar = (GameObject)target;
		tar.SetActive(true);
		yield return new WaitForSeconds(6f);
		tar.SetActive(false);
		yield break;
	}
}
