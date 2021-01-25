using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000344 RID: 836
	public class WgBackpackItem : Widget
	{
		// Token: 0x06001314 RID: 4884 RVA: 0x000A5D04 File Offset: 0x000A3F04
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			UICharacter uicharacter = layer as UICharacter;
			this.m_CtrlCharacter = uicharacter.m_CtrlCharacter;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000A5D2C File Offset: 0x000A3F2C
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgBackpackItem.<>f__switch$map33 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("BackpackItem", 0);
					dictionary.Add("Icon", 1);
					dictionary.Add("Name", 2);
					dictionary.Add("Amount", 3);
					dictionary.Add("Text", 4);
					dictionary.Add("OnHover", 5);
					WgBackpackItem.<>f__switch$map33 = dictionary;
				}
				int num;
				if (WgBackpackItem.<>f__switch$map33.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						control.OnClick += this.BackpackItemOnClick;
						control.OnHover += this.BackpackItemOnHover;
						control.OnKeySelect += this.BackpackItemOnKeySelect;
						this.m_BackpackItem = control;
						this.ParentLayer.SetInputButton(5, control.Listener);
						this.ParentLayer.SetInputButton(6, control.Listener);
						break;
					case 1:
						this.m_Icon = control;
						break;
					case 2:
						this.m_Name = control;
						break;
					case 3:
						this.m_Amount = control;
						break;
					case 4:
						this.m_Text = control;
						break;
					case 5:
						this.m_OnHover = control;
						break;
					}
				}
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000A5E84 File Offset: 0x000A4084
		public void SetSecretBook(BackpackNewDataNode node)
		{
			if (node == null)
			{
				return;
			}
			this.Obj.SetActive(true);
			this.Obj.GetComponent<BackpackDataNode>().m_BackpackNode = node;
			string strIcon = node._ItemDataNode.m_strIcon;
			this.SetTexture(strIcon, this.m_Icon);
			this.m_Name.Text = node._ItemDataNode.m_strItemName;
			string @string = Game.StringTable.GetString(110304);
			this.m_Amount.Text = string.Format(@string, node.m_iAmount);
			ItemDataNode itemDataNode = node._ItemDataNode;
			string text = string.Empty;
			for (int i = 0; i < itemDataNode.m_UseLimitNodeList.Count; i++)
			{
				string text2 = string.Empty;
				if (i == 0)
				{
					int iItemType = itemDataNode.m_iItemType;
					if (iItemType == 6)
					{
						text2 = Game.StringTable.GetString(110201) + "： ";
					}
				}
				UseLimitNode useLimitNode = itemDataNode.m_UseLimitNodeList[i];
				UseLimitType type = useLimitNode.m_Type;
				int iInde = useLimitNode.m_iInde;
				int iValue = useLimitNode.m_iValue;
				if (type == UseLimitType.MoreNpcProperty)
				{
					text2 += Game.StringTable.GetString(110100 + iInde);
					text2 += iValue.ToString();
					text = text + text2 + " ";
				}
			}
			if (text.Length != 0)
			{
				this.m_Text.Text = text;
			}
			else
			{
				this.m_Text.Text = itemDataNode.m_strTip;
			}
			this.isSecretBook = true;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x000A6020 File Offset: 0x000A4220
		public void SetMedicine(BackpackNewDataNode node)
		{
			if (node == null)
			{
				return;
			}
			this.Obj.SetActive(true);
			this.Obj.GetComponent<BackpackDataNode>().m_BackpackNode = node;
			string strIcon = node._ItemDataNode.m_strIcon;
			this.SetTexture(strIcon, this.m_Icon);
			this.m_Name.Text = node._ItemDataNode.m_strItemName;
			string @string = Game.StringTable.GetString(110304);
			this.m_Amount.Text = string.Format(@string, node.m_iAmount);
			ItemDataNode itemDataNode = node._ItemDataNode;
			string text = string.Empty;
			for (int i = 0; i < itemDataNode.m_ItmeEffectNodeList.Count; i++)
			{
				ItmeEffectNode itmeEffectNode = itemDataNode.m_ItmeEffectNodeList[i];
				int iItemType = itmeEffectNode.m_iItemType;
				int iRecoverType = itmeEffectNode.m_iRecoverType;
				int iValue = itmeEffectNode.m_iValue;
				int num = iItemType;
				if (num != 16)
				{
					if (num != 17)
					{
						if (num == 1)
						{
							string string2 = Game.StringTable.GetString(110100 + iRecoverType);
							string text2;
							if (iValue > 0)
							{
								text2 = "  +";
							}
							else
							{
								text2 = "  ";
							}
							string text3 = text;
							text = string.Concat(new string[]
							{
								text3,
								string2,
								text2,
								iValue.ToString(),
								" "
							});
						}
					}
					else
					{
						string string2 = Game.StringTable.GetString(110102);
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							string2,
							" ",
							iValue.ToString(),
							"% "
						});
					}
				}
				else
				{
					string string2 = Game.StringTable.GetString(110100);
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						string2,
						" ",
						iValue.ToString(),
						"% "
					});
				}
			}
			if (text.Length != 0)
			{
				this.m_Text.Text = text;
			}
			else
			{
				this.m_Text.Text = itemDataNode.m_strTip;
			}
			this.isSecretBook = false;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0000C666 File Offset: 0x0000A866
		private void BackpackItemOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.isSecretBook)
			{
				this.m_CtrlCharacter.UseSecretBook(go);
			}
			else
			{
				this.m_CtrlCharacter.UseMedicine(go);
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x000A6250 File Offset: 0x000A4450
		private void BackpackItemOnKeySelect(GameObject go, bool bSelect)
		{
			this.m_OnHover.GameObject.SetActive(bSelect);
			if (bSelect)
			{
				this.ParentLayer.ShowKeySelect(go, new Vector3(-350f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
				if (this.SetScrollBar != null)
				{
					this.SetScrollBar.Invoke(go);
				}
			}
			else
			{
				this.ParentLayer.HideKeySelect();
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0000C69C File Offset: 0x0000A89C
		private void BackpackItemOnHover(GameObject go, bool bHover)
		{
			this.m_OnHover.GameObject.SetActive(bHover);
			if (bHover && GameCursor.IsShow)
			{
				this.m_CtrlCharacter.SetCurrent(go);
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000A5CAC File Offset: 0x000A3EAC
		private void SetTexture(string name, Control Icon)
		{
			Texture texture = Game.g_Item.Load("2dtexture/gameui/item/" + name) as Texture;
			if (texture != null)
			{
				Icon.Texture = texture;
			}
			else
			{
				Icon.Texture = null;
				GameDebugTool.Log("無此裝備貼圖 : " + name);
			}
		}

		// Token: 0x04001716 RID: 5910
		private CtrlCharacter m_CtrlCharacter;

		// Token: 0x04001717 RID: 5911
		public new Action<GameObject> SetScrollBar;

		// Token: 0x04001718 RID: 5912
		private Control m_BackpackItem;

		// Token: 0x04001719 RID: 5913
		private Control m_Icon;

		// Token: 0x0400171A RID: 5914
		private Control m_Name;

		// Token: 0x0400171B RID: 5915
		private Control m_Amount;

		// Token: 0x0400171C RID: 5916
		private Control m_Text;

		// Token: 0x0400171D RID: 5917
		private Control m_OnHover;

		// Token: 0x0400171E RID: 5918
		private bool isSecretBook;
	}
}
