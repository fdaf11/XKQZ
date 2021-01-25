using System;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000395 RID: 917
	public class SmallGame : PlayerTarget
	{
		// Token: 0x0600153F RID: 5439 RVA: 0x0000D7E9 File Offset: 0x0000B9E9
		private void Awake()
		{
			this.m_TargetType = PlayerTarget.eTargetType.SmallGame;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0000D7F2 File Offset: 0x0000B9F2
		private void Start()
		{
			base.AddPlayerTargetList(this);
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000B626C File Offset: 0x000B446C
		public void GotoSmallGame()
		{
			if (Game.IsLoading())
			{
				return;
			}
			GameGlobal.m_iSpecialGame = this.m_iSpecialGame;
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			string empty = string.Empty;
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				GameGlobal.m_TransferPos = gameObject.transform.localPosition;
				GameGlobal.m_fDir = gameObject.transform.localRotation.y;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Equals("cFormLoad"))
				{
					array[i].GetComponent<UILoad>().LoadStage(this.m_strGameName);
					break;
				}
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040019F5 RID: 6645
		public string m_strGameName;

		// Token: 0x040019F6 RID: 6646
		public float m_fAddTime;

		// Token: 0x040019F7 RID: 6647
		public int m_iType;

		// Token: 0x040019F8 RID: 6648
		public int m_iSpecialGame;
	}
}
