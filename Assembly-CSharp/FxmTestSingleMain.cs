using System;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class FxmTestSingleMain : MonoBehaviour
{
	// Token: 0x0600191C RID: 6428 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnEnable()
	{
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0001053E File Offset: 0x0000E73E
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		base.Invoke("CreateEffect", 1f);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000CBCB8 File Offset: 0x000C9EB8
	private void CreateEffect()
	{
		if (this.m_EffectPrefabs[this.m_nIndex] == null)
		{
			return;
		}
		if (this.m_EffectGUIText != null)
		{
			this.m_EffectGUIText.text = this.m_EffectPrefabs[this.m_nIndex].name;
		}
		float num = 0f;
		if (1 < this.m_nCreateCount)
		{
			num = this.m_fRandomRange;
		}
		for (int i = 0; i < this.GetInstanceRoot().transform.childCount; i++)
		{
			Object.Destroy(this.GetInstanceRoot().transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < this.m_nCreateCount; j++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(this.m_EffectPrefabs[this.m_nIndex], new Vector3(Random.Range(-num, num), 0f, Random.Range(-num, num)), Quaternion.identity);
			gameObject.transform.localScale = gameObject.transform.localScale * this.m_fCreateScale;
			NsEffectManager.PreloadResource(gameObject);
			gameObject.transform.parent = this.GetInstanceRoot().transform;
			FxmTestSingleMain.SetActiveRecursively(gameObject, true);
		}
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000CBDF4 File Offset: 0x000C9FF4
	private void OnGUI()
	{
		float num = (float)(Screen.height / 10);
		float num2 = GUI.VerticalSlider(new Rect(10f, num + 10f + 30f, 25f, (float)Screen.height - (num + 10f + 50f)), this.GetFXMakerMouse().m_fDistance, this.GetFXMakerMouse().m_fDistanceMin, this.GetFXMakerMouse().m_fDistanceMax);
		if (num2 != this.GetFXMakerMouse().m_fDistance)
		{
			this.GetFXMakerMouse().SetDistance(num2);
		}
		if (GUI.Button(FxmTestSingleMain.GetButtonRect(0), "Next"))
		{
			if (this.m_nIndex < this.m_EffectPrefabs.Length - 1)
			{
				this.m_nIndex++;
			}
			else
			{
				this.m_nIndex = 0;
			}
			this.CreateEffect();
		}
		if (GUI.Button(FxmTestSingleMain.GetButtonRect(1), "Recreate"))
		{
			this.CreateEffect();
		}
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x00010556 File Offset: 0x0000E756
	public FxmTestSingleMouse GetFXMakerMouse()
	{
		if (this.m_FXMakerMouse == null)
		{
			this.m_FXMakerMouse = base.GetComponentInChildren<FxmTestSingleMouse>();
		}
		return this.m_FXMakerMouse;
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x000103C4 File Offset: 0x0000E5C4
	public GameObject GetInstanceRoot()
	{
		return NcEffectBehaviour.GetRootInstanceEffect();
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x000CBEE4 File Offset: 0x000CA0E4
	public static Rect GetButtonRect()
	{
		int num = 2;
		return new Rect((float)(Screen.width - Screen.width / 10 * num), (float)(Screen.height - Screen.height / 10), (float)(Screen.width / 10 * num), (float)(Screen.height / 10));
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x0001057B File Offset: 0x0000E77B
	public static Rect GetButtonRect(int nIndex)
	{
		return new Rect((float)(Screen.width - Screen.width / 10 * (nIndex + 1)), (float)(Screen.height - Screen.height / 10), (float)(Screen.width / 10), (float)(Screen.height / 10));
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000CBF2C File Offset: 0x000CA12C
	public static void SetActiveRecursively(GameObject target, bool bActive)
	{
		int num = target.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < target.transform.childCount)
			{
				FxmTestSingleMain.SetActiveRecursively(target.transform.GetChild(num).gameObject, bActive);
			}
			num--;
		}
		target.SetActive(bActive);
	}

	// Token: 0x04001D87 RID: 7559
	public GameObject[] m_EffectPrefabs = new GameObject[1];

	// Token: 0x04001D88 RID: 7560
	public GUIText m_EffectGUIText;

	// Token: 0x04001D89 RID: 7561
	public int m_nIndex;

	// Token: 0x04001D8A RID: 7562
	public float m_fCreateScale = 1f;

	// Token: 0x04001D8B RID: 7563
	public int m_nCreateCount = 1;

	// Token: 0x04001D8C RID: 7564
	public float m_fRandomRange = 1f;

	// Token: 0x04001D8D RID: 7565
	public FxmTestSingleMouse m_FXMakerMouse;
}
