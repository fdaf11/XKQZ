using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class PathMover : MonoBehaviour
{
	// Token: 0x0600098F RID: 2447 RVA: 0x00051754 File Offset: 0x0004F954
	private void Start()
	{
		this.Path = iTweenPath.GetPath("startpath");
		this.ReversePath = iTweenPath.GetPath("startpath");
		Array.Reverse(this.ReversePath);
		this.CurPath = this.Path;
		this.m_bReversePath = false;
		this.m_UIStringOverlay = Game.UI.Root.transform.GetComponent<UIStringOverlay>();
		base.Invoke("StartMove", this.delay);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00007CA7 File Offset: 0x00005EA7
	private void StartMove()
	{
		this.m_bStart = true;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x000517CC File Offset: 0x0004F9CC
	private void CheckMsg()
	{
		if (this.m_MsgList != null)
		{
			for (int i = 0; i < this.m_MsgList.Length; i++)
			{
				PathMoverMsg pathMoverMsg = this.m_MsgList[i];
				if (pathMoverMsg != null)
				{
					if (!pathMoverMsg.m_bShow && this.pathPercent > pathMoverMsg.Percent && pathMoverMsg.m_bReverse == this.m_bReversePath)
					{
						pathMoverMsg.m_bShow = true;
						if (this.m_UIStringOverlay != null)
						{
							string @string = Game.StringTable.GetString(pathMoverMsg.StringID);
							this.m_UIStringOverlay.AddOneLineString(base.gameObject, @string, pathMoverMsg.ShowTime);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00051880 File Offset: 0x0004FA80
	private void ResetMsg()
	{
		for (int i = 0; i < this.m_MsgList.Length; i++)
		{
			PathMoverMsg pathMoverMsg = this.m_MsgList[i];
			if (pathMoverMsg != null)
			{
				pathMoverMsg.m_bShow = false;
			}
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x000518C4 File Offset: 0x0004FAC4
	private void Update()
	{
		if (!this.m_bStart)
		{
			return;
		}
		this.pathPercent += Time.deltaTime * this.speed;
		this.CheckMsg();
		if (this.pathPercent >= 1f)
		{
			this.pathPercent -= 1f;
			this.m_bReversePath = !this.m_bReversePath;
			if (this.m_bReversePath)
			{
				this.CurPath = this.ReversePath;
			}
			else
			{
				this.CurPath = this.Path;
			}
			this.ResetMsg();
		}
		base.transform.position = iTween.PointOnPath(this.CurPath, this.pathPercent);
		Vector3 vector = iTween.PointOnPath(this.CurPath, Mathf.Clamp(this.pathPercent + 0.02f, 0f, 1f));
		base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
		this.setPlayerOnTerrain();
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x000519D8 File Offset: 0x0004FBD8
	private void setPlayerOnTerrain()
	{
		float num = 10f;
		Vector3 vector;
		vector..ctor(base.transform.position.x, base.transform.position.y + num, base.transform.position.z);
		int num2 = 2048;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, Vector3.down, ref raycastHit, num + 1f, num2))
		{
			base.transform.position = raycastHit.point;
		}
	}

	// Token: 0x04000980 RID: 2432
	public float speed = 1f;

	// Token: 0x04000981 RID: 2433
	public float delay;

	// Token: 0x04000982 RID: 2434
	public PathMoverMsg[] m_MsgList;

	// Token: 0x04000983 RID: 2435
	private Vector3[] Path;

	// Token: 0x04000984 RID: 2436
	private Vector3[] ReversePath;

	// Token: 0x04000985 RID: 2437
	private Vector3[] CurPath;

	// Token: 0x04000986 RID: 2438
	private float pathPercent;

	// Token: 0x04000987 RID: 2439
	private bool m_bReversePath;

	// Token: 0x04000988 RID: 2440
	private UIStringOverlay m_UIStringOverlay;

	// Token: 0x04000989 RID: 2441
	private bool m_bStart;
}
