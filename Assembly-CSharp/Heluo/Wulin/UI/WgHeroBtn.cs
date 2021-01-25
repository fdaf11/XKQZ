using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000350 RID: 848
	public class WgHeroBtn : Widget
	{
		// Token: 0x06001361 RID: 4961 RVA: 0x000A7D6C File Offset: 0x000A5F6C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgHeroBtn.<>f__switch$map3F == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("Back", 0);
					dictionary.Add("Box", 1);
					dictionary.Add("Face", 2);
					dictionary.Add("Hurt", 3);
					dictionary.Add("Hover", 4);
					dictionary.Add("Name", 5);
					WgHeroBtn.<>f__switch$map3F = dictionary;
				}
				int num;
				if (WgHeroBtn.<>f__switch$map3F.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Back = sender;
						break;
					case 1:
						this.m_Box = sender;
						break;
					case 2:
						this.m_Face = sender;
						this.m_Face.OnClick += this.OnHeroClick;
						this.m_Face.OnHover += this.OnHeroHover;
						break;
					case 3:
						this.m_Hurt = sender;
						break;
					case 4:
						this.m_Hover = sender;
						break;
					case 5:
						this.m_Name = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
		public void ResetInfo()
		{
			this.SetUnit(this.info);
			this.SetHero(this.hero);
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0000C8FE File Offset: 0x0000AAFE
		public void SetCharaHead(DLCUnitInfo _info, CharacterData _data)
		{
			this.info = _info;
			this.hero = _data;
			this.SetUnit(this.info);
			this.SetHero(this.hero);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000A7EB0 File Offset: 0x000A60B0
		private void SetUnit(DLCUnitInfo info)
		{
			if (info == null)
			{
				return;
			}
			this.info = info;
			string name = "2dtexture/gameui/hexhead/" + info.Data._NpcDataNode.m_strSmallImage;
			if (Game.g_HexHeadBundle.Contains(name))
			{
				this.m_Face.Texture = (Game.g_HexHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				name = "2dtexture/gameui/bighead/B000001";
				if (Game.g_BigHeadBundle.Contains(name))
				{
					this.m_Face.Texture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
				}
				else
				{
					this.m_Face.Texture = null;
				}
			}
			this.m_Name.Text = info.Data._NpcDataNode.m_strNpcName;
			int num = info.Data.iLevel - 1;
			if (num > 0)
			{
				this.m_Box.GameObject.SetActive(true);
				this.m_Box.SpriteName = "FaceLevel" + num;
			}
			else
			{
				this.m_Box.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x000A7FCC File Offset: 0x000A61CC
		private void SetHero(CharacterData data)
		{
			if (data == null)
			{
				return;
			}
			this.hero = data;
			string name = "2dtexture/gameui/hexhead/" + data._NpcDataNode.m_strSmallImage;
			if (Game.g_HexHeadBundle.Contains(name))
			{
				this.m_Face.Texture = (Game.g_HexHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				name = "2dtexture/gameui/bighead/B000001";
				if (Game.g_BigHeadBundle.Contains(name))
				{
					this.m_Face.Texture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
				}
				else
				{
					this.m_Face.Texture = null;
				}
			}
			this.m_Name.Text = data._NpcDataNode.m_strNpcName;
			this.m_Hurt.GameObject.SetActive(data.iHurtTurn > 0);
			this.m_Box.SpriteName = "FaceLevel3";
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0000C926 File Offset: 0x0000AB26
		public void UnSelect()
		{
			this.m_Back.SpriteName = "cdata_024";
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000A80B0 File Offset: 0x000A62B0
		public void OnHeroClick(GameObject go)
		{
			if (this.UnSelectAll != null)
			{
				this.UnSelectAll.Invoke();
			}
			this.m_Back.SpriteName = "cdata_023";
			if (this.OnClick != null)
			{
				this.OnClick.Invoke(this.Obj);
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0000C938 File Offset: 0x0000AB38
		public void OnHeroHover(GameObject go, bool hover)
		{
			this.m_Hover.GameObject.SetActive(hover);
		}

		// Token: 0x0400176F RID: 5999
		private Control m_Back;

		// Token: 0x04001770 RID: 6000
		private Control m_Box;

		// Token: 0x04001771 RID: 6001
		private Control m_Face;

		// Token: 0x04001772 RID: 6002
		private Control m_Hurt;

		// Token: 0x04001773 RID: 6003
		private Control m_Hover;

		// Token: 0x04001774 RID: 6004
		private Control m_Name;

		// Token: 0x04001775 RID: 6005
		private DLCUnitInfo info;

		// Token: 0x04001776 RID: 6006
		private CharacterData hero;

		// Token: 0x04001777 RID: 6007
		public Action<GameObject> OnClick;

		// Token: 0x04001778 RID: 6008
		public Action UnSelectAll;
	}
}
