using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000132 RID: 306
	public class GameCursor
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00044B3C File Offset: 0x00042D3C
		public GameCursor()
		{
			for (int i = 0; i < 10; i++)
			{
				Texture2D texture2D;
				if (i == 9)
				{
					texture2D = this.generateCursorNoneTexture();
				}
				else
				{
					texture2D = (Resources.Load("MouseImage/" + this.m_CursorName[i]) as Texture2D);
				}
				if (!this.m_CursorList.Contains(texture2D))
				{
					this.m_CursorList.Add(texture2D);
				}
			}
			this.m_MousefxNormal = (Resources.Load("MouseImage/MouseEffect/Efx_Click_Green") as GameObject);
			this.m_MousefxAttack = (Resources.Load("MouseImage/MouseEffect/Efx_Click_Red") as GameObject);
			this.generateMousefxTestDebug();
			this.generateMousefxArrow();
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00005971 File Offset: 0x00003B71
		public static GameCursor Instance
		{
			get
			{
				if (GameCursor.m_Instance == null)
				{
					GameCursor.m_Instance = new GameCursor();
				}
				return GameCursor.m_Instance;
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00044C54 File Offset: 0x00042E54
		private Texture2D generateCursorNoneTexture()
		{
			Texture2D texture2D = new Texture2D(32, 32, 5, false);
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					if (i == 0 && j == 0)
					{
						texture2D.SetPixel(0, 0, new Color(1f, 0f, 0f, 0.1f));
					}
					else
					{
						texture2D.SetPixel(i, j, new Color(0f, 1f, 0f, 0f));
					}
				}
			}
			return texture2D;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00044CE8 File Offset: 0x00042EE8
		private void generateMousefxTestDebug()
		{
			GameObject gameObject = Resources.Load("MouseImage/MouseEffect/Cube") as GameObject;
			this.m_MousefxTestDebug = (Object.Instantiate(gameObject, new Vector3(0f, 500f, 0f), Quaternion.identity) as GameObject);
			this.m_MousefxTestDebug.SetActive(false);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00044D3C File Offset: 0x00042F3C
		private void generateMousefxArrow()
		{
			GameObject gameObject = Resources.Load("MouseImage/MouseEffect/Efx_Click_Arrow") as GameObject;
			this.m_MousefxArrow = (Object.Instantiate(gameObject, new Vector3(0f, 500f, 0f), Quaternion.identity) as GameObject);
			this.m_MousefxArrow.SetActive(false);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00044D90 File Offset: 0x00042F90
		public GameObject SetCursorEffect(GameCursor.eCursorEffect ese, Vector3 point, Quaternion rotation, Transform parent = null)
		{
			GameObject result = null;
			switch (ese)
			{
			case GameCursor.eCursorEffect.Normal:
				result = (Object.Instantiate(this.m_MousefxNormal, point, Quaternion.identity) as GameObject);
				break;
			case GameCursor.eCursorEffect.Attack:
				result = (Object.Instantiate(this.m_MousefxAttack, point, Quaternion.identity) as GameObject);
				break;
			case GameCursor.eCursorEffect.Arrow:
				if (this.m_MousefxArrow == null)
				{
					this.generateMousefxArrow();
				}
				if (parent == null)
				{
					this.m_MousefxArrow.transform.parent = null;
					this.m_MousefxArrow.transform.position = point;
					this.m_MousefxArrow.SetActive(false);
				}
				else
				{
					this.m_MousefxArrow.transform.parent = parent;
					this.m_MousefxArrow.transform.localPosition = Vector3.zero;
					this.m_MousefxArrow.SetActive(true);
				}
				break;
			case GameCursor.eCursorEffect.TestDebug:
				if (this.m_MousefxTestDebug == null)
				{
					this.generateMousefxTestDebug();
				}
				this.m_MousefxTestDebug.transform.localPosition = point;
				this.m_MousefxTestDebug.SetActive(true);
				break;
			}
			return result;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000598C File Offset: 0x00003B8C
		public void ShowCursor()
		{
			GameCursor.IsShow = true;
			this.SetMouseCursor(GameCursor.eCursorState.Normal);
			Debug.Log("ShowCursor");
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000059A5 File Offset: 0x00003BA5
		public void HideCursor()
		{
			GameCursor.IsShow = false;
			this.SetMouseCursor(GameCursor.eCursorState.None);
			Debug.Log("HideCursor");
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00044EC0 File Offset: 0x000430C0
		public void SetMouseCursor(GameCursor.eCursorState state)
		{
			int num;
			if (!GameCursor.IsShow)
			{
				num = 9;
			}
			else
			{
				num = (int)state;
			}
			if (num < 0 || num >= 10)
			{
				return;
			}
			Cursor.SetCursor(this.m_CursorList[num], this.m_HotSpot, this.m_CursorMode);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00044F10 File Offset: 0x00043110
		public bool CheckCursorOnUI()
		{
			if (UICamera.currentCamera)
			{
				Ray ray = UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
				int num = 1 << LayerMask.NameToLayer("UI");
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, 50f, num) && raycastHit.collider != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040006CD RID: 1741
		private static GameCursor m_Instance;

		// Token: 0x040006CE RID: 1742
		public static bool IsShow = true;

		// Token: 0x040006CF RID: 1743
		private List<Texture2D> m_CursorList = new List<Texture2D>();

		// Token: 0x040006D0 RID: 1744
		public GameObject m_MousefxNormal;

		// Token: 0x040006D1 RID: 1745
		public GameObject m_MousefxAttack;

		// Token: 0x040006D2 RID: 1746
		public GameObject m_MousefxArrow;

		// Token: 0x040006D3 RID: 1747
		public GameObject m_MousefxTestDebug;

		// Token: 0x040006D4 RID: 1748
		private CursorMode m_CursorMode;

		// Token: 0x040006D5 RID: 1749
		private Vector2 m_HotSpot = Vector2.zero;

		// Token: 0x040006D6 RID: 1750
		public UIFont m_NameFont;

		// Token: 0x040006D7 RID: 1751
		public UIFont m_TextFont;

		// Token: 0x040006D8 RID: 1752
		private string[] m_CursorName = new string[]
		{
			"Cursor_Normal",
			"Cursor_Attack",
			"Cursor_Npc Talk",
			"Cursor_Pick",
			"Cursor_LockTarget",
			"Cursor_View",
			"Cursor_herb",
			"Cursor_hunt",
			"Cursor_fishing"
		};

		// Token: 0x02000133 RID: 307
		public enum eCursorState
		{
			// Token: 0x040006DA RID: 1754
			Normal,
			// Token: 0x040006DB RID: 1755
			Attack,
			// Token: 0x040006DC RID: 1756
			Npc,
			// Token: 0x040006DD RID: 1757
			Pick,
			// Token: 0x040006DE RID: 1758
			LockTarget,
			// Token: 0x040006DF RID: 1759
			Convey,
			// Token: 0x040006E0 RID: 1760
			Mining,
			// Token: 0x040006E1 RID: 1761
			Hunt,
			// Token: 0x040006E2 RID: 1762
			Fish,
			// Token: 0x040006E3 RID: 1763
			None,
			// Token: 0x040006E4 RID: 1764
			Count
		}

		// Token: 0x02000134 RID: 308
		public enum eCursorEffect
		{
			// Token: 0x040006E6 RID: 1766
			Normal,
			// Token: 0x040006E7 RID: 1767
			Attack,
			// Token: 0x040006E8 RID: 1768
			Arrow,
			// Token: 0x040006E9 RID: 1769
			TestDebug
		}
	}
}
