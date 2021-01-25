using System;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class FxmReplayDemo : MonoBehaviour
{
	// Token: 0x0600195D RID: 6493 RVA: 0x000107D8 File Offset: 0x0000E9D8
	private void Start()
	{
		this.CreateEffect();
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x000107E0 File Offset: 0x0000E9E0
	private void CreateEffect()
	{
		if (this.m_TargetPrefab == null)
		{
			return;
		}
		this.m_InstanceObj = NsEffectManager.CreateReplayEffect(this.m_TargetPrefab);
		NsEffectManager.PreloadResource(this.m_InstanceObj);
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x00010811 File Offset: 0x0000EA11
	private void Replay(bool bClearOldParticle)
	{
		NsEffectManager.RunReplayEffect(this.m_InstanceObj, bClearOldParticle);
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x0001081F File Offset: 0x0000EA1F
	private void OnGUI()
	{
		if (GUI.Button(FxmReplayDemo.GetButtonRect(0), "Replay"))
		{
			this.Replay(false);
		}
		if (GUI.Button(FxmReplayDemo.GetButtonRect(1), "Replay(ClearParticle)"))
		{
			this.Replay(true);
		}
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x00010859 File Offset: 0x0000EA59
	public static Rect GetButtonRect(int nIndex)
	{
		return new Rect((float)(Screen.width - Screen.width / 8 * (nIndex + 1)), (float)(Screen.height - Screen.height / 10), (float)(Screen.width / 8), (float)(Screen.height / 10));
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000CD0D8 File Offset: 0x000CB2D8
	public static void SetActiveRecursively(GameObject target, bool bActive)
	{
		int num = target.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < target.transform.childCount)
			{
				FxmReplayDemo.SetActiveRecursively(target.transform.GetChild(num).gameObject, bActive);
			}
			num--;
		}
		target.SetActive(bActive);
	}

	// Token: 0x04001DEC RID: 7660
	public GameObject m_TargetPrefab;

	// Token: 0x04001DED RID: 7661
	public GameObject m_InstanceObj;
}
